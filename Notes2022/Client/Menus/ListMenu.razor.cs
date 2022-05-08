/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Description:
    **     Menu bar for Main List of notes
    **
    ** This program is free software: you can redistribute it and / or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ** GNU General Public License version 3 for more details.
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
using Notes2022.Shared;
using Syncfusion.Blazor.LinearGauge;
using Syncfusion.Blazor.Navigations;
using System.Net.Http.Json;
using System.Text;

namespace Notes2022.Client.Menus
{
    /// <summary>
    /// Menu at top of noteindex listing
    /// </summary>
    public partial class ListMenu
    {
        /// <summary>
        ///  for showing dialogs
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// reference to data model for index display
        /// </summary>
        [Parameter] public NoteDisplayIndexModel Model { get; set; }

        /// <summary>
        /// reference to the caller/container
        /// </summary>
        [Parameter] public NoteIndex Caller { get; set; }

        /// <summary>
        /// Menu items/structure
        /// </summary>
        private static List<MenuItem> menuItems { get; set; }

        /// <summary>
        /// Top menu item instance
        /// </summary>
        protected SfMenu<MenuItem> topMenu { get; set; }

        public SfLinearGauge myGauge { get; set; }

        private bool HamburgerMode { get; set; } = false;

        /// <summary>
        /// Are we printing?
        /// </summary>
        public bool IsPrinting { get; set; } = false;

        /// <summary>
        /// Text value for slider while doing background processing
        /// </summary>
        //public string sliderValueText { get; set; }

        /// <summary>
        /// Number of base notes we have
        /// </summary>
        protected int baseNotes { get; set; }

        /// <summary>
        /// Ordinal of the current note
        /// </summary>
        protected int currNote { get; set; }

        [Inject] NavigationManager Navigation { get; set; }
        public ListMenu()   // needed for injection above...
        {
        }

        /// <summary>
        /// Initializations
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            baseNotes = Model.Notes.List.Count;
            //sliderValueText = "1/" + baseNotes;

            // construct the menu based on user access
            menuItems = new List<MenuItem>();

            MenuItem item = new() { Id = "ListNoteFiles", Text = "List Note Files" };
            menuItems.Add(item);
            if (Model.MyAccess.Write)
            {
                item = new() { Id = "NewBaseNote", Text = "New Base Note" };
                menuItems.Add(item);
            }
            if (Model.MyAccess.ReadAccess)
            {
                MenuItem item2 = new() { Id = "OutPutMenu", Text = "Output" };
                item2.Items = new List<MenuItem>
                {
                    new () { Id = "eXport", Text = "eXport" },
                    new () { Id = "HtmlFromIndex", Text = "Html (expandable)" },
                    new () { Id = "htmlFromIndex", Text = "html (flat)" },
                    new () { Id = "mailFromIndex", Text = "mail" },
                    //item2.Items.Add(new MenuItem() { Id = "MarkMine", Text = "Mark my notes for output" });
                    new MenuItem() { Id = "PrintFile", Text = "Print entire file" },

                    //if (Model.isMarked)
                    //{
                    //    item2.Items.Add(new MenuItem() { Id = "OutputMarked", Text = "Output marked notes" });
                    //}

                    new (){ Id = "JsonExport", Text = "Json Export" }
                };

                menuItems.Add(item2);

                menuItems.Add(new MenuItem() { Id = "SearchFromIndex", Text = "Search" });
                menuItems.Add(new MenuItem() { Id = "ListHelp", Text = "Z for HELP" });
                if (Model.MyAccess.EditAccess || Model.MyAccess.ViewAccess)
                {
                    menuItems.Add(new MenuItem() { Id = "AccessControls", Text = "Access Controls" });
                }
            }
        }

