// ***********************************************************************
// Assembly         : Notes2022.Server
// Author           : Dale Sinder
// Created          : 04-19-2022
//
// Last Modified By : Dale Sinder
// Last Modified On : 05-05-2022
// ***********************************************************************
// <copyright file="Program.cs" company="Notes2022.Server">
//     Copyright (c) 2022 Dale Sinder. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Notes2022.Server.Data;
using Notes2022.Server.Services;
using Notes2022.Server.Entities;
using Notes2022.Server;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlServer(connectionString));

// For Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NotesDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = false,
         ValidAudience = configuration["JWTAuth:ValidAudienceURL"],
         ValidIssuer = configuration["JWTAuth:ValidIssuerURL"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTAuth:SecretKey"]))
     };
 });

// Add services to the container.

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddGrpc();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

//builder.Services.AddControllers();
//builder.Services.AddRazorPages();


Globals.SendGridApiKey = builder.Configuration["SendGridApiKey"];
Globals.SendGridEmail = builder.Configuration["SendGridEmail"];
Globals.SendGridName = builder.Configuration["SendGridName"];
Globals.ImportRoot = builder.Configuration["ImportRoot"];
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();  // ?? maybe needed?

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors();

//app.MapRazorPages();
//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<Notes2022Service>().EnableGrpcWeb().RequireCors("AllowAll");
});

app.MapFallbackToFile("index.html");

app.Run();


////////////////////////////////////////////////////////////////////////////////////////////////

//public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
//{
//    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

//    public async Task HandleAsync(
//        RequestDelegate next,
//        HttpContext context,
//        AuthorizationPolicy policy,
//        PolicyAuthorizationResult authorizeResult)
//    {
//        // If the authorization was forbidden and the resource had a specific requirement,
//        // provide a custom 404 response.
//        if (!(authorizeResult.Succeeded))
//        {
//            // Return a 404 to make it appear as if the resource doesn't exist.
//            context.Response.StatusCode = StatusCodes.Status404NotFound;

//            return;
//        }

//        // Fall back to the default implementation.
//        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
//    }
//}

//public class Show404Requirement : IAuthorizationRequirement { }
