// Decompiled with JetBrains decompiler
// Type: PickledMealConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PickledMealConfig : IEntityConfig
{
  public const string ID = "PickledMeal";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", (string) ITEMS.FOOD.PICKLEDMEAL.NAME, (string) ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim((HashedString) "pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.PICKLEDMEAL);
    food.GetComponent<KPrefabID>().AddTag(GameTags.Pickled, false);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
