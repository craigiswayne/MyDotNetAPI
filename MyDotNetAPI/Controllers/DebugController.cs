using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace MyDotNetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public DebugController(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Get()
    {
        var env = new {
            hostEnvironmentName = _hostEnvironment.EnvironmentName,
            isProduction = _hostEnvironment.IsProduction(),
            isDevelopment = _hostEnvironment.IsDevelopment(),
            aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            dbType = _configuration.GetRequiredSection("Db").GetValue<string>("Type"),
            defaultConnection = _configuration.GetConnectionString("DefaultConnection")
        };
        return env;
    }
}