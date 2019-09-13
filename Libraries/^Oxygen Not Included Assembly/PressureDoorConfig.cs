// Decompiled with JetBrains decompiler
// Type: PressureDoorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PressureDoorConfig : IBuildingConfig
{
  public const string ID = "PressureDoor";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "PressureDoor";
    int width = 1;
    int height = 2;
    string anim = "door_external_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 1f);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.IsFoundation = true;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.TileLayer = ObjectLayer.FoundationTile;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
    SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
    SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
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
    door.hasComplexUserControls = true;
    door.unpoweredAnimSpeed = 0.65f;
    door.poweredAnimSpeed = 5f;
    door.doorClosingSoundEventName = "MechanizedAirlock_closing";
    door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
    go.AddOrGet<ZoneTile>();
    go.AddOrGet<AccessControl>();
    go.AddOrGet<KBoxCollider2D>();
    Prioritizable.AddRef(go);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Door;
    go.AddOrGet<Workable>().workTime = 5f;
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    go.GetComponent<AccessControl>().controlEnabled = true;
    go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
  }
}
