// Decompiled with JetBrains decompiler
// Type: CO2ScrubberConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CO2ScrubberConfig : IBuildingConfig
{
  public const string ID = "CO2Scrubber";
  private const float CO2_CONSUMPTION_RATE = 0.3f;
  private const float H2O_CONSUMPTION_RATE = 1f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "CO2Scrubber";
    int width = 2;
    int height = 2;
    string anim = "co2scrubber_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR3, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.ViewMode = OverlayModes.Oxygen.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = 30000f;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<AirFilter>().filterTag = GameTagExtensions.Create(SimHashes.Water);
    ElementConsumer elementConsumer = (ElementConsumer) go.AddOrGet<PassiveElementConsumer>();
    elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    elementConsumer.consumptionRate = 0.6f;
    elementConsumer.capacityKG = 0.6f;
    elementConsumer.consumptionRadius = (byte) 3;
    elementConsumer.showInStatusPanel = true;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
    elementConsumer.isRequired = false;
    elementConsumer.storeOnConsume = true;
    elementConsumer.showDescriptor = false;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
    {
      new ElementConverter.ConsumedElement(GameTagExtensions.Create(SimHashes.Water), 1f),
      new ElementConverter.ConsumedElement(GameTagExtensions.Create(SimHashes.CarbonDioxide), 0.3f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(1f, SimHashes.DirtyWater, 0.0f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
    };
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 2f;
    conduitConsumer.capacityKG = 2f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    conduitConsumer.forceAlwaysSatisfied = true;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.Water
    };
    go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
