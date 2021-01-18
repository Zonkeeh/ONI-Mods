using TUNING;
using UnityEngine;
using WirelessPower.Components;
using WirelessPower.Configuration;

namespace WirelessPower.BuildingDefs
{
    public class WirelessReceiverConfig : IBuildingConfig
    {
		public static string ID = WirelessPowerStrings.WirelessPowerReceiverID;
        public static string Name = WirelessPowerStrings.WirelessPowerReceiverName;
        public static string Description = WirelessPowerStrings.WirelessPowerReceiverDescription;
        public static string Effect = WirelessPowerStrings.WirelessPowerReceiverEffect;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID, 
                width: 1, 
                height: 2, 
                anim: "wireless_receiver_kanim", 
                hitpoints: 30, 
                construction_time: WirelessPowerConfigChecker.SenderReceiverBuildTime,
                construction_mass: new float[] { WirelessPowerConfigChecker.SenderReceiverMaterialCost }, 
                construction_materials: WirelessPowerConfigChecker.BuildUsesOnlySteel ? new string[] { "Steel" } : MATERIALS.REFINED_METALS,
                melting_point: 400f, 
                build_location_rule: BuildLocationRule.OnFloor, 
                decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER1, 
                noise: TUNING.NOISE_POLLUTION.NONE, 
                temperature_modification_mass_scale:0.2f
            );

            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.GeneratorWattageRating = WirelessPowerBattery.CalculateEnergyAfterFalloff(WirelessPowerConfigChecker.DefaultTransfer);
            buildingDef.GeneratorBaseCapacity = 2*WirelessPowerConfigChecker.MaxTransfer;
            buildingDef.ExhaustKilowattsWhenActive = 0.25f;
            buildingDef.SelfHeatKilowattsWhenActive = 0.25f;
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PowerOutputOffset = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<WirelessPowerReceiver>();
            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
