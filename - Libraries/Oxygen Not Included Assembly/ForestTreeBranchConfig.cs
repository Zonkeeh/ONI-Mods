// Decompiled with JetBrains decompiler
// Type: ForestTreeBranchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ForestTreeBranchConfig : IEntityConfig
{
  public const string ID = "ForestTreeBranch";
  public const float WOOD_AMOUNT = 300f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestTreeBranch", (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME, (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC, 8f, Assets.GetAnim((HashedString) "tree_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, new List<Tag>(), 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 313.15f, 448.15f, (SimHashes[]) null, true, 0.0f, 0.15f, "WoodLog", true, true, false, true, 12000f);
    placedEntity.AddOrGet<TreeBud>();
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<BudUprootedMonitor>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
