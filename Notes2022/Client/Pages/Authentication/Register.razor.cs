using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Notes2022.Client;
using Notes2022.Client.Shared;
using Blazored;
using Blazored.Modal;
using Blazored.Modal.Services;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.LinearGauge;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.SplitButtons;
using Syncfusion.Blazor.Calendars;
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