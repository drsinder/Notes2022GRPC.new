// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 05-06-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-08-2022
// ***********************************************************************
// <copyright file="Index.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Notes2022.Proto;
using Syncfusion.Blazor.DropDowns;
using System.Timers;


namespace Notes2022.Client.Pages
{
    /// <summary>
    /// Class Index.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class Index
    {
        /// <summary>
        /// Gets or sets the hp model.
        /// </summary>
        /// <value>The hp model.</value>
        private HomePageModel? hpModel { get; set; }

        /// <summary>
        /// The dummy file
        /// </summary>
        private GNotefile dummyFile = new GNotefile { Id = 0, NoteFileName = " ", NoteFileTitle = " " };

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        private GNotefile item { get; set; }


        /// <summary>
        /// Gets or sets the file list.
        /// </summary>
        /// <value>The file list.</value>
        private List<GNotefile> fileList { get; set; }

        /// <summary>
        /// List of files ordered by title
        /// </summary>
        /// <value>The name list.</value>
        private GNotefileList nameList { get; set; }

        /// <summary>
        /// Important file list
        /// </summary>
        /// <value>The impfile list.</value>
        private GNotefileList impfileList { get; set; }

        /// <summary>
        /// History file list
        /// </summary>
        /// <value>The histfile list.</value>
        private GNotefileList histfileList { get; set; }

        // For clock update
        /// <summary>
        /// Gets or sets the timer2.
        /// </summary>
        /// <value>The timer2.</value>
        private System.Timers.Timer timer2 { get; set; }

        /// <summary>
        /// For access to server via Http
        /// </summary>
        /// <value>The navigation.</value>
        [Inject] NavigationManager Navigation { get; set; }
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        public Index()  // Needed for above Injection
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
        /// on this component instance; otherwise <c>false</c>.</param>
        /// <remarks>The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
        /// once.</remarks>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(1000);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                timer2.Elapsed += TimerTick2;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                timer2.Enabled = true;

                myState.OnChange += OnParametersSet; // get notified of login status changes
            }
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        protected override void OnParametersSet()
        {
            OnParametersSetAsync().GetAwaiter();    // notified of login status change
            StateHasChanged();
        }

        /// <summary>
        /// On parameters set as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            fileList = new List<GNotefile>();
            nameList = new GNotefileList();
            histfileList = new GNotefileList();
            impfileList = new GNotefileList();

            if (myState.IsAuthenticated)
            {
                // Set and reset local state vars
                await sessionStorage.SetItemAsync<int>("ArcId", 0);
                await sessionStorage.SetItemAsync<int>("IndexPage", 1);

                await sessionStorage.SetItemAsync<bool>("IsSeq", false);
                await sessionStorage.RemoveItemAsync("SeqList");
                await sessionStorage.RemoveItemAsync("SeqItem");
                await sessionStorage.RemoveItemAsync("SeqIndex");

                await sessionStorage.RemoveItemAsync("SeqHeaders");
                await sessionStorage.RemoveItemAsync("SeqHeaderIndex");
                await sessionStorage.RemoveItemAsync("CurrentSeqHeader");

                await sessionStorage.SetItemAsync<bool>("InSearch", false);
                await sessionStorage.RemoveItemAsync("SearchIndex");
                await sessionStorage.RemoveItemAsync("SearchList");

                hpModel = await Client.GetHomePageModelAsync(new NoRequest(), myState.AuthHeader);

                //if (hpModel.NoteFiles == null)
                //    return;

                GNotefileList fileList1 = hpModel.NoteFiles;
                GNotefileList nameList1 = hpModel.NoteFiles;
                fileList = fileList1.Notefiles.ToList().OrderBy(p => p.NoteFileName).ToList();
                nameList = nameList1;

                impfileList.Notefiles.Clear();
                histfileList.Notefiles.Clear();

                for (int i = 0; i < fileList1.Notefiles.Count; i++)
                {
                    GNotefile work = new GNotefile { Id = fileList1.Notefiles[i].Id, NoteFileName = fileList1.Notefiles[i].NoteFileName, NoteFileTitle = fileList1.Notefiles[i].NoteFileTitle };

                    // handle special important and history files
                    string fname = work.NoteFileName;
                    if (fname == "Opbnotes" || fname == "Gnotes")
                        histfileList.Notefiles.Add(work);

                    if (fname == "announce" || fname == "pbnotes" || fname == "noteshelp")
                        impfileList.Notefiles.Add(work);
                }
            }
        }

        /// <summary>
        /// The ticks
        /// </summary>
        private int ticks = 0;

        /// <summary>
        /// Timers the tick2.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            if (++ticks == 10)
                timer2.Interval = 5000;
            else if (++ticks == 60)
                timer2.Interval = 15000;

            Globals.LoginDisplay?.Reload();
            Globals.NavMenu?.Reload().GetAwaiter();
            StateHasChanged();
        }

        /// <summary>
        /// Handle typed in file name
        /// </summary>
        /// <param name="value">The value.</param>
        protected void TextHasChanged(string value)
        {
            value = value.Trim().Replace("'\n", "").Replace("'\r", ""); //.Replace(" ", "");

            try
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    item = fileList[i];
                    if (value == item.NoteFileName)
                    {
                        Navigation.NavigateTo("noteindex/" + item.Id); // goto the file
                        return;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Values the change handler.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void ValueChangeHandler(ChangeEventArgs<int, GNotefile> args)
        {
            Navigation.NavigateTo("noteindex/" + args.Value); // goto the file
            
        }

        /// <summary>
        /// Recent menu item - start sequencing
        /// </summary>
        private async Task StartSeq()
        {
            // get users list of files
            //List<GSequencer> sequencers = await DAL.GetSequencer(Http);

            List<GSequencer> sequencers = (await Client.GetSequencerAsync(new NoRequest(), myState.AuthHeader)).List.ToList();
            if (sequencers.Count == 0)
                return;

            // order them as prefered by user
            sequencers = sequencers.OrderBy(p => p.Ordinal).ToList();

            // set up state for sequencing
            await sessionStorage.SetItemAsync<List<GSequencer>>("SeqList", sequencers);
            await sessionStorage.SetItemAsync<int>("SeqIndex", 0);
            await sessionStorage.SetItemAsync<GSequencer>("SeqItem", sequencers[0]);
            await sessionStorage.SetItemAsync<bool>("IsSeq", true); // flag for noteindex
            // begin

            string go = "noteindex/" + sequencers[0].NoteFileId;
            Navigation.NavigateTo(go);
            return;
        }


    }
}