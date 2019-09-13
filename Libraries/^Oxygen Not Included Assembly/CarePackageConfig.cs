// Decompiled with JetBrains decompiler
// Type: CarePackageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class CarePackageConfig : IEntityConfig
{
  public static readonly string ID = "CarePackage";

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, (string) ITEMS.CARGO_CAPSULE.NAME, (string) ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim((HashedString) "portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, (List<Tag>) null);
  }

  public void OnPrefabInit(GameObject go)
  {
    go.AddOrGet<CarePackage>();
  }

  public void OnSpawn(GameObject go)
  {
  }
}
