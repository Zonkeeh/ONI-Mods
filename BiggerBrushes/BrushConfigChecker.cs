using Zolibrary.Logging;
using Zolibrary.Config;

namespace BiggerBrushes
{
    public static class BrushConfigChecker
    {
        private static Config config;

        private static float minSize;
        public static float MinSize { get { return minSize; } }

        private static float maxSize;
        public static float MaxSize { get { return maxSize; } }

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                BrushConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool ValidIntSanityCheck(int num) => (num > 0);


            private static void CheckConfigVariables()
            {
                int temp_minSize = 1;
                int temp_maxSize = 10;

                if (ValidIntSanityCheck(config.MinSize))
                    temp_minSize = config.MinSize;
                else
                    LogManager.LogError("Config file argument (MinSize) was invalid therefore the default of 1 was used." +
                            "\nValue was not a positive integer (>0): " + config.MinSize);

                if (ValidIntSanityCheck(config.MaxSize))
                    temp_maxSize = config.MaxSize;
                else
                    LogManager.LogError("Config file argument (MaxSize) was invalid and so the default of 10kg was used." +
                            "\nValue was not a positive integer (>0): " + config.MaxSize);

                BrushConfigChecker.minSize = temp_minSize;
                BrushConfigChecker.maxSize = temp_maxSize;

                #if DEBUG
                    LogManager.LogDebug("CheckConfigVariables()\n" +
                        "MinSize: " + CompostConfigChecker.MinSize + "\n" +
                        "MaxSize: " + CompostConfigChecker.MaxSize + "\n" +
                    );
                #endif
            }
        }
    }
}
