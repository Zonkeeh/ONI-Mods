using Zolibrary.Config;
using Zolibrary.Logging;

namespace AutosaveDragFix
{
    public static class AutosaveConfigChecker
    {
        private static Config config;

        public static bool IgnoreIfOnlyDragging { get { return ignoreIfOnlyDragging; } }
        private static bool ignoreIfOnlyDragging;

        public static bool ResetToolOnAutosave { get { return resetToolOnAutosave; } }
        private static bool resetToolOnAutosave;

        public static bool SendAutosaveWarning { get { return sendAutosaveWarning; } }
        private static bool sendAutosaveWarning;

        public static int WarningSecondsBeforeAutosave { get { return warningSecondsBeforeAutosave; } }
        private static int warningSecondsBeforeAutosave;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                AutosaveConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool ValidIntSanityCheck(int num) => (num > 0 && num < 61);

            private static void CheckConfigVariables()
            {
                int warningSeconds = 3;

                if (ValidIntSanityCheck(config.WarningSecondsBeforeAutosave))
                    warningSeconds = config.WarningSecondsBeforeAutosave;
                else
                    LogManager.LogError("Config file argument (WarningSecondsBeforeAutosave) was invalid therefore the default of 3 was used." +
                            "\nValue was not a positive integer (>0 && <61): " + config.WarningSecondsBeforeAutosave);

                AutosaveConfigChecker.ignoreIfOnlyDragging = config.IgnoreIfOnlyDragging;
                AutosaveConfigChecker.resetToolOnAutosave = config.ResetToolOnAutosave;
                AutosaveConfigChecker.sendAutosaveWarning = config.SendAutosaveWarning;
                AutosaveConfigChecker.warningSecondsBeforeAutosave = warningSeconds;
            }
        }
    }
}