        /// <summary>
        /// When a Menu item is selected
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }

        /// <summary>
        /// The container has a refernce to "this" and can call this method...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ExecMenu(string id)
        {
            switch (id)
            {
                case "ListNoteFiles":
                    Navigation.NavigateTo("notesfiles/");
                    break;

                case "ReloadIndex": // only a direct type in
                    Navigation.NavigateTo("noteindex/" + Model.NoteFile.Id, true);
                    break;

                case "NewBaseNote":
                    Navigation.NavigateTo("newnote/" + Model.NoteFile.Id + "/0" + "/0");
                    break;

                case "ListHelp":
                    Modal.Show<HelpDialog>();
                    break;

                case "AccessControls":
                    var parameters = new ModalParameters();
                    parameters.Add("fileId", Model.NoteFile.Id);
                    Modal.Show<AccessList>("", parameters);
                    break;

                case "eXport":
                    DoExport(false, false);
                    break;

                case "HtmlFromIndex":
                    DoExport(true, true);
                    break;

                case "htmlFromIndex":
                    DoExport(true, false);
                    break;

                case "JsonExport":
                    DoJson();
                    break;

                case "mailFromIndex":
                    await DoEmail();
                    break;

                case "PrintFile":
                    await PrintFile();
                    break;

                case "SearchFromIndex":
                    await SetSearch();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Display Search dialog in prep for a search
        /// </summary>
        /// <returns></returns>
        private async Task SetSearch()
        {
            var parameters = new ModalParameters();
            parameters.Add("searchtype", "File");
            var formModal = Modal.Show<SearchDlg>();
            var result = await formModal.Result;
            if (result.Cancelled)
                return;
            else
            {
                NoteIndex.Search target = (NoteIndex.Search)result.Data;
                // start the search - call contrainer method
                await Caller.StartSearch(target);
                return;
            }

        }

        /// <summary>
        /// Print file set up
        /// </summary>
        /// <returns></returns>
        private async Task PrintFile()
        {
            currNote = 1;
            IsPrinting = true;
            await PrintFile2();
            IsPrinting = false;
        }

        /// <summary>
        /// Print the whole file
        /// </summary>
        protected async Task PrintFile2()
        {
            string respX = String.Empty;

            // keep track of base note
            GNoteHeader baseHeader = Model.Notes.List[0];

            GNoteHeader currentHeader = Model.Notes.List[0];

            StringBuilder sb = new();

            sb.Append("<h4 class=\"text-center\">" + Model.NoteFile.NoteFileTitle + "</h4>");

        reloop: // come back here to do another note
            respX = "";
            if (currentHeader.ResponseCount > 0)
                respX = " - " + currentHeader.ResponseCount + " Responses ";
            else if (currentHeader.ResponseOrdinal > 0)
                respX = " Response " + currentHeader.ResponseOrdinal;


            sb.Append("<div class=\"noteheader\"><p> <span class=\"keep-right\">Note: ");
            sb.Append(currentHeader.NoteOrdinal + " " + respX);
            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;</span></p><h4>Subject: ");
            sb.Append(currentHeader.NoteSubject);
            sb.Append("<br />Author: ");
            sb.Append(currentHeader.AuthorName + "    ");
            sb.Append((Globals.LocalTimeBlazor(currentHeader.LastEdited.ToDateTime()).ToLongDateString()) + " " 
                + (Globals.LocalTimeBlazor(currentHeader.LastEdited.ToDateTime()).ToShortTimeString()));

            GNoteContent currentContent = await Client.GetExport2Async(new NoteId() { Id = currentHeader.Id }, myState.AuthHeader);

            if (!string.IsNullOrEmpty(currentHeader.DirectorMessage))
            {
                sb.Append("<br /><span>Director Message: ");
                sb.Append(currentHeader.DirectorMessage);
                sb.Append("</span>");
            }
            //if (tags is not null && tags.Count > 0)
            //{
            //    sb.Append(" <br /><span>Tags: ");
            //    foreach (Tags tag in tags)
            //    {
            //        sb.Append(tag.Tag + " ");
            //    }
            //    sb.Append("</span>");
            //}
            sb.Append("</h4></div><div class=\"notebody\" >");
            sb.Append(currentContent.NoteBody);
            sb.Append("</div>");

            if (currentHeader.ResponseOrdinal < baseHeader.ResponseCount) // more responses in string
            {
                currentHeader = Model.AllNotes.List.Single(p => p.NoteOrdinal == currentHeader.NoteOrdinal && p.ResponseOrdinal == currentHeader.ResponseOrdinal + 1);

                goto reloop;        // print another note
            }

            currentHeader = baseHeader; // set back to base note

            GNoteHeader next = Model.Notes.List.SingleOrDefault(p => p.NoteOrdinal == currentHeader.NoteOrdinal + 1);
            if (next is not null)       // still base notes left to print
            {
                currentHeader = next;   // set current note and base note
                baseHeader = next;
                //await SetNote();        // set important stuff
                //sliderValueText = currentHeader.NoteOrdinal + "/" + baseNotes;  // update progress test
                currNote = currentHeader.NoteOrdinal;                           // update progress bar
                myGauge.SetPointerValue(0, 0, currNote);

                goto reloop;    // print another string
            }

            string stuff = sb.ToString();           // turn accumulated output into a string

            var parameters = new ModalParameters();
            parameters.Add("PrintStuff", stuff);    // pass string to print dialog
            Modal.Show<PrintDlg>("", parameters);   // invoke print dialog with our output

        }

        /// <summary>
        /// Export a file
        /// </summary>
        /// <param name="isHtml">true if in html format - else text </param>
        /// <param name="isCollapsible">collapsible/expandable html?</param>
        /// <param name="isEmail">Should we mail it?</param>
        /// <param name="emailaddr">Where to mail it</param>
        private void DoExport(bool isHtml, bool isCollapsible, bool isEmail = false, string emailaddr = null)
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new();
            vm.ArchiveNumber = Model.ArcId;
            vm.isCollapsible = isCollapsible;
            vm.isDirectOutput = !isEmail;
            vm.isHtml = isHtml;
            vm.NoteFile = Model.NoteFile;
            vm.NoteOrdinal = 0;
            vm.Email = emailaddr;
            vm.myMenu = this;
            currNote = 1;

            parameters.Add("Model", vm);
            parameters.Add("FileName", Model.NoteFile.NoteFileName + (isHtml ? ".html" : ".txt"));

            Modal.Show<ExportUtil1>("", parameters);
        }

        public void Replot()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Prepare Json output
        /// </summary>
        private void DoJson()
        {
            var parameters = new ModalParameters();

            ExportViewModel vm = new();
            vm.ArchiveNumber = Model.ArcId;
            vm.NoteFile = Model.NoteFile;
            vm.NoteOrdinal = 0;

            parameters.Add("model", vm);

            Modal.Show<ExportJson>("", parameters);
        }

        private async Task DoEmail()
        {
            string emailaddr;
            var parameters = new ModalParameters();
            var formModal = Modal.Show<Email>("", parameters);
            var result = await formModal.Result;
            if (result.Cancelled)
                return;
            emailaddr = (string)result.Data;
            if (string.IsNullOrEmpty(emailaddr))
                return;

            DoExport(true, true, true, emailaddr);
        }

        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("", parameters);
        }
    }
}
