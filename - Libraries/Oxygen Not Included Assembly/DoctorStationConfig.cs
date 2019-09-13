// Decompiled with JetBrains decompiler
// Type: DoctorStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class DoctorStationConfig : IBuildingConfig
{
  private static Tag SUPPLY_TAG = (Tag) "IntermediateCure";
  public const string ID = "DoctorStation";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "DoctorStation";
    int width = 3;
    int height = 2;
    string anim = "treatment_chair_kanim";
    int hitpoints = 10;
    float construction_time = 10f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Clinic, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = DoctorStationConfig.SUPPLY_TAG;
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
    DoctorStation doctorStation = go.AddOrGet<DoctorStation>();
    doctorStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_sick_kanim")
    };
    doctorStation.workLayer = Grid.SceneLayer.BuildingFront;
    doctorStation.supplyTag = DoctorStationConfig.SUPPLY_TAG;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
    roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
    roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
    DoctorStationDoctorWorkable stationDoctorWorkable = go.AddOrGet<DoctorStationDoctorWorkable>();
    stationDoctorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_treatment_chair_doctor_kanim")
    };
    stationDoctorWorkable.SetWorkTime(40f);
    stationDoctorWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanDoctor.Id;
  }
}
