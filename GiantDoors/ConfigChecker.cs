using Zolibrary.Logging;
using Zolibrary.Config;

namespace GiantsDoor
{
    public static class ConfigChecker
    {
        private static Config config;

        private static float normalSpeed;
        public static float NormalSpeed { get { return normalSpeed; } }

        private static float poweredSpeedUnpowered;
        public static float PoweredSpeedUnpowered { get { return poweredSpeedUnpowered; } }

        private static float poweredSpeedPowered;
        public static float PoweredSpeed { get { return poweredSpeedPowered; } }

        private static float animMultiplier = 8f;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                ConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool ValidFloatSanityCheck(float num) => (num > 0f && num <= 15f);


            private static void CheckConfigVariables()
            {
                float temp_normalSpeed = 1f*animMultiplier;
                float temp_poweredSpeedUnpowered= 0.9f*animMultiplier;
                float temp_poweredSpeed = 1.2f*animMultiplier;

                if (ValidFloatSanityCheck(config.NormalSpeed))
                    temp_normalSpeed = config.NormalSpeed*animMultiplier;
                else
                    LogManager.LogError("Config file argument (NormalSpeed) was invalid therefore the default of 1 was used." +
                            "\nValue was not a positive floating point number (>0 && <=15): " + config.NormalSpeed);

                if (ValidFloatSanityCheck(config.PoweredDoorUnpoweredSpeed))
                    temp_poweredSpeedUnpowered = config.PoweredDoorUnpoweredSpeed*animMultiplier;
                else
                    LogManager.LogError("Config file argument (PoweredDoorUnpoweredSpeed) was invalid therefore the default of 0.9 was used." +
                            "\nValue was not a positive floating point number (>0 && <=15): " + config.PoweredDoorUnpoweredSpeed);

                if (ValidFloatSanityCheck(config.PoweredDoorPoweredSpeed))
                    temp_poweredSpeed = config.PoweredDoorPoweredSpeed*animMultiplier;
                else
                    LogManager.LogError("Config file argument (PoweredDoorPoweredSpeed) was invalid therefore the default of 1.2 was used." +
                            "\nValue was not a positive floating point number (>0 && <=15): " + config.PoweredDoorPoweredSpeed);

                ConfigChecker.normalSpeed = temp_normalSpeed;
                ConfigChecker.poweredSpeedUnpowered = temp_poweredSpeedUnpowered;
                ConfigChecker.poweredSpeedPowered = temp_poweredSpeed;

#if DEBUG
                    LogManager.LogDebug("CheckConfigVariables()\n" +
                        "NormalSpeed: " + CompostConfigChecker.normalSpeed + "\n" +
                        "PoweredDoorUnpoweredSpeed: " + CompostConfigChecker.poweredSpeedUnpowered + "\n" +
                        "PoweredDoorPoweredSpeed: " + CompostConfigChecker.poweredSpeedPowered + "\n"
                    );
#endif
            }
        }
    }
}
