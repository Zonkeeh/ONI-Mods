// Decompiled with JetBrains decompiler
// Type: GlobalChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GlobalChoreProvider : ChoreProvider, ISim200ms, IRender200ms
{
  private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();
  private static WorkItemCollection<GlobalChoreProvider.FindTopPriorityTask, object> find_top_priority_job = new WorkItemCollection<GlobalChoreProvider.FindTopPriorityTask, object>();
  public List<FetchChore> fetchChores = new List<FetchChore>();
  public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();
  private TagBits storageFetchableBits = new TagBits();
  public static GlobalChoreProvider Instance;
  private ClearableManager clearableManager;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GlobalChoreProvider.Instance = this;
    this.clearableManager = new ClearableManager();
  }

  public override void AddChore(Chore chore)
  {
    base.AddChore(chore);
    FetchChore fetchChore = chore as FetchChore;
    if (fetchChore == null)
      return;
    this.fetchChores.Add(fetchChore);
  }

  public override void RemoveChore(Chore chore)
  {
    base.RemoveChore(chore);
    FetchChore fetchChore = chore as FetchChore;
    if (fetchChore == null)
      return;
    this.fetchChores.Remove(fetchChore);
  }

  public void UpdateFetches(PathProber path_prober)
  {
    this.fetches.Clear();
    Navigator component = path_prober.GetComponent<Navigator>();
    foreach (FetchChore fetchChore in this.fetchChores)
    {
      if (!((Object) fetchChore.driver != (Object) null) && (!((Object) fetchChore.automatable != (Object) null) || !fetchChore.automatable.GetAutomationOnly()))
      {
        Storage destination = fetchChore.destination;
        if (!((Object) destination == (Object) null))
        {
          int navigationCost = component.GetNavigationCost((IApproachable) destination);
          if (navigationCost != -1)
            this.fetches.Add(new GlobalChoreProvider.Fetch()
            {
              chore = fetchChore,
              tagBitsHash = fetchChore.tagBitsHash,
              cost = navigationCost,
              priority = fetchChore.masterPriority,
              category = destination.fetchCategory
            });
        }
      }
    }
    if (this.fetches.Count <= 0)
      return;
    this.fetches.Sort((IComparer<GlobalChoreProvider.Fetch>) GlobalChoreProvider.Comparer);
    int index1 = 1;
    int index2 = 0;
    for (; index1 < this.fetches.Count; ++index1)
    {
      if (!this.fetches[index2].IsBetterThan(this.fetches[index1]))
      {
        ++index2;
        this.fetches[index2] = this.fetches[index1];
      }
    }
    this.fetches.RemoveRange(index2 + 1, this.fetches.Count - index2 - 1);
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    base.CollectChores(consumer_state, succeeded, failed_contexts);
    this.clearableManager.CollectChores(consumer_state, succeeded, failed_contexts);
    foreach (GlobalChoreProvider.Fetch fetch in this.fetches)
      fetch.chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
  }

  public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
  {
    return this.clearableManager.RegisterClearable(clearable);
  }

  public void UnregisterClearable(HandleVector<int>.Handle handle)
  {
    this.clearableManager.UnregisterClearable(handle);
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
    GlobalChoreProvider.Instance = (GlobalChoreProvider) null;
  }

  public void Sim200ms(float time_delta)
  {
    GlobalChoreProvider.find_top_priority_job.Reset((object) null);
    GlobalChoreProvider.FindTopPriorityTask.abort = false;
    int num = 512;
    for (int start = 0; start < Components.Prioritizables.Items.Count; start += num)
    {
      int end = start + num;
      if (Components.Prioritizables.Items.Count < end)
        end = Components.Prioritizables.Items.Count;
      GlobalChoreProvider.find_top_priority_job.Add(new GlobalChoreProvider.FindTopPriorityTask(start, end));
    }
    GlobalJobManager.Run((IWorkItemCollection) GlobalChoreProvider.find_top_priority_job);
    bool on = false;
    for (int idx = 0; idx != GlobalChoreProvider.find_top_priority_job.Count; ++idx)
    {
      if (GlobalChoreProvider.find_top_priority_job.GetWorkItem(idx).found)
      {
        on = true;
        break;
      }
    }
    VignetteManager.Instance.Get().HasTopPriorityChore(on);
  }

  public void Render200ms(float dt)
  {
    this.UpdateStorageFetchableBits();
  }

  private void UpdateStorageFetchableBits()
  {
    ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
    ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
    this.storageFetchableBits.ClearAll();
    foreach (FetchChore fetchChore in this.fetchChores)
    {
      if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && (bool) ((Object) fetchChore.destination))
      {
        int cell = Grid.PosToCell((KMonoBehaviour) fetchChore.destination);
        if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
          this.storageFetchableBits.Or(ref fetchChore.tagBits);
      }
    }
  }

  public bool ClearableHasDestination(Pickupable pickupable)
  {
    KPrefabID kprefabId = pickupable.KPrefabID;
    kprefabId.UpdateTagBits();
    return kprefabId.HasAnyTags_AssumeLaundered(ref this.storageFetchableBits);
  }

  public struct Fetch
  {
    public FetchChore chore;
    public int tagBitsHash;
    public int cost;
    public PrioritySetting priority;
    public Storage.FetchCategory category;

    public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
    {
      if (this.category != fetch.category || this.tagBitsHash != fetch.tagBitsHash || (this.chore.choreType != fetch.chore.choreType || !this.chore.tagBits.AreEqual(ref fetch.chore.tagBits)))
        return false;
      if (this.priority.priority_class > fetch.priority.priority_class)
        return true;
      if (this.priority.priority_class == fetch.priority.priority_class)
      {
        if (this.priority.priority_value > fetch.priority.priority_value)
          return true;
        if (this.priority.priority_value == fetch.priority.priority_value)
          return this.cost <= fetch.cost;
      }
      return false;
    }
  }

  private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
  {
    public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
    {
      int num1 = b.priority.priority_class - a.priority.priority_class;
      if (num1 != 0)
        return num1;
      int num2 = b.priority.priority_value - a.priority.priority_value;
      if (num2 != 0)
        return num2;
      return a.cost - b.cost;
    }
  }

  private struct FindTopPriorityTask : IWorkItem<object>
  {
    private int start;
    private int end;
    public bool found;
    public static bool abort;

    public FindTopPriorityTask(int start, int end)
    {
      this.start = start;
      this.end = end;
      this.found = false;
    }

    public void Run(object context)
    {
      if (GlobalChoreProvider.FindTopPriorityTask.abort)
        return;
      for (int start = this.start; start != this.end; ++start)
      {
        if (Components.Prioritizables.Items[start].IsTopPriority())
        {
          this.found = true;
          break;
        }
      }
      if (!this.found)
        return;
      GlobalChoreProvider.FindTopPriorityTask.abort = true;
    }
  }
}
