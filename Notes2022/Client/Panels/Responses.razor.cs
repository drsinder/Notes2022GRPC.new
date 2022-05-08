/*--------------------------------------------------------------------------
    **
    ** Copyright © 2022, Dale Sinder
    **
    ** Name: Responses.razor.cs
    **
    ** Description:
    **      Displays response headers (and content) inside the NoteIndex
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
using Notes2022.Client.Pages;
using Notes2022.Proto;
using Syncfusion.Blazor.Grids;
using System.Timers;

namespace Notes2022.Client.Panels
{
    /// <summary>
    /// Shows note responses
    /// </summary>
    public partial class Responses
    {
        /// <summary>
        /// List of response headers
        /// </summary>
        [Parameter] public List<GNoteHeader> Headers { get; set; }

        /// <summary>
        /// Show content for responses
        /// </summary>
        [Parameter] public bool ShowContentR { get; set; }

        /// <summary>
        /// Expand all rows
        /// </summary>
        [Parameter] public bool ExpandAllR { get; set; }

        //[Parameter] public NoteIndex Parent {get; set;}

        //public bool ShowContent { get; set; }
        //public bool ExpandAll { get; set; }


        protected SfGrid<GNoteHeader> sfGrid2 { get; set; }

        [Inject] NavigationManager Navigation { get; set; }
        public Responses()
        {
        }

        /// <summary>
        /// Copy parameter to local copy
        /// </summary>
        /// <returns></returns>
        //protected override async Task OnParametersSetAsync()
        //{
        //}

        public void DataBoundHandler()
        {
            // Expand if appropriate
            if (ExpandAllR)
            {
                sfGrid2.ExpandAllDetailRowAsync().GetAwaiter();
            }
        }

        //System.Timers.Timer timer2;

        //protected void ActionCompleteHandler(ActionEventArgs<GNoteHeader> action)
        //{
        //    if (action.RequestType == Syncfusion.Blazor.Grids.Action.Filtering)
        //    {
        //        timer2 = new System.Timers.Timer(1000);
        //        timer2.Elapsed += TimerTick2;
        //        timer2.Enabled = true;
        //    }
        //}

        //protected void TimerTick2(Object source, ElapsedEventArgs e)
        //{
        //    timer2.Elapsed -= TimerTick2;
        //    timer2.Stop();
        //    timer2.Enabled = false;

        //    this.StateHasChanged();
        //}

        /// <summary>
        /// GO show the note
        /// </summary>
        /// <param name="args"></param>
        protected void DisplayIt(RowSelectEventArgs<GNoteHeader> args)
        {
            Navigation.NavigateTo("notedisplay/" + args.Data.Id);
        }
    }
}
