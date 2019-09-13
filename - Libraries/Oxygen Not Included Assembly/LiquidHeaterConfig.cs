// Decompiled with JetBrains decompiler
// Type: LiquidHeaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidHeaterConfig : IBuildingConfig
{
  public const string ID = "LiquidHeater";
  public const float CONSUMPTION_RATE = 1f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidHeater";
    int width = 4;
    int height = 1;
    string anim = "boiler_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 3200f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.Floodable = false;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.ExhaustKilowattsWhenActive = 4000f;
    buildingDef.SelfHeatKilowattsWhenActive = 64f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "SolidMetal";
    buildingDef.OverheatTemperature = 398.15f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
    spaceHeater.SetLiquidHeater();
    spaceHeater.targetTemperature = 358.15f;
    spaceHeater.minimumCellMass = 400f;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_0);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
