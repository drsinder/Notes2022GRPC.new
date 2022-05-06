using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using System.ComponentModel.DataAnnotations;

namespace Notes2022.Client.Pages.Authentication
{
    public partial class Login
    {
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            //[EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public int RememberHours { get; set; }
        }

        [Parameter]
        public string returnURL { get; set; }


        public string myCookieValue { get; set; } = "";

        protected InputModel Input = new InputModel { Email = string.Empty, Password = string.Empty };

        protected string Message = string.Empty;

        private async Task GotoLogin()
        {
            string retUrl = Globals.returnUrl;
            Globals.returnUrl = string.Empty;
            LoginRequest req = new LoginRequest()
            { Email = Input.Email, Password = Input.Password, Hours = Input.RememberHours };
            LoginReply ar = await AuthClient.LoginAsync(req);
            if (ar.Status == 200)
            {
                ar.Hours = Input.RememberHours;
                myState.LoginReply = ar;
            }
            else
            {
                Message = ar.Message;
                return;
            }

            //await AuthClient.SendEmailAsync(new Email() { Address = Input.Email, Subject = "Login", Body = "Notes 2022 Login" });

            Globals.LoginDisplay?.Reload();
            Globals.NavMenu?.Reload();

            Navigation.NavigateTo(retUrl);
        }
    }
}