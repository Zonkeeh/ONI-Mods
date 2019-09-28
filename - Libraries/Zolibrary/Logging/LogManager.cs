using System;
using System.Reflection;
using System.Linq;
using Harmony;

namespace Zolibrary.Logging
{
    public static class LogManager
    {
        private static string modName = string.Empty;
        private static string modVersion = string.Empty;

        public static void SetModInfo(string name, string version)
        {
            LogManager.modName = name;
            LogManager.modVersion = version;
        }

        public static string GetModName()
        {
            if (LogManager.modName != string.Empty)
                return LogManager.modName;
            else
                return Assembly.GetExecutingAssembly().GetName().Name.Replace("-Merged","");
        }

        private static string GetModVersion()
        {
            if (LogManager.modVersion != string.Empty)
                return LogManager.modVersion;
            else
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void LogInit()
        {
            Debug.Log("[Zolibrary] [Zonkeeh] -> Successfully loaded the mod " + GetModName() + " with version " + GetModVersion());
        }

        public static void Log(string message)
        {
            Debug.Log("[" + GetModName() + "] [Zonkeeh] -> " + message);
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning("[" + GetModName() + "] [Zonkeeh] -> " + message);
        }

        public static void LogDebug(string message)
        {
            Debug.Log("[DEBUG] [" + GetModName() + "] [Zonkeeh] -> " + message);
        }

        public static void LogError(string message)
        {
            Debug.LogError("[" + GetModName() + "] [Zonkeeh] -> " + message);
        }

        public static void LogException(string message, Exception e)
        {
            Debug.LogError("[" + GetModName() + "] [Zonkeeh] -> " + message + "\n" + e);
        }
    }
}
