using Microsoft.AspNetCore.Mvc;

namespace PhishGuardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            service = "PhishGuard AI API",
            version = "1.0.0",
            timestampUtc = DateTime.UtcNow
        });
    }
}
