// Decompiled with JetBrains decompiler
// Type: OilWellCapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class OilWellCapConfig : IBuildingConfig
{
  private const float WATER_INTAKE_RATE = 1f;
  private const float WATER_TO_OIL_RATIO = 3.333333f;
  private const float LIQUID_STORAGE = 10f;
  private const float GAS_RATE = 0.03333334f;
  private const float OVERPRESSURE_TIME = 2400f;
  private const float PRESSURE_RELEASE_TIME = 180f;
  private const float PRESSURE_RELEASE_RATE = 0.4444445f;
  public const string ID = "OilWellCap";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "OilWellCap";
    int width = 4;
    int height = 4;
    string anim = "geyser_oil_cap_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 1);
    buildingDef.PowerInputOffset = new CellOffset(1, 1);
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.OilWell;
    buildingDef.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
    buildingDef.ObjectLayer = ObjectLayer.AttachableBuilding;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityKG = 10f;
    conduitConsumer.capacityTag = GameTags.Liquid;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("Water"), 1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(3.333333f, SimHashes.CrudeOil, 363.15f, false, false, 2f, 1.5f, 0.0f, byte.MaxValue, 0)
    };
    OilWellCap oilWellCap = go.AddOrGet<OilWellCap>();
    oilWellCap.gasElement = SimHashes.Methane;
    oilWellCap.gasTemperature = 573.15f;
    oilWellCap.addGasRate = 0.03333334f;
    oilWellCap.maxGasPressure = 80.00001f;
    oilWellCap.releaseGasRate = 0.4444445f;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
    go.AddOrGet<LogicOperationalController>();
  }
}
