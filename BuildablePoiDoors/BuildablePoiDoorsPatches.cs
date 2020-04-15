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

namespace BuildablePoiDoors
{
    public class BuildablePoiDoorsPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Buildable POI Doors", "1.0.3");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(BuildablePOIFacilityDoorConfig.ID, BuildablePOIFacilityDoorConfig.DisplayName, BuildablePOIFacilityDoorConfig.Description, BuildablePOIFacilityDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", BuildablePOIFacilityDoorConfig.ID, "ManualPressureDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIFacilityDoorConfig.ID);

                BuildingUtils.AddStrings(BuildablePOISecurityDoorConfig.ID, BuildablePOISecurityDoorConfig.DisplayName, BuildablePOISecurityDoorConfig.Description, BuildablePOISecurityDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", BuildablePOISecurityDoorConfig.ID, "PressureDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOISecurityDoorConfig.ID);

                BuildingUtils.AddStrings(BuildablePOIInternalDoorConfig.ID, BuildablePOIInternalDoorConfig.DisplayName, BuildablePOIInternalDoorConfig.Description, BuildablePOIInternalDoorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", BuildablePOIInternalDoorConfig.ID, "Door");
                BuildingUtils.AddToTechnology("RefinedObjects", BuildablePOIInternalDoorConfig.ID);
            }
        }
    }
}
