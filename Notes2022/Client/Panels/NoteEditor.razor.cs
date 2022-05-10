﻿// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : sinde
// Created          : 04-29-2022
//
// Last Modified By : sinde
// Last Modified On : 05-04-2022
// ***********************************************************************
// <copyright file="NoteEditor.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Name: NoteEditor.razor.cs
    **
    ** Description: Edits and creates new notes
    **
    **
    ** This program is free software: you can redistribute it and/or modify
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
using Microsoft.AspNetCore.Components.Web;
using Notes2022.Client.Dialogs;
using Notes2022.Proto;
using Notes2022.Shared;
using Syncfusion.Blazor.RichTextEditor;
using System.Net.Http.Json;

namespace Notes2022.Client.Panels
{
    /// <summary>
    /// Editor for notes
    /// Has a fields for:
    /// Subject, Tags, Director Message, and the Note Body
    /// </summary>
    public partial class NoteEditor
    {
        /// <summary>
        /// For dialogs
        /// </summary>
        /// <value>The modal.</value>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// The data model used
        /// </summary>
        /// <value>The model.</value>
        [Parameter] public TextViewModel Model { get; set; }

        /// <summary>
        /// The show child
        /// </summary>
        private bool ShowChild = false;

        /// <summary>
        /// The Notefile we are using
        /// </summary>
        /// <value>The note file.</value>
        private GNotefile noteFile { get; set; } = new GNotefile();

        /// <summary>
        /// Reference to the Body Editor
        /// </summary>
        /// <value>The edit object.</value>
        private SfRichTextEditor EditObj { get; set; }

        /// <summary>
        /// Reference to the Editor Tool bar
        /// </summary>
        /// <value>The tool bar object.</value>
        private RichTextEditorToolbarSettings ToolBarObj { get; set; }

        /// <summary>
        /// Holding place to prepared code to be inserted
        /// </summary>
        /// <value>The prepared code.</value>
        protected string PreparedCode { get; set; }

        /// <summary>
        /// Define the content of the toolbar
        /// </summary>
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
            new ToolbarItemModel() { Command = ToolbarCommand.FontName },
            new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
            new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
            new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
            new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
            new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.Image },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.Print },
            //new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
            new ToolbarItemModel() { Name = "PCode1", TooltipText = "Prepare Code for Insertion" },
            new ToolbarItemModel() { Name = "PCode", TooltipText = "Insert Prepared Code" },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.FullScreen }
        };

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        /// <summary>
        /// Gets or sets the navigation.
        /// </summary>
        /// <value>The navigation.</value>
        [Inject] NavigationManager Navigation { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteEditor"/> class.
        /// </summary>
        public NoteEditor()
        {
        }

        /// <summary>
        /// Get a NoteFile Object for the file we are using
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected async override Task OnParametersSetAsync()
        {
            if (Model.NoteFileID != 0)
            {
                //noteFile = await DAL.GetNewNote(Http, Model.NoteFileID);
                noteFile = await Client.GetNoteFileAsync(new NoteIndexRequest() { NoteFileId = Model.NoteFileID}, myState.AuthHeader);
            }
        }

        /// <summary>
        /// User has asked to store the note
        /// </summary>
        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Model.MySubject))
            {
                ShowMessage("Please provide a note Subject");
                return;
            }

            GNoteHeader noteHeader;

            if (Model.NoteID == 0)    // new note
            {
                noteHeader = await Client.CreateNewNoteAsync(Model, myState.AuthHeader);
            }
            else // editing
            {
                noteHeader = await Client.UpdateNoteAsync(Model, myState.AuthHeader);
            }
            Navigation.NavigateTo("notedisplay/" + noteHeader.Id);
            return;

        }

        //public async Task OnToolbarClickHandler(ToolbarClickEventArgs args)
        //{
        //    if (args.Item.Id == "InsertCode")
        //    {
        //        string xx = await EditObj.GetSelectedHtmlAsync();
        //        if (xx is not null && xx.Length > 0)
        //        {
        //            ShowMessage("Code can not be edited.  Please Copy, Delete, and Reinsert");
        //            return;
        //        }
        //        // get insertion point?? how??

        //        var parameters = new ModalParameters();
        //        parameters.Add("stuff", xx);
        //        parameters.Add("EditObj", EditObj);
        //        var formModal = Modal.Show<CodeFormat>("", parameters);
        //        var result = await formModal.Result;
        //        if (!result.Cancelled)
        //        {
        //            PreparedCode = (string)result.Data;
        //        }
        //    }
        //}

        /// <summary>
        /// Prepare code for insertion - collect the text
        /// </summary>
        public async Task InsertCode1()
        {
            string xx = await EditObj.GetSelectedHtmlAsync();
            if (xx is not null && xx.Length > 0)
            {
                ShowMessage("Code can not be edited.  Please Copy, Delete, and Reinsert");
                return;
            }
            // get insertion point?? how??

            var parameters = new ModalParameters();
            parameters.Add("stuff", xx);
            parameters.Add("EditObj", EditObj);
            var formModal = Modal.Show<CodeFormat>("", parameters);
            var result = await formModal.Result;
            if (!result.Cancelled)
            {
                PreparedCode = (string)result.Data;
            }
        }

        /// <summary>
        /// Insert the previously prepared code.
        /// </summary>
        public async Task InsertCode2()
        {
            if (!string.IsNullOrEmpty(PreparedCode))
                await EditObj.ExecuteCommandAsync(CommandName.InsertHTML, PreparedCode);
        }

        /// <summary>
        /// Shows a message
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }

        /// <summary>
        /// Cancel out of editing
        /// </summary>
        protected void CancelEdit()
        {
            Navigation.NavigateTo("noteindex/" + Model.NoteFileID);
        }

        //protected void OnClickRef(MouseEventArgs args)
        //{
        //    ShowChild = true;
        //}

        //private void OnClickRefHide(MouseEventArgs args)
        //{
        //    ShowChild = false;
        //}
    }
}
