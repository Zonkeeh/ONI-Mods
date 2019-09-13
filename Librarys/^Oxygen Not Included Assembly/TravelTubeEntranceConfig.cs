// Decompiled with JetBrains decompiler
// Type: TravelTubeEntranceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class TravelTubeEntranceConfig : IBuildingConfig
{
  public const string ID = "TravelTubeEntrance";
  private const float JOULES_PER_LAUNCH = 10000f;
  private const float LAUNCHES_FROM_FULL_CHARGE = 4f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "TravelTubeEntrance";
    int width = 3;
    int height = 2;
    string anim = "tube_launcher_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    TravelTubeEntrance travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
    travelTubeEntrance.joulesPerLaunch = 10000f;
    travelTubeEntrance.jouleCapacity = 40000f;
    go.AddOrGet<TravelTubeEntrance.Work>();
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<EnergyConsumerSelfSustaining>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().visualizeRequirements = false;
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_1_1);
  }
}
