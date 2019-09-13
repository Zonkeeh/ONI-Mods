// Decompiled with JetBrains decompiler
// Type: CometDetectorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CometDetectorConfig : IBuildingConfig
{
  public static string ID = "CometDetector";

  public override BuildingDef CreateBuildingDef()
  {
    string id = CometDetectorConfig.ID;
    int width = 2;
    int height = 4;
    string anim = "meteor_detector_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR0, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, CometDetectorConfig.ID);
    return buildingDef;
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
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
    go.AddOrGetDef<CometDetector.Def>();
  }
}
