using System.Collections.Generic;
using TUNING;
using UnityEngine;
using WirelessPower.Components;
using WirelessPower.Configuration;

namespace WirelessPower.BuildingDefs
{
    public class WirelessBatteryConfig : IBuildingConfig
    {
        public static string ID = WirelessPowerStrings.WirelessPowerBatteryID;
        public static string Name = WirelessPowerStrings.WirelessPowerBatteryName;
        public static string Description = WirelessPowerStrings.WirelessPowerBatteryDescription;
        public static string Effect = WirelessPowerStrings.WirelessPowerBatteryEffect;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 2,
                height: 3,
                anim: "wireless_battery_kanim",
                hitpoints: 30,
                construction_time: WirelessPowerConfigChecker.BatteryBuildTime,
                construction_mass: new float[] { WirelessPowerConfigChecker.BatteryMaterialCost },
                construction_materials: WirelessPowerConfigChecker.BuildUsesOnlySteel ? new string[] {"Steel"} : MATERIALS.REFINED_METALS,
                melting_point: 400f,
                build_location_rule: BuildLocationRule.OnFloor,
                decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
                noise: TUNING.NOISE_POLLUTION.NOISY.TIER1,
                temperature_modification_mass_scale: 0.2f
            );

            SoundEventVolumeCache.instance.AddVolume("wireless_battery_kanim", "Battery_med_rattle", TUNING.NOISE_POLLUTION.NOISY.TIER2);
            buildingDef.AudioCategory = "Metal";
            buildingDef.ExhaustKilowattsWhenActive = 0.5f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
            buildingDef.Entombable = false;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<LoopingSounds>();
            go.AddOrGet<WirelessPowerBattery>();
            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
