using JwtAuthentication_ASP.net8.Core.OtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication_ASP.net8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("only-user")]
        [Authorize(Roles =StaticUserRoles.USER)]
        public IActionResult OnlyUser()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("only-admin")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public IActionResult OnlyAdmin()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("only-owner")]
        [Authorize(Roles =StaticUserRoles.OWNER)]
        public IActionResult OnlyOwner()
        {
            return Ok(Summaries);
        }
    }
}
