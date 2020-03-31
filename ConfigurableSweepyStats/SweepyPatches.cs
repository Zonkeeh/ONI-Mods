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
using PeterHan.PLib;
using PeterHan.PLib.UI;

namespace ConfigurableSweepyStats
{
    public class SweepyPatches
    {
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                LogManager.SetModInfo("ConfigurableSweepyStats", "1.0.0");
                LogManager.LogInit();
                PUtil.InitLibrary(true);
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                Trait trait = Db.Get().CreateTrait(
                    id: SweepyStrings.CustomTraitName,
                    name: ROBOTS.MODELS.SWEEPBOT.NAME,
                    description: ROBOTS.MODELS.SWEEPBOT.NAME,
                    group_name: null,
                    should_save: false,
                    disabled_chore_groups: (ChoreGroup[])null,
                    positive_trait: true,
                    is_valid_starter_trait: true
                    );

                trait.Add(new AttributeModifier(
                    attribute_id: Db.Get().Amounts.InternalBattery.maxAttribute.Id,
                    value: SweepyConfigChecker.BatteryCapacity,
                    description: ROBOTS.MODELS.SWEEPBOT.NAME,
                    is_multiplier: false,
                    uiOnly: false,
                    is_readonly: false
                    ));

                float battery_rate = SweepyConfigChecker.BatteryDrainBasedOnSpeed ? SweepyConfigChecker.BatteryDepletionRate*SweepyConfigChecker.BaseMovementSpeed*SweepyConfigChecker.DrainSpeedMultiplier : SweepyConfigChecker.BatteryDepletionRate;
                battery_rate = !SweepyConfigChecker.SweepyUsesPower ? 0 : battery_rate;
                trait.Add(new AttributeModifier(
                    attribute_id: Db.Get().Amounts.InternalBattery.deltaAttribute.Id,
                    value: -battery_rate,
                    description: ROBOTS.MODELS.SWEEPBOT.NAME,
                    is_multiplier: false,
                    uiOnly: false,
                    is_readonly: false
                    ));
            }
        }

        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                if(SweepyConfigChecker.UseCustomSliders)
                    PUIUtils.AddSideScreenContent<SweepBotStationSideScreen>((GameObject)null);
            }
        }

        [HarmonyPatch(typeof(SweepBotConfig), "CreatePrefab")]
        public static class SweepBotConfig_CreatePrefab_Patch
        {
            public static void Postfix(ref GameObject __result)
            {
                __result.AddOrGet<SweepyConfigurator>();
                Modifiers modifiers = __result.AddOrGet<Modifiers>();
                modifiers.initialTraits = new string[1] {"SweepBotCustomTrait"};
            }
        }

        [HarmonyPatch(typeof(SweepBotStationConfig), "CreateBuildingDef")]
        public static class SweepBotStationConfig_CreateBuildingDef_Patch
        {
            public static void Postfix(ref BuildingDef __result)
            {
                __result.RequiresPowerInput = SweepyConfigChecker.StationUsesPower;
                __result.Overheatable = SweepyConfigChecker.StationCanOverheat;
                __result.Floodable = SweepyConfigChecker.StationCanFlood;
                __result.EnergyConsumptionWhenActive = SweepyConfigChecker.StationEnergyConsumption;
            }
        }

        [HarmonyPatch(typeof(SweepBotStationConfig), "ConfigureBuildingTemplate")]
        public static class SweepBotStationConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                Storage storage = go.AddOrGet<Storage>();
                storage.capacityKg = SweepyConfigChecker.StationStorageCapacity;
            }
        }

        [HarmonyPatch(typeof(SweepBotStationConfig), "DoPostConfigureComplete")]
        public static class SweepBotStationConfig_DoPostConfigureComplete_Patch
        {
            public static void Postfix(GameObject go)
            {
                StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
                choreRangeVisualizer.x = -SweepyConfigChecker.BaseProbingRadius;
                choreRangeVisualizer.y = 0;
                choreRangeVisualizer.width = (2*SweepyConfigChecker.BaseProbingRadius)+2;
                choreRangeVisualizer.height = 1;
                choreRangeVisualizer.movable = false;
            }
        }

        [HarmonyPatch(typeof(SweepBotStation), "OnSpawn")]
        public static class SweepBotStation_OnSpawn_Patch
        {
            public static void Postfix(SweepBotStation __instance)
            {
                __instance.GetComponent<KSelectable>().AddStatusItem(SweepyStrings.StorageStatus, __instance.GetComponent<Storage>());
            }
        }
    }
}
