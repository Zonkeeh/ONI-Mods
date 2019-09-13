// Decompiled with JetBrains decompiler
// Type: SpiceBreadConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class SpiceBreadConfig : IEntityConfig
{
  public const string ID = "SpiceBread";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SpiceBread", (string) ITEMS.FOOD.SPICEBREAD.NAME, (string) ITEMS.FOOD.SPICEBREAD.DESC, 1f, false, Assets.GetAnim((HashedString) "pepperbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.SPICEBREAD);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
