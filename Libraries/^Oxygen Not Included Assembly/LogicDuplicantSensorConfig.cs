// Decompiled with JetBrains decompiler
// Type: LogicDuplicantSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LogicDuplicantSensorConfig : IBuildingConfig
{
  public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_INACTIVE, true, false);
  public const string ID = "LogicDuplicantSensor";
  private const int RANGE = 4;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicDuplicantSensor", 1, 1, "presence_sensor_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicDuplicantSensor");
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicDuplicantSensorConfig.OUTPUT_PORT);
    LogicDuplicantSensorConfig.AddVisualizer(go, true);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicDuplicantSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    GeneratedBuildings.RegisterLogicPorts(go, LogicDuplicantSensorConfig.OUTPUT_PORT);
    LogicDuplicantSensor logicDuplicantSensor = go.AddOrGet<LogicDuplicantSensor>();
    logicDuplicantSensor.defaultState = false;
    logicDuplicantSensor.manuallyControlled = false;
    logicDuplicantSensor.pickupRange = 4;
    LogicDuplicantSensorConfig.AddVisualizer(go, false);
  }

  private static void AddVisualizer(GameObject prefab, bool movable)
  {
    StationaryChoreRangeVisualizer choreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
    choreRangeVisualizer.x = -2;
    choreRangeVisualizer.y = 0;
    choreRangeVisualizer.width = 5;
    choreRangeVisualizer.height = 5;
    choreRangeVisualizer.movable = movable;
  }
}
