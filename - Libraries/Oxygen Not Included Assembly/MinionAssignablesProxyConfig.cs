// Decompiled with JetBrains decompiler
// Type: MinionAssignablesProxyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MinionAssignablesProxyConfig : IEntityConfig
{
  public static string ID = "MinionAssignablesProxy";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<Ownables>();
    entity.AddOrGet<Equipment>();
    entity.AddOrGet<MinionAssignablesProxy>();
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
