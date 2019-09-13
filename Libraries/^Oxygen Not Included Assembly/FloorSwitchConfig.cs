// Decompiled with JetBrains decompiler
// Type: FloorSwitchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FloorSwitchConfig : IBuildingConfig
{
  public const string ID = "FloorSwitch";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FloorSwitch";
    int width = 1;
    int height = 1;
    string anim = "pressureswitch_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Tile;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.IsFoundation = true;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.TileLayer = ObjectLayer.FoundationTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "FloorSwitch");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT.BONUS_2;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
    LogicMassSensor logicMassSensor = go.AddOrGet<LogicMassSensor>();
    logicMassSensor.rangeMin = 0.0f;
    logicMassSensor.rangeMax = 2000f;
    logicMassSensor.Threshold = 10f;
    logicMassSensor.ActivateAboveThreshold = true;
    logicMassSensor.manuallyControlled = false;
  }
}
