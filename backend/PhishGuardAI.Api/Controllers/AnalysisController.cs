using Microsoft.AspNetCore.Mvc;
using PhishGuardAI.Api.DTOs.Analysis;
using PhishGuardAI.Api.Services.UrlAnalysis;

namespace PhishGuardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly UrlAnalysisService _urlAnalysisService;

    public AnalysisController(UrlAnalysisService urlAnalysisService)
    {
        _urlAnalysisService = urlAnalysisService;
    }

    [HttpPost("url")]
    public IActionResult AnalyzeUrl([FromBody] AnalyzeUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
        {
            return BadRequest(new
            {
                message = "Analiz edilecek URL boş olamaz."
            });
        }

        var result = _urlAnalysisService.Analyze(request.Url);

        return Ok(result);
    }
}
