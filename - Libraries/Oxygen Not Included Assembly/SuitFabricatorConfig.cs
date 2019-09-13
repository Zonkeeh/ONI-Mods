// Decompiled with JetBrains decompiler
// Type: SuitFabricatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SuitFabricatorConfig : IBuildingConfig
{
  public const string ID = "SuitFabricator";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SuitFabricator";
    int width = 4;
    int height = 3;
    string anim = "suit_maker_kanim";
    int hitpoints = 100;
    float construction_time = 240f;
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3 = TUNING.NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tieR3, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<Prioritizable>();
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.resultState = ComplexFabricator.ResultState.Heated;
    fabricator.heatedTemperature = 318.15f;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_suit_fabricator_kanim")
    };
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfigureRecipes();
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Copper.CreateTag(), 300f),
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f)
    };
    AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.suitsOverlay.parentTech.Id
    };
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 300f),
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f)
    };
    AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.suitsOverlay.parentTech.Id
    };
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Iron.CreateTag(), 300f),
      new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 2f)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Atmo_Suit".ToTag(), 1f)
    };
    AtmoSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.suitsOverlay.parentTech.Id
    };
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) SimHashes.Steel.ToString(), 200f),
      new ComplexRecipe.RecipeElement((Tag) SimHashes.Petroleum.ToString(), 25f)
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("Jet_Suit".ToTag(), 1f)
    };
    JetSuitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SuitFabricator", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_FABTIME,
      description = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
      fabricators = new List<Tag>()
      {
        (Tag) "SuitFabricator"
      },
      requiredTech = Db.Get().TechItems.jetSuit.parentTech.Id
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true));
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    });
  }
}
