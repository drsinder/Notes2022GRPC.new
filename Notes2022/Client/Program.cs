using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using Blazored.Modal;
using Blazored.SessionStorage;
using Notes2022.Client;
using Notes2022.Proto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

string licenseKey = "NjIxNzc4QDMyMzAyZTMxMmUzMEY5eUlKTXBBSFFKNzdBSCsyNE12eTBGM2dwUkRnbUJmbjEraEwraTJqN2s9";
SyncfusionLicenseProvider.RegisterLicense(licenseKey);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddSingleton<StateContainer>();

builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

HttpClient? httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

builder.Services.AddSingleton(services =>
{
	var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
	var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
	return new Notes2022Server.Notes2022ServerClient(channel);
});


await builder.Build().RunAsync();
