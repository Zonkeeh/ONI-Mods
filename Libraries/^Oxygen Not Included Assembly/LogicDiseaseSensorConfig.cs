// Decompiled with JetBrains decompiler
// Type: LogicDiseaseSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LogicDiseaseSensorConfig : IBuildingConfig
{
  public static string ID = "LogicDiseaseSensor";
  public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICDISEASESENSOR.LOGIC_PORT_INACTIVE, true, false);

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicDiseaseSensorConfig.ID;
    int width = 1;
    int height = 1;
    string anim = "diseasesensor_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "Plastic"
    };
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("diseasesensor_kanim", "PowerSwitch_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicDiseaseSensorConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicDiseaseSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicDiseaseSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    GeneratedBuildings.RegisterLogicPorts(go, LogicDiseaseSensorConfig.OUTPUT_PORT);
    LogicDiseaseSensor logicDiseaseSensor = go.AddOrGet<LogicDiseaseSensor>();
    logicDiseaseSensor.Threshold = 0.0f;
    logicDiseaseSensor.ActivateAboveThreshold = true;
    logicDiseaseSensor.manuallyControlled = false;
  }
}
