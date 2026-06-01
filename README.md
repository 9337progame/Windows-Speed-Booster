# Windows Speed Booster V10 - Ultimate Edition

Windows Speed Booster V10 is a comprehensive automation utility designed to streamline system maintenance and maximize hardware potential. This batch-based tool integrates deep cleaning protocols, advanced registry tweaks, and professional diagnostic utilities into a single, centralized interface. By automating complex tasks such as TCP/IP stack resets and kernel-level optimizations, it provides a powerful solution for users looking to enhance their computing experience.

## ⚠️ Critical Warnings and Safety Information

The use of this script involves significant modifications to the Windows operating system, including the registry, system services, and boot configuration data. These changes can lead to system instability, boot failures, or unexpected software behavior if not handled with caution. Users should be aware that high-performance configurations, such as disabling Core Parking or enabling Ultimate Performance modes, will lead to increased power consumption and higher thermal output from hardware components.

While the script attempts to create a system restore point upon execution, this feature may not be available on all Windows versions (notably Windows 11). Therefore, it is imperative that users maintain a complete system backup before applying any major tweaks. The author assumes no responsibility for any damage, data loss, or hardware failure resulting from the use of this software. **Execution of this script constitutes acceptance of these risks.**

## 📋 System Requirements and Prerequisites

To ensure the successful execution of all integrated tools and scripts, certain prerequisites must be met. The following table outlines the necessary environment and software dependencies.

| Requirement | Description |
| :--- | :--- |
| **Privileges** | Must be executed with full **Administrator** rights (Right-click > Run as Administrator). |
| **Operating System** | Compatible with Windows 10 and Windows 11. |
| **Software Dependency** | **Visual C++ Redistributable (vc_redist)** is required for the AI Assistant (Section 14). |
| **Connectivity** | Internet access is required for downloading external tools like `spren-ai` or MAS. |

## 🚀 Core Functionalities

### Performance and Optimization
The script provides a multi-layered approach to performance enhancement. It begins with a deep cleaning protocol that flushes the DNS cache, clears temporary system and user directories, and purges the Windows Update download cache. For memory management, it includes a specialized module to calculate and set the optimal paging file size based on the total physical RAM detected. Furthermore, it offers network-specific optimizations, such as disabling Nagle's Algorithm to reduce latency in real-time applications and online gaming.

### Advanced Tweaks and Gaming
For power users and gamers, the utility unlocks hidden Windows features and optimizes hardware scheduling. This includes enabling the "Ultimate Performance" power scheme and activating Hardware-Accelerated GPU Scheduling (HAGS). To minimize input lag, the script can force a high-precision global timer resolution of 0.5ms or 1ms. It also features a "Total Gaming Mode" which disables the Xbox Game Bar, HPET, and enables exclusive fullscreen optimizations to ensure maximum frame stability.

### Diagnostic and Professional Tools
Beyond optimization, the script includes a suite of professional tools for system analysis and security. Users can access a live performance dashboard, generate detailed hardware reports, and scan for suspicious processes running from non-standard directories. Additionally, it features utilities for recovering saved Wi-Fi passwords and Windows product keys directly from the system. For convenience, the script integrates an AI Terminal Assistant and the Microsoft Activation Scripts (MAS) for legitimate license management.

## 🛠️ Usage Instructions

The deployment process is designed to be straightforward. Users should first download the `Windows_Speed_Booster.bat` file from the **Releases** section of this repository. Once downloaded, the file must be launched with administrative privileges. The interactive menu will guide the user through the various optimization categories. It is highly recommended to **restart the computer** after applying significant changes to allow the Windows kernel and registry modifications to take full effect.

---
*Disclaimer: This project is intended for educational and system optimization purposes. Always verify the source of scripts before execution.*
