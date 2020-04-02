using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zolibrary.Logging;
using Klei.CustomSettings;
using System.Linq;
using TMPro;
using Klei.AI;

namespace SleepNotIncluded
{
    public class SleepNotIncludedPatches
    {
        private static SettingConfig RemoveSleepEffects;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("SleepNotIncluded", "1.0.0");
                LogManager.LogInit();
                
                SleepNotIncludedPatches.RemoveSleepEffects = (SettingConfig)new ToggleSettingConfig(
                    id: "RemoveSleepEffects",
                    label: "Disable Sleep Needs",
                    tooltip: "When active, duplicants will not need to sleep or gain negative effects.",
                    off_level: new SettingLevel("Disabled", "Disabled", "Unchecked: Duplicants will sleep normally (Default)", 0, null),
                    on_level: new SettingLevel("Enabled", "Enabled", "Checked: Duplicants won't need sleep", 0, (object)null),
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
                __instance.AddSettingConfig(SleepNotIncludedPatches.RemoveSleepEffects);
            }
        }

        [HarmonyPatch(typeof(StaminaMonitor.Instance), "WantsToSleep")]
        [HarmonyPatch(typeof(StaminaMonitor.Instance), "NeedsToSleep")]
        public static class StaminaMonitor_Instance_Patch
        {
            public static bool Prefix(ref bool __result, StaminaMonitor.Instance __instance)
            {
                if (SleepNotIncludedConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(SleepNotIncludedPatches.RemoveSleepEffects).id == "Enabled")
                {
                    Effects component = __instance.GetComponent<Effects>();

                    Effect TerribleSleep = Db.Get().effects.Get("TerribleSleep");
                    Effect BadSleep = Db.Get().effects.Get("BadSleep");
                    Effect SoreBack = Db.Get().effects.Get("SoreBack");
                    Effect PassedOut = Db.Get().effects.Get("PassedOutSleep");
                    Effect FloorSleep = Db.Get().effects.Get("FloorSleep");

                    if (component.HasEffect(TerribleSleep))
                        component.Remove(TerribleSleep);

                    if (component.HasEffect(BadSleep))
                        component.Remove(BadSleep);

                    if (component.HasEffect(SoreBack))
                        component.Remove(SoreBack);

                    if (component.HasEffect(PassedOut))
                        component.Remove(PassedOut);

                    if (component.HasEffect(FloorSleep))
                        component.Remove(FloorSleep);

                    __instance.stamina.SetValue(__instance.stamina.GetMax());
                    __result = false;
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SleepChoreMonitor), "InitializeStates")]
        public static class SleepChoreMonitor_Patch
        {
            public static bool Prefix(ref StateMachine.BaseState default_state, SleepChoreMonitor __instance)
            {
                if (SleepNotIncludedConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(SleepNotIncludedPatches.RemoveSleepEffects).id == "Enabled")
                {
                    default_state = (StateMachine.BaseState)__instance.satisfied;
                    __instance.root.TagTransition(GameTags.Minion, __instance.satisfied, false);
                    return false;
                }
                else
                    return true;
            }
        }
    }
}
