// Decompiled with JetBrains decompiler
// Type: KilnConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class KilnConfig : IBuildingConfig
{
  public const string ID = "Kiln";
  public const float INPUT_CLAY_PER_SECOND = 1f;
  public const float CERAMIC_PER_SECOND = 1f;
  public const float CO2_RATIO = 0.1f;
  public const float OUTPUT_TEMP = 353.15f;
  public const float REFILL_RATE = 2400f;
  public const float CERAMIC_STORAGE_AMOUNT = 2400f;
  public const float COAL_RATE = 0.1f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Kiln";
    int width = 2;
    int height = 2;
    string anim = "kiln_kanim";
    int hitpoints = 100;
    float construction_time = 30f;
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR5 = TUNING.NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tieR5, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.RequiresPowerInput = false;
    buildingDef.ExhaustKilowattsWhenActive = 16f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.AudioCategory = "HollowMetal";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.resultState = ComplexFabricator.ResultState.Heated;
    fabricator.heatedTemperature = 353.15f;
    fabricator.duplicantOperated = false;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    this.ConfgiureRecipes();
    Prioritizable.AddRef(go);
  }

  private void ConfgiureRecipes()
  {
    Tag tag1 = SimHashes.Ceramic.CreateTag();
    Tag tag2 = SimHashes.Clay.CreateTag();
    Tag tag3 = SimHashes.Carbon.CreateTag();
    float amount1 = 100f;
    float amount2 = 25f;
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(tag2, amount1),
      new ComplexRecipe.RecipeElement(tag3, amount2)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag1, amount1)
    };
    string obsolete_id1 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag1);
    string str1 = ComplexRecipeManager.MakeRecipeID("Kiln", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1);
    ComplexRecipe complexRecipe1 = new ComplexRecipe(str1, ingredients1, results1)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Clay).name, (object) ElementLoader.FindElementByHash(SimHashes.Ceramic).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("Kiln")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id1, str1);
    Tag tag4 = SimHashes.RefinedCarbon.CreateTag();
    ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag3, amount1 + amount2)
    };
    ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(tag4, amount1)
    };
    string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("Kiln", tag4);
    string str2 = ComplexRecipeManager.MakeRecipeID("Kiln", (IList<ComplexRecipe.RecipeElement>) ingredients2, (IList<ComplexRecipe.RecipeElement>) results2);
    ComplexRecipe complexRecipe2 = new ComplexRecipe(str2, ingredients2, results2)
    {
      time = 40f,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Carbon).name, (object) ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).name),
      fabricators = new List<Tag>()
      {
        TagManager.Create("Kiln")
      },
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, str2);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
