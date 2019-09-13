// Decompiled with JetBrains decompiler
// Type: RawEggConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class RawEggConfig : IEntityConfig
{
  public const string ID = "RawEgg";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("RawEgg", (string) ITEMS.FOOD.RAWEGG.NAME, (string) ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim((HashedString) "rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, (List<Tag>) null);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.RAWEGG);
    TemperatureCookable temperatureCookable = looseEntity.AddOrGet<TemperatureCookable>();
    temperatureCookable.cookTemperature = 344.15f;
    temperatureCookable.cookedID = "CookedEgg";
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
