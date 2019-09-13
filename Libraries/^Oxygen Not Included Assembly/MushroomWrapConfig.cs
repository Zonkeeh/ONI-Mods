// Decompiled with JetBrains decompiler
// Type: MushroomWrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MushroomWrapConfig : IEntityConfig
{
  public const string ID = "MushroomWrap";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushroomWrap", (string) ITEMS.FOOD.MUSHROOMWRAP.NAME, (string) ITEMS.FOOD.MUSHROOMWRAP.DESC, 1f, false, Assets.GetAnim((HashedString) "mushroom_wrap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.5f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.MUSHROOM_WRAP);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
