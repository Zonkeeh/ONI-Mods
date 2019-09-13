// Decompiled with JetBrains decompiler
// Type: TilePOIConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TilePOIConfig : IBuildingConfig
{
  public static string ID = "TilePOI";

  public override BuildingDef CreateBuildingDef()
  {
    string id = TilePOIConfig.ID;
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
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.ShowInBuildMenu = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.Repairable = false;
    buildingDef.Replaceable = false;
    buildingDef.Invincible = true;
    buildingDef.IsFoundation = true;
    buildingDef.UseStructureTemperature = false;
    buildingDef.TileLayer = ObjectLayer.FoundationTile;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.isKAnimTile = true;
    buildingDef.isSolidTile = true;
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_POI");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_POI");
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_POI_tops_decor_info");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<SimCellOccupier>().doReplaceElement = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Bunker, false);
    go.AddComponent<SimTemperatureTransfer>();
    go.GetComponent<Deconstructable>().allowDeconstruction = true;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
