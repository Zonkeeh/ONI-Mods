// Decompiled with JetBrains decompiler
// Type: GrilledPrickleFruitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class GrilledPrickleFruitConfig : IEntityConfig
{
  public const string ID = "GrilledPrickleFruit";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("GrilledPrickleFruit", (string) ITEMS.FOOD.GRILLEDPRICKLEFRUIT.NAME, (string) ITEMS.FOOD.GRILLEDPRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim((HashedString) "gristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
