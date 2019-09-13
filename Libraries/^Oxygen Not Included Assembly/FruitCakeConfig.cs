// Decompiled with JetBrains decompiler
// Type: FruitCakeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class FruitCakeConfig : IEntityConfig
{
  public const string ID = "FruitCake";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FruitCake", (string) ITEMS.FOOD.FRUITCAKE.NAME, (string) ITEMS.FOOD.FRUITCAKE.DESC, 1f, false, Assets.GetAnim((HashedString) "fruitcake_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.FRUITCAKE);
    ComplexRecipeManager.Get().GetRecipe(FruitCakeConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(food);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
