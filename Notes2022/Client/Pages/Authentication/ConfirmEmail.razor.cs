using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using System.Text.Json;

namespace Notes2022.Client.Pages.Authentication
{
    public partial class ConfirmEmail
    {
        [Parameter]
        public string payload { get; set; }

        private string Message { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            ConfirmEmailContainer stuff = JsonSerializer.Deserialize<ConfirmEmailContainer>(Globals.Base64Decode(payload));

            AuthReply reply = await AuthClient.ConfirmEmailAsync(stuff);

            if (reply != null)
            {
                Message = reply.Message;
            }
            else
            {
                Message = "Confirming email call failed!";
            }
        }
    }
}