﻿// Decompiled with JetBrains decompiler
// Type: DigPlacerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
  public static string ID = "DigPlacer";

  public GameObject CreatePrefab()
  {
    GameObject prefab = this.CreatePrefab(DigPlacerConfig.ID, (string) MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
    Diggable diggable = prefab.AddOrGet<Diggable>();
    diggable.workTime = 5f;
    diggable.synchronizeAnims = false;
    diggable.workAnims = new HashedString[2]
    {
      (HashedString) "place",
      (HashedString) "release"
    };
    diggable.materials = Assets.instance.digPlacerAssets.materials;
    diggable.materialDisplay = prefab.GetComponentInChildren<MeshRenderer>(true);
    prefab.AddOrGet<CancellableDig>();
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }

  [Serializable]
  public class DigPlacerAssets
  {
    public Material[] materials;
  }
}