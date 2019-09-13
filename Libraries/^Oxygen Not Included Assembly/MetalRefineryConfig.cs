// Decompiled with JetBrains decompiler
// Type: MetalRefineryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MetalRefineryConfig : IBuildingConfig
{
  private static readonly Tag COOLANT_TAG = GameTags.Liquid;
  private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };
  public const string ID = "MetalRefinery";
  private const float INPUT_KG = 100f;
  private const float LIQUID_COOLED_HEAT_PORTION = 0.8f;
  private const float COOLANT_MASS = 400f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "MetalRefinery";
    int width = 3;
    int height = 4;
    string anim = "metalrefinery_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR5 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMinerals = MATERIALS.ALL_MINERALS;
    float melting_point = 2400f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR6 = TUNING.NOISE_POLLUTION.NOISY.TIER6;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, allMinerals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tieR6, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 1200f;
    buildingDef.SelfHeatKilowattsWhenActive = 16f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 1);
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    LiquidCooledRefinery liquidCooledRefinery = go.AddOrGet<LiquidCooledRefinery>();
    liquidCooledRefinery.duplicantOperated = true;
    liquidCooledRefinery.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    liquidCooledRefinery.keepExcessLiquids = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, (ComplexFabricator) liquidCooledRefinery);
    liquidCooledRefinery.coolantTag = MetalRefineryConfig.COOLANT_TAG;
    liquidCooledRefinery.minCoolantMass = 400f;
    liquidCooledRefinery.outStorage.capacityKg = 2000f;
    liquidCooledRefinery.thermalFudge = 0.8f;
    liquidCooledRefinery.inStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
    liquidCooledRefinery.buildStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
    liquidCooledRefinery.outStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
    liquidCooledRefinery.outputOffset = new Vector3(1f, 0.5f);
    fabricatorWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_metalrefinery_kanim")
    };
    go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.capacityTag = GameTags.Liquid;
    conduitConsumer.capacityKG = 800f;
    conduitConsumer.storage = liquidCooledRefinery.inStorage;
    conduitConsumer.alwaysConsume = true;
    conduitConsumer.forceAlwaysSatisfied = true;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.storage = liquidCooledRefinery.outStorage;
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    conduitDispenser.alwaysDispense = true;
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
        ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
        {
          new ComplexRecipe.RecipeElement(lowTempTransition.tag, 100f)
        };
        string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element.tag);
        string str = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results);
        new ComplexRecipe(str, ingredients, results)
        {
          time = 40f,
          description = string.Format((string) STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object) lowTempTransition.name, (object) element.name),
          nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult
        }.fabricators = new List<Tag>()
        {
          TagManager.Create("MetalRefinery")
        };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
      }
    }
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Steel);
    ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag, 70f),
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 20f),
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 10f)
    };
    ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Steel).tag, 100f)
    };
    string obsolete_id1 = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", elementByHash.tag);
    string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>) ingredients1, (IList<ComplexRecipe.RecipeElement>) results1);
    new ComplexRecipe(str1, ingredients1, results1)
    {
      time = 40f,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object) ElementLoader.FindElementByHash(SimHashes.Steel).name, (object) ElementLoader.FindElementByHash(SimHashes.Iron).name)
    }.fabricators = new List<Tag>()
    {
      TagManager.Create("MetalRefinery")
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id1, str1);
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
    go.AddOrGetDef<PoweredActiveStoppableController.Def>();
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
