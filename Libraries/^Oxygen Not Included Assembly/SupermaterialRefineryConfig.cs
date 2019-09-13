// Decompiled with JetBrains decompiler
// Type: SupermaterialRefineryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SupermaterialRefineryConfig : IBuildingConfig
{
  public const string ID = "SupermaterialRefinery";
  private const float INPUT_KG = 100f;
  private const float OUTPUT_KG = 100f;
  private const float OUTPUT_TEMPERATURE = 313.15f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SupermaterialRefinery";
    int width = 4;
    int height = 5;
    string anim = "supermaterial_refinery_kanim";
    int hitpoints = 30;
    float construction_time = 480f;
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR6 = TUNING.NOISE_POLLUTION.NOISY.TIER6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tieR6, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 1600f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.resultState = ComplexFabricator.ResultState.Heated;
    fabricator.heatedTemperature = 313.15f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_supermaterial_refinery_kanim")
    };
    Prioritizable.AddRef(go);
    float num1 = 0.01f;
    float num2 = (float) ((1.0 - (double) num1) * 0.5);
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f * num1),
      new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 100f * num2),
      new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * num2)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SuperCoolant.CreateTag(), 100f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("SupermaterialRefinery")
    };
    float num3 = 0.15f;
    float num4 = 0.05f;
    float num5 = 1f - num4 - num3;
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num3),
      new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num5),
      new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 100f * num4)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("SupermaterialRefinery")
    };
    float num6 = 0.05f;
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 100f * num6),
      new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), (float) (100.0 * (1.0 - (double) num6)))
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("SupermaterialRefinery")
    };
    float num7 = 0.35f;
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num7),
      new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), (float) (100.0 * (1.0 - (double) num7)))
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.ViscoGel.CreateTag(), 100f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = 80f,
      description = (string) STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.VISCOGEL_RECIPE_DESCRIPTION,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("SupermaterialRefinery")
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    });
  }
}
