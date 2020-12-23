using System;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;
using static DetailsScreen;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScannerPatches
    {

        private static Dictionary<int, int[]> AdvancedScannerCells;
        private  static Config config;
        public static Config Config {get { return config; }}

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("AdvancedSpaceScanner", "1.0.5");
                LogManager.LogInit();
                AdvancedSpaceScannerPatches.AdvancedScannerCells = new Dictionary<int, int[]>();

                ConfigManager cm = new ConfigManager();
                AdvancedSpaceScannerPatches.config = cm.LoadConfig<Config>(new Config());
            }
        }
        
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BuildingUtils.AddToPlanning("Automation", AdvancedSpaceScannerConfig.ID, CometDetectorConfig.ID);
                BuildingUtils.AddToTechnology("BasicRocketry", AdvancedSpaceScannerConfig.ID);
                LocString.CreateLocStringKeys(typeof(AdvancedSpaceScannerStrings.BUILDINGS));
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix(List<SideScreenRef> ___sideScreens, ref GameObject ___sideScreenContentBody)
            {
                AdvancedSpaceScannerSideScreen advancedSpaceScanner = (AdvancedSpaceScannerSideScreen) ___sideScreenContentBody.AddComponent(typeof(AdvancedSpaceScannerSideScreen));

                ___sideScreens.Add(new SideScreenRef
                {
                    name = "AdvancedSpaceScannerSideScreen",
                    offset = Vector2.zero,
                    screenPrefab = advancedSpaceScanner
                });;

            }
        }

        [HarmonyPatch(typeof(UnstableGroundManager), "Spawn", new Type[] { typeof(int), typeof(Element), typeof(float), typeof(float), typeof(byte), typeof(int) })]
        public static class UnstableGroundManager_Spawn_Patch
        {
            public static bool Prefix(int cell)
            {
                if (config.DoesRegolithSpawn)
                    return true;
                else if (AdvancedSpaceScannerPatches.AdvancedScannerCells.ContainsKey(cell))
                    return false;
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (config.DoesRegolithSpawn)
                    return;

                if (__instance.Def.Name.Contains(AdvancedSpaceScannerConfig.DisplayName))
                    foreach (int cell in __instance.PlacementCells)
                        AdvancedSpaceScannerPatches.AdvancedScannerCells.Add(cell, __instance.PlacementCells);
            }
        }

        [HarmonyPatch(typeof(Deconstructable), "TriggerDestroy")]
        public static class Deconstructable_TriggerDestroy_Patch
        {
            public static void Postfix(Deconstructable __instance)
            {
                if (config.DoesRegolithSpawn)
                    return;

                Building temp = __instance.GetComponent<Building>();

                if (temp.Def.Name.Contains(AdvancedSpaceScannerConfig.DisplayName) && __instance.transform != null)
                    RemoveDebugCells(temp.PlacementCells[0]);
            }
        }

        [HarmonyPatch(typeof(DebugTool), "DestroyCell")]
        public static class DebugTool_DestroyCell_Patch
        {
            public static void Postfix(int cell)
            {
                if (!config.DoesRegolithSpawn)
                    AdvancedSpaceScannerPatches.RemoveDebugCells(cell);
            }
        }

        [HarmonyPatch(typeof(SandboxDestroyerTool), "OnPaintCell")]
        public static class SandboxDestroyerTool_OnPaintCell_Patch
        {
            public static void Postfix(int cell)
            {
                if (!config.DoesRegolithSpawn)
                    AdvancedSpaceScannerPatches.RemoveDebugCells(cell);
            }
        }

        private static void RemoveDebugCells(int cell)
        {
            if (AdvancedSpaceScannerPatches.AdvancedScannerCells.ContainsKey(cell))
                foreach (int tempCell in AdvancedSpaceScannerPatches.AdvancedScannerCells[cell])
                    if (AdvancedSpaceScannerPatches.AdvancedScannerCells.ContainsKey(tempCell))
                        AdvancedSpaceScannerPatches.AdvancedScannerCells.Remove(tempCell); 
        }

        [HarmonyPatch(typeof(PauseScreen), "OnQuit")]
        public static class PauseScreen_OnQuit_Patch
        {
            public static void Postfix()
            {
                if (!config.DoesRegolithSpawn)
                    AdvancedSpaceScannerPatches.AdvancedScannerCells.Clear();
            }
        }
    }
}
