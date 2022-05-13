﻿// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 04-28-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-09-2022
// ***********************************************************************
// <copyright file="NoteMenu.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Name: NoteMenu.razor.cs
    **
    ** Description:
    ** Menu bar for Note display
    **
    **  This program is free software: you can redistribute it and / or modify
    **  it under the terms of the GNU General Public License version 3 as
    **  published by the Free Software Foundation.
    **
    **  This program is distributed in the hope that it will be useful,
    **  but WITHOUT ANY WARRANTY; without even the implied warranty of
    **  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    **  GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/

using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Notes2022.Client.Dialogs;
using Notes2022.Client.Pages;
using Notes2022.Proto;
using Syncfusion.Blazor.Navigations;

namespace Notes2022.Client.Menus
{
    /// <summary>
    /// Menu for top of a Note
    /// </summary>
    public partial class NoteMenu
    {
        /// <summary>
        /// For dialogs
        /// </summary>
        /// <value>The modal.</value>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// Model data reference from container
        /// </summary>
        /// <value>The model.</value>
        [Parameter] public DisplayModel Model { get; set; }

        /// <summary>
        /// Reference to our caller/container so we can call back into it.
        /// </summary>
        /// <value>The index of my note.</value>
        [Parameter] public NoteIndex MyNoteIndex { get; set; }

        /// <summary>
        /// Menu structure
        /// </summary>
        /// <value>The menu items.</value>
#pragma warning disable IDE1006 // Naming Styles
        private static List<MenuItem> menuItems { get; set; }

