using Zolibrary.Logging;
using Zolibrary.Config;

namespace NoMoreFlooding
{
    public static class ConfigChecker
    {
        private static Config config;

        private static bool generateFertilizer;
        public static bool GenerateFertilizer { get { return generateFertilizer; } }

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                ConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool ValidFloatSanityCheck(float num) => (num > 0f);


            private static void CheckConfigVariables()
            {
                bool temp_generateFertilizer = config.GenerateFeritilzer;

                ConfigChecker.generateFertilizer = temp_generateFertilizer;

                #if DEBUG
                    LogManager.LogDebug("CheckConfigVariables()\n" +
                        "GenerateFeritilzer: " + ConfigChecker.generateFertilizer + "\n"
                    );
                #endif
            }
        }
    }
}
