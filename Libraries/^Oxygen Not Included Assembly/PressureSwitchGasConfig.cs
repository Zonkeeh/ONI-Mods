// Decompiled with JetBrains decompiler
// Type: PressureSwitchGasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PressureSwitchGasConfig : IBuildingConfig
{
  public static string ID = "PressureSwitchGas";

  public override BuildingDef CreateBuildingDef()
  {
    string id = PressureSwitchGasConfig.ID;
    int width = 1;
    int height = 1;
    string anim = "switchgaspressure_kanim";
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
    buildingDef.Floodable = true;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    PressureSwitch pressureSwitch = go.AddOrGet<PressureSwitch>();
    pressureSwitch.objectLayer = ObjectLayer.Wire;
    pressureSwitch.rangeMin = 0.0f;
    pressureSwitch.rangeMax = 2f;
    pressureSwitch.Threshold = 1f;
    pressureSwitch.ActivateAboveThreshold = false;
    pressureSwitch.manuallyControlled = false;
    pressureSwitch.desiredState = Element.State.Gas;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<BuildingCellVisualizer>();
  }
}
