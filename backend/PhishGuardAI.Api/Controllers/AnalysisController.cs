using Microsoft.AspNetCore.Mvc;
using PhishGuardAI.Api.DTOs.Analysis;
using PhishGuardAI.Api.Services.EmailAnalysis;
using PhishGuardAI.Api.Services.UrlAnalysis;

namespace PhishGuardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly UrlAnalysisService _urlAnalysisService;
    private readonly EmailAnalysisService _emailAnalysisService;

    public AnalysisController(
        UrlAnalysisService urlAnalysisService,
        EmailAnalysisService emailAnalysisService)
    {
        _urlAnalysisService = urlAnalysisService;
        _emailAnalysisService = emailAnalysisService;
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

    [HttpPost("email")]
    public IActionResult AnalyzeEmail([FromBody] AnalyzeEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Subject) &&
            string.IsNullOrWhiteSpace(request.Sender) &&
            string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest(new
            {
                message = "Analiz edilecek e-posta içeriği boş olamaz."
            });
        }

        var result = _emailAnalysisService.Analyze(
            request.Subject,
            request.Sender,
            request.Body
        );

        return Ok(result);
    }
}
