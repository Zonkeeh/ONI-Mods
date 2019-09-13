// Decompiled with JetBrains decompiler
// Type: RockCrusherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class RockCrusherConfig : IBuildingConfig
{
  public const string ID = "RockCrusher";
  private const float INPUT_KG = 100f;
  private const float METAL_ORE_EFFICIENCY = 0.5f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "RockCrusher";
    int width = 4;
    int height = 4;
    string anim = "rockrefinery_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR6 = TUNING.NOISE_POLLUTION.NOISY.TIER6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tieR6, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
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
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_rockrefinery_kanim")
    };
    fabricatorWorkable.workingPstComplete = (HashedString) "working_pst_complete";
    Tag tag = SimHashes.Sand.CreateTag();
    foreach (Element element in ElementLoader.elements.FindAll((Predicate<Element>) (e => e.HasTag(GameTags.Crushable))))
    {
      ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(element.tag, 100f)
      };
      ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
      {
        new ComplexRecipe.RecipeElement(tag, 100f)
      };
      string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", element.tag);
      string str = ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results);
      new ComplexRecipe(str, ingredients, results)
      {
        time = 40f,
        description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) element.name, (object) tag.ProperName()),
        nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
      }.fabricators = new List<Tag>()
      {
        TagManager.Create("RockCrusher")
      };
      ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
    }
    foreach (Element element in ElementLoader.elements.FindAll((Predicate<Element>) (e =>
    {
      if (e.IsSolid)
        return e.HasTag(GameTags.Metal);
      return false;
    })))
    {
      Element lowTempTransition = element.highTempTransition.lowTempTransition;
      if (lowTempTransition != element)
      {
        ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1]
        {
          new ComplexRecipe.RecipeElement(element.tag, 100f)
        };
        ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[2]
        {
          new ComplexRecipe.RecipeElement(lowTempTransition.tag, 50f),
          new ComplexRecipe.RecipeElement(tag, 50f)
        };
        string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", lowTempTransition.tag);
        string str = ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results);
        new ComplexRecipe(str, ingredients, results)
        {
          time = 40f,
          description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.METAL_RECIPE_DESCRIPTION, (object) lowTempTransition.name, (object) element.name),
          nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
        }.fabricators = new List<Tag>()
        {
          TagManager.Create("RockCrusher")
        };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
      }
    }
    Element elementByHash1 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "EggShell", 5f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f)
    };
    string obsolete_id1 = ComplexRecipeManager.MakeObsoleteRecipeID("RockCrusher", elementByHash1.tag);
    string str1 = ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1);
    new ComplexRecipe(str1, ingredients1, results1)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) MISC.TAGS.EGGSHELL),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("RockCrusher")
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id1, str1);
    Element elementByHash2 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "BabyCrabShell", 1f)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(elementByHash2.tag, 5f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("RockCrusher")
    };
    Element elementByHash3 = ElementLoader.FindElementByHash(SimHashes.Lime);
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CrabShell", 1f)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(elementByHash3.tag, 10f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_RECIPE_DESCRIPTION, (object) SimHashes.Lime.CreateTag().ProperName(), (object) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("RockCrusher")
    };
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Fossil).tag, 100f)
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 5f),
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag, 95f)
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION, (object) SimHashes.Fossil.CreateTag().ProperName(), (object) SimHashes.SedimentaryRock.CreateTag().ProperName(), (object) SimHashes.Lime.CreateTag().ProperName()),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("RockCrusher")
    };
    float num = 5E-05f;
    ComplexRecipe.RecipeElement[] ingredients5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Salt.CreateTag(), 100f)
    };
    ComplexRecipe.RecipeElement[] results5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(TableSaltConfig.ID.ToTag(), 100f * num),
      new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), (float) (100.0 * (1.0 - (double) num)))
    };
    new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("RockCrusher", (IList<ComplexRecipe.RecipeElement>) ingredients5, (IList<ComplexRecipe.RecipeElement>) results5), ingredients5, results5)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER.RECIPE_DESCRIPTION, (object) SimHashes.Salt.CreateTag().ProperName(), (object) ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME),
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("RockCrusher")
    };
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
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
