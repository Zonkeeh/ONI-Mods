// Decompiled with JetBrains decompiler
// Type: FetchListStatusItemUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class FetchListStatusItemUpdater : KMonoBehaviour, IRender1000ms
{
  private List<FetchList2> fetchLists = new List<FetchList2>();
  public static FetchListStatusItemUpdater instance;

  public static void DestroyInstance()
  {
    FetchListStatusItemUpdater.instance = (FetchListStatusItemUpdater) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FetchListStatusItemUpdater.instance = this;
  }

  public void AddFetchList(FetchList2 fetch_list)
  {
    this.fetchLists.Add(fetch_list);
  }

  public void RemoveFetchList(FetchList2 fetch_list)
  {
    this.fetchLists.Remove(fetch_list);
  }

  public void Render1000ms(float dt)
  {
    DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary1 = DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.Allocate();
    foreach (FetchList2 fetchList in this.fetchLists)
    {
      if (!((Object) fetchList.Destination == (Object) null))
      {
        ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList pooledList = (ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList) null;
        int instanceId = fetchList.Destination.GetInstanceID();
        if (!pooledDictionary1.TryGetValue(instanceId, out pooledList))
        {
          pooledList = ListPool<FetchList2, FetchListStatusItemUpdater>.Allocate();
          pooledDictionary1[instanceId] = pooledList;
        }
        pooledList.Add(fetchList);
      }
    }
    DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
    DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary3 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
    foreach (KeyValuePair<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList> keyValuePair1 in (Dictionary<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList>) pooledDictionary1)
    {
      ListPool<Tag, FetchListStatusItemUpdater>.PooledList pooledList1 = ListPool<Tag, FetchListStatusItemUpdater>.Allocate();
      Storage destination = keyValuePair1.Value[0].Destination;
      foreach (FetchList2 fetchList2 in (List<FetchList2>) keyValuePair1.Value)
      {
        fetchList2.UpdateRemaining();
        foreach (KeyValuePair<Tag, float> keyValuePair2 in fetchList2.GetRemaining())
        {
          if (!pooledList1.Contains(keyValuePair2.Key))
            pooledList1.Add(keyValuePair2.Key);
        }
      }
      ListPool<Pickupable, FetchListStatusItemUpdater>.PooledList pooledList2 = ListPool<Pickupable, FetchListStatusItemUpdater>.Allocate();
      foreach (GameObject gameObject in destination.items)
      {
        if (!((Object) gameObject == (Object) null))
        {
          Pickupable component = gameObject.GetComponent<Pickupable>();
          if (!((Object) component == (Object) null))
            pooledList2.Add(component);
        }
      }
      DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary4 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
      foreach (Tag tag in (List<Tag>) pooledList1)
      {
        float num = 0.0f;
        foreach (Pickupable pickupable in (List<Pickupable>) pooledList2)
        {
          if (pickupable.KPrefabID.HasTag(tag))
            num += pickupable.TotalAmount;
        }
        pooledDictionary4[tag] = num;
      }
      foreach (Tag index in (List<Tag>) pooledList1)
      {
        if (!pooledDictionary2.ContainsKey(index))
          pooledDictionary2[index] = WorldInventory.Instance.GetTotalAmount(index);
        if (!pooledDictionary3.ContainsKey(index))
          pooledDictionary3[index] = WorldInventory.Instance.GetAmount(index);
      }
      foreach (FetchList2 fetchList2 in (List<FetchList2>) keyValuePair1.Value)
      {
        bool should_add1 = false;
        bool should_add2 = true;
        bool should_add3 = false;
        foreach (KeyValuePair<Tag, float> keyValuePair2 in fetchList2.GetRemaining())
        {
          Tag key = keyValuePair2.Key;
          float a = keyValuePair2.Value;
          float num1 = pooledDictionary4[key];
          float b = pooledDictionary2[key];
          float num2 = pooledDictionary3[key] + Mathf.Min(a, b);
          float minimumAmount = fetchList2.GetMinimumAmount(key);
          if ((double) num1 + (double) num2 < (double) minimumAmount)
            should_add1 = true;
          if ((double) num2 < (double) a)
            should_add2 = false;
          if ((double) num1 + (double) num2 > (double) a && (double) a > (double) num2)
            should_add3 = true;
        }
        fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, ref fetchList2.waitingForMaterialsHandle, should_add2);
        fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, ref fetchList2.materialsUnavailableHandle, should_add1);
        fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailableForRefill, ref fetchList2.materialsUnavailableForRefillHandle, should_add3);
      }
      pooledDictionary4.Recycle();
      pooledList2.Recycle();
      pooledList1.Recycle();
      keyValuePair1.Value.Recycle();
    }
    pooledDictionary3.Recycle();
    pooledDictionary2.Recycle();
    pooledDictionary1.Recycle();
  }
}
