// Decompiled with JetBrains decompiler
// Type: MedicalCotConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MedicalCotConfig : IBuildingConfig
{
  public const string ID = "MedicalCot";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MedicalCot";
    int width = 3;
    int height = 2;
    string anim = "medical_cot_kanim";
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
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    go.GetComponent<KPrefabID>().AddTag(TagManager.Create("Bed"), false);
    Clinic clinic = go.AddOrGet<Clinic>();
    clinic.doctorVisitInterval = 300f;
    clinic.workerInjuredAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_healing_bed_kanim")
    };
    clinic.workerDiseasedAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_med_cot_sick_kanim")
    };
    clinic.workLayer = Grid.SceneLayer.BuildingFront;
    string str1 = "MedicalCot";
    string str2 = "MedicalCotDoctored";
    clinic.healthEffect = str1;
    clinic.doctoredHealthEffect = str2;
    clinic.diseaseEffect = str1;
    clinic.doctoredDiseaseEffect = str2;
    clinic.doctoredPlaceholderEffect = "DoctoredOffCotEffect";
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Hospital.Id;
    roomTracker.requirement = RoomTracker.Requirement.CustomRecommended;
    roomTracker.customStatusItemID = Db.Get().BuildingStatusItems.ClinicOutsideHospital.Id;
    go.AddOrGet<Sleepable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_med_cot_sick_kanim")
    };
    DoctorChoreWorkable doctorChoreWorkable = go.AddOrGet<DoctorChoreWorkable>();
    doctorChoreWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_med_cot_doctor_kanim")
    };
    doctorChoreWorkable.workTime = 45f;
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Clinic.Id;
  }
}
