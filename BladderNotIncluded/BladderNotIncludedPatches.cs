using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zolibrary.Logging;
using Klei.CustomSettings;
using System.Linq;
using TMPro;
using Klei.AI;

namespace BladderNotIncluded
{
    public class BladderNotIncludedPatches
    {
        private static SettingConfig RemoveBladderEffects;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("BladderNotIncluded", "1.0.0");
                LogManager.LogInit();
                
                BladderNotIncludedPatches.RemoveBladderEffects = (SettingConfig)new ToggleSettingConfig(
                    id: "RemoveBladderEffects",
                    label: "Disable Duplicant Bladders",
                    tooltip: "When active, duplicants will not need to empty their bladders or gain negative effects.",
                    off_level: new SettingLevel("Disabled", "Disabled", "Unchecked: Duplicant's bladders function normally (Default)", 0, null),
                    on_level: new SettingLevel("Enabled", "Enabled", "Checked: Duplicant's bladders don't function", 0, (object)null),
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
                __instance.AddSettingConfig(BladderNotIncludedPatches.RemoveBladderEffects);
            }
        }

        [HarmonyPatch(typeof(BladderMonitor.Instance), "WantsToPee")]
        [HarmonyPatch(typeof(BladderMonitor.Instance), "NeedsToPee")]
        public static class BladderMonitor_Instance_Patch
        {
            public static bool Prefix(ref bool __result, BladderMonitor.Instance __instance)
            {
                if (BladderNotIncludedConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(BladderNotIncludedPatches.RemoveBladderEffects).id == "Enabled")
                {
                    Db.Get().Amounts.Bladder.Lookup(__instance.master.gameObject).SetValue(0.0f);
                    __result = false;
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(BladderMonitor), "InitializeStates")]
        public static class BladderMonitor_Patch
        {
            public static bool Prefix(ref StateMachine.BaseState default_state, BladderMonitor __instance)
            {
                if (BladderNotIncludedConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(BladderNotIncludedPatches.RemoveBladderEffects).id == "Enabled")
                {
                    default_state = (StateMachine.BaseState)__instance.satisfied;
                    __instance.root.TagTransition(GameTags.Minion, __instance.satisfied, false);
                    __instance.satisfied.Transition(__instance.satisfied, (smi => smi.WantsToPee()), UpdateRate.SIM_4000ms);
                    return false;
                }
                else
                    return true;
            }
        }
    }
}
