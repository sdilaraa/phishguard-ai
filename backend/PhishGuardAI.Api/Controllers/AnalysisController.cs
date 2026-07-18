using Microsoft.AspNetCore.Mvc;
using PhishGuardAI.Api.DTOs.Analysis;
using PhishGuardAI.Api.Services.EmailAnalysis;
using PhishGuardAI.Api.Services.UrlAnalysis;
using PhishGuardAI.Api.Services.History;

namespace PhishGuardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly UrlAnalysisService _urlAnalysisService;
    private readonly EmailAnalysisService _emailAnalysisService;
    private readonly AnalysisHistoryService _analysisHistoryService;

    public AnalysisController(
        UrlAnalysisService urlAnalysisService,
        EmailAnalysisService emailAnalysisService,
        AnalysisHistoryService analysisHistoryService)
    {
        _urlAnalysisService = urlAnalysisService;
        _emailAnalysisService = emailAnalysisService;
        _analysisHistoryService = analysisHistoryService;
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
        _analysisHistoryService.Add("Url", request.Url, result);

        return Ok(result);
    }

    [HttpGet("history")]
    public IActionResult GetHistory()
    {
        var history = _analysisHistoryService.GetAll();

        return Ok(history);
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

        var preview = string.IsNullOrWhiteSpace(request.Subject)
            ? request.Body
            : request.Subject;

        _analysisHistoryService.Add("Email", preview, result);

        return Ok(result);
    }
}
