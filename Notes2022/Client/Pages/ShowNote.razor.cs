using Microsoft.AspNetCore.Components;
using Notes2022.Proto;

namespace Notes2022.Client.Pages
{
    public partial class ShowNote
    {
        [Parameter] public long NoteId { get; set; }

        public int FileId { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public ShowNote()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            // find the file id for this note - get note header
            FileId = (await Client.GetHeaderForNoteIdAsync(new NoteId() { Id = NoteId }, myState.AuthHeader)).NoteFileId;
        }

    }

}
