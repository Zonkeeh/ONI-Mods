// Decompiled with JetBrains decompiler
// Type: MeterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MeterConfig : IEntityConfig
{
  public static readonly string ID = "Meter";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
    entity.AddOrGet<KBatchedAnimController>();
    entity.AddOrGet<KBatchedAnimTracker>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
