using Microsoft.AspNetCore.Mvc;

[Route("healthcheck")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is running!");
    }
}