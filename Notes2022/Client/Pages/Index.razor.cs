using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Notes2022.Proto;
using Syncfusion.Blazor.DropDowns;
using System.Timers;


namespace Notes2022.Client.Pages
{
    public partial class Index
    {
        private HomePageModel? hpModel { get; set; }

        private GNotefile dummyFile = new GNotefile { Id = 0, NoteFileName = " ", NoteFileTitle = " " };

        private GNotefile item { get; set; }


        private List<GNotefile> fileList { get; set; }

        /// <summary>
        /// List of files ordered by title
        /// </summary>
        private GNotefileList nameList { get; set; }

        /// <summary>
        /// Important file list
        /// </summary>
        private GNotefileList impfileList { get; set; }

        /// <summary>
        /// History file list
        /// </summary>
        private GNotefileList histfileList { get; set; }

        // For clock update
        private System.Timers.Timer timer2 { get; set; }

        /// <summary>
        /// For access to server via Http
        /// </summary>
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Index()  // Needed for above Injection
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

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

        protected override void OnParametersSet()
        {
            OnParametersSetAsync().GetAwaiter();    // notified of login status change
            StateHasChanged();
        }

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

        private int ticks = 0;

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
        /// <param name="value"></param>
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

        private void ValueChangeHandler(ChangeEventArgs<int, GNotefile> args)
        {
            Navigation.NavigateTo("noteindex/" + args.Value); // goto the file
            
        }

        /// <summary>
        /// Recent menu item - start sequencing
        /// </summary>
        /// <returns></returns>
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