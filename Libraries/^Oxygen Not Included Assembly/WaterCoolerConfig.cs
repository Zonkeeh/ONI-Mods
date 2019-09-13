// Decompiled with JetBrains decompiler
// Type: WaterCoolerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WaterCoolerConfig : IBuildingConfig
{
  public const string ID = "WaterCooler";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WaterCooler";
    int width = 2;
    int height = 2;
    string anim = "watercooler_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 10f;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTags.Water;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 9f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<WaterCooler>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
