// Decompiled with JetBrains decompiler
// Type: EspressoMachineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class EspressoMachineConfig : IBuildingConfig
{
  public const string ID = "EspressoMachine";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "EspressoMachine";
    int width = 3;
    int height = 3;
    string anim = "espresso_machine_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(1, 2);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(1, 2);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 20f;
    storage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.capacityKG = 2f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = new Tag("SpiceNut");
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<EspressoMachineWorkable>();
    go.AddOrGet<EspressoMachine>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
