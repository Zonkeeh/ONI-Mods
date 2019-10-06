using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace GiantsDoor
{
    public class DoorPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("GiantsDoor", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(GiantsDoorConfig.ID, GiantsDoorConfig.DisplayName, GiantsDoorConfig.Description, GiantsDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", GiantsDoorConfig.ID, "Door");
                BuildingUtils.AddToTechnology("Jobs", GiantsDoorConfig.ID);
            }
        }
    }
}
