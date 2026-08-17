using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsSpeedBooster
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (LanguageSelector selector = new LanguageSelector())
            {
                if (selector.ShowDialog() != DialogResult.OK) return;
                AppText.UseEnglish = selector.UseEnglish;
            }
            using (StartupForm startup = new StartupForm())
            {
                if (startup.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(startup.AppDirectory));
                }
            }
        }
    }

    internal static class AppText
    {
        private static readonly Dictionary<string, string> English = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "Avvio protocollo di pulizia profonda...", "Starting deep-clean protocol..." },
            { "Pulizia iniziale in corso...", "Initial cleanup in progress..." },
            { "Pulizia iniziale completata. Apertura menu...", "Initial cleanup completed. Opening menu..." },
            { "PRONTO ALL'OTTIMIZZAZIONE", "READY FOR OPTIMIZATION" },
            { "Scegli un modulo per aprire le relative impostazioni. Il pannello operativo e il log restano sempre disponibili.", "Choose a module to open its settings. The operations panel and log remain available at all times." },
            { "CONSOLE AVANZATA  /  LOG IN TEMPO REALE", "ADVANCED CONSOLE  /  LIVE LOG" },
            { "Sistema pronto · seleziona un modulo", "System ready · select a module" },
            { "■  INTERROMPI OPERAZIONE", "■  STOP OPERATION" },
            { "MENU DI OTTIMIZZAZIONE V14.0", "OPTIMIZATION MENU V14.0" },
            { "15 moduli di ottimizzazione. Passa il mouse sui moduli e scegli la funzione da applicare.", "15 optimization modules. Hover over a module and choose the function to apply." },
            { "CENTRO DI CONTROLLO", "CONTROL CENTER" },
            { "Tutti i tweak del menu originale sono disponibili qui. Seleziona un modulo per procedere.", "All tweaks from the original menu are available here. Select a module to continue." },
            { "Modulo selezionato. Le opzioni e l'output dell'operazione appariranno qui senza chiudere l'applicazione.", "Module selected. Its options and operation output will appear here without closing the application." },
            { "RICERCA GLOBALE DELLE FUNZIONI", "GLOBAL FUNCTION SEARCH" },
            { "RICERCA FUNZIONI", "FUNCTION SEARCH" },
            { "Apri il risultato desiderato: l’app ti porterà direttamente alla funzione o alla relativa schermata operativa.", "Open the desired result: the app will take you directly to the function or its operating screen." },
            { "Nessuna funzione corrisponde alla ricerca", "No function matches the search" },
            { "Torna al menu", "Back to menu" },
            { "Memoria Virtuale", "Virtual Memory" },
            { "Memoria Virtuale (Calcolo Automatico)", "Virtual Memory (Automatic Calculation)" },
            { "Gestione Core CPU", "CPU Core Management" },
            { "Gestione Core CPU (Boot)", "CPU Core Management (Boot)" },
            { "Effetti Visivi", "Visual Effects" },
            { "Network Boost (TCP + Reset Stack)", "Network Boost (TCP + Reset Stack)" },
            { "Scanner Hardware", "Hardware Scanner" },
            { "Scanner Hardware Avanzato", "Advanced Hardware Scanner" },
            { "Piano di Alimentazione", "Power Plan" },
            { "Ottimizzazione Disco", "Disk Optimization" },
            { "Ottimizzazione Disco (HDD/SSD)", "Disk Optimization (HDD/SSD)" },
            { "Servizi Inutili", "Unnecessary Services" },
            { "Servizi Inutili (Disabilita/Ripristina)", "Unnecessary Services (Disable/Restore)" },
            { "Pulizia Profonda", "Deep Cleanup" },
            { "Pulizia Profonda (Manuale)", "Deep Cleanup (Manual)" },
            { "Rapporto Prestazioni", "Performance Report" },
            { "Rapporto Prestazioni Sistema", "System Performance Report" },
            { "Tweaks Avanzati", "Advanced Tweaks" },
            { "Tweaks Avanzati e Segreti", "Advanced and Secret Tweaks" },
            { "Strumenti Pro", "Pro Tools" },
            { "Funzioni Segrete e Strumenti Pro", "Secret Features and Pro Tools" },
            { "Assistente IA", "AI Assistant" },
            { "Assistente IA (richiede token Groq gratuito)", "AI Assistant (requires a free Groq token)" },
            { "Microsoft Activation Scripts (MAS) - Attivazione Windows/Office", "Microsoft Activation Scripts (MAS) - Windows/Office Activation" },
            { "ESCI", "EXIT" },
            { "MEMORIA VIRTUALE (PAGING)", "VIRTUAL MEMORY (PAGING)" },
            { "Calcolo valori ottimali: RAM fisica e valore massimo consigliato. Il comportamento corrisponde alle tre scelte del batch.", "Calculates optimal values: physical RAM and the recommended maximum. Behavior matches the three batch choices." },
            { "Imposta valori consigliati automaticamente", "Set recommended values automatically" },
            { "Inserisci valori manualmente", "Enter values manually" },
            { "Ripristina gestione automatica Windows", "Restore automatic Windows management" },
            { "GESTIONE CORE CPU (BOOT)", "CPU CORE MANAGEMENT (BOOT)" },
            { "Core logici disponibili: ", "Available logical cores: " },
            { ". Modificare il valore può rallentare il boot. Lascia 0 per usare tutti i core (consigliato).", ". Changing this value may slow down boot. Leave 0 to use all cores (recommended)." },
            { "Core da allocare al boot (0=Auto/Tutti, M=Menu)", "Cores to allocate at boot (0=Auto/All, M=Menu)" },
            { "EFFETTI VISIVI WINDOWS", "WINDOWS VISUAL EFFECTS" },
            { "Seleziona il profilo effetti visivi corrispondente al menu originale.", "Select the visual-effects profile matching the original menu." },
            { "Prestazioni massime (disabilita tutto)", "Best performance (disable all)" },
            { "Bilanciato (consigliato)", "Balanced (recommended)" },
            { "Aspetto migliore (tutto abilitato)", "Best appearance (enable all)" },
            { "Piano attuale e le quattro scelte del batch.", "Current plan and the four batch choices." },
            { "Prestazioni Elevate", "High Performance" },
            { "Bilanciato (default Windows)", "Balanced (Windows default)" },
            { "Risparmio Energetico", "Power Saver" },
            { "Massima Prestazione Assoluta (Ultimate)", "Ultimate Performance" },
            { "Rilevamento tipo disco e le tre opzioni per l’unità C:.", "Disk-type detection and the three options for drive C:." },
            { "Ottimizza disco C: (Defrag HDD o TRIM SSD, automatico)", "Optimize drive C: (HDD defrag or SSD TRIM, automatic)" },
            { "Pianifica controllo errori C: (chkdsk al prossimo avvio)", "Schedule drive C: error check (chkdsk at next startup)" },
            { "Comprimi OS disco C: (libera spazio, lento)", "Compact OS on drive C: (frees space, slow)" },
            { "GESTIONE SERVIZI NON ESSENZIALI", "NON-ESSENTIAL SERVICES MANAGEMENT" },
            { "Servizi gestiti: SysMain, DiagTrack, WSearch e Fax.", "Managed services: SysMain, DiagTrack, WSearch, and Fax." },
            { "Disabilita servizi inutili (ottimizza RAM/CPU)", "Disable unnecessary services (optimizes RAM/CPU)" },
            { "Ripristina servizi ai valori default", "Restore services to default values" },
            { "PULIZIA PROFONDA MANUALE", "MANUAL DEEP CLEANUP" },
            { "Le stesse tre opzioni del batch per Pulizia disco, componenti Windows e cache browser.", "The same three batch options for Disk Cleanup, Windows components, and browser cache." },
            { "Apri Pulizia Disco di Windows (GUI)", "Open Windows Disk Cleanup (GUI)" },
            { "Analizza e pulisci componenti Windows (DISM)", "Analyze and clean Windows components (DISM)" },
            { "Pulizia cache browser (Chrome/Edge/Firefox)", "Clean browser cache (Chrome/Edge/Firefox)" },
            { "TWEAKS AVANZATI E SEGRETI", "ADVANCED AND SECRET TWEAKS" },
            { "Le dieci opzioni avanzate e il ripristino sono conservati con la stessa numerazione.", "The ten advanced options and restore action keep the original numbering." },
            { "Modalita' Gaming Totale (Game Mode + HPET off + GameBar off)", "Total Gaming Mode (Game Mode + HPET off + GameBar off)" },
            { "Disabilita Algoritmo di Nagle (riduce latenza di rete)", "Disable Nagle Algorithm (reduces network latency)" },
            { "Disabilita Core Parking (tutti i core sempre attivi)", "Disable Core Parking (all cores always active)" },
            { "Timer di Precisione 0.5ms (riduce input lag)", "0.5ms Precision Timer (reduces input lag)" },
            { "GPU Hardware Scheduling (HAGS - migliora gaming)", "GPU Hardware Scheduling (HAGS - improves gaming)" },
            { "Disabilita Telemetria Profonda (registry + task schedulati)", "Disable Deep Telemetry (registry + scheduled tasks)" },
            { "Ottimizzazione SSD Avanzata (8.3, last access, ecc.)", "Advanced SSD Optimization (8.3, last access, etc.)" },
            { "Priorita' App in Primo Piano (boost CPU)", "Foreground App Priority (CPU boost)" },
            { "Disabilita Power Throttling (CPU sempre a piena potenza)", "Disable Power Throttling (CPU always at full power)" },
            { "Ottimizza IRQ e Interrupt Affinity (gaming/audio)", "Optimize IRQ and Interrupt Affinity (gaming/audio)" },
            { "Ripristina tutti i tweaks ai valori default", "Restore all tweaks to default values" },
            { "GPU HARDWARE SCHEDULING (HAGS)", "GPU HARDWARE SCHEDULING (HAGS)" },
            { "HAGS sposta la gestione della memoria GPU dalla CPU alla GPU. Richiede GPU recente e riavvio.", "HAGS moves GPU memory management from the CPU to the GPU. It requires a recent GPU and restart." },
            { "Abilita HAGS", "Enable HAGS" },
            { "Disabilita HAGS", "Disable HAGS" },
            { "Annulla", "Cancel" },
            { "PRIORITA' APP IN PRIMO PIANO", "FOREGROUND APP PRIORITY" },
            { "Aumenta la quota CPU assegnata all’app attiva. Utile per gaming, editing video e rendering.", "Increases the CPU share assigned to the active app. Useful for gaming, video editing, and rendering." },
            { "Massima priorita' primo piano (gaming/editing)", "Maximum foreground priority (gaming/editing)" },
            { "Bilanciata (default Windows)", "Balanced (Windows default)" },
            { "DISABILITAZIONE POWER THROTTLING", "DISABLE POWER THROTTLING" },
            { "Power Throttling limita la frequenza CPU dei processi in background. Disabilitarlo mantiene la CPU a piena potenza.", "Power Throttling limits CPU frequency for background processes. Disabling it keeps the CPU at full power." },
            { "Disabilita Power Throttling (massime prestazioni)", "Disable Power Throttling (maximum performance)" },
            { "Riabilita Power Throttling (default)", "Re-enable Power Throttling (default)" },
            { "FUNZIONI SEGRETE E STRUMENTI PRO", "SECRET FEATURES AND PRO TOOLS" },
            { "Le stesse dieci funzioni e strumenti professionali del batch originale.", "The same ten professional tools and functions from the original batch." },
            { "Mostra password WiFi salvate nel PC", "Show saved WiFi passwords on this PC" },
            { "Recupera Product Key di Windows", "Retrieve Windows Product Key" },
            { "Dashboard live CPU/RAM/DISCO in tempo reale", "Live CPU/RAM/DISK dashboard" },
            { "Benchmark velocita' disco (lettura/scrittura)", "Disk speed benchmark (read/write)" },
            { "Scanner processi sospetti e malware", "Suspicious process and malware scanner" },
            { "Info segrete sistema (seriale, UUID, MAC, BIOS)", "Secret system info (serial, UUID, MAC, BIOS)" },
            { "Startup Manager (vedi e rimuovi avvii automatici)", "Startup Manager (view and remove startup items)" },
            { "Mappa rete locale (tutti i dispositivi connessi)", "Local network map (all connected devices)" },
            { "Analisi dipendenze DLL di un eseguibile", "Executable DLL dependency analysis" },
            { "Genera report completo sistema (esportato su Desktop)", "Generate full system report (exported to Desktop)" },
            { "ASSISTENTE IA - CONFIGURAZIONE RICHIESTA", "AI ASSISTANT - SETUP REQUIRED" },
            { "Continuare? (S)", "Continue? (Y)" },
            { "Annulla (N) - Torna al menu", "Cancel (N) - Back to menu" },
            { "MICROSOFT ACTIVATION SCRIPTS (MAS)", "MICROSOFT ACTIVATION SCRIPTS (MAS)" },
            { "Apri attivazione Windows/Office", "Open Windows/Office activation" },
            { "PASSWORD WiFi SALVATE", "SAVED WiFi PASSWORDS" },
            { "Inserisci il nome esatto della rete per visualizzare la password", "Enter the exact network name to view the password" },
            { "Torna agli Strumenti Pro", "Back to Pro Tools" },
            { "STARTUP MANAGER", "STARTUP MANAGER" },
            { "Visualizza le voci di avvio automatico, oppure rimuovi una voce inserendo il nome esatto.", "View startup entries, or remove one by entering its exact name." },
            { "Visualizza programmi di avvio automatico", "View startup programs" },
            { "Rimuovi voce dal registro Run", "Remove entry from Run registry" },
            { "OPERAZIONI E OUTPUT NEL PANNELLO", "OPTIONS AND OUTPUT IN THE PANEL" },
            { "MODULO ATTIVO  /  WINDOWS SPEED BOOSTER", "ACTIVE MODULE  /  WINDOWS SPEED BOOSTER" },
            { "MENU DI OTTIMIZZAZIONE  •  PERFORMANCE SU MISURA", "OPTIMIZATION MENU  •  TAILORED PERFORMANCE" },
            { "SISTEMA PRONTO", "SYSTEM READY" },
            { "CERCA", "SEARCH" },
            { "Conferma", "Confirm" },
            { "Operazione in corso: ", "Operation in progress: " },
            { "Operazione completata · menu ancora disponibile", "Operation completed · menu still available" },
            { "Operazione terminata · controlla l’output", "Operation ended · check the output" },
            { "Operazione completata.", "Operation completed." },
            { "Operazione terminata con codice ", "Operation ended with code " },
            { "Confermi l’esecuzione di ", "Do you confirm execution of " },
            { "Valore MIN (MB):", "MIN value (MB):" },
            { "Valore MAX (MB):", "MAX value (MB):" },
            { "Memoria virtuale", "Virtual memory" },
            { "Valore MIN o MAX non valido.", "Invalid MIN or MAX value." },
            { "Core da allocare al boot (0=Auto/Tutti):", "Cores to allocate at boot (0=Auto/All):" },
            { "Inserire un numero valido (0 per Auto/Tutti).", "Enter a valid number (0 for Auto/All)." },
            { "Nome ESATTO della rete WiFi (0 per uscire):", "Exact WiFi network name (0 to exit):" },
            { "Nome ESATTO da rimuovere dal registro Run:", "Exact name to remove from the Run registry:" },
            { "Domanda (scrivi exit per tornare al menu):", "Question (type exit to return to the menu):" },
            { "ANALISI DIPENDENZE DLL", "DLL DEPENDENCY ANALYSIS" },
            { "Trascina o seleziona l’eseguibile (.exe)", "Drag or select the executable (.exe)" },
            { "Eseguibili (*.exe)|*.exe|Tutti i file (*.*)|*.*", "Executables (*.exe)|*.exe|All files (*.*)|*.*" },
            { "File dll_analyzer.ps1 non trovato nella cartella dello script.", "dll_analyzer.ps1 was not found in the application folder." },
            { "File ask_ai.ps1 non trovato nella cartella dello script.", "ask_ai.ps1 was not found in the application folder." },
            { "Memoria Virtuale (apri opzioni)", "Virtual Memory (open options)" },
            { "Mappa rete locale", "Local network map" },
            { "Benchmark velocita' disco", "Disk speed benchmark" },
            { "Info segrete sistema", "Secret system info" },
            { "Genera report completo sistema", "Generate full system report" },
            { "Inserisci rete WiFi per visualizzare la password", "Enter WiFi network to view password" },
            { "Assistente IA (configurazione e chat)", "AI Assistant (setup and chat)" },
            { "Memoria Virtuale - valori manuali", "Virtual Memory - manual values" },
            { "Elenco reti WiFi salvate", "Saved WiFi networks list" },
            { "ASSISTENTE IA", "AI ASSISTANT" },
            { "Per utilizzare l’assistente IA è necessario disporre di una chiave API gratuita di Groq. Come ottenerla: 1. Vai su https://console.groq.com e registrati. 2. Crea una nuova API Key. 3. Apri ask_ai.ps1 nella cartella dell’app. 4. Sostituisci gsk_tuo-token-qui con la tua chiave.", "To use the AI Assistant you need a free Groq API key. How to get one: 1. Go to https://console.groq.com and sign up. 2. Create a new API key. 3. Open ask_ai.ps1 in the app folder. 4. Replace gsk_tuo-token-qui with your key." },
            { "L’app conserva il nome e la posizione della voce 15. Per la sicurezza della licenza, l’app non scarica né esegue script esterni di attivazione: apre la sezione ufficiale di attivazione Windows.", "The app keeps the name and position of item 15. For license safety, it does not download or run external activation scripts: it opens the official Windows activation section." },
            { "Il batch elenca prima le reti WiFi memorizzate e poi richiede il nome esatto della rete. L’elenco viene eseguito automaticamente qui sotto.", "The batch first lists saved WiFi networks, then asks for the exact network name. The list runs automatically below." },
            { "Seleziona un modulo.", "Select a module." },
            { "Apri Pulizia Disco di Windows", "Open Windows Disk Cleanup" },
            { "Comprimi OS disco C:", "Compact OS drive C:" },
            { "Core da allocare al boot", "Cores to allocate at boot" },
            { "Dashboard live CPU/RAM/DISCO", "Live CPU/RAM/DISK dashboard" },
            { "Disabilita Algoritmo di Nagle", "Disable Nagle Algorithm" },
            { "Disabilita Core Parking", "Disable Core Parking" },
            { "Disabilita Power Throttling", "Disable Power Throttling" },
            { "Disabilita Telemetria Profonda", "Disable Deep Telemetry" },
            { "Disabilita servizi inutili", "Disable unnecessary services" },
            { "GPU Hardware Scheduling (HAGS)", "GPU Hardware Scheduling (HAGS)" },
            { "Massima priorita' primo piano", "Maximum foreground priority" },
            { "Matrix Mode", "Matrix Mode" },
            { "Microsoft Activation Scripts", "Microsoft Activation Scripts" },
            { "Modalita' Gaming Totale", "Total Gaming Mode" },
            { "Mostra password WiFi salvate", "Show saved WiFi passwords" },
            { "Network Boost", "Network Boost" },
            { "OTTIMIZZAZIONE DISCO", "DISK OPTIMIZATION" },
            { "Ottimizza IRQ e Interrupt Affinity", "Optimize IRQ and Interrupt Affinity" },
            { "Ottimizza disco C: (Defrag HDD o TRIM SSD)", "Optimize drive C: (HDD defrag or SSD TRIM)" },
            { "Ottimizzazione SSD Avanzata", "Advanced SSD Optimization" },
            { "PIANO DI ALIMENTAZIONE", "POWER PLAN" },
            { "Pianifica controllo errori C: (chkdsk)", "Schedule drive C: error check (chkdsk)" },
            { "Priorita' App in Primo Piano", "Foreground App Priority" },
            { "Priorita' bilanciata", "Balanced priority" },
            { "Pulizia cache browser", "Clean browser cache" },
            { "Riabilita Power Throttling", "Re-enable Power Throttling" },
            { "Startup Manager", "Startup Manager" },
            { "Timer di Precisione 0.5ms", "0.5ms Precision Timer" },
            { "PROFILO GAMING PER SINGOLO GIOCO", "PER-GAME GAMING PROFILE" },
            { "Profilo Gaming per singolo gioco", "Per-Game Gaming Profile" },
            { "Avvia un gioco scelto dall’utente e applica priorità Alta solo a quel processo. Nessuna modifica permanente al sistema.", "Launches a game selected by the user and applies High priority only to that process. No permanent system changes." },
            { "Seleziona gioco e avvia profilo temporaneo", "Select game and launch temporary profile" },
            { "Seleziona l’eseguibile del gioco (.exe)", "Select the game executable (.exe)" },
            { "PROFILO NOTEBOOK E BATTERIA", "LAPTOP AND BATTERY PROFILE" },
            { "Profilo Notebook e Batteria", "Laptop and Battery Profile" },
            { "Funzioni dedicate ai portatili: controlla batteria e piano di alimentazione, salva il piano attuale e permette il ripristino.", "Laptop-focused functions: checks battery and power plan, saves the current plan, and enables restoration." },
            { "Stato batteria e piano attuale", "Battery status and current power plan" },
            { "Applica profilo notebook bilanciato (salva piano attuale)", "Apply balanced laptop profile (save current plan)" },
            { "Apri impostazioni ufficiali batteria", "Open official battery settings" },
            { "DASHBOARD PRIVACY", "PRIVACY DASHBOARD" },
            { "Dashboard Privacy", "Privacy Dashboard" },
            { "Controllo trasparente delle impostazioni privacy: visualizza lo stato e apri le impostazioni ufficiali senza disattivazioni automatiche.", "Transparent privacy-settings review: view status and open official settings with no automatic disabling." },
            { "Analizza stato privacy e telemetria", "Analyze privacy and telemetry status" },
            { "Apri impostazioni privacy ufficiali", "Open official privacy settings" },
            { "CENTRO RIPRISTINO E ANNULLAMENTO", "RESTORE AND UNDO CENTER" },
            { "Centro Ripristino e Annullamento", "Restore and Undo Center" },
            { "Mostra il backup del piano notebook e consente di ripristinarlo. I tweak avanzati originali restano disponibili nella loro opzione R.", "Shows the laptop power-plan backup and allows restoration. Original advanced tweaks remain available under their R option." },
            { "Mostra backup profilo notebook", "Show laptop profile backup" },
            { "Ripristina piano notebook precedente", "Restore previous laptop power plan" },
            { "Torna ai Tweaks Avanzati", "Back to Advanced Tweaks" },
            { "DIAGNOSTICA INTEGRITÀ WINDOWS", "WINDOWS INTEGRITY DIAGNOSTICS" },
            { "Diagnostica Integrità Windows", "Windows Integrity Diagnostics" },
            { "Analisi in sola lettura dei file di sistema e dell’immagine Windows. Non esegue riparazioni e non modifica il sistema.", "Read-only analysis of system files and the Windows image. It performs no repairs and makes no system changes." },
            { "Esegui analisi integrità Windows", "Run Windows integrity analysis" },
            { "ANALIZZATORE SPAZIO INTELLIGENTE", "SMART STORAGE ANALYZER" },
            { "Analizzatore Spazio Intelligente", "Smart Storage Analyzer" },
            { "Analisi di spazio, cache e cartelle principali in sola lettura. Non elimina file automaticamente.", "Read-only analysis of storage, cache, and main folders. It does not delete files automatically." },
            { "Analizza spazio disco e cartelle principali", "Analyze disk space and main folders" },
            { "STATO DRIVER E DISPOSITIVI", "DRIVER AND DEVICE HEALTH" },
            { "Stato Driver e Dispositivi", "Driver and Device Health" },
            { "Rileva dispositivi con errori e mostra i driver principali. Non scarica né aggiorna driver automaticamente.", "Detects devices with errors and shows key drivers. It does not download or update drivers automatically." },
            { "Analizza dispositivi e driver principali", "Analyze devices and key drivers" },
            { "SUITE DIAGNOSTICA RETE", "NETWORK DIAGNOSTIC SUITE" },
            { "Suite Diagnostica Rete", "Network Diagnostic Suite" },
            { "Controlli in sola lettura su adattatore, gateway, DNS e connettività. Non resetta le impostazioni di rete.", "Read-only checks for adapter, gateway, DNS, and connectivity. It does not reset network settings." },
            { "Esegui diagnostica rete", "Run network diagnostics" },
            { "SNAPSHOT PRESTAZIONI", "PERFORMANCE SNAPSHOT" },
            { "Snapshot Prestazioni", "Performance Snapshot" },
            { "Fotografia immediata di CPU, RAM e spazio disco. Il benchmark disco originale resta nella sua funzione Pro già esistente.", "Instant view of CPU, RAM, and disk space. The original disk benchmark remains in its existing Pro function." },
            { "Raccogli snapshot CPU, RAM e disco", "Collect CPU, RAM, and disk snapshot" },
            { "CONTROLLO AGGIORNAMENTI E RIAVVIO", "UPDATE AND RESTART CHECK" },
            { "Controllo Aggiornamenti e Riavvio", "Update and Restart Check" },
            { "Verifica se Windows richiede un riavvio e mostra lo stato dei servizi di aggiornamento. Non modifica Windows Update.", "Checks whether Windows requires a restart and shows update service status. It does not modify Windows Update." },
            { "Verifica aggiornamenti e riavvio pendente", "Check updates and pending restart" },
            { "Apri Windows Update ufficiale", "Open official Windows Update" },
            { "Le dieci opzioni avanzate originali restano invariate; qui sotto sono presenti anche moduli aggiuntivi separati e reversibili.", "The original ten advanced options remain unchanged; separate and reversible additional modules are available below." },
            { "RIPRISTINO MPO (FLICKER / STUTTER)", "MPO RECOVERY (FLICKER / STUTTER)" },
            { "Ripristino MPO (flicker / stutter)", "MPO Recovery Toggle (flicker / stutter)" },
            { "MPO può aiutare solo in caso di flicker, schermate nere o stutter grafico specifico. Il valore precedente viene salvato; è necessario riavviare.", "MPO can help only with specific flicker, black-screen, or graphics-stutter cases. The previous value is saved; a restart is required." },
            { "Disabilita MPO (salva backup, riavvio richiesto)", "Disable MPO (save backup, restart required)" },
            { "Ripristina MPO dal backup", "Restore MPO from backup" },
            { "ASSOCIAZIONE GPU AD ALTE PRESTAZIONI", "HIGH-PERFORMANCE GPU BINDING" },
            { "Associazione GPU Alte Prestazioni per gioco", "GPU High-Performance Binding for a game" },
            { "Scegli un eseguibile: Windows applicherà a quel solo gioco la preferenza GPU High Performance. Non modifica HAGS, priorità o driver.", "Choose an executable: Windows applies the High Performance GPU preference to that game only. It does not modify HAGS, priority, or drivers." },
            { "Seleziona gioco e applica GPU High Performance", "Select game and apply High-Performance GPU" },
            { "Ripristina GPU predefinita per un gioco", "Restore default GPU preference for a game" },
            { "ACCELERAZIONE MOUSE RAW", "RAW MOUSE ACCELERATION" },
            { "Disabilita accelerazione mouse (Enhance Pointer Precision)", "Disable mouse acceleration (Enhance Pointer Precision)" },
            { "Disattiva Enhance Pointer Precision solo per l’utente corrente. I tre valori originali vengono salvati e possono essere ripristinati.", "Disables Enhance Pointer Precision for the current user only. The three original values are saved and can be restored." },
            { "Disabilita accelerazione mouse (salva backup)", "Disable mouse acceleration (save backup)" },
            { "Ripristina accelerazione mouse dal backup", "Restore mouse acceleration from backup" },
            { "Seleziona l’eseguibile del gioco per la preferenza GPU (.exe)", "Select the game executable for GPU preference (.exe)" },
            { "Nessun backup MPO disponibile.", "No MPO backup available." },
            { "Nessun backup GPU disponibile per questo eseguibile.", "No GPU backup is available for this executable." },
            { "Nessun backup mouse disponibile.", "No mouse backup available." },
            { "Windows Speed Booster", "Windows Speed Booster" }
        };

        private static readonly Dictionary<string, string> PrintEnglish = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "Punto di ripristino creato.", "Restore point created." },
            { "Punto di ripristino non disponibile (normale su Win11).", "Restore point unavailable (normal on Windows 11)." },
            { "Pulizia cartelle TEMP di sistema...", "Cleaning system TEMP folders..." },
            { "Pulizia TEMP utente...", "Cleaning user TEMP folder..." },
            { "Pulizia Prefetch e Log di sistema...", "Cleaning Prefetch and system logs..." },
            { "Pulizia cache Windows Update...", "Cleaning Windows Update cache..." },
            { "Svuotamento Cestino...", "Emptying Recycle Bin..." },
            { "Pulizia cache miniature...", "Cleaning thumbnail cache..." },
            { "Pulizia iniziale completata!", "Initial cleanup completed!" },
            { "Pagefile impostato:", "Pagefile set:" },
            { "Gestione automatica ripristinata.", "Automatic management restored." },
            { "Ripristinato uso automatico di tutti i core.", "Automatic use of all cores restored." },
            { "Boot configurato con", "Boot configured with" },
            { "Effetti visivi: Prestazioni Massime", "Visual effects: Best Performance" },
            { "Effetti visivi: Bilanciato", "Visual effects: Balanced" },
            { "Effetti visivi: Aspetto Migliore", "Visual effects: Best Appearance" },
            { "Piano: Prestazioni Elevate", "Plan: High Performance" },
            { "Piano: Bilanciato", "Plan: Balanced" },
            { "Piano: Risparmio Energetico", "Plan: Power Saver" },
            { "Piano Ultimate Performance attivato.", "Ultimate Performance plan enabled." },
            { "Ottimizzazione completata con successo.", "Optimization completed successfully." },
            { "Defrag completato.", "Defragmentation completed." },
            { "Controllo disco pianificato al prossimo riavvio.", "Disk check scheduled for next restart." },
            { "Compressione completata.", "Compression completed." },
            { "disabilitato.", "disabled." },
            { "impostato ad avvio ritardato.", "set to delayed start." },
            { "ripristinato.", "restored." },
            { "Apertura Pulizia Disco...", "Opening Disk Cleanup..." },
            { "cleanmgr non disponibile: aperto Storage Sense.", "cleanmgr unavailable: Storage Sense opened." },
            { "Cache browser eliminate.", "Browser cache cleared." },
            { "Core Parking disabilitato sul piano corrente e su tutti i piani disponibili.", "Core Parking disabled on the current and all available plans." },
            { "Timer impostato a 1ms via API (attivo fino al riavvio).", "Timer set to 1ms via API (active until restart)." },
            { "Ottimizzazione SSD completata.", "SSD optimization completed." },
            { "HAGS abilitato. Riavvia il PC per applicare.", "HAGS enabled. Restart the PC to apply." },
            { "HAGS disabilitato. Riavvia il PC per applicare.", "HAGS disabled. Restart the PC to apply." },
            { "Priorita primo piano massimizzata.", "Foreground priority maximized." },
            { "Priorita bilanciata ripristinata.", "Balanced priority restored." },
            { "Power Throttling disabilitato.", "Power Throttling disabled." },
            { "Power Throttling riabilitato.", "Power Throttling re-enabled." },
            { "Aperte le impostazioni di attivazione Windows.", "Windows activation settings opened." },
            { "Inserisci il nome esatto della rete usando l’opzione 1.", "Enter the exact network name using option 1." },
            { "Password non trovata o rete non esistente.", "Password not found or network does not exist." },
            { "Voce rimossa da tutti i Run keys (se esisteva).", "Entry removed from all Run keys (if it existed)." },
            { "Network Boost completato!", "Network Boost completed!" },
            { "Riavvia il PC per applicare tutti i cambiamenti.", "Restart the PC to apply all changes." },
            { "Gaming Mode totale attivato.", "Total Gaming Mode enabled." },
            { "Nagle disabilitato.", "Nagle disabled." },
            { "Telemetria profonda disabilitata.", "Deep telemetry disabled." },
            { "Interrupt Moderation off su:", "Interrupt Moderation off on:" },
            { "IRQ e interrupt ottimizzati.", "IRQ and interrupts optimized." },
            { "Tutti i tweaks ripristinati. Riavvia il PC.", "All tweaks restored. Restart the PC." },
            { "Report salvato:", "Report saved:" },
            { "Operazione interrotta dall’utente.", "Operation stopped by user." },
            { "Errore:", "Error:" },
            { "Velocita:", "Speed:" },
            { "RAM Totale:", "Total RAM:" },
            { "RAM Libera:", "Free RAM:" },
            { "Dischi:", "Disks:" },
            { "Sistema:", "System:" },
            { "Carico CPU:", "CPU Load:" },
            { "RAM Usata:", "RAM Used:" },
            { "Processi:", "Processes:" },
            { "Processi analizzati:", "Processes analyzed:" },
            { "Nessun processo sospetto rilevato.", "No suspicious process detected." },
            { "Connessioni TCP attive:", "Active TCP connections:" },
            { "Seriale PC:", "PC serial:" },
            { "Produttore:", "Manufacturer:" },
            { "Modello:", "Model:" },
            { "Versione BIOS:", "BIOS version:" },
            { "Seriale BIOS:", "BIOS serial:" },
            { "Scheda Madre:", "Motherboard:" },
            { "Seriale scheda madre:", "Motherboard serial:" },
            { "RETE:", "NETWORK:" },
            { "IP Pubblico:", "Public IP:" },
            { "Gateway rilevato:", "Detected gateway:" },
            { "Dispositivi nella cache ARP:", "Devices in ARP cache:" },
            { "Scansione rete in corso...", "Network scan in progress..." },
            { "Disco ", "Drive " },
            { " usato", " used" },
            { "Edizione:", "Edition:" },
            { "Trovata con:", "Found via:" },
            { "Chiave non leggibile via software. Il PC potrebbe usare una licenza digitale.", "Key cannot be read by software. This PC may use a digital license." },
            { "DASHBOARD LIVE - SISTEMA IN TEMPO REALE", "LIVE DASHBOARD - REAL-TIME SYSTEM" },
            { "Ora:", "Time:" },
            { "TOP 5 CPU:", "TOP 5 CPU:" },
            { "Scrittura:", "Write:" },
            { "Lettura:", "Read:" },
            { "Errore benchmark:", "Benchmark error:" },
            { "non disponibile", "unavailable" },
            { "Gateway non trovato", "Gateway not found" },
            { "Sistema Operativo", "Operating System" },
            { "Architettura:", "Architecture:" },
            { "Nome:", "Name:" },
            { "Banco:", "Slot:" },
            { "Disco:", "Drive:" },
            { "PROGRAMMI INSTALLATI", "INSTALLED PROGRAMS" },
            { "AVVII AUTOMATICI", "STARTUP ITEMS" },
            { "FINE REPORT", "END OF REPORT" },
            { "Avvio gioco e applicazione profilo temporaneo...", "Launching game and applying temporary profile..." },
            { "Profilo gaming temporaneo applicato al processo:", "Temporary gaming profile applied to process:" },
            { "Priorità High", "High Priority" },
            { "Gioco avviato, ma la priorità non è stata modificata:", "Game launched, but priority was not changed:" },
            { "Rilevamento batteria e piano di alimentazione...", "Detecting battery and power plan..." },
            { "Batteria:", "Battery:" },
            { "Stato:", "Status:" },
            { "Batteria non rilevata: il PC potrebbe essere un desktop.", "Battery not detected: this PC may be a desktop." },
            { "Piano attivo:", "Active power plan:" },
            { "Salvataggio piano attuale e applicazione profilo notebook...", "Saving current plan and applying laptop profile..." },
            { "Profilo notebook bilanciato applicato. Piano precedente salvato.", "Balanced laptop profile applied. Previous plan saved." },
            { "Impostazioni batteria aperte.", "Battery settings opened." },
            { "Analisi impostazioni privacy e telemetria...", "Analyzing privacy and telemetry settings..." },
            { "Telemetria ", "Telemetry " },
            { "non configurata", "not configured" },
            { "Impostazioni privacy aperte.", "Privacy settings opened." },
            { "Lettura backup profilo notebook...", "Reading laptop profile backup..." },
            { "Nessun backup notebook disponibile.", "No laptop backup available." },
            { "Verifica backup e ripristino piano notebook...", "Checking backup and restoring laptop power plan..." },
            { "Nessun piano precedente salvato.", "No previous power plan saved." },
            { "Backup piano non valido.", "Invalid power-plan backup." },
            { "GUID piano non valido.", "Invalid power-plan GUID." },
            { "Piano notebook precedente ripristinato.", "Previous laptop power plan restored." },
            { "Analisi elementi di avvio...", "Analyzing startup items..." },
            { "Voci Run trovate:", "Run entries found:" },
            { "Prime attività pianificate non Microsoft:", "First non-Microsoft scheduled tasks:" },
            { "Analisi avvio completata.", "Startup analysis completed." },
            { "Avvio verifica file di sistema (SFC VerifyOnly)...", "Starting system file check (SFC VerifyOnly)..." },
            { "Avvio analisi immagine Windows (DISM ScanHealth)...", "Starting Windows image analysis (DISM ScanHealth)..." },
            { "Diagnostica integrità completata.", "Integrity diagnostics completed." },
            { "Avvio riparazione file di sistema (SFC)...", "Starting system file repair (SFC)..." },
            { "Avvio riparazione immagine Windows (DISM)...", "Starting Windows image repair (DISM)..." },
            { "Riparazione integrità completata. Riavvia il PC se richiesto.", "Integrity repair completed. Restart the PC if requested." },
            { "Analisi spazio del disco di sistema...", "Analyzing system disk space..." },
            { "Usato:", "Used:" },
            { "Libero:", "Free:" },
            { "accesso parziale", "partial access" },
            { "Analisi spazio completata.", "Storage analysis completed." },
            { "Ricerca dispositivi con stato non OK...", "Searching for devices with non-OK status..." },
            { "Nessun dispositivo con errore segnalato.", "No device error reported." },
            { "Driver firmati più recenti:", "Most recent signed drivers:" },
            { "Analisi driver completata.", "Driver analysis completed." },
            { "Rilevamento adattatori attivi...", "Detecting active adapters..." },
            { "Test gateway...", "Testing gateway..." },
            { "Gateway non rilevato.", "Gateway not detected." },
            { "Test risoluzione DNS...", "Testing DNS resolution..." },
            { "Diagnostica rete completata.", "Network diagnostics completed." },
            { "Raccolta snapshot prestazioni...", "Collecting performance snapshot..." },
            { "RAM usata:", "RAM used:" },
            { "Snapshot completato.", "Snapshot completed." },
            { "Verifica riavvio pendente...", "Checking pending restart..." },
            { "Operazioni file in attesa", "Pending file operations" },
            { "Riavvio di Windows consigliato.", "Windows restart recommended." },
            { "Nessun riavvio pendente rilevato.", "No pending restart detected." },
            { "Controllo aggiornamenti completato.", "Update check completed." },
            { "Windows Update aperto.", "Windows Update opened." },
            { "Salvataggio valore MPO corrente...", "Saving current MPO value..." },
            { "MPO disabilitato. Riavvia il PC per applicare.", "MPO disabled. Restart the PC to apply." },
            { "Ripristino MPO dal backup...", "Restoring MPO from backup..." },
            { "MPO ripristinato. Riavvia il PC per applicare.", "MPO restored. Restart the PC to apply." },
            { "Nessun backup MPO disponibile.", "No MPO backup available." },
            { "Salvataggio preferenza GPU precedente...", "Saving previous GPU preference..." },
            { "Preferenza GPU High Performance applicata a:", "High-Performance GPU preference applied to:" },
            { "L’impostazione avrà effetto al prossimo avvio del gioco.", "The setting takes effect the next time the game starts." },
            { "Ripristino preferenza GPU predefinita...", "Restoring default GPU preference..." },
            { "Preferenza GPU precedente ripristinata per:", "Previous GPU preference restored for:" },
            { "Nessun backup GPU disponibile per questo eseguibile.", "No GPU backup is available for this executable." },
            { "Salvataggio impostazioni mouse correnti...", "Saving current mouse settings..." },
            { "Accelerazione mouse disabilitata per l’utente corrente.", "Mouse acceleration disabled for the current user." },
            { "Ripristino impostazioni mouse dal backup...", "Restoring mouse settings from backup..." },
            { "Accelerazione mouse ripristinata.", "Mouse acceleration restored." },
            { "Nessun backup mouse disponibile.", "No mouse backup available." },
            { "Backup MPO non valido.", "Invalid MPO backup." },
            { "Backup GPU non valido.", "Invalid GPU backup." }
        };

        internal static bool UseEnglish { get; set; }

        internal static string P(string text)
        {
            if (!UseEnglish || String.IsNullOrEmpty(text)) return text;
            List<KeyValuePair<string, string>> rules = new List<KeyValuePair<string, string>>(PrintEnglish);
            rules.Sort(delegate(KeyValuePair<string, string> a, KeyValuePair<string, string> b) { return b.Key.Length.CompareTo(a.Key.Length); });
            foreach (KeyValuePair<string, string> pair in rules) text = text.Replace(pair.Key, pair.Value);
            return text;
        }

        internal static string T(string italian)
        {
            if (!UseEnglish || String.IsNullOrEmpty(italian)) return italian;
            string translated;
            if (English.TryGetValue(italian, out translated)) return translated;
            if (italian.StartsWith("["))
            {
                int end = italian.IndexOf(']');
                if (end > 0)
                {
                    int bodyStart = end + 1;
                    while (bodyStart < italian.Length && Char.IsWhiteSpace(italian[bodyStart])) bodyStart++;
                    string body = italian.Substring(bodyStart);
                    if (English.TryGetValue(body, out translated)) return italian.Substring(0, bodyStart) + translated;
                }
            }
            return italian;
        }

        internal static string Choose(string italian, string english) { return UseEnglish ? english : italian; }
    }

    internal static class SafetyStore
    {
        private static readonly string StateDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Windows Speed Booster");
        private static readonly string StatePath = Path.Combine(StateDirectory, "safe_state.ini");
        private static readonly string LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SpeedBooster_Log.txt");

        internal static void RunHealthCheck(string appDirectory)
        {
            try
            {
                bool admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                bool ai = File.Exists(Path.Combine(appDirectory, "ask_ai.ps1"));
                bool dll = File.Exists(Path.Combine(appDirectory, "dll_analyzer.ps1"));
                long freeGb = 0;
                try { freeGb = new DriveInfo(Path.GetPathRoot(appDirectory)).AvailableFreeSpace / 1073741824L; } catch { }
                File.AppendAllText(LogPath, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] [TOOL HEALTH CHECK] Admin=" + admin + " | ask_ai.ps1=" + ai + " | dll_analyzer.ps1=" + dll + " | FreeGB=" + freeGb + " | OS=" + Environment.OSVersion.VersionString + Environment.NewLine, Encoding.UTF8);
            }
            catch { }
        }

        internal static void Save(string key, string value)
        {
            try
            {
                Directory.CreateDirectory(StateDirectory);
                Dictionary<string, string> values = ReadAll();
                values[key] = value ?? String.Empty;
                using (StreamWriter writer = new StreamWriter(StatePath, false, Encoding.UTF8))
                {
                    foreach (KeyValuePair<string, string> pair in values) writer.WriteLine(pair.Key + "=" + pair.Value);
                }
            }
            catch { }
        }

        internal static string Load(string key)
        {
            string value;
            return ReadAll().TryGetValue(key, out value) ? value : String.Empty;
        }

        private static Dictionary<string, string> ReadAll()
        {
            Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                if (!File.Exists(StatePath)) return values;
                foreach (string line in File.ReadAllLines(StatePath, Encoding.UTF8))
                {
                    int sep = line.IndexOf('=');
                    if (sep > 0) values[line.Substring(0, sep)] = line.Substring(sep + 1);
                }
            }
            catch { }
            return values;
        }
    }

    internal sealed class LanguageSelector : Form
    {
        internal bool UseEnglish { get; private set; }

        internal LanguageSelector()
        {
            Text = "Windows Speed Booster · Language";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            ClientSize = new Size(720, 420);
            BackColor = Color.FromArgb(32, 25, 88);
            DoubleBuffered = true;

            LanguageChoiceButton italian = new LanguageChoiceButton();
            italian.Caption = "ITALIANO";
            italian.Detail = "Interfaccia completa in italiano";
            italian.Accent = Color.FromArgb(52, 213, 252);
            italian.Location = new Point(58, 255);
            italian.Size = new Size(280, 116);
            italian.Click += delegate { UseEnglish = false; DialogResult = DialogResult.OK; Close(); };
            Controls.Add(italian);

            LanguageChoiceButton english = new LanguageChoiceButton();
            english.Caption = "ENGLISH";
            english.Detail = "Full interface in English";
            english.Accent = Color.FromArgb(255, 190, 69);
            english.Location = new Point(382, 255);
            english.Size = new Size(280, 116);
            english.Click += delegate { UseEnglish = true; DialogResult = DialogResult.OK; Close(); };
            Controls.Add(english);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = ClientRectangle;
            using (LinearGradientBrush bg = new LinearGradientBrush(r, Color.FromArgb(54, 33, 137), Color.FromArgb(14, 118, 154), LinearGradientMode.Horizontal)) e.Graphics.FillRectangle(bg, r);
            using (Font brand = new Font("Segoe UI Semibold", 22F, FontStyle.Bold))
            using (Font title = new Font("Segoe UI Semibold", 14F, FontStyle.Bold))
            using (Font note = new Font("Segoe UI", 10F))
            using (SolidBrush white = new SolidBrush(Color.FromArgb(245, 250, 255)))
            using (SolidBrush cyan = new SolidBrush(Color.FromArgb(91, 231, 255)))
            using (SolidBrush soft = new SolidBrush(Color.FromArgb(225, 233, 255)))
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center })
            {
                e.Graphics.DrawString("WINDOWS SPEED BOOSTER", brand, white, new Rectangle(30, 45, Width - 60, 40), center);
                e.Graphics.DrawString("V14.0  ·  Made by 9337progame", note, cyan, new Rectangle(30, 91, Width - 60, 28), center);
                e.Graphics.DrawString("SCEGLI LA LINGUA  /  CHOOSE LANGUAGE", title, white, new Rectangle(30, 141, Width - 60, 32), center);
                e.Graphics.DrawString("La scelta verrà applicata prima dell’avvio e della pulizia iniziale.\nYour choice is applied before startup and initial cleanup.", note, soft, new Rectangle(30, 172, Width - 60, 54), center);
            }
        }
    }

    internal sealed class LanguageChoiceButton : Button
    {
        private bool hover;
        public Color Accent { get; set; }
        public string Caption { get; set; }
        public string Detail { get; set; }

        public LanguageChoiceButton()
        {
            DoubleBuffered = true;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Accent = Color.FromArgb(52, 213, 252);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width < 3 || Height < 3) return;
            using (GraphicsPath shape = Shape(new Rectangle(0, 0, Width - 1, Height - 1), 16)) { Region = new Region(shape); }
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent != null ? Parent.BackColor : BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Color top = Color.FromArgb(hover ? 248 : 230, Math.Min(255, Accent.R + 38), Math.Min(255, Accent.G + 38), Math.Min(255, Accent.B + 38));
            Color bottom = Color.FromArgb(hover ? 232 : 214, Math.Max(35, (int)Accent.R), Math.Max(35, (int)Accent.G), Math.Max(35, (int)Accent.B));
            using (GraphicsPath path = Shape(r, 16))
            using (LinearGradientBrush fill = new LinearGradientBrush(r, top, bottom, LinearGradientMode.Vertical))
            using (Pen edge = new Pen(Color.FromArgb(255, 239, 252, 255), 1.2F))
            using (Font caption = new Font("Segoe UI Semibold", 16F, FontStyle.Bold))
            using (Font detail = new Font("Segoe UI", 9.5F))
            using (SolidBrush text = new SolidBrush(Color.FromArgb(34, 20, 58)))
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(edge, path);
                e.Graphics.DrawString(Caption ?? String.Empty, caption, text, new Rectangle(16, 22, Width - 32, 36), center);
                e.Graphics.DrawString(Detail ?? String.Empty, detail, text, new Rectangle(16, 62, Width - 32, 26), center);
            }
        }

        private static GraphicsPath Shape(Rectangle r, int cut)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddPolygon(new Point[] { new Point(r.X + cut, r.Y), new Point(r.Right - cut, r.Y), new Point(r.Right, r.Y + cut), new Point(r.Right, r.Bottom - cut), new Point(r.Right - cut, r.Bottom), new Point(r.X + cut, r.Bottom), new Point(r.X, r.Bottom - cut), new Point(r.X, r.Y + cut) });
            p.CloseFigure();
            return p;
        }
    }

    internal sealed class StartupForm : Form
    {
        private readonly RichTextBox output;
        private readonly Label state;
        private readonly string appDirectory;
        private readonly string logPath;
        internal string AppDirectory { get { return appDirectory; } }

        public StartupForm()
        {
            appDirectory = Application.StartupPath;
            logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SpeedBooster_Log.txt");
            Text = "Windows Speed Booster V14";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ControlBox = false;
            ClientSize = new Size(760, 430);
            BackColor = Color.FromArgb(8, 10, 24);

            Label title = new Label();
            title.Text = "WINDOWS SPEED BOOSTER V14";
            title.Font = new Font("Segoe UI Black", 20F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(231, 242, 255);
            title.AutoSize = true;
            title.Location = new Point(28, 24);
            Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = AppText.T("Avvio protocollo di pulizia profonda...");
            subtitle.Font = new Font("Segoe UI", 10F);
            subtitle.ForeColor = Color.FromArgb(157, 221, 182);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(31, 68);
            Controls.Add(subtitle);

            output = new RichTextBox();
            output.ReadOnly = true;
            output.BackColor = Color.FromArgb(5, 7, 16);
            output.ForeColor = Color.FromArgb(214, 229, 218);
            output.BorderStyle = BorderStyle.FixedSingle;
            output.Font = new Font("Consolas", 9.5F);
            output.Location = new Point(28, 104);
            output.Size = new Size(704, 258);
            Controls.Add(output);

            state = new Label();
            state.ForeColor = Color.FromArgb(255, 211, 110);
            state.Font = new Font("Segoe UI", 9F);
            state.AutoSize = true;
            state.Location = new Point(28, 382);
            Controls.Add(state);
            Shown += StartupForm_Shown;
        }

        private void StartupForm_Shown(object sender, EventArgs e)
        {
            SafetyStore.RunHealthCheck(appDirectory);
            Append(AppText.Choose("[*] Creazione punto di ripristino automatico...", "[*] Creating automatic restore point..."));
            RunInitialSequence();
        }

        private void RunInitialSequence()
        {
            string script = "$WarningPreference = 'SilentlyContinue'; $VerbosePreference = 'SilentlyContinue'; $ProgressPreference = 'SilentlyContinue'; try { Checkpoint-Computer -Description 'SpeedBooster V14' -RestorePointType MODIFY_SETTINGS -ErrorAction Stop; Write-Output '[OK] Punto di ripristino creato.' } catch { Write-Output '[!] Punto di ripristino non disponibile (normale su Win11).' }; Start-Sleep -Seconds 2; Write-Output ''; Write-Output '[1/7] Flush cache DNS...'; ipconfig /flushdns | Out-Null; Write-Output '       OK'; Write-Output '[2/7] Pulizia cartelle TEMP di sistema...'; Get-ChildItem (Join-Path $env:windir 'Temp') -Force -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue; Write-Output '       OK'; Write-Output '[3/7] Pulizia TEMP utente...'; Get-ChildItem $env:TEMP -Force -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue; Write-Output '       OK'; Write-Output '[4/7] Pulizia Prefetch e Log di sistema...'; Remove-Item (Join-Path $env:windir 'Prefetch\\*') -Force -Recurse -ErrorAction SilentlyContinue; Get-ChildItem $env:windir -Filter '*.log' -File -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue; Write-Output '       OK'; Write-Output '[5/7] Pulizia cache Windows Update...'; Stop-Service wuauserv,bits -ErrorAction SilentlyContinue 3>$null 4>$null 5>$null 6>$null; Remove-Item (Join-Path $env:windir 'SoftwareDistribution\\Download') -Force -Recurse -ErrorAction SilentlyContinue; Start-Service wuauserv,bits -ErrorAction SilentlyContinue 3>$null 4>$null 5>$null 6>$null; Write-Output '       OK'; Write-Output '[6/7] Svuotamento Cestino...'; Clear-RecycleBin -Force -ErrorAction SilentlyContinue; Write-Output '       OK'; Write-Output '[7/7] Pulizia cache miniature...'; Remove-Item (Join-Path $env:LOCALAPPDATA 'Microsoft\\Windows\\Explorer\\thumbcache_*.db') -Force -ErrorAction SilentlyContinue; Write-Output '       OK'; Write-Output ''; Write-Output '[==] Pulizia iniziale completata!'; Start-Sleep -Seconds 2";
            state.Text = AppText.T("Pulizia iniziale in corso...");
            Execute(script, delegate(int code)
            {
                state.ForeColor = Color.FromArgb(157, 221, 182);
                state.Text = AppText.T("Pulizia iniziale completata. Apertura menu...");
                Timer timer = new Timer();
                timer.Interval = 700;
                timer.Tick += delegate
                {
                    timer.Stop();
                    DialogResult = DialogResult.OK;
                    Close();
                };
                timer.Start();
            });
        }

        private void Execute(string script, Action<int> complete)
        {
            ProcessStartInfo info = PowerShellInfo(script, appDirectory);
            Process process = new Process();
            process.StartInfo = info;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += delegate(object s, DataReceivedEventArgs e) { if (!String.IsNullOrEmpty(e.Data)) Append(e.Data); };
            process.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e) { if (!String.IsNullOrEmpty(e.Data)) Append(e.Data); };
            Task.Factory.StartNew(delegate
            {
                int code = -1;
                try { process.Start(); process.StandardInput.WriteLine(script); process.StandardInput.Close(); process.BeginOutputReadLine(); process.BeginErrorReadLine(); process.WaitForExit(); code = process.ExitCode; } catch (Exception ex) { Append("[!] " + ex.Message); }
                BeginInvoke(new Action<int>(complete), code);
            });
        }

        private void Append(string text)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string>(Append), text); return; }
            string visible = AppText.P(text);
            output.AppendText(visible + Environment.NewLine);
            output.ScrollToCaret();
            try { File.AppendAllText(logPath, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] [STARTUP] " + text + Environment.NewLine, Encoding.UTF8); } catch { }
        }

        internal static ProcessStartInfo PowerShellInfo(string script, string directory)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            string exe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "WindowsPowerShell\\v1.0\\powershell.exe");
            info.FileName = File.Exists(exe) ? exe : "powershell.exe";
            info.Arguments = "-NoProfile -Command -";
            info.WorkingDirectory = directory;
            string systemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string windowsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string machinePath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? String.Empty;
            string userPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? String.Empty;
            info.EnvironmentVariables["Path"] = systemDirectory + ";" + windowsDirectory + ";" + machinePath + ";" + userPath;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.StandardOutputEncoding = Encoding.UTF8;
            info.StandardErrorEncoding = Encoding.UTF8;
            return info;
        }
    }

    internal sealed class MainForm : Form
    {
        private readonly string appDirectory;
        private readonly string logPath;
        private readonly Panel page;
        private readonly Panel optionsViewport;
        private readonly FlowLayoutPanel options;
        private readonly RichTextBox output;
        private readonly Label title;
        private readonly Label note;
        private readonly Button stopButton;
        private readonly ProgressBar activityProgress;
        private readonly Label activityLabel;
        private readonly FeatureShowcase showcase;
        private readonly HeaderSearchBar tweakSearch;
        private readonly HeaderGithubButton authorGithub;
        private readonly List<SearchEntry> searchEntries;
        private bool galleryMode;
        private bool searchMode;
        private bool fullScreen;
        private FormBorderStyle savedBorderStyle;
        private FormWindowState savedWindowState;
        private Rectangle savedBounds;
        private Size savedMinimumSize;

        private sealed class SearchEntry
        {
            internal int ModuleIndex;
            internal string ModuleName;
            internal string Title;
            internal string Keywords;
            internal Action Navigate;
        }
        private int activeModule;
        private Process activeProcess;
        private bool busy;

        public MainForm(string directory)
        {
            appDirectory = directory;
            logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "SpeedBooster_Log.txt");
            Text = "Windows Speed Booster V14";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1180, 760);
            Size = new Size(1420, 880);
            BackColor = Color.FromArgb(48, 37, 118);
            Font = new Font("Segoe UI", 9F);
            DoubleBuffered = true;
            KeyPreview = true;
            KeyDown += MainForm_KeyDown;

            AuroraHeader header = new AuroraHeader();
            header.Dock = DockStyle.Top;
            header.Height = 138;
            Controls.Add(header);

            tweakSearch = new HeaderSearchBar();
            header.Controls.Add(tweakSearch);
            authorGithub = new HeaderGithubButton();
            authorGithub.AccessibleName = "GitHub 9337progame";
            authorGithub.Click += OpenAuthorGithub;
            header.Controls.Add(authorGithub);
            Action layoutSearch = delegate
            {
                int leftEdge = 520;
                int rightEdge = Math.Max(leftEdge + 300, header.Width - 316);
                int available = Math.Max(300, rightEdge - leftEdge);
                tweakSearch.Width = Math.Min(520, available);
                tweakSearch.Left = leftEdge + Math.Max(0, (available - tweakSearch.Width) / 2);
                tweakSearch.Top = 48;
                tweakSearch.Height = 42;
                authorGithub.Size = new Size(178, 38);
                authorGithub.Left = Math.Max(486, header.Width - authorGithub.Width - 28);
                authorGithub.Top = 17;
            };
            layoutSearch();
            header.SizeChanged += delegate { layoutSearch(); };
            tweakSearch.QueryChanged += delegate { ApplySearch(tweakSearch.Query); };

            TableLayoutPanel workspace = new TableLayoutPanel();
            workspace.Dock = DockStyle.Fill;
            workspace.ColumnCount = 2;
            workspace.RowCount = 1;
            workspace.BackColor = Color.FromArgb(48, 37, 118);
            workspace.Padding = new Padding(18, 16, 18, 18);
            workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Controls.Add(workspace);

            page = new Panel();
            page.Dock = DockStyle.Fill;
            page.Padding = new Padding(0);
            page.BackColor = Color.FromArgb(37, 31, 96);
            TableLayoutPanel navigationLayout = new TableLayoutPanel();
            navigationLayout.Dock = DockStyle.Fill;
            navigationLayout.ColumnCount = 1;
            navigationLayout.RowCount = 2;
            navigationLayout.Padding = new Padding(14, 11, 12, 10);
            navigationLayout.BackColor = Color.FromArgb(37, 31, 96);
            navigationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            navigationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            page.Controls.Add(navigationLayout);
            Panel menuHeader = new Panel();
            menuHeader.Dock = DockStyle.Fill;
            menuHeader.BackColor = Color.FromArgb(37, 31, 96);
            navigationLayout.Controls.Add(menuHeader, 0, 0);
            title = new Label();
            title.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(238, 244, 255);
            title.AutoSize = false;
            title.Height = 38;
            title.Dock = DockStyle.Top;
            title.Padding = new Padding(5, 0, 0, 0);
            menuHeader.Controls.Add(title);
            note = new Label();
            note.Font = new Font("Segoe UI", 9.5F);
            note.ForeColor = Color.FromArgb(155, 173, 214);
            note.AutoSize = false;
            note.Dock = DockStyle.Fill;
            note.Padding = new Padding(7, 3, 8, 0);
            menuHeader.Controls.Add(note);
            optionsViewport = new Panel();
            optionsViewport.Dock = DockStyle.Fill;
            optionsViewport.AutoScroll = true;
            optionsViewport.BackColor = Color.FromArgb(37, 31, 96);
            navigationLayout.Controls.Add(optionsViewport, 0, 1);
            options = new FlowLayoutPanel();
            options.Location = new Point(0, 0);
            options.AutoScroll = false;
            options.FlowDirection = FlowDirection.TopDown;
            options.WrapContents = false;
            options.BackColor = Color.FromArgb(37, 31, 96);
            options.Padding = new Padding(5, 34, 18, 16);
            optionsMarginReset(options);
            optionsViewport.Controls.Add(options);
            optionsViewport.SizeChanged += delegate { RefreshOptionLayout(); };

            Panel right = new Panel();
            right.Dock = DockStyle.Fill;
            right.Padding = new Padding(13, 3, 0, 0);
            right.BackColor = Color.FromArgb(50, 35, 121);
            workspace.Controls.Add(page, 0, 0);
            workspace.Controls.Add(right, 1, 0);

            TableLayoutPanel rightLayout = new TableLayoutPanel();
            rightLayout.Dock = DockStyle.Fill;
            rightLayout.ColumnCount = 1;
            rightLayout.RowCount = 2;
            rightLayout.BackColor = Color.FromArgb(50, 35, 121);
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
            right.Controls.Add(rightLayout);
            showcase = new FeatureShowcase();
            showcase.Dock = DockStyle.Fill;
            showcase.FeatureIndex = 3;
            showcase.FeatureTitle = AppText.T("PRONTO ALL'OTTIMIZZAZIONE");
            showcase.FeatureNote = AppText.T("Scegli un modulo per aprire le relative impostazioni. Il pannello operativo e il log restano sempre disponibili.");
            rightLayout.Controls.Add(showcase, 0, 0);

            Panel consoleShell = new Panel();
            consoleShell.Dock = DockStyle.Fill;
            consoleShell.Padding = new Padding(13, 10, 13, 10);
            consoleShell.BackColor = Color.FromArgb(61, 46, 137);
            rightLayout.Controls.Add(consoleShell, 0, 1);
            TableLayoutPanel logLayout = new TableLayoutPanel();
            logLayout.Dock = DockStyle.Fill;
            logLayout.ColumnCount = 1;
            logLayout.RowCount = 6;
            logLayout.BackColor = Color.FromArgb(61, 46, 137);
            logLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            logLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            logLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
            logLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            logLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            logLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            consoleShell.Controls.Add(logLayout);
            Label outTitle = new Label();
            outTitle.Text = AppText.T("CONSOLE AVANZATA  /  LOG IN TEMPO REALE");
            outTitle.Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold);
            outTitle.ForeColor = Color.FromArgb(108, 229, 255);
            outTitle.Dock = DockStyle.Fill;
            outTitle.TextAlign = ContentAlignment.MiddleLeft;
            logLayout.Controls.Add(outTitle, 0, 0);
            activityLabel = new Label();
            activityLabel.Text = AppText.T("Sistema pronto · seleziona un modulo");
            activityLabel.ForeColor = Color.FromArgb(166, 181, 220);
            activityLabel.Dock = DockStyle.Fill;
            activityLabel.TextAlign = ContentAlignment.MiddleLeft;
            logLayout.Controls.Add(activityLabel, 0, 1);
            activityProgress = new ProgressBar();
            activityProgress.Dock = DockStyle.Fill;
            activityProgress.Style = ProgressBarStyle.Marquee;
            activityProgress.MarqueeAnimationSpeed = 28;
            activityProgress.Visible = false;
            logLayout.Controls.Add(activityProgress, 0, 2);
            stopButton = new NeonActionButton();
            stopButton.Text = AppText.T("■  INTERROMPI OPERAZIONE");
            stopButton.Dock = DockStyle.Fill;
            stopButton.FlatStyle = FlatStyle.Flat;
            stopButton.FlatAppearance.BorderSize = 0;
            stopButton.BackColor = Color.FromArgb(116, 33, 70);
            stopButton.ForeColor = Color.White;
            stopButton.Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold);
            stopButton.Enabled = false;
            stopButton.Click += StopButton_Click;
            logLayout.Controls.Add(stopButton, 0, 3);
            output = new RichTextBox();
            output.Dock = DockStyle.Fill;
            output.ReadOnly = true;
            output.BackColor = Color.FromArgb(55, 43, 128);
            output.ForeColor = Color.FromArgb(235, 228, 255);
            output.BorderStyle = BorderStyle.None;
            output.Font = new Font("Consolas", 9.2F);
            output.WordWrap = false;
            logLayout.Controls.Add(output, 0, 4);
            Label log = new Label();
            log.Text = "●  Log: Desktop\\SpeedBooster_Log.txt";
            log.ForeColor = Color.FromArgb(132, 146, 183);
            log.Dock = DockStyle.Fill;
            log.TextAlign = ContentAlignment.MiddleLeft;
            logLayout.Controls.Add(log, 0, 5);
            searchEntries = BuildSearchIndex();
            ShowMenu();
            WriteLog("Sessione avviata.");
        }

        private void OpenAuthorGithub(object sender, EventArgs e)
        {
            try { Process.Start(new ProcessStartInfo("https://github.com/9337progame") { UseShellExecute = true }); }
            catch (Exception ex) { MessageBox.Show(this, AppText.Choose("Impossibile aprire il profilo GitHub.\n\n", "Unable to open the GitHub profile.\n\n") + ex.Message, "Windows Speed Booster", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F11) return;
            ToggleFullScreen();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void ToggleFullScreen()
        {
            SuspendLayout();
            if (!fullScreen)
            {
                savedBorderStyle = FormBorderStyle;
                savedWindowState = WindowState;
                savedBounds = Bounds;
                savedMinimumSize = MinimumSize;
                MinimumSize = Size.Empty;
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.FromControl(this).Bounds;
                fullScreen = true;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = savedBorderStyle;
                MinimumSize = savedMinimumSize;
                Bounds = savedBounds;
                WindowState = savedWindowState;
                fullScreen = false;
            }
            ResumeLayout(true);
        }

        private PremiumButton MakeButton(string text, Color color)
        {
            PremiumButton b = new PremiumButton();
            b.Text = AppText.T(text);
            b.AccentColor = color;
            b.ModuleIndex = ExtractIndex(text);
            b.ModuleMode = galleryMode && b.ModuleIndex > 0;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            return b;
        }

        private void OpenPage(string pageTitle, string pageNote)
        {
            galleryMode = false;
            title.Text = AppText.T(pageTitle);
            note.Text = AppText.T(pageNote);
            options.Controls.Clear();
            options.FlowDirection = FlowDirection.TopDown;
            options.WrapContents = false;
            optionsViewport.AutoScrollPosition = new Point(0, 0);
            showcase.FeatureTitle = AppText.T(pageTitle);
            showcase.FeatureNote = AppText.T(pageNote);
            showcase.FeatureIndex = activeModule;
            showcase.Invalidate();
            RefreshOptionLayout();
        }

        private void OpenGallery()
        {
            galleryMode = true;
            activeModule = 3;
            title.Text = AppText.T("MENU DI OTTIMIZZAZIONE V14.0");
            note.Text = AppText.T("15 moduli di ottimizzazione. Passa il mouse sui moduli e scegli la funzione da applicare.");
            options.Controls.Clear();
            options.FlowDirection = FlowDirection.LeftToRight;
            options.WrapContents = true;
            optionsViewport.AutoScrollPosition = new Point(0, 0);
            showcase.FeatureIndex = 3;
            showcase.FeatureTitle = AppText.T("CENTRO DI CONTROLLO");
            showcase.FeatureNote = AppText.T("Tutti i tweak del menu originale sono disponibili qui. Seleziona un modulo per procedere.");
            showcase.Invalidate();
            RefreshOptionLayout();
        }

        private void Add(string text, Action action)
        {
            PremiumButton b = MakeButton(text, AccentFor(ExtractIndex(text)));
            b.Click += delegate
            {
                if (busy) return;
                if (b.ModuleIndex > 0)
                {
                    activeModule = b.ModuleIndex;
                    showcase.FeatureIndex = activeModule;
                    showcase.FeatureTitle = AppText.T(PremiumButton.CleanTitle(text)).ToUpperInvariant();
                    showcase.FeatureNote = AppText.T("Modulo selezionato. Le opzioni e l'output dell'operazione appariranno qui senza chiudere l'applicazione.");
                    showcase.Invalidate();
                }
                action();
            };
            PlaceOption(b);
        }

        private void PlaceOption(Button button)
        {
            options.Controls.Add(button);
            RefreshOptionLayout();
        }

        private static void optionsMarginReset(Control control)
        {
            control.Margin = new Padding(0);
        }

        private void RefreshOptionLayout()
        {
            if (options == null || options.IsDisposed || optionsViewport == null || optionsViewport.IsDisposed) return;
            int viewportWidth = Math.Max(280, optionsViewport.ClientSize.Width - 2);
            options.Width = viewportWidth;
            int usable = Math.Max(280, viewportWidth - options.Padding.Horizontal - 8);
            foreach (Control control in options.Controls)
            {
                PremiumButton b = control as PremiumButton;
                if (galleryMode && b != null && b.ModuleIndex > 0)
                {
                    int columns = usable >= 760 ? 3 : (usable >= 480 ? 2 : 1);
                    int gap = 12;
                    control.Width = Math.Max(220, (usable - gap * (columns - 1)) / columns);
                    control.Height = 138;
                    control.Margin = new Padding(0, 0, gap, 12);
                    b.ModuleMode = true;
                }
                else if (galleryMode && b != null && b.ModuleIndex == 0)
                {
                    control.Width = usable - 6;
                    control.Height = 50;
                    control.Margin = new Padding(0, 1, 8, 8);
                    b.ModuleMode = false;
                }
                else
                {
                    control.Width = usable - 4;
                    control.Height = 78;
                    control.Margin = new Padding(0, 0, 8, 11);
                    if (b != null) b.ModuleMode = false;
                }
            }
            options.PerformLayout();
            Size preferred = options.GetPreferredSize(new Size(viewportWidth, 0));
            options.Height = Math.Max(preferred.Height, options.Padding.Vertical + 1);
        }

        private void AddRun(string text, string script, bool confirm)
        {
            Add(text, delegate { Run(text, script, confirm); });
        }

        private void AddBack(string text, Action back)
        {
            PremiumButton b = MakeButton(text, Color.FromArgb(98, 108, 148));
            b.ModuleIndex = 0;
            b.ModuleMode = false;
            b.Click += delegate { if (!busy) back(); };
            PlaceOption(b);
        }

        private void ShowMenu()
        {
            searchMode = false;
            OpenGallery();
            Add("[1]  Memoria Virtuale (Calcolo Automatico)", ShowMemory);
            Add("[2]  Gestione Core CPU (Boot)", ShowBoot);
            Add("[3]  Effetti Visivi", ShowVisual);
            Add("[4]  Network Boost (TCP + Reset Stack)", delegate { Run("Network Boost (TCP + Reset Stack)", NetworkBoost(), true); });
            Add("[5]  Scanner Hardware Avanzato", delegate { Run("Scanner Hardware Avanzato", HardwareScan(), false); });
            Add("[6]  Piano di Alimentazione", ShowPower);
            Add("[7]  Ottimizzazione Disco (HDD/SSD)", ShowDisk);
            Add("[8]  Servizi Inutili (Disabilita/Ripristina)", ShowServices);
            Add("[9]  Pulizia Profonda (Manuale)", ShowManualCleanup);
            Add("[10] Rapporto Prestazioni Sistema", delegate { Run("Rapporto Prestazioni Sistema", PerformanceReport(), false); });
            Add("[11] Matrix Mode", delegate { Run("Matrix Mode", "while($true){ $line=(1..8|ForEach-Object{Get-Random -Minimum 0 -Maximum 10}) -join ' '; Write-Output $line; Start-Sleep -Milliseconds 30 }", false); });
            Add("[12] Tweaks Avanzati e Segreti", ShowAdvanced);
            Add("[13] Funzioni Segrete e Strumenti Pro", ShowProTools);
            Add("[14] Assistente IA (richiede token Groq gratuito)", ShowAiIntro);
            Add("[15] Microsoft Activation Scripts (MAS) - Attivazione Windows/Office", ShowActivation);
            Add("[0]  ESCI", delegate { Close(); });
            if (!String.IsNullOrWhiteSpace(tweakSearch.Query)) ShowSearchResults(tweakSearch.Query);
        }

        private void ApplySearch(string query)
        {
            query = (query ?? String.Empty).Trim();
            if (String.IsNullOrEmpty(query))
            {
                if (searchMode || !galleryMode) ShowMenu();
                return;
            }
            ShowSearchResults(query);
        }

        private void ShowSearchResults(string query)
        {
            query = (query ?? String.Empty).Trim();
            if (String.IsNullOrEmpty(query)) { ShowMenu(); return; }
            searchMode = true;
            galleryMode = false;
            title.Text = AppText.T("RICERCA GLOBALE DELLE FUNZIONI");
            options.Controls.Clear();
            options.FlowDirection = FlowDirection.TopDown;
            options.WrapContents = false;
            optionsViewport.AutoScrollPosition = new Point(0, 0);
            showcase.FeatureIndex = 0;
            showcase.FeatureTitle = AppText.T("RICERCA FUNZIONI");
            showcase.FeatureNote = AppText.T("Apri il risultato desiderato: l’app ti porterà direttamente alla funzione o alla relativa schermata operativa.");
            showcase.Invalidate();

            int matches = 0;
            foreach (SearchEntry entry in searchEntries)
            {
                if (!MatchesSearch(entry, query)) continue;
                SearchEntry selected = entry;
                PremiumButton result = MakeButton("[" + selected.ModuleIndex + "] " + AppText.T(selected.ModuleName) + "  ›  " + AppText.T(selected.Title), AccentFor(selected.ModuleIndex));
                result.ModuleIndex = 0;
                result.ModuleMode = false;
                result.Click += delegate
                {
                    if (busy) return;
                    tweakSearch.ClearQuery();
                    selected.Navigate();
                };
                PlaceOption(result);
                matches++;
            }

            note.Text = AppText.Choose("Ricerca globale: \"", "Global search: \"") + query + "\" · " + matches + (matches == 1 ? AppText.Choose(" funzione trovata.", " function found.") : AppText.Choose(" funzioni trovate.", " functions found."));
            if (matches == 0)
            {
                PremiumButton none = MakeButton(AppText.T("Nessuna funzione corrisponde alla ricerca"), Color.FromArgb(98, 108, 148));
                none.ModuleIndex = 0;
                none.Enabled = false;
                PlaceOption(none);
            }
            AddBack("[0] Torna al menu", delegate { tweakSearch.ClearQuery(); });
            RefreshOptionLayout();
        }

        private static bool MatchesSearch(SearchEntry entry, string query)
        {
            string searchable = (AppText.T(entry.ModuleName) + " " + AppText.T(entry.Title) + " " + entry.Keywords + " " + SearchKeywords(entry.ModuleIndex)).ToUpperInvariant();
            string[] tokens = query.ToUpperInvariant().Split(new char[] { ' ', '-', '_', '/', '\\', '+', '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                if (searchable.IndexOf(token, StringComparison.Ordinal) < 0) return false;
            }
            return true;
        }

        private List<SearchEntry> BuildSearchIndex()
        {
            List<SearchEntry> index = new List<SearchEntry>();
            AddSearch(index, 1, "Memoria Virtuale", "Memoria Virtuale (apri opzioni)", "RAM PAGEFILE PAGING CALCOLO AUTOMATICO", ShowMemory);
            AddSearch(index, 1, "Memoria Virtuale", "Imposta valori consigliati automaticamente", "RAM PAGEFILE AUTOMATICO CONSIGLIATI", ShowMemory);
            AddSearch(index, 1, "Memoria Virtuale", "Inserisci valori manualmente", "RAM PAGEFILE MIN MAX MB", ShowMemory);
            AddSearch(index, 1, "Memoria Virtuale", "Ripristina gestione automatica Windows", "RAM PAGEFILE DEFAULT", ShowMemory);
            AddSearch(index, 2, "Gestione Core CPU", "Core da allocare al boot", "CPU PROCESSORE NUMPROC AVVIO AUTO TUTTI", ShowBoot);
            AddSearch(index, 3, "Effetti Visivi", "Prestazioni massime (disabilita tutto)", "GRAFICA ANIMAZIONI TRASPARENZA", ShowVisual);
            AddSearch(index, 3, "Effetti Visivi", "Bilanciato (consigliato)", "GRAFICA DEFAULT", ShowVisual);
            AddSearch(index, 3, "Effetti Visivi", "Aspetto migliore (tutto abilitato)", "GRAFICA ANIMAZIONI", ShowVisual);
            AddSearch(index, 4, "Network Boost", "Network Boost (TCP + Reset Stack)", "RETE INTERNET WIFI DNS PING WINSOCK IPCONFIG", delegate { Run("Network Boost (TCP + Reset Stack)", NetworkBoost(), true); });
            AddSearch(index, 5, "Scanner Hardware", "Scanner Hardware Avanzato", "CPU GPU RAM DISCO SISTEMA", delegate { Run("Scanner Hardware Avanzato", HardwareScan(), false); });
            AddSearch(index, 6, "Piano di Alimentazione", "Prestazioni Elevate", "POWER ENERGIA BATTERIA", ShowPower);
            AddSearch(index, 6, "Piano di Alimentazione", "Bilanciato (default Windows)", "POWER ENERGIA DEFAULT", ShowPower);
            AddSearch(index, 6, "Piano di Alimentazione", "Risparmio Energetico", "POWER BATTERIA", ShowPower);
            AddSearch(index, 6, "Piano di Alimentazione", "Massima Prestazione Assoluta (Ultimate)", "ULTIMATE PERFORMANCE POWER", ShowPower);
            AddSearch(index, 7, "Ottimizzazione Disco", "Ottimizza disco C: (Defrag HDD o TRIM SSD)", "HDD SSD TRIM DEFRAG STORAGE", ShowDisk);
            AddSearch(index, 7, "Ottimizzazione Disco", "Pianifica controllo errori C: (chkdsk)", "DISCO CHKDSK ERRORI AVVIO", ShowDisk);
            AddSearch(index, 7, "Ottimizzazione Disco", "Comprimi OS disco C:", "DISCO COMPATTA SPAZIO", ShowDisk);
            AddSearch(index, 8, "Servizi Inutili", "Disabilita servizi inutili", "SYSMAIN DIAGTRACK WSEARCH FAX RAM CPU", ShowServices);
            AddSearch(index, 8, "Servizi Inutili", "Ripristina servizi ai valori default", "SYSMAIN DIAGTRACK WSEARCH FAX DEFAULT", ShowServices);
            AddSearch(index, 9, "Pulizia Profonda", "Apri Pulizia Disco di Windows", "CLEANMGR STORAGE SENSE", ShowManualCleanup);
            AddSearch(index, 9, "Pulizia Profonda", "Analizza e pulisci componenti Windows (DISM)", "DISM COMPONENT STORE RESTORE HEALTH", ShowManualCleanup);
            AddSearch(index, 9, "Pulizia Profonda", "Pulizia cache browser", "CHROME EDGE FIREFOX CACHE", ShowManualCleanup);
            AddSearch(index, 10, "Rapporto Prestazioni", "Rapporto Prestazioni Sistema", "CPU RAM DISCO UPTIME REPORT", delegate { Run("Rapporto Prestazioni Sistema", PerformanceReport(), false); });
            AddSearch(index, 11, "Matrix Mode", "Matrix Mode", "TERMINALE EFFETTO", delegate { Run("Matrix Mode", "while($true){ $line=(1..8|ForEach-Object{Get-Random -Minimum 0 -Maximum 10}) -join ' '; Write-Output $line; Start-Sleep -Milliseconds 30 }", false); });
            AddSearch(index, 12, "Tweaks Avanzati", "Modalita' Gaming Totale", "GAME MODE GAMEBAR HPET GAMING", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita Algoritmo di Nagle", "RETE LATENZA TCP ACK", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita Core Parking", "CPU PROCESSORE CORE POWER", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Timer di Precisione 0.5ms", "INPUT LAG AUDIO TIMER", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "GPU Hardware Scheduling (HAGS)", "GPU GRAFICA GAMING", ShowHags);
            AddSearch(index, 12, "Tweaks Avanzati", "Abilita HAGS", "GPU HARDWARE SCHEDULING GRAFICA", ShowHags);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita HAGS", "GPU HARDWARE SCHEDULING GRAFICA", ShowHags);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita Telemetria Profonda", "PRIVACY DIAGTRACK TASK SCHEDULATI", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Ottimizzazione SSD Avanzata", "SSD TRIM INDEXING LAST ACCESS", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Priorita' App in Primo Piano", "CPU GAMING EDITING RENDERING", ShowForegroundPriority);
            AddSearch(index, 12, "Tweaks Avanzati", "Massima priorita' primo piano", "CPU GAMING EDITING", ShowForegroundPriority);
            AddSearch(index, 12, "Tweaks Avanzati", "Priorita' bilanciata", "CPU DEFAULT WINDOWS", ShowForegroundPriority);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita Power Throttling", "CPU PIENA POTENZA PERFORMANCE", ShowPowerThrottling);
            AddSearch(index, 12, "Tweaks Avanzati", "Riabilita Power Throttling", "CPU DEFAULT PERFORMANCE", ShowPowerThrottling);
            AddSearch(index, 12, "Tweaks Avanzati", "Ottimizza IRQ e Interrupt Affinity", "GAMING AUDIO RETE GPU", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Ripristina tutti i tweaks ai valori default", "RESET DEFAULT RIPRISTINO", ShowAdvanced);
            AddSearch(index, 12, "Tweaks Avanzati", "Profilo Gaming per singolo gioco", "GAME EXE PROCESS PRIORITY PROFILO TEMPORANEO", ShowPerGameProfile);
            AddSearch(index, 12, "Tweaks Avanzati", "Profilo Notebook e Batteria", "LAPTOP BATTERY ENERGIA POWER BILANCIATO", ShowLaptopProfile);
            AddSearch(index, 12, "Tweaks Avanzati", "Dashboard Privacy", "PRIVACY TELEMETRIA ADVERTISING SETTINGS", ShowPrivacyDashboard);
            AddSearch(index, 12, "Tweaks Avanzati", "Centro Ripristino e Annullamento", "UNDO RESTORE BACKUP NOTEBOOK PIANO", ShowRestoreCenter);
            AddSearch(index, 12, "Tweaks Avanzati", "Ripristino MPO (flicker / stutter)", "MPO OVERLAY TEST MODE FLICKER BLACK SCREEN STUTTER GPU", ShowMpoRecovery);
            AddSearch(index, 12, "Tweaks Avanzati", "Associazione GPU Alte Prestazioni per gioco", "GPU HIGH PERFORMANCE PREFERENCE GAME EXE DEDICATA", ShowGpuBinding);
            AddSearch(index, 12, "Tweaks Avanzati", "Disabilita accelerazione mouse (Enhance Pointer Precision)", "MOUSE RAW INPUT POINTER PRECISION ACCELERAZIONE EPP", ShowRawMouse);
            AddSearch(index, 13, "Strumenti Pro", "Mostra password WiFi salvate", "WLAN SSID RETE PASSWORD", ShowWifi);
            AddSearch(index, 13, "Strumenti Pro", "Inserisci rete WiFi per visualizzare la password", "WLAN SSID KEY CONTENT", ShowWifi);
            AddSearch(index, 13, "Strumenti Pro", "Recupera Product Key di Windows", "LICENZA BIOS UEFI DIGITALPRODUCTID", delegate { Run("Recupera Product Key di Windows", ProductKey(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Dashboard live CPU/RAM/DISCO", "TEMPO REALE PROCESSI", delegate { Run("Dashboard live CPU/RAM/DISCO in tempo reale", Dashboard(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Benchmark velocita' disco", "LETTURA SCRITTURA MB S", delegate { Run("Benchmark velocita' disco", Benchmark(), true); });
            AddSearch(index, 13, "Strumenti Pro", "Scanner processi sospetti e malware", "PROCESSI TCP CONNESSIONI", delegate { Run("Scanner processi sospetti e malware", ProcessScanner(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Info segrete sistema", "SERIALE UUID MAC BIOS IP FIREWALL", delegate { Run("Info segrete sistema", SecretInfo(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Startup Manager", "AVVIO AUTOMATICO RUN TASK SCHEDULATI", StartupManager);
            AddSearch(index, 13, "Strumenti Pro", "Visualizza programmi di avvio automatico", "STARTUP RUN TASK SCHEDULATI", StartupManager);
            AddSearch(index, 13, "Strumenti Pro", "Rimuovi voce dal registro Run", "STARTUP AVVIO AUTOMATICO", StartupManager);
            AddSearch(index, 13, "Strumenti Pro", "Mappa rete locale", "LAN GATEWAY ARP DISPOSITIVI CONNESSI", delegate { Run("Mappa rete locale", NetMap(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Analisi dipendenze DLL di un eseguibile", "EXE LIBRERIE DLL ANALYZER", DllAnalysis);
            AddSearch(index, 13, "Strumenti Pro", "Genera report completo sistema", "DESKTOP CPU GPU RAM STORAGE RETE", delegate { Run("Genera report completo sistema", FullReport(), false); });
            AddSearch(index, 13, "Strumenti Pro", "Diagnostica Integrità Windows", "SYSTEM HEALTH SFC DISM VERIFY SCAN", ShowSystemHealth);
            AddSearch(index, 13, "Strumenti Pro", "Analizzatore Spazio Intelligente", "SMART STORAGE DISK TEMP DOWNLOADS CACHE", ShowSmartStorage);
            AddSearch(index, 13, "Strumenti Pro", "Stato Driver e Dispositivi", "DRIVER DEVICE PNP HARDWARE HEALTH", ShowDriverHealth);
            AddSearch(index, 13, "Strumenti Pro", "Suite Diagnostica Rete", "NETWORK DIAGNOSTIC DNS GATEWAY PING ADAPTER", ShowNetworkDiagnostics);
            AddSearch(index, 13, "Strumenti Pro", "Snapshot Prestazioni", "CPU RAM DISK PERFORMANCE SNAPSHOT", ShowPerformanceSnapshot);
            AddSearch(index, 13, "Strumenti Pro", "Controllo Aggiornamenti e Riavvio", "WINDOWS UPDATE RESTART PENDING", ShowUpdateRestartCheck);
            AddSearch(index, 14, "Assistente IA", "Assistente IA (configurazione e chat)", "AI GROQ TOKEN API ASK_AI", ShowAiIntro);
            AddSearch(index, 15, "Microsoft Activation Scripts", "Apri attivazione Windows/Office", "MAS LICENZA PRODUCT KEY", ShowActivation);
            return index;
        }

        private static void AddSearch(List<SearchEntry> index, int module, string moduleName, string title, string keywords, Action navigate)
        {
            SearchEntry entry = new SearchEntry();
            entry.ModuleIndex = module;
            entry.ModuleName = moduleName;
            entry.Title = title;
            entry.Keywords = keywords;
            entry.Navigate = navigate;
            index.Add(entry);
        }

        private static string SearchKeywords(int module)
        {
            switch (module)
            {
                case 1: return "RAM PAGEFILE PAGING MEMORIA VIRTUALE";
                case 2: return "CPU PROCESSORE CORE BOOT AVVIO";
                case 3: return "GRAFICA ANIMAZIONI TRASPARENZA EFFETTI";
                case 4: return "RETE INTERNET WIFI DNS TCP PING WINSOCK";
                case 5: return "HARDWARE GPU RAM DISCO SCANNER";
                case 6: return "ENERGIA BATTERIA POWER PERFORMANCE ALIMENTAZIONE";
                case 7: return "HDD SSD TRIM DEFRAG DISCO STORAGE";
                case 8: return "SERVIZI WINDOWS UPDATE SPOOLER DISABILITA RIPRISTINA";
                case 9: return "TEMP CACHE PREFETCH PULIZIA FILE";
                case 10: return "REPORT PRESTAZIONI CPU RAM SISTEMA";
                case 11: return "MATRIX TERMINALE EFFETTO";
                case 12: return "AVANZATI HAGS PRIORITA THROTTLING RIPRISTINO";
                case 13: return "PRO WIFI STARTUP DLL KEY DASHBOARD BENCHMARK PROCESSI RETE";
                case 14: return "IA AI GROQ ASSISTENTE CHAT";
                case 15: return "ACTIVATION ATTIVAZIONE LICENZA WINDOWS OFFICE MAS";
                default: return "";
            }
        }

        private static int ExtractIndex(string source)
        {
            if (String.IsNullOrEmpty(source) || !source.StartsWith("[")) return 0;
            int end = source.IndexOf(']');
            int value;
            return end > 1 && Int32.TryParse(source.Substring(1, end - 1).Trim(), out value) ? value : 0;
        }

        private static Color AccentFor(int index)
        {
            Color[] palette = new Color[]
            {
                Color.FromArgb(255, 188, 60), Color.FromArgb(255, 74, 87), Color.FromArgb(57, 219, 255),
                Color.FromArgb(67, 234, 138), Color.FromArgb(255, 184, 56), Color.FromArgb(255, 151, 52),
                Color.FromArgb(45, 220, 245), Color.FromArgb(184, 96, 255), Color.FromArgb(51, 176, 255),
                Color.FromArgb(255, 79, 87), Color.FromArgb(55, 157, 255), Color.FromArgb(195, 80, 255),
                Color.FromArgb(255, 193, 60), Color.FromArgb(58, 206, 255), Color.FromArgb(255, 198, 57)
            };
            return index >= 1 && index <= palette.Length ? palette[index - 1] : Color.FromArgb(121, 135, 184);
        }

        private void ShowMemory()
        {
            OpenPage("MEMORIA VIRTUALE (PAGING)", "Calcolo valori ottimali: RAM fisica e valore massimo consigliato. Il comportamento corrisponde alle tre scelte del batch.");
            AddRun("[1] Imposta valori consigliati automaticamente", "$ram=[math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory/1MB); if(!$ram){$ram=4096}; $max=[math]::Round($ram*1.5); if(!$max){$max=6144}; $cs=Get-CimInstance Win32_ComputerSystem; Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$false}; $pf=Get-CimInstance Win32_PageFileSetting -ErrorAction SilentlyContinue; if($pf){Set-CimInstance -InputObject $pf -Property @{InitialSize=$ram;MaximumSize=$max}}else{New-CimInstance -ClassName Win32_PageFileSetting -Property @{Name='C:\\pagefile.sys';InitialSize=$ram;MaximumSize=$max}|Out-Null}; Write-Output ('[OK] Pagefile impostato: '+$ram+' - '+$max+' MB')", true);
            Add("[2] Inserisci valori manualmente", ManualPagefile);
            AddRun("[3] Ripristina gestione automatica Windows", "$cs=Get-CimInstance Win32_ComputerSystem; Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$true}; Write-Output '[OK] Gestione automatica ripristinata.'", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ManualPagefile()
        {
            string min = Prompt.Show(this, "Valore MIN (MB):", "Memoria virtuale", "");
            if (min == null) return;
            string max = Prompt.Show(this, "Valore MAX (MB):", "Memoria virtuale", "");
            if (max == null) return;
            int a, b;
            if (!Int32.TryParse(min, out a) || !Int32.TryParse(max, out b) || a <= 0 || a > b) { MessageBox.Show(this, AppText.T("Valore MIN o MAX non valido."), "Windows Speed Booster", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            Run("Memoria Virtuale - valori manuali", "$cs=Get-CimInstance Win32_ComputerSystem; Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile=$false}; $pf=Get-CimInstance Win32_PageFileSetting -ErrorAction SilentlyContinue; if($pf){Set-CimInstance -InputObject $pf -Property @{InitialSize=" + a + ";MaximumSize=" + b + "}}else{New-CimInstance -ClassName Win32_PageFileSetting -Property @{Name='C:\\pagefile.sys';InitialSize=" + a + ";MaximumSize=" + b + "}|Out-Null}; Write-Output '[OK] Pagefile impostato: " + a + " - " + b + " MB'", true);
        }

        private void ShowBoot()
        {
            OpenPage("GESTIONE CORE CPU (BOOT)", AppText.Choose("Core logici disponibili: ", "Available logical cores: ") + Environment.ProcessorCount + AppText.Choose(". Modificare il valore può rallentare il boot. Lascia 0 per usare tutti i core (consigliato).", ". Changing this value may slow down boot. Leave 0 to use all cores (recommended)."));
            Add("Core da allocare al boot (0=Auto/Tutti, M=Menu)", BootCore);
            AddBack("[M] Torna al menu", ShowMenu);
        }

        private void BootCore()
        {
            string x = Prompt.Show(this, "Core da allocare al boot (0=Auto/Tutti):", "Gestione Core CPU (Boot)", "0");
            if (x == null) return;
            int n;
            if (!Int32.TryParse(x, out n) || n < 0) { MessageBox.Show(this, AppText.T("Inserire un numero valido (0 per Auto/Tutti)."), "Windows Speed Booster", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            Run("Gestione Core CPU (Boot)", n == 0 ? "bcdedit /deletevalue numproc; Write-Output '[OK] Ripristinato uso automatico di tutti i core.'" : "bcdedit /set numproc " + n + "; Write-Output '[OK] Boot configurato con " + n + " core.'", true);
        }

        private void ShowVisual()
        {
            OpenPage("EFFETTI VISIVI WINDOWS", "Seleziona il profilo effetti visivi corrispondente al menu originale.");
            AddRun("[1] Prestazioni massime (disabilita tutto)", "New-Item 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' -Force|Out-Null; Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' VisualFXSetting 2; Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced' ListviewAlphaSelect 0 -ErrorAction SilentlyContinue; Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced' TaskbarAnimations 0 -ErrorAction SilentlyContinue; Write-Output '[OK] Effetti visivi: Prestazioni Massime'", true);
            AddRun("[2] Bilanciato (consigliato)", "New-Item 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' -Force|Out-Null; Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' VisualFXSetting 3; Write-Output '[OK] Effetti visivi: Bilanciato'", true);
            AddRun("[3] Aspetto migliore (tutto abilitato)", "New-Item 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' -Force|Out-Null; Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\VisualEffects' VisualFXSetting 0; Write-Output '[OK] Effetti visivi: Aspetto Migliore'", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowPower()
        {
            OpenPage("PIANO DI ALIMENTAZIONE", "Piano attuale e le quattro scelte del batch.");
            AddRun("[1] Prestazioni Elevate", "powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c; Write-Output '[OK] Piano: Prestazioni Elevate'", true);
            AddRun("[2] Bilanciato (default Windows)", "powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e; Write-Output '[OK] Piano: Bilanciato'", true);
            AddRun("[3] Risparmio Energetico", "powercfg /setactive a1841308-3541-4fab-bc81-f71556f20b4a; Write-Output '[OK] Piano: Risparmio Energetico'", true);
            AddRun("[4] Massima Prestazione Assoluta (Ultimate)", "powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61; $u=(powercfg /list|Select-String 'e9a42b02'|Select-Object -First 1).ToString(); if($u -match '([0-9a-fA-F-]{36})'){powercfg /setactive $matches[1];Write-Output '[OK] Piano Ultimate Performance attivato.'}else{powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c;Write-Output '[OK] Piano: Prestazioni Elevate attivato.'}", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowDisk()
        {
            OpenPage("OTTIMIZZAZIONE DISCO", "Rilevamento tipo disco e le tre opzioni per l’unità C:.");
            AddRun("[1] Ottimizza disco C: (Defrag HDD o TRIM SSD, automatico)", "try{Optimize-Volume -DriveLetter C -Verbose -ErrorAction Stop;Write-Output '[OK] Ottimizzazione completata con successo.'}catch{defrag C: /U /V;Write-Output '[OK] Defrag completato.'}", true);
            AddRun("[2] Pianifica controllo errori C: (chkdsk al prossimo avvio)", "cmd.exe /c 'echo Y|chkdsk C: /f /r'; fsutil dirty set C:; Write-Output '[OK] Controllo disco pianificato al prossimo riavvio.'", true);
            AddRun("[3] Comprimi OS disco C: (libera spazio, lento)", "compact /CompactOs:always; Write-Output '[OK] Compressione completata.'", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowServices()
        {
            OpenPage("GESTIONE SERVIZI NON ESSENZIALI", "Servizi gestiti: SysMain, DiagTrack, WSearch e Fax.");
            AddRun("[1] Disabilita servizi inutili (ottimizza RAM/CPU)", "foreach($s in 'SysMain','DiagTrack','Fax'){sc.exe config $s start= disabled;Stop-Service $s -Force -ErrorAction SilentlyContinue;Write-Output ('[OK] '+$s+' disabilitato.')};sc.exe config WSearch start= delayed-auto;Write-Output '[OK] WSearch impostato ad avvio ritardato.'", true);
            AddRun("[2] Ripristina servizi ai valori default", "foreach($s in 'SysMain','DiagTrack','Fax','WSearch'){sc.exe config $s start= auto;Start-Service $s -ErrorAction SilentlyContinue;Write-Output ('[OK] '+$s+' ripristinato.')}", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowManualCleanup()
        {
            OpenPage("PULIZIA PROFONDA MANUALE", "Le stesse tre opzioni del batch per Pulizia disco, componenti Windows e cache browser.");
            AddRun("[1] Apri Pulizia Disco di Windows (GUI)", "if(Get-Command cleanmgr.exe -ErrorAction SilentlyContinue){Start-Process cleanmgr.exe -ArgumentList '/d C:';Write-Output '[*] Apertura Pulizia Disco...'}else{Start-Process 'ms-settings:storagesense';Write-Output '[!] cleanmgr non disponibile: aperto Storage Sense.'}", false);
            AddRun("[2] Analizza e pulisci componenti Windows (DISM)", "DISM /Online /Cleanup-Image /AnalyzeComponentStore; DISM /Online /Cleanup-Image /StartComponentCleanup; if($LASTEXITCODE -ne 0){DISM /Online /Cleanup-Image /RestoreHealth}", true);
            AddRun("[3] Pulizia cache browser (Chrome/Edge/Firefox)", "Remove-Item (Join-Path $env:LOCALAPPDATA 'Google\\Chrome\\User Data\\Default\\Cache') -Recurse -Force -ErrorAction SilentlyContinue; Remove-Item (Join-Path $env:LOCALAPPDATA 'Microsoft\\Edge\\User Data\\Default\\Cache') -Recurse -Force -ErrorAction SilentlyContinue; Get-ChildItem (Join-Path $env:APPDATA 'Mozilla\\Firefox\\Profiles') -Directory -ErrorAction SilentlyContinue|ForEach-Object{Remove-Item (Join-Path $_.FullName 'cache2') -Recurse -Force -ErrorAction SilentlyContinue};Write-Output '[OK] Cache browser eliminate.'", true);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowAdvanced()
        {
            OpenPage("TWEAKS AVANZATI E SEGRETI", "Le dieci opzioni avanzate originali restano invariate; qui sotto sono presenti anche moduli aggiuntivi separati e reversibili.");
            AddRun("[1]  Modalita' Gaming Totale (Game Mode + HPET off + GameBar off)", Gaming(), true);
            AddRun("[2]  Disabilita Algoritmo di Nagle (riduce latenza di rete)", Nagle(), true);
            AddRun("[3]  Disabilita Core Parking (tutti i core sempre attivi)", "powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 100;powercfg /setdcvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 100;powercfg /apply;$g=(powercfg /list|ForEach-Object{if($_ -match '([0-9a-fA-F-]{36})'){$matches[1]}}|Where-Object{$_});foreach($x in $g){powercfg /setacvalueindex $x SUB_PROCESSOR CPMINCORES 100 2>$null;powercfg /setdcvalueindex $x SUB_PROCESSOR CPMINCORES 100 2>$null};Write-Output '[OK] Core Parking disabilitato sul piano corrente e su tutti i piani disponibili.'", true);
            AddRun("[4]  Timer di Precisione 0.5ms (riduce input lag)", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\kernel' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\kernel' GlobalTimerResolutionRequests 1 -Type DWord;bcdedit /set tscsyncpolicy Enhanced;Add-Type -TypeDefinition 'using System; using System.Runtime.InteropServices; public class WinTimer{[DllImport(\"winmm.dll\")] public static extern int timeBeginPeriod(int p);}';[WinTimer]::timeBeginPeriod(1)|Out-Null;Write-Output '[OK] Timer impostato a 1ms via API (attivo fino al riavvio).'", true);
            Add("[5]  GPU Hardware Scheduling (HAGS - migliora gaming)", ShowHags);
            AddRun("[6]  Disabilita Telemetria Profonda (registry + task schedulati)", Telemetry(), true);
            AddRun("[7]  Ottimizzazione SSD Avanzata (8.3, last access, ecc.)", "fsutil behavior set disable8dot3 1;fsutil behavior set disablelastaccess 1;fsutil behavior set memoryusage 2;$t=fsutil behavior query DisableDeleteNotify;if($t -notmatch '= 0'){fsutil behavior set DisableDeleteNotify 0};try{$d=Get-CimInstance Win32_Volume -Filter 'DriveLetter=\"C:\"';if($d){Set-CimInstance -InputObject $d -Property @{IndexingEnabled=$false}}}catch{};Write-Output '[OK] Ottimizzazione SSD completata.'", true);
            Add("[8]  Priorita' App in Primo Piano (boost CPU)", ShowForegroundPriority);
            Add("[9]  Disabilita Power Throttling (CPU sempre a piena potenza)", ShowPowerThrottling);
            AddRun("[10] Ottimizza IRQ e Interrupt Affinity (gaming/audio)", Irq(), true);
            AddRun("[R]  Ripristina tutti i tweaks ai valori default", Restore(), true);
            Add("[11] Profilo Gaming per singolo gioco", ShowPerGameProfile);
            Add("[12] Profilo Notebook e Batteria", ShowLaptopProfile);
            Add("[13] Dashboard Privacy", ShowPrivacyDashboard);
            Add("[14] Centro Ripristino e Annullamento", ShowRestoreCenter);
            Add("[15] Ripristino MPO (flicker / stutter)", ShowMpoRecovery);
            Add("[16] Associazione GPU Alte Prestazioni per gioco", ShowGpuBinding);
            Add("[17] Disabilita accelerazione mouse (Enhance Pointer Precision)", ShowRawMouse);
            AddBack("[0]  Torna al menu", ShowMenu);
        }

        private void ShowPerGameProfile()
        {
            OpenPage("PROFILO GAMING PER SINGOLO GIOCO", "Avvia un gioco scelto dall’utente e applica priorità Alta solo a quel processo. Nessuna modifica permanente al sistema.");
            Add("[1] Seleziona gioco e avvia profilo temporaneo", SelectPerGameProfile);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void SelectPerGameProfile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = AppText.T("Seleziona l’eseguibile del gioco (.exe)");
            dialog.Filter = AppText.T("Eseguibili (*.exe)|*.exe|Tutti i file (*.*)|*.*");
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            string file = dialog.FileName;
            Run("Profilo Gaming per singolo gioco", "Write-Output '[*] Avvio gioco e applicazione profilo temporaneo...';$p=Start-Process -FilePath '" + Escape(file) + "' -PassThru;Start-Sleep -Milliseconds 700;try{$p.PriorityClass='High';Write-Output '[OK] Profilo gaming temporaneo applicato al processo: '+$p.ProcessName+' (Priorità High).'}catch{Write-Output '[!] Gioco avviato, ma la priorità non è stata modificata: '+$_.Exception.Message}", true);
        }

        private void ShowLaptopProfile()
        {
            OpenPage("PROFILO NOTEBOOK E BATTERIA", "Funzioni dedicate ai portatili: controlla batteria e piano di alimentazione, salva il piano attuale e permette il ripristino.");
            AddRun("[1] Stato batteria e piano attuale", "Write-Output '[*] Rilevamento batteria e piano di alimentazione...';$b=Get-CimInstance Win32_Battery -ErrorAction SilentlyContinue;if($b){$b|ForEach-Object{'Batteria: '+$_.EstimatedChargeRemaining+'% | Stato: '+$_.BatteryStatus}}else{Write-Output '[!] Batteria non rilevata: il PC potrebbe essere un desktop.'};Write-Output '[*] Piano attivo:';powercfg /getactivescheme", false);
            AddRun("[2] Applica profilo notebook bilanciato (salva piano attuale)", "Write-Output '[*] Salvataggio piano attuale e applicazione profilo notebook...';$dir=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster';New-Item -ItemType Directory -Path $dir -Force|Out-Null;$old=(powercfg /getactivescheme|Select-String -Pattern '[0-9a-fA-F-]{36}'|Select-Object -First 1).Matches.Value;if($old){Set-Content -Path (Join-Path $dir 'laptop_profile.txt') -Value ('LaptopPreviousScheme='+$old) -Encoding UTF8};powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e;Write-Output '[OK] Profilo notebook bilanciato applicato. Piano precedente salvato.'", true);
            AddRun("[3] Apri impostazioni ufficiali batteria", "Start-Process 'ms-settings:batterysaver';Write-Output '[OK] Impostazioni batteria aperte.'", false);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void ShowPrivacyDashboard()
        {
            OpenPage("DASHBOARD PRIVACY", "Controllo trasparente delle impostazioni privacy: visualizza lo stato e apri le impostazioni ufficiali senza disattivazioni automatiche.");
            AddRun("[1] Analizza stato privacy e telemetria", "Write-Output '[*] Analisi impostazioni privacy e telemetria...';$paths='HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection';foreach($p in $paths){if(Test-Path $p){$v=Get-ItemProperty $p -ErrorAction SilentlyContinue;Write-Output ('Telemetria '+$p+': AllowTelemetry='+$v.AllowTelemetry)}else{Write-Output ('Telemetria '+$p+': non configurata')}};Get-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo' -ErrorAction SilentlyContinue|ForEach-Object{Write-Output ('Advertising ID enabled: '+$_.Enabled)}", false);
            AddRun("[2] Apri impostazioni privacy ufficiali", "Start-Process 'ms-settings:privacy';Write-Output '[OK] Impostazioni privacy aperte.'", false);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void ShowRestoreCenter()
        {
            OpenPage("CENTRO RIPRISTINO E ANNULLAMENTO", "Mostra il backup del piano notebook e consente di ripristinarlo. I tweak avanzati originali restano disponibili nella loro opzione R.");
            AddRun("[1] Mostra backup profilo notebook", "Write-Output '[*] Lettura backup profilo notebook...';$state=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster\\laptop_profile.txt';if(Test-Path $state){Get-Content $state}else{Write-Output '[!] Nessun backup notebook disponibile.'}", false);
            AddRun("[2] Ripristina piano notebook precedente", "Write-Output '[*] Verifica backup e ripristino piano notebook...';$state=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster\\laptop_profile.txt';if(!(Test-Path $state)){Write-Output '[!] Nessun piano precedente salvato.';return};$line=Get-Content $state|Where-Object{$_ -like 'LaptopPreviousScheme=*'}|Select-Object -First 1;if(!$line){Write-Output '[!] Backup piano non valido.';return};$guid=$line.Substring('LaptopPreviousScheme='.Length);if($guid -notmatch '^[0-9a-fA-F-]{36}$'){Write-Output '[!] GUID piano non valido.';return};powercfg /setactive $guid;Write-Output '[OK] Piano notebook precedente ripristinato.'", true);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void ShowMpoRecovery()
        {
            OpenPage("RIPRISTINO MPO (FLICKER / STUTTER)", "MPO può aiutare solo in caso di flicker, schermate nere o stutter grafico specifico. Il valore precedente viene salvato; è necessario riavviare.");
            AddRun("[1] Disabilita MPO (salva backup, riavvio richiesto)", "Write-Output '[*] Salvataggio valore MPO corrente...';$dir=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster';New-Item -ItemType Directory -Path $dir -Force|Out-Null;$state=Join-Path $dir 'mpo_backup.txt';$path='HKLM:\\SOFTWARE\\Microsoft\\Windows\\Dwm';$name='OverlayTestMode';New-Item -Path $path -Force|Out-Null;if(!(Test-Path $state)){if((Get-Item $path).Property -contains $name){Set-Content -Path $state -Value ('VALUE='+[int](Get-ItemPropertyValue -Path $path -Name $name)) -Encoding UTF8}else{Set-Content -Path $state -Value 'ABSENT' -Encoding UTF8}};Set-ItemProperty -Path $path -Name $name -Value 5 -Type DWord;Write-Output '[OK] MPO disabilitato. Riavvia il PC per applicare.'", true);
            AddRun("[2] Ripristina MPO dal backup", "Write-Output '[*] Ripristino MPO dal backup...';$state=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster\\mpo_backup.txt';$path='HKLM:\\SOFTWARE\\Microsoft\\Windows\\Dwm';$name='OverlayTestMode';if(!(Test-Path $state)){Write-Output '[!] Nessun backup MPO disponibile.';return};$v=(Get-Content $state -ErrorAction Stop|Select-Object -First 1);New-Item -Path $path -Force|Out-Null;if($v -eq 'ABSENT'){Remove-ItemProperty -Path $path -Name $name -ErrorAction SilentlyContinue}elseif($v -like 'VALUE=*'){$n=0;if([int]::TryParse($v.Substring(6),[ref]$n)){Set-ItemProperty -Path $path -Name $name -Value $n -Type DWord}else{Write-Output '[!] Backup MPO non valido.';return}}else{Write-Output '[!] Backup MPO non valido.';return};Write-Output '[OK] MPO ripristinato. Riavvia il PC per applicare.'", true);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void ShowGpuBinding()
        {
            OpenPage("ASSOCIAZIONE GPU AD ALTE PRESTAZIONI", "Scegli un eseguibile: Windows applicherà a quel solo gioco la preferenza GPU High Performance. Non modifica HAGS, priorità o driver.");
            Add("[1] Seleziona gioco e applica GPU High Performance", SelectGpuHighPerformance);
            Add("[2] Ripristina GPU predefinita per un gioco", RestoreGpuPreference);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void SelectGpuHighPerformance()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = AppText.T("Seleziona l’eseguibile del gioco per la preferenza GPU (.exe)");
            dialog.Filter = AppText.T("Eseguibili (*.exe)|*.exe|Tutti i file (*.*)|*.*");
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            string file = dialog.FileName;
            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(file)).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            Run("Associazione GPU Alte Prestazioni per gioco", "Write-Output '[*] Salvataggio preferenza GPU precedente...';$dir=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster';New-Item -ItemType Directory -Path $dir -Force|Out-Null;$exe='" + Escape(file) + "';$state=Join-Path $dir ('gpu_'+ '" + token + "' + '.txt');$reg='HKCU:\\Software\\Microsoft\\DirectX\\UserGpuPreferences';New-Item -Path $reg -Force|Out-Null;$item=Get-ItemProperty -Path $reg;if(!(Test-Path $state)){if($item.PSObject.Properties.Name -contains $exe){$raw=[string]$item.PSObject.Properties[$exe].Value;Set-Content -Path $state -Value ('VALUE='+[Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($raw))) -Encoding UTF8}else{Set-Content -Path $state -Value 'ABSENT' -Encoding UTF8}};Set-ItemProperty -Path $reg -Name $exe -Value 'GpuPreference=2;';Write-Output ('[OK] Preferenza GPU High Performance applicata a: '+$exe);Write-Output '[*] L’impostazione avrà effetto al prossimo avvio del gioco.'", true);
        }

        private void RestoreGpuPreference()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = AppText.T("Seleziona l’eseguibile del gioco per la preferenza GPU (.exe)");
            dialog.Filter = AppText.T("Eseguibili (*.exe)|*.exe|Tutti i file (*.*)|*.*");
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            string file = dialog.FileName;
            string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(file)).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            Run("Associazione GPU Alte Prestazioni per gioco", "Write-Output '[*] Ripristino preferenza GPU predefinita...';$exe='" + Escape(file) + "';$state=Join-Path $env:LOCALAPPDATA ('Windows Speed Booster\\gpu_'+ '" + token + "' + '.txt');$reg='HKCU:\\Software\\Microsoft\\DirectX\\UserGpuPreferences';if(!(Test-Path $state)){Write-Output '[!] Nessun backup GPU disponibile per questo eseguibile.';return};$v=(Get-Content $state -ErrorAction Stop|Select-Object -First 1);New-Item -Path $reg -Force|Out-Null;if($v -eq 'ABSENT'){Remove-ItemProperty -Path $reg -Name $exe -ErrorAction SilentlyContinue}elseif($v -like 'VALUE=*'){try{$raw=[Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($v.Substring(6)));Set-ItemProperty -Path $reg -Name $exe -Value $raw}catch{Write-Output '[!] Backup GPU non valido.';return}}else{Write-Output '[!] Backup GPU non valido.';return};Write-Output ('[OK] Preferenza GPU precedente ripristinata per: '+$exe)", true);
        }

        private void ShowRawMouse()
        {
            OpenPage("ACCELERAZIONE MOUSE RAW", "Disattiva Enhance Pointer Precision solo per l’utente corrente. I tre valori originali vengono salvati e possono essere ripristinati.");
            AddRun("[1] Disabilita accelerazione mouse (salva backup)", "Write-Output '[*] Salvataggio impostazioni mouse correnti...';$dir=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster';New-Item -ItemType Directory -Path $dir -Force|Out-Null;$state=Join-Path $dir 'mouse_acceleration_backup.txt';$key='HKCU:\\Control Panel\\Mouse';$names='MouseSpeed','MouseThreshold1','MouseThreshold2';$item=Get-ItemProperty -Path $key;if(!(Test-Path $state)){$lines=@();foreach($n in $names){if($item.PSObject.Properties.Name -contains $n){$lines+=($n+'=VALUE:'+[string]$item.PSObject.Properties[$n].Value)}else{$lines+=($n+'=ABSENT')}};Set-Content -Path $state -Value $lines -Encoding UTF8};Set-ItemProperty -Path $key -Name MouseSpeed -Value '0';Set-ItemProperty -Path $key -Name MouseThreshold1 -Value '0';Set-ItemProperty -Path $key -Name MouseThreshold2 -Value '0';Start-Process rundll32.exe -ArgumentList 'user32.dll,UpdatePerUserSystemParameters 1, True' -Wait -ErrorAction SilentlyContinue;Write-Output '[OK] Accelerazione mouse disabilitata per l’utente corrente.'", true);
            AddRun("[2] Ripristina accelerazione mouse dal backup", "Write-Output '[*] Ripristino impostazioni mouse dal backup...';$state=Join-Path $env:LOCALAPPDATA 'Windows Speed Booster\\mouse_acceleration_backup.txt';$key='HKCU:\\Control Panel\\Mouse';if(!(Test-Path $state)){Write-Output '[!] Nessun backup mouse disponibile.';return};foreach($line in Get-Content $state){$i=$line.IndexOf('=');if($i -lt 1){continue};$name=$line.Substring(0,$i);$data=$line.Substring($i+1);if($data -eq 'ABSENT'){Remove-ItemProperty -Path $key -Name $name -ErrorAction SilentlyContinue}elseif($data -like 'VALUE:*'){Set-ItemProperty -Path $key -Name $name -Value $data.Substring(6)}};Start-Process rundll32.exe -ArgumentList 'user32.dll,UpdatePerUserSystemParameters 1, True' -Wait -ErrorAction SilentlyContinue;Write-Output '[OK] Accelerazione mouse ripristinata.'", true);
            AddBack("[0] Torna ai Tweaks Avanzati", ShowAdvanced);
        }

        private void ShowHags()
        {
            OpenPage("GPU HARDWARE SCHEDULING (HAGS)", "HAGS sposta la gestione della memoria GPU dalla CPU alla GPU. Richiede GPU recente e riavvio.");
            AddRun("[1] Abilita HAGS", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' HwSchMode 2 -Type DWord;Write-Output '[OK] HAGS abilitato. Riavvia il PC per applicare.'", true);
            AddRun("[2] Disabilita HAGS", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' HwSchMode 1 -Type DWord;Write-Output '[OK] HAGS disabilitato. Riavvia il PC per applicare.'", true);
            AddBack("[0] Annulla", ShowAdvanced);
        }

        private void ShowForegroundPriority()
        {
            OpenPage("PRIORITA' APP IN PRIMO PIANO", "Aumenta la quota CPU assegnata all’app attiva. Utile per gaming, editing video e rendering.");
            AddRun("[1] Massima priorita' primo piano (gaming/editing)", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' Win32PrioritySeparation 38 -Type DWord;Write-Output '[OK] Priorita primo piano massimizzata.'", true);
            AddRun("[2] Bilanciata (default Windows)", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' Win32PrioritySeparation 2 -Type DWord;Write-Output '[OK] Priorita bilanciata ripristinata.'", true);
            AddBack("[0] Annulla", ShowAdvanced);
        }

        private void ShowPowerThrottling()
        {
            OpenPage("DISABILITAZIONE POWER THROTTLING", "Power Throttling limita la frequenza CPU dei processi in background. Disabilitarlo mantiene la CPU a piena potenza.");
            AddRun("[1] Disabilita Power Throttling (massime prestazioni)", "New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Power\\PowerThrottling' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Power\\PowerThrottling' PowerThrottlingOff 1 -Type DWord;powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR PERFAUTONOMOUS 0;Write-Output '[OK] Power Throttling disabilitato.'", true);
            AddRun("[2] Riabilita Power Throttling (default)", "Remove-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Power\\PowerThrottling' PowerThrottlingOff -ErrorAction SilentlyContinue;powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR PERFAUTONOMOUS 1;Write-Output '[OK] Power Throttling riabilitato.'", true);
            AddBack("[0] Annulla", ShowAdvanced);
        }

        private void ShowProTools()
        {
            OpenPage("FUNZIONI SEGRETE E STRUMENTI PRO", "Le stesse dieci funzioni e strumenti professionali del batch originale.");
            Add("[1]  Mostra password WiFi salvate nel PC", ShowWifi);
            AddRun("[2]  Recupera Product Key di Windows", ProductKey(), false);
            AddRun("[3]  Dashboard live CPU/RAM/DISCO in tempo reale", Dashboard(), false);
            AddRun("[4]  Benchmark velocita' disco (lettura/scrittura)", Benchmark(), true);
            AddRun("[5]  Scanner processi sospetti e malware", ProcessScanner(), false);
            AddRun("[6]  Info segrete sistema (seriale, UUID, MAC, BIOS)", SecretInfo(), false);
            Add("[7]  Startup Manager (vedi e rimuovi avvii automatici)", StartupManager);
            AddRun("[8]  Mappa rete locale (tutti i dispositivi connessi)", NetMap(), false);
            Add("[9]  Analisi dipendenze DLL di un eseguibile", DllAnalysis);
            AddRun("[10] Genera report completo sistema (esportato su Desktop)", FullReport(), false);
            Add("[11] Diagnostica Integrità Windows", ShowSystemHealth);
            Add("[12] Analizzatore Spazio Intelligente", ShowSmartStorage);
            Add("[13] Stato Driver e Dispositivi", ShowDriverHealth);
            Add("[14] Suite Diagnostica Rete", ShowNetworkDiagnostics);
            Add("[15] Snapshot Prestazioni", ShowPerformanceSnapshot);
            Add("[16] Controllo Aggiornamenti e Riavvio", ShowUpdateRestartCheck);
            AddBack("[0]  Torna al menu", ShowMenu);
        }

        private void ShowSystemHealth()
        {
            OpenPage("DIAGNOSTICA INTEGRITÀ WINDOWS", "Analisi in sola lettura dei file di sistema e dell’immagine Windows. Non esegue riparazioni e non modifica il sistema.");
            AddRun("[1] Esegui analisi integrità Windows", "Write-Output '[*] Avvio verifica file di sistema (SFC VerifyOnly)...';sfc /verifyonly;Write-Output '[*] Avvio analisi immagine Windows (DISM ScanHealth)...';DISM /Online /Cleanup-Image /ScanHealth;Write-Output '[OK] Diagnostica integrità completata.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowSmartStorage()
        {
            OpenPage("ANALIZZATORE SPAZIO INTELLIGENTE", "Analisi di spazio, cache e cartelle principali in sola lettura. Non elimina file automaticamente.");
            AddRun("[1] Analizza spazio disco e cartelle principali", "Write-Output '[*] Analisi spazio del disco di sistema...';$drive=Get-PSDrive -Name ($env:SystemDrive.TrimEnd(':')) -ErrorAction SilentlyContinue;if($drive){$used=$drive.Used/1GB;$free=$drive.Free/1GB;Write-Output ('[DISK] '+$env:SystemDrive+' | Usato: '+[math]::Round($used,2)+' GB | Libero: '+[math]::Round($free,2)+' GB')};$targets=@($env:TEMP,$env:LOCALAPPDATA,$env:USERPROFILE+'\\Downloads');foreach($t in $targets){if(Test-Path $t){try{$size=(Get-ChildItem $t -Force -ErrorAction SilentlyContinue|Measure-Object -Property Length -Sum).Sum/1MB;Write-Output ('[FOLDER] '+$t+' | '+[math]::Round($size,1)+' MB')}catch{Write-Output ('[FOLDER] '+$t+' | accesso parziale')}}};Write-Output '[OK] Analisi spazio completata.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowDriverHealth()
        {
            OpenPage("STATO DRIVER E DISPOSITIVI", "Rileva dispositivi con errori e mostra i driver principali. Non scarica né aggiorna driver automaticamente.");
            AddRun("[1] Analizza dispositivi e driver principali", "Write-Output '[*] Ricerca dispositivi con stato non OK...';$bad=Get-PnpDevice -ErrorAction SilentlyContinue|Where-Object{$_.Status -ne 'OK'};if($bad){$bad|Select-Object Class,FriendlyName,Status,Problem|Format-Table -AutoSize|Out-String}else{Write-Output '[OK] Nessun dispositivo con errore segnalato.'};Write-Output '[*] Driver firmati più recenti:';Get-CimInstance Win32_PnPSignedDriver -ErrorAction SilentlyContinue|Sort-Object DriverDate -Descending|Select-Object -First 15 DeviceName,DriverVersion,DriverDate,Manufacturer|Format-Table -AutoSize|Out-String;Write-Output '[OK] Analisi driver completata.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowNetworkDiagnostics()
        {
            OpenPage("SUITE DIAGNOSTICA RETE", "Controlli in sola lettura su adattatore, gateway, DNS e connettività. Non resetta le impostazioni di rete.");
            AddRun("[1] Esegui diagnostica rete", "Write-Output '[*] Rilevamento adattatori attivi...';Get-NetAdapter -Physical -ErrorAction SilentlyContinue|Where-Object{$_.Status -eq 'Up'}|Select-Object Name,InterfaceDescription,LinkSpeed,MacAddress|Format-Table -AutoSize|Out-String;$cfg=Get-NetIPConfiguration|Where-Object{$_.IPv4DefaultGateway}|Select-Object -First 1;if($cfg){$gw=$cfg.IPv4DefaultGateway.NextHop;Write-Output ('[NET] Gateway: '+$gw);Write-Output '[*] Test gateway...';Test-Connection -ComputerName $gw -Count 2 -ErrorAction SilentlyContinue|Select-Object Address,ResponseTime,Status|Format-Table -AutoSize|Out-String;Write-Output '[NET] DNS: '+($cfg.DNSServer.ServerAddresses -join ', ')}else{Write-Output '[!] Gateway non rilevato.'};Write-Output '[*] Test risoluzione DNS...';Resolve-DnsName www.microsoft.com -ErrorAction SilentlyContinue|Select-Object -First 3 Name,IPAddress|Format-Table -AutoSize|Out-String;Write-Output '[OK] Diagnostica rete completata.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowPerformanceSnapshot()
        {
            OpenPage("SNAPSHOT PRESTAZIONI", "Fotografia immediata di CPU, RAM e spazio disco. Il benchmark disco originale resta nella sua funzione Pro già esistente.");
            AddRun("[1] Raccogli snapshot CPU, RAM e disco", "Write-Output '[*] Raccolta snapshot prestazioni...';$cpu=Get-CimInstance Win32_Processor|Select-Object -First 1;$os=Get-CimInstance Win32_OperatingSystem;Write-Output ('CPU: '+$cpu.Name+' | Carico: '+$cpu.LoadPercentage+'%');$ram=[math]::Round(100-($os.FreePhysicalMemory/$os.TotalVisibleMemorySize*100),1);Write-Output ('RAM usata: '+$ram+'%');Get-PSDrive -PSProvider FileSystem|Where-Object{$_.Used -gt 0}|ForEach-Object{Write-Output ('Disco '+$_.Name+': '+[math]::Round($_.Used/($_.Used+$_.Free)*100,1)+'% usato')};Write-Output '[OK] Snapshot completato.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowUpdateRestartCheck()
        {
            OpenPage("CONTROLLO AGGIORNAMENTI E RIAVVIO", "Verifica se Windows richiede un riavvio e mostra lo stato dei servizi di aggiornamento. Non modifica Windows Update.");
            AddRun("[1] Verifica aggiornamenti e riavvio pendente", "Write-Output '[*] Verifica riavvio pendente...';$pending=$false;$keys='HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Component Based Servicing\\RebootPending','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\WindowsUpdate\\Auto Update\\RebootRequired';foreach($k in $keys){if(Test-Path $k){$pending=$true;Write-Output ('[PENDING] '+$k)}};$sm=Get-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Session Manager' -ErrorAction SilentlyContinue;if($sm.PendingFileRenameOperations){$pending=$true;Write-Output '[PENDING] Operazioni file in attesa'};if($pending){Write-Output '[!] Riavvio di Windows consigliato.'}else{Write-Output '[OK] Nessun riavvio pendente rilevato.'};Get-Service wuauserv,bits -ErrorAction SilentlyContinue|Select-Object Name,Status,StartType|Format-Table -AutoSize|Out-String;Write-Output '[OK] Controllo aggiornamenti completato.'", false);
            AddRun("[2] Apri Windows Update ufficiale", "Start-Process 'ms-settings:windowsupdate';Write-Output '[OK] Windows Update aperto.'", false);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void ShowAiIntro()
        {
            OpenPage("ASSISTENTE IA - CONFIGURAZIONE RICHIESTA", "Per utilizzare l’assistente IA è necessario disporre di una chiave API gratuita di Groq. Come ottenerla: 1. Vai su https://console.groq.com e registrati. 2. Crea una nuova API Key. 3. Apri ask_ai.ps1 nella cartella dell’app. 4. Sostituisci gsk_tuo-token-qui con la tua chiave.");
            Add("Continuare? (S)", AiChat);
            AddBack("Annulla (N) - Torna al menu", ShowMenu);
        }

        private void ShowActivation()
        {
            OpenPage("MICROSOFT ACTIVATION SCRIPTS (MAS)", "L’app conserva il nome e la posizione della voce 15. Per la sicurezza della licenza, l’app non scarica né esegue script esterni di attivazione: apre la sezione ufficiale di attivazione Windows.");
            AddRun("Apri attivazione Windows/Office", "Start-Process 'ms-settings:activation';Write-Output '[OK] Aperte le impostazioni di attivazione Windows.'", false);
            AddBack("[0] Torna al menu", ShowMenu);
        }

        private void ShowWifi()
        {
            OpenPage("PASSWORD WiFi SALVATE", "Il batch elenca prima le reti WiFi memorizzate e poi richiede il nome esatto della rete. L’elenco viene eseguito automaticamente qui sotto.");
            Add("[1] Inserisci il nome esatto della rete per visualizzare la password", WifiPassword);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
            Run("Elenco reti WiFi salvate", "netsh wlan show profiles;Write-Output '';Write-Output '[*] Inserisci il nome esatto della rete usando l’opzione 1.'", false);
        }

        private void WifiPassword()
        {
            string ssid = Prompt.Show(this, "Nome ESATTO della rete WiFi (0 per uscire):", "PASSWORD WiFi SALVATE", "");
            if (String.IsNullOrWhiteSpace(ssid) || ssid == "0") return;
            Run("PASSWORD WiFi SALVATE", "netsh wlan show profile name='" + Escape(ssid) + "' key=clear | Select-String -Pattern 'Contenuto chiave|Key Content';if(!$?){Write-Output 'Password non trovata o rete non esistente.'}", false);
        }

        private void StartupManager()
        {
            OpenPage("STARTUP MANAGER", "Visualizza le voci di avvio automatico, oppure rimuovi una voce inserendo il nome esatto.");
            AddRun("Visualizza programmi di avvio automatico", "$regs='HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run','HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce';foreach($r in $regs){if(Test-Path $r){'--- '+$r;(Get-Item $r).Property|ForEach-Object{$_+' : '+(Get-ItemProperty $r).$_}}};'Task schedulati attivi (non Microsoft):';Get-ScheduledTask -ErrorAction SilentlyContinue|Where-Object{$_.State -eq 'Ready' -and $_.TaskPath -notlike '\\Microsoft\\*'}|Select-Object -First 15 TaskName,TaskPath|Format-Table -AutoSize|Out-String", false);
            Add("Rimuovi voce dal registro Run", RemoveStartup);
            AddBack("[0] Torna agli Strumenti Pro", ShowProTools);
        }

        private void RemoveStartup()
        {
            string name = Prompt.Show(this, "Nome ESATTO da rimuovere dal registro Run:", "Startup Manager", "");
            if (String.IsNullOrWhiteSpace(name)) return;
            Run("Startup Manager", "$r='HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run','HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce';foreach($x in $r){Remove-ItemProperty -Path $x -Name '" + Escape(name) + "' -ErrorAction SilentlyContinue};Write-Output '[OK] Voce rimossa da tutti i Run keys (se esisteva).'", true);
        }

        private void DllAnalysis()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = AppText.T("Trascina o seleziona l’eseguibile (.exe)");
            dialog.Filter = AppText.T("Eseguibili (*.exe)|*.exe|Tutti i file (*.*)|*.*");
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            string ps = Path.Combine(appDirectory, "dll_analyzer.ps1");
            if (!File.Exists(ps)) { MessageBox.Show(this, AppText.T("File dll_analyzer.ps1 non trovato nella cartella dello script."), "Windows Speed Booster", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            Run("ANALISI DIPENDENZE DLL", "& '" + Escape(ps) + "' -Path '" + Escape(dialog.FileName) + "'", false);
        }

        private void AiChat()
        {
            string q = Prompt.Show(this, "Domanda (scrivi exit per tornare al menu):", "ASSISTENTE IA", "");
            if (String.IsNullOrWhiteSpace(q) || String.Equals(q, "exit", StringComparison.OrdinalIgnoreCase)) { ShowMenu(); return; }
            string ps = Path.Combine(appDirectory, "ask_ai.ps1");
            if (!File.Exists(ps)) { MessageBox.Show(this, AppText.T("File ask_ai.ps1 non trovato nella cartella dello script."), "Windows Speed Booster", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            Run("ASSISTENTE IA", "& '" + Escape(ps) + "' -query '" + Escape(q) + "'", false);
        }

        private void Run(string action, string script, bool confirmation)
        {
            if (busy) return;
            action = AppText.T(action);
            if (confirmation && MessageBox.Show(this, AppText.Choose("Confermi l’esecuzione di \"", "Do you confirm execution of \"") + action + "\"?", AppText.T("Conferma"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            busy = true;
            stopButton.Enabled = true;
            activityLabel.Text = AppText.T("Operazione in corso: ") + action;
            activityLabel.ForeColor = Color.FromArgb(255, 210, 110);
            activityProgress.Visible = true;
            output.Clear();
            Append("=====================================================");
            Append(" " + action);
            Append("=====================================================");
            WriteLog("NUOVO TWEAK: " + action);
            activeProcess = new Process();
            activeProcess.StartInfo = StartupForm.PowerShellInfo(script, appDirectory);
            activeProcess.EnableRaisingEvents = true;
            activeProcess.OutputDataReceived += delegate(object s, DataReceivedEventArgs e) { if (!String.IsNullOrEmpty(e.Data)) Append(e.Data); };
            activeProcess.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e) { if (!String.IsNullOrEmpty(e.Data)) Append(e.Data); };
            Task.Factory.StartNew(delegate
            {
                int code = -1;
                try { activeProcess.Start(); activeProcess.StandardInput.WriteLine(script); activeProcess.StandardInput.Close(); activeProcess.BeginOutputReadLine(); activeProcess.BeginErrorReadLine(); activeProcess.WaitForExit(); code = activeProcess.ExitCode; } catch (Exception ex) { Append("[!] Errore: " + ex.Message); }
                Finish(action, code);
            });
        }

        private void Finish(string action, int code)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string, int>(Finish), action, code); return; }
            busy = false;
            stopButton.Enabled = false;
            activityProgress.Visible = false;
            activityLabel.Text = code == 0 ? AppText.T("Operazione completata · menu ancora disponibile") : AppText.T("Operazione terminata · controlla l’output");
            activityLabel.ForeColor = code == 0 ? Color.FromArgb(145, 221, 176) : Color.FromArgb(255, 190, 107);
            Append(code == 0 ? "[OK] " + AppText.T("Operazione completata.") : "[!] " + AppText.T("Operazione terminata con codice ") + code + ".");
            WriteLog(action + " - codice " + code);
            activeProcess = null;
            if (String.Equals(action, AppText.T("ASSISTENTE IA"), StringComparison.OrdinalIgnoreCase)) BeginInvoke(new Action(AiChat));
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try { if (activeProcess != null && !activeProcess.HasExited) { activeProcess.Kill(); Append("[!] Operazione interrotta dall’utente."); } } catch (Exception ex) { Append("[!] " + ex.Message); }
        }

        private void Append(string text)
        {
            if (InvokeRequired) { BeginInvoke(new Action<string>(Append), text); return; }
            string visible = AppText.P(text);
            output.AppendText(visible + Environment.NewLine);
            output.ScrollToCaret();
            WriteLog(text);
        }

        private void WriteLog(string text)
        {
            try { File.AppendAllText(logPath, "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + text + Environment.NewLine, Encoding.UTF8); } catch { }
        }

        private static string Escape(string value) { return value.Replace("'", "''"); }

        private static string NetworkBoost() { return "netsh int ip reset;netsh winsock reset;netsh int tcp set global autotuninglevel=normal;netsh int tcp set global rss=enabled;netsh int tcp set global ecncapability=enabled;ipconfig /flushdns;ipconfig /registerdns;arp -d *;Write-Output '[OK] Network Boost completato!';Write-Output '[!] Riavvia il PC per applicare tutti i cambiamenti.'"; }
        private static string HardwareScan() { return "$cpu=Get-CimInstance Win32_Processor|Select-Object -First 1;$os=Get-CimInstance Win32_OperatingSystem;$cs=Get-CimInstance Win32_ComputerSystem;$gpu=Get-CimInstance Win32_VideoController|Select-Object -First 1;'CPU: '+$cpu.Name;'Core/Thread: '+$cpu.NumberOfCores+'/'+$cpu.NumberOfLogicalProcessors;'Velocita: '+$cpu.MaxClockSpeed+' MHz';'GPU: '+$gpu.Name;'RAM Totale: '+[math]::Round($cs.TotalPhysicalMemory/1GB,2)+' GB';'RAM Libera: '+[math]::Round($os.FreePhysicalMemory/1MB,2)+' GB';'OS: '+$os.Caption+' '+$os.BuildNumber;'Uptime: '+((Get-Date)-$os.LastBootUpTime);'Dischi:';try{Get-PhysicalDisk|ForEach-Object{$_.FriendlyName+' | '+$_.MediaType+' | '+[math]::Round($_.Size/1GB,0)+' GB'}}catch{Get-CimInstance Win32_DiskDrive|ForEach-Object{$_.Model}}"; }
        private static string PerformanceReport() { return "$cpu=Get-CimInstance Win32_Processor|Select-Object -First 1;$os=Get-CimInstance Win32_OperatingSystem;$ram=[math]::Round(100-($os.FreePhysicalMemory/$os.TotalVisibleMemorySize*100),1);'Sistema: '+$os.Caption;'CPU: '+$cpu.Name;'Carico CPU: '+$cpu.LoadPercentage+'%';'RAM Usata: '+$ram+'%';'RAM Libera: '+[math]::Round($os.FreePhysicalMemory/1MB,1)+' GB';'Uptime: '+((Get-Date)-$os.LastBootUpTime);Get-PSDrive -PSProvider FileSystem|Where-Object{$_.Used -gt 0}|ForEach-Object{'Disco '+$_.Name+': '+[math]::Round($_.Used/($_.Used+$_.Free)*100,1)+'% usato'}"; }
        private static string Gaming() { return "New-Item 'HKCU:\\Software\\Microsoft\\GameBar' -Force|Out-Null;Set-ItemProperty 'HKCU:\\Software\\Microsoft\\GameBar' AllowAutoGameMode 1 -Type DWord;Set-ItemProperty 'HKCU:\\Software\\Microsoft\\GameBar' AutoGameModeEnabled 1 -Type DWord;New-Item 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR' -Force|Out-Null;Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR' AppCaptureEnabled 0 -Type DWord;New-Item 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\GameDVR' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\GameDVR' AllowGameDVR 0 -Type DWord;bcdedit /deletevalue useplatformclock;bcdedit /set disabledynamictick yes;New-Item 'HKCU:\\System\\GameConfigStore' -Force|Out-Null;Set-ItemProperty 'HKCU:\\System\\GameConfigStore' GameDVR_FSEBehaviorMode 2 -Type DWord;Set-ItemProperty 'HKCU:\\System\\GameConfigStore' GameDVR_HonorUserFSEBehaviorMode 1 -Type DWord;$g='HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile\\Tasks\\Games';New-Item $g -Force|Out-Null;Set-ItemProperty $g 'GPU Priority' 8 -Type DWord;Set-ItemProperty $g Priority 6 -Type DWord;Set-ItemProperty $g 'Scheduling Category' 'High';Write-Output '[OK] Gaming Mode totale attivato.'"; }
        private static string Nagle() { return "Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces'|ForEach-Object{Set-ItemProperty $_.PSPath TcpAckFrequency 1 -Type DWord -Force;Set-ItemProperty $_.PSPath TcpNoDelay 1 -Type DWord -Force;Set-ItemProperty $_.PSPath TCPDelAckTicks 0 -Type DWord -Force};New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters' MaxUserPort 65534 -Type DWord;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters' TcpTimedWaitDelay 30 -Type DWord;Write-Output '[OK] Nagle disabilitato.'"; }
        private static string Telemetry() { return "$tasks='\\Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser','\\Microsoft\\Windows\\Application Experience\\ProgramDataUpdater','\\Microsoft\\Windows\\Autochk\\Proxy','\\Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator','\\Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip','\\Microsoft\\Windows\\DiskDiagnostic\\Microsoft-Windows-DiskDiagnosticDataCollector','\\Microsoft\\Windows\\Feedback\\Siuf\\DmClient','\\Microsoft\\Windows\\Windows Error Reporting\\QueueReporting';foreach($t in $tasks){schtasks /Change /TN $t /DISABLE};sc.exe config DiagTrack start= disabled;Stop-Service DiagTrack -ErrorAction SilentlyContinue;sc.exe config dmwappushservice start= disabled;Stop-Service dmwappushservice -ErrorAction SilentlyContinue;$a='HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection';New-Item $a -Force|Out-Null;Set-ItemProperty $a AllowTelemetry 0 -Type DWord;$b='HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection';New-Item $b -Force|Out-Null;Set-ItemProperty $b AllowTelemetry 0 -Type DWord;$c='HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat';New-Item $c -Force|Out-Null;Set-ItemProperty $c DisableInventory 1 -Type DWord;$d='HKLM:\\SOFTWARE\\Policies\\Microsoft\\SQMClient\\Windows';New-Item $d -Force|Out-Null;Set-ItemProperty $d CEIPEnable 0 -Type DWord;$h=Join-Path $env:windir 'System32\\drivers\\etc\\hosts';foreach($s in 'vortex.data.microsoft.com','watson.telemetry.microsoft.com','telecommand.telemetry.microsoft.com'){if(-not(Select-String -Path $h -SimpleMatch $s -Quiet)){Add-Content $h ('0.0.0.0 '+$s)}};Write-Output '[OK] Telemetria profonda disabilitata.'"; }
        private static string Irq() { return "Get-NetAdapter -Physical -ErrorAction SilentlyContinue|Where-Object{$_.Status -eq 'Up'}|ForEach-Object{Disable-NetAdapterPowerManagement -Name $_.Name -ErrorAction SilentlyContinue;Set-NetAdapterAdvancedProperty -Name $_.Name -DisplayName 'Interrupt Moderation' -DisplayValue 'Disabled' -ErrorAction SilentlyContinue;Write-Output ('[OK] Interrupt Moderation off su: '+$_.Name)};New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' TdrDelay 8 -Type DWord;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' TdrDdiDelay 8 -Type DWord;New-Item 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' SystemResponsiveness 0 -Type DWord;Set-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' NetworkThrottlingIndex 4294967295 -Type DWord;Write-Output '[OK] IRQ e interrupt ottimizzati.'"; }
        private static string Restore() { return "Remove-Item 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\GameDVR' -Recurse -Force -ErrorAction SilentlyContinue;New-Item 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR' -Force|Out-Null;Set-ItemProperty 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR' AppCaptureEnabled 1 -Type DWord;bcdedit /deletevalue disabledynamictick;bcdedit /deletevalue tscsyncpolicy;Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces'|ForEach-Object{Remove-ItemProperty $_.PSPath TcpAckFrequency -ErrorAction SilentlyContinue;Remove-ItemProperty $_.PSPath TcpNoDelay -ErrorAction SilentlyContinue;Remove-ItemProperty $_.PSPath TCPDelAckTicks -ErrorAction SilentlyContinue};powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 0;powercfg /apply;New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\PriorityControl' Win32PrioritySeparation 2 -Type DWord;Remove-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\Power\\PowerThrottling' PowerThrottlingOff -ErrorAction SilentlyContinue;New-Item 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers' HwSchMode 1 -Type DWord;New-Item 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' -Force|Out-Null;Set-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' SystemResponsiveness 20 -Type DWord;Set-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Multimedia\\SystemProfile' NetworkThrottlingIndex 10 -Type DWord;fsutil behavior set disable8dot3 0;fsutil behavior set disablelastaccess 0;Write-Output '[OK] Tutti i tweaks ripristinati. Riavvia il PC.'"; }
        private static string ProductKey() { return "$os=Get-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion' -ErrorAction SilentlyContinue;'Sistema: '+$os.ProductName;'Edizione: '+$os.EditionID;'Build: '+$os.CurrentBuild;$key=$null;$src='';try{$k=(Get-CimInstance -ClassName SoftwareLicensingService -ErrorAction Stop).OA3xOriginalProductKey;if($k -and $k.Trim() -ne ''){$key=$k;$src='UEFI/BIOS OEM'}}catch{};if(!$key){try{$k=(Get-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SoftwareProtectionPlatform' -ErrorAction Stop).BackupProductKeyDefault;if($k -and $k.Trim() -ne ''){$key=$k;$src='Registro BackupProductKey'}}catch{}};if(!$key){try{$rb=[byte[]]((Get-ItemProperty 'HKLM:\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion' -ErrorAction Stop).DigitalProductId[52..66]);$map='BCDFGHJKMPQRTVWXY2346789';$res='';for($i=24;$i -ge 0;$i--){$r=0;for($j=14;$j -ge 0;$j--){$r=$r*256+[int]$rb[$j];$rb[$j]=[math]::Floor($r/24);$r=$r%24};$res=$map[$r]+$res;if($i%5 -eq 0 -and $i -ne 0){$res='-'+$res}};if($res -match '^[B-Z2-9-]+$' -and $res.Length -eq 29){$key=$res;$src='DigitalProductId legacy'}}catch{}};if($key){'Product Key: '+$key;'Trovata con: '+$src}else{'[!] Chiave non leggibile via software. Il PC potrebbe usare una licenza digitale.'}"; }
        private static string Dashboard() { return "while($true){$cpu=Get-CimInstance Win32_Processor|Select-Object -First 1;$os=Get-CimInstance Win32_OperatingSystem;$ram=[math]::Round(100-$os.FreePhysicalMemory/$os.TotalVisibleMemorySize*100,1);'=====================================================';'DASHBOARD LIVE - SISTEMA IN TEMPO REALE';'Ora: '+(Get-Date -Format 'HH:mm:ss');'CPU Usage: '+$cpu.LoadPercentage+'%';'CPU Freq: '+$cpu.CurrentClockSpeed+' MHz';'RAM Usage: '+$ram+'%';'RAM Libera: '+[math]::Round($os.FreePhysicalMemory/1MB,1)+' GB';'Processi: '+(Get-Process).Count;'TOP 5 CPU:';Get-Process|Sort-Object CPU -Descending|Select-Object -First 5|ForEach-Object{'  '+$_.Name+' CPU:'+[math]::Round($_.CPU,1)+'s RAM:'+[math]::Round($_.WorkingSet/1MB,0)+'MB'};'DISCHI:';Get-PSDrive -PSProvider FileSystem|Where-Object{$_.Used -gt 0}|ForEach-Object{'  '+$_.Name+': '+[math]::Round($_.Used/($_.Used+$_.Free)*100,1)+'%'};Start-Sleep -Seconds 2}"; }
        private static string Benchmark() { return "try{$p='C:\\__speedtest_bench__.tmp';$size=256MB;$buf=New-Object byte[] $size;(New-Object Random).NextBytes($buf);$sw=[Diagnostics.Stopwatch]::StartNew();[IO.File]::WriteAllBytes($p,$buf);$sw.Stop();'Scrittura: '+[math]::Round($size/1MB/$sw.Elapsed.TotalSeconds,1)+' MB/s';$sw=[Diagnostics.Stopwatch]::StartNew();[IO.File]::ReadAllBytes($p)|Out-Null;$sw.Stop();'Lettura: '+[math]::Round($size/1MB/$sw.Elapsed.TotalSeconds,1)+' MB/s';Remove-Item $p -Force}catch{Remove-Item 'C:\\__speedtest_bench__.tmp' -Force -ErrorAction SilentlyContinue;'Errore benchmark: '+$_.Exception.Message}"; }
        private static string ProcessScanner() { return "$p=Get-Process -ErrorAction SilentlyContinue|Where-Object{$_.Path};'Processi analizzati: '+$p.Count;$s=@();foreach($x in $p){$z=$x.Path.ToLower();if($z -like '*\\temp\\*' -or ($z -like '*\\appdata\\roaming\\*' -and $z -notlike '*\\microsoft\\*') -or $z -like '*\\users\\public\\*'){$s += [pscustomobject]@{Nome=$x.Name;PID=$x.Id;Percorso=$x.Path}}};if($s.Count -eq 0){'[OK] Nessun processo sospetto rilevato.'}else{$s|Format-List|Out-String};'Connessioni TCP attive:';Get-NetTCPConnection -State Established -ErrorAction SilentlyContinue|Select-Object -First 8|ForEach-Object{(Get-Process -Id $_.OwningProcess -ErrorAction SilentlyContinue).Name+' -> '+$_.RemoteAddress+':'+$_.RemotePort}"; }
        private static string SecretInfo() { return "$bios=Get-CimInstance Win32_BIOS;$prod=Get-CimInstance Win32_ComputerSystemProduct;$mb=Get-CimInstance Win32_BaseBoard;'Seriale PC: '+$prod.IdentifyingNumber;'UUID: '+$prod.UUID;'Produttore: '+$prod.Vendor;'Modello: '+$prod.Name;'Versione BIOS: '+$bios.SMBIOSBIOSVersion;'Seriale BIOS: '+$bios.SerialNumber;'Scheda Madre: '+$mb.Product;'Seriale scheda madre: '+$mb.SerialNumber;'RETE:';Get-NetAdapter -Physical -ErrorAction SilentlyContinue|Where-Object{$_.Status -eq 'Up'}|ForEach-Object{$_.Name+': MAC '+$_.MacAddress+' Speed '+$_.LinkSpeed};Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue|Where-Object{$_.IPAddress -ne '127.0.0.1'}|ForEach-Object{$_.InterfaceAlias+': '+$_.IPAddress};'IP Pubblico: '+(try{(Invoke-WebRequest 'https://api.ipify.org' -UseBasicParsing -TimeoutSec 4).Content}catch{'non disponibile'});'Defender RT: '+(try{(Get-MpComputerStatus).RealTimeProtectionEnabled}catch{'N/D'});'Firewall: '+(Get-NetFirewallProfile -Profile Public).Enabled"; }
        private static string NetMap() { return "try{$gw=(Get-NetIPConfiguration|Where-Object{$_.IPv4DefaultGateway}|Select-Object -First 1).IPv4DefaultGateway.NextHop;if(!$gw){throw 'Gateway non trovato'};'Gateway rilevato: '+$gw;'Dispositivi nella cache ARP:';arp -a;$p=$gw.Split('.');$sub=$p[0]+'.'+$p[1]+'.'+$p[2];'Scansione rete in corso...';1..254|ForEach-Object{$ip=$sub+'.'+$_;if(Test-Connection -ComputerName $ip -Count 1 -Quiet -ErrorAction SilentlyContinue){'[+] '+$ip}}}catch{'[!] '+$_.Exception.Message}"; }
        private static string FullReport() { return "$r=Join-Path ([Environment]::GetFolderPath('Desktop')) ('SystemReport_'+(Get-Date -Format 'yyyyMMddHHmmss')+'.txt');$out=@();$out+='=====================================================';$out+='REPORT COMPLETO DI SISTEMA - '+(Get-Date);$out+='=====================================================';$os=Get-CimInstance Win32_OperatingSystem;$cpu=Get-CimInstance Win32_Processor|Select-Object -First 1;$out+='';$out+='[SISTEMA OPERATIVO]';$out+='OS: '+$os.Caption;$out+='Build: '+$os.BuildNumber;$out+='Architettura: '+$os.OSArchitecture;$out+='';$out+='[CPU]';$out+='Nome: '+$cpu.Name;$out+='Core/Thread: '+$cpu.NumberOfCores+' / '+$cpu.NumberOfLogicalProcessors;$out+='Max GHz: '+[math]::Round($cpu.MaxClockSpeed/1000,2);$out+='';$out+='[MEMORIA RAM]';$out+=(Get-CimInstance Win32_PhysicalMemory|ForEach-Object{'Banco: '+$_.DeviceLocator+' | '+[math]::Round($_.Capacity/1GB,0)+'GB | '+$_.Speed+'MHz | '+$_.Manufacturer});$out+='';$out+='[GPU]';$out+=(Get-CimInstance Win32_VideoController|ForEach-Object{'GPU: '+$_.Name;'Driver: '+$_.DriverVersion});$out+='';$out+='[STORAGE]';$out+=(Get-CimInstance Win32_DiskDrive|ForEach-Object{'Disco: '+$_.Model+' | '+[math]::Round($_.Size/1GB,0)+'GB | '+$_.InterfaceType});$out+='';$out+='[RETE]';$out+=(ipconfig /all);$out+='';$out+='[PROGRAMMI INSTALLATI]';$out+=((Get-ItemProperty 'HKLM:\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\*' -ErrorAction SilentlyContinue|Select-Object DisplayName,DisplayVersion,Publisher|Where-Object{$_.DisplayName}|Sort-Object DisplayName|Format-Table -AutoSize|Out-String));$out+='';$out+='[AVVII AUTOMATICI]';$regs='HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run','HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run';foreach($x in $regs){if(Test-Path $x){$out+=((Get-Item $x).Property|ForEach-Object{$_+' : '+(Get-ItemProperty $x).$_})}};$out+='';$out+='=====================================================';$out+='FINE REPORT';$out+='=====================================================';$out|Out-File $r -Encoding UTF8;Start-Process notepad $r;Write-Output '[OK] Report salvato: '+$r"; }
    }

    internal sealed class AuroraHeader : Panel
    {
        private readonly Timer animator;
        private int phase;

        public AuroraHeader()
        {
            DoubleBuffered = true;
            animator = new Timer();
            animator.Interval = 35;
            animator.Tick += delegate { phase = (phase + 2) % 720; Invalidate(); };
            animator.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = ClientRectangle;
            using (LinearGradientBrush bg = new LinearGradientBrush(area, Color.FromArgb(89, 34, 168), Color.FromArgb(18, 144, 187), LinearGradientMode.Horizontal)) e.Graphics.FillRectangle(bg, area);
            DrawGlow(e.Graphics, new Point((phase % Math.Max(1, Width + 500)) - 250, 65), 280, Color.FromArgb(120, 255, 82, 218));
            DrawGlow(e.Graphics, new Point(Width - ((phase * 2) % Math.Max(1, Width + 600)) + 300, 15), 270, Color.FromArgb(112, 84, 244, 255));
            for (int i = 0; i < 24; i++)
            {
                int x = (i * 97 + phase * 3) % Math.Max(1, Width);
                int y = 15 + (i * 31 % 105);
                using (SolidBrush speck = new SolidBrush(Color.FromArgb(78 + (i % 3) * 30, 255, 232, 178))) e.Graphics.FillEllipse(speck, x, y, 2 + i % 3, 2 + i % 3);
            }
            using (Pen line = new Pen(Color.FromArgb(205, 255, 216, 119), 1.5F)) e.Graphics.DrawLine(line, 0, Height - 2, Width, Height - 2);
            DrawHeaderText(e.Graphics);
        }

        private void DrawHeaderText(Graphics g)
        {
            using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (Font brand = new Font("Segoe UI Semibold", 18F, FontStyle.Bold))
            using (Font version = new Font("Segoe UI Semibold", 18F, FontStyle.Bold))
            using (Font sub = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            using (Font live = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            using (SolidBrush brandBrush = new SolidBrush(Color.FromArgb(82, 239, 255)))
            using (SolidBrush versionBrush = new SolidBrush(Color.FromArgb(255, 196, 82)))
            using (SolidBrush subBrush = new SolidBrush(Color.FromArgb(241, 218, 255)))
            using (SolidBrush liveBrush = new SolidBrush(Color.FromArgb(195, 255, 174)))
            {
                g.DrawString("WINDOWS SPEED BOOSTER", brand, brandBrush, new Rectangle(38, 24, 336, 34), left);
                g.DrawString("V14.0", version, versionBrush, new Rectangle(378, 24, 96, 34), left);
                g.DrawString(AppText.T("MENU DI OTTIMIZZAZIONE  •  PERFORMANCE SU MISURA"), sub, subBrush, new Rectangle(42, 69, 460, 24), left);
                g.DrawString("●  " + AppText.T("SISTEMA PRONTO"), live, liveBrush, new Rectangle(Math.Max(16, Width - 242), 72, 210, 28), center);
            }
        }

        private static void DrawGlow(Graphics g, Point center, int radius, Color color)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(center.X - radius, center.Y - radius / 2, radius * 2, radius);
                using (PathGradientBrush glow = new PathGradientBrush(path))
                {
                    glow.CenterColor = color;
                    glow.SurroundColors = new Color[] { Color.FromArgb(0, color.R, color.G, color.B) };
                    g.FillPath(glow, path);
                }
            }
        }
    }

    internal sealed class HeaderSearchBar : UserControl
    {
        private readonly TextBox input;
        public event EventHandler QueryChanged;
        public string Query { get { return input.Text; } }
        public void ClearQuery() { input.Clear(); }

        public HeaderSearchBar()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.FromArgb(60, 52, 146);
            input = new TextBox();
            input.BorderStyle = BorderStyle.None;
            input.BackColor = BackColor;
            input.ForeColor = Color.FromArgb(247, 250, 255);
            input.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular);
            input.Margin = Padding.Empty;
            input.TextAlign = HorizontalAlignment.Left;
            input.TextChanged += delegate { if (QueryChanged != null) QueryChanged(this, EventArgs.Empty); };
            Controls.Add(input);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width >= 4 && Height >= 4)
            {
                using (GraphicsPath shape = Rounded(new Rectangle(0, 0, Width - 1, Height - 1), 15)) { Region = new Region(shape); }
            }
            int inputHeight = Math.Max(18, input.PreferredHeight);
            input.Size = new Size(Math.Max(80, Width - 128), inputHeight);
            input.Location = new Point(113, Math.Max(1, ((Height - inputHeight) / 2) + 1));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent != null ? Parent.BackColor : BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            Rectangle outer = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Rectangle inner = new Rectangle(2, 2, Math.Max(1, Width - 5), Math.Max(1, Height - 5));
            using (GraphicsPath outerPath = Rounded(outer, 15))
            using (GraphicsPath innerPath = Rounded(inner, 13))
            using (SolidBrush outline = new SolidBrush(Color.FromArgb(255, 174, 241, 255)))
            using (SolidBrush fill = new SolidBrush(BackColor))
            using (Pen glass = new Pen(Color.FromArgb(255, 222, 249, 255), 1.7F))
            using (Font label = new Font("Segoe UI Semibold", 8.7F, FontStyle.Bold))
            {
                e.Graphics.FillPath(outline, outerPath);
                e.Graphics.FillPath(fill, innerPath);
                e.Graphics.DrawEllipse(glass, 18, Height / 2 - 8, 14, 14);
                e.Graphics.DrawLine(glass, 29, Height / 2 + 3, 37, Height / 2 + 10);
                TextRenderer.DrawText(e.Graphics, AppText.T("CERCA"), label, new Rectangle(45, 0, 63, Height), Color.FromArgb(242, 226, 246, 255), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class HeaderGithubButton : Button
    {
        private bool hovering;

        public HeaderGithubButton()
        {
            DoubleBuffered = true;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.FromArgb(76, 47, 139);
            Cursor = Cursors.Hand;
            TabStop = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width < 2 || Height < 2) return;
            using (GraphicsPath shape = Rounded(new Rectangle(0, 0, Width - 1, Height - 1), 12)) Region = new Region(shape);
        }

        protected override void OnMouseEnter(EventArgs e) { hovering = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hovering = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent != null ? Parent.BackColor : BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Color top = hovering ? Color.FromArgb(99, 106, 222) : Color.FromArgb(74, 72, 175);
            Color bottom = hovering ? Color.FromArgb(24, 169, 205) : Color.FromArgb(19, 122, 172);
            using (GraphicsPath path = Rounded(r, 12))
            using (LinearGradientBrush fill = new LinearGradientBrush(r, top, bottom, LinearGradientMode.Horizontal))
            using (Pen edge = new Pen(Color.FromArgb(235, 226, 252, 255), 1.1F))
            using (SolidBrush glyph = new SolidBrush(Color.FromArgb(225, 244, 254, 255)))
            using (Font icon = new Font("Segoe UI Black", 9.5F, FontStyle.Bold))
            using (Font name = new Font("Segoe UI Semibold", 8.3F, FontStyle.Bold))
            using (Font creator = new Font("Segoe UI", 7.7F, FontStyle.Regular))
            using (StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawPath(edge, path);
                e.Graphics.FillEllipse(glyph, 11, 8, 22, 22);
                using (SolidBrush mark = new SolidBrush(Color.FromArgb(35, 43, 104))) e.Graphics.DrawString("GH", icon, mark, new Rectangle(12, 10, 21, 17), left);
                e.Graphics.DrawString("GITHUB", name, glyph, new Rectangle(42, 3, Width - 50, 17), left);
                e.Graphics.DrawString("Made by 9337progame", creator, glyph, new Rectangle(42, 18, Width - 50, 16), left);
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            GraphicsPath path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class PremiumButton : Button
    {
        private bool hovering;
        private int shimmer;
        private readonly Timer animator;
        public Color AccentColor { get; set; }
        public int ModuleIndex { get; set; }
        private bool moduleMode;
        public bool ModuleMode
        {
            get { return moduleMode; }
            set { if (moduleMode != value) { moduleMode = value; ApplyShapeRegion(); Invalidate(); } }
        }

        public PremiumButton()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.FromArgb(37, 31, 96);
            TextAlign = ContentAlignment.MiddleLeft;
            AccentColor = Color.FromArgb(57, 219, 255);
            animator = new Timer();
            animator.Interval = 45;
            animator.Tick += delegate { if (hovering) { shimmer = (shimmer + 7) % Math.Max(1, Width + 90); Invalidate(); } };
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            SyncCanvasColor();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            Invalidate();
        }

        private void SyncCanvasColor()
        {
            if (Parent != null) BackColor = Parent.BackColor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyShapeRegion();
        }

        private void ApplyShapeRegion()
        {
            if (Width < 2 || Height < 2) return;
            using (GraphicsPath shape = Beveled(new Rectangle(0, 0, Width - 1, Height - 1), ModuleMode ? 17 : 13))
            {
                Region = new Region(shape);
            }
        }

        private Color CanvasColor()
        {
            return Parent != null ? Parent.BackColor : BackColor;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(CanvasColor());
        }

        protected override void OnMouseEnter(EventArgs e) { hovering = true; animator.Start(); Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hovering = false; animator.Stop(); Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(CanvasColor());
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (ModuleMode) DrawModule(e.Graphics); else DrawOption(e.Graphics);
        }

        private void DrawModule(Graphics g)
        {
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Color light = Color.FromArgb(244, Math.Min(255, AccentColor.R + 68), Math.Min(255, AccentColor.G + 68), Math.Min(255, AccentColor.B + 68));
            Color rich = Color.FromArgb(231, Math.Max(45, (int)AccentColor.R), Math.Max(45, (int)AccentColor.G), Math.Max(45, (int)AccentColor.B));
            using (GraphicsPath path = Beveled(r, 17))
            using (LinearGradientBrush fill = new LinearGradientBrush(r, light, rich, LinearGradientMode.Vertical))
            {
                if (hovering) Glow(g, r, AccentColor);
                g.FillPath(fill, path);
            }
            using (SolidBrush gloss = new SolidBrush(Color.FromArgb(65, 255, 255, 255))) g.FillRectangle(gloss, r.X + 14, r.Y + 11, Math.Max(1, r.Width - 28), 4);
            DrawGlyph(g, new Rectangle(r.Right - 74, r.Y + 23, 45, 45));
            using (Font numFont = new Font("Segoe UI Black", 26F, FontStyle.Bold))
            using (SolidBrush numBrush = new SolidBrush(Color.FromArgb(42, 21, 48))) g.DrawString(ModuleIndex.ToString("00"), numFont, numBrush, new PointF(r.X + 19, r.Y + 19));
            Rectangle text = new Rectangle(r.X + 20, r.Bottom - 56, r.Width - 42, 42);
            using (Font titleFont = new Font("Segoe UI Semibold", 10.1F, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(35, 21, 57)))
            using (StringFormat fmt = new StringFormat())
            {
                fmt.Alignment = StringAlignment.Near;
                fmt.LineAlignment = StringAlignment.Center;
                fmt.Trimming = StringTrimming.EllipsisCharacter;
                fmt.FormatFlags = StringFormatFlags.NoWrap;
                g.DrawString(CleanTitle(Text), titleFont, titleBrush, text, fmt);
            }
            if (hovering)
            {
                using (Pen scan = new Pen(Color.FromArgb(150, 255, 255, 255), 1F)) g.DrawLine(scan, r.X + Math.Min(Math.Max(8, shimmer), r.Width - 10), r.Y + 13, r.X + Math.Min(Math.Max(8, shimmer), r.Width - 10), r.Bottom - 13);
            }
        }

        private void DrawOption(Graphics g)
        {
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Color light = Color.FromArgb(245, Math.Min(255, AccentColor.R + 84), Math.Min(255, AccentColor.G + 84), Math.Min(255, AccentColor.B + 84));
            Color rich = Color.FromArgb(224, Math.Max(55, (int)AccentColor.R), Math.Max(55, (int)AccentColor.G), Math.Max(55, (int)AccentColor.B));
            using (GraphicsPath path = Beveled(r, 13))
            using (LinearGradientBrush fill = new LinearGradientBrush(r, light, rich, LinearGradientMode.Horizontal))
            {
                if (hovering) Glow(g, r, AccentColor);
                g.FillPath(fill, path);
            }
            using (SolidBrush stripe = new SolidBrush(Color.FromArgb(150, 39, 21, 78))) g.FillRectangle(stripe, r.X + 12, r.Y + 12, 5, r.Height - 24);
            Rectangle text = new Rectangle(r.X + 33, r.Y + 9, Math.Max(80, r.Width - 87), r.Height - 18);
            using (Font titleFont = new Font("Segoe UI Semibold", 10F, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(35, 21, 57)))
            using (StringFormat fmt = new StringFormat())
            {
                fmt.LineAlignment = StringAlignment.Center;
                fmt.Trimming = StringTrimming.EllipsisCharacter;
                g.DrawString(CleanTitle(Text), titleFont, textBrush, text, fmt);
            }
            using (Pen arrow = new Pen(Color.FromArgb(180, 39, 21, 78), 2F))
            {
                arrow.StartCap = LineCap.Round; arrow.EndCap = LineCap.Round;
                int x = r.Right - 29; int y = r.Y + r.Height / 2;
                g.DrawLines(arrow, new Point[] { new Point(x - 6, y - 7), new Point(x + 2, y), new Point(x - 6, y + 7) });
            }
        }

        private void DrawGlyph(Graphics g, Rectangle r)
        {
            using (Pen pen = new Pen(AccentColor, 2F))
            using (SolidBrush fill = new SolidBrush(Color.FromArgb(38, AccentColor.R, AccentColor.G, AccentColor.B)))
            {
                g.FillEllipse(fill, r); g.DrawEllipse(pen, r);
                int cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2;
                if (ModuleIndex % 5 == 1) { g.DrawRectangle(pen, cx - 12, cy - 8, 24, 16); g.DrawLine(pen, cx - 7, cy + 11, cx + 7, cy + 11); }
                else if (ModuleIndex % 5 == 2) { g.DrawEllipse(pen, cx - 10, cy - 10, 20, 20); g.DrawLine(pen, cx - 14, cy, cx + 14, cy); }
                else if (ModuleIndex % 5 == 3) { g.DrawLine(pen, cx - 13, cy + 10, cx, cy - 12); g.DrawLine(pen, cx, cy - 12, cx + 13, cy + 10); }
                else if (ModuleIndex % 5 == 4) { g.DrawRectangle(pen, cx - 10, cy - 10, 20, 20); g.DrawLine(pen, cx - 7, cy - 4, cx + 7, cy - 4); g.DrawLine(pen, cx - 7, cy + 3, cx + 7, cy + 3); }
                else { g.DrawArc(pen, cx - 11, cy - 11, 22, 22, 25, 290); g.DrawLine(pen, cx + 8, cy - 8, cx + 14, cy - 14); }
            }
        }

        private static void Glow(Graphics g, Rectangle r, Color color)
        {
            using (GraphicsPath p = Beveled(new Rectangle(r.X - 5, r.Y - 5, r.Width + 10, r.Height + 10), 20))
            using (PathGradientBrush glow = new PathGradientBrush(p))
            {
                glow.CenterColor = Color.FromArgb(35, color.R, color.G, color.B);
                glow.SurroundColors = new Color[] { Color.FromArgb(0, color.R, color.G, color.B) };
                g.FillPath(glow, p);
            }
        }

        internal static string CleanTitle(string source)
        {
            int end = String.IsNullOrEmpty(source) ? -1 : source.IndexOf(']');
            if (source != null && source.StartsWith("[") && end > 1) return source.Substring(end + 1).Trim();
            return source;
        }

        private static GraphicsPath Beveled(Rectangle r, int cut)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddPolygon(new Point[] { new Point(r.X + cut, r.Y), new Point(r.Right - cut, r.Y), new Point(r.Right, r.Y + cut), new Point(r.Right, r.Bottom - cut), new Point(r.Right - cut, r.Bottom), new Point(r.X + cut, r.Bottom), new Point(r.X, r.Bottom - cut), new Point(r.X, r.Y + cut) });
            p.CloseFigure();
            return p;
        }
    }

    internal sealed class NeonActionButton : Button
    {
        private bool hover;
        public NeonActionButton() { DoubleBuffered = true; SetStyle(ControlStyles.SupportsTransparentBackColor, true); FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0; UseVisualStyleBackColor = false; }
        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); ApplyShapeRegion(); }
        private void ApplyShapeRegion() { if (Width < 2 || Height < 2) return; using (GraphicsPath shape = Bevel(new Rectangle(0, 0, Width - 1, Height - 1), 12)) { Region = new Region(shape); } }
        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            Color canvas = Parent != null ? Parent.BackColor : BackColor;
            e.Graphics.Clear(canvas);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            using (GraphicsPath path = Bevel(r, 12))
            using (LinearGradientBrush fill = new LinearGradientBrush(r, hover ? Color.FromArgb(186, 41, 109) : Color.FromArgb(110, 31, 69), Color.FromArgb(63, 18, 47), LinearGradientMode.Vertical))
            using (Pen border = new Pen(Color.FromArgb(255, 116, 219), 1.2F))
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (SolidBrush text = new SolidBrush(Enabled ? Color.White : Color.FromArgb(140, 153, 170)))
            {
                e.Graphics.FillPath(fill, path); e.Graphics.DrawPath(border, path); e.Graphics.DrawString(Text, Font, text, r, center);
            }
        }
        private static GraphicsPath Bevel(Rectangle r, int c) { GraphicsPath p = new GraphicsPath(); p.AddPolygon(new Point[] { new Point(r.X+c,r.Y),new Point(r.Right-c,r.Y),new Point(r.Right,r.Y+c),new Point(r.Right,r.Bottom-c),new Point(r.Right-c,r.Bottom),new Point(r.X+c,r.Bottom),new Point(r.X,r.Bottom-c),new Point(r.X,r.Y+c)}); p.CloseFigure(); return p; }
    }

    internal sealed class FeatureShowcase : Control
    {
        private readonly Timer animator;
        private int phase;
        public int FeatureIndex { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureNote { get; set; }

        public FeatureShowcase()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(50, 35, 121);
            FeatureTitle = AppText.T("CENTRO DI CONTROLLO");
            FeatureNote = AppText.Choose("Seleziona un modulo.", "Select a module.");
            animator = new Timer(); animator.Interval = 45; animator.Tick += delegate { phase = (phase + 2) % 720; Invalidate(); }; animator.Start();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyShapeRegion();
        }

        private void ApplyShapeRegion()
        {
            if (Width < 2 || Height < 2) return;
            using (GraphicsPath shape = Bevel(new Rectangle(0, 0, Width - 1, Height - 1), 22))
            {
                Region = new Region(shape);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent != null ? Parent.BackColor : BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Math.Max(1, Width - 1), Math.Max(1, Height - 1));
            Color accent = Accent(FeatureIndex);
            using (GraphicsPath path = Bevel(r, 22))
            using (LinearGradientBrush bg = new LinearGradientBrush(r, Color.FromArgb(231, Math.Min(255, accent.R + 65), Math.Min(255, accent.G + 65), Math.Min(255, accent.B + 65)), Color.FromArgb(208, accent.R, accent.G, accent.B), LinearGradientMode.Vertical))
            { e.Graphics.FillPath(bg, path); }
            DrawGlow(e.Graphics, new Point(Width - 40 + (int)(Math.Sin(phase / 50.0) * 50), 70), 190, accent);
            DrawGlow(e.Graphics, new Point(85, Height - 20), 150, Color.FromArgb(127, 77, 255));
            for (int i = 0; i < 18; i++)
            {
                int x = (i * 71 + phase * 2) % Math.Max(1, Width); int y = 28 + ((i * 43 + phase) % Math.Max(1, Height - 55));
                using (SolidBrush dot = new SolidBrush(Color.FromArgb(80, accent.R, accent.G, accent.B))) e.Graphics.FillEllipse(dot, x, y, 2 + i % 3, 2 + i % 3);
            }
            using (Font num = new Font("Segoe UI Black", 48F, FontStyle.Bold))
            using (SolidBrush numBrush = new SolidBrush(Color.FromArgb(225, accent.R, accent.G, accent.B))) e.Graphics.DrawString(FeatureIndex.ToString("00"), num, numBrush, new PointF(28, 27));
            using (Font kicker = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
            using (SolidBrush kickerBrush = new SolidBrush(Color.FromArgb(130, 35, 21, 57))) e.Graphics.DrawString(AppText.T("MODULO ATTIVO  /  WINDOWS SPEED BOOSTER"), kicker, kickerBrush, new PointF(32, 92));
            Rectangle titleRect = new Rectangle(28, 120, Math.Max(100, Width - 56), 68);
            using (Font title = new Font("Segoe UI Semibold", 18F, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(35, 21, 57)))
            using (StringFormat fmt = new StringFormat { Trimming = StringTrimming.EllipsisCharacter }) e.Graphics.DrawString(FeatureTitle ?? String.Empty, title, titleBrush, titleRect, fmt);
            Rectangle noteRect = new Rectangle(31, 189, Math.Max(100, Width - 66), Math.Max(40, Height - 286));
            using (Font note = new Font("Segoe UI", 9.2F))
            using (SolidBrush noteBrush = new SolidBrush(Color.FromArgb(175, 35, 21, 57)))
            using (StringFormat fmt = new StringFormat { Trimming = StringTrimming.EllipsisWord }) e.Graphics.DrawString(FeatureNote ?? String.Empty, note, noteBrush, noteRect, fmt);
            Rectangle cta = new Rectangle(31, Height - 70, Math.Max(100, Width - 62), 38);
            using (GraphicsPath ctaPath = Bevel(cta, 11))
            using (LinearGradientBrush ctaFill = new LinearGradientBrush(cta, Color.FromArgb(170, 255, 246, 188), Color.FromArgb(155, 255, 201, 101), LinearGradientMode.Vertical))
            using (Font ctaFont = new Font("Segoe UI Semibold", 9.3F, FontStyle.Bold))
            using (SolidBrush ctaText = new SolidBrush(Color.FromArgb(35, 21, 57)))
            using (StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            { e.Graphics.FillPath(ctaFill, ctaPath); e.Graphics.DrawString(AppText.T("OPERAZIONI E OUTPUT NEL PANNELLO"), ctaFont, ctaText, cta, center); }
        }

        private static Color Accent(int value)
        {
            Color[] colors = new Color[] { Color.FromArgb(255,188,60), Color.FromArgb(255,74,87), Color.FromArgb(57,219,255), Color.FromArgb(67,234,138), Color.FromArgb(255,184,56), Color.FromArgb(255,151,52), Color.FromArgb(45,220,245), Color.FromArgb(184,96,255), Color.FromArgb(51,176,255), Color.FromArgb(255,79,87), Color.FromArgb(55,157,255), Color.FromArgb(195,80,255), Color.FromArgb(255,193,60), Color.FromArgb(58,206,255), Color.FromArgb(255,198,57) };
            return value >= 1 && value <= colors.Length ? colors[value - 1] : Color.FromArgb(57, 219, 255);
        }
        private static void DrawGlow(Graphics g, Point center, int radius, Color color)
        {
            using (GraphicsPath p = new GraphicsPath()) { p.AddEllipse(center.X-radius, center.Y-radius/2, radius*2, radius); using (PathGradientBrush glow = new PathGradientBrush(p)) { glow.CenterColor = Color.FromArgb(72,color.R,color.G,color.B); glow.SurroundColors = new Color[] { Color.FromArgb(0,color.R,color.G,color.B) }; g.FillPath(glow,p); } }
        }
        private static GraphicsPath Bevel(Rectangle r, int c) { GraphicsPath p = new GraphicsPath(); p.AddPolygon(new Point[] { new Point(r.X+c,r.Y),new Point(r.Right-c,r.Y),new Point(r.Right,r.Y+c),new Point(r.Right,r.Bottom-c),new Point(r.Right-c,r.Bottom),new Point(r.X+c,r.Bottom),new Point(r.X,r.Bottom-c),new Point(r.X,r.Y+c)}); p.CloseFigure(); return p; }
    }

    internal sealed class SoftSurface : Panel
    {
        public SoftSurface()
        {
            DoubleBuffered = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width < 2 || Height < 2) return;
            using (GraphicsPath shape = Rounded(new Rectangle(0, 0, Width - 1, Height - 1), 18)) { Region = new Region(shape); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = Rounded(rect, 18))
            using (LinearGradientBrush gradient = new LinearGradientBrush(rect, Color.FromArgb(42, 49, 61), Color.FromArgb(25, 31, 39), LinearGradientMode.Vertical))
            using (Pen border = new Pen(Color.FromArgb(76, 112, 140, 150), 1F))
            {
                e.Graphics.FillPath(gradient, path);
                e.Graphics.DrawPath(border, path);
            }
            using (Pen sheen = new Pen(Color.FromArgb(38, 188, 255, 227), 1F))
            {
                e.Graphics.DrawLine(sheen, 21, 14, Width - 21, 14);
            }
        }

        private static GraphicsPath Rounded(Rectangle rectangle, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal sealed class Prompt : Form
    {
        private readonly TextBox box;
        private Prompt(string caption, string title, string value)
        {
            Text = title; StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog; MinimizeBox = false; MaximizeBox = false; ClientSize = new Size(460, 148); BackColor = Color.FromArgb(29, 35, 43);
            Label label = new Label(); label.Text = caption; label.ForeColor = Color.White; label.Location = new Point(18, 16); label.Size = new Size(420, 30); Controls.Add(label);
            box = new TextBox(); box.Text = value; box.Location = new Point(18, 53); box.Size = new Size(420, 24); Controls.Add(box);
            Button ok = new Button(); ok.Text = AppText.T("Conferma"); ok.DialogResult = DialogResult.OK; ok.Location = new Point(256, 101); ok.Size = new Size(86, 30); Controls.Add(ok);
            Button cancel = new Button(); cancel.Text = AppText.T("Annulla"); cancel.DialogResult = DialogResult.Cancel; cancel.Location = new Point(352, 101); cancel.Size = new Size(86, 30); Controls.Add(cancel); AcceptButton = ok; CancelButton = cancel;
        }
        public static string Show(IWin32Window owner, string caption, string title, string value)
        {
            using (Prompt p = new Prompt(AppText.T(caption), AppText.T(title), value)) { p.box.SelectAll(); p.box.Focus(); return p.ShowDialog(owner) == DialogResult.OK ? p.box.Text : null; }
        }
    }
}
