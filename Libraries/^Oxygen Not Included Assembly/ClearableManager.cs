// Decompiled with JetBrains decompiler
// Type: ClearableManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

internal class ClearableManager
{
  private KCompactedVector<ClearableManager.MarkedClearable> markedClearables = new KCompactedVector<ClearableManager.MarkedClearable>(0);
  private List<ClearableManager.SortedClearable> sortedClearables = new List<ClearableManager.SortedClearable>();

  public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
  {
    return this.markedClearables.Allocate(new ClearableManager.MarkedClearable()
    {
      clearable = clearable,
      pickupable = clearable.GetComponent<Pickupable>(),
      prioritizable = clearable.GetComponent<Prioritizable>()
    });
  }

  public void UnregisterClearable(HandleVector<int>.Handle handle)
  {
    this.markedClearables.Free(handle);
  }

  private static void CollectSortedClearables(
    Navigator navigator,
    KCompactedVector<ClearableManager.MarkedClearable> clearables,
    List<ClearableManager.SortedClearable> sorted_clearables)
  {
    sorted_clearables.Clear();
    foreach (ClearableManager.MarkedClearable data in clearables.GetDataList())
    {
      int navigationCost = data.pickupable.GetNavigationCost(navigator, data.pickupable.cachedCell);
      if (navigationCost != -1)
        sorted_clearables.Add(new ClearableManager.SortedClearable()
        {
          pickupable = data.pickupable,
          masterPriority = data.prioritizable.GetMasterPriority(),
          cost = navigationCost
        });
    }
    sorted_clearables.Sort((IComparer<ClearableManager.SortedClearable>) ClearableManager.SortedClearable.comparer);
  }

  public void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    ChoreType transport = Db.Get().ChoreTypes.Transport;
    int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
    int num = !Game.Instance.advancedPersonalPriorities ? transport.priority : transport.explicitPriority;
    ClearableManager.CollectSortedClearables(consumer_state.navigator, this.markedClearables, this.sortedClearables);
    bool flag = false;
    foreach (ClearableManager.SortedClearable sortedClearable in this.sortedClearables)
    {
      Pickupable pickupable = sortedClearable.pickupable;
      PrioritySetting masterPriority = sortedClearable.masterPriority;
      Chore.Precondition.Context context = new Chore.Precondition.Context();
      context.personalPriority = personalPriority;
      KPrefabID kprefabId = pickupable.KPrefabID;
      kprefabId.UpdateTagBits();
      foreach (GlobalChoreProvider.Fetch fetch in GlobalChoreProvider.Instance.fetches)
      {
        if (kprefabId.HasAnyTags_AssumeLaundered(ref fetch.chore.tagBits))
        {
          context.Set((Chore) fetch.chore, consumer_state, false, (object) pickupable);
          context.choreTypeForPermission = transport;
          context.RunPreconditions();
          if (context.IsSuccess())
          {
            context.masterPriority = masterPriority;
            context.priority = num;
            context.interruptPriority = transport.interruptPriority;
            succeeded.Add(context);
            flag = true;
            break;
          }
        }
      }
      if (flag)
        break;
    }
  }

  private struct MarkedClearable
  {
    public Clearable clearable;
    public Pickupable pickupable;
    public Prioritizable prioritizable;
  }

  private struct SortedClearable
  {
    public static ClearableManager.SortedClearable.Comparer comparer = new ClearableManager.SortedClearable.Comparer();
    public Pickupable pickupable;
    public PrioritySetting masterPriority;
    public int cost;

    public class Comparer : IComparer<ClearableManager.SortedClearable>
    {
      public int Compare(ClearableManager.SortedClearable a, ClearableManager.SortedClearable b)
      {
        int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
        if (num == 0)
          return a.cost - b.cost;
        return num;
      }
    }
  }
}
