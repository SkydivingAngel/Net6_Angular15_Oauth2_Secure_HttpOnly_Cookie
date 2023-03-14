using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net6AngularOauth2.Models;
using System.Collections.Generic;

namespace Net6AngularOauth2.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly List<WeatherForecast> Repo = Enumerable.Range(1, 171).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToList();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        ////[Authorize]
        //[HttpGet, Route("weatherforecast")]
        //public IEnumerable<WeatherForecast> GetWeatherForecast(
        //    int pageIndex = 0,
        //    int pageSize = 10)
        //{
        //    string token = Guid.NewGuid().ToString("N");

        //    //return Enumerable.Range(1, 151).Select(index => new WeatherForecast
        //    //{
        //    //    Date = DateTime.Now.AddDays(index),
        //    //    TemperatureC = Random.Shared.Next(-20, 55),
        //    //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    //})
        //    //.ToArray();

        //    System.IO.File.AppendAllText(@"C:\Temp\Pagination.log", pageIndex + " - " + pageSize + Environment.NewLine);
        //    return repo; //.Skip(pageIndex).Take(pageSize);

        //}


        [Authorize]
        [HttpGet, Route("weatherforecast")]
        public Result GetWeatherForecast(
            int pageIndex,
            int pageSize)
        {
            string token = Guid.NewGuid().ToString("N");

            //return Enumerable.Range(1, 151).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            System.IO.File.AppendAllText(@"C:\Temp\Pagination.log", pageIndex + " - " + pageSize + " - " + pageIndex * pageSize + Environment.NewLine);

            var returnResul = new Result
            {
                Data = Repo.Skip(pageIndex * pageSize).Take(pageSize).ToArray(),
                Count = Repo.Count
            };

            System.IO.File.AppendAllText(@"C:\Temp\Pagination.log",  "Dimensione: " + returnResul.Data.Length + " - " + returnResul.Count + Environment.NewLine);

            return returnResul;
            //return repo; //.Skip(pageIndex).Take(pageSize);
        }
    }
}