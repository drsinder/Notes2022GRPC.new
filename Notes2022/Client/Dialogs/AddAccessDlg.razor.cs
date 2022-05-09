using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Notes2022.Shared;
using System.Net.Http.Json;
using System.Timers;


namespace Notes2022.Client.Dialogs
{
    /// <summary>
    /// Add a user to the access list
    /// </summary>
    public partial class AddAccessDlg
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        [Parameter] public List<GAppUser> userList { get; set; }
        [Parameter] public int NoteFileId { get; set; }

        protected string selectedUserId { get; set; }

        protected System.Timers.Timer delay { get; set; }

        protected string ArcString { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public AddAccessDlg()
        {
        }

        protected override void OnParametersSet()
        {
            selectedUserId = "none";
            //if (NoteFile is null)
            //    Cancel();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        private async Task Create()
        {
            if (selectedUserId != "none")
            {
                int aId = await sessionStorage.GetItemAsync<int>("ArcId");

                GNoteAccess item = new();

                item.UserID = selectedUserId;
                item.NoteFileId = NoteFileId;
                item.ArchiveId = aId;
                // all access options left false

                _ = await Client.AddAccessItemAsync(item, myState.AuthHeader);

                delay = new(250);       // allow time for server to respond
                delay.Enabled = true;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                delay.Elapsed += Done;  // then close the dialog automatically
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                return;
            }

            await ModalInstance.CancelAsync();
        }

        public void Done(Object source, ElapsedEventArgs e)
        {
            delay.Enabled = false;
            delay.Stop();
            ModalInstance.CancelAsync();
        }


    }
}
