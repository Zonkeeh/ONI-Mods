// Decompiled with JetBrains decompiler
// Type: SolarPanelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class SolarPanelConfig : IBuildingConfig
{
  public const string ID = "SolarPanel";
  public const float WATTS_PER_LUX = 0.00053f;
  public const float MAX_WATTS = 380f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SolarPanel";
    int width = 7;
    int height = 3;
    string anim = "solar_panel_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] glasses = MATERIALS.GLASSES;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, glasses, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tieR5, 0.2f);
    buildingDef.GeneratorWattageRating = 380f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.BuildLocationRule = BuildLocationRule.Anywhere;
    buildingDef.HitPoints = 10;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    Tinkerable.MakePowerTinkerable(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
    go.AddOrGet<SolarPanel>().powerDistributionOrder = 9;
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
