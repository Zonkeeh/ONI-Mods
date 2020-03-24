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
using Klei.CustomSettings;

namespace StartWithAllResearch
{
    public class StartWithAllResearchPatches
    {
        private static SettingConfig StartWithAllResearch;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("StartWithAllResearch", "1.0.0");
                LogManager.LogInit();
                StartWithAllResearchPatches.StartWithAllResearch = (SettingConfig)new ToggleSettingConfig(
                    id: "StartWithAllResearch",
                    label: "Start with all Research",
                    tooltip: "When active will start a save with all research nodes completed/researched.",
                    off_level: new SettingLevel("Disabled", "Disabled", "Unchecked: Start with all Research is turned off (Default)", 0, null),
                    on_level: new SettingLevel("Enabled", "Enabled", "Checked: Start with all Research is turned on", 0, (object)null),
                    default_level_id: "Disabled",
                    nosweat_default_level_id: "Disabled",
                    coordinate_dimension: -1,
                    coordinate_dimension_width: -1,
                    debug_only: false
                    );
            }
        }

        [HarmonyPatch(typeof(CustomGameSettings), "OnPrefabInit")]
        public class CustomGameSettings_OnPrefabInit_Patch
        {
            public static void Postfix(CustomGameSettings __instance)
            {
                __instance.AddSettingConfig(StartWithAllResearchPatches.StartWithAllResearch);
            }
        }

        [HarmonyPatch(typeof(ResearchScreen), "OnSpawn")]
        public class Game_OnSpawn_Patch
        {
            public static void Postfix(ResearchScreen __instance)
            {
                if (CustomGameSettings.Instance.GetCurrentQualitySetting(StartWithAllResearchPatches.StartWithAllResearch).id != "Enabled")
                    return;

                foreach (Tech tech in Db.Get().Techs.resources)
                {
                    if (!tech.IsComplete())
                    {
                        ResearchEntry entry = __instance.GetEntry(tech);
                        if ((UnityEngine.Object)entry != (UnityEngine.Object)null)
                            entry.ResearchCompleted(false);
                    }
                }

                Traverse.Create(__instance).Method("UpdateProgressBars").GetValue();
                Traverse.Create(__instance).Method("UpdatePointDisplay").GetValue();
            }
        }
    }
}
