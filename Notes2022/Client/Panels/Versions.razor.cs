/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Versions.razor.cs
    **
    ** Description:
    **      Displays Versions content
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

using Microsoft.AspNetCore.Components;
using Notes2022.Proto;
using Notes2022.Shared;
using System.Net.Http.Json;

namespace Notes2022.Client.Panels
{
    /// <summary>
    /// Displays versions for edited notes
    /// </summary>
    public partial class Versions
    {
        /// <summary>
        /// These four parameters identify the note
        /// </summary>
        [Parameter] public int FileId { get; set; }
        [Parameter] public int NoteOrdinal { get; set; }
        [Parameter] public int ResponseOrdinal { get; set; }
        [Parameter] public int ArcId { get; set; }

        protected GNoteHeaderList Headers { get; set; }

        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public Versions()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            Headers = await Client.GetVersionsAsync(new GetVersionsRequest() 
                { FileId=FileId, NoteOrdinal=NoteOrdinal, ResponseOrdinal=ResponseOrdinal, ArcId=ArcId },
                myState.AuthHeader);
        }
    }
}
