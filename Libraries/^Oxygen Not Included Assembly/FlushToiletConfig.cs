// Decompiled with JetBrains decompiler
// Type: FlushToiletConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FlushToiletConfig : IBuildingConfig
{
  private const float WATER_USAGE = 5f;
  public const string ID = "FlushToilet";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FlushToilet";
    int width = 2;
    int height = 3;
    string anim = "toiletflush_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.DiseaseCellVisName = "FoodPoisoning";
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_flush", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_door_close", NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("toiletflush_kanim", "Lavatory_door_open", NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Toilet, false);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FlushToilet, false);
    FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
    flushToilet.massConsumedPerUse = 5f;
    flushToilet.massEmittedPerUse = 11.7f;
    flushToilet.newPeeTemperature = 310.15f;
    flushToilet.diseaseId = "FoodPoisoning";
    flushToilet.diseasePerFlush = 100000;
    flushToilet.diseaseOnDupePerFlush = 5000;
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_toiletflush_kanim")
    };
    ToiletWorkableUse toiletWorkableUse = go.AddOrGet<ToiletWorkableUse>();
    toiletWorkableUse.overrideAnims = kanimFileArray;
    toiletWorkableUse.workLayer = Grid.SceneLayer.BuildingFront;
    toiletWorkableUse.resetProgressOnStop = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.capacityKG = 5f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.Water
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 25f;
    storage.doDiseaseTransfer = false;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
    ownable.canBePublic = true;
    go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
