// Decompiled with JetBrains decompiler
// Type: ResetSkillsStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class ResetSkillsStationConfig : IBuildingConfig
{
  public const string ID = "ResetSkillsStation";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "ResetSkillsStation";
    int width = 3;
    int height = 3;
    string anim = "reSpeccer_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR1, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddTag(GameTags.NotRoomAssignable);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.ResetSkillsStation.Id;
    ResetSkillsStation resetSkillsStation = go.AddOrGet<ResetSkillsStation>();
    resetSkillsStation.workTime = 180f;
    resetSkillsStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_reSpeccer_kanim")
    };
    resetSkillsStation.workLayer = Grid.SceneLayer.BuildingFront;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
