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

namespace Trashcans
{
    public class TrashcansPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Trashcans", "1.0.1");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(GasTrashcanConfig.ID, GasTrashcanConfig.Name, GasTrashcanConfig.Description, GasTrashcanConfig.Effect);
                BuildingUtils.AddToPlanning("Base", GasTrashcanConfig.ID, "GasReservoir");
                BuildingUtils.AddToTechnology("SmartStorage", GasTrashcanConfig.ID);

                BuildingUtils.AddStrings(LiquidTrashcanConfig.ID, LiquidTrashcanConfig.Name, LiquidTrashcanConfig.Description, LiquidTrashcanConfig.Effect);
                BuildingUtils.AddToPlanning("Base", LiquidTrashcanConfig.ID, "LiquidReservoir");
                BuildingUtils.AddToTechnology("SmartStorage", LiquidTrashcanConfig.ID);

                BuildingUtils.AddStrings(SolidTrashcanConfig.ID, SolidTrashcanConfig.Name, SolidTrashcanConfig.Description, SolidTrashcanConfig.Effect);
                BuildingUtils.AddToPlanning("Base", SolidTrashcanConfig.ID, "StorageLockerSmart");
                BuildingUtils.AddToTechnology("SmartStorage", SolidTrashcanConfig.ID);
            }
        }
    }
}
