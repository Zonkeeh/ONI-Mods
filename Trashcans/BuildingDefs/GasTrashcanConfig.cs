using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Trashcans
{
    public class GasTrashcanConfig : IBuildingConfig
    {
        public static string ID = TrashcansStrings.GasTrashcanID;
        public static string Name = TrashcansStrings.GasTrashcanName;
        public static string Description = TrashcansStrings.GasTrashcanDescription;
        public static string Effect = TrashcansStrings.GasTrashcanEffect;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2, "trashcan_gas_kanim",
                hitpoints: 100,
                construction_time: 60f,
                construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
                construction_materials: MATERIALS.REFINED_METALS,
                melting_point: 1600f,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER1,
                noise: TUNING.NOISE_POLLUTION.NONE,
                temperature_modification_mass_scale: 0.2f
            );
            buildingDef.RequiresPowerInput = TrashcansConfigChecker.RequiresPower;
            buildingDef.EnergyConsumptionWhenActive = TrashcansConfigChecker.EnergyConsumptionWhenActive;
            buildingDef.ExhaustKilowattsWhenActive = 0.0f;
            buildingDef.SelfHeatKilowattsWhenActive = 2f;
            buildingDef.Overheatable = TrashcansConfigChecker.CanOverheat;
            buildingDef.Floodable = TrashcansConfigChecker.CanFlood;
            buildingDef.AudioCategory = "Metal";

            buildingDef.ViewMode = OverlayModes.GasConduits.ID;
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", TUNING.NOISE_POLLUTION.NOISY.TIER1);
            Prioritizable.AddRef(go);
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<Reservoir>();

            Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
            defaultStorage.showDescriptor = true;
            defaultStorage.allowItemRemoval = false;
            defaultStorage.storageFilters = STORAGEFILTERS.GASES;
            defaultStorage.capacityKg = TrashcansConfigChecker.GasTrashCapicityKG;
            defaultStorage.showInUI = true;
            defaultStorage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);

            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Gas;
            conduitConsumer.ignoreMinMassCheck = true;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.alwaysConsume = false;
            conduitConsumer.capacityKG = defaultStorage.capacityKg;

            go.AddOrGet<Trashcan>();
            go.AddOrGet<UserNameable>();
            go.AddOrGet<ReservoirTrashcanAnim>();
        }
    }
}
