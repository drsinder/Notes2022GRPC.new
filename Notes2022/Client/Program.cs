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
using Notes2022.Client.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

string licenseKey = builder.Configuration["SyncfusionKey"];

SyncfusionLicenseProvider.RegisterLicense(licenseKey);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddSingleton<MainLayout>();

builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

HttpClient? httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));

//var handler = new SubdirectoryHandler(new HttpClientHandler(), "/Notes2022GRCP");

builder.Services.AddSingleton(services =>
{
	var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
	var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
	return new Notes2022Server.Notes2022ServerClient(channel);
});


await builder.Build().RunAsync();



/// <summary>
/// A delegating handler that adds a subdirectory to the URI of gRPC requests.
/// </summary>
public class SubdirectoryHandler : DelegatingHandler
{
    private readonly string _subdirectory;

    public SubdirectoryHandler(HttpMessageHandler innerHandler, string subdirectory)
        : base(innerHandler)
    {
        _subdirectory = subdirectory;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var old = request.RequestUri;

        var url = $"{old.Scheme}://{old.Host}:{old.Port}";
        url += $"{_subdirectory}{request.RequestUri.AbsolutePath}";
        request.RequestUri = new Uri(url, UriKind.Absolute);

        var x = base.SendAsync(request, cancellationToken);

        return x;
    }
}