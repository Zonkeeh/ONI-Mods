﻿using System;
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
        private static List<int> critterProofDoorCells = new List<int>();
        public static Config config;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Critter Proof Doors", "1.0.2");
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
                    critterProofDoorCells.Add(__instance.GetCell());
            }
        }

        [HarmonyPatch(typeof(Deconstructable), "TriggerDestroy")]
        public static class Deconstructable_TriggerDestroy_Patch
        {
            public static void Postfix(Deconstructable __instance)
            {
                if (CritterPathingPatches.config.TreatDefaultDoorsAsCritterProof)
                    return;

                int cell = __instance.GetCell();

                if (critterProofDoorCells.Contains(cell))
                    critterProofDoorCells.Remove(cell);

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
                    doorCheck = critterProofDoorCells.Contains(toCell);

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