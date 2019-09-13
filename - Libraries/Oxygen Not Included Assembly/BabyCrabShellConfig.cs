// Decompiled with JetBrains decompiler
// Type: BabyCrabShellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class BabyCrabShellConfig : IEntityConfig
{
  public static readonly Tag TAG = TagManager.Create("BabyCrabShell");
  public const string ID = "BabyCrabShell";
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("BabyCrabShell", (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 5f, true, Assets.GetAnim((HashedString) "crabshells_small_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.Organics
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
