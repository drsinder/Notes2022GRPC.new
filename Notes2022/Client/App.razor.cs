// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 05-08-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-09-2022
// ***********************************************************************
// <copyright file="App.razor.cs" company="Notes2022.Client">
//     Copyright (c) Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.JSInterop;
using Notes2022.Proto;
using Syncfusion.Licensing;
using Grpc.Core;
using System.Text.Json;

namespace Notes2022.Client
{
    /// <summary>
    /// Class App.
    /// Implements the <see cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// Implements the <see cref="System.IAsyncDisposable" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// <seealso cref="System.IAsyncDisposable" />
    public partial class App
    {
        /// <summary>
        /// The saved login value
        /// </summary>
        private LoginReply? savedLoginValue;        // used while updating cookies

        /// <summary>
        /// The module
        /// </summary>
        private IJSObjectReference? module;         // for calling javascript

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        {
            if (module is not null)
            {
                await module.DisposeAsync();
                module = null;
            }
        }

        /// <summary>
        /// On parameters set as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            AString key = await Client.GetTextFileAsync(new AString()
            {Val = "syncfusionkey.rsghjjsrsrj43632353"});
            SyncfusionLicenseProvider.RegisterLicense(key.Val);

            // JS injected in .razor file - make sure the cookie.js is loaded
            if (module is null)
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./cookies.js");

            //try
            //{
            //    await Client.SpinUpAsync(new NoRequest());
            //}
            //catch (Exception ex)
            //{ }

            if (myState.IsAuthenticated)    // nothing more to do here!
                return;

            savedLoginValue = myState.LoginReply;   // should be null

            try
            {
                await GetLoginReplyAsync();   // try to get a cookie to authenticate
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Try to get login cookie
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task GetLoginReplyAsync()
        {
            try
            {
                if (module is null)
                    module = await JS.InvokeAsync<IJSObjectReference>("import", "./cookies.js");

                string? cookie = await ReadCookie(Globals.Cookie);
                if (!string.IsNullOrEmpty(cookie))
                {
                    // found a cookie!
                    savedLoginValue = JsonSerializer.Deserialize<LoginReply>(cookie);

                    savedLogin = savedLoginValue;   // save the value - login

                    if (Globals.NavMenu != null)
                        await Globals.NavMenu.Reload();
                    if (Globals.LoginDisplay != null)
                        Globals.LoginDisplay.Reload();

                    NotifyStateChanged();           // notify subscribers
                }
            }
            catch (Exception )
            {
            }
        }

        /// <summary>
        /// Read a cookie
        /// </summary>
        /// <param name="cookieName">cookie name</param>
        /// <returns>needs to be deserialized)</returns>
        public async Task<string?> ReadCookie(string cookieName)
        {
            if (module is not null)
            {
                try
                {
                    return Globals.Base64Decode(await module.InvokeAsync<string>("ReadCookie", cookieName));
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// Write a Cookie
        /// </summary>
        /// <param name="cookieName">Name of the cookie</param>
        /// <param name="newCookie">Serialized cookie</param>
        /// <param name="hours">expiry</param>
        public async Task WriteCookie(string cookieName, string newCookie, int hours)
        {
            if (module is not null)
            {
                try
                {
                    await module.InvokeAsync<string>("CreateCookie", cookieName, Globals.Base64Encode(newCookie), hours);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Dealing with login related info
        /// /
        /// </summary>

        private LoginReply? savedLogin;

        /// <summary>
        /// Gets or sets the login reply.
        /// </summary>
        /// <value>The login reply.</value>
        public LoginReply? LoginReply
        {
            get
            {
                return savedLogin;
            }
            set
            {
                savedLogin = value;

                // now save login cookie state

                if (savedLogin != null)
                {
                    WriteCookie(Globals.Cookie, JsonSerializer.Serialize(savedLogin), savedLogin.Hours).GetAwaiter();
                }
                else
                {
                    WriteCookie(Globals.Cookie, JsonSerializer.Serialize(new LoginReply()), 0).GetAwaiter();
                }

                NotifyStateChanged();   // notify subscribers
            }
        }

        /// <summary>
        /// Occurs when [on change].
        /// </summary>
        public event System.Action? OnChange;

        /// <summary>
        /// Notifies the state changed.
        /// </summary>
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        /// <summary>
        /// Check if user is authenticated - Login replay is not null and status == 200
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated
        {
            get
            {
                return (LoginReply is not null) && LoginReply.Status == 200;
            }
        }

        /// <summary>
        /// Is user in Admin role
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin
        {
            get
            {
                if (LoginReply is null || LoginReply.Status != 200)
                    return false;
                return UserInfo is not null && UserInfo.IsAdmin;
            }
        }

        /// <summary>
        /// Is user in User role
        /// </summary>
        /// <value><c>true</c> if this instance is user; otherwise, <c>false</c>.</value>
        public bool IsUser
        {
            get
            {
                if (LoginReply is null || LoginReply.Status != 200)
                    return false;
                return UserInfo is not null && UserInfo.IsUser;
            }
        }

        /// <summary>
        /// Get a Metadata/header for authetication to server in gRPC calls
        /// </summary>
        /// <value>The authentication header.</value>
        public Metadata AuthHeader
        {
            get
            {
                var headers = new Metadata();
                if (LoginReply is not null && LoginReply.Status == 200)
                    headers.Add("Authorization", $"Bearer {LoginReply.Jwt}");
                return headers;
            }
        }

        /// <summary>
        /// Get the decoded user info
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo? UserInfo
        {
            get
            {
                if (LoginReply is not null && LoginReply.Status == 200)
                {

                    return LoginReply.Info;
                }

                return null;
            }
        }

    }
}