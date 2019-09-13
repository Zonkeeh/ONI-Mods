// Decompiled with JetBrains decompiler
// Type: GlassTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GlassTileConfig : IBuildingConfig
{
  public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_glass_tops");
  public const string ID = "GlassTile";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GlassTile";
    int width = 1;
    int height = 1;
    string anim = "floor_glass_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] transparents = MATERIALS.TRANSPARENTS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, transparents, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.GlassTile;
    buildingDef.isKAnimTile = true;
    buildingDef.isSolidTile = true;
    buildingDef.BlockTileIsTransparent = true;
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_glass");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_glass_place");
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.setTransparent = true;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = GlassTileConfig.BlockTileConnectorID;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.GetComponent<KPrefabID>().AddTag(GameTags.Window, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
