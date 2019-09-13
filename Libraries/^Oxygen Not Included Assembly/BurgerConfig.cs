// Decompiled with JetBrains decompiler
// Type: BurgerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BurgerConfig : IEntityConfig
{
  public const string ID = "Burger";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Burger", (string) ITEMS.FOOD.BURGER.NAME, (string) ITEMS.FOOD.BURGER.DESC, 1f, false, Assets.GetAnim((HashedString) "frost_burger_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.BURGER);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
