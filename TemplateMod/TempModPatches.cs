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

namespace $temp_namespace$
{
    public class TempModPatches
    {
        public static Config config;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("TempMod", "1.0.0");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                TempModPatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                //Call setup methods and add strings, planning, tech
            }
        }
    }
}
