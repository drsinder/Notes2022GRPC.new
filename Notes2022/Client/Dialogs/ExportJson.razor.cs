﻿// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 05-03-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-08-2022
// ***********************************************************************
// <copyright file="ExportJson.razor.cs" company="Notes2022.Client">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Notes2022.Proto;
using System.Text;

namespace Notes2022.Client.Dialogs
{
    /// <summary>
    /// Class ExportJson.
    /// Implements the <see cref="ComponentBase" />
    /// Implements the <see cref="System.IAsyncDisposable" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    /// <seealso cref="System.IAsyncDisposable" />
    public partial class ExportJson
    {
        /// <summary>
        /// Gets or sets the modal instance.
        /// </summary>
        /// <value>The modal instance.</value>
        [CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        [Parameter] public ExportViewModel model { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        private string FileName { get; set; }

        /// <summary>
        /// The module
        /// </summary>
        private IJSObjectReference? module;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExportJson"/> is marked.
        /// </summary>
        /// <value><c>true</c> if marked; otherwise, <c>false</c>.</value>
        private bool marked { get; set; }
        /// <summary>
        /// The message
        /// </summary>
        private string message = "Getting ready...";

        /// <summary>
        /// On initialized as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected async override Task OnInitializedAsync()
        {
            FileName = model.NoteFile.NoteFileName + ".json";

            if (model.Marks is not null && model.Marks.Count > 0)
                marked = true;
            else
                marked = false;

            message = "Exporting " + FileName;

            MemoryStream ms2 = await DoExport();

            await SaveAs(FileName, ms2.GetBuffer());
            ms2.Dispose();

            await ModalInstance.CancelAsync();
        }

        /// <summary>
        /// Does the export.
        /// </summary>
        /// <returns>MemoryStream.</returns>
        private async Task<MemoryStream> DoExport()
        {
            GNotefile nf = model.NoteFile;
            int nfid = nf.Id;
            StringContent stringContent;

            JsonExport stuff;
            stuff = await Client.GetExportJsonAsync(new ExportRequest() { FileId = nfid, ArcId = 0 }, myState.AuthHeader);
            stringContent = new StringContent(JsonConvert.SerializeObject(stuff, Newtonsoft.Json.Formatting.Indented), Encoding.UTF8, "application/json");

            Stream ms0 = await stringContent.ReadAsStreamAsync();
            MemoryStream ms = new MemoryStream();
            await ms0.CopyToAsync(ms);
            ms0.Dispose();
            ms.Close();
            return ms;
        }

        /// <summary>
        /// On after render as an asynchronous operation.
        /// </summary>
        /// <param name="firstRender">Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
        /// on this component instance; otherwise <c>false</c>.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <remarks>The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
        /// once.</remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./scripts.js");
            }
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }

        /// <summary>
        /// Saves as.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="data">The data.</param>
        public async Task SaveAs(string filename, byte[] data)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            await module.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
#pragma warning restore CS8604 // Possible null reference argument.
        }


    }
}
