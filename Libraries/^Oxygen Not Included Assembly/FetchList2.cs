// Decompiled with JetBrains decompiler
// Type: FetchList2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FetchList2 : IFetchList
{
  public Guid waitingForMaterialsHandle = Guid.Empty;
  public Guid materialsUnavailableForRefillHandle = Guid.Empty;
  public Guid materialsUnavailableHandle = Guid.Empty;
  public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();
  public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();
  private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();
  private bool bShowStatusItem = true;
  private System.Action OnComplete;
  private ChoreType choreType;

  public FetchList2(Storage destination, ChoreType chore_type)
  {
    this.Destination = destination;
    this.choreType = chore_type;
  }

  public bool ShowStatusItem
  {
    get
    {
      return this.bShowStatusItem;
    }
    set
    {
      this.bShowStatusItem = value;
    }
  }

  public bool IsComplete
  {
    get
    {
      return this.FetchOrders.Count == 0;
    }
  }

  public bool InProgress
  {
    get
    {
      if (this.FetchOrders.Count < 0)
        return false;
      bool flag = false;
      foreach (FetchOrder2 fetchOrder in this.FetchOrders)
      {
        if (fetchOrder.InProgress)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }
  }

  public Storage Destination { get; private set; }

  public int PriorityMod { get; private set; }

  public void SetPriorityMod(int priorityMod)
  {
    this.PriorityMod = priorityMod;
    for (int index = 0; index < this.FetchOrders.Count; ++index)
      this.FetchOrders[index].SetPriorityMod(this.PriorityMod);
  }

  public void Add(
    Tag[] tags,
    Tag[] required_tags = null,
    Tag[] forbidden_tags = null,
    float amount = 1f,
    FetchOrder2.OperationalRequirement operationalRequirementDEPRECATED = FetchOrder2.OperationalRequirement.None)
  {
    foreach (Tag tag in tags)
    {
      if (!this.MinimumAmount.ContainsKey(tag))
        this.MinimumAmount[tag] = amount;
    }
    this.FetchOrders.Add(new FetchOrder2(this.choreType, tags, required_tags, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod));
  }

  public void Add(
    Tag tag,
    Tag[] required_tags = null,
    Tag[] forbidden_tags = null,
    float amount = 1f,
    FetchOrder2.OperationalRequirement operationalRequirementDEPRECATED = FetchOrder2.OperationalRequirement.None)
  {
    this.Add(new Tag[1]{ tag }, required_tags, forbidden_tags, amount, operationalRequirementDEPRECATED);
  }

  public float GetMinimumAmount(Tag tag)
  {
    float num = 0.0f;
    this.MinimumAmount.TryGetValue(tag, out num);
    return num;
  }

  private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
  {
    this.FetchOrders.Remove(fetch_order);
    if (this.FetchOrders.Count != 0)
      return;
    if (this.OnComplete != null)
      this.OnComplete();
    FetchListStatusItemUpdater.instance.RemoveFetchList(this);
    this.ClearStatus();
  }

  public void Cancel(string reason)
  {
    foreach (FetchOrder2 fetchOrder in this.FetchOrders)
      fetchOrder.Cancel(reason);
    this.ClearStatus();
    FetchListStatusItemUpdater.instance.RemoveFetchList(this);
  }

  public void UpdateRemaining()
  {
    this.Remaining.Clear();
    for (int index1 = 0; index1 < this.FetchOrders.Count; ++index1)
    {
      FetchOrder2 fetchOrder = this.FetchOrders[index1];
      for (int index2 = 0; index2 < fetchOrder.Tags.Length; ++index2)
      {
        Tag tag = fetchOrder.Tags[index2];
        float num = 0.0f;
        this.Remaining.TryGetValue(tag, out num);
        this.Remaining[tag] = num + fetchOrder.AmountWaitingToFetch();
      }
    }
  }

  public Dictionary<Tag, float> GetRemaining()
  {
    return this.Remaining;
  }

  public Dictionary<Tag, float> GetRemainingMinimum()
  {
    Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
    foreach (FetchOrder2 fetchOrder in this.FetchOrders)
    {
      foreach (Tag tag in fetchOrder.Tags)
        dictionary[tag] = this.MinimumAmount[tag];
    }
    foreach (GameObject gameObject in this.Destination.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Pickupable component = gameObject.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (Tag tag in component.GetComponent<KPrefabID>().Tags)
          {
            if (dictionary.ContainsKey(tag))
              dictionary[tag] = Math.Max(dictionary[tag] - component.TotalAmount, 0.0f);
          }
        }
      }
    }
    return dictionary;
  }

  public void Suspend(string reason)
  {
    foreach (FetchOrder2 fetchOrder in this.FetchOrders)
      fetchOrder.Suspend(reason);
  }

  public void Resume(string reason)
  {
    foreach (FetchOrder2 fetchOrder in this.FetchOrders)
      fetchOrder.Resume(reason);
  }

  public void Submit(System.Action on_complete, bool check_storage_contents)
  {
    this.OnComplete = on_complete;
    foreach (FetchOrder2 fetchOrder2 in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
      fetchOrder2.Submit(new System.Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, (System.Action<FetchOrder2, Pickupable>) null);
    if (this.IsComplete || !this.ShowStatusItem)
      return;
    FetchListStatusItemUpdater.instance.AddFetchList(this);
  }

  private void ClearStatus()
  {
    if (!((UnityEngine.Object) this.Destination != (UnityEngine.Object) null))
      return;
    KSelectable component = this.Destination.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
    this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
    this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
  }

  public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
  {
    bool flag = handle != Guid.Empty;
    if (should_add == flag)
      return;
    if (should_add)
    {
      KSelectable component = this.Destination.GetComponent<KSelectable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      handle = component.AddStatusItem((StatusItem) status_item, (object) this);
    }
    else
    {
      KSelectable component = this.Destination.GetComponent<KSelectable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      handle = component.RemoveStatusItem(handle, false);
    }
  }
}
