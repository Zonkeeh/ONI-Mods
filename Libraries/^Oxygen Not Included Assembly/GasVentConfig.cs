// Decompiled with JetBrains decompiler
// Type: GasVentConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasVentConfig : IBuildingConfig
{
  public const string ID = "GasVent";
  public const float OVERPRESSURE_MASS = 2f;
  private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GasVent";
    int width = 1;
    int height = 1;
    string anim = "ventgas_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
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
    vent.overpressureMass = 2f;
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
