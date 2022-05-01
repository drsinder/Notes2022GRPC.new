using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.Client.Pages
{
    /// <summary>
    /// Setup for calling note editor panel to edit an existing note
    /// </summary>
    public partial class EditNote
    {
        [Parameter] public long NoteId { get; set; }   //  what we are editing

        /// <summary>
        /// our data for the note in edit model
        /// </summary>
        protected TextViewModel Model { get; set; } = new TextViewModel();

        /// <summary>
        /// A note display model
        /// </summary>
        protected DisplayModel stuff { get; set; }

        protected bool go = false;

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public EditNote()
        {
        }

        // get all the data
        protected override async Task OnParametersSetAsync()
        {
            stuff = await Client.GetNoteContentAsync(new DisplayModelRequest() { NoteId = NoteId, Vers = 0 }, myState.AuthHeader);

            Model.NoteFileID = stuff.NoteFile.Id;
            Model.NoteID = NoteId;
            Model.BaseNoteHeaderID = stuff.Header.BaseNoteId;
            Model.RefId = stuff.Header.RefId;
            Model.MyNote = stuff.Content.NoteBody;
            Model.MySubject = stuff.Header.NoteSubject;
            Model.DirectorMessage = stuff.Header.DirectorMessage;

            string tags = "";
            foreach (var tag in stuff.Tags.List)
            {
                tags += tag + " ";
            }
            Model.TagLine = tags;
            go = true;
        }
    }
}
