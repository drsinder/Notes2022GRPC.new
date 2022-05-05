﻿/*--------------------------------------------------------------------------
    **
    **Copyright © 2022, Dale Sinder
    **
    **  Name: NoteIndex.razor
    **
    ** Description: Displays the main file index grid
    **     Base notes and expands to show responses
    **
    **  This program is free software: you can redistribute it and/or modify
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
using Notes2022.Client.Menus;
using Notes2022.Proto;
//using Notes2022.RCL.User.Dialogs;
using Notes2022.Shared;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Google.Protobuf.WellKnownTypes;

//using SearchOption = Notes2022.Shared.SearchOption;

namespace Notes2022.Client.Pages
{
    /// <summary>
    /// Index/Listing for a NoteFile.  Has an embeded NotePanel that can be
    /// displayed in place of the index when the user selects a note to view.
    /// </summary>
    public partial class NoteIndex
    {
        /// <summary>
        /// For dialogs
        /// </summary>
        [CascadingParameter] public IModalService Modal { get; set; }

        /// <summary>
        /// The NoteFileId we are using
        /// </summary>
        [Parameter] public int NotesfileId { get; set; }

        /// <summary>
        /// Non zero when viewing a note
        /// </summary>
        [Parameter] public long CurrentNoteId { get; set; }

        /// <summary>
        /// Reference to the menu so we can talk to it.
        /// </summary>
        protected ListMenu MyMenu { get; set; }

        /// <summary>
        /// Accumulator for the navigation string
        /// </summary>
        public string NavString { get; set; }

        /// <summary>
        /// Our direct navigation typin box
        /// </summary>
        protected SfTextBox sfTextBox { get; set; }

        /// <summary>
        /// Our index grid
        /// </summary>
        protected SfGrid<GNoteHeader> sfGrid1 { get; set; }

        /// <summary>
        /// Filter setting for the grid
        /// </summary>
        protected GridFilterSettings FilterSettings { get; set; }

        /// <summary>
        /// Page settings for the grid
        /// </summary>
        protected GridPageSettings PageSettings { get; set; }

        /// <summary>
        /// Grid page size
        /// </summary>
        protected int PageSize { get; set; }

        /// <summary>
        /// Current page of grid
        /// </summary>
        protected int CurPage { get; set; }

        /// <summary>
        /// Should note body be shown?
        /// </summary>
        protected bool ShowContent { get; set; }

        /// <summary>
        /// Should resopnse body be shown?
        /// </summary>
        protected bool ShowContentR { get; set; }

        /// <summary>
        /// If the grid expanded fully expanded
        /// </summary>
        protected bool ExpandAll { get; set; }

        /// <summary>
        /// Are we sequencing?
        /// </summary>
        protected bool IsSeq { get; set; }

        /// <summary>
        /// Model for the index data
        /// </summary>
        public NoteDisplayIndexModel Model { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public NoteIndex()
        {
        }

        /// <summary>
        /// Set up and get data
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            await sessionStorage.SetItemAsync<bool>("InSearch", false);
            await sessionStorage.RemoveItemAsync("SearchIndex");
            await sessionStorage.RemoveItemAsync("SearchList");

            IsSeq = await sessionStorage.GetItemAsync<bool>("IsSeq");
            if (IsSeq && NotesfileId < 0)
            {
                NotesfileId = -NotesfileId;
            }

            // Get the notefile data
            Model = await Client.GetNoteFileIndexDataAsync(new NoteIndexRequest() { NoteFileId = NotesfileId }, myState.AuthHeader);

            // Set preferences for user
            PageSize = Model.UserData.Ipref2;
            ShowContent = Model.UserData.Pref7;
            ExpandAll = false; // Model.UserData.Pref3;

            // restore page
            CurPage = await sessionStorage.GetItemAsync<int>("IndexPage");

            if (IsSeq)
                await StartSeq();
        }

        /// <summary>
        /// Note selected for display
        /// </summary>
        /// <param name="args"></param>
        protected void DisplayIt(RowSelectEventArgs<GNoteHeader> args)
        {
            sessionStorage.SetItemAsync("IndexPage", sfGrid1.PageSettings.CurrentPage).GetAwaiter();
            CurrentNoteId = args.Data.Id;
            StateHasChanged();
        }

        /// <summary>
        /// Goto a specific note
        /// </summary>
        /// <param name="Id"></param>
        public void GotoNote(long Id)
        {
            CurrentNoteId = Id;
            StateHasChanged();
        }

        /// <summary>
        /// Goto the listing mode from note display mode
        /// </summary>
        public void Listing()
        {
            CurrentNoteId = 0;
            StateHasChanged();
        }

        /// <summary>
        /// Get the next base note header given the current one
        /// </summary>
        /// <param name="oh"></param>
        /// <returns></returns>
        public long GetNextBaseNote(GNoteHeader oh)
        {
            long newId = 0;
            GNoteHeader nh =Model.Notes.List.SingleOrDefault(p => p.NoteOrdinal == oh.NoteOrdinal + 1 && p.ResponseOrdinal == 0 && p.Version == 0);
            if (nh is not null)
                newId = nh.Id;
            return newId;
        }

        /// <summary>
        /// Get the nest note given the current one
        /// </summary>
        /// <param name="oh"></param>
        /// <returns></returns>
        public long GetNextNote(GNoteHeader oh)
        {
            long newId = 0;
            GNoteHeader nh = null;
            nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == oh.NoteOrdinal && p.ResponseOrdinal == (oh.ResponseOrdinal + 1) && p.Version == 0);
            if (nh is null)
                nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == (oh.NoteOrdinal + 1) && p.ResponseOrdinal == 0 && p.Version == 0);
            if (nh is not null)
                newId = nh.Id;
            return newId;
        }

        /// <summary>
        /// Get the previous base note
        /// </summary>
        /// <param name="oh"></param>
        /// <returns></returns>
        public long GetPreviousBaseNote(GNoteHeader oh)
        {
            long newId = 0;
            GNoteHeader nh = Model.Notes.List.SingleOrDefault(p => p.NoteOrdinal == oh.NoteOrdinal - 1 && p.ResponseOrdinal == 0 && p.Version == 0);
            if (nh is not null)
                newId = nh.Id;
            return newId;
        }

        /// <summary>
        /// Get the previous note
        /// </summary>
        /// <param name="oh"></param>
        /// <returns></returns>
        public long GetPreviousNote(GNoteHeader oh)
        {
            long newId = 0;
            GNoteHeader nh = null;
            nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == oh.NoteOrdinal && p.ResponseOrdinal == oh.ResponseOrdinal - 1 && p.Version == 0);
            if (nh is null)
                nh = Model.Notes.List.SingleOrDefault(p => p.NoteOrdinal == oh.NoteOrdinal - 1 && p.ResponseOrdinal == 0 && p.Version == 0);
            if (nh is not null)
                newId = nh.Id;
            return newId;
        }

        /// <summary>
        /// Get just the response headers for the given noteid
        /// </summary>
        /// <param name="headerId"></param>
        /// <returns></returns>
        public List<GNoteHeader> GetResponseHeaders(long headerId)
        {
            return Model.AllNotes.List.Where(p => p.BaseNoteId == headerId && (p.ResponseOrdinal != 0) && p.IsDeleted == false && p.Version == 0)
                .OrderBy(p => p.ResponseOrdinal).ToList();
        }

        /// <summary>
        /// Get the Index model - used by the NotePanel
        /// </summary>
        /// <returns></returns>
        public NoteDisplayIndexModel GetModel()
        {
            return Model;
        }

        /// <summary>
        /// Get note header Id given note ordinal and response ordinal
        /// </summary>
        /// <param name="noteOrd"></param>
        /// <param name="respOrd"></param>
        /// <returns></returns>
        public long GetNoteHeaderId(int noteOrd, int respOrd)
        {
            long newId = 0;
            GNoteHeader nh;

            nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == noteOrd && p.ResponseOrdinal == respOrd && p.Version == 0);
            if (nh is null && respOrd > -1) // try next base note -- special case if noteOrd == 0 and ResponseOrd == 0  ==> get first base note in file
            {
                nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == noteOrd + 1 && p.ResponseOrdinal == 0 && p.Version == 0);
            }
            else if (nh is null)    // try previous base note
            {
                nh = Model.AllNotes.List.SingleOrDefault(p => p.NoteOrdinal == noteOrd - 1 && p.ResponseOrdinal == 0 && p.Version == 0);
            }
            if (nh is not null)
                newId = nh.Id;

            return newId;
        }

        /// <summary>
        /// Search results
        /// </summary>
        private List<GNoteHeader> results { get; set; }

        /// <summary>
        /// Are we searching?
        /// </summary>
        private bool isSearch { get; set; }

        /// <summary>
        /// Temp used for navigation
        /// </summary>
        private long mode { get; set; }

        public enum SearchOption { Author, Title, Content, Tag, DirMess, TimeIsAfter, TimeIsBefore }
        [DataContract]
        public class Search
        {
            // User doing the search
            [StringLength(450)]
            [DataMember(Order = 1)]
            public string? UserId { get; set; }

            // search specs Option
            [Display(Name = "Search By")]
            [DataMember(Order = 2)]
            public SearchOption Option { get; set; }

            // Text to search for
            [Display(Name = "Search Text")]
            [DataMember(Order = 3)]
            public string? Text { get; set; }

            // DateTime to compare to
            [Display(Name = "Search Date/Time")]
            [DataMember(Order = 4)]
            public DateTime Time { get; set; }

            // current/next info -- where we are in the search
            [Column(Order = 0)]
            [DataMember(Order = 5)]
            public int NoteFileId { get; set; }

            [Required]
            [Column(Order = 1)]
            [DataMember(Order = 6)]
            public int ArchiveId { get; set; }

            [Column(Order = 2)]
            [DataMember(Order = 7)]
            public int BaseOrdinal { get; set; }
            [Column(Order = 3)]
            [DataMember(Order = 8)]
            public int ResponseOrdinal { get; set; }
            [Column(Order = 4)]
            [DataMember(Order = 9)]
            public long NoteID { get; set; }
        }
         
        public async Task StartSearch(Search target)
        {
            //message = "Searching... Please Wait...";
            //StateHasChanged();

            target.Text = target.Text.ToLower();

            switch (target.Option)
            {
                case SearchOption.Author:
                case SearchOption.Title:
                case SearchOption.TimeIsAfter:
                case SearchOption.TimeIsBefore:
                case SearchOption.DirMess:
                    await SearchHeader(target);
                    break;

                case SearchOption.Content:
                    await SearchContents(target);
                    break;

                case SearchOption.Tag:
                    await SearchTags(target);
                    break;

                default:
                    break;
            }

            //message = null;
            //StateHasChanged();
        }


        protected async Task SearchTags(Search target)
        {
            if (Model.Tags.List == null || Model.Tags.List.Count == 0)
            {
                ShowMessage("Nothing Found.");
                return;
            }

            List<GTags> tags = Model.Tags.List.Where(p => p.Tag.ToLower().Contains(target.Text)).ToList();
            if (tags == null || tags.Count == 0)
            {
                ShowMessage("Nothing Found.");
                return;
            }

            results = new List<GNoteHeader>();
            foreach (GTags tag in tags)
            {
                GNoteHeader h = Model.AllNotes.List.SingleOrDefault(p => p.Id == tag.NoteHeaderId);
                if (h != null)
                    results.Add(h);
            }

            if (results.Count == 0)
            {
                ShowMessage("Nothing Found.");
                return;
            }

            results = results.OrderBy(p => p.NoteOrdinal).ThenBy(p => p.ResponseOrdinal).ToList();

            mode = results[0].Id;
            isSearch = true;

            await sessionStorage.SetItemAsync<bool>("InSearch", true);
            await sessionStorage.SetItemAsync<int>("SearchIndex", 0);
            await sessionStorage.SetItemAsync<List<GNoteHeader>>("SearchList", results);

            CurrentNoteId = mode;
            StateHasChanged();
        }

        protected async Task SearchHeader(Search target)
        {
            results = new List<GNoteHeader>();
            List<GNoteHeader> lookin = Model.AllNotes.List.ToList();

            foreach (GNoteHeader nh in lookin)
            {
                bool isMatch = false;
                switch (target.Option)
                {
                    case SearchOption.Author:
                        isMatch = nh.AuthorName.ToLower().Contains(target.Text);
                        break;
                    case SearchOption.Title:
                        isMatch = nh.NoteSubject.ToLower().Contains(target.Text);
                        break;
                    case SearchOption.DirMess:
                        if (!string.IsNullOrEmpty(nh.DirectorMessage))
                            isMatch = nh.DirectorMessage.ToLower().Contains(target.Text);
                        break;
                    case SearchOption.TimeIsAfter:
                        isMatch = DateTime.Compare(nh.LastEdited.ToDateTime(), target.Time) > 0;
                        break;
                    case SearchOption.TimeIsBefore:
                        isMatch = DateTime.Compare(nh.LastEdited.ToDateTime(), target.Time) < 0;
                        break;
                }
                if (isMatch)
                    results.Add(nh);
            }

            if (results.Count == 0)
            {
                ShowMessage("Nothing Found.");
                return;
            }

            results = results.OrderBy(p => p.NoteOrdinal).ThenBy(p => p.ResponseOrdinal).ToList();

            mode = results[0].Id;
            isSearch = true;

            await sessionStorage.SetItemAsync<bool>("InSearch", true);
            await sessionStorage.SetItemAsync<int>("SearchIndex", 0);
            await sessionStorage.SetItemAsync<List<GNoteHeader>>("SearchList", results);

            CurrentNoteId = mode;
            StateHasChanged();
        }

        protected async Task SearchContents(Search target)
        {
            results = new List<GNoteHeader>();
            List<GNoteHeader> lookin = Model.AllNotes.List.ToList();

            foreach (GNoteHeader nh in lookin)
            {
                //DisplayModel dm = await DAL.GetNoteContent(Http, nh.Id);

                DisplayModel dm = await Client.GetNoteContentAsync(new DisplayModelRequest() { NoteId = nh.Id }, myState.AuthHeader);

                GNoteContent nc = dm.Content;

                bool isMatch = false;
                switch (target.Option)
                {
                    case SearchOption.Content:
                        isMatch = nc.NoteBody.ToLower().Contains(target.Text);
                        break;
                }
                if (isMatch)
                    results.Add(nh);
            }

            if (results.Count == 0)
            {
                ShowMessage("Nothing Found.");
                return;
            }

            results = results.OrderBy(p => p.NoteOrdinal).ThenBy(p => p.ResponseOrdinal).ToList();

            mode = results[0].Id;

            await sessionStorage.SetItemAsync<bool>("InSearch", true);
            await sessionStorage.SetItemAsync<int>("SearchIndex", 0);
            await sessionStorage.SetItemAsync<List<GNoteHeader>>("SearchList", results);

            CurrentNoteId = mode;
            StateHasChanged();
        }

        protected async Task StartSeq()
        {
            GSequencer seq = await sessionStorage.GetItemAsync<GSequencer>("SeqItem");
            if (seq is null)
                return;

            List<GNoteHeader> noteHeaders1 = Model.AllNotes.List.ToList().FindAll(p => p.IsDeleted == false && p.Version == 0);

            List<GNoteHeader> noteHeaders2 = new List<GNoteHeader>();
            foreach (GNoteHeader noteHeader in noteHeaders1)
            {
                if (DateTime.Compare(noteHeader.LastEdited.ToDateTime(), seq.LastTime.ToDateTime()) >= 0L)
                {
                    noteHeaders2.Add(noteHeader);
                }
            }

            List<GNoteHeader> noteHeaders = noteHeaders2
                    .OrderBy(p => p.NoteOrdinal)
                    .ThenBy(p => p.ResponseOrdinal)
                    .ToList();

            if (noteHeaders.Count == 0)
            {
                List<GSequencer> sequencers = await sessionStorage.GetItemAsync<List<GSequencer>>("SeqList");
                int seqIndex = await sessionStorage.GetItemAsync<int>("SeqIndex");
                if (sequencers.Count <= ++seqIndex)
                {
                    await sessionStorage.SetItemAsync("IsSeq", false);
                    await sessionStorage.RemoveItemAsync("SeqList");
                    await sessionStorage.RemoveItemAsync("SeqItem");
                    await sessionStorage.RemoveItemAsync("SeqIndex");

                    await sessionStorage.RemoveItemAsync("SeqHeaders");
                    await sessionStorage.RemoveItemAsync("SeqHeaderIndex");
                    await sessionStorage.RemoveItemAsync("CurrentSeqHeader");

                    ShowMessage("You have seen all the new notes!");

                    Navigation.NavigateTo("");

                    return;  // end it all
                }

                GSequencer currSeq = sequencers[seqIndex];

                await sessionStorage.SetItemAsync("SeqIndex", seqIndex);

                Navigation.NavigateTo("noteindex/" + -currSeq.NoteFileId);
                return;
            }

            await sessionStorage.SetItemAsync("SeqHeaders", noteHeaders);
            await sessionStorage.SetItemAsync("SeqHeaderIndex", 0);

            GNoteHeader currHeader = noteHeaders[0];

            await sessionStorage.SetItemAsync("CurrentSeqHeader", currHeader);

            seq.Active = true;

            await Client.UpdateSequencerAsync(seq, myState.AuthHeader);

            CurrentNoteId = currHeader.Id;
            StateHasChanged();
        }

        public async void ActionCompleteHandler(ActionEventArgs<GNoteHeader> args)
        {
            await sessionStorage.SetItemAsync("IndexPage", sfGrid1.PageSettings.CurrentPage);
            CurPage = await sessionStorage.GetItemAsync<int>("IndexPage");
        }

        /// <summary>
        /// Potential navigation event when ever a key up occurs
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task KeyUpHandler(KeyboardEventArgs args)
        {
            // handle single key press events
            // call up into the menu to execute
            switch (NavString)
            {
                case "L":
                    await ClearNav();
                    await MyMenu.ExecMenu("ListNoteFiles");
                    return;

                case "N":
                    await ClearNav();
                    await MyMenu.ExecMenu("NewBaseNote");
                    return;

                case "X":
                    await ClearNav();
                    await MyMenu.ExecMenu("eXport");
                    return;

                case "J":
                    await ClearNav();
                    await MyMenu.ExecMenu("JsonExport");
                    return;

                case "m":
                    await ClearNav();
                    await MyMenu.ExecMenu("mailFromIndex");
                    return;

                case "P":
                    await ClearNav();
                    await MyMenu.ExecMenu("PrintFile");
                    return;

                case "Z":
                    await ClearNav();
                    Modal.Show<HelpDialog>();
                    return;

                case "H":
                    await ClearNav();
                    await MyMenu.ExecMenu("HtmlFromIndex");
                    return;

                case "h":
                    await ClearNav();
                    await MyMenu.ExecMenu("htmlFromIndex");
                    return;

                case "R":
                    await ClearNav();
                    await MyMenu.ExecMenu("ReloadIndex");
                    return;

                case "A":
                    await ClearNav();
                    await MyMenu.ExecMenu("AccessControls");
                    return;

                case "S":
                    await ClearNav();
                    await MyMenu.ExecMenu("SearchFromIndex");
                    return;

                default:
                    break;
            }

            // Enter press - look for processing
            // Look at NotePanel documentation for how this is processed...
            // It's more involved there anyway...
            if (args.Key == "Enter")
            {
                if (!string.IsNullOrEmpty(NavString))
                {
                    string stuff = NavString.Replace(";", "").Replace(" ", "");

                    // parse string for # or #.#

                    string[] parts = stuff.Split('.');
                    if (parts.Length > 2)
                    {
                        ShowMessage("Too many '.'s : " + parts.Length);
                    }
                    int noteNum;
                    if (parts.Length == 1)
                    {
                        if (!int.TryParse(parts[0], out noteNum))
                        {
                            ShowMessage("Could not parse : " + parts[0]);
                        }
                        else
                        {
                            long headerId = GetNoteHeaderId(noteNum, 0); 
                            if (headerId != 0)
                            {
                                CurrentNoteId = headerId;
                                StateHasChanged();
                                return;
                            }
                            else
                                ShowMessage("Could not find note : " + stuff);
                        }
                    }
                    else if (parts.Length == 2)
                    {
                        if (!int.TryParse(parts[0], out noteNum))
                        {
                            ShowMessage("Could not parse : " + parts[0]);
                        }
                        int noteRespOrd;
                        if (!int.TryParse(parts[1], out noteRespOrd))
                        {
                            ShowMessage("Could not parse : " + parts[1]);
                        }
                        if (noteNum != 0 && noteRespOrd != 0)
                        {
                            long headerId = GetNoteHeaderId(noteNum, noteRespOrd);
                            if (headerId != 0)
                            {
                                CurrentNoteId = headerId;
                                StateHasChanged();
                                return;
                            }
                            else
                                ShowMessage("Could not find note : " + stuff);
                        }
                    }
                    await ClearNav();
                }
            }
        }

        /// <summary>
        /// Accumulate input
        /// </summary>
        /// <param name="args"></param>
        private async void NavInputHandler(InputEventArgs args)
        {
            NavString = args.Value;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Clear accumulated input
        /// </summary>
        /// <returns></returns>
        private async Task ClearNav()
        {
            NavString = null;

            await Task.CompletedTask;
        }

        /// <summary>
        /// Handle state change for expand all switch
        /// </summary>
        /// <param name="args"></param>
        private async void ExpandAllChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (ExpandAll)
            {
                await sfGrid1.ExpandAllDetailRowAsync();
            }
            else
            {
                await sfGrid1.CollapseAllDetailRowAsync();
            }
        }

        /// <summary>
        /// Shows a simple text message dialog
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            //Modal.Show<MessageBox>("", parameters);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender)
            {   // have to wait a bit before putting focus in textbox
                if (ExpandAll)
                    await sfGrid1.ExpandAllDetailRowAsync();

                if (sfTextBox is not null)
                {
                    await Task.Delay(300);
                    await sfTextBox.FocusAsync();
                }
            }
            else
            {
            }
        }
    }
}
