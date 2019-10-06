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

namespace BiggerBrushes
{
    public class BrushPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("BiggerBrushes", "1.0.0");
                LogManager.LogInit();
            }
        }

        [HarmonyPatch(typeof(SandboxToolParameterMenu), "OnSpawn")]
        public static class SandboxToolParameterMenu_Patch
        {
            public static void Postfix(SandboxToolParameterMenu __instance) => __instance.brushRadiusSlider.SetRange(BrushConfigChecker.MinSize, BrushConfigChecker.MaxSize);
        }
    }
}
