// Decompiled with JetBrains decompiler
// Type: WoodLogConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class WoodLogConfig : IEntityConfig
{
  public static readonly Tag TAG = TagManager.Create("WoodLog");
  public const string ID = "WoodLog";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("WoodLog", (string) ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.WOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "wood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.Organics
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
