using Microsoft.AspNetCore.Components;
using Notes2022.Proto;


namespace Notes2022.Client.Comp
{
    public partial class FileButton
    {
        [Parameter] public GNotefile NoteFile { get; set; }

        [Inject] NavigationManager Navigation { get; set; }
        public FileButton()
        {
        }

        protected void OnClick()
        {
            Navigation.NavigateTo("noteindex/" + NoteFile.Id);
        }

    }
}
