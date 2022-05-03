using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Notes2022.Proto;

namespace Notes2022.Client.Dialogs
{
    public partial class Forward
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public ForwardViewModel ForwardView { get; set; }

        private async Task Forwardit()
        {
            if (ForwardView.ToEmail is null || ForwardView.ToEmail.Length < 8 || !ForwardView.ToEmail.Contains("@") || !ForwardView.ToEmail.Contains("."))
                return;
            await Client.DoForwardAsync(ForwardView, myState.AuthHeader);
            await ModalInstance.CancelAsync();
        }

        private void Cancel()
        {
            ModalInstance.CancelAsync();
        }
    }
}