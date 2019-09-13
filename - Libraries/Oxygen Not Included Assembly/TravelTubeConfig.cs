// Decompiled with JetBrains decompiler
// Type: TravelTubeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TravelTubeConfig : IBuildingConfig
{
  public const string ID = "TravelTube";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "TravelTube";
    int width = 1;
    int height = 1;
    string anim = "travel_tube_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] plastics = MATERIALS.PLASTICS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.TileLayer = ObjectLayer.TravelTubeTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementTravelTube;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = 0.0f;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.isKAnimTile = true;
    buildingDef.isUtility = true;
    buildingDef.DragBuild = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<TravelTube>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
    graphTileVisualizer.isPhysicalBuilding = false;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Tube;
    graphTileVisualizer.isPhysicalBuilding = true;
  }
}
