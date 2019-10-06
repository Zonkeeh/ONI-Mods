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

namespace CritterProofDoors
{
    public class CritterPathingPatches
    {
        private static Dictionary<int, int[]> critterProofDoorCells = new Dictionary<int, int[]>();
        public static Config config;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Critter Proof Doors", "1.0.5");
                LogManager.LogInit();
                ConfigManager cm = new ConfigManager();
                CritterPathingPatches.config = cm.LoadConfig<Config>(new Config());
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                CritterProofDoorConfig.Setup();
                CritterProofManualPressureDoorConfig.Setup();
                CritterProofPressureDoorConfig.Setup();
            }
        }


        [HarmonyPatch(typeof(BuildingComplete),"OnSpawn")]
        public static class BuildingComplete_OnSpawn_Patch
        {
            public static void Postfix(BuildingComplete __instance)
            {
                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    return;

                if (__instance.Def.name.ToUpper().Contains("CRITTERPROOF"))
                    foreach (int cell in __instance.PlacementCells)
                        if(!critterProofDoorCells.ContainsKey(cell))
                            critterProofDoorCells.Add(cell, __instance.PlacementCells);
            }
        }

        [HarmonyPatch(typeof(Deconstructable), "TriggerDestroy")]
        public static class Deconstructable_TriggerDestroy_Patch
        {
            public static void Postfix(Deconstructable __instance)
            {
                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    return;

                if (__instance.name.ToUpper().Contains("CRITTERPROOF") && __instance.transform != null)
                    RemoveDebugCells(__instance.GetComponent<Building>().PlacementCells[0]);
            }
        }

        [HarmonyPatch(typeof(DebugTool), "DestroyCell")]
        public static class DebugTool_DestroyCell_Patch
        {
            public static void Postfix(int cell)
            {
                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    return;

                CritterPathingPatches.RemoveDebugCells(cell);
            }
        }

        [HarmonyPatch(typeof(SandboxDestroyerTool), "OnPaintCell")]
        public static class SandboxDestroyerTool_OnPaintCell_Patch
        {
            public static void Postfix(int cell)
            {
                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    return;

                CritterPathingPatches.RemoveDebugCells(cell);
            }
        }

        private static void RemoveDebugCells(int cell)
        {
            if (CritterPathingPatches.critterProofDoorCells.ContainsKey(cell))
            {
                foreach (int tempCell in CritterPathingPatches.critterProofDoorCells.GetValueSafe(cell))
                    if (CritterPathingPatches.critterProofDoorCells.ContainsKey(tempCell))
                        CritterPathingPatches.critterProofDoorCells.Remove(tempCell);
            }
        }


        [HarmonyPatch(typeof(CreaturePathFinderAbilities), "TraversePath")]
        public static class CreaturePathFinderAbilities_TraversePath_Patch
        {
            public static void Postfix(ref bool __result, ref PathFinder.PotentialPath path)
            {               
                int toCell = path.cell;
                bool doorCheck = false;

                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    doorCheck = (Grid.BuildMasks[toCell] & Grid.BuildFlags.Door) != 0;
                else
                    doorCheck = critterProofDoorCells.ContainsKey(toCell);

                if (__result && doorCheck)
                    __result = false;
            }
        }

        [HarmonyPatch(typeof(PauseScreen), "OnQuit")]
        public static class PauseScreen_OnQuit_Patch
        {
            public static void Postfix()
            {
                CritterPathingPatches.critterProofDoorCells.Clear();
            }
        }
    }
}
