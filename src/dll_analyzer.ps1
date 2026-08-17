param([string]$Path)

$path = $Path.Trim().Trim([char]34)
if (-not (Test-Path $path)) {
    Write-Host '[!] File non trovato.' -ForegroundColor Red
    exit 1
}

$bytes = [IO.File]::ReadAllBytes($path)
$str = [Text.Encoding]::ASCII.GetString($bytes)
$dlls = [regex]::Matches($str, '(?i)[A-Za-z0-9_]+\.dll')
$found = $dlls.Value | Where-Object { $_.Length -gt 5 -and $_ -match '\.dll$' } | Sort-Object -Unique

Write-Host '  DLL referenziate nel binario:' $found.Count -ForegroundColor Cyan
Write-Host ''

$miss = @()
$ok = 0

foreach ($dll in $found) {
    $sys32 = Test-Path ('C:\Windows\System32\' + $dll)
    $syswow = Test-Path ('C:\Windows\SysWOW64\' + $dll)
    if (-not $sys32 -and -not $syswow) {
        $miss += $dll
        Write-Host '  [?]' $dll -ForegroundColor Yellow
    } else {
        $ok++
        Write-Host '  [OK]' $dll -ForegroundColor DarkGray
    }
}

Write-Host ''
Write-Host ('  Trovate nel sistema: {0} / {1}' -f $ok, $found.Count) -ForegroundColor Green

if ($miss.Count -gt 0) {
    Write-Host '  DLL non in System32/SysWOW64 (' $miss.Count '):' -ForegroundColor Yellow
    Write-Host '  (potrebbero essere side-by-side, nel path, o mancanti)' -ForegroundColor Gray
}