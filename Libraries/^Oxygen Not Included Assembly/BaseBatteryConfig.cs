// Decompiled with JetBrains decompiler
// Type: BaseBatteryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public abstract class BaseBatteryConfig : IBuildingConfig
{
  public BuildingDef CreateBuildingDef(
    string id,
    int width,
    int height,
    int hitpoints,
    string anim,
    float construction_time,
    float[] construction_mass,
    string[] construction_materials,
    float melting_point,
    float exhaust_temperature_active,
    float self_heat_kilowatts_active,
    EffectorValues decor,
    EffectorValues noise)
  {
    string id1 = id;
    int width1 = width;
    int height1 = height;
    int hitpoints1 = hitpoints;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id1, width1, height1, anim, hitpoints1, construction_time, construction_mass, construction_materials, melting_point, BuildLocationRule.OnFloor, decor, tieR0, 0.2f);
    buildingDef.ExhaustKilowattsWhenActive = exhaust_temperature_active;
    buildingDef.SelfHeatKilowattsWhenActive = self_heat_kilowatts_active;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerOutput = true;
    buildingDef.UseWhitePowerOutputConnectorColour = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddComponent<RequireInputs>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Battery>().powerSortOrder = 1000;
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
