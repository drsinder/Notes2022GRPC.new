using System.ComponentModel.DataAnnotations;
using Notes2022.Proto;

namespace Notes2022.Client.Pages.Authentication
{
    public partial class Register
    {
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password2 { get; set; }
        }

        protected InputModel Input = new InputModel{Email = string.Empty, UserName = string.Empty, Password = string.Empty, Password2 = string.Empty};
        protected string Message = string.Empty;

        private async Task GotoRegister()
        {
            if (Input.Password != Input.Password2)
            {
                Message = "Passwords do not match!";
                return;
            }

            RegisterRequest regreq = new RegisterRequest() { Email = Input.Email, Password = Input.Password, Username = Input.UserName };

            AuthReply ar = await AuthClient.RegisterAsync(regreq);

            if (ar.Status != 200)
            { 
                Message = ar.Message;
                return;
            }

            Globals.LoginDisplay.Reload();
            Navigation.NavigateTo("");
        }

    }
}