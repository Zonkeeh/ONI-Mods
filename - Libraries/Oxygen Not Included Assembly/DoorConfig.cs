// Decompiled with JetBrains decompiler
// Type: DoorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class DoorConfig : IBuildingConfig
{
  public static readonly LogicPorts.Port[] INPUT_PORTS_0_0 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(Door.OPEN_CLOSE_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN, (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE, false, false)
  };
  public static readonly LogicPorts.Port[] INPUT_PORTS_N1_0 = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(Door.OPEN_CLOSE_PORT_ID, new CellOffset(-1, 0), (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN, (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.DOOR.LOGIC_OPEN_INACTIVE, false, false)
  };
  public const string ID = "Door";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Door";
    int width = 1;
    int height = 2;
    string anim = "door_internal_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
    buildingDef.Entombable = true;
    buildingDef.Floodable = false;
    buildingDef.IsFoundation = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER2);
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", TUNING.NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Door door = go.AddOrGet<Door>();
    door.unpoweredAnimSpeed = 1f;
    door.doorType = Door.DoorType.Internal;
    door.doorOpeningSoundEventName = "Open_DoorInternal";
    door.doorClosingSoundEventName = "Close_DoorInternal";
    go.AddOrGet<AccessControl>().controlEnabled = true;
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
    go.AddOrGet<Workable>().workTime = 3f;
    go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
    go.AddOrGet<ZoneTile>();
    go.AddOrGet<KBoxCollider2D>();
    Prioritizable.AddRef(go);
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
  }
}
