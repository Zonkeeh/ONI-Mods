// Decompiled with JetBrains decompiler
// Type: LogicMemoryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LogicMemoryConfig : IBuildingConfig
{
  public static string ID = "LogicMemory";
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[2]
  {
    new LogicPorts.Port(LogicMemory.SET_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.SET_PORT_INACTIVE, true, LogicPortSpriteType.Input, true),
    new LogicPorts.Port(LogicMemory.RESET_PORT_ID, new CellOffset(1, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.RESET_PORT_INACTIVE, true, LogicPortSpriteType.ResetUpdate, true)
  };
  private static readonly LogicPorts.Port[] OUTPUT_PORTS = new LogicPorts.Port[1]
  {
    new LogicPorts.Port(LogicMemory.READ_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICMEMORY.READ_PORT_INACTIVE, true, LogicPortSpriteType.Output, true)
  };

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicMemoryConfig.ID;
    int width = 2;
    int height = 2;
    string anim = "logic_memory_kanim";
    int hitpoints = 10;
    float construction_time = 30f;
    float[] tieR0 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR0, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Deprecated = false;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.ObjectLayer = ObjectLayer.LogicGates;
    SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("logic_memory_kanim", "PowerMemory_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicMemoryConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicMemoryConfig.INPUT_PORTS, LogicMemoryConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicMemoryConfig.INPUT_PORTS, LogicMemoryConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    go.AddOrGet<LogicMemory>();
    GeneratedBuildings.RegisterLogicPorts(go, LogicMemoryConfig.INPUT_PORTS, LogicMemoryConfig.OUTPUT_PORTS);
  }
}
