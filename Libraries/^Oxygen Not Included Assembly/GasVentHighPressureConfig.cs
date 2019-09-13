// Decompiled with JetBrains decompiler
// Type: GasVentHighPressureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasVentHighPressureConfig : IBuildingConfig
{
  public const string ID = "GasVentHighPressure";
  private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
  public const float OVERPRESSURE_MASS = 20f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GasVentHighPressure";
    int width = 1;
    int height = 1;
    string anim = "ventgas_powered_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "Plastic"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, construction_materials, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    SoundEventVolumeCache.instance.AddVolume("ventgas_kanim", "GasVent_clunk", NOISE_POLLUTION.NOISY.TIER0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<Exhaust>();
    Vent vent = go.AddOrGet<Vent>();
    vent.conduitType = ConduitType.Gas;
    vent.endpointType = Endpoint.Sink;
    vent.overpressureMass = 20f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.ignoreMinMassCheck = true;
    BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
    go.AddOrGet<SimpleVent>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<VentController.Def>();
  }
}
