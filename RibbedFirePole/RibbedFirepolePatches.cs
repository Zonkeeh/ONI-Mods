using System;
using System.Collections.Generic;
using Database;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace RibbedFirePole
{
    public class RibbedFirePolePatches
    {
        public class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("[Zonkeeh] [RibbedFirePole] - " + Timestamp() + " - Succesfully loaded mod.");
            }
            private static string Timestamp()
            {
                return System.DateTime.UtcNow.ToString("[HH:mm:ss]");
            }
        }


        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch("LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                AddStrings("RibbedFirePole", "Ribbed Fire Pole", "A slippery fie pole with drilled climing notches.", "All the perks in one. Fast climbing and sliding.");
                AddPlanning("Base", "RibbedFirePole", "FirePole");
            }

            private static void AddStrings(string ID, string Name, string Description, string Effect) {
                Strings.Add(new string[]
                {
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".NAME",
                    UI.FormatAsLink(Name, ID)
                });

                Strings.Add(new string[]
                {
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".DESC",
                    Description
                });

                Strings.Add(new string[]
                {
                    "STRINGS.BUILDINGS.PREFABS." + ID.ToUpperInvariant() + ".EFFECT",
                    Effect
                });
            }

            private static void AddPlanning(HashedString Category, string ID, string beforeID)
            {
                int catIndex = TUNING.BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo y) => y.category == Category);

                if (catIndex < 0)
                {
                    Debug.LogError("[Zonkeeh] [RibbedFirePole] Error adding fire pole to planning list. Category doesn't exist :" + Category);
                    return;
                }

                IList<string> list = TUNING.BUILDINGS.PLANORDER[catIndex].data as IList<string>;

                int index = -1;

                foreach (string s in list) {
                    if (s.Equals(beforeID)) {
                        index = list.IndexOf(s);
                    }
                }

                if (index == -1)
                {
                    Debug.LogError("[Zonkeeh] [RibbedFirePole] Error adding fire pole to planning list. ID doesn't exist :" + Category + " " + beforeID);
                    return;
                }
                else {
                    list.Insert(index + 1, ID);
                }
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                AddTechnology("HighTempForging", "RibbedFirePole");
            }

            private static void AddTechnology(string Tech, string ID)
            {
                List<string> list = new List<string>(Techs.TECH_GROUPING[Tech]){ID};
                Techs.TECH_GROUPING[Tech] = list.ToArray();
            }
        }
    }
}
