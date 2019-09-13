// Decompiled with JetBrains decompiler
// Type: AirFilterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class AirFilterConfig : IBuildingConfig
{
  public const string ID = "AirFilter";
  public const float DIRTY_AIR_CONSUMPTION_RATE = 0.1f;
  private const float SAND_CONSUMPTION_RATE = 0.1333333f;
  private const float REFILL_RATE = 2400f;
  private const float SAND_STORAGE_AMOUNT = 320f;
  private const float CLAY_PER_LOAD = 10f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "AirFilter";
    int width = 1;
    int height = 1;
    string anim = "co2filter_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR0, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = 200f;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.elementToConsume = SimHashes.ContaminatedOxygen;
    elementConsumer.consumptionRate = 0.1f;
    elementConsumer.consumptionRadius = (byte) 3;
    elementConsumer.showInStatusPanel = true;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
    elementConsumer.isRequired = false;
    elementConsumer.storeOnConsume = true;
    elementConsumer.showDescriptor = false;
    ElementDropper elementDropper = go.AddComponent<ElementDropper>();
    elementDropper.emitMass = 10f;
    elementDropper.emitTag = new Tag("Clay");
    elementDropper.emitOffset = new Vector3(0.0f, 0.0f, 0.0f);
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
    {
      new ElementConverter.ConsumedElement(new Tag("Filter"), 0.1333333f),
      new ElementConverter.ConsumedElement(new Tag("ContaminatedOxygen"), 0.1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(0.1433333f, SimHashes.Clay, 0.0f, false, true, 0.0f, 0.5f, 0.25f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.09f, SimHashes.Oxygen, 0.0f, false, false, 0.0f, 0.0f, 0.75f, byte.MaxValue, 0)
    };
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(defaultStorage);
    manualDeliveryKg.requestedItemTag = new Tag("Filter");
    manualDeliveryKg.capacity = 320f;
    manualDeliveryKg.refillMass = 32f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    go.AddOrGet<AirFilter>().filterTag = new Tag("Filter");
    go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<ActiveController.Def>();
  }
}
