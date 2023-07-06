using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BffAngular.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    // [Requires(Permissions.OrderAllAccess)]
    // [Authorize("RequireInteractiveUser")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("{hola}")]
    public string Get(string hola)
    {
        
        return hola;
    }


    // [HttpGet("Permissions")]
    // [AllowAnonymous]
    // public IEnumerable<Permissions> GetPermissionsEnumerable()
    // {
    //     var claims = HttpContext.User.Claims;
    //     var permissionsClaim =
    //         claims?.SingleOrDefault(c => c.Type == Constants.ClaimType);
    //
    //     return permissionsClaim?.Value.UnpackPermissionsFromString()!;
    // }
    //
    // [HttpGet("And")]
    // // [Requires(PermissionOperator.And, Permissions.UserAllAccess, Permissions.OrderCreate)]
    // public IEnumerable<Permissions> GetAnd()
    // {
    //     var claims = HttpContext.User.Claims;
    //     var permissionsClaim =
    //         claims?.SingleOrDefault(c => c.Type == Constants.ClaimType);
    //
    //     return permissionsClaim?.Value.UnpackPermissionsFromString()!;
    // }
    //
    // [HttpGet("Or")]
    // // [Requires(PermissionOperator.Or, Permissions.UserAllAccess, Permissions.OrderCreate)]
    // public IEnumerable<Permissions> GetOr()
    // {
    //     var claims = HttpContext.User.Claims;
    //     var permissionsClaim =
    //         claims?.SingleOrDefault(c => c.Type == Constants.ClaimType);
    //
    //     return permissionsClaim?.Value.UnpackPermissionsFromString()!;
    // }
}