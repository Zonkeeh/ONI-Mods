using System.Collections.Generic;
using TUNING;
using UnityEngine;
using Zolibrary.Utilities;

namespace ConversionChambers
{
    public class GasConversionConfig : IBuildingConfig
    {
        public static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>()
        {
         Storage.StoredItemModifier.Hide,
         Storage.StoredItemModifier.Seal
        };
        public const string ID = "GasConversionChamber";
        public const float CONVERSION_RATIO = 0.8f;
        public const float OUTPUT_TEMPERATURE_MULTIPLIER = 1.2f;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
                id: ID, 
                width: 2, 
                height: 1, 
                anim: "electrolyzer_kanim", 
                hitpoints: 100, 
                construction_time: 60f, 
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: MATERIALS.REFINED_METALS, 
                melting_point: 1600f, 
                build_location_rule: BuildLocationRule.Anywhere, 
                decor: BUILDINGS.DECOR.PENALTY.TIER2, 
                noise: NOISE_POLLUTION.NOISY.TIER2, 
                temperature_modification_mass_scale: 0.2f
                );

            buildingDef.RequiresPowerInput = true;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.EnergyConsumptionWhenActive = 960f;
            buildingDef.ExhaustKilowattsWhenActive = 1.5f;
            buildingDef.SelfHeatKilowattsWhenActive = 5f;
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.OutputConduitType = ConduitType.Liquid;
            buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            ConversionChamber chamber = go.AddOrGet<ConversionChamber>();
            chamber.maxMass = 1f;
            chamber.hasMeter = true;
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = 1f;
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.SetDefaultStoredItemModifiers(GasConversionConfig.StoredItemModifiers);
            Prioritizable.AddRef(go);
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Liquid;
            conduitConsumer.consumptionRate = 1f;
            ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
            conduitDispenser.conduitType = ConduitType.Liquid;
            conduitDispenser.elementFilter = null;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
            go.AddOrGetDef<StorageController.Def>();
        }
    }

}
