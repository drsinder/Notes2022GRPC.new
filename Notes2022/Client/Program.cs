// ***********************************************************************
// Assembly         : Notes2022.Client
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-09-2022
// ***********************************************************************
// <copyright file="Program.cs" company="Notes2022.Client">
//     Copyright (c) Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Blazored.Modal;
using Blazored.SessionStorage;
using Notes2022.Client;
using Notes2022.Proto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }); // not needed when using grpc and no controllers at server.

builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddSingleton<App>();   // for login state mgt = "myState" injection in _imports.razor

builder.Services.AddSyncfusionBlazor();   // options => { options.IgnoreScriptIsolation = true; });

//var handler = new SubdirectoryHandler(new HttpClientHandler(), "/Notes2022GRCP");

// Add my gRPC service so it can be injected.
builder.Services.AddSingleton(services =>
{
	HttpClient? httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
	var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
	var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
	return new Notes2022Server.Notes2022ServerClient(channel);
});

await builder.Build().RunAsync();



///// <summary>
///// A delegating handler that adds a subdirectory to the URI of gRPC requests.
///// </summary>
//public class SubdirectoryHandler : DelegatingHandler
//{
//    private readonly string _subdirectory;

//    public SubdirectoryHandler(HttpMessageHandler innerHandler, string subdirectory)
//        : base(innerHandler)
//    {
//        _subdirectory = subdirectory;
//    }

//    protected override Task<HttpResponseMessage> SendAsync(
//        HttpRequestMessage request, CancellationToken cancellationToken)
//    {
//        var old = request.RequestUri;

//        var url = $"{old.Scheme}://{old.Host}:{old.Port}";
//        url += $"{_subdirectory}{request.RequestUri.AbsolutePath}";
//        request.RequestUri = new Uri(url, UriKind.Absolute);

//        var x = base.SendAsync(request, cancellationToken);

//        return x;
//    }
//}