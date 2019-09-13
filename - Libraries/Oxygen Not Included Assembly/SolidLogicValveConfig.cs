// Decompiled with JetBrains decompiler
// Type: SolidLogicValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolidLogicValveConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT_INACTIVE, true, false)
  };
  public const string ID = "SolidLogicValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SolidLogicValve";
    int width = 1;
    int height = 2;
    string anim = "conveyor_shutoff_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR1 = TUNING.NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tieR1, 0.2f);
    buildingDef.InputConduitType = ConduitType.Solid;
    buildingDef.OutputConduitType = ConduitType.Solid;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidLogicValve");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, SolidLogicValveConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, SolidLogicValveConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, SolidLogicValveConfig.INPUT_PORTS);
    go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
    go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
    go.AddOrGet<SolidConduitBridge>();
    go.AddOrGet<SolidLogicValve>();
  }
}
