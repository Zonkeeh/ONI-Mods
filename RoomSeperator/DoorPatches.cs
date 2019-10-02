using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Utilities;

namespace RoomSeperator
{
    public class DoorPatches
    {

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("AutomopSpreadingLiquids", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddStrings(RoomSeperatorConfig.ID, RoomSeperatorConfig.DisplayName, RoomSeperatorConfig.Description, RoomSeperatorConfig.Effect);
                BuildingUtils.AddToPlanning("Base", RoomSeperatorConfig.ID, "BunkerDoor");
                BuildingUtils.AddToTechnology("RefinedObjects", RoomSeperatorConfig.ID);
            }
        }
    }
}
