// Decompiled with JetBrains decompiler
// Type: BasicBoosterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BasicBoosterConfig : IEntityConfig
{
  public const string ID = "BasicBooster";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("BasicBooster", (string) ITEMS.PILLS.BASICBOOSTER.NAME, (string) ITEMS.PILLS.BASICBOOSTER.DESC, 1f, true, Assets.GetAnim((HashedString) "pill_2_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null);
    EntityTemplates.ExtendEntityToMedicine(looseEntity, TUNING.MEDICINE.BASICBOOSTER);
    ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) "Carbon", 1f)
    };
    ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("BasicBooster".ToTag(), 1f)
    };
    BasicBoosterConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results), ingredients, results)
    {
      time = 50f,
      description = (string) ITEMS.PILLS.BASICBOOSTER.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 1
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
