using Microsoft.AspNetCore.Components;
using Notes2022.Proto;

namespace Notes2022.Client.Pages
{
    public partial class ShowNote
    {
        [Parameter] public long NoteId { get; set; }

        public int FileId { get; set; }

        [Inject] NavigationManager Navigation { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public ShowNote()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            bool x = myState.IsAuthenticated;
            if (!x)
            {
                await myState.GetLoginReplyAsync();
                if (!myState.IsAuthenticated)
                {
                    Globals.returnUrl = Navigation.Uri;
                    Navigation.NavigateTo("authentication/login");
                }
            }
            // find the file id for this note - get note header
            FileId = (await Client.GetHeaderForNoteIdAsync(new NoteId() { Id = NoteId }, myState.AuthHeader)).NoteFileId;

            Globals.GotoNote = NoteId;
            Navigation.NavigateTo("noteindex/" + FileId);
        }

    }

}
