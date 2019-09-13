// Decompiled with JetBrains decompiler
// Type: OilRefineryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class OilRefineryConfig : IBuildingConfig
{
  public const string ID = "OilRefinery";
  public const SimHashes INPUT_ELEMENT = SimHashes.CrudeOil;
  private const SimHashes OUTPUT_LIQUID_ELEMENT = SimHashes.Petroleum;
  private const SimHashes OUTPUT_GAS_ELEMENT = SimHashes.Methane;
  public const float CONSUMPTION_RATE = 10f;
  public const float OUTPUT_LIQUID_RATE = 5f;
  public const float OUTPUT_GAS_RATE = 0.09f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "OilRefinery";
    int width = 4;
    int height = 4;
    string anim = "oilrefinery_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3_2 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3_1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR3_2, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 2f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    OilRefinery oilRefinery = go.AddOrGet<OilRefinery>();
    oilRefinery.overpressureWarningMass = 4.5f;
    oilRefinery.overpressureMass = 5f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = SimHashes.CrudeOil.CreateTag();
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.capacityKG = 100f;
    conduitConsumer.forceAlwaysSatisfied = true;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.CrudeOil
    };
    go.AddOrGet<Storage>().showInUI = true;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(SimHashes.CrudeOil.CreateTag(), 10f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(5f, SimHashes.Petroleum, 348.15f, false, true, 0.0f, 1f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.09f, SimHashes.Methane, 348.15f, false, false, 0.0f, 3f, 1f, byte.MaxValue, 0)
    };
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
