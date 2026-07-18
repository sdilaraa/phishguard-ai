namespace PhishGuardAI.Api.DTOs.Analysis;

public class AnalysisHistoryItemDto
{
    public Guid AnalysisId { get; set; }
    public string AnalysisType { get; set; } = string.Empty;
    public string InputPreview { get; set; } = string.Empty;
    public int RiskScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime AnalyzedAtUtc { get; set; }
}
