using System;
using System.Collections.Generic;
using Database;
using Harmony;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;

namespace OxygenNotNeeded
{
    public static class OxygenNotNeededConfigChecker
    {
        private static Config config;

        public static bool ForceLoad { get { return forceLoad; } }
        private static bool forceLoad;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                OxygenNotNeededConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static void CheckConfigVariables()
            {
                forceLoad = config.ForceLoad;
            }
        }
    }
}
