using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Trashcans
{
    public class SolidTrashcanConfig : IBuildingConfig
    {
        public static string ID = TrashcansStrings.SolidTrashcanID;
        public static string Name = TrashcansStrings.SolidTrashcanName;
        public static string Description = TrashcansStrings.SolidTrashcanDescription;
        public static string Effect = TrashcansStrings.SolidTrashcanEffect;

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, 1, 2, "trashcan_solid_kanim", 
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

            buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
            buildingDef.InputConduitType = ConduitType.Solid;
            buildingDef.PowerInputOffset = new CellOffset(0, 0);
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "Trashcan");
            return buildingDef;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", TUNING.NOISE_POLLUTION.NOISY.TIER1);
            Prioritizable.AddRef(go);
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
            go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
            go.AddOrGet<EnergyConsumer>();

            List<Tag> tagList = new List<Tag>();
            tagList.AddRange((IEnumerable<Tag>)STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
            tagList.AddRange((IEnumerable<Tag>)STORAGEFILTERS.FOOD);
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = TrashcansConfigChecker.SolidTrashCapicityKG;
            storage.showInUI = true;
            storage.allowItemRemoval = true;
            storage.showDescriptor = true;
            storage.onlyTransferFromLowerPriority = true;
            storage.storageFilters = tagList;
            storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;

            SolidConduitConsumer conduitConsumer = go.AddOrGet<SolidConduitConsumer>();
            conduitConsumer.alwaysConsume = false;
            conduitConsumer.storage = storage;
            conduitConsumer.capacityKG = storage.capacityKg;

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
            go.AddOrGet<StorageLocker>();
            go.AddOrGet<Trashcan>();
            go.AddOrGet<UserNameable>();
            go.AddOrGet<SolidTrashcanAnim>();
        }
    }
}
