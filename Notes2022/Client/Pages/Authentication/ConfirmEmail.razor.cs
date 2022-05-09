using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using System.Text.Json;

namespace Notes2022.Client.Pages.Authentication
{
    public partial class ConfirmEmail
    {
        [Parameter]
        public string? payload { get; set; }

        private string? Message { get; set; }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
            ConfirmEmailRequest stuff = JsonSerializer.Deserialize<ConfirmEmailRequest>(Globals.Base64Decode(payload));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

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
