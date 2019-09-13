// Decompiled with JetBrains decompiler
// Type: ShearingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

public class ShearingStationConfig : IBuildingConfig
{
  public const string ID = "ShearingStation";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "ShearingStation";
    int width = 3;
    int height = 3;
    string anim = "shearing_station_kanim";
    int hitpoints = 100;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.ShowInBuildMenu = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStation, false);
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.isCreatureEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) =>
    {
      ScaleGrowthMonitor.Instance smi = creature_go.GetSMI<ScaleGrowthMonitor.Instance>();
      if (smi == null)
        return false;
      return smi.IsFullyGrown();
    });
    def.onRanchCompleteCb = (System.Action<GameObject>) (creature_go => creature_go.GetSMI<ScaleGrowthMonitor.Instance>().Shear());
    def.interactLoopCount = 6;
    def.rancherInteractAnim = (HashedString) "anim_interacts_shearingstation_kanim";
    def.synchronizeBuilding = true;
    Prioritizable.AddRef(go);
  }
}
