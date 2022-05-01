using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Notes2022.Proto;
using Notes2022.Shared;
using Notes2022.Server.Data;
using Notes2022.Server.Entities;
using System.Security.Claims;

namespace Notes2022.Server.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly ILogger<WeatherService> _logger;
        //private readonly NotesDbContext _data;
        private readonly UserManager<ApplicationUser> _userManager;

        public WeatherService(ILogger<WeatherService> logger, /*NotesDbContext data,*/ UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            //_data = data;
            _userManager = userManager;
        }

        [Authorize]
        public override async Task<WeatherReply> GetWeatherForecast(WeatherRequest request, ServerCallContext context)
        {
            //var user = context.GetHttpContext().User;
            //ApplicationUser applicationUser = await _userManager.FindByIdAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            //string name = user.Identity.Name;

            //bool auth = user.Identity.IsAuthenticated;
            //bool admin = user.IsInRole(UserRoles.Admin);


            List<WeatherForecast> list = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();

            WeatherReply reply = new WeatherReply();
            reply.Forecast.Add(list);

            return (reply);
        }

    }
}
