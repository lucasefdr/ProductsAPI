using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAPI.Controllers.v2;

[ApiVersion("2.0")]
[Route("api/v{version:apiversion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
public class VersioningController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult Get() => Ok("Version 2");
}
