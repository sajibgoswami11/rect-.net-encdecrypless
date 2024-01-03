using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizWebAPI.Areas.xEmptyArea.Dtos;
using BizWebAPI.Areas.xEmptyArea.Repository;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BizWebAPI.Areas.xEmptyArea.Controllers
{
    [Authorize]
    [Area("xEmptyArea")]
    [ApiController]
    [Route("xEmptyArea/[controller]")]
    public class WeatherForecastController : ControllerBase
    {        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "Internet Banking"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("xEmptyAreaGetAsync")]
        public async Task<IEnumerable<WeatherForecast>> xEmptyAreaGetAsync()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
