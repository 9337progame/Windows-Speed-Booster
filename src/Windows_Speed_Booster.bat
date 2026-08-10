@echo off
setlocal EnableDelayedExpansion

:: =====================================================
:: AGGIUNGI POWERSHELL AL PATH (se manca)
:: =====================================================
set "PS_PATH=%SystemRoot%\System32\WindowsPowerShell\v1.0"
if exist "%PS_PATH%\powershell.exe" (
    set "PATH=%PS_PATH%;%PATH%"
) else (
    echo  ERRORE: PowerShell non trovato in %PS_PATH%
    echo  Questo sistema non ha PowerShell installato.
    echo  Il batch non puo' funzionare senza PowerShell.
    pause
    exit
)

:: =====================================================
:: AGGIUNGI SYSTEM32 AL PATH (per findstr, netsh, ecc.)
:: =====================================================
set "PATH=%SystemRoot%\System32;%PATH%"

cd /d "%~dp0"
title Windows Speed Booster V14
color 0A
cls

:: =====================================================
:: VARIABILI GLOBALI
:: =====================================================
set "LOG_FILE=%USERPROFILE%\Desktop\SpeedBooster_Log.txt"
set "VERSION=14.0"
set "DATE_NOW=%date% %time%"

:: =====================================================
:: 1. CONTROLLO PRIVILEGI
:: =====================================================
echo test > "%windir%\test_admin.tmp" 2>nul
if !errorlevel!==1 (
    color 0C
    echo.
    echo  ERRORE: Esegui come Amministratore!
    echo  Tasto destro ^> Esegui come admin
    echo.
    pause
    exit
)
del "%windir%\test_admin.tmp" 2>nul

:: =====================================================
:: 2. INIZIALIZZAZIONE LOG
:: =====================================================
echo ===================================================== > "%LOG_FILE%"
echo  Windows Speed Booster V%VERSION% - Log Sessione     >> "%LOG_FILE%"
echo  Data/Ora: %DATE_NOW%                                 >> "%LOG_FILE%"
echo ===================================================== >> "%LOG_FILE%"

:: =====================================================
:: 3. PUNTO DI RIPRISTINO AUTOMATICO
:: =====================================================
call :header
echo  [*] Creazione punto di ripristino automatico...
powershell -NonInteractive -command "try { Checkpoint-Computer -Description 'SpeedBooster V%VERSION%' -RestorePointType 'MODIFY_SETTINGS' -ErrorAction Stop; Write-Host '[OK] Punto di ripristino creato.' } catch { Write-Host '[!] Punto di ripristino non disponibile (normale su Win11).' }"
echo [%DATE_NOW%] Avvio sessione. >> "%LOG_FILE%"
timeout /t 2 /nobreak >nul

:: =====================================================
:: 4. FASE DI PULIZIA PROFONDA ALL'AVVIO
:: =====================================================
call :header
echo  [*] Avvio protocollo di pulizia profonda...
echo.

echo  [1/7] Flush cache DNS...
ipconfig /flushdns >nul 2>&1
echo        OK

echo  [2/7] Pulizia cartelle TEMP di sistema...
for /f "delims=" %%F in ('dir /a:-d /b /s "%windir%\Temp" 2^>nul') do (del /f /q "%%F" >nul 2>&1)
for /d %%x in ("%windir%\Temp\*") do rd /s /q "%%x" >nul 2>&1
echo        OK

echo  [3/7] Pulizia TEMP utente...
for /f "delims=" %%F in ('dir /a:-d /b /s "%temp%" 2^>nul') do (del /f /q "%%F" >nul 2>&1)
for /d %%x in ("%temp%\*") do rd /s /q "%%x" >nul 2>&1
echo        OK

echo  [4/7] Pulizia Prefetch e Log di sistema...
del /f /s /q "C:\Windows\Prefetch\*" >nul 2>&1
del /f /s /q "%windir%\*.log" >nul 2>&1
echo        OK

echo  [5/7] Pulizia cache Windows Update...
net stop wuauserv >nul 2>&1
net stop bits >nul 2>&1
rd /s /q "C:\Windows\SoftwareDistribution\Download" >nul 2>&1
net start wuauserv >nul 2>&1
net start bits >nul 2>&1
echo        OK

echo  [6/7] Svuotamento Cestino...
powershell -NonInteractive -command "Clear-RecycleBin -Force -ErrorAction SilentlyContinue" >nul 2>&1
echo        OK

echo  [7/7] Pulizia cache miniature...
del /f /s /q "%LocalAppData%\Microsoft\Windows\Explorer\thumbcache_*.db" >nul 2>&1
echo        OK

echo.
echo  [==] Pulizia iniziale completata!
echo [%DATE_NOW%] Pulizia iniziale eseguita. >> "%LOG_FILE%"
timeout /t 2 /nobreak >nul

:: =====================================================
:: 5. MENU PRINCIPALE
:: =====================================================
:menu
call :header
echo  ---------------------------------------------
echo    MENU DI OTTIMIZZAZIONE V%VERSION%
echo  ---------------------------------------------
echo    [1]  Memoria Virtuale (Calcolo Automatico)
echo    [2]  Gestione Core CPU (Boot)
echo    [3]  Effetti Visivi
echo    [4]  Network Boost (TCP + Reset Stack)
echo    [5]  Scanner Hardware Avanzato
echo    [6]  Piano di Alimentazione
echo    [7]  Ottimizzazione Disco (HDD/SSD)
echo    [8]  Servizi Inutili (Disabilita/Ripristina)
echo    [9]  Pulizia Profonda (Manuale)
echo    [10] Rapporto Prestazioni Sistema
echo    [11] Matrix Mode
echo    [12] Tweaks Avanzati e Segreti
echo    [13] Funzioni Segrete e Strumenti Pro
echo    [14] Assistente IA (richiede token Groq gratuito)
echo    [15] Microsoft Activation Scripts (MAS) - Attivazione Windows/Office
echo    [0]  ESCI
echo  ---------------------------------------------
echo.
set /p scelta="  > Seleziona opzione: "

if "%scelta%"=="1"  goto mem_virt
if "%scelta%"=="2"  goto boot_core
if "%scelta%"=="3"  goto vis_fx
if "%scelta%"=="4"  goto network_boost
if "%scelta%"=="5"  goto sys_info
if "%scelta%"=="6"  goto power_plan
if "%scelta%"=="7"  goto disk_opt
if "%scelta%"=="8"  goto services
if "%scelta%"=="9"  goto manual_clean
if "%scelta%"=="10" goto perf_report
if "%scelta%"=="11" goto matrix
if "%scelta%"=="12" goto advanced_tweaks
if "%scelta%"=="13" goto pro_tools
if "%scelta%"=="14" goto ai_assistant
if "%scelta%"=="15" goto mas_activate
if "%scelta%"=="0"  goto exit_script

echo  [!] Opzione non valida. Riprova.
timeout /t 2 /nobreak >nul
goto menu

:: =====================================================
:: SEZIONE 1 — MEMORIA VIRTUALE
:: =====================================================
:mem_virt
call :header
echo  --- MEMORIA VIRTUALE (PAGING) ---
echo.
echo  [*] Calcolo valori ottimali...
for /f "delims=" %%a in ('powershell -NonInteractive -command "[math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory/1MB)"') do set "RAM_MB=%%a"
if "!RAM_MB!"=="" set "RAM_MB=4096"
for /f "delims=" %%b in ('powershell -NonInteractive -command "[math]::Round(!RAM_MB!*1.5)"') do set "RAM_MAX=%%b"
if "!RAM_MAX!"=="" set "RAM_MAX=6144"

echo.
echo  RAM Fisica rilevata   : !RAM_MB! MB
echo  Valore MIN consigliato: !RAM_MB! MB
echo  Valore MAX consigliato: !RAM_MAX! MB
echo.
echo  [1] Imposta valori consigliati automaticamente
echo  [2] Inserisci valori manualmente
echo  [3] Ripristina gestione automatica Windows
echo  [0] Torna al menu
echo.
set /p mv_scelta="  > Scegli: "

if "%mv_scelta%"=="0" goto menu
if "%mv_scelta%"=="1" (
    powershell -NonInteractive -command ^
      "try {" ^
      "  $cs = Get-CimInstance Win32_ComputerSystem;" ^
      "  Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$False};" ^
      "  $pf = Get-CimInstance Win32_PageFileSetting -ErrorAction SilentlyContinue;" ^
      "  if ($pf) { Set-CimInstance -InputObject $pf -Property @{InitialSize=!RAM_MB!;MaximumSize=!RAM_MAX!} }" ^
      "  else { New-CimInstance -ClassName Win32_PageFileSetting -Property @{Name='C:\\pagefile.sys';InitialSize=!RAM_MB!;MaximumSize=!RAM_MAX!} | Out-Null }" ^
      "  Write-Host '[OK] Pagefile impostato: !RAM_MB! - !RAM_MAX! MB'" ^
      "} catch { Write-Host '[!] Errore:' $_.Exception.Message }"
    echo [%DATE_NOW%] Pagefile: !RAM_MB!-!RAM_MAX! MB >> "%LOG_FILE%"
)
if "%mv_scelta%"=="2" (
    set /p min="  > Valore MIN (MB): "
    set /p max="  > Valore MAX (MB): "
    
    :: VALIDAZIONE MIN
    echo !min! | findstr /r "^[0-9][0-9]*$" >nul
    if errorlevel 1 (
        echo  [!] Valore MIN non valido. Inserire solo numeri.
        pause
        goto menu
    )
    :: VALIDAZIONE MAX
    echo !max! | findstr /r "^[0-9][0-9]*$" >nul
    if errorlevel 1 (
        echo  [!] Valore MAX non valido. Inserire solo numeri.
        pause
        goto menu
    )
    :: CONTROLLO MIN <= MAX
    if !min! gtr !max! (
        echo  [!] Il valore MIN non puo' essere maggiore del MAX.
        pause
        goto menu
    )
    
    powershell -NonInteractive -command ^
      "try {" ^
      "  $cs = Get-CimInstance Win32_ComputerSystem;" ^
      "  Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$False};" ^
      "  $pf = Get-CimInstance Win32_PageFileSetting -ErrorAction SilentlyContinue;" ^
      "  if ($pf) { Set-CimInstance -InputObject $pf -Property @{InitialSize=!min!;MaximumSize=!max!} }" ^
      "  else { New-CimInstance -ClassName Win32_PageFileSetting -Property @{Name='C:\\pagefile.sys';InitialSize=!min!;MaximumSize=!max!} | Out-Null }" ^
      "  Write-Host '[OK] Pagefile impostato: !min! - !max! MB'" ^
      "} catch { Write-Host '[!] Errore:' $_.Exception.Message }"
)
if "%mv_scelta%"=="3" (
    powershell -NonInteractive -command ^
      "try {" ^
      "  $cs = Get-CimInstance Win32_ComputerSystem;" ^
      "  Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$True};" ^
      "  Write-Host '[OK] Gestione automatica ripristinata.'" ^
      "} catch { Write-Host '[!] Errore:' $_.Exception.Message }"
)
pause
goto menu

