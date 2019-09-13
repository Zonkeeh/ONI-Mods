// Decompiled with JetBrains decompiler
// Type: GourmetCookingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GourmetCookingStationConfig : IBuildingConfig
{
  private static readonly List<Storage.StoredItemModifier> GourmetCookingStationStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };
  private Tag FUEL_TAG = new Tag("Methane");
  public const string ID = "GourmetCookingStation";
  private const float FUEL_STORE_CAPACITY = 10f;
  private const float FUEL_CONSUME_RATE = 0.1f;
  private const float CO2_EMIT_RATE = 0.025f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GourmetCookingStation";
    int width = 3;
    int height = 3;
    string anim = "cookstation_gourmet_kanim";
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
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.PowerInputOffset = new CellOffset(1, 0);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    GourmetCookingStation gourmetCookingStation = go.AddOrGet<GourmetCookingStation>();
    gourmetCookingStation.resultState = ComplexFabricator.ResultState.Heated;
    gourmetCookingStation.heatedTemperature = 368.15f;
    gourmetCookingStation.duplicantOperated = true;
    gourmetCookingStation.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) gourmetCookingStation);
    gourmetCookingStation.fuelTag = this.FUEL_TAG;
    gourmetCookingStation.outStorage.capacityKg = 10f;
    gourmetCookingStation.inStorage.SetDefaultStoredItemModifiers(GourmetCookingStationConfig.GourmetCookingStationStoredItemModifiers);
    gourmetCookingStation.buildStorage.SetDefaultStoredItemModifiers(GourmetCookingStationConfig.GourmetCookingStationStoredItemModifiers);
    gourmetCookingStation.outStorage.SetDefaultStoredItemModifiers(GourmetCookingStationConfig.GourmetCookingStationStoredItemModifiers);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.capacityTag = this.FUEL_TAG;
    conduitConsumer.capacityKG = 10f;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.storage = gourmetCookingStation.inStorage;
    conduitConsumer.forceAlwaysSatisfied = true;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(this.FUEL_TAG, 0.1f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.025f, SimHashes.CarbonDioxide, 348.15f, false, false, 0.0f, 3f, 1f, byte.MaxValue, 0)
    };
    this.ConfigureRecipes();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveStoppableController.Def>();
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (game_object =>
    {
      ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
      component.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
      component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    });
  }

  private void ConfigureRecipes()
  {
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "GrilledPrickleFruit", 2f),
      new ComplexRecipe.RecipeElement((Tag) SpiceNutConfig.ID, 2f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Salsa", 1f)
    };
    SalsaConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1), ingredients1, results1)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.SALSA.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 300
    };
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "FriedMushroom", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Lettuce", 4f)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "MushroomWrap", 1f)
    };
    MushroomWrapConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2), ingredients2, results2)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.MUSHROOMWRAP.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 400
    };
    ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "CookedMeat", 1f),
      new ComplexRecipe.RecipeElement((Tag) "CookedFish", 1f)
    };
    ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SurfAndTurf", 1f)
    };
    SurfAndTurfConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients3, (IList<ComplexRecipe.RecipeElement>) results3), ingredients3, results3)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.SURFANDTURF.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 500
    };
    ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatSeed", 10f),
      new ComplexRecipe.RecipeElement((Tag) SpiceNutConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SpiceBread", 1f)
    };
    SpiceBreadConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients4, (IList<ComplexRecipe.RecipeElement>) results4), ingredients4, results4)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.SPICEBREAD.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 600
    };
    ComplexRecipe.RecipeElement[] ingredients5 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "Tofu", 1f),
      new ComplexRecipe.RecipeElement((Tag) SpiceNutConfig.ID, 1f)
    };
    ComplexRecipe.RecipeElement[] results5 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "SpicyTofu", 1f)
    };
    SpicyTofuConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients5, (IList<ComplexRecipe.RecipeElement>) results5), ingredients5, results5)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.SPICYTOFU.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 800
    };
    ComplexRecipe.RecipeElement[] ingredients6 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement((Tag) "ColdWheatBread", 1f),
      new ComplexRecipe.RecipeElement((Tag) "Lettuce", 1f),
      new ComplexRecipe.RecipeElement((Tag) "CookedMeat", 1f)
    };
    ComplexRecipe.RecipeElement[] results6 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Burger", 1f)
    };
    BurgerConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("GourmetCookingStation", (IList<ComplexRecipe.RecipeElement>) ingredients6, (IList<ComplexRecipe.RecipeElement>) results6), ingredients6, results6)
    {
      time = TUNING.FOOD.RECIPES.STANDARD_COOK_TIME,
      description = (string) ITEMS.FOOD.BURGER.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>()
      {
        (Tag) "GourmetCookingStation"
      },
      sortOrder = 900
    };
  }
}
