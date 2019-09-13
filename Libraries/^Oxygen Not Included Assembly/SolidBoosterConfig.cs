// Decompiled with JetBrains decompiler
// Type: SolidBoosterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolidBoosterConfig : IBuildingConfig
{
  public const string ID = "SolidBooster";
  public const float capacity = 400f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SolidBooster";
    int width = 7;
    int height = 5;
    string anim = "rocket_solid_booster_kanim";
    int hitpoints = 1000;
    float construction_time = 480f;
    float[] engineMassSmall = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_SMALL;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 9999f;
    BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engineMassSmall, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.Invincible = true;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SolidBooster solidBooster = go.AddOrGet<SolidBooster>();
    solidBooster.mainEngine = false;
    solidBooster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.BOOSTER;
    solidBooster.fuelTag = ElementLoader.FindElementByHash(SimHashes.Iron).tag;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.capacityKg = 800f;
    solidBooster.fuelStorage = storage;
    ManualDeliveryKG manualDeliveryKg1 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg1.SetStorage(storage);
    manualDeliveryKg1.requestedItemTag = solidBooster.fuelTag;
    manualDeliveryKg1.refillMass = storage.capacityKg / 2f;
    manualDeliveryKg1.capacity = storage.capacityKg / 2f;
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.SetStorage(storage);
    manualDeliveryKg2.requestedItemTag = ElementLoader.FindElementByHash(SimHashes.OxyRock).tag;
    manualDeliveryKg2.refillMass = storage.capacityKg / 2f;
    manualDeliveryKg2.capacity = storage.capacityKg / 2f;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_solid_booster_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
