// Decompiled with JetBrains decompiler
// Type: OuthouseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class OuthouseConfig : IBuildingConfig
{
  public const string ID = "Outhouse";
  private const int USES_PER_REFILL = 15;
  private const float DIRT_PER_REFILL = 200f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Outhouse";
    int width = 2;
    int height = 3;
    string anim = "outhouse_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER4, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.DiseaseCellVisName = "FoodPoisoning";
    buildingDef.AudioCategory = "Metal";
    SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Toilet, false);
    Toilet toilet = go.AddOrGet<Toilet>();
    toilet.maxFlushes = 15;
    toilet.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.ToxicSand, 6.7f, 0.0f);
    toilet.solidWasteTemperature = 310.15f;
    toilet.gasWasteWhenFull = new Toilet.SpawnInfo(SimHashes.ContaminatedOxygen, 0.1f, 15f);
    toilet.diseaseId = "FoodPoisoning";
    toilet.diseasePerFlush = 100000;
    toilet.diseaseOnDupePerFlush = 100000;
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_outhouse_kanim")
    };
    ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
    toiletWorkableUse.overrideAnims = kanimFileArray;
    toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingFront;
    ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
    toiletWorkableClean.workTime = 90f;
    toiletWorkableClean.overrideAnims = kanimFileArray;
    toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = new Tag("Dirt");
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 0.01f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
    ownable.canBePublic = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
