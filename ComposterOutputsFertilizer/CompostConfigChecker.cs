using Zolibrary.Logging;
using Zolibrary.Config;

namespace ComposterOutputsFertilizer
{
    public static class CompostConfigChecker
    {
        private static Config config;

        private static bool generateFertilizer;
        public static bool GenerateFertilizer { get { return generateFertilizer; } }

        private static float genMultiplier;
        public static float GenMultiplier { get { return genMultiplier; } }

        private static float composterEmitMass;
        public static float EmitMass { get { return composterEmitMass; } }

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                CompostConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool ValidFloatSanityCheck(float num) => (num > 0f);


            private static void CheckConfigVariables()
            {
                bool temp_generateFertilizer = config.GenerateFeritilzer;
                float temp_genMultiplier= 1f;
                float temp_emitMass = 10f;

                if (ValidFloatSanityCheck(config.GenerationMultiplier))
                    temp_genMultiplier = config.GenerationMultiplier;
                else
                    LogManager.LogError("Config file argument (GenerationMultiplier) was invalid therefore the default of 1 was used." +
                            "\nValue was not a positive floating point number (>0): " + config.GenerationMultiplier);

                if (ValidFloatSanityCheck(config.ComposterEmitMass))
                    temp_emitMass = config.ComposterEmitMass;
                else
                    LogManager.LogError("Config file argument (ComposterEmitMass) was invalid and so the default of 10kg was used." +
                            "\nValue was not a positive floating point number (>0): " + config.ComposterEmitMass);

                CompostConfigChecker.generateFertilizer = temp_generateFertilizer;
                CompostConfigChecker.genMultiplier = temp_genMultiplier;
                CompostConfigChecker.composterEmitMass = temp_emitMass;

                #if DEBUG
                    LogManager.LogDebug("CheckConfigVariables()\n" +
                        "GenerateFeritilzer: " + CompostConfigChecker.generateFertilizer + "\n" +
                        "GenerationMultiplier: " + CompostConfigChecker.genMultiplier + "\n" +
                        "ComposterEmitMass: " + CompostConfigChecker.composterEmitMass + "\n"
                    );
                #endif
            }
        }
    }
}
