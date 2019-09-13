// Decompiled with JetBrains decompiler
// Type: FetchOrder2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FetchOrder2
{
  public List<FetchChore> Chores = new List<FetchChore>();
  private FetchOrder2.OperationalRequirement operationalRequirement = FetchOrder2.OperationalRequirement.None;
  public System.Action<FetchOrder2, Pickupable> OnComplete;
  public System.Action<FetchOrder2, Pickupable> OnBegin;
  private ChoreType choreType;
  private float _UnfetchedAmount;
  private bool checkStorageContents;

  public FetchOrder2(
    ChoreType chore_type,
    Tag[] tags,
    Tag[] required_tags,
    Tag[] forbidden_tags,
    Storage destination,
    float amount,
    FetchOrder2.OperationalRequirement operationalRequirementDEPRECATED = FetchOrder2.OperationalRequirement.None,
    int priorityMod = 0)
  {
    if ((double) amount <= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
      DebugUtil.LogWarningArgs((object) string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", (object) chore_type.Id, (object) tags[0], (object) amount, !((UnityEngine.Object) destination != (UnityEngine.Object) null) ? (object) "to nowhere" : (object) destination.name));
    this.choreType = chore_type;
    this.Tags = tags;
    this.RequiredTags = required_tags;
    this.ForbiddenTags = forbidden_tags;
    this.Destination = destination;
    this.TotalAmount = amount;
    this.UnfetchedAmount = amount;
    this.PriorityMod = priorityMod;
    this.operationalRequirement = operationalRequirementDEPRECATED;
  }

  public float TotalAmount { get; set; }

  public int PriorityMod { get; set; }

  public Tag[] Tags { get; protected set; }

  public Tag[] RequiredTags { get; protected set; }

  public Tag[] ForbiddenTags { get; protected set; }

  public Storage Destination { get; set; }

  private float UnfetchedAmount
  {
    get
    {
      return this._UnfetchedAmount;
    }
    set
    {
      this._UnfetchedAmount = value;
      this.Assert((double) this._UnfetchedAmount <= (double) this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
      this.Assert((double) this._UnfetchedAmount >= 0.0, "_UnfetchedAmount >= 0");
    }
  }

  public bool InProgress
  {
    get
    {
      bool flag = false;
      foreach (Chore chore in this.Chores)
      {
        if (chore.InProgress())
        {
          flag = true;
          break;
        }
      }
      return flag;
    }
  }

  private void IssueTask()
  {
    if ((double) this.UnfetchedAmount <= 0.0)
      return;
    this.SetFetchTask(this.UnfetchedAmount);
    this.UnfetchedAmount = 0.0f;
  }

  public void SetPriorityMod(int priorityMod)
  {
    this.PriorityMod = priorityMod;
    for (int index = 0; index < this.Chores.Count; ++index)
      this.Chores[index].SetPriorityMod(this.PriorityMod);
  }

  private void SetFetchTask(float amount)
  {
    this.Chores.Add(new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.RequiredTags, this.ForbiddenTags, (ChoreProvider) null, true, new System.Action<Chore>(this.OnFetchChoreComplete), new System.Action<Chore>(this.OnFetchChoreBegin), new System.Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod));
  }

  private void OnFetchChoreEnd(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    if (!this.Chores.Contains(fetchChore))
      return;
    this.UnfetchedAmount += fetchChore.amount;
    fetchChore.Cancel("FetchChore Redistribution");
    this.Chores.Remove(fetchChore);
    this.IssueTask();
  }

  private void OnFetchChoreComplete(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    this.Chores.Remove(fetchChore);
    if (this.Chores.Count != 0 || this.OnComplete == null)
      return;
    this.OnComplete(this, fetchChore.fetchTarget);
  }

  private void OnFetchChoreBegin(Chore chore)
  {
    FetchChore fetchChore = (FetchChore) chore;
    this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
    this.IssueTask();
    if (this.OnBegin == null)
      return;
    this.OnBegin(this, fetchChore.fetchTarget);
  }

  public void Cancel(string reason)
  {
    while (this.Chores.Count > 0)
    {
      FetchChore chore = this.Chores[0];
      chore.Cancel(reason);
      this.Chores.Remove(chore);
    }
  }

  public void Suspend(string reason)
  {
    Debug.LogError((object) "UNIMPLEMENTED!");
  }

  public void Resume(string reason)
  {
    Debug.LogError((object) "UNIMPLEMENTED!");
  }

  public void Submit(
    System.Action<FetchOrder2, Pickupable> on_complete,
    bool check_storage_contents,
    System.Action<FetchOrder2, Pickupable> on_begin = null)
  {
    this.OnComplete = on_complete;
    this.OnBegin = on_begin;
    this.checkStorageContents = check_storage_contents;
    if (check_storage_contents)
    {
      Pickupable out_item = (Pickupable) null;
      this.UnfetchedAmount = this.GetRemaining(out out_item);
      if ((double) this.UnfetchedAmount <= (double) this.Destination.storageFullMargin)
      {
        if (this.OnComplete == null)
          return;
        this.OnComplete(this, out_item);
      }
      else
        this.IssueTask();
    }
    else
      this.IssueTask();
  }

  public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
  {
    foreach (GameObject gameObject in this.Destination.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Pickupable component = gameObject.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          KPrefabID kprefabId = component.KPrefabID;
          foreach (Tag tag in this.Tags)
          {
            if (kprefabId.HasTag(tag))
            {
              amount = component.TotalAmount;
              out_item = component;
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  public float AmountWaitingToFetch()
  {
    if (this.checkStorageContents)
    {
      Pickupable out_item;
      return this.GetRemaining(out out_item);
    }
    float unfetchedAmount = this.UnfetchedAmount;
    for (int index = 0; index < this.Chores.Count; ++index)
      unfetchedAmount += this.Chores[index].AmountWaitingToFetch();
    return unfetchedAmount;
  }

  public float GetRemaining(out Pickupable out_item)
  {
    float num = this.TotalAmount;
    float amount = 0.0f;
    out_item = (Pickupable) null;
    if (this.IsMaterialOnStorage(this.Destination, ref amount, ref out_item))
      num = Math.Max(num - amount, 0.0f);
    return num;
  }

  public bool IsComplete()
  {
    for (int index = 0; index < this.Chores.Count; ++index)
    {
      if (!this.Chores[index].isComplete)
        return false;
    }
    return true;
  }

  private void Assert(bool condition, string message)
  {
    if (condition)
      return;
    string str = "FetchOrder error: " + message;
    Debug.LogError((object) ((!((UnityEngine.Object) this.Destination == (UnityEngine.Object) null) ? (object) (str + "\nDestination: " + this.Destination.name) : (object) (str + "\nDestination: None")).ToString() + "\nTotal Amount: " + (object) this.TotalAmount + "\nUnfetched Amount: " + (object) this._UnfetchedAmount));
  }

  public enum OperationalRequirement
  {
    Operational,
    Functional,
    None,
  }
}
