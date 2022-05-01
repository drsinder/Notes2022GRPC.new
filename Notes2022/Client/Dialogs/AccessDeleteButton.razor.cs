using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Notes2022.Shared;

namespace Notes2022.Client.Dialogs
{
    public partial class AccessDeleteButton
    {
        [Parameter]
        public GNoteAccess noteAccess { get; set; }

        [Parameter]
        public EventCallback<string> OnClick { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public AccessDeleteButton() { }

        protected async Task Delete()
        {
            await Client.DeleteAccessItemAsync(noteAccess, myState.AuthHeader);
            await OnClick.InvokeAsync("Delete");
        }
    }
}