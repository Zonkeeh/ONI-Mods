// Decompiled with JetBrains decompiler
// Type: EntityConfigManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityConfigManager : KMonoBehaviour
{
  public static EntityConfigManager Instance;

  public static void DestroyInstance()
  {
    EntityConfigManager.Instance = (EntityConfigManager) null;
  }

  protected override void OnPrefabInit()
  {
    EntityConfigManager.Instance = this;
  }

  private static int GetSortOrder(System.Type type)
  {
    foreach (Attribute customAttribute in type.GetCustomAttributes(true))
    {
      if (customAttribute.GetType() == typeof (EntityConfigOrder))
        return (customAttribute as EntityConfigOrder).sortOrder;
    }
    return 0;
  }

  public void LoadGeneratedEntities(List<System.Type> types)
  {
    System.Type type1 = typeof (IEntityConfig);
    System.Type type2 = typeof (IMultiEntityConfig);
    List<EntityConfigManager.ConfigEntry> configEntryList = new List<EntityConfigManager.ConfigEntry>();
    foreach (System.Type type3 in types)
    {
      if ((type1.IsAssignableFrom(type3) || type2.IsAssignableFrom(type3)) && (!type3.IsAbstract && !type3.IsInterface))
      {
        int sortOrder = EntityConfigManager.GetSortOrder(type3);
        EntityConfigManager.ConfigEntry configEntry = new EntityConfigManager.ConfigEntry()
        {
          type = type3,
          sortOrder = sortOrder
        };
        configEntryList.Add(configEntry);
      }
    }
    configEntryList.Sort((Comparison<EntityConfigManager.ConfigEntry>) ((x, y) => x.sortOrder.CompareTo(y.sortOrder)));
    foreach (EntityConfigManager.ConfigEntry configEntry in configEntryList)
    {
      object instance = Activator.CreateInstance(configEntry.type);
      if (instance is IEntityConfig)
        this.RegisterEntity(instance as IEntityConfig);
      if (instance is IMultiEntityConfig)
        this.RegisterEntities(instance as IMultiEntityConfig);
    }
  }

  public void RegisterEntity(IEntityConfig config)
  {
    KPrefabID component = config.CreatePrefab().GetComponent<KPrefabID>();
    component.prefabInitFn += new KPrefabID.PrefabFn(config.OnPrefabInit);
    component.prefabSpawnFn += new KPrefabID.PrefabFn(config.OnSpawn);
    Assets.AddPrefab(component);
  }

  public void RegisterEntities(IMultiEntityConfig config)
  {
    foreach (GameObject prefab in config.CreatePrefabs())
    {
      KPrefabID component = prefab.GetComponent<KPrefabID>();
      component.prefabInitFn += new KPrefabID.PrefabFn(config.OnPrefabInit);
      component.prefabSpawnFn += new KPrefabID.PrefabFn(config.OnSpawn);
      Assets.AddPrefab(component);
    }
  }

  private struct ConfigEntry
  {
    public System.Type type;
    public int sortOrder;
  }
}
