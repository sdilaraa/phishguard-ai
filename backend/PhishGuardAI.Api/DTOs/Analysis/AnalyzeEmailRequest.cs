namespace PhishGuardAI.Api.DTOs.Analysis;

public class AnalyzeEmailRequest
{
    public string Subject { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
