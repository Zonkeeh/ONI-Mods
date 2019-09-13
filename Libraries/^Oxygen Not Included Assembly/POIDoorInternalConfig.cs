// Decompiled with JetBrains decompiler
// Type: POIDoorInternalConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class POIDoorInternalConfig : IBuildingConfig
{
  public static string ID = "POIDoorInternal";

  public override BuildingDef CreateBuildingDef()
  {
    string id = POIDoorInternalConfig.ID;
    int width = 1;
    int height = 2;
    string anim = "door_poi_internal_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
    buildingDef.ShowInBuildMenu = false;
    buildingDef.Entombable = false;
    buildingDef.Floodable = false;
    buildingDef.Invincible = true;
    buildingDef.IsFoundation = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ForegroundLayer = Grid.SceneLayer.InteriorWall;
    SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Open_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
    SoundEventVolumeCache.instance.AddVolume("door_poi_internal_kanim", "Close_DoorInternal", NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Door door = go.AddOrGet<Door>();
    door.unpoweredAnimSpeed = 1f;
    door.doorType = Door.DoorType.Internal;
    go.AddOrGet<ZoneTile>();
    go.AddOrGet<AccessControl>();
    go.AddOrGet<Workable>().workTime = 3f;
    go.AddOrGet<KBoxCollider2D>();
    Prioritizable.AddRef(go);
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
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
    GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS_0_0);
    AccessControl component = go.GetComponent<AccessControl>();
    go.GetComponent<Door>().hasComplexUserControls = false;
    component.controlEnabled = false;
    go.GetComponent<Deconstructable>().allowDeconstruction = true;
    go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
  }
}
