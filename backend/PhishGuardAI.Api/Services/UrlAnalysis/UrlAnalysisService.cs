using System.Net;
using PhishGuardAI.Api.DTOs.Analysis;

namespace PhishGuardAI.Api.Services.UrlAnalysis;

public class UrlAnalysisService
{
    private static readonly HashSet<string> UrlShorteners = new(StringComparer.OrdinalIgnoreCase)
    {
        "bit.ly",
        "tinyurl.com",
        "t.co",
        "goo.gl",
        "ow.ly",
        "is.gd",
        "cutt.ly",
        "shorturl.at"
    };

    private static readonly string[] SuspiciousFileExtensions =
    [
        ".exe",
        ".scr",
        ".bat",
        ".cmd",
        ".js",
        ".vbs",
        ".zip",
        ".rar"
    ];

    public AnalysisResponse Analyze(string input)
    {
        var findings = new List<RiskFindingDto>();
        var normalizedInput = input.Trim();

        if (!Uri.TryCreate(normalizedInput, UriKind.Absolute, out var uri))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_INVALID_FORMAT",
                Title = "Geçersiz URL formatı",
                Description = "Girilen bağlantı geçerli bir URL formatında değil.",
                Severity = "Medium",
                ScoreContribution = 30
            });

            return BuildResponse(findings);
        }

        var host = uri.Host;
        var normalizedHost = host.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
            ? host[4..]
            : host;

        if (uri.Scheme != Uri.UriSchemeHttps)
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_NO_HTTPS",
                Title = "HTTPS kullanılmıyor",
                Description = "Bağlantı HTTPS yerine güvenli olmayan bir protokol kullanıyor.",
                Severity = "Medium",
                ScoreContribution = 15
            });
        }

        if (IPAddress.TryParse(host, out _))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_IP_ADDRESS",
                Title = "Domain yerine IP adresi kullanılmış",
                Description = "Bağlantının alan adı yerine doğrudan IP adresi kullanması phishing riskini artırabilir.",
                Severity = "High",
                ScoreContribution = 25
            });
        }

        if (UrlShorteners.Contains(normalizedHost))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_SHORTENER",
                Title = "URL kısaltıcı servis tespit edildi",
                Description = "Kısaltılmış bağlantılar gerçek hedef adresi gizleyebilir.",
                Severity = "High",
                ScoreContribution = 20
            });
        }

        if (host.Length > 35)
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_LONG_DOMAIN",
                Title = "Uzun domain yapısı",
                Description = "Alan adının normalden uzun olması kullanıcıyı yanıltmaya yönelik olabilir.",
                Severity = "Medium",
                ScoreContribution = 10
            });
        }

        var hyphenCount = host.Count(character => character == '-');
        if (hyphenCount >= 2)
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_MANY_HYPHENS",
                Title = "Domain içinde fazla tire kullanımı",
                Description = "Alan adında fazla tire kullanımı sahte domainlerde sık görülebilir.",
                Severity = "Medium",
                ScoreContribution = 10
            });
        }

        var digitCount = host.Count(char.IsDigit);
        if (digitCount >= 3)
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_MANY_DIGITS",
                Title = "Domain içinde fazla rakam kullanımı",
                Description = "Alan adında fazla rakam bulunması şüpheli bir bağlantı göstergesi olabilir.",
                Severity = "Medium",
                ScoreContribution = 10
            });
        }

        if (host.StartsWith("xn--", StringComparison.OrdinalIgnoreCase) ||
            host.Contains(".xn--", StringComparison.OrdinalIgnoreCase))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_PUNYCODE",
                Title = "Punycode kullanımı tespit edildi",
                Description = "Punycode kullanımı benzer karakterlerle marka taklidi yapılması riskini artırabilir.",
                Severity = "High",
                ScoreContribution = 20
            });
        }

        if (SuspiciousFileExtensions.Any(extension =>
                uri.AbsolutePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "URL_SUSPICIOUS_FILE",
                Title = "Şüpheli dosya uzantısı",
                Description = "Bağlantı çalıştırılabilir veya riskli olabilecek bir dosyaya yönleniyor olabilir.",
                Severity = "High",
                ScoreContribution = 20
            });
        }

        return BuildResponse(findings);
    }

    private static AnalysisResponse BuildResponse(List<RiskFindingDto> findings)
    {
        var score = Math.Min(100, findings.Sum(finding => finding.ScoreContribution));
        var level = score switch
        {
            <= 30 => "Low",
            <= 65 => "Medium",
            _ => "High"
        };

        var summary = findings.Count == 0
            ? "Bağlantıda temel kurallara göre belirgin bir risk göstergesi bulunmadı."
            : "Bağlantıda bir veya daha fazla risk göstergesi tespit edildi.";

        return new AnalysisResponse
        {
            AnalysisType = "Url",
            RiskScore = score,
            RiskLevel = level,
            Summary = summary,
            Findings = findings,
            AnalyzedAtUtc = DateTime.UtcNow
        };
    }
}
