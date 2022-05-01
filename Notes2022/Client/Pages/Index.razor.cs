using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Notes2022.Proto;
using System.Timers;
using System.Web;
using System.Text;
using System.Text.Json;

namespace Notes2022.Client.Pages
{
    public partial class Index
    {
        private IJSObjectReference? module;

        private HomePageModel? hpModel { get; set; }

        private DateTime mytime { get; set; }

        private GNotefile dummyFile = new GNotefile { Id = 0, NoteFileName = " ", NoteFileTitle = " " };

        private GNotefile item;


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

        public Index()  // Needed for above Injection
        {
        }

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

                // if logged in nothing to do
                if (myState.LoginReply != null && myState.LoginReply.Status == 200)
                    return;

                // check for a cookie with our creds
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./cookies.js");

                string cookie = await ReadCookies();

                if (!string.IsNullOrEmpty(cookie))
                {
                    // found a cookie
                    string json = HttpUtility.HtmlDecode(Base64Decode(cookie));
                    LoginReply? ar = JsonSerializer.Deserialize<LoginReply>(json);

                    if (ar != null && ar.Status == 200 )
                    {

                        //string expires = await ReadCookieExpire();

                        // we have an auth cookie cred - set login and reload
                        myState.LoginReply = ar;

                        Globals.LoginDisplay?.Reload();

                        Navigation.NavigateTo("reload");
                    }
                }
             }
        }

        private static string Base64Decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        //protected async Task<string> ReadCookieExpire()
        //{
        //    try
        //    {
        //        string cookie = await module.InvokeAsync<string>("ReadCookie", "expires");
        //        return cookie;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}

        protected async Task<string> ReadCookies()
        {
            try
            {
                string cookie = await module.InvokeAsync<string>("ReadCookie", Globals.Cookie);
                return cookie;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        protected override async Task OnParametersSetAsync()
        {
            fileList = new List<GNotefile>();
            nameList = new GNotefileList();
            histfileList = new GNotefileList();
            impfileList = new GNotefileList();

            mytime = DateTime.Now;


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

                GNotefileList fileList1 = hpModel.NoteFiles;
                GNotefileList nameList1 = hpModel.NoteFiles;
                fileList = fileList1.Notefiles.ToList().OrderBy(p => p.NoteFileName).ToList();
                nameList = nameList1;

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
        /// Update the clock once per second
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(1000);
                timer2.Elapsed += TimerTick2;
                timer2.Enabled = true;
            }
        }

        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            mytime = DateTime.Now;
            Globals.LoginDisplay?.Reload();
            Globals.NavMenu?.Reload();
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

    }
}