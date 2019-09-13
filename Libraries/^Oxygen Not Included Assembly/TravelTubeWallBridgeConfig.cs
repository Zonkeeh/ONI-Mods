// Decompiled with JetBrains decompiler
// Type: TravelTubeWallBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TravelTubeWallBridgeConfig : IBuildingConfig
{
  public const string ID = "TravelTubeWallBridge";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "TravelTubeWallBridge";
    int width = 1;
    int height = 1;
    string anim = "tube_tile_bridge_kanim";
    int hitpoints = 100;
    float construction_time = 3f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] plastics = MATERIALS.PLASTICS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.PENALTY_3;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<TravelTubeBridge>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
    go.AddOrGet<KPrefabID>().AddTag(GameTags.TravelTubeBridges, false);
  }

  protected virtual TravelTubeUtilityNetworkLink AddNetworkLink(
    GameObject go)
  {
    TravelTubeUtilityNetworkLink utilityNetworkLink = go.AddOrGet<TravelTubeUtilityNetworkLink>();
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
