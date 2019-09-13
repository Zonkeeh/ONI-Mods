// Decompiled with JetBrains decompiler
// Type: AdvancedCureConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedCureConfig : IEntityConfig
{
  public const string ID = "AdvancedCure";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("AdvancedCure", (string) ITEMS.PILLS.ADVANCEDCURE.NAME, (string) ITEMS.PILLS.ADVANCEDCURE.DESC, 1f, true, Assets.GetAnim((HashedString) "vial_spore_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null);
    looseEntity.GetComponent<KPrefabID>().AddTag(GameTags.MedicalSupplies, false);
    ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 1f),
      new ComplexRecipe.RecipeElement((Tag) "LightBugOrangeEgg", 1f)
    };
    ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "AdvancedCure", 1f)
    };
    AdvancedCureConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results), ingredients, results)
    {
      time = 200f,
      description = (string) ITEMS.PILLS.ADVANCEDCURE.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 20,
      requiredTech = "MedicineIV"
    };
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
