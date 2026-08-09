# Windows Speed Booster V13

Windows Speed Booster is a comprehensive automation utility designed to streamline system maintenance and maximize hardware potential. This utility integrates deep cleaning protocols, advanced registry tweaks, and professional diagnostic tools into a centralized interface. By automating complex tasks such as TCP/IP stack resets and kernel-level optimizations, it provides a powerful solution for enhancing the Windows computing experience.

## ⚠️ Critical Warnings and Safety Information

The use of this script involves significant modifications to the Windows operating system, including the registry, system services, and boot configuration data. These changes can lead to system instability, boot failures, or unexpected software behavior if not handled with caution. Users should be aware that high-performance configurations, such as disabling Core Parking or enabling Ultimate Performance modes, will lead to increased power consumption and higher thermal output from hardware components.

While the script attempts to create a system restore point upon execution, this feature may not be available on all Windows versions (notably Windows 11). Therefore, it is imperative that users maintain a complete system backup before applying any major tweaks. The author assumes no responsibility for any damage, data loss, or hardware failure resulting from the use of this software. **Execution of this script constitutes acceptance of these risks.**

## 📋 System Requirements and Prerequisites

The latest version has been optimized to minimize external dependencies and improve compatibility across different Windows environments.

| Requirement | Description |
| :--- | :--- |
| **Privileges** | Must be executed with full **Administrator** rights (Right-click > Run as Administrator). |
| **Operating System** | Compatible with Windows 10 and Windows 11. |
| **Distribution** | Distributed as a **.zip** archive. All files must be extracted before execution. |
| **Connectivity** | Internet access and a **Groq API Token** are required for the AI Assistant. |

## 🚀 Core Functionalities

### Performance and Optimization
The script provides a multi-layered approach to performance enhancement. It begins with a deep cleaning protocol that flushes the DNS cache, clears temporary system directories, and purges the Windows Update download cache. For memory management, it includes a specialized module to calculate and set the optimal paging file size based on the total physical RAM detected. Furthermore, it offers network-specific optimizations, such as disabling Nagle's Algorithm to reduce latency in real-time applications and online gaming.

### Advanced Tweaks and Gaming
For power users and gamers, the utility unlocks hidden Windows features and optimizes hardware scheduling. This includes enabling the "Ultimate Performance" power scheme and activating Hardware-Accelerated GPU Scheduling (HAGS). To minimize input lag, the script can force a high-precision global timer resolution. It also features a "Total Gaming Mode" which disables the Xbox Game Bar, HPET, and enables exclusive fullscreen optimizations to ensure maximum frame stability.

### AI-Powered Diagnostic Tools (V13 Update)
The integrated **AI Terminal Assistant** has been significantly upgraded in V13. It now uses the **Groq Llama-3.1-8b-instant** model for near-instantaneous responses. The assistant automatically receives real-time system metrics (CPU, RAM, Disk, OS Build) as context. **Note:** A free Groq API token is now required to use this feature; the script includes a built-in tutorial to help you obtain one.

## 🛠️ Usage Instructions

1.  Download the latest `WindowsSpeedBooster.zip` from the **Releases** section.
2.  **Extract the entire ZIP folder** to a location of your choice.
3.  Right-click the `Windows_ Speed_ Booster.bat` file and select **Run as Administrator**.
4.  Follow the interactive menu to apply your desired optimizations.
5.  **Restart your computer** after applying significant changes for the best results.

---
*Disclaimer: This project is intended for educational and system optimization purposes. Always verify the source of scripts before execution.*
