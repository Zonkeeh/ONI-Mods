// Decompiled with JetBrains decompiler
// Type: SolidConduitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolidConduitConfig : IBuildingConfig
{
  public const string ID = "SolidConduit";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SolidConduit";
    int width = 1;
    int height = 1;
    string anim = "utilities_conveyor_kanim";
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.ObjectLayer = ObjectLayer.SolidConduit;
    buildingDef.TileLayer = ObjectLayer.SolidConduitTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementSolidConduit;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = 0.0f;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.SceneLayer = Grid.SceneLayer.SolidConduits;
    buildingDef.isKAnimTile = true;
    buildingDef.isUtility = true;
    buildingDef.DragBuild = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduit");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<SolidConduit>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Solid;
    graphTileVisualizer.isPhysicalBuilding = false;
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<Building>().Def.BuildingUnderConstruction.GetComponent<Constructable>().isDiggingRequired = false;
    KAnimGraphTileVisualizer graphTileVisualizer = go.AddComponent<KAnimGraphTileVisualizer>();
    graphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Solid;
    graphTileVisualizer.isPhysicalBuilding = true;
    LiquidConduitConfig.CommonConduitPostConfigureComplete(go);
  }
}
