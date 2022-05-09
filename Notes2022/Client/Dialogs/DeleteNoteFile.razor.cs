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
    public partial class DeleteNoteFile
    {
        public CreateFileModel dummyFile = new();
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public int FileId { get; set; }

        [Parameter]
        public string FileName { get; set; }

        [Parameter]
        public string FileTitle { get; set; }

        private async Task HandleValidSubmit()
        {
            await Client.DeleteNoteFileAsync(new GNotefile()
            {Id = FileId}, myState.AuthHeader);
            await ModalInstance.CloseAsync(ModalResult.Ok($"Delete was submitted successfully."));
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