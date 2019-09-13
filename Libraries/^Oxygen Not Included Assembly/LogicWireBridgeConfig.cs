// Decompiled with JetBrains decompiler
// Type: LogicWireBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LogicWireBridgeConfig : IBuildingConfig
{
  public static readonly HashedString BRIDGE_LOGIC_IO_ID = new HashedString("BRIDGE_LOGIC_IO");
  public static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[2]
  {
    LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(-1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false),
    LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false)
  };
  public const string ID = "LogicWireBridge";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LogicWireBridge";
    int width = 3;
    int height = 1;
    string anim = "logic_bridge_kanim";
    int hitpoints = 30;
    float construction_time = 3f;
    float[] tierTiny = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.LogicBridge;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tierTiny, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.ObjectLayer = ObjectLayer.LogicGates;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicWireBridge");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
    GeneratedBuildings.RegisterLogicPorts(go, LogicWireBridgeConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
    GeneratedBuildings.RegisterLogicPorts(go, LogicWireBridgeConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
    GeneratedBuildings.RegisterLogicPorts(go, LogicWireBridgeConfig.INPUT_PORTS);
  }

  private LogicUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    LogicUtilityNetworkLink utilityNetworkLink = go.AddOrGet<LogicUtilityNetworkLink>();
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
