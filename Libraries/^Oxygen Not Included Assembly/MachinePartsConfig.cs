// Decompiled with JetBrains decompiler
// Type: MachinePartsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MachinePartsConfig : IEntityConfig
{
  public static readonly Tag TAG = TagManager.Create("MachineParts");
  public const string ID = "MachineParts";
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateLooseEntity("MachineParts", (string) ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim((HashedString) "buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, (List<Tag>) null);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
