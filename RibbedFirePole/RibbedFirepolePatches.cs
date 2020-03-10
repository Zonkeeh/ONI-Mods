using System;
using System.Collections.Generic;
using Database;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;

namespace RibbedFirePole
{
    public class RibbedFirePolePatches
    {
        public static Config config;

        public class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Ribbed Fire Pole", "1.0.3");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                RibbedFirePolePatches.config = cm.LoadConfig<Config>(new Config());
            }
        }


        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch("LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(RibbedFirePoleConfig.ID, RibbedFirePoleConfig.DisplayName, RibbedFirePoleConfig.Description, RibbedFirePoleConfig.Effect);
                BuildingUtils.AddToPlanning("Base", RibbedFirePoleConfig.ID, "FirePole");
                BuildingUtils.AddToTechnology("HighTempForging", RibbedFirePoleConfig.ID);
            }
        }
    }
}
