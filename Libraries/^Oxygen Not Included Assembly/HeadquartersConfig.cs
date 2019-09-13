// Decompiled with JetBrains decompiler
// Type: HeadquartersConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class HeadquartersConfig : IBuildingConfig
{
  public const string ID = "Headquarters";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Headquarters";
    int width = 4;
    int height = 4;
    string anim = "hqbase_kanim";
    int hitpoints = 250;
    float construction_time = 30f;
    float[] tieR7 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR7, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = 400f;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.DefaultAnimState = "idle";
    SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_LP", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_open", NOISE_POLLUTION.NOISY.TIER4);
    SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_close", NOISE_POLLUTION.NOISY.TIER4);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoreBearer>();
    go.AddOrGet<Telepad>().startingSkillPoints = 1f;
    SocialGatheringPoint socialGatheringPoint = go.AddOrGet<SocialGatheringPoint>();
    socialGatheringPoint.choreOffsets = new CellOffset[6]
    {
      new CellOffset(-1, 0),
      new CellOffset(-2, 0),
      new CellOffset(2, 0),
      new CellOffset(3, 0),
      new CellOffset(0, 0),
      new CellOffset(1, 0)
    };
    socialGatheringPoint.choreCount = 4;
    socialGatheringPoint.basePriority = RELAXATION.PRIORITY.TIER0;
    Light2D light2D = go.AddOrGet<Light2D>();
    light2D.Color = LIGHT2D.HEADQUARTERS_COLOR;
    light2D.Range = 5f;
    light2D.Offset = LIGHT2D.HEADQUARTERS_OFFSET;
    light2D.overlayColour = LIGHT2D.HEADQUARTERS_OVERLAYCOLOR;
    light2D.shape = LightShape.Circle;
    light2D.drawOverlay = true;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
    go.GetComponent<KPrefabID>().AddTag(GameTags.Experimental, false);
    RoleStation roleStation = go.AddOrGet<RoleStation>();
    roleStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_hqbase_skill_upgrade_kanim")
    };
    roleStation.workAnims = new HashedString[1]
    {
      (HashedString) "upgrade"
    };
    roleStation.workingPstComplete = HashedString.Invalid;
    roleStation.workingPstFailed = HashedString.Invalid;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
