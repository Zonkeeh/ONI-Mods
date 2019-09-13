// Decompiled with JetBrains decompiler
// Type: CookingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class CookingStationConfig : IBuildingConfig
{
  public const string ID = "CookingStation";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "CookingStation";
    int width = 3;
    int height = 2;
    string anim = "cookstation_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR3 = TUNING.NOISE_POLLUTION.NOISY.TIER3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tieR3, 0.2f);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    CookingStation cookingStation = go.AddOrGet<CookingStation>();
    cookingStation.resultState = ComplexFabricator.ResultState.Heated;
    cookingStation.heatedTemperature = 368.15f;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_cookstation_kanim")
    };
    cookingStation.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    this.ConfigureRecipes();
    go.AddOrGetDef<PoweredController.Def>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) cookingStation);
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "BasicPlantFood", 3f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "PickledMeal", 1f)
    };
    PickledMealConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = TUNING.FOOD.RECIPES.SMALL_COOK_TIME,
      description = (string) ITEMS.FOOD.PICKLEDMEAL.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 21
    };
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MushBar", 1f)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("FriedMushBar".ToTag(), 1f)
    };
    FriedMushBarConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.FRIEDMUSHBAR.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 1
    };
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) MushroomConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "FriedMushroom", 1f)
    };
    FriedMushroomConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.FRIEDMUSHROOM.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 20
    };
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Meat", 2f)
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedMeat", 1f)
    };
    CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 21
    };
    ComplexRecipe.RecipeElement[] ingredients5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "FishMeat", 1f)
    };
    ComplexRecipe.RecipeElement[] results5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedFish", 1f)
    };
    CookedMeatConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients5, (IList<ComplexRecipe.RecipeElement>) results5), ingredients5, results5)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.COOKEDMEAT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 21
    };
    ComplexRecipe.RecipeElement[] ingredients6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) PrickleFruitConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] results6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "GrilledPrickleFruit", 1f)
    };
    GrilledPrickleFruitConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients6, (IList<ComplexRecipe.RecipeElement>) results6), ingredients6, results6)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.GRILLEDPRICKLEFRUIT.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 20
    };
    ComplexRecipe.RecipeElement[] ingredients7 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatSeed", 3f)
    };
    ComplexRecipe.RecipeElement[] results7 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatBread", 1f)
    };
    ColdWheatBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients7, (IList<ComplexRecipe.RecipeElement>) results7), ingredients7, results7)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.COLDWHEATBREAD.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 50
    };
    ComplexRecipe.RecipeElement[] ingredients8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "RawEgg", 1f)
    };
    ComplexRecipe.RecipeElement[] results8 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedEgg", 1f)
    };
    CookedEggConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients8, (IList<ComplexRecipe.RecipeElement>) results8), ingredients8, results8)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.COOKEDEGG.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "CookingStation"
      },
      sortOrder = 1
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
