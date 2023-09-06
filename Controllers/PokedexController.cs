using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MyDotNetAPI.Models;
using MyDotNetAPI.Services;

namespace MyDotNetAPI.Controllers;

[ApiController]
// [LogActionFilter] // TODO
[Route("[controller]")]
public class PokedexController : ControllerBase
{
    // TODO: Custom ControllerBase
    // TODO: Custom ControllerBase for logging
    private readonly IPokedexService _pokedexService;

    public PokedexController(IPokedexService pokedexService)
    {
        _pokedexService = pokedexService;
    }
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<List<Pokemon>> List()
    {
        IQueryable<Pokemon> result;
        try
        {
            result = _pokedexService.List();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new {
                summary = "DB is unavailable",
                ex.Message
            });
        }

        if (!result.Any())
        {
            return StatusCode(StatusCodes.Status204NoContent);
        }
        else
        {
            return Ok(result);
        }
    }
}