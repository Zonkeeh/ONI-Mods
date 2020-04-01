using Zolibrary.Config;

namespace BladderNotIncluded
{
    public static class BladderNotIncludedConfigChecker
    {
        private static Config config;

        public static bool ForceLoad { get { return forceLoad; } }
        private static bool forceLoad;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                BladderNotIncludedConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static void CheckConfigVariables()
            {
                forceLoad = config.ForceLoad;
            }
        }
    }
}
