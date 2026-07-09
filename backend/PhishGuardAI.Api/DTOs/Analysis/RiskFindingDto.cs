namespace PhishGuardAI.Api.DTOs.Analysis;

public class RiskFindingDto
{
    public string RuleCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public int ScoreContribution { get; set; }
}
