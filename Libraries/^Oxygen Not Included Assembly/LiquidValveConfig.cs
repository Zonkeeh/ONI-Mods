// Decompiled with JetBrains decompiler
// Type: LiquidValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidValveConfig : IBuildingConfig
{
  public const string ID = "LiquidValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidValve";
    int width = 1;
    int height = 2;
    string anim = "valveliquid_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, tieR1, 0.2f);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    ValveBase valveBase = go.AddOrGet<ValveBase>();
    valveBase.conduitType = ConduitType.Liquid;
    valveBase.maxFlow = 10f;
    valveBase.animFlowRanges = new ValveBase.AnimRangeInfo[3]
    {
      new ValveBase.AnimRangeInfo(3f, "lo"),
      new ValveBase.AnimRangeInfo(7f, "med"),
      new ValveBase.AnimRangeInfo(10f, "hi")
    };
    go.AddOrGet<Valve>();
    go.AddOrGet<Workable>().workTime = 5f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<RequireInputs>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
  }
}
