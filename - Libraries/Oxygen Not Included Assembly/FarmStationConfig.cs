// Decompiled with JetBrains decompiler
// Type: FarmStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class FarmStationConfig : IBuildingConfig
{
  public static Tag MATERIAL_FOR_TINKER = GameTags.Fertilizer;
  public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;
  public const string ID = "FarmStation";
  public const float MASS_PER_TINKER = 5f;
  public const float OUTPUT_TEMPERATURE = 308.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FarmStation";
    int width = 2;
    int height = 3;
    string anim = "planttender_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR1, 0.2f);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FarmStation, false);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
    go.AddOrGet<LogicOperationalController>();
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = FarmStationConfig.MATERIAL_FOR_TINKER;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.capacity = 50f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
    tinkerStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_planttender_kanim")
    };
    tinkerStation.inputMaterial = FarmStationConfig.MATERIAL_FOR_TINKER;
    tinkerStation.massPerTinker = 5f;
    tinkerStation.outputPrefab = FarmStationConfig.TINKER_TOOLS;
    tinkerStation.outputTemperature = 308.15f;
    tinkerStation.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
    tinkerStation.choreType = Db.Get().ChoreTypes.FarmingFabricate.IdHash;
    tinkerStation.fetchChoreType = Db.Get().ChoreTypes.FarmFetch.IdHash;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object =>
    {
      TinkerStation component = game_object.GetComponent<TinkerStation>();
      component.AttributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
      component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    });
  }
}
