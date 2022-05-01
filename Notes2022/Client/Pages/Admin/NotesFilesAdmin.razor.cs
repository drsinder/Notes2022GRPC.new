using Blazored.Modal;
using Blazored.Modal.Services;
//using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;
using Notes2022.Shared;
using System.Net.Http.Json;
using Notes2022.Proto;

namespace Notes2022.Client.Pages.Admin
{
    public partial class NotesFilesAdmin
    {
        private List<string> todo { get; set; }
        private GNotefileList files { get; set; }
        private string? message;
        private HomePageModel model { get; set; }
        private List<GAppUser> uList { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IModalService Modal { get; set; }
        public NotesFilesAdmin()
        {
        }

        public async Task Reload()
        {
            await GetStuff();
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetStuff();
        }

        protected async Task GetStuff()
        {
            model = await Client.GetAdminPageModelAsync(new NoRequest(), myState.AuthHeader);

            uList = GetApplicationUsers(model.UserDataList);

            todo = new List<string> { "announce", "pbnotes", "noteshelp", "pad", "homepagemessages" };

            foreach (GNotefile file in model.NoteFiles.Notefiles)
            {
                if (file.NoteFileName == "announce")
                    todo.Remove("announce");
                if (file.NoteFileName == "pbnotes")
                    todo.Remove("pbnotes");
                if (file.NoteFileName == "noteshelp")
                    todo.Remove("noteshelp");
                if (file.NoteFileName == "pad")
                    todo.Remove("pad");
                if (file.NoteFileName == "homepagemessages")
                    todo.Remove("homepagemessages");
            }
            files = model.NoteFiles;
        }

        private async Task CreateAnnounce()
        {
            await Client.CreateNoteFileAsync(new GNotefile() { NoteFileName = "announce", NoteFileTitle = "Notes 2022 Announcements" }, myState.AuthHeader);
            await Reload();
            Navigation.NavigateTo("admin/notefilelist");
        }

        private async Task CreatePbnotes()
        {
            await Client.CreateNoteFileAsync(new GNotefile() { NoteFileName = "pbnotes", NoteFileTitle = "Public Notes" }, myState.AuthHeader);
            await Reload();
            Navigation.NavigateTo("admin/notefilelist");
        }

        private async Task CreateNotesHelp()
        {
            await Client.CreateNoteFileAsync(new GNotefile() { NoteFileName = "noteshelp", NoteFileTitle = "Help with Notes 2022" }, myState.AuthHeader);
            await Reload();
            Navigation.NavigateTo("admin/notefilelist");
        }

        private async Task CreatePad()
        {
            await Client.CreateNoteFileAsync(new GNotefile() { NoteFileName = "pad", NoteFileTitle = "Traditional Pad" }, myState.AuthHeader);
            await Reload();
            Navigation.NavigateTo("admin/notefilelist");
        }

        private async Task CreateHomePageMessages()
        {
            await Client.CreateNoteFileAsync(new GNotefile() { NoteFileName = "homepagemessages", NoteFileTitle = "Home Page Messages" }, myState.AuthHeader);
            await Reload();
            Navigation.NavigateTo("admin/notefilelist");
        }

        async void CreateNoteFile(int Id)
        {
            StateHasChanged();
            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            var xModal = Modal.Show<Dialogs.CreateNoteFile>("Create Notefile", parameters);
            var result = await xModal.Result;
            await Reload();
            if (!result.Cancelled)
                Navigation.NavigateTo("admin/notefilelist");
        }


        private List<GAppUser> GetApplicationUsers(GAppUserList other)
        {
            List<GAppUser> list = new List<GAppUser>();
            foreach (GAppUser user in other.List)
            {
                list.Add(user);
            }
            return list;
        }


        async void DeleteNoteFile(int Id)
        {
            GNotefile file = files.Notefiles.ToList().Find(x => x.Id == Id);

            StateHasChanged();
            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            var xModal = Modal.Show<Dialogs.DeleteNoteFile>("Delete", parameters);
            var result = await xModal.Result;
            if (!result.Cancelled)
            {
                await Reload();
                Navigation.NavigateTo("admin/notefilelist");
            }

        }

        async void NoteFileDetails(int Id)
        {
            GNotefile file = files.Notefiles.ToList().Find(x => x.Id == Id);

            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            parameters.Add("LastEdited", file.LastEdited);
            parameters.Add("NumberArchives", file.NumberArchives);
            parameters.Add("Owner", model.UserDataList.List.ToList().Find(p => p.Id == file.OwnerId).DisplayName);
            var xModal = Modal.Show<Dialogs.NoteFileDetails>("Details", parameters);
            await xModal.Result;
        }

        async void EditNoteFile(int Id)
        {
            GNotefile file = files.Notefiles.ToList().Find(x => x.Id == Id);

            var parameters = new ModalParameters();
            parameters.Add("FileId", Id);
            parameters.Add("FileName", file.NoteFileName);
            parameters.Add("FileTitle", file.NoteFileTitle);
            parameters.Add("LastEdited", file.LastEdited);
            parameters.Add("NumberArchives", file.NumberArchives);
            parameters.Add("Owner", file.OwnerId);
            var xModal = Modal.Show<Dialogs.EditNoteFile>("Edit Notefile", parameters);
            var result = await xModal.Result;
            if (!result.Cancelled)
            {
                await Reload();
                Navigation.NavigateTo("admin/notefilelist");
            }
        }

        async Task ImportNoteFile(int Id)
        {
            var parameters = new ModalParameters();
            var xModal = Modal.Show<Dialogs.Upload1>("Upload1", parameters);
            var result = await xModal.Result;
            if (result.Cancelled)
            {
                return;
            }
            else
            {
                GNotefile file = files.Notefiles.ToList().Find(x => x.Id == Id);

                string filename = (string)result.Data;
                parameters = new ModalParameters();
                parameters.Add("UploadFile", filename);
                parameters.Add("NoteFile", file.NoteFileName);

                var yModal = Modal.Show<Dialogs.Upload2>("Upload2", parameters);
                await yModal.Result;

                Navigation.NavigateTo("noteindex/" + Id);
                return;
            }
        }

    }
}
