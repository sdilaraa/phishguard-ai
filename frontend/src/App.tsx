import { useState } from 'react'
import './App.css'

type RiskFinding = {
  ruleCode: string
  title: string
  description: string
  severity: string
  scoreContribution: number
}

type AnalysisResponse = {
  analysisType: string
  riskScore: number
  riskLevel: string
  summary: string
  recommendation: string
  findings: RiskFinding[]
  analyzedAtUtc: string
}

const API_BASE_URL = 'http://localhost:5223'

function App() {
  const [activeTab, setActiveTab] = useState<'url' | 'email'>('url')
  const [url, setUrl] = useState('http://bit.ly/login-update')
  const [subject, setSubject] = useState('Hesabınız askıya alınacak')
  const [sender, setSender] = useState('security@example.com')
  const [body, setBody] = useState('Hesabınız bugün içinde askıya alınacak. Hemen giriş yaparak şifrenizi doğrulayın.')
  const [result, setResult] = useState<AnalysisResponse | null>(null)
  const [error, setError] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  const analyzeUrl = async () => {
    setError('')
    setResult(null)

    if (!url.trim()) {
      setError('Analiz edilecek URL boş olamaz.')
      return
    }

    setIsLoading(true)

    try {
      const response = await fetch(`${API_BASE_URL}/api/analysis/url`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ url }),
      })

      if (!response.ok) {
        throw new Error('URL analizi sırasında hata oluştu.')
      }

      const data: AnalysisResponse = await response.json()
      setResult(data)
    } catch {
      setError('Backend bağlantısı kurulamadı. API çalışıyor mu kontrol edin.')
    } finally {
      setIsLoading(false)
    }
  }

  const analyzeEmail = async () => {
    setError('')
    setResult(null)

    if (!subject.trim() && !sender.trim() && !body.trim()) {
      setError('Analiz edilecek e-posta içeriği boş olamaz.')
      return
    }

    setIsLoading(true)

    try {
      const response = await fetch(`${API_BASE_URL}/api/analysis/email`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ subject, sender, body }),
      })

      if (!response.ok) {
        throw new Error('E-posta analizi sırasında hata oluştu.')
      }

      const data: AnalysisResponse = await response.json()
      setResult(data)
    } catch {
      setError('Backend bağlantısı kurulamadı. API çalışıyor mu kontrol edin.')
    } finally {
      setIsLoading(false)
    }
  }

  const getRiskClass = (riskLevel: string) => {
    const normalized = riskLevel.toLowerCase()

    if (normalized === 'high') return 'risk-high'
    if (normalized === 'medium') return 'risk-medium'
    return 'risk-low'
  }

  return (
    <main className="app-shell">
      <section className="hero">
        <div>
          <span className="eyebrow">PhishGuard AI</span>
          <h1>Hibrit Oltalama Tespit ve Risk Analiz Platformu</h1>
          <p>
            Şüpheli URL ve e-posta içeriklerini kural tabanlı güvenlik kontrolleriyle analiz eder,
            risk seviyesini ve tespit edilen göstergeleri açıklanabilir şekilde sunar.
          </p>
        </div>

        <div className="status-card">
          <span>Backend API</span>
          <strong>http://localhost:5223</strong>
          <small>URL ve e-posta analiz endpointleri hazırlandı.</small>
        </div>
      </section>

      <section className="workspace">
        <div className="tabs">
          <button
            className={activeTab === 'url' ? 'active' : ''}
            onClick={() => {
              setActiveTab('url')
              setResult(null)
              setError('')
            }}
          >
            URL Analizi
          </button>
          <button
            className={activeTab === 'email' ? 'active' : ''}
            onClick={() => {
              setActiveTab('email')
              setResult(null)
              setError('')
            }}
          >
            E-posta Analizi
          </button>
        </div>

        <div className="content-grid">
          <section className="panel">
            {activeTab === 'url' ? (
              <>
                <h2>Şüpheli URL Analizi</h2>
                <p>
                  Bağlantı formatı, HTTPS kullanımı, kısaltıcı servis, IP adresi, uzun domain,
                  fazla tire/rakam, punycode ve şüpheli dosya uzantısı gibi göstergeler kontrol edilir.
                </p>

                <label htmlFor="url">Analiz edilecek URL</label>
                <input
                  id="url"
                  value={url}
                  onChange={(event) => setUrl(event.target.value)}
                  placeholder="https://example.com"
                />

                <button className="primary-button" onClick={analyzeUrl} disabled={isLoading}>
                  {isLoading ? 'Analiz ediliyor...' : 'URL Analiz Et'}
                </button>
              </>
            ) : (
              <>
                <h2>E-posta / Metin Analizi</h2>
                <p>
                  Aciliyet dili, kimlik doğrulama talebi, ödeme bilgisi isteği, şüpheli ek ifadeleri
                  ve yanıltıcı kampanya dili gibi göstergeler incelenir.
                </p>

                <label htmlFor="subject">Konu</label>
                <input
                  id="subject"
                  value={subject}
                  onChange={(event) => setSubject(event.target.value)}
                />

                <label htmlFor="sender">Gönderen</label>
                <input
                  id="sender"
                  value={sender}
                  onChange={(event) => setSender(event.target.value)}
                />

                <label htmlFor="body">E-posta içeriği</label>
                <textarea
                  id="body"
                  value={body}
                  onChange={(event) => setBody(event.target.value)}
                  rows={7}
                />

                <button className="primary-button" onClick={analyzeEmail} disabled={isLoading}>
                  {isLoading ? 'Analiz ediliyor...' : 'E-posta Analiz Et'}
                </button>
              </>
            )}
          </section>

          <section className="panel result-panel">
            <h2>Analiz Sonucu</h2>

            {error && <div className="error-box">{error}</div>}

            {!result && !error && (
              <div className="empty-state">
                <strong>Henüz analiz sonucu yok.</strong>
                <span>Bir URL veya e-posta içeriği girip analiz başlatın.</span>
              </div>
            )}

            {result && (
              <div className="result-content">
                <div className={`risk-card ${getRiskClass(result.riskLevel)}`}>
                  <span>Risk Skoru</span>
                  <strong>{result.riskScore}/100</strong>
                  <em>{result.riskLevel}</em>
                </div>

                <p className="summary">{result.summary}</p>

                <div className="recommendation-box">
                  <strong>Güvenlik Önerisi</strong>
                  <p>{result.recommendation}</p>
                </div>

                <div className="findings">
                  <h3>Tespit Edilen Bulgular</h3>

                  {result.findings.length === 0 ? (
                    <p>Belirgin bir risk göstergesi bulunmadı.</p>
                  ) : (
                    result.findings.map((finding) => (
                      <article className="finding-card" key={finding.ruleCode}>
                        <div>
                          <strong>{finding.title}</strong>
                          <span>{finding.ruleCode}</span>
                        </div>
                        <p>{finding.description}</p>
                        <small>
                          Önem: {finding.severity} • Puan katkısı: +{finding.scoreContribution}
                        </small>
                      </article>
                    ))
                  )}
                </div>
              </div>
            )}
          </section>
        </div>
      </section>
    </main>
  )
}

export default App
