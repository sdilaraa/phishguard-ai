namespace PhishGuardAI.Api.DTOs.Analysis;

public class AnalysisResponse
{
    public string AnalysisType { get; set; } = string.Empty;
    public int RiskScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<RiskFindingDto> Findings { get; set; } = [];
    public DateTime AnalyzedAtUtc { get; set; } = DateTime.UtcNow;
}
