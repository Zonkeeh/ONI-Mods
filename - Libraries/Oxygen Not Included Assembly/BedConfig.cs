// Decompiled with JetBrains decompiler
// Type: BedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BedConfig : IBuildingConfig
{
  public static string ID = "Bed";

  public override BuildingDef CreateBuildingDef()
  {
    string id = BedConfig.ID;
    int width = 2;
    int height = 2;
    string anim = "bedlg_kanim";
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
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Bed, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    Bed bed = go.AddOrGet<Bed>();
    bed.effects = new string[2]{ "BedStamina", "BedHealth" };
    bed.workLayer = Grid.SceneLayer.BuildingFront;
    Sleepable sleepable = go.AddOrGet<Sleepable>();
    sleepable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_sleep_bed_kanim")
    };
    sleepable.workLayer = Grid.SceneLayer.BuildingFront;
    go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
  }
}
