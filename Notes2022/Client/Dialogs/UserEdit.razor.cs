using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;

namespace Notes2022.Client.Dialogs
{
    public partial class UserEdit
    {
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }

        [Parameter] public string UserId { get; set; }

        protected EditUserViewModel Model { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }

        public UserEdit()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            Model = await Client.GetUserRolesAsync(new AppUserRequest() { Subject = UserId }, myState.AuthHeader);
        }

        private async Task Submit()
        {
            await Client.UpdateUserRolesAsync(Model, myState.AuthHeader);
            await ModalInstance.CancelAsync();
        }


        private async Task Done()
        {
            await ModalInstance.CancelAsync();
        }


    }
}