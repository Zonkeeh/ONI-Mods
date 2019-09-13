// Decompiled with JetBrains decompiler
// Type: MassageTableConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class MassageTableConfig : IBuildingConfig
{
  public const string ID = "MassageTable";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MassageTable";
    int width = 2;
    int height = 2;
    string anim = "masseur_kanim";
    int hitpoints = 10;
    float construction_time = 10f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR0, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.Overheatable = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.AudioCategory = "Metal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.MassageTable, false);
    MassageTable massageTable = go.AddOrGet<MassageTable>();
    massageTable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_masseur_kanim")
    };
    massageTable.stressModificationValue = -30f;
    massageTable.roomStressModificationValue = -60f;
    massageTable.workLayer = Grid.SceneLayer.BuildingFront;
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.MassageTable.Id;
    ownable.canBePublic = true;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.MassageClinic.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KAnimControllerBase>().initialAnim = "off";
    go.AddOrGet<CopyBuildingSettings>();
  }
}
