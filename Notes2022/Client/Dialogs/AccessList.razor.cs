using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Syncfusion.Blazor.Grids;

namespace Notes2022.Client.Dialogs
{
    /// <summary>
    /// Access editor for a files access tokens
    /// </summary>
    public partial class AccessList
    {
        [CascadingParameter] public IModalService Modal { get; set; }
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        
        /// <summary>
        /// File Id we are working on
        /// </summary>
        [Parameter] public int fileId { get; set; }

        /// <summary>
        /// Grid of tokens
        /// </summary>
        private SfGrid<GNoteAccess> MyGrid;

        /// <summary>
        /// List of tokens
        /// </summary>
        private List<GNoteAccess> myList { get; set; }

        /// <summary>
        /// Temp list of tokens
        /// </summary>
        private List<GNoteAccess> temp { get; set; }

        /// <summary>
        /// List of all users
        /// </summary>
        private List<GAppUser> userList { get; set; }

        /// <summary>
        /// My access
        /// </summary>
        private GNoteAccess myAccess { get; set; }
        private int arcId { get; set; }

        /// <summary>
        /// message to display
        /// </summary>
        private string message { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        public AccessList()
        {
        }

        protected async override Task OnParametersSetAsync()
        {
            arcId = await sessionStorage.GetItemAsync<int>("ArcId");

            AccessAndUserList myLists = await Client.GetAccessAndUserListAsync(new AccessAndUserListRequest() { FileId = fileId, ArcId = arcId, UserId = myState.UserInfo?.Subject }, myState.AuthHeader);

            myList = myLists.AccessList.List.ToList();
            userList = myLists.AppUsers.List.ToList();
            myAccess = myLists.UserAccess;
        }

        /// <summary>
        /// We are done
        /// </summary>
        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }

        /// <summary>
        /// Add a new token for another user
        /// </summary>
        protected async void CreateNew()
        {
            var parameters = new ModalParameters();
            parameters.Add("userList", userList);
            parameters.Add("NoteFileId", fileId);

            var xx = Modal.Show<AddAccessDlg>("", parameters);
            await xx.Result;

            StateHasChanged();
            await MyGrid.Refresh();
        }

        /// <summary>
        /// Item deleted - refresh list
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        protected async Task ClickHandler(string newMessage)
        {
            arcId = await sessionStorage.GetItemAsync<int>("ArcId");

            GNoteAccessList myLists = await Client.GetAccessListAsync(new AccessAndUserListRequest() { FileId = fileId, ArcId = arcId, UserId = myState.UserInfo?.Subject }, myState.AuthHeader);
            myList = myLists.List.ToList();
            StateHasChanged();
            await MyGrid.Refresh();
        }

    }
}
