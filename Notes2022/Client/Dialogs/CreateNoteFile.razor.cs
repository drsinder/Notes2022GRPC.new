using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Notes2022.Client;
using Notes2022.Client.Shared;
using Blazored;
using Blazored.Modal;
using Blazored.Modal.Services;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.LinearGauge;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.SplitButtons;
using Syncfusion.Blazor.Calendars;
using System.ComponentModel.DataAnnotations;
using Notes2022.Proto;

namespace Notes2022.Client.Dialogs
{
    public partial class CreateNoteFile
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public int FileId { get; set; }

        public CreateFileModel dummyFile = new CreateFileModel();
        private async Task HandleValidSubmit()
        {
            await Client.CreateNoteFileAsync(new GNotefile()
            {NoteFileName = dummyFile.NoteFileName, NoteFileTitle = dummyFile.NoteFileTitle}, myState.AuthHeader);
            await ModalInstance.CloseAsync();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        public class CreateFileModel
        {
            [Required]
            public string NoteFileName { get; set; }

            [Required]
            public string NoteFileTitle { get; set; }
        }
    }
}