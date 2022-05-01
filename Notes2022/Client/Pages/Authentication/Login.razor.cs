using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Notes2022.Proto;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Web;

namespace Notes2022.Client.Pages.Authentication
{
    public partial class Login
    {
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public int RememberHours { get; set; }
        }


        public string myCookieValue { get; set; } = "";

        protected InputModel Input = new InputModel { Email = string.Empty, Password = string.Empty };

        protected string Message = string.Empty;

        private IJSObjectReference? module;

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./cookies.js");
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        protected async Task WriteCookies(LoginReply ar)
        {
            try
            {
                string xx = Base64Encode(HttpUtility.HtmlEncode(JsonSerializer.Serialize(ar)));
                await module.InvokeAsync<string>("CreateCookie", Globals.Cookie, xx, Input.RememberHours);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task GotoLogin()
        {

            LoginRequest req = new LoginRequest()
            { Email = Input.Email, Password = Input.Password };
            LoginReply ar = await AuthClient.LoginAsync(req);
            if (ar.Status == 200)
            {
                myState.LoginReply = ar;
                await WriteCookies(ar);
            }
            else
            {
                Message = ar.Message;
                return;
            }

            Globals.LoginDisplay?.Reload();
            Globals.NavMenu?.Reload();

            Navigation.NavigateTo("");
        }
    }
}