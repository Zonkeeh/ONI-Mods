// Decompiled with JetBrains decompiler
// Type: HandSanitizerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class HandSanitizerConfig : IBuildingConfig
{
  public const string ID = "HandSanitizer";
  private const float STORAGE_SIZE = 15f;
  private const float MASS_PER_USE = 0.07f;
  private const int DISEASE_REMOVAL_COUNT = 480000;
  private const float WORK_TIME = 1.8f;
  private const SimHashes CONSUMED_ELEMENT = SimHashes.BleachStone;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "HandSanitizer";
    int width = 2;
    int height = 3;
    string anim = "handsanitizer_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] construction_materials = new string[2]
    {
      "Metal",
      "BleachStone"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, construction_materials, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_out", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_in", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_slurp", NOISE_POLLUTION.NOISY.TIER0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
    HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
    handSanitizer.massConsumedPerUse = 0.07f;
    handSanitizer.consumedElement = SimHashes.BleachStone;
    handSanitizer.diseaseRemovalCount = 480000;
    HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
    work.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_handsanitizer_kanim")
    };
    work.workTime = 1.8f;
    work.trackUses = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<DirectionControl>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = GameTagExtensions.Create(SimHashes.BleachStone);
    manualDeliveryKg.capacity = 15f;
    manualDeliveryKg.refillMass = 3f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
