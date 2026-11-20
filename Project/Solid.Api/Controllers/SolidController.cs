using Microsoft.AspNetCore.Mvc;

namespace Solid.Api.Controllers;

[ApiController]
public class SolidController : ControllerBase
{
    // Só de exemplo, depois tiramos daqui
    [HttpGet("/api/solid")]
    public IActionResult Ping()
    {
        return Ok("SOLID middleware a funcionar");
    }
}