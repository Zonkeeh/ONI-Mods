// Decompiled with JetBrains decompiler
// Type: AlgaeHabitatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class AlgaeHabitatConfig : IBuildingConfig
{
  private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Seal
  };
  public const string ID = "AlgaeHabitat";
  private const float ALGAE_RATE = 0.03f;
  private const float WATER_RATE = 0.3f;
  private const float OXYGEN_RATE = 0.04f;
  private const float CO2_RATE = 0.0003333333f;
  private const float ALGAE_CAPACITY = 90f;
  private const float WATER_CAPACITY = 360f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "AlgaeHabitat";
    int width = 1;
    int height = 2;
    string anim = "algaefarm_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] farmable = MATERIALS.FARMABLE;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR0, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage1 = go.AddOrGet<Storage>();
    storage1.showInUI = true;
    List<Tag> tagList = new List<Tag>()
    {
      SimHashes.DirtyWater.CreateTag()
    };
    Tag tag1 = SimHashes.Algae.CreateTag();
    Tag tag2 = SimHashes.Water.CreateTag();
    Storage storage2 = go.AddComponent<Storage>();
    storage2.capacityKg = 360f;
    storage2.showInUI = true;
    storage2.SetDefaultStoredItemModifiers(AlgaeHabitatConfig.PollutedWaterStorageModifiers);
    storage2.allowItemRemoval = false;
    storage2.storageFilters = tagList;
    ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg1.SetStorage(storage1);
    manualDeliveryKg1.requestedItemTag = tag1;
    manualDeliveryKg1.capacity = 90f;
    manualDeliveryKg1.refillMass = 18f;
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.SetStorage(storage1);
    manualDeliveryKg2.requestedItemTag = tag2;
    manualDeliveryKg2.capacity = 360f;
    manualDeliveryKg2.refillMass = 72f;
    manualDeliveryKg2.allowPause = true;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_algae_terarrium_kanim")
    };
    AlgaeHabitatEmpty algaeHabitatEmpty = go.AddOrGet<AlgaeHabitatEmpty>();
    algaeHabitatEmpty.workTime = 5f;
    algaeHabitatEmpty.overrideAnims = kanimFileArray;
    algaeHabitatEmpty.workLayer = Grid.SceneLayer.BuildingFront;
    AlgaeHabitat algaeHabitat = go.AddOrGet<AlgaeHabitat>();
    algaeHabitat.lightBonusMultiplier = 1.1f;
    algaeHabitat.pressureSampleOffset = new CellOffset(0, 1);
    ElementConverter elementConverter = go.AddComponent<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
    {
      new ElementConverter.ConsumedElement(tag1, 0.03f),
      new ElementConverter.ConsumedElement(tag2, 0.3f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.04f, SimHashes.Oxygen, 303.15f, false, false, 0.0f, 1f, 1f, byte.MaxValue, 0)
    };
    go.AddComponent<ElementConverter>().outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.2903333f, SimHashes.DirtyWater, 303.15f, false, true, 0.0f, 1f, 1f, byte.MaxValue, 0)
    };
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    elementConsumer.consumptionRate = 0.0003333333f;
    elementConsumer.consumptionRadius = (byte) 3;
    elementConsumer.showInStatusPanel = true;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
    elementConsumer.isRequired = false;
    PassiveElementConsumer passiveElementConsumer = go.AddComponent<PassiveElementConsumer>();
    passiveElementConsumer.elementToConsume = SimHashes.Water;
    passiveElementConsumer.consumptionRate = 1.2f;
    passiveElementConsumer.consumptionRadius = (byte) 1;
    passiveElementConsumer.showDescriptor = false;
    passiveElementConsumer.storeOnConsume = true;
    passiveElementConsumer.capacityKG = 360f;
    passiveElementConsumer.showInStatusPanel = false;
    go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
