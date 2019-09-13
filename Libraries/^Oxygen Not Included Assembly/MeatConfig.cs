// Decompiled with JetBrains decompiler
// Type: MeatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MeatConfig : IEntityConfig
{
  public const string ID = "Meat";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("Meat", (string) ITEMS.FOOD.MEAT.NAME, (string) ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim((HashedString) "creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.MEAT);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
