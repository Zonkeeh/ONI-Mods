// Decompiled with JetBrains decompiler
// Type: RepairableStorageProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RepairableStorageProxy : IEntityConfig
{
  public static string ID = nameof (RepairableStorageProxy);

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
    entity.AddOrGet<Storage>();
    entity.AddTag(GameTags.NotConversationTopic);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
