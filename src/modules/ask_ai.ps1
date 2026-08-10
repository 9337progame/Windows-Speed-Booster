param([string]$query)

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

# ============================================================
# 1. RACCOLTA DATI DEL PC
# ============================================================
$cpu = (Get-CimInstance Win32_Processor | Select-Object -First 1).Name
$cores = (Get-CimInstance Win32_Processor | Select-Object -First 1).NumberOfCores
$ramTot = [math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB, 1)
$ramUsed = [math]::Round((Get-CimInstance Win32_OperatingSystem).TotalVisibleMemorySize/1MB - (Get-CimInstance Win32_OperatingSystem).FreePhysicalMemory/1MB, 1)
$diskUsed = [math]::Round((Get-PSDrive C).Used / 1GB, 1)
$diskFree = [math]::Round((Get-PSDrive C).Free / 1GB, 1)
$gpu = (Get-CimInstance Win32_VideoController | Select-Object -First 1).Name
$os = (Get-CimInstance Win32_OperatingSystem).Caption
$build = (Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion').CurrentBuild
$uptime = [math]::Round(((Get-Date) - (Get-CimInstance Win32_OperatingSystem).LastBootUpTime).TotalHours, 1)

$sysinfo = "CPU: $cpu ($cores core) | RAM: $ramTot GB tot ($ramUsed GB usati) | Disco C: $diskUsed GB usati, $diskFree GB liberi | GPU: $gpu | OS: $os Build $build | Uptime: $uptime ore"

$fullPrompt = "Sei un assistente IT esperto. Rispondi in italiano. Ecco i dati del PC: $sysinfo. Domanda: $query"

# ============================================================
# 2. CONFIGURAZIONE GROQ
# ============================================================
$apiKey = "gsk_tuo-token-qui"  # SOSTITUISCI

$model = "llama-3.1-8b-instant"

$body = @{
    model = $model
    messages = @(
        @{
            role = "user"
            content = $fullPrompt
        }
    )
    max_tokens = 500
    temperature = 0.7
} | ConvertTo-Json -Depth 10

$headers = @{
    "Authorization" = "Bearer $apiKey"
    "Content-Type" = "application/json"
}

$apiUrl = "https://api.groq.com/openai/v1/chat/completions"

# ============================================================
# 3. CHIAMATA API
# ============================================================
try {
    Write-Host "⏳ Sto pensando... (modello: $model)" -ForegroundColor Cyan
    
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Body $body -Headers $headers -TimeoutSec 90 -ErrorAction Stop
    $answer = $response.choices[0].message.content
    
    # ============================================================
    # 4. NORMALIZZAZIONE PER RIMUOVERE ACCENTI (metodo corretto)
    # ============================================================
    # Converte il testo in forma NFKD e rimuove i diacritici
    $normalized = $answer.Normalize([System.Text.NormalizationForm]::FormKD)
    
    # Filtra solo i caratteri che non sono diacritici
    $chars = $normalized.ToCharArray()
    $cleanChars = $chars | Where-Object {
        [System.Globalization.CharUnicodeInfo]::GetUnicodeCategory($_) -ne [System.Globalization.UnicodeCategory]::NonSpacingMark
    }
    
    # Ricostruisce la stringa senza accenti
    $answer = -join $cleanChars
    
    Write-Host ""
    Write-Host $answer -ForegroundColor White
    Write-Host ""
}
catch {
    $err = $_.Exception.Message
    Write-Host "❌ Errore: $err" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        Write-Host "💡 Dettaglio: $responseBody" -ForegroundColor Yellow
    }
    Write-Host "💡 Suggerimenti:" -ForegroundColor Yellow
    Write-Host "   - Verifica che il token Groq sia corretto (inizia con gsk_)" -ForegroundColor Yellow
    Write-Host "   - Groq ha un limite di 30 richieste/minuto" -ForegroundColor Yellow
    Write-Host "   - Se l'errore persiste, controlla la connessione internet" -ForegroundColor Yellow
}