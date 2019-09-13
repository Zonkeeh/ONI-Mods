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

namespace ShineBugLights
{
    public class ShineLightPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(ShineLightConfig.ID, ShineLightConfig.DisplayName, ShineLightConfig.Description, ShineLightConfig.Effect);
                BuildingUtils.AddToPlanning("Furniture", ShineLightConfig.ID, "CeilingLight");
                BuildingUtils.AddToTechnology("Artistry", ShineLightConfig.ID);
            }
        }
    }
}
