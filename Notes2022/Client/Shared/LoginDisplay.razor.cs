using Microsoft.AspNetCore.Components;

namespace Notes2022.Client.Shared
{
    public partial class LoginDisplay
    {
        private void BeginSignOut()
        {
            Navigation.NavigateTo("authentication/logout");
        }

        private void GotoProfile()
        {
            //Navigation.NavigateTo("authentication/profile");
        }

        private void GotoRegister()
        {
            Navigation.NavigateTo("authentication/register");
        }

        private void GotoLogin()
        {
            Navigation.NavigateTo("authentication/login");
        }

        private void GotoHome()
        {
            Navigation.NavigateTo("");
        }

        protected override void OnInitialized()
        {
            Globals.LoginDisplay = this;
        }

        public void Reload()
        {
            StateHasChanged();
        }
    }
}