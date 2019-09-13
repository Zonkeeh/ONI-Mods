// Decompiled with JetBrains decompiler
// Type: ForestForagePlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ForestForagePlantConfig : IEntityConfig
{
  public const string ID = "ForestForagePlant";

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ForestForagePlant", (string) ITEMS.FOOD.FORESTFORAGEPLANT.NAME, (string) ITEMS.FOOD.FORESTFORAGEPLANT.DESC, 1f, false, Assets.GetAnim((HashedString) "podmelon_fruit_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.FORESTFORAGEPLANT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
