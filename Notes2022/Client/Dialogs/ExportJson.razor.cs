using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Notes2022.Proto;
using System.Text;

namespace Notes2022.Client.Dialogs
{
    public partial class ExportJson
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public ExportViewModel model { get; set; }
        private string FileName { get; set; }

        private IJSObjectReference? module;

        private bool marked { get; set; }
        private string message = "Getting ready...";

        protected async override Task OnInitializedAsync()
        {
            FileName = model.NoteFile.NoteFileName + ".json";

            if (model.Marks is not null && model.Marks.Count > 0)
                marked = true;
            else
                marked = false;

            message = "Exporting " + FileName;

            MemoryStream ms2 = await DoExport();

            await SaveAs(FileName, ms2.GetBuffer());
            ms2.Dispose();

            await ModalInstance.CancelAsync();
        }

        private async Task<MemoryStream> DoExport()
        {
            GNotefile nf = model.NoteFile;
            int nfid = nf.Id;
            StringContent stringContent;

            JsonExport stuff;
            stuff = await Client.GetExportJsonAsync(new ExportRequest() { FileId = nfid, ArcId = 0 }, myState.AuthHeader);
            stringContent = new StringContent(JsonConvert.SerializeObject(stuff, Newtonsoft.Json.Formatting.Indented), Encoding.UTF8, "application/json");

            Stream ms0 = await stringContent.ReadAsStreamAsync();
            MemoryStream ms = new MemoryStream();
            await ms0.CopyToAsync(ms);
            ms0.Dispose();
            ms.Close();
            return ms;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./scripts.js");
            }
        }


        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }

        public async Task SaveAs(string filename, byte[] data)
        {
            await module.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
        }


    }
}
