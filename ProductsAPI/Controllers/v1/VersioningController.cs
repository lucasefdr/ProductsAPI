using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAPI.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiversion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
public class VersioningController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult Get() => Ok("Version 1");
}
