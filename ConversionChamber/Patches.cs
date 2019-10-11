using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace ConversionChambers
{
    public class Patches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("ConversionChambers", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(KingDoorConfig.ID, KingDoorConfig.DisplayName, KingDoorConfig.Description, KingDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", KingDoorConfig.ID, "Door");
                BuildingUtils.AddToTechnology("Jobs", KingDoorConfig.ID);

                BuildingUtils.AddStrings(KingPowerDoorConfig.ID, KingPowerDoorConfig.DisplayName, KingPowerDoorConfig.Description, KingPowerDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", KingPowerDoorConfig.ID, "PressureDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", KingPowerDoorConfig.ID);
            }
        }
    }
}
