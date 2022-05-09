/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Tracker.razor.cs
    **
    ** Description:
    **      Sequencer / Tracker editor
    **
    ** This program is free software: you can redistribute it and/or modify
    ** it under the terms of the GNU General Public License version 3 as
    ** published by the Free Software Foundation.   
    **
    ** This program is distributed in the hope that it will be useful,
    ** but WITHOUT ANY WARRANTY; without even the implied warranty of
    ** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    ** GNU General Public License version 3 for more details.
    **
    **  You should have received a copy of the GNU General Public License
    **  version 3 along with this program in file "license-gpl-3.0.txt".
    **  If not, see <http://www.gnu.org/licenses/gpl-3.0.txt>.
    **
    **--------------------------------------------------------------------------*/



using Notes2022.Proto;
using Notes2022.Shared;

namespace Notes2022.Client.Pages
{
    public partial class Tracker
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private List<GNotefile> stuff { get; set; }

        private List<GNotefile> files { get; set; }

        private List<GSequencer> trackers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnParametersSetAsync()
        {
            trackers = (await Client.GetSequencerAsync(new NoRequest(), myState.AuthHeader)).List.ToList();
            HomePageModel model = await Client.GetHomePageModelAsync(new NoRequest(), myState.AuthHeader);

            stuff = model.NoteFiles.Notefiles.OrderBy(p => p.NoteFileName).ToList();
            await Shuffle();
        }

        public async Task Shuffle()
        {
            files = new List<GNotefile>();

            trackers = (await Client.GetSequencerAsync(new NoRequest(), myState.AuthHeader)).List.ToList();
            if (trackers is not null)
            {
                trackers = trackers.OrderBy(p => p.Ordinal).ToList();
                foreach (var tracker in trackers)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    files.Add(stuff.Find(p => p.Id == tracker.NoteFileId));
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
            foreach (var s in stuff)
            {
                if (files.Find(p => p.Id == s.Id) is null)
                    files.Add(s);
            }
            StateHasChanged();
        }

        private void Cancel()
        {
            NavMan.NavigateTo("");
        }
    }
}