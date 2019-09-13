// Decompiled with JetBrains decompiler
// Type: GasGrassHarvestedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class GasGrassHarvestedConfig : IEntityConfig
{
  public const string ID = "GasGrassHarvested";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("GasGrassHarvested", (string) CREATURES.SPECIES.GASGRASS.NAME, (string) CREATURES.SPECIES.GASGRASS.DESC, 1f, false, Assets.GetAnim((HashedString) "harvested_gassygrass_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>()
    {
      GameTags.Other
    });
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
