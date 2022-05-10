// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 05-05-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-05-2022
// ***********************************************************************
// <copyright file="UserList.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Notes2022.Client.Dialogs;
using Notes2022.Proto;


namespace Notes2022.Client.Pages.Admin
{
    /// <summary>
    /// Class UserList.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class UserList
    {
        /// <summary>
        /// Gets or sets the modal.
        /// </summary>
        /// <value>The modal.</value>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// Gets or sets the u list.
        /// </summary>
        /// <value>The u list.</value>
        private GAppUserList UList { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserList"/> class.
        /// </summary>
        public UserList()
        { }

        /// <summary>
        /// On parameters set as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            UList = await Client.GetUserListAsync(new NoRequest(), myState.AuthHeader);
        }

        /// <summary>
        /// Edits the link.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        protected void EditLink(string Id)
        {
            ModalParameters Parameters = new ModalParameters();
            Parameters.Add("UserId", Id);

            Modal.Show<UserEdit>("", Parameters);
        }
    }
}
