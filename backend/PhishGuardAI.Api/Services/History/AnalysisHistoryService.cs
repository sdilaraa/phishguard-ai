using PhishGuardAI.Api.DTOs.Analysis;

namespace PhishGuardAI.Api.Services.History;

public class AnalysisHistoryService
{
    private readonly List<AnalysisHistoryItemDto> _history = [];

    public void Add(string analysisType, string inputPreview, AnalysisResponse response)
    {
        var item = new AnalysisHistoryItemDto
        {
            AnalysisId = Guid.NewGuid(),
            AnalysisType = analysisType,
            InputPreview = TrimPreview(inputPreview),
            RiskScore = response.RiskScore,
            RiskLevel = response.RiskLevel,
            Summary = response.Summary,
            AnalyzedAtUtc = response.AnalyzedAtUtc
        };

        _history.Insert(0, item);
    }

    public IReadOnlyList<AnalysisHistoryItemDto> GetAll()
    {
        return _history.AsReadOnly();
    }

    private static string TrimPreview(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Boş içerik";
        }

        var trimmed = value.Trim();

        return trimmed.Length <= 120
            ? trimmed
            : $"{trimmed[..120]}...";
    }
}
