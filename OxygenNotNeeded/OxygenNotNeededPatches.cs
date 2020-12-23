using Harmony;
using Klei.CustomSettings;
using Zolibrary.Logging;

namespace RevealWholeMap
{
    public class OxygenNotNeededPatches
    {
        private static SettingConfig WorldRequiresOxygen;
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("OxygenNotNeeded", "1.0.1");
                LogManager.LogInit();
                
                OxygenNotNeededPatches.WorldRequiresOxygen = (SettingConfig)new ToggleSettingConfig(
                    id: "WorldRequiresOxygen",
                    label: "Duplicants won't require Oxygen",
                    tooltip: "When active, duplicants will not need to breathe in oxygen.",
                    off_level: new SettingLevel("Disabled", "Disabled", "Unchecked: Duplicant's require Oxygen (Default)", 0, null),
                    on_level: new SettingLevel("Enabled", "Enabled", "Checked: Duplicant's won't require Oxygen", 0, (object)null),
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
                __instance.AddSettingConfig(OxygenNotNeededPatches.WorldRequiresOxygen);
            }
        }

        [HarmonyPatch(typeof(OxygenBreather), "OnSpawn")]
        public static class OxygenBreather_OnSpawn_Patch
        {
            public static void Postfix(OxygenBreather __instance)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    KSelectable component = __instance.GetComponent<KSelectable>();
                    component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2);
                    component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2);
                }
            }
        }

        [HarmonyPatch(typeof(DrowningMonitor), "IsCellSafe")]
        public static class DrowningMonitor_Patch
        {
            public static bool Prefix(ref bool __result, DrowningMonitor __instance)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    if (__instance.HasTag(GameTags.Minion))
                    {
                        __result = true;
                        return false;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(SuffocationMonitor), "InitializeStates")]
        public static class SuffocationMonitor_Patch
        {
            public static bool Prefix(ref StateMachine.BaseState default_state, SuffocationMonitor __instance)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    default_state = (StateMachine.BaseState)__instance.satisfied;
                    __instance.root.TagTransition(GameTags.Minion, __instance.satisfied, false).TagTransition(GameTags.Dead, __instance.dead, false);
                    __instance.dead.DoNothing();
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SuitSuffocationMonitor), "InitializeStates")]
        public static class SuitSuffocationMonitor_Patch
        {
            public static bool Prefix(ref StateMachine.BaseState default_state, SuitSuffocationMonitor __instance)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    default_state = (StateMachine.BaseState)__instance.satisfied;
                    __instance.root.TagTransition(GameTags.Minion, __instance.satisfied, false);
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SuitLocker), "HasOxygen")]
        public static class SuitLocker_HasOxygen_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    __result = true;
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SuitLocker), "ChargeSuit")]
        public static class SuitLocker_ChargeSuit_Patch
        {
            public static bool Prefix(SuitLocker __instance)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    KPrefabID storedOutfit = __instance.GetStoredOutfit();
                    if ((UnityEngine.Object)storedOutfit == (UnityEngine.Object)null)
                        return false;

                    SuitTank component = storedOutfit.GetComponent<SuitTank>();
                    component.amount = component.capacity;
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SuitLocker), "IsOxygenTankFull")]
        public static class SuitLocker_IsOxygenTankFull_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    __result = true;
                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(SuitTank), "ConsumeGas")]
        public static class SuitTank_ConsumeGas_Patch
        {
            public static bool Prefix(ref bool __result)
            {
                if (OxygenNotNeededConfigChecker.ForceLoad || CustomGameSettings.Instance.GetCurrentQualitySetting(OxygenNotNeededPatches.WorldRequiresOxygen).id == "Enabled")
                {
                    __result = true;
                    return false;
                }
                else
                    return true;
            }
        }

    }
}
