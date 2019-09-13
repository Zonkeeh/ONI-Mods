// Decompiled with JetBrains decompiler
// Type: TemperatureControlledSwitchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TemperatureControlledSwitchConfig : IBuildingConfig
{
  public static string ID = "TemperatureControlledSwitch";

  public override BuildingDef CreateBuildingDef()
  {
    string id = TemperatureControlledSwitchConfig.ID;
    int width = 1;
    int height = 1;
    string anim = "switchthermal_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.Deprecated = true;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("switchthermal_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    TemperatureControlledSwitch controlledSwitch = go.AddOrGet<TemperatureControlledSwitch>();
    controlledSwitch.objectLayer = ObjectLayer.Wire;
    controlledSwitch.manuallyControlled = false;
    controlledSwitch.minTemp = 0.0f;
    controlledSwitch.maxTemp = 573.15f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<BuildingCellVisualizer>();
  }
}
