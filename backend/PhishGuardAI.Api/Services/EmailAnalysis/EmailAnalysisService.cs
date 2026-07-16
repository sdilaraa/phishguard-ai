using PhishGuardAI.Api.DTOs.Analysis;

namespace PhishGuardAI.Api.Services.EmailAnalysis;

public class EmailAnalysisService
{
    private static readonly string[] UrgencyKeywords =
    [
        "acil",
        "hemen",
        "son uyarı",
        "son tarih",
        "24 saat",
        "bugün içinde",
        "askıya alınacak",
        "kapatılacak",
        "erişiminiz kısıtlanacak"
    ];

    private static readonly string[] CredentialKeywords =
    [
        "şifre",
        "parola",
        "giriş yap",
        "hesabınızı doğrulayın",
        "kimlik doğrulama",
        "kullanıcı adı",
        "oturum aç"
    ];

    private static readonly string[] PaymentKeywords =
    [
        "kart bilgisi",
        "kredi kartı",
        "cvv",
        "ödeme bilgisi",
        "banka",
        "iban",
        "fatura ödeme",
        "internet bankacılığı"
    ];

    private static readonly string[] AttachmentKeywords =
    [
        "ekli dosya",
        "fatura ektedir",
        "belgeyi indir",
        "dosyayı indir",
        "makro",
        ".exe",
        ".zip",
        ".rar"
    ];

    private static readonly string[] InformalOrSuspiciousPhrases =
    [
        "kazandınız",
        "hediyeniz hazır",
        "ücretsiz fırsat",
        "hesabınız bloke",
        "güvenlik nedeniyle tıklayın"
    ];

    public AnalysisResponse Analyze(string subject, string sender, string body)
    {
        var findings = new List<RiskFindingDto>();

        var combinedText = $"{subject} {sender} {body}".ToLowerInvariant();

        AddKeywordFinding(
            findings,
            combinedText,
            UrgencyKeywords,
            "EMAIL_URGENCY_LANGUAGE",
            "Aciliyet veya baskı dili tespit edildi",
            "E-posta metninde kullanıcıyı hızlı karar vermeye yönlendiren aciliyet/baskı ifadeleri bulunuyor.",
            "Medium",
            15
        );

        AddKeywordFinding(
            findings,
            combinedText,
            CredentialKeywords,
            "EMAIL_CREDENTIAL_REQUEST",
            "Kimlik doğrulama veya şifre talebi göstergesi",
            "E-posta içeriğinde kullanıcıdan oturum açma, şifre veya hesap doğrulama işlemi yapması isteniyor olabilir.",
            "High",
            25
        );

        AddKeywordFinding(
            findings,
            combinedText,
            PaymentKeywords,
            "EMAIL_PAYMENT_REQUEST",
            "Ödeme veya finansal bilgi talebi göstergesi",
            "E-posta içeriğinde ödeme, kart, banka veya finansal bilgi talebiyle ilişkili ifadeler bulunuyor.",
            "High",
            25
        );

        AddKeywordFinding(
            findings,
            combinedText,
            AttachmentKeywords,
            "EMAIL_SUSPICIOUS_ATTACHMENT",
            "Şüpheli ek veya dosya indirme ifadesi",
            "E-posta içeriğinde ekli dosya, indirme bağlantısı veya riskli dosya türleriyle ilişkili ifadeler bulunuyor.",
            "Medium",
            15
        );

        AddKeywordFinding(
            findings,
            combinedText,
            InformalOrSuspiciousPhrases,
            "EMAIL_SUSPICIOUS_LANGUAGE",
            "Şüpheli veya ikna edici pazarlama dili",
            "E-posta içeriğinde kullanıcıyı yanıltabilecek kampanya, ödül veya güvenlik bahanesi içeren ifadeler bulunuyor.",
            "Medium",
            10
        );

        if (!string.IsNullOrWhiteSpace(sender) && !sender.Contains("@"))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = "EMAIL_INVALID_SENDER",
                Title = "Gönderen adresi e-posta formatında değil",
                Description = "Gönderen bilgisi geçerli bir e-posta adresi gibi görünmüyor.",
                Severity = "Medium",
                ScoreContribution = 10
            });
        }

        return BuildResponse(findings);
    }

    private static void AddKeywordFinding(
        List<RiskFindingDto> findings,
        string text,
        string[] keywords,
        string ruleCode,
        string title,
        string description,
        string severity,
        int scoreContribution)
    {
        if (keywords.Any(keyword => text.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
        {
            findings.Add(new RiskFindingDto
            {
                RuleCode = ruleCode,
                Title = title,
                Description = description,
                Severity = severity,
                ScoreContribution = scoreContribution
            });
        }
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
            ? "E-posta içeriğinde temel kurallara göre belirgin bir risk göstergesi bulunmadı."
            : "E-posta içeriğinde bir veya daha fazla risk göstergesi tespit edildi.";

        return new AnalysisResponse
        {
            AnalysisType = "Email",
            RiskScore = score,
            RiskLevel = level,
            Summary = summary,
            Findings = findings,
            AnalyzedAtUtc = DateTime.UtcNow
        };
    }
}
