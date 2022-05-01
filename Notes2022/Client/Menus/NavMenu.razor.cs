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

using Notes2022.Shared;
using Notes2022.Proto;
using System.Timers;
using Notes2022.Client.Dialogs;

namespace Notes2022.Client.Menus
{
    public partial class NavMenu
    {
        /// <summary>
        /// For display of error message during initialization
        /// </summary>
        [CascadingParameter]
        public IModalService Modal { get; set; }

        /// <summary>
        /// The list of menu bar items (structure of the menu)
        /// </summary>
        protected static List<MenuItem> menuItemsTop { get; set; }

        /// <summary>
        /// Root menu item
        /// </summary>
        protected SfMenu<MenuItem> topMenu { get; set; }

        /// <summary>
        /// Current time
        /// </summary>
        private string mytime { get; set; }

        /// <summary>
        /// Used to compare time and abort re-render in same minute
        /// </summary>
        private string mytime2 { get; set; } = "";
        /// <summary>
        /// Used to update menu bar time - tick once per second
        /// </summary>
        private System.Timers.Timer timer2 { get; set; }

        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        [Inject] NavigationManager Navigation { get; set; }
        [Inject] Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }
        [Inject] Notes2022Server.Notes2022ServerClient Client { get; set; }
        public NavMenu()
        {
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        /// <summary>
        /// Update the clock once per second
        /// </summary>
        /// <param name="firstRender"></param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer2 = new System.Timers.Timer(1000);
                timer2.Elapsed += TimerTick2;
                timer2.Enabled = true;

                myState.OnChange += StateHasChanged;
            }
        }

        /// <summary>
        /// Invoked once per second
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void TimerTick2(Object source, ElapsedEventArgs e)
        {
            mytime = DateTime.Now.ToShortTimeString();
            //if (mytime != mytime2) // do we need to re-render?
            {
                StateHasChanged();
                mytime2 = mytime;
            }
        }


        /// <summary>
        /// Invoked when an Item is selected
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public async Task OnSelect(MenuEventArgs<MenuItem> e)
        {
            await ExecMenu(e.Item.Id);
        }


        /// <summary>
        /// This could potentially be called from other places...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task ExecMenu(string id)
        {
            switch (id)
            {
                case "MainHelp":
                    Navigation.NavigateTo("help");
                    break;
                case "About":
                    Navigation.NavigateTo("about");
                    break;
                case "License":
                    Navigation.NavigateTo("license");
                    break;

                case "Subscriptions":
                    Navigation.NavigateTo("subscribe");
                    break;

                case "MRecent":
                    Navigation.NavigateTo("tracker");
                    break;

                case "Recent":
                    await StartSeq();
                    break;

                case "NoteFiles":
                    Navigation.NavigateTo("admin/notefilelist");
                    break;

                case "Preferences":
                    Navigation.NavigateTo("preferences");
                    break;

                //case "Hangfire":
                //    Navigation.NavigateTo(Globals.EditUserVModel.HangfireLoc, true);
                //    break;

                case "Roles":
                    Navigation.NavigateTo("admin/editroles");
                    break;

                case "Linked":
                    Navigation.NavigateTo("admin/linkindex");
                    break;
            }
        }

        public async Task Reload()
        {
            StateHasChanged();
            await UpdateMenu();
        }

        protected override async Task OnParametersSetAsync()
        {
            Globals.NavMenu = this;
            await UpdateMenu();
        }

        /// <summary>
        /// Enable only items available to logged in user
        /// </summary>
        /// <returns></returns>
        public async Task UpdateMenu()
        {

            bool isAdmin = false;
            bool isUser = false;

            if (myState.IsAuthenticated)
            {
                isAdmin = myState.IsAdmin;
                isUser = myState.IsUser;
            }

            // make the whole menu
            menuItemsTop = new List<MenuItem>();
            MenuItem item;

            item = new() { Id = "Recent", Text = "Recent Notes" };
            menuItemsTop.Add(item);

            MenuItem item3 = new() { Id = "Manage", Text = "Manage" };
            item3.Items = new List<MenuItem>
            {
                new () { Id = "MRecent", Text = "Recent" },
                new () { Id = "Subscriptions", Text = "Subscriptions" },
                new () { Id = "Preferences", Text = "Preferences" }
            };
            menuItemsTop.Add(item3);

            item = new() { Id = "Help", Text = "Help" };
            item.Items = new List<MenuItem>
            {
                new () { Id = "MainHelp", Text = "Help" },
                new () { Id = "About", Text = "About" },
                new () { Id = "License", Text = "License" }
            };
            menuItemsTop.Add(item);

            item = new MenuItem() { Id = "Admin", Text = "Admin" };
            item.Items = new List<MenuItem>
            {
                new () { Id = "NoteFiles", Text = "NoteFiles" },
                new () { Id = "Roles", Text = "Roles" },
                new () { Id = "Linked", Text = "Linked" }
                //new () { Id = "Hangfire", Text = "Hangfire" }
            };

            menuItemsTop.Add(item);

            // remove what does not apply to this user
            if (!isAdmin)
            {
                menuItemsTop.RemoveAt(3);
            }
            if (isUser || isAdmin)
            {
            }
            else
            {
                menuItemsTop.RemoveAt(1);
                menuItemsTop.RemoveAt(0);
            }
        }

        /// <summary>
        /// Recent menu item - start sequencing
        /// </summary>
        /// <returns></returns>
        private async Task StartSeq()
        {
            // get users list of files
            //List<GSequencer> sequencers = await DAL.GetSequencer(Http);

            List<GSequencer> sequencers = (await Client.GetSequencerAsync(new NoRequest(), myState.AuthHeader)).List.ToList();
            if (sequencers.Count == 0)
                return;

            // order them as prefered by user
            sequencers = sequencers.OrderBy(p => p.Ordinal).ToList();

            // set up state for sequencing
            await sessionStorage.SetItemAsync<List<GSequencer>>("SeqList", sequencers);
            await sessionStorage.SetItemAsync<int>("SeqIndex", 0);
            await sessionStorage.SetItemAsync<GSequencer>("SeqItem", sequencers[0]);
            await sessionStorage.SetItemAsync<bool>("IsSeq", true); // flag for noteindex
            // begin
            Navigation.NavigateTo("noteindex/" + sequencers[0].NoteFileId);
        }

        /// <summary>
        /// Show error message
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            var parameters = new ModalParameters();
            parameters.Add("MessageInput", message);
            Modal.Show<MessageBox>("Error", parameters);
        }


    }
}