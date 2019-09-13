// Decompiled with JetBrains decompiler
// Type: CosmicResearchCenterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CosmicResearchCenterConfig : IBuildingConfig
{
  public static readonly Tag INPUT_MATERIAL = ResearchDatabankConfig.TAG;
  public const string ID = "CosmicResearchCenter";
  public const float BASE_RESEARCH_SPEED = 0.02f;
  public const float MASS_PER_POINT = 1f;
  public const float CAPACITY = 300f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "CosmicResearchCenter";
    int width = 4;
    int height = 4;
    string anim = "research_space_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR1, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.requestedItemTag = CosmicResearchCenterConfig.INPUT_MATERIAL;
    manualDeliveryKg.refillMass = 3f;
    manualDeliveryKg.capacity = 300f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
    ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
    researchCenter.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_research_space_kanim")
    };
    researchCenter.research_point_type_id = "gamma";
    researchCenter.inputMaterial = CosmicResearchCenterConfig.INPUT_MATERIAL;
    researchCenter.mass_per_point = 1f;
    researchCenter.requiredSkillPerk = Db.Get().SkillPerks.AllowAdvancedResearch.Id;
    researchCenter.workLayer = Grid.SceneLayer.BuildingFront;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(CosmicResearchCenterConfig.INPUT_MATERIAL, 0.02f)
    };
    elementConverter.showDescriptors = false;
    go.AddOrGetDef<PoweredController.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
