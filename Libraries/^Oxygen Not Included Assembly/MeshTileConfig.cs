// Decompiled with JetBrains decompiler
// Type: MeshTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MeshTileConfig : IBuildingConfig
{
  public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_mesh_tops");
  public const string ID = "MeshTile";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MeshTile";
    int width = 1;
    int height = 1;
    string anim = "floor_mesh_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.isKAnimTile = true;
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_mesh");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_mesh_place");
    buildingDef.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_mesh_spec");
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<SimCellOccupier>().doReplaceElement = false;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
    go.AddComponent<SimTemperatureTransfer>();
    go.AddComponent<ZoneTile>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
