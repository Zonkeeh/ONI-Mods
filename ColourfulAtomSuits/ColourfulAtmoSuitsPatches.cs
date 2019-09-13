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

namespace ColourfulAtmoSuits
{
    public class ColourfulAtmoSuitsPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("Colourful Atmo Suits", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(ComplexFabricator), "SpawnOrderProduct")]
        public static class ComplexFabricator_SpawnOrderProduct_Patch
        {
            public static void Postfix(ref List<GameObject> __result, ref ComplexRecipe recipe)
            {

                if (recipe.results[0].material.Equals("Atmo_Suit".ToTag()))
                {
                    PrimaryElement primary = __result[0].GetComponent<PrimaryElement>();
                    KAnimControllerBase kBase = __result[0].GetComponent<KAnimControllerBase>();

                    LogManager.LogWarning(primary.ToString());
                    LogManager.LogWarning(kBase.ToString());

                    if (primary == null || kBase == null)
                        return;
                    else
                        kBase.TintColour = primary.Element.substance.uiColour;
                }
            }
        }
    }
}
