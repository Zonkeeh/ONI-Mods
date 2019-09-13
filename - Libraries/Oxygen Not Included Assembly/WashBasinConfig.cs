// Decompiled with JetBrains decompiler
// Type: WashBasinConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WashBasinConfig : IBuildingConfig
{
  public const string ID = "WashBasin";
  public const int DISEASE_REMOVAL_COUNT = 120000;
  public const float WATER_PER_USE = 5f;
  public const int USES_PER_FLUSH = 40;
  public const float WORK_TIME = 5f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WashBasin";
    int width = 2;
    int height = 3;
    string anim = "wash_basin_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    return BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, rawMinerals, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, tieR0, 0.2f);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
    HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
    handSanitizer.massConsumedPerUse = 5f;
    handSanitizer.consumedElement = SimHashes.Water;
    handSanitizer.outputElement = SimHashes.DirtyWater;
    handSanitizer.diseaseRemovalCount = 120000;
    handSanitizer.maxUses = 40;
    handSanitizer.dumpWhenFull = true;
    go.AddOrGet<DirectionControl>();
    HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
    work.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_washbasin_kanim")
    };
    work.workTime = 5f;
    work.trackUses = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTagExtensions.Create(SimHashes.Water);
    manualDeliveryKg.minimumMass = 5f;
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 40f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    go.AddOrGet<LoopingSounds>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
