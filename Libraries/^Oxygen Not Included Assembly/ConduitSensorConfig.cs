// Decompiled with JetBrains decompiler
// Type: ConduitSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public abstract class ConduitSensorConfig : IBuildingConfig
{
  protected abstract ConduitType ConduitType { get; }

  protected BuildingDef CreateBuildingDef(
    string ID,
    string anim,
    float[] required_mass,
    string[] required_materials)
  {
    string id = ID;
    int width = 1;
    int height = 1;
    string anim1 = anim;
    int hitpoints = 30;
    float construction_time = 30f;
    float[] construction_mass = required_mass;
    string[] construction_materials = required_materials;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim1, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume(anim, "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }
}
