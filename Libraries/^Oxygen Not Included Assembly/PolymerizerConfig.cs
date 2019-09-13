// Decompiled with JetBrains decompiler
// Type: PolymerizerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PolymerizerConfig : IBuildingConfig
{
  public const string ID = "Polymerizer";
  private const ConduitType INPUT_CONDUIT_TYPE = ConduitType.Liquid;
  private const ConduitType OUTPUT_CONDUIT_TYPE = ConduitType.Gas;
  private const float CONSUMED_OIL_KG_PER_DAY = 500f;
  private const float GENERATED_PLASTIC_KG_PER_DAY = 300f;
  private const float SECONDS_PER_PLASTIC_BLOCK = 60f;
  private const float GENERATED_EXHAUST_STEAM_KG_PER_DAY = 5f;
  private const float GENERATED_EXHAUST_CO2_KG_PER_DAY = 5f;
  public const SimHashes INPUT_ELEMENT = SimHashes.Petroleum;
  private const SimHashes PRODUCED_ELEMENT = SimHashes.Polypropylene;
  private const SimHashes EXHAUST_ENVIRONMENT_ELEMENT = SimHashes.Steam;
  private const SimHashes EXHAUST_CONDUIT_ELEMENT = SimHashes.CarbonDioxide;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Polymerizer";
    int width = 3;
    int height = 3;
    string anim = "plasticrefinery_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR3, 0.2f);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 32f;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    Polymerizer polymerizer = go.AddOrGet<Polymerizer>();
    polymerizer.emitMass = 30f;
    polymerizer.emitTag = GameTagExtensions.Create(SimHashes.Polypropylene);
    polymerizer.emitOffset = new Vector3(-1.5f, 1f, 0.0f);
    polymerizer.exhaustElement = SimHashes.Steam;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 1.666667f;
    conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Petroleum);
    conduitConsumer.capacityKG = 1.666667f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.invertElementFilter = false;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    };
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(GameTagExtensions.Create(SimHashes.Petroleum), 0.8333333f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[3]
    {
      new ElementConverter.OutputElement(0.5f, SimHashes.Polypropylene, 348.15f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.008333334f, SimHashes.Steam, 473.15f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0),
      new ElementConverter.OutputElement(0.008333334f, SimHashes.CarbonDioxide, 423.15f, false, true, 0.0f, 0.5f, 1f, byte.MaxValue, 0)
    };
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