:: =====================================================
:: SEZIONE 2 — CORE CPU BOOT
:: =====================================================
:boot_core
call :header
echo  --- GESTIONE CORE CPU (BOOT) ---
echo.
echo  Core logici disponibili: %NUMBER_OF_PROCESSORS%
echo.
echo  ATTENZIONE: Modificare questo valore puo' rallentare
echo  il boot. Lascia 0 per usare tutti i core (consigliato).
echo.
set /p core="  > Core da allocare al boot (0=Auto/Tutti, M=Menu): "

:: ============ INIZIO CODICE AGGIUNTO (VALIDAZIONE) ============
if /i not "!core!"=="M" (
    if not "!core!"=="0" (
        echo !core! | findstr /r "^[1-9][0-9]*$" >nul
        if errorlevel 1 (
            echo  [!] Inserire un numero valido (0 per Auto/Tutti).
            pause
            goto menu
        )
    )
)
:: ============ FINE CODICE AGGIUNTO ============

if /i "%core%"=="M" goto menu
if "%core%"=="0" (
    bcdedit /deletevalue numproc
    if !errorlevel!==0 (
        echo  [OK] Ripristinato uso automatico di tutti i core.
        echo [%DATE_NOW%] CPU Boot: ripristinato Auto >> "%LOG_FILE%"
    ) else (
        echo  [!] Comando eseguito ^(il valore potrebbe non esistere, e' normale^).
    )
) else (
    bcdedit /set numproc %core%
    if !errorlevel!==0 (
        echo  [OK] Boot configurato con %core% core.
        echo [%DATE_NOW%] CPU Boot: %core% core >> "%LOG_FILE%"
    ) else (
        echo  [!] Errore nell'impostare i core. Verifica che il valore sia valido.
    )
)
pause
goto menu

:: =====================================================
:: SEZIONE 3 — EFFETTI VISIVI
:: =====================================================
:vis_fx
call :header
echo  --- EFFETTI VISIVI WINDOWS ---
echo.
echo  [1] Prestazioni massime (disabilita tutto)
echo  [2] Bilanciato (consigliato)
echo  [3] Aspetto migliore (tutto abilitato)
echo  [0] Torna al menu
echo.
set /p fx="  > Scegli: "

if "%fx%"=="0" goto menu
if "%fx%"=="1" (
    reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 2 /f >nul 2>&1
    powershell -NonInteractive -command "Set-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced' -Name 'ListviewAlphaSelect' -Value 0 -ErrorAction SilentlyContinue; Set-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced' -Name 'TaskbarAnimations' -Value 0 -ErrorAction SilentlyContinue" >nul 2>&1
    echo  [OK] Effetti visivi: Prestazioni Massime
    echo [%DATE_NOW%] Effetti visivi: Max Prestazioni >> "%LOG_FILE%"
)
if "%fx%"=="2" (
    reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 3 /f >nul 2>&1
    echo  [OK] Effetti visivi: Bilanciato
    echo [%DATE_NOW%] Effetti visivi: Bilanciato >> "%LOG_FILE%"
)
if "%fx%"=="3" (
    reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 0 /f >nul 2>&1
    echo  [OK] Effetti visivi: Aspetto Migliore
    echo [%DATE_NOW%] Effetti visivi: Aspetto Migliore >> "%LOG_FILE%"
)
pause
goto menu

:: =====================================================
:: SEZIONE 4 — NETWORK BOOST
:: =====================================================
:network_boost
call :header
echo  --- NETWORK BOOST - TCP + STACK ---
echo.
echo  [*] Reset IP e Winsock...
netsh int ip reset >nul 2>&1
if !errorlevel!==0 (echo  [OK] IP stack resettato.) else (echo  [!] Reset IP: gia' pulito o non necessario.)
netsh winsock reset >nul 2>&1
if !errorlevel!==0 (echo  [OK] Winsock resettato.) else (echo  [!] Winsock: gia' pulito o non necessario.)

echo  [*] Ottimizzazione parametri TCP...
netsh int tcp set global autotuninglevel=normal >nul 2>&1
netsh int tcp set global rss=enabled >nul 2>&1
netsh int tcp set global ecncapability=enabled >nul 2>&1
echo  [OK] Parametri TCP aggiornati.

echo  [*] Flush DNS e ARP...
ipconfig /flushdns >nul 2>&1
ipconfig /registerdns >nul 2>&1
arp -d * >nul 2>&1
echo  [OK] Cache DNS e ARP svuotate.

echo.
echo  [OK] Network Boost completato!
echo  [!] Riavvia il PC per applicare tutti i cambiamenti.
echo [%DATE_NOW%] Network Boost eseguito. >> "%LOG_FILE%"
pause
goto menu

:: =====================================================
:: SEZIONE 5 — SCANNER HARDWARE AVANZATO
:: =====================================================
:sys_info
call :header
echo  --- SCANNER HARDWARE AVANZATO ---
echo.
powershell -NonInteractive -command ^
  "try {" ^
  "  $cpu = Get-CimInstance Win32_Processor | Select-Object -First 1;" ^
  "  $os  = Get-CimInstance Win32_OperatingSystem;" ^
  "  $cs  = Get-CimInstance Win32_ComputerSystem;" ^
  "  $gpu = Get-CimInstance Win32_VideoController | Select-Object -First 1;" ^
  "  $ramTotGB  = [math]::Round($cs.TotalPhysicalMemory / 1GB, 2);" ^
  "  $ramFreeGB = [math]::Round($os.FreePhysicalMemory / 1MB, 2);" ^
  "  $ramUsedGB = [math]::Round($ramTotGB - $ramFreeGB, 2);" ^
  "  $uptime    = (Get-Date) - $os.LastBootUpTime;" ^
  "  Write-Host '  CPU        :' $cpu.Name -ForegroundColor Cyan;" ^
  "  Write-Host '  Core/Thread:' $cpu.NumberOfCores '/' $cpu.NumberOfLogicalProcessors -ForegroundColor Cyan;" ^
  "  Write-Host '  Velocita   :' $cpu.MaxClockSpeed 'MHz' -ForegroundColor Cyan;" ^
  "  Write-Host '';" ^
  "  Write-Host '  GPU        :' $gpu.Name -ForegroundColor Yellow;" ^
  "  Write-Host '';" ^
  "  Write-Host '  RAM Totale :' $ramTotGB 'GB' -ForegroundColor Green;" ^
  "  Write-Host '  RAM Usata  :' $ramUsedGB 'GB' -ForegroundColor Green;" ^
  "  Write-Host '  RAM Libera :' $ramFreeGB 'GB' -ForegroundColor Green;" ^
  "  Write-Host '';" ^
  "  Write-Host '  OS         :' $os.Caption $os.BuildNumber -ForegroundColor Magenta;" ^
  "  Write-Host '  Uptime     :' $uptime.Days 'giorni,' $uptime.Hours 'ore,' $uptime.Minutes 'min' -ForegroundColor Cyan;" ^
  "  Write-Host '';" ^
  "  Write-Host '  Dischi:' -ForegroundColor White;" ^
  "  try {" ^
  "    Get-PhysicalDisk | ForEach-Object { Write-Host '    -' $_.FriendlyName '|' $_.MediaType '|' ([math]::Round($_.Size/1GB,0)) 'GB' -ForegroundColor White }" ^
  "  } catch {" ^
  "    Get-CimInstance Win32_DiskDrive | ForEach-Object { Write-Host '    -' $_.Model '|' ([math]::Round($_.Size/1GB,0)) 'GB' -ForegroundColor White }" ^
  "  }" ^
  "} catch { Write-Host '[!] Errore durante la scansione:' $_.Exception.Message -ForegroundColor Red }"
echo.
pause
goto menu

:: =====================================================
:: SEZIONE 6 — PIANO DI ALIMENTAZIONE
:: =====================================================
:power_plan
call :header
echo  --- PIANO DI ALIMENTAZIONE ---
echo.
echo  Piano attuale:
powershell -NonInteractive -command "try { (Get-CimInstance -Namespace root\cimv2\power -ClassName Win32_PowerPlan -Filter 'IsActive=True').ElementName } catch { 'Non rilevato' }"
echo.
echo  [1] Prestazioni Elevate
echo  [2] Bilanciato (default Windows)
echo  [3] Risparmio Energetico
echo  [4] Massima Prestazione Assoluta (Ultimate)
echo  [0] Torna al menu
echo.
set /p pp="  > Scegli: "

if "%pp%"=="0" goto menu
if "%pp%"=="1" (
    powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c
    if !errorlevel!==0 (echo  [OK] Piano: Prestazioni Elevate) else (echo  [!] Piano non disponibile su questo sistema.)
    echo [%DATE_NOW%] Power Plan: Prestazioni Elevate >> "%LOG_FILE%"
)
if "%pp%"=="2" (
    powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e
    if !errorlevel!==0 (echo  [OK] Piano: Bilanciato) else (echo  [!] Piano non disponibile su questo sistema.)
    echo [%DATE_NOW%] Power Plan: Bilanciato >> "%LOG_FILE%"
)
if "%pp%"=="3" (
    powercfg /setactive a1841308-3541-4fab-bc81-f71556f20b4a
    if !errorlevel!==0 (echo  [OK] Piano: Risparmio Energetico) else (echo  [!] Piano non disponibile su questo sistema.)
    echo [%DATE_NOW%] Power Plan: Risparmio >> "%LOG_FILE%"
)
if "%pp%"=="4" (
    echo  [*] Attivazione piano Ultimate Performance...
    powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 >nul 2>&1
    set "ULTIMATE_GUID="
    for /f "tokens=4" %%G in ('powercfg /list 2^>nul ^| findstr /i "e9a42b02"') do set "ULTIMATE_GUID=%%G"
    if not "!ULTIMATE_GUID!"=="" (
        powercfg /setactive !ULTIMATE_GUID!
        if !errorlevel!==0 (echo  [OK] Piano Ultimate Performance attivato.) else (echo  [!] Errore nell'attivazione.)
    ) else (
        echo  [!] Piano Ultimate non disponibile su questo sistema.
        echo      Attivazione Prestazioni Elevate come alternativa...
        powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c >nul 2>&1
        echo  [OK] Piano: Prestazioni Elevate attivato.
    )
    echo [%DATE_NOW%] Power Plan: Ultimate >> "%LOG_FILE%"
)
pause
goto menu

:: =====================================================
:: SEZIONE 7 — OTTIMIZZAZIONE DISCO
:: =====================================================
:disk_opt
call :header
echo  --- OTTIMIZZAZIONE DISCO ---
echo.
echo  [*] Rilevamento tipo disco...
powershell -NonInteractive -command ^
  "try { Get-PhysicalDisk | ForEach-Object { Write-Host '  Disco:' $_.FriendlyName '- Tipo:' $_.MediaType } }" ^
  "catch { Get-CimInstance Win32_DiskDrive | ForEach-Object { Write-Host '  Disco:' $_.Model } }"
echo.
echo  [1] Ottimizza disco C: (Defrag HDD o TRIM SSD, automatico)
echo  [2] Pianifica controllo errori C: (chkdsk al prossimo avvio)
echo  [3] Comprimi OS disco C: (libera spazio, lento)
echo  [0] Torna al menu
echo.
set /p do_="  > Scegli: "

if "%do_%"=="0" goto menu

if "%do_%"=="1" (
    echo  [*] Ottimizzazione in corso... Attendere, puo' richiedere alcuni minuti.
    echo      Non chiudere questa finestra.
    echo.
    powershell -NonInteractive -command ^
      "try {" ^
      "  Optimize-Volume -DriveLetter C -Verbose -ErrorAction Stop" ^
      "  Write-Host '[OK] Ottimizzazione completata con successo.'" ^
      "} catch {" ^
      "  Write-Host '[!] Optimize-Volume non disponibile, uso defrag...';" ^
      "  $result = & defrag C: /U /V 2>&1;" ^
      "  $result | Select-Object -Last 5 | ForEach-Object { Write-Host $_ };" ^
      "  Write-Host '[OK] Defrag completato.'" ^
      "}"
    echo [%DATE_NOW%] Disco ottimizzato. >> "%LOG_FILE%"
)

if "%do_%"=="2" (
    echo  [*] Pianificazione chkdsk per il prossimo avvio...
    echo.
    :: chkdsk su C: non puo' girare mentre Windows e' attivo: si pianifica
    :: Risponde Y alla domanda "vuoi pianificarlo al prossimo riavvio?"
    echo Y | chkdsk C: /f /r >nul 2>&1
    fsutil dirty set C: >nul 2>&1
    echo  [OK] Controllo disco pianificato al prossimo riavvio.
    echo  [*] Riavvia il PC per eseguire chkdsk.
    echo [%DATE_NOW%] chkdsk pianificato. >> "%LOG_FILE%"
)

if "%do_%"=="3" (
    echo  [*] Compressione OS in corso... Puo' richiedere diversi minuti.
    echo      Non chiudere questa finestra.
    echo.
    compact /CompactOs:always
    if !errorlevel!==0 (
        echo  [OK] Compressione completata.
        echo [%DATE_NOW%] Disco compresso. >> "%LOG_FILE%"
    ) else (
        echo  [!] Compressione non riuscita o gia' attiva.
    )
)
pause
goto menu

:: =====================================================
:: SEZIONE 8 — SERVIZI INUTILI
:: =====================================================
:services
call :header
echo  --- GESTIONE SERVIZI NON ESSENZIALI ---
echo.
echo  [1] Disabilita servizi inutili (ottimizza RAM/CPU)
echo  [2] Ripristina servizi ai valori default
echo  [0] Torna al menu
echo.
echo  Servizi gestiti:
echo    - SysMain (SuperFetch)
echo    - DiagTrack (Telemetria Microsoft)
echo    - WSearch (Windows Search)
echo    - Fax
echo.
set /p sv="  > Scegli: "

if "%sv%"=="0" goto menu
if "%sv%"=="1" (
    echo  [*] Disabilitazione servizi in corso...
    for %%S in (SysMain DiagTrack Fax) do (
        sc query %%S >nul 2>&1
        if !errorlevel!==0 (
            sc config %%S start= disabled >nul 2>&1
            net stop %%S >nul 2>&1
            echo  [OK] %%S disabilitato.
        ) else (
            echo  [-] %%S non trovato, salto.
        )
    )
    sc config WSearch start= delayed-auto >nul 2>&1
    echo  [OK] WSearch impostato ad avvio ritardato.
    echo [%DATE_NOW%] Servizi inutili disabilitati. >> "%LOG_FILE%"
)
if "%sv%"=="2" (
    echo  [*] Ripristino servizi in corso...
    for %%S in (SysMain DiagTrack Fax) do (
        sc query %%S >nul 2>&1
        if !errorlevel!==0 (
            sc config %%S start= auto >nul 2>&1
            net start %%S >nul 2>&1
            echo  [OK] %%S ripristinato.
        ) else (
            echo  [-] %%S non trovato, salto.
        )
    )
     sc query WSearch >nul 2>&1
    if !errorlevel!==0 (
        sc config WSearch start= auto >nul 2>&1
        net start WSearch >nul 2>&1
        echo  [OK] WSearch ripristinato.
    ) else (
        echo  [-] WSearch non trovato, salto.
    )
    echo [%DATE_NOW%] Servizi ripristinati. >> "%LOG_FILE%"
)
pause
goto menu

:: =====================================================
:: SEZIONE 9 — PULIZIA PROFONDA MANUALE
:: =====================================================
:manual_clean
call :header
echo  --- PULIZIA PROFONDA MANUALE ---
echo.
echo  [1] Apri Pulizia Disco di Windows (GUI)
echo  [2] Analizza e pulisci componenti Windows (DISM)
echo  [3] Pulizia cache browser (Chrome/Edge/Firefox)
echo  [0] Torna al menu
echo.
set /p cl="  > Scegli: "

if "%cl%"=="0" goto menu

if "%cl%"=="1" (
    echo  [*] Apertura Pulizia Disco...
    cleanmgr /d C:
    if !errorlevel! neq 0 (
        echo  [!] cleanmgr non disponibile. Provo con Storage Sense...
        start ms-settings:storagesense
    )
)

if "%cl%"=="2" (
    echo  [*] Analisi componenti Windows in corso...
    echo      Questa operazione puo' richiedere diversi minuti.
    echo.
    DISM /Online /Cleanup-Image /AnalyzeComponentStore
    echo.
    echo  [*] Pulizia componenti obsoleti...
    DISM /Online /Cleanup-Image /StartComponentCleanup
    if !errorlevel!==0 (
        echo.
        echo  [OK] Pulizia DISM completata.
        echo [%DATE_NOW%] DISM cleanup eseguito. >> "%LOG_FILE%"
    ) else (
        echo.
        echo  [!] DISM ha riportato un errore. Provo riparazione immagine...
        DISM /Online /Cleanup-Image /RestoreHealth
        if !errorlevel!==0 (
            echo  [OK] Riparazione immagine completata.
        ) else (
            echo  [!] Impossibile completare. Prova a eseguire 'sfc /scannow' manualmente.
        )
    )
)

if "%cl%"=="3" (
    echo  [*] Pulizia cache browser in corso...
    :: Chrome
    set "CHROME_CACHE=%USERPROFILE%\AppData\Local\Google\Chrome\User Data\Default\Cache"
    if exist "!CHROME_CACHE!" (
        rd /s /q "!CHROME_CACHE!" >nul 2>&1
        echo  [OK] Cache Chrome eliminata.
    ) else (
        echo  [-] Chrome non trovato o gia' pulito.
    )
    :: Edge
    set "EDGE_CACHE=%USERPROFILE%\AppData\Local\Microsoft\Edge\User Data\Default\Cache"
    if exist "!EDGE_CACHE!" (
        rd /s /q "!EDGE_CACHE!" >nul 2>&1
        echo  [OK] Cache Edge eliminata.
    ) else (
        echo  [-] Edge non trovato o gia' pulito.
    )
    :: Firefox
    set "FF_CACHE=%USERPROFILE%\AppData\Local\Mozilla\Firefox\Profiles"
    if exist "!FF_CACHE!" (
        for /d %%P in ("!FF_CACHE!\*") do (
            if exist "%%P\cache2" rd /s /q "%%P\cache2" >nul 2>&1
        )
        echo  [OK] Cache Firefox eliminata.
    ) else (
        echo  [-] Firefox non trovato o gia' pulito.
    )
    echo [%DATE_NOW%] Cache browser eliminate. >> "%LOG_FILE%"
)
pause
goto menu

:: =====================================================
:: SEZIONE 10 — RAPPORTO PRESTAZIONI
:: =====================================================
:perf_report
call :header
echo  --- RAPPORTO PRESTAZIONI SISTEMA ---
echo.
echo  [*] Generazione rapporto in corso...
powershell -NonInteractive -command ^
  "try {" ^
  "  $cpu = Get-CimInstance Win32_Processor | Select-Object -First 1;" ^
  "  $os  = Get-CimInstance Win32_OperatingSystem;" ^
  "  $uptime = (Get-Date) - $os.LastBootUpTime;" ^
  "  $ramUsedPct = [math]::Round(100 - ($os.FreePhysicalMemory / $os.TotalVisibleMemorySize * 100), 1);" ^
  "  $cpuLoad = $cpu.LoadPercentage;" ^
  "  Write-Host '';" ^
  "  Write-Host '  Sistema    :' $os.Caption -ForegroundColor Magenta;" ^
  "  Write-Host '  CPU        :' $cpu.Name -ForegroundColor Cyan;" ^
  "  $cColor = if($cpuLoad -gt 80){'Red'} elseif($cpuLoad -gt 50){'Yellow'} else {'Green'};" ^
  "  Write-Host '  Carico CPU :' $cpuLoad '%25' -ForegroundColor $cColor;" ^
  "  $rColor = if($ramUsedPct -gt 85){'Red'} elseif($ramUsedPct -gt 60){'Yellow'} else {'Green'};" ^
  "  Write-Host '  RAM Usata  :' $ramUsedPct '%25' -ForegroundColor $rColor;" ^
  "  Write-Host '  RAM Libera :' ([math]::Round($os.FreePhysicalMemory/1MB,1)) 'GB' -ForegroundColor Green;" ^
  "  Write-Host '  Uptime     :' $uptime.Days 'giorni,' $uptime.Hours 'ore,' $uptime.Minutes 'min' -ForegroundColor Cyan;" ^
  "  Write-Host '';" ^
  "  $drives = Get-PSDrive -PSProvider FileSystem -ErrorAction SilentlyContinue | Where-Object { $_.Used -gt 0 };" ^
  "  foreach ($d in $drives) {" ^
  "    $pct = [math]::Round($d.Used / ($d.Used + $d.Free) * 100, 1);" ^
  "    $dColor = if($pct -gt 90){'Red'} elseif($pct -gt 70){'Yellow'} else {'Green'};" ^
  "    Write-Host ('  Disco ' + $d.Name + ':     ' + $pct + '%25 usato') -ForegroundColor $dColor" ^
  "  }" ^
  "} catch { Write-Host '[!] Errore:' $_.Exception.Message -ForegroundColor Red }"
echo.
echo  [*] Log salvato su Desktop\SpeedBooster_Log.txt
echo [%DATE_NOW%] Rapporto prestazioni generato. >> "%LOG_FILE%"
pause
goto menu

:: =====================================================
:: SEZIONE 12 — TWEAKS AVANZATI E SEGRETI
:: =====================================================
:advanced_tweaks
call :header
echo  --- TWEAKS AVANZATI E SEGRETI ---
echo.
echo  [1]  Modalita' Gaming Totale (Game Mode + HPET off + GameBar off)
echo  [2]  Disabilita Algoritmo di Nagle (riduce latenza di rete)
echo  [3]  Disabilita Core Parking (tutti i core sempre attivi)
echo  [4]  Timer di Precisione 0.5ms (riduce input lag)
echo  [5]  GPU Hardware Scheduling (HAGS - migliora gaming)
echo  [6]  Disabilita Telemetria Profonda (registry + task schedulati)
echo  [7]  Ottimizzazione SSD Avanzata (8.3, last access, ecc.)
echo  [8]  Priorita' App in Primo Piano (boost CPU)
echo  [9]  Disabilita Power Throttling (CPU sempre a piena potenza)
echo  [10] Ottimizza IRQ e Interrupt Affinity (gaming/audio)
echo  [R]  Ripristina tutti i tweaks ai valori default
echo  [0]  Torna al menu
echo.
set /p tw="  > Scegli: "

if "%tw%"=="0"  goto menu
if /i "%tw%"=="R" goto tweaks_restore
if "%tw%"=="1"  goto tw_gaming
if "%tw%"=="2"  goto tw_nagle
if "%tw%"=="3"  goto tw_coreparking
if "%tw%"=="4"  goto tw_timer
if "%tw%"=="5"  goto tw_hags
if "%tw%"=="6"  goto tw_telemetry
if "%tw%"=="7"  goto tw_ssd
if "%tw%"=="8"  goto tw_fgpriority
if "%tw%"=="9"  goto tw_powerthrottle
if "%tw%"=="10" goto tw_irq
echo  [!] Opzione non valida.
timeout /t 2 /nobreak >nul
goto advanced_tweaks

:tw_gaming
call :header
echo  --- GAMING MODE TOTALE ---
echo.
echo  [*] Attivazione Game Mode di Windows...
reg add "HKCU\Software\Microsoft\GameBar" /v AllowAutoGameMode /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKCU\Software\Microsoft\GameBar" /v AutoGameModeEnabled /t REG_DWORD /d 1 /f >nul 2>&1
echo  [OK] Game Mode attivata.
echo  [*] Disabilitazione Xbox Game Bar...
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\GameDVR" /v AppCaptureEnabled /t REG_DWORD /d 0 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /v AllowGameDVR /t REG_DWORD /d 0 /f >nul 2>&1
echo  [OK] Game Bar e DVR disabilitati.
echo  [*] Disabilitazione HPET...
bcdedit /deletevalue useplatformclock >nul 2>&1
bcdedit /set disabledynamictick yes >nul 2>&1
if !errorlevel!==0 (
    echo  [OK] HPET disabilitato.
) else (
    echo  [!] Errore nella configurazione HPET.
)
echo  [*] Fullscreen esclusivo forzato...
reg add "HKCU\System\GameConfigStore" /v GameDVR_FSEBehaviorMode /t REG_DWORD /d 2 /f >nul 2>&1
reg add "HKCU\System\GameConfigStore" /v GameDVR_HonorUserFSEBehaviorMode /t REG_DWORD /d 1 /f >nul 2>&1
echo  [OK] Fullscreen esclusivo attivato.
echo  [*] Priorita' GPU massima...
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d 8 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v Priority /t REG_DWORD /d 6 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f >nul 2>&1
echo  [OK] Priorita' GPU e CPU per gaming massimizzate.
echo [%DATE_NOW%] Gaming Mode totale attivato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_nagle
call :header
echo  --- DISABILITAZIONE ALGORITMO DI NAGLE ---
echo.
echo  L'algoritmo di Nagle raggruppa pacchetti TCP piccoli,
echo  aumentando la latenza nei giochi online e nelle app real-time.
echo.
echo  [*] Applicazione tweaks TCP/IP nel registro...
powershell -NonInteractive -command "try { $adapters=Get-ChildItem 'HKLM:\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces' -ErrorAction Stop; $count=0; foreach($a in $adapters){ Set-ItemProperty -Path $a.PSPath -Name 'TcpAckFrequency' -Value 1 -Type DWord -Force -ErrorAction SilentlyContinue; Set-ItemProperty -Path $a.PSPath -Name 'TcpNoDelay' -Value 1 -Type DWord -Force -ErrorAction SilentlyContinue; Set-ItemProperty -Path $a.PSPath -Name 'TCPDelAckTicks' -Value 0 -Type DWord -Force -ErrorAction SilentlyContinue; $count++ }; Write-Host '[OK] Nagle disabilitato su' $count 'interfacce.' } catch { Write-Host '[!] Errore:' $_.Exception.Message }"
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "MaxUserPort" /t REG_DWORD /d 65534 /f >nul 2>&1
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpTimedWaitDelay" /t REG_DWORD /d 30 /f >nul 2>&1
echo  [OK] Porte TCP massimizzate e tempo attesa ridotto.
echo [%DATE_NOW%] Nagle disabilitato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_coreparking
call :header
echo  --- DISABILITAZIONE CORE PARKING ---
echo.
echo  Il Core Parking mette in standby i core CPU non usati.
echo  Disabilitarlo mantiene tutti i core sempre pronti.
echo.
echo  [*] Disabilitazione Core Parking sul piano attivo...
powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 100 >nul 2>&1
powercfg /setdcvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 100 >nul 2>&1
powercfg /apply >nul 2>&1
echo  [OK] Core Parking disabilitato sul piano corrente.
echo  [*] Disabilitazione su tutti i piani esistenti...
powershell -NonInteractive -command "try { $out=powercfg /list 2>&1; $guids=$out|ForEach-Object{ if($_ -match '([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})'){ $matches[1] } }|Where-Object{$_}; $count=0; foreach($guid in $guids){ powercfg /setacvalueindex $guid SUB_PROCESSOR CPMINCORES 100 2>&1|Out-Null; powercfg /setdcvalueindex $guid SUB_PROCESSOR CPMINCORES 100 2>&1|Out-Null; $count++ }; Write-Host '[OK] Applicato a' $count 'piani di alimentazione.' -ForegroundColor Green } catch { Write-Host '[!] Errore piani multipli:' $_.Exception.Message -ForegroundColor Yellow }"
echo [%DATE_NOW%] Core Parking disabilitato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_timer
call :header
echo  --- TIMER DI PRECISIONE ---
echo.
echo  Windows usa di default un timer a 15.6ms.
echo  Portarlo a 1ms riduce l'input lag e migliora la reattivita'.
echo.
echo  [*] Impostazione timer via registro...
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v GlobalTimerResolutionRequests /t REG_DWORD /d 1 /f >nul 2>&1
if !errorlevel!==0 (echo  [OK] GlobalTimerResolutionRequests abilitato.) else (echo  [!] Registro non modificabile.)
bcdedit /set tscsyncpolicy Enhanced >nul 2>&1
if !errorlevel!==0 (echo  [OK] TSC Sync Policy: Enhanced.) else (echo  [!] TSC non modificabile.)
echo  [*] Impostazione via API Windows...
powershell -NonInteractive -command "try { Add-Type -TypeDefinition 'using System; using System.Runtime.InteropServices; public class WinTimer { [DllImport(\"winmm.dll\")] public static extern int timeBeginPeriod(int p); }' -ErrorAction Stop; [WinTimer]::timeBeginPeriod(1)|Out-Null; Write-Host '[OK] Timer impostato a 1ms via API (attivo fino al riavvio).' } catch { Write-Host '[!] API non disponibile:' $_.Exception.Message }"
echo [%DATE_NOW%] Timer precision applicato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_hags
call :header
echo  --- GPU HARDWARE SCHEDULING (HAGS) ---
echo.
echo  HAGS sposta la gestione della memoria GPU dalla CPU alla GPU,
echo  riducendo latenza e migliorando la stabilita' dei frame rate.
echo  Richiede GPU recente (NVIDIA 10xx+ / AMD RX 5000+) e Win10 2004+.
echo.
echo  [1] Abilita HAGS
echo  [2] Disabilita HAGS
echo  [0] Annulla
echo.
set /p hags="  > Scegli: "
if "%hags%"=="0" goto advanced_tweaks
if "%hags%"=="1" goto tw_hags_on
if "%hags%"=="2" goto tw_hags_off
goto advanced_tweaks
:tw_hags_on
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul 2>&1
echo  [OK] HAGS abilitato. Riavvia il PC per applicare.
echo [%DATE_NOW%] HAGS abilitato. >> "%LOG_FILE%"
pause
goto advanced_tweaks
:tw_hags_off
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 1 /f >nul 2>&1
echo  [OK] HAGS disabilitato. Riavvia il PC per applicare.
echo [%DATE_NOW%] HAGS disabilitato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_telemetry
call :header
echo  --- DISABILITAZIONE TELEMETRIA PROFONDA ---
echo.
echo  Disabilita task schedulati, servizi, registro e blocca host.
echo.
echo  [*] Disabilitazione task schedulati telemetria...
schtasks /Change /TN "\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Application Experience\ProgramDataUpdater" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Autochk\Proxy" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\Consolidator" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Feedback\Siuf\DmClient" /DISABLE >nul 2>&1
schtasks /Change /TN "\Microsoft\Windows\Windows Error Reporting\QueueReporting" /DISABLE >nul 2>&1
echo  [OK] Task telemetria disabilitati.
echo  [*] Stop servizi telemetria...
sc config DiagTrack start= disabled >nul 2>&1 & net stop DiagTrack >nul 2>&1
sc config dmwappushservice start= disabled >nul 2>&1 & net stop dmwappushservice >nul 2>&1
echo  [OK] Servizi telemetria fermati.
echo  [*] Chiavi registro...
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat" /v DisableInventory /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Policies\Microsoft\SQMClient\Windows" /v CEIPEnable /t REG_DWORD /d 0 /f >nul 2>&1
echo  [OK] Registro configurato.
echo  [*] Blocco server telemetria nel file hosts...
set "HOSTS=C:\Windows\System32\drivers\etc\hosts"
findstr /C:"vortex.data.microsoft.com" "%HOSTS%" >nul 2>&1
if !errorlevel! neq 0 echo 0.0.0.0 vortex.data.microsoft.com >> "%HOSTS%"
findstr /C:"watson.telemetry.microsoft.com" "%HOSTS%" >nul 2>&1
if !errorlevel! neq 0 echo 0.0.0.0 watson.telemetry.microsoft.com >> "%HOSTS%"
findstr /C:"telecommand.telemetry.microsoft.com" "%HOSTS%" >nul 2>&1
if !errorlevel! neq 0 echo 0.0.0.0 telecommand.telemetry.microsoft.com >> "%HOSTS%"
echo  [OK] Server telemetria bloccati.
echo [%DATE_NOW%] Telemetria profonda disabilitata. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_ssd
call :header
echo  --- OTTIMIZZAZIONE SSD AVANZATA ---
echo.
echo  [*] Disabilitazione nomi file 8.3 (velocizza NTFS)...
fsutil behavior set disable8dot3 1 >nul 2>&1
if !errorlevel!==0 (echo  [OK] Nomi 8.3 disabilitati.) else (echo  [!] Non modificabile o gia' disabilitato.)
echo  [*] Disabilitazione Last Access Time...
fsutil behavior set disablelastaccess 1 >nul 2>&1
if !errorlevel!==0 (echo  [OK] Last Access Time disabilitato.) else (echo  [!] Non modificabile.)
echo  [*] Ottimizzazione buffer NTFS...
fsutil behavior set memoryusage 2 >nul 2>&1
echo  [OK] Buffer NTFS massimizzato.
echo  [*] Verifica e attivazione TRIM...
powershell -NonInteractive -command "try { $t=fsutil behavior query DisableDeleteNotify 2>&1; if($t -match '= 0'){ Write-Host '[OK] TRIM attivo.' } else { fsutil behavior set DisableDeleteNotify 0|Out-Null; Write-Host '[OK] TRIM attivato.' } } catch { Write-Host '[!] Errore verifica TRIM.' }"
echo  [*] Rimozione indicizzazione disco C:...
powershell -NonInteractive -command "try { $d=Get-WmiObject -Class Win32_Volume -Filter 'DriveLetter=""C:""' -ErrorAction Stop; if($d){ $d.IndexingEnabled=$false; $d.Put()|Out-Null; Write-Host '[OK] Indicizzazione C: disabilitata.' } else { $d2=Get-CimInstance -Class Win32_Volume -Filter ('DriveLetter='+[char]34+'C:'+[char]34) -ErrorAction SilentlyContinue; if($d2){ Set-CimInstance -InputObject $d2 -Property @{IndexingEnabled=$false} -ErrorAction SilentlyContinue; Write-Host '[OK] Indicizzazione C: disabilitata (CIM).' } else { Write-Host '[!] Volume C: non trovato.' } } } catch { try { $d2=Get-CimInstance -Class Win32_Volume -Filter ('DriveLetter='+[char]34+'C:'+[char]34) -ErrorAction SilentlyContinue; if($d2){ Set-CimInstance -InputObject $d2 -Property @{IndexingEnabled=$false} -ErrorAction SilentlyContinue; Write-Host '[OK] Indicizzazione C: disabilitata (CIM).' } else { Write-Host '[!] Impossibile trovare il volume C:.' } } catch { Write-Host '[!] Errore:' $_.Exception.Message } }"
echo [%DATE_NOW%] SSD ottimizzato. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_fgpriority
call :header
echo  --- PRIORITA' APP IN PRIMO PIANO ---
echo.
echo  Aumenta la quota CPU assegnata all'app attiva.
echo  Utile per gaming, editing video, rendering.
echo.
echo  [1] Massima priorita' primo piano (gaming/editing)
echo  [2] Bilanciata (default Windows)
echo  [0] Annulla
echo.
set /p fg="  > Scegli: "
if "%fg%"=="0" goto advanced_tweaks
if "%fg%"=="1" goto tw_fg_max
if "%fg%"=="2" goto tw_fg_bal
goto advanced_tweaks
:tw_fg_max
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 38 /f >nul 2>&1
echo  [OK] Priorita' primo piano massimizzata.
echo [%DATE_NOW%] FG Priority: max >> "%LOG_FILE%"
pause
goto advanced_tweaks
:tw_fg_bal
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 2 /f >nul 2>&1
echo  [OK] Priorita' bilanciata ripristinata.
echo [%DATE_NOW%] FG Priority: bilanciata >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_powerthrottle
call :header
echo  --- DISABILITAZIONE POWER THROTTLING ---
echo.
echo  Power Throttling limita la frequenza CPU dei processi in background.
echo  Disabilitarlo mantiene la CPU sempre a piena potenza.
echo.
echo  [1] Disabilita Power Throttling (massime prestazioni)
echo  [2] Riabilita Power Throttling (default)
echo  [0] Annulla
echo.
set /p pt_val="  > Scegli: "
if "%pt_val%"=="0" goto advanced_tweaks
if "%pt_val%"=="1" goto tw_pt_off
if "%pt_val%"=="2" goto tw_pt_on
goto advanced_tweaks
:tw_pt_off
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v PowerThrottlingOff /t REG_DWORD /d 1 /f >nul 2>&1
powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR PERFAUTONOMOUS 0 >nul 2>&1
echo  [OK] Power Throttling disabilitato.
echo [%DATE_NOW%] Power Throttling: off >> "%LOG_FILE%"
pause
goto advanced_tweaks
:tw_pt_on
reg delete "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v PowerThrottlingOff /f >nul 2>&1
powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR PERFAUTONOMOUS 1 >nul 2>&1
echo  [OK] Power Throttling riabilitato.
echo [%DATE_NOW%] Power Throttling: on >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tw_irq
call :header
echo  --- OTTIMIZZAZIONE IRQ E INTERRUPT AFFINITY ---
echo.
echo  [*] Disabilitazione risparmio energetico schede di rete...
powershell -NonInteractive -command "try { Get-NetAdapter -Physical -ErrorAction Stop | Where-Object {$_.Status -eq 'Up'} | ForEach-Object { Disable-NetAdapterPowerManagement -Name $_.Name -ErrorAction SilentlyContinue; Write-Host '[OK] Power mgmt off su:' $_.Name } } catch { Write-Host '[!] Errore:' $_.Exception.Message }"
echo  [*] Interrupt Moderation off...
powershell -NonInteractive -command "Get-NetAdapter -Physical -ErrorAction SilentlyContinue | Where-Object {$_.Status -eq 'Up'} | ForEach-Object { Set-NetAdapterAdvancedProperty -Name $_.Name -DisplayName 'Interrupt Moderation' -DisplayValue 'Disabled' -ErrorAction SilentlyContinue; Write-Host '[OK] Interrupt Moderation off su:' $_.Name }"
echo  [*] GPU TDR Delay ottimizzato...
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v TdrDelay /t REG_DWORD /d 8 /f >nul 2>&1
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v TdrDdiDelay /t REG_DWORD /d 8 /f >nul 2>&1
echo  [OK] GPU TDR Delay: 8s.
echo  [*] Priorita' MMCSS audio...
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v SystemResponsiveness /t REG_DWORD /d 0 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v NetworkThrottlingIndex /t REG_DWORD /d 4294967295 /f >nul 2>&1
echo  [OK] MMCSS ottimizzato.
echo [%DATE_NOW%] IRQ e interrupt ottimizzati. >> "%LOG_FILE%"
pause
goto advanced_tweaks

:tweaks_restore
call :header
echo  --- RIPRISTINO TWEAKS AI VALORI DEFAULT ---
echo.
echo  [*] Ripristino Game Bar...
reg delete "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /f >nul 2>&1
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\GameDVR" /v AppCaptureEnabled /t REG_DWORD /d 1 /f >nul 2>&1
echo  [OK] Game Bar ripristinata.
echo  [*] Ripristino HPET...
bcdedit /deletevalue disabledynamictick >nul 2>&1
bcdedit /deletevalue tscsyncpolicy >nul 2>&1
if !errorlevel!==0 (
    echo  [OK] HPET ripristinato.
) else (
    echo  [!] Errore nel ripristino HPET.
)
echo  [*] Ripristino Nagle...
powershell -NonInteractive -command "Get-ChildItem 'HKLM:\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces' | ForEach-Object { Remove-ItemProperty -Path $_.PSPath -Name 'TcpAckFrequency' -ErrorAction SilentlyContinue; Remove-ItemProperty -Path $_.PSPath -Name 'TcpNoDelay' -ErrorAction SilentlyContinue; Remove-ItemProperty -Path $_.PSPath -Name 'TCPDelAckTicks' -ErrorAction SilentlyContinue }"
echo  [OK] Nagle ripristinato.
echo  [*] Ripristino Core Parking...
powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 0 >nul 2>&1
powercfg /apply >nul 2>&1
echo  [OK] Core Parking ripristinato.
echo  [*] Ripristino priorita' e power throttling...
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 2 /f >nul 2>&1
reg delete "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v PowerThrottlingOff /f >nul 2>&1
echo  [OK] Priorita' e Power Throttling ripristinati.
echo  [*] Ripristino HAGS e MMCSS...
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 1 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v SystemResponsiveness /t REG_DWORD /d 20 /f >nul 2>&1
reg add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v NetworkThrottlingIndex /t REG_DWORD /d 10 /f >nul 2>&1
echo  [OK] HAGS e MMCSS ripristinati.
echo  [*] Ripristino SSD...
fsutil behavior set disable8dot3 0 >nul 2>&1
fsutil behavior set disablelastaccess 0 >nul 2>&1
echo  [OK] Comportamento SSD ripristinato.
echo [%DATE_NOW%] Tweaks avanzati ripristinati. >> "%LOG_FILE%"
echo.
echo  [OK] Tutti i tweaks ripristinati. Riavvia il PC.
pause
goto menu

:: =====================================================
:: SEZIONE 13 — FUNZIONI SEGRETE E STRUMENTI PRO
:: =====================================================
:pro_tools
call :header
echo  --- FUNZIONI SEGRETE E STRUMENTI PRO ---
echo.
echo  [1]  Mostra password WiFi salvate nel PC
echo  [2]  Recupera Product Key di Windows
echo  [3]  Dashboard live CPU/RAM/DISCO in tempo reale
echo  [4]  Benchmark velocita' disco (lettura/scrittura)
echo  [5]  Scanner processi sospetti e malware
echo  [6]  Info segrete sistema (seriale, UUID, MAC, BIOS)
echo  [7]  Startup Manager (vedi e rimuovi avvii automatici)
echo  [8]  Mappa rete locale (tutti i dispositivi connessi)
echo  [9]  Analisi dipendenze DLL di un eseguibile
echo  [10] Genera report completo sistema (esportato su Desktop)
echo  [0]  Torna al menu
echo.
set /p pt_="  > Scegli: "

if "%pt_%"=="0"  goto menu
if "%pt_%"=="1"  goto pt_wifi
if "%pt_%"=="2"  goto pt_key
if "%pt_%"=="3"  goto pt_dashboard
if "%pt_%"=="4"  goto pt_bench
if "%pt_%"=="5"  goto pt_scanner
if "%pt_%"=="6"  goto pt_sysinfo
if "%pt_%"=="7"  goto pt_startup
if "%pt_%"=="8"  goto pt_netmap
if "%pt_%"=="9"  goto pt_dll
if "%pt_%"=="10" goto pt_report
echo  [!] Opzione non valida.
timeout /t 2 /nobreak >nul
goto pro_tools

:pt_wifi
call :header
echo  --- PASSWORD WiFi SALVATE ---
echo.
echo  [*] Elenco reti salvate...
echo.
set "TMPFILE=%TEMP%\wifi_profiles.txt"
netsh wlan show profiles > "%TMPFILE%" 2>nul
findstr /i "Profilo utente User Profile" "%TMPFILE%" >nul
if !errorlevel!==1 (
    echo  Nessuna rete WiFi trovata.
    del "%TMPFILE%" 2>nul
    pause
    goto pro_tools
)
echo  Reti disponibili:
echo.
for /f "tokens=2 delims=:" %%a in ('findstr /i "Profilo utente User Profile" "%TMPFILE%"') do (
    set "ssid=%%a"
    set "ssid=!ssid:~1!"
    echo    !ssid!
)
del "%TMPFILE%" 2>nul
echo.
echo  Per vedere la password di una rete, digita il nome ESATTO (0 per uscire):
set /p "scelta_ssid=  > "
if "!scelta_ssid!"=="0" goto pro_tools
if "!scelta_ssid!"=="" goto pro_tools
echo.
echo  Password per la rete "!scelta_ssid!":
netsh wlan show profile name="!scelta_ssid!" key=clear | findstr /i "Contenuto chiave Key Content"
if !errorlevel!==1 echo  Password non trovata o rete non esistente.
echo.
pause
goto pro_tools

:pt_key
call :header
echo  --- PRODUCT KEY WINDOWS ---
echo.
echo  [*] Recupero chiave di licenza (3 metodi in cascata)...
echo.
powershell -NonInteractive -command "try { $os=Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -EA SilentlyContinue; Write-Host '  Sistema  :' $os.ProductName -ForegroundColor White; Write-Host '  Edizione :' $os.EditionID -ForegroundColor Cyan; Write-Host '  Build    :' $os.CurrentBuild -ForegroundColor Cyan; Write-Host ''; $key=$null; $src=''; if(-not $key){ try{ $k=(Get-CimInstance -ClassName SoftwareLicensingService -EA Stop).OA3xOriginalProductKey; if($k -and $k.Trim() -ne ''){ $key=$k; $src='UEFI/BIOS OEM (metodo principale)' } }catch{} }; if(-not $key){ try{ $k=(Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform' -EA Stop).BackupProductKeyDefault; if($k -and $k.Trim() -ne ''){ $key=$k; $src='Registro di sistema (BackupProductKey)' } }catch{} }; if(-not $key){ try{ $rb=[byte[]]((Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -EA Stop).DigitalProductId[52..66]); $map='BCDFGHJKMPQRTVWXY2346789'; $res=''; for($i=24;$i -ge 0;$i--){ $r=0; for($j=14;$j -ge 0;$j--){ $r=$r*256+[int]$rb[$j]; $rb[$j]=[math]::Floor($r/24); $r=$r%24 }; $res=$map[$r]+$res; if($i%5 -eq 0 -and $i -ne 0){ $res='-'+$res } }; if($res -notmatch '^[B-Z2-9-]+$' -or $res.Length -ne 29){ throw 'chiave non valida' }; $key=$res; $src='DigitalProductId (metodo legacy)' }catch{} }; if($key){ Write-Host '  Product Key :' $key -ForegroundColor Yellow; Write-Host '  Trovata con :' $src -ForegroundColor Gray; Write-Host '' }else{ Write-Host '  [!] Chiave non leggibile via software.' -ForegroundColor Red; Write-Host '  Questo PC usa una licenza digitale collegata' -ForegroundColor Yellow; Write-Host '  al tuo account Microsoft (normale su Win10/11).' -ForegroundColor Yellow; Write-Host '  Non hai bisogno della chiave: la licenza e collegata' -ForegroundColor Yellow; Write-Host '  al tuo hardware e si riattiva automaticamente.' -ForegroundColor Yellow } } catch { Write-Host '[!] Errore:' $_.Exception.Message -ForegroundColor Red }"
echo.
echo  [!] Salva la chiave in un posto sicuro se trovata.
pause
goto pro_tools

:pt_dashboard
call :header
echo  --- DASHBOARD LIVE ---
echo  Premi CTRL+C per uscire.
echo.
timeout /t 2 /nobreak >nul
:live_loop
cls
echo  =====================================================
echo    DASHBOARD LIVE - SISTEMA IN TEMPO REALE
echo  =====================================================
powershell -NonInteractive -command "$cpu=(Get-CimInstance Win32_Processor|Select-Object -First 1);$os=Get-CimInstance Win32_OperatingSystem;$date=Get-Date -Format 'HH:mm:ss';$load=$cpu.LoadPercentage;$ramPct=[math]::Round(100-$os.FreePhysicalMemory/$os.TotalVisibleMemorySize*100,1);$ramFG=[math]::Round($os.FreePhysicalMemory/1MB,1);$bar={param($v,$w=30) $f=[math]::Floor($v/100*$w);'#'*$f+'-'*($w-$f)};$cBar=& $bar $load;$rBar=& $bar $ramPct;$cCol=if($load -gt 80){'Red'} elseif($load -gt 50){'Yellow'} else {'Green'};$rCol=if($ramPct -gt 85){'Red'} elseif($ramPct -gt 60){'Yellow'} else {'Green'};Write-Host '  Ora        :' $date -ForegroundColor White;Write-Host '';Write-Host '  CPU Usage  :' -NoNewline;Write-Host ('{0,3}%%  [{1}]' -f $load,$cBar) -ForegroundColor $cCol;Write-Host '  CPU Freq   :' $cpu.CurrentClockSpeed 'MHz' -ForegroundColor Cyan;Write-Host '';Write-Host '  RAM Usage  :' -NoNewline;Write-Host ('{0,4}%%  [{1}]' -f $ramPct,$rBar) -ForegroundColor $rCol;Write-Host '  RAM Libera :' $ramFG 'GB' -ForegroundColor Green;Write-Host '';Write-Host '  Processi   :' (Get-Process).Count -ForegroundColor White;Write-Host '  TOP 5 CPU  :' -ForegroundColor Yellow;Get-Process|Sort-Object CPU -Descending|Select-Object -First 5|ForEach-Object{Write-Host ('    {0,-22} CPU:{1,6}s  RAM:{2}MB' -f $_.Name,[math]::Round($_.CPU,1),[math]::Round($_.WorkingSet/1MB,0)) -ForegroundColor White};Write-Host '';Write-Host '  DISCHI:' -ForegroundColor Yellow;Get-PSDrive -PSProvider FileSystem -ErrorAction SilentlyContinue|Where-Object{$_.Used -gt 0}|ForEach-Object{$p=[math]::Round($_.Used/($_.Used+$_.Free)*100,1);$b='#'*[math]::Floor($p/5)+'-'*(20-[math]::Floor($p/5));Write-Host ('    {0}:  [{1}]  {2}%%' -f $_.Name,$b,$p) -ForegroundColor $(if($p -gt 90){'Red'} elseif($p -gt 70){'Yellow'} else {'Green'})}"
ping -n 2 -w 1000 127.0.0.1 >nul 2>&1
goto live_loop

:pt_bench
call :header
echo  --- BENCHMARK VELOCITA' DISCO ---
echo.
echo  [*] Scrittura e lettura file di test da 256MB su C:\...
echo      Attendere, potrebbe richiedere qualche secondo.
echo.
set "BENCH_PATH=C:\__speedtest_bench__.tmp"
powershell -NonInteractive -command "try { $bp='C:\__speedtest_bench__.tmp'; $size=256MB; $buf=New-Object byte[] $size; (New-Object Random).NextBytes($buf); $sw=[Diagnostics.Stopwatch]::StartNew(); [IO.File]::WriteAllBytes($bp,$buf); $sw.Stop(); $wSpeed=[math]::Round($size/1MB/$sw.Elapsed.TotalSeconds,1); Write-Host '  Scrittura   :' $wSpeed 'MB/s' -ForegroundColor Yellow; $sw2=[Diagnostics.Stopwatch]::StartNew(); [IO.File]::ReadAllBytes($bp)|Out-Null; $sw2.Stop(); $rSpeed=[math]::Round($size/1MB/$sw2.Elapsed.TotalSeconds,1); Write-Host '  Lettura     :' $rSpeed 'MB/s' -ForegroundColor Green; Remove-Item $bp -Force -ErrorAction SilentlyContinue; Write-Host ''; $rating=if($rSpeed -gt 3000){'NVMe Ultra'} elseif($rSpeed -gt 1500){'NVMe standard'} elseif($rSpeed -gt 400){'SSD SATA'} elseif($rSpeed -gt 80){'HDD veloce'} else {'HDD lento o quasi pieno'}; Write-Host '  Tipo stimato:' $rating -ForegroundColor Cyan; if($wSpeed -lt 50){Write-Host '  [!] Scrittura lenta: possibile frammentazione o disco quasi pieno.' -ForegroundColor Red}; if($rSpeed -gt 1500){Write-Host '  [OK] Velocita eccellente.' -ForegroundColor Green} } catch { Remove-Item 'C:\__speedtest_bench__.tmp' -Force -ErrorAction SilentlyContinue; Write-Host '[!] Errore benchmark:' $_.Exception.Message -ForegroundColor Red; Write-Host '    Verifica di avere spazio libero su C:\ e diritti di scrittura.' -ForegroundColor Yellow }"
echo [%DATE_NOW%] Benchmark disco eseguito. >> "%LOG_FILE%"
pause
goto pro_tools

:pt_scanner
call :header
echo  --- SCANNER PROCESSI SOSPETTI ---
echo.
echo  [*] Analisi processi in esecuzione...
echo.
powershell -NonInteractive -command "try { $sospetti=@(); $procs=Get-Process -ErrorAction SilentlyContinue|Where-Object{$_.Path}; Write-Host '  Processi analizzati:' $procs.Count -ForegroundColor Cyan; Write-Host ''; foreach($p in $procs){ $path=$p.Path.ToLower(); $flag=$false; $reason=''; if($path -like '*\temp\*'){$flag=$true;$reason='esegue da TEMP'}; if($path -like '*\appdata\roaming\*' -and $path -notlike '*\microsoft\*'){$flag=$true;$reason='esegue da AppData/Roaming'}; if($path -like '*\users\public\*'){$flag=$true;$reason='esegue da Users\Public'}; if($flag){$sospetti+=[pscustomobject]@{Nome=$p.Name;PID=$p.Id;Motivo=$reason;Percorso=$p.Path}} }; if($sospetti.Count -eq 0){ Write-Host '  [OK] Nessun processo sospetto rilevato.' -ForegroundColor Green } else { Write-Host '  [!] PROCESSI SOSPETTI:' $sospetti.Count -ForegroundColor Red; Write-Host ''; foreach($s in $sospetti){ Write-Host '  Nome   :' $s.Nome -ForegroundColor Red; Write-Host '  PID    :' $s.PID -ForegroundColor Yellow; Write-Host '  Motivo :' $s.Motivo -ForegroundColor Yellow; Write-Host '  Path   :' $s.Percorso -ForegroundColor White; Write-Host '' } }; Write-Host '  Connessioni TCP attive:' -ForegroundColor Cyan; Get-NetTCPConnection -State Established -ErrorAction SilentlyContinue|Select-Object -First 8|ForEach-Object{$proc=(Get-Process -Id $_.OwningProcess -ErrorAction SilentlyContinue).Name; Write-Host ('  {0,-20} -> {1}:{2}' -f $proc,$_.RemoteAddress,$_.RemotePort) -ForegroundColor White} } catch { Write-Host '[!] Errore:' $_.Exception.Message -ForegroundColor Red }"
echo [%DATE_NOW%] Scansione processi eseguita. >> "%LOG_FILE%"
pause
goto pro_tools

:pt_sysinfo
call :header
echo  --- INFO SEGRETE SISTEMA ---
echo.
powershell -NonInteractive -command "try { $bios=Get-CimInstance Win32_BIOS;$prod=Get-CimInstance Win32_ComputerSystemProduct;$mb=Get-CimInstance Win32_BaseBoard; Write-Host '  --- IDENTITA MACCHINA ---' -ForegroundColor Yellow; Write-Host '  Seriale PC  :' $prod.IdentifyingNumber -ForegroundColor Cyan; Write-Host '  UUID        :' $prod.UUID -ForegroundColor Cyan; Write-Host '  Produttore  :' $prod.Vendor -ForegroundColor White; Write-Host '  Modello     :' $prod.Name -ForegroundColor White; Write-Host ''; Write-Host '  --- BIOS ---' -ForegroundColor Yellow; Write-Host '  Versione    :' $bios.SMBIOSBIOSVersion -ForegroundColor Cyan; Write-Host '  Data BIOS   :' $bios.ReleaseDate -ForegroundColor White; Write-Host '  Produttore  :' $bios.Manufacturer -ForegroundColor White; Write-Host '  Seriale BIOS:' $bios.SerialNumber -ForegroundColor Cyan; Write-Host ''; Write-Host '  --- SCHEDA MADRE ---' -ForegroundColor Yellow; Write-Host '  Modello     :' $mb.Product -ForegroundColor White; Write-Host '  Seriale     :' $mb.SerialNumber -ForegroundColor Cyan; Write-Host ''; Write-Host '  --- RETE ---' -ForegroundColor Yellow; Get-NetAdapter -Physical -ErrorAction SilentlyContinue|Where-Object{$_.Status -eq 'Up'}|ForEach-Object{Write-Host ('  {0,-15}: MAC {1}  Speed {2}' -f $_.Name,$_.MacAddress,$_.LinkSpeed) -ForegroundColor Green}; Write-Host ''; Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue|Where-Object{$_.IPAddress -ne '127.0.0.1'}|ForEach-Object{Write-Host '  ' $_.InterfaceAlias ':' $_.IPAddress -ForegroundColor Green}; $pubIP=try{(Invoke-WebRequest 'https://api.ipify.org' -UseBasicParsing -TimeoutSec 4).Content}catch{'non disponibile'}; Write-Host '  IP Pubblico :' $pubIP -ForegroundColor Magenta; Write-Host ''; Write-Host '  --- SICUREZZA ---' -ForegroundColor Yellow; $wd=try{(Get-MpComputerStatus -ErrorAction Stop).RealTimeProtectionEnabled}catch{'N/D'}; Write-Host '  Defender RT :' $wd -ForegroundColor $(if($wd -eq $true){'Green'} elseif($wd -eq $false){'Red'} else {'Yellow'}); $fw=(Get-NetFirewallProfile -Profile Public -ErrorAction SilentlyContinue).Enabled; Write-Host '  Firewall    :' $fw -ForegroundColor $(if($fw){'Green'} else {'Red'}) } catch { Write-Host '[!] Errore:' $_.Exception.Message -ForegroundColor Red }"
echo [%DATE_NOW%] Info sistema estratte. >> "%LOG_FILE%"
pause
goto pro_tools

:pt_startup
call :header
echo  --- STARTUP MANAGER ---
echo.
echo  [*] Programmi di avvio automatico...
echo.
powershell -NonInteractive -command "try { $regs=@('HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run','HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run','HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce','HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce'); $i=1; foreach($reg in $regs){ if(Test-Path $reg -ErrorAction SilentlyContinue){ $src=if($reg -like 'HKCU*'){'[USR]'} else {'[SYS]'}; $props=(Get-Item $reg -ErrorAction SilentlyContinue).Property; if($props){ foreach($name in $props){ $val=(Get-ItemProperty $reg -ErrorAction SilentlyContinue).$name; Write-Host ('['+$i+'] '+$src+' '+$name) -ForegroundColor Cyan; Write-Host '     ' $val -ForegroundColor White; $i++ } } } }; if($i -eq 1){Write-Host '  Nessun avvio automatico trovato nel registro.' -ForegroundColor Yellow}; Write-Host ''; Write-Host '  Task schedulati attivi (non Microsoft):' -ForegroundColor Yellow; $tasks=Get-ScheduledTask -ErrorAction SilentlyContinue|Where-Object{$_.State -eq 'Ready' -and $_.TaskPath -notlike '\Microsoft\*'}|Select-Object -First 15; if($tasks){$tasks|ForEach-Object{Write-Host ('  - {0,-35} {1}' -f $_.TaskName,$_.TaskPath) -ForegroundColor White}}else{Write-Host '  Nessun task di terze parti attivo.' -ForegroundColor Gray} } catch { Write-Host '[!] Errore:' $_.Exception.Message -ForegroundColor Red }"
echo.
set "rm_="
set /p rm_="  > Nome ESATTO da rimuovere dal registro Run (Invio=salta): "
if "!rm_!"=="" goto pt_startup_done
reg delete "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" /v "!rm_!" /f >nul 2>&1
reg delete "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" /v "!rm_!" /f >nul 2>&1
reg delete "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce" /v "!rm_!" /f >nul 2>&1
reg delete "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce" /v "!rm_!" /f >nul 2>&1
echo  [OK] Voce '!rm_!' rimossa da tutti i Run keys (se esisteva).
echo [%DATE_NOW%] Startup rimosso: !rm_! >> "%LOG_FILE%"
:pt_startup_done
pause
goto pro_tools


:pt_netmap
call :header
echo  --- MAPPA RETE LOCALE ---
echo.
echo  [*] Rilevamento gateway e dispositivi connessi...
echo.

:: Trova il gateway con ipconfig (metodo robusto)
set "GATEWAY="
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /i "Gateway predefinito Default Gateway"') do (
    set "GATEWAY=%%a"
    set "GATEWAY=!GATEWAY: =!"
)
if "!GATEWAY!"=="" (
    echo  [!] Gateway non trovato. Sei connesso a una rete?
    echo  [*] Verifica la connessione di rete.
    pause
    goto pro_tools
)
echo  Gateway rilevato: !GATEWAY!
echo.

:: Mostra i dispositivi dalla cache ARP (già attivi)
echo  [*] Dispositivi nella cache ARP (connessi recentemente):
echo  ----------------------------------------
arp -a
echo  ----------------------------------------
echo.

:: Scansione Ping con batch (compatibile con tutte le versioni di Windows)
echo  [*] Scansione rete in corso... (attendere circa 30 secondi)
echo  ^(potrebbero non rispondere tutti i dispositivi a causa del firewall^)
echo.

:: Estrai subnet dal gateway
for /f "tokens=1-3 delims=." %%a in ("!GATEWAY!") do set "SUBNET=%%a.%%b.%%c"

set "FOUND=0"
echo  Host attivi:
echo  ----------------------------------------
for /l %%i in (1,1,254) do (
    set "IP=!SUBNET!.%%i"
    ping -n 1 -w 500 !IP! >nul 2>&1
    if !errorlevel!==0 (
        echo  [+] !IP!
        set /a FOUND+=1
    )
)
echo  ----------------------------------------
echo.
echo  Host attivi trovati: !FOUND!

echo.
echo  [*] Mappa rete completata.
echo [%DATE_NOW%] Mappa rete eseguita. >> "%LOG_FILE%"
pause
goto pro_tools

:pt_dll
call :header
echo  --- ANALISI DIPENDENZE DLL ---
echo.
set "exe_path="
set /p exe_path="  > Trascina o incolla il percorso dell'eseguibile (.exe): "
if "!exe_path!"=="" goto pro_tools
set "exe_path=!exe_path:"=!"
if "!exe_path!"=="" goto pro_tools
if not exist "!exe_path!" (
    echo  [!] File non trovato: !exe_path!
    echo  [*] Suggerimento: usa le virgolette se il percorso ha spazi.
    pause
    goto pro_tools
)
echo.
echo  [*] Analisi: !exe_path!
echo.

:: Verifica che il file dll_analyzer.ps1 esista
if not exist "%~dp0dll_analyzer.ps1" (
    echo  [!] File dll_analyzer.ps1 non trovato nella cartella dello script.
    echo  [*] Assicurati che sia presente.
    pause
    goto pro_tools
)

:: Esegui lo script
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0dll_analyzer.ps1" -Path "!exe_path!"

echo.
echo [%DATE_NOW%] Analisi DLL eseguita su: !exe_path! >> "%LOG_FILE%"
pause
goto pro_tools

:pt_report
call :header
echo  --- REPORT COMPLETO SISTEMA ---
echo.
set "REPORT=%USERPROFILE%\Desktop\SystemReport_%date:~-4%%date:~3,2%%date:~0,2%.txt"
echo  [*] Generazione report completo... attendere.
echo.
(
echo =====================================================
echo  REPORT COMPLETO DI SISTEMA - %date% %time%
echo =====================================================
echo.
echo [SISTEMA OPERATIVO]
powershell -NonInteractive -command "$os=Get-CimInstance Win32_OperatingSystem; 'OS: '+$os.Caption; 'Build: '+$os.BuildNumber; 'Architettura: '+$os.OSArchitecture"
echo.
echo [CPU]
powershell -NonInteractive -command "$c=Get-CimInstance Win32_Processor|Select-Object -First 1; 'Nome: '+$c.Name; 'Core/Thread: '+$c.NumberOfCores+' / '+$c.NumberOfLogicalProcessors; 'Max GHz: '+[math]::Round($c.MaxClockSpeed/1000,2)"
echo.
echo [MEMORIA RAM]
powershell -NonInteractive -command "Get-CimInstance Win32_PhysicalMemory|ForEach-Object{ 'Banco: '+$_.DeviceLocator+' | '+[math]::Round($_.Capacity/1GB,0)+'GB | '+$_.Speed+'MHz | '+$_.Manufacturer }"
echo.
echo [GPU]
powershell -NonInteractive -command "Get-CimInstance Win32_VideoController|ForEach-Object{ 'GPU: '+$_.Name; 'Driver: '+$_.DriverVersion }"
echo.
echo [STORAGE]
powershell -NonInteractive -command "Get-CimInstance Win32_DiskDrive|ForEach-Object{ 'Disco: '+$_.Model+' | '+[math]::Round($_.Size/1GB,0)+'GB | '+$_.InterfaceType }"
echo.
echo [RETE]
ipconfig /all
echo.
echo [PROGRAMMI INSTALLATI]
powershell -NonInteractive -command "Get-ItemProperty 'HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*'|Select-Object DisplayName,DisplayVersion,Publisher|Where-Object{$_.DisplayName}|Sort-Object DisplayName|Format-Table -AutoSize|Out-String"
echo.
echo [AVVII AUTOMATICI]
powershell -NonInteractive -command "$regs=@('HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run','HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run'); foreach($r in $regs){if(Test-Path $r -EA SilentlyContinue){(Get-Item $r -EA SilentlyContinue).Property|ForEach-Object{ $_+' : '+(Get-ItemProperty $r -EA SilentlyContinue).$_ }}}"
echo.
echo =====================================================
echo  FINE REPORT
echo =====================================================
) > "%REPORT%" 2>&1
echo  [OK] Report salvato: %REPORT%
echo  [*] Apertura in Notepad...
start notepad "%REPORT%"
echo [%DATE_NOW%] Report completo generato. >> "%LOG_FILE%"
pause
goto pro_tools

:: =====================================================
:: SEZIONE 14 — ASSISTENTE IA (dati PC automatici)
:: =====================================================
:: =====================================================
:: SEZIONE 14 — ASSISTENTE IA (con tutorial)
:: =====================================================
:ai_assistant
echo.
echo  =====================================================
echo    ASSISTENTE IA - CONFIGURAZIONE RICHIESTA
echo  =====================================================
echo.
echo  Per utilizzare l'assistente IA e' necessario disporre
echo  di una chiave API gratuita di Groq.
echo.
echo  COME OTTENERLA:
echo  1. Vai su https://console.groq.com e registrati (gratis)
echo  2. Crea una nuova API Key (inizia con gsk_...)
echo  3. Apri il file ask_ai.ps1 nella cartella dello script
echo  4. Sostituisci "gsk_tuo-token-qui" con la tua chiave
echo.
set /p cont="  > Continuare? (S/N): "
if /i not "!cont!"=="S" (
    echo.
    echo  [*] Operazione annullata. Torno al menu...
    timeout /t 2 /nobreak >nul
    goto menu
)
:: =====================================================
:: CODICE ESISTENTE (invariato)
:: =====================================================
call :header
if not exist "%~dp0ask_ai.ps1" (
    echo  [!] File ask_ai.ps1 non trovato nella cartella dello script.
    echo  [*] Assicurati che sia presente nella stessa cartella di Windows_Speed_Booster.bat
    echo  [*] Oppure rimuovi l'opzione 14 dal menu.
    pause
    goto menu
)
echo  --- ASSISTENTE IA (con dati PC) ---
echo.
echo  Chat attiva. Scrivi 'exit' per tornare al menu principale.
echo.
:ai_loop
set /p "query=  > Domanda: "
if /i "!query!"=="exit" goto menu
if "!query!"=="" goto ai_loop
echo.
echo  [*] IA sta pensando...
powershell -ExecutionPolicy Bypass -File "%~dp0ask_ai.ps1" -query "!query!"
echo.
goto ai_loop

:: =====================================================
:: SEZIONE 15 — MICROSOFT ACTIVATION SCRIPTS (MAS)
:: =====================================================
:mas_activate
call :header
echo  --- MICROSOFT ACTIVATION SCRIPTS (MAS) ---
echo.
echo  Fonte ufficiale: https://github.com/massgravel/Microsoft-Activation-Scripts
echo  Script open source - nessun virus, nessun malware.
echo.
echo  Attiva: Windows 10/11, Windows Server, Office (tutti gli anni)
echo  Metodi: HWID (permanente), KMS38, Online KMS
echo.
echo  [*] Avvio MAS in una finestra separata...
echo      Quando esci da MAS, quella finestra si chiude da sola
echo      e tu torni automaticamente qui al menu.
echo.
echo  [*] Attendere l'apertura di MAS...
start /wait powershell -ExecutionPolicy Bypass -NoExit -Command "irm https://get.activated.win | iex"

:: ============ INIZIO CODICE AGGIUNTO (CONTROLLO ERRORE) ============
if !errorlevel! neq 0 (
    echo  [!] Errore durante l'esecuzione di MAS.
    echo  [*] Verifica la connessione internet e riprova.
    pause
    goto menu
)
echo  [OK] MAS eseguito correttamente.
:: ============ FINE CODICE AGGIUNTO ============

echo.
echo  [OK] MAS chiuso. Premi un tasto per tornare al menu principale.
echo [%DATE_NOW%] MAS eseguito. >> "%LOG_FILE%"
pause
goto menu

:: SEZIONE 11 — MATRIX MODE
:: =====================================================
:matrix
cls
color 02
echo.
echo  Premi CTRL+C per uscire dal Matrix Mode...
timeout /t 2 /nobreak >nul
cls
:matrix_loop
set "line="
for /l %%i in (1,1,8) do (
    set /a "r=!random! %% 10"
    set "line=!line!!r! "
)
echo !line!
ping -n 1 -w 30 127.0.0.1 >nul 2>&1
goto matrix_loop

:: =====================================================
:: EXIT
:: =====================================================
:exit_script
cls
color 0A
echo.
echo  Speed Booster V%VERSION% completato!
echo  Log salvato su: Desktop\SpeedBooster_Log.txt
echo.
echo  Arrivederci!
echo.
echo [%DATE_NOW%] Sessione terminata. >> "%LOG_FILE%"
timeout /t 3 /nobreak >nul
exit

:: =====================================================
:: FUNZIONE: HEADER
:: =====================================================
:header
cls
color 0A
echo.
echo  =====================================================
echo    WINDOWS SPEED BOOSTER V%VERSION%
echo  =====================================================
echo.
goto :eof
