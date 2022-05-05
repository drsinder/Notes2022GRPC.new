using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Notes2022.Client.Dialogs;
using Notes2022.Proto;


namespace Notes2022.Client.Pages.Admin
{
    public partial class UserList
    {
        [CascadingParameter] public IModalService Modal { get; set; }

        private GAppUserList UList { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public UserList()
        { }

        protected override async Task OnParametersSetAsync()
        {
            UList = await Client.GetUserListAsync(new NoRequest(), myState.AuthHeader);
        }

        protected void EditLink(string Id)
        {
            ModalParameters Parameters = new ModalParameters();
            Parameters.Add("UserId", Id);

            Modal.Show<UserEdit>("", Parameters);
        }
    }
}