        /// <summary>
        /// Top level menu instance
        /// </summary>
        /// <value>The top menu.</value>
        protected SfMenu<MenuItem> topMenu { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Gets or sets a value indicating whether [hamburger mode].
        /// </summary>
        /// <value><c>true</c> if [hamburger mode]; otherwise, <c>false</c>.</value>
        private bool HamburgerMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the navigation.
        /// </summary>
        /// <value>The navigation.</value>
        [Inject] NavigationManager Navigation { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteMenu"/> class.
        /// </summary>
        public NoteMenu()
        {
        }

        /// <summary>
        /// Construct our menu based on user access token
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override Task OnParametersSetAsync()
        {
            menuItems = new List<MenuItem>();

#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            MenuItem item = new() { Id = "ListNotes", Text = "Listing" };
            menuItems.Add(item);

            item = new() { Id = "NextBase", Text = "Next Base" };
            menuItems.Add(item);

            item = new() { Id = "PreviousBase", Text = "Previous Base" };
            menuItems.Add(item);

            item = new() { Id = "NextNote", Text = "Next" };
            menuItems.Add(item);

            item = new() { Id = "PreviousNote", Text = "Previous" };
            menuItems.Add(item);

            if (Model.Access.ReadAccess)
            {
                MenuItem item2 = new()
                {
                    Id = "OutPutMenu",
                    Text = "Output",
                    Items = new List<MenuItem>
                {
                    new() { Id = "Forward", Text = "Forward" },
                    new() { Id = "Copy", Text = "Copy" },
                    new() { Id = "mail", Text = "mail" },
                    //item2.Items.Add(new MenuItem() { Id = "Mark", Text = "Mark for output" });
                    new() { Id = "Html", Text = "Html (expandable)" },
                    new() { Id = "html", Text = "html (flat)" },
                    new() { Id = "Json", Text = "Json" }
                }
                };
                menuItems.Add(item2);

                if (Model.Access.Respond)
                {
                    item = new() { Id = "NewResponse", Text = "New Response" };
                    menuItems.Add(item);
                }

                if (Model.CanEdit)
                {
                    item = new() { Id = "Edit", Text = "Edit" };
                    menuItems.Add(item);

                    if (Model.Access.UserID == Model.Header.AuthorID || Model.IsAdmin)
                    {
                        item = new() { Id = "Delete", Text = "Delete" };
                        menuItems.Add(item);
                    }
                }

                //menuItems.Add(new() { Id = "SearchFromNote", Text = "Search" });
                menuItems.Add(new() { Id = "NoteHelp", Text = "Z for HELP" });
            }

            return Task.CompletedTask;
        }
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
        /// <summary>
        /// Menu item invoked
        /// </summary>
        /// <param name="e">The e.</param>
        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

        /// <summary>
        /// This can be called not only from above but also by the container
        /// that shares the same model
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async Task ExecMenu(string id)
        {
            long myId;
            switch (id)
            {
                case "ListNotes":
                    MyNoteIndex.Listing();
                    break;

                case "NewResponse":
                    long bnId = Model.Header.Id;           // if base note
                    if (Model.Header.ResponseOrdinal > 0)   // if response
                    {
                        bnId = Model.Header.BaseNoteId;
                    }
                    Navigation.NavigateTo("newnote/" + Model.NoteFile.Id + "/" + bnId + "/" + Model.Header.Id);
                    break;

                case "Edit":
                    if (Model.CanEdit)
                        Navigation.NavigateTo("editnote/" + Model.Header.Id);
                    break;

                case "NextBase":
                    myId = MyNoteIndex.GetNextBaseNote(Model.Header);
                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "PreviousBase":
                    myId = MyNoteIndex.GetPreviousBaseNote(Model.Header);

                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "NextNote":
                    myId = MyNoteIndex.GetNextNote(Model.Header);
                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "PreviousNote":
                    myId = MyNoteIndex.GetPreviousNote(Model.Header);

                    if (myId > 0)
                    {
                        MyNoteIndex.GotoNote(myId);
                    }
                    break;

                case "NoteHelp":
                    Modal.Show<HelpDialog2>();
                    break;

                case "Delete":
                    if (Model.IsAdmin || Model.CanEdit)
                    {
                        if (!await YesNo("Are you sure you want to delete this note?"))
                            return;
                        await Client.DeleteNoteAsync(new NoteId() { Id = Model.Header.Id }, myState.AuthHeader); 
                        Navigation.NavigateTo("notedisplay/" + Model.Header.Id);   //, true);
                    }
                    else
                    {
                        ShowMessage("You may not delete this note.");
                    }

                    break;

                case "Forward":
                    Forward();
                    break;

                case "Copy":
                    var parameters = new ModalParameters();
                    parameters.Add("Note", Model.Header);
                    Modal.Show<Copy>("", parameters);
                    break;

                case "mail":
                    await DoEmail();
                    break;

                case "Html":
                    DoExport(true, true);
                    break;

                case "html":
                    DoExport(true, false);
                    break;

                case "Json":
                    DoJson();
                    break;
            }
        }

        protected void DoJson()
        {
            NoteHeadersRequest request = new()
            {
                NoteFileId = Model.NoteFile.Id,
                ArcId = Model.Header.ArchiveId,
                NoteOrdinal = Model.Header.NoteOrdinal,
                ResponseOrdinal = -1,
                ContentAndTags = true,
                FileName = Model.NoteFile.NoteFileName + ".note-" + Model.Header.NoteOrdinal + ".json"
            };

            if (Model.Header.ResponseOrdinal > 0)
            { 
                request.ResponseOrdinal = Model.Header.ResponseOrdinal;
                request.FileName = Model.NoteFile.NoteFileName + ".note-" + Model.Header.NoteOrdinal + "-response-" + Model.Header.ResponseOrdinal + ".json";
            }
            else if (!MyNoteIndex.MyNotePanel.RespShown)
            {
                request.ResponseOrdinal = 0;
            }


            var parameters = new ModalParameters();
            parameters.Add("model", request);
            Modal.Show<ExportJsonString>("", parameters);

        }

        /// <summary>
        /// Forwards a note
        /// </summary>
        protected void Forward()
        {
            var parameters = new ModalParameters();
            ForwardViewModel fv = new()
            {
                NoteID = Model.Header.Id,
                FileID = Model.Header.NoteFileId,
                ArcID = Model.Header.ArchiveId,
                NoteOrdinal = Model.Header.NoteOrdinal,
                NoteSubject = Model.Header.NoteSubject,
                NoteFile = Model.NoteFile
            };

            if (Model.Header.ResponseCount > 0 || Model.Header.BaseNoteId > 0)
                fv.Hasstring = true;

            parameters.Add("ForwardView", fv);

            Modal.Show<Forward>("", parameters);
        }

        /// <summary>
        /// Exports a note
        /// </summary>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="isCollapsible">if set to <c>true</c> [is collapsible].</param>
        /// <param name="isEmail">if set to <c>true</c> [is email].</param>
        /// <param name="emailaddr">The emailaddr.</param>
        private void DoExport(bool isHtml, bool isCollapsible, bool isEmail = false, string emailaddr = "")
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new()
            {
                ArchiveNumber = Model.Header.ArchiveId,
                isCollapsible = isCollapsible,
                isDirectOutput = !isEmail,
                isHtml = isHtml,
                NoteFile = Model.NoteFile,
                NoteOrdinal = Model.Header.NoteOrdinal,
                Email = emailaddr
            };
            //vm.myMenu = this;

            parameters.Add("Model", vm);
            parameters.Add("FileName", Model.NoteFile.NoteFileName + (isHtml ? ".html" : ".txt"));

            Modal.Show<ExportUtil1>("", parameters);
        }

        /// <summary>
        /// Emails a note
        /// </summary>
        private async Task DoEmail()
        {
            string emailaddr;
            var parameters = new ModalParameters();
            var formModal = Modal.Show<Dialogs.Email>("", parameters);
            var result = await formModal.Result;
            if (result.Cancelled)
                return;
            emailaddr = (string)result.Data;
            if (string.IsNullOrEmpty(emailaddr))
                return;

            DoExport(true, true, true, emailaddr);

        }

        /// <summary>
        /// Confirmation dialog
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private async Task<bool> YesNo(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            var formModal = Modal.Show<YesNo>("", parameters);
            var result = await formModal.Result;
            return !result.Cancelled;
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }
    }
}
