// Decompiled with JetBrains decompiler
// Type: WaterPurifierConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WaterPurifierConfig : IBuildingConfig
{
  public const string ID = "WaterPurifier";
  private const float FILTER_INPUT_RATE = 1f;
  private const float DIRTY_WATER_INPUT_RATE = 5f;
  private const float FILTER_CAPACITY = 1200f;
  private const float USED_FILTER_OUTPUT_RATE = 0.2f;
  private const float CLEAN_WATER_OUTPUT_RATE = 5f;
  private const float TARGET_OUTPUT_TEMPERATURE = 313.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "WaterPurifier";
    int width = 4;
    int height = 3;
    string anim = "waterpurifier_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tieR3_2, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.PowerInputOffset = new CellOffset(2, 0);
    buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<WaterPurifier>();
    Prioritizable.AddRef(go);
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
    {
      new ElementConverter.ConsumedElement(new Tag("Filter"), 1f),
      new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 5f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(5f, SimHashes.Water, 0.0f, false, true, 0.0f, 0.5f, 0.75f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0.0f, false, true, 0.0f, 0.5f, 0.25f, byte.MaxValue, 0)
    };
    ElementDropper elementDropper = go.AddComponent<ElementDropper>();
    elementDropper.emitMass = 10f;
    elementDropper.emitTag = new Tag("ToxicSand");
    elementDropper.emitOffset = new Vector3(0.0f, 1f, 0.0f);
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(defaultStorage);
    manualDeliveryKg.requestedItemTag = new Tag("Filter");
    manualDeliveryKg.capacity = 1200f;
    manualDeliveryKg.refillMass = 300f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityKG = 20f;
    conduitConsumer.capacityTag = GameTags.AnyWater;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.DirtyWater
    };
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_N1_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_N1_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_N1_0);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
