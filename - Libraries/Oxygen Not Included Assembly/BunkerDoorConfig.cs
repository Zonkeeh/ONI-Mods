// Decompiled with JetBrains decompiler
// Type: BunkerDoorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BunkerDoorConfig : IBuildingConfig
{
  public const string ID = "BunkerDoor";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "BunkerDoor";
    int width = 4;
    int height = 1;
    string anim = "door_bunker_kanim";
    int hitpoints = 1000;
    float construction_time = 120f;
    float[] construction_mass = new float[1]{ 500f };
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.OverheatTemperature = 1273.15f;
    buildingDef.Entombable = false;
    buildingDef.IsFoundation = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
    buildingDef.TileLayer = ObjectLayer.FoundationTile;
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
    SoundEventVolumeCache.instance.AddVolume("door_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_N1_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_N1_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_N1_0);
    Door door = go.AddOrGet<Door>();
    door.unpoweredAnimSpeed = 0.01f;
    door.poweredAnimSpeed = 0.1f;
    door.hasComplexUserControls = true;
    door.allowAutoControl = false;
    door.doorOpeningSoundEventName = "BunkerDoor_opening";
    door.doorClosingSoundEventName = "BunkerDoor_closing";
    door.verticalOrientation = Orientation.R90;
    go.AddOrGet<Workable>().workTime = 3f;
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    component.initialAnim = "closed";
    component.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    go.AddOrGet<ZoneTile>();
    go.AddOrGet<KBoxCollider2D>();
    Prioritizable.AddRef(go);
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    go.GetComponent<KPrefabID>().AddTag(GameTags.Bunker, false);
  }
}
