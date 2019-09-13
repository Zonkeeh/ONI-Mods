// Decompiled with JetBrains decompiler
// Type: FarmTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FarmTileConfig : IBuildingConfig
{
  public const string ID = "FarmTile";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FarmTile";
    int width = 1;
    int height = 1;
    string anim = "farmtilerotating_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] farmable = MATERIALS.FARMABLE;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.PermittedRotations = PermittedRotations.FlipV;
    buildingDef.isSolidTile = false;
    buildingDef.DragBuild = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.AddDepositTag(GameTags.WaterSeed);
    plantablePlot.SetFertilizationFlags(true, false);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles, false);
    FarmTileConfig.SetUpFarmPlotTags(go);
  }

  public static void SetUpFarmPlotTags(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (inst =>
    {
      Rotatable component1 = inst.GetComponent<Rotatable>();
      PlantablePlot component2 = inst.GetComponent<PlantablePlot>();
      switch (component1.GetOrientation())
      {
        case Orientation.Neutral:
        case Orientation.FlipH:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
          break;
        case Orientation.R90:
        case Orientation.R270:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Side);
          break;
        case Orientation.R180:
        case Orientation.FlipV:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Bottom);
          break;
      }
    });
  }
}
