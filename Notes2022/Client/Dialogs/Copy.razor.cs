using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Notes2022.Shared;

namespace Notes2022.Client.Dialogs
{
    public partial class Copy
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public GNoteHeader Note { get; set; }

        //[Parameter] public UserData UserData { get; set; }
        private GNotefileList Files { get; set; }

        private bool WholeString { get; set; }

        private int SelectedId { get; set; } = 0;
        protected async override Task OnInitializedAsync()
        {
            Files = await Client.GetNoteFilesOrderedByNameAsync(new NoRequest(), myState.AuthHeader);
            Files.Notefiles.Insert(0, new GNotefile { Id = 0, NoteFileName = "Select a file" });
        }

        protected async Task OnSubmit()
        {
            if (SelectedId == 0)
                return;
            CopyModel cm = new CopyModel();
            cm.FileId = SelectedId;
            cm.Note = Note;
            cm.WholeString = WholeString;
            //cm.UserData = UserData;
            await Client.CopyNoteAsync(cm, myState.AuthHeader);
            await ModalInstance.CloseAsync();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}