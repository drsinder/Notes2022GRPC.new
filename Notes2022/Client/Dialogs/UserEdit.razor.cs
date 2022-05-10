// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : sinde
// Created          : 05-05-2022
//
// Last Modified By : sinde
// Last Modified On : 05-05-2022
// ***********************************************************************
// <copyright file="UserEdit.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;

namespace Notes2022.Client.Dialogs
{
    /// <summary>
    /// Class UserEdit.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class UserEdit
    {
        /// <summary>
        /// Gets or sets the modal instance.
        /// </summary>
        /// <value>The modal instance.</value>
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Parameter] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        protected EditUserViewModel Model { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEdit"/> class.
        /// </summary>
        public UserEdit()
        {
        }

        /// <summary>
        /// On parameters set as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            Model = await Client.GetUserRolesAsync(new AppUserRequest() { Subject = UserId }, myState.AuthHeader);
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        private async Task Submit()
        {
            await Client.UpdateUserRolesAsync(Model, myState.AuthHeader);
            await ModalInstance.CancelAsync();
        }


        /// <summary>
        /// Dones this instance.
        /// </summary>
        private async Task Done()
        {
            await ModalInstance.CancelAsync();
        }


    }
}