// Decompiled with JetBrains decompiler
// Type: LiquidLogicValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidLogicValveConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_INACTIVE, true, false)
  };
  public const string ID = "LiquidLogicValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidLogicValve";
    int width = 1;
    int height = 2;
    string anim = "valveliquid_logic_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR1_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR1_2 = TUNING.NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1_1, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tieR1_2, 0.2f);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    OperationalValve operationalValve = go.AddOrGet<OperationalValve>();
    operationalValve.conduitType = ConduitType.Liquid;
    operationalValve.maxFlow = 10f;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LiquidLogicValveConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LiquidLogicValveConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    go.GetComponent<RequireInputs>().SetRequirements(true, false);
    GeneratedBuildings.RegisterLogicPorts(go, LiquidLogicValveConfig.INPUT_PORTS);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
  }
}
