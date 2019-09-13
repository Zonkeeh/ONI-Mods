// Decompiled with JetBrains decompiler
// Type: FetchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FetchManager : KMonoBehaviour, ISim1000ms
{
  public static TagBits disallowedTagBits = new TagBits(GameTags.Preserved);
  public static TagBits disallowedTagMask = TagBits.MakeComplement(ref FetchManager.disallowedTagBits);
  private static readonly FetchManager.PickupComparerIncludingPriority ComparerIncludingPriority = new FetchManager.PickupComparerIncludingPriority();
  private static readonly FetchManager.PickupComparerNoPriority ComparerNoPriority = new FetchManager.PickupComparerNoPriority();
  private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();
  public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();
  private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void BeginDetailedSample(string region_name)
  {
  }

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void BeginDetailedSample(string region_name, int count)
  {
  }

  [Conditional("ENABLE_FETCH_PROFILING")]
  private static void EndDetailedSample()
  {
  }

  public HandleVector<int>.Handle Add(Pickupable pickupable)
  {
    Tag index = pickupable.PrefabID();
    FetchManager.FetchablesByPrefabId fetchablesByPrefabId = (FetchManager.FetchablesByPrefabId) null;
    if (!this.prefabIdToFetchables.TryGetValue(index, out fetchablesByPrefabId))
    {
      fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(index);
      this.prefabIdToFetchables[index] = fetchablesByPrefabId;
    }
    return fetchablesByPrefabId.AddPickupable(pickupable);
  }

  public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
  {
    this.prefabIdToFetchables[prefab_tag].RemovePickupable(fetchable_handle);
  }

  public void UpdateStorage(
    Tag prefab_tag,
    HandleVector<int>.Handle fetchable_handle,
    Storage storage)
  {
    this.prefabIdToFetchables[prefab_tag].UpdateStorage(fetchable_handle, storage);
  }

  public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
  {
    this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
  }

  public void Sim1000ms(float dt)
  {
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
      prefabIdToFetchable.Value.Sim1000ms(dt);
  }

  public void UpdatePickups(PathProber path_prober, Worker worker)
  {
    Navigator component = worker.GetComponent<Navigator>();
    this.updatePickupsWorkItems.Reset((object) null);
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
    {
      FetchManager.FetchablesByPrefabId fetchablesByPrefabId = prefabIdToFetchable.Value;
      fetchablesByPrefabId.UpdateOffsetTables();
      this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem()
      {
        fetchablesByPrefabId = fetchablesByPrefabId,
        pathProber = path_prober,
        navigator = component,
        worker = worker.gameObject
      });
    }
    OffsetTracker.isExecutingWithinJob = true;
    GlobalJobManager.Run((IWorkItemCollection) this.updatePickupsWorkItems);
    OffsetTracker.isExecutingWithinJob = false;
    this.pickups.Clear();
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
      this.pickups.AddRange((IEnumerable<FetchManager.Pickup>) prefabIdToFetchable.Value.finalPickups);
    this.pickups.Sort((IComparer<FetchManager.Pickup>) FetchManager.ComparerNoPriority);
  }

  public static bool IsFetchablePickup(
    KPrefabID pickup_id,
    Storage source,
    float pickup_unreserved_amount,
    ref TagBits tag_bits,
    ref TagBits required_tags,
    ref TagBits forbid_tags,
    Storage destination)
  {
    if ((double) pickup_unreserved_amount <= 0.0 || (Object) pickup_id == (Object) null)
      return false;
    pickup_id.UpdateTagBits();
    return pickup_id.HasAnyTags_AssumeLaundered(ref tag_bits) && pickup_id.HasAllTags_AssumeLaundered(ref required_tags) && !pickup_id.HasAnyTags_AssumeLaundered(ref forbid_tags) && (!((Object) source != (Object) null) || (source.ignoreSourcePriority || !destination.ShouldOnlyTransferFromLowerPriority || !(destination.masterPriority <= source.masterPriority)) && (destination.storageNetworkID == -1 || destination.storageNetworkID != source.storageNetworkID));
  }

  public static bool IsFetchablePickup(
    Pickupable pickupable,
    ref TagBits tag_bits,
    ref TagBits required_tags,
    ref TagBits forbid_tags,
    Storage destination)
  {
    return FetchManager.IsFetchablePickup(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, ref tag_bits, ref required_tags, ref forbid_tags, destination);
  }

  public static Pickupable FindFetchTarget(
    List<Pickupable> pickupables,
    Storage destination,
    ref TagBits tag_bits,
    ref TagBits required_tags,
    ref TagBits forbid_tags,
    float required_amount)
  {
    foreach (Pickupable pickupable in pickupables)
    {
      if (FetchManager.IsFetchablePickup(pickupable, ref tag_bits, ref required_tags, ref forbid_tags, destination))
        return pickupable;
    }
    return (Pickupable) null;
  }

  public Pickupable FindFetchTarget(
    Storage destination,
    ref TagBits tag_bits,
    ref TagBits required_tags,
    ref TagBits forbid_tags,
    float required_amount)
  {
    foreach (FetchManager.Pickup pickup in this.pickups)
    {
      if (FetchManager.IsFetchablePickup(pickup.pickupable, ref tag_bits, ref required_tags, ref forbid_tags, destination))
        return pickup.pickupable;
    }
    return (Pickupable) null;
  }

  public Pickupable FindEdibleFetchTarget(
    Storage destination,
    ref TagBits tag_bits,
    ref TagBits required_tags,
    ref TagBits forbid_tags,
    float required_amount)
  {
    FetchManager.Pickup pickup1 = new FetchManager.Pickup()
    {
      PathCost = ushort.MaxValue,
      foodQuality = 0
    };
    int num1 = int.MaxValue;
    foreach (FetchManager.Pickup pickup2 in this.pickups)
    {
      if (FetchManager.IsFetchablePickup(pickup2.pickupable, ref tag_bits, ref required_tags, ref forbid_tags, destination))
      {
        int num2 = (int) pickup2.PathCost + (5 - (int) pickup2.foodQuality) * 50;
        if (num2 < num1)
        {
          pickup1 = pickup2;
          num1 = num2;
        }
      }
    }
    return pickup1.pickupable;
  }

  public struct Fetchable
  {
    public Pickupable pickupable;
    public int tagBitsHash;
    public byte masterPriority;
    public byte freshness;
    public byte foodQuality;
  }

  [DebuggerDisplay("{pickupable.name}")]
  public struct Pickup
  {
    public Pickupable pickupable;
    public int tagBitsHash;
    public ushort PathCost;
    public byte masterPriority;
    public byte freshness;
    public byte foodQuality;
  }

  private class PickupComparerIncludingPriority : IComparer<FetchManager.Pickup>
  {
    public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
    {
      int num = a.tagBitsHash - b.tagBitsHash;
      if (num != 0)
        return num;
      if ((int) a.masterPriority != (int) b.masterPriority)
        return (int) b.masterPriority - (int) a.masterPriority;
      if ((int) a.PathCost != (int) b.PathCost)
        return (int) a.PathCost - (int) b.PathCost;
      if ((int) a.foodQuality != (int) b.foodQuality)
        return (int) b.foodQuality - (int) a.foodQuality;
      return (int) b.freshness - (int) a.freshness;
    }
  }

  private class PickupComparerNoPriority : IComparer<FetchManager.Pickup>
  {
    public int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
    {
      if ((int) a.PathCost != (int) b.PathCost)
        return (int) a.PathCost - (int) b.PathCost;
      if ((int) a.foodQuality != (int) b.foodQuality)
        return (int) b.foodQuality - (int) a.foodQuality;
      return (int) b.freshness - (int) a.freshness;
    }
  }

  public class FetchablesByPrefabId
  {
    public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();
    private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();
    private Dictionary<int, int> cellCosts = new Dictionary<int, int>();
    public KCompactedVector<FetchManager.Fetchable> fetchables;
    private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;

    public FetchablesByPrefabId(Tag prefab_id)
    {
      this.prefabId = prefab_id;
      this.fetchables = new KCompactedVector<FetchManager.Fetchable>(0);
      this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
      this.finalPickups = new List<FetchManager.Pickup>();
    }

    public Tag prefabId { get; private set; }

    public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
    {
      byte num1 = 5;
      Edible component1 = pickupable.GetComponent<Edible>();
      if ((Object) component1 != (Object) null)
        num1 = (byte) component1.GetQuality();
      byte num2 = 0;
      if ((Object) pickupable.storage != (Object) null)
      {
        Prioritizable prioritizable = pickupable.storage.prioritizable;
        if ((Object) prioritizable != (Object) null)
          num2 = (byte) prioritizable.GetMasterPriority().priority_value;
      }
      Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
      byte num3 = 0;
      if (!smi.IsNullOrStopped())
        num3 = FetchManager.FetchablesByPrefabId.QuantizeRotValue(smi.RotValue);
      KPrefabID component2 = pickupable.GetComponent<KPrefabID>();
      TagBits rhs = new TagBits(ref FetchManager.disallowedTagMask);
      component2.AndTagBits(ref rhs);
      HandleVector<int>.Handle index = this.fetchables.Allocate(new FetchManager.Fetchable()
      {
        pickupable = pickupable,
        foodQuality = num1,
        freshness = num3,
        masterPriority = num2,
        tagBitsHash = rhs.GetHashCode()
      });
      if (!smi.IsNullOrStopped())
        this.rotUpdaters[index] = smi;
      return index;
    }

    public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
    {
      this.fetchables.Free(fetchable_handle);
      this.rotUpdaters.Remove(fetchable_handle);
    }

    public void UpdatePickups(
      PathProber path_prober,
      Navigator worker_navigator,
      GameObject worker_go)
    {
      this.GatherPickupablesWhichCanBePickedUp(worker_go);
      this.GatherReachablePickups(worker_navigator);
      this.finalPickups.Sort((IComparer<FetchManager.Pickup>) FetchManager.ComparerIncludingPriority);
      if (this.finalPickups.Count <= 0)
        return;
      FetchManager.Pickup pickup = this.finalPickups[0];
      TagBits tagBits = new TagBits(ref FetchManager.disallowedTagMask);
      pickup.pickupable.KPrefabID.AndTagBits(ref tagBits);
      int num = pickup.tagBitsHash;
      int count = this.finalPickups.Count;
      int index1 = 0;
      for (int index2 = 1; index2 < this.finalPickups.Count; ++index2)
      {
        bool flag = false;
        FetchManager.Pickup finalPickup = this.finalPickups[index2];
        TagBits rhs = new TagBits();
        int tagBitsHash = finalPickup.tagBitsHash;
        if ((int) pickup.masterPriority == (int) finalPickup.masterPriority)
        {
          rhs = new TagBits(ref FetchManager.disallowedTagMask);
          finalPickup.pickupable.KPrefabID.AndTagBits(ref rhs);
          if (finalPickup.tagBitsHash == num && rhs.AreEqual(ref tagBits))
            flag = true;
        }
        if (flag)
        {
          --count;
        }
        else
        {
          ++index1;
          pickup = finalPickup;
          tagBits = rhs;
          num = tagBitsHash;
          if (index2 > index1)
            this.finalPickups[index1] = finalPickup;
        }
      }
      this.finalPickups.RemoveRange(count, this.finalPickups.Count - count);
    }

    private void GatherPickupablesWhichCanBePickedUp(GameObject worker_go)
    {
      this.pickupsWhichCanBePickedUp.Clear();
      foreach (FetchManager.Fetchable data in this.fetchables.GetDataList())
      {
        Pickupable pickupable = data.pickupable;
        if (pickupable.CouldBePickedUpByMinion(worker_go))
          this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup()
          {
            pickupable = pickupable,
            tagBitsHash = data.tagBitsHash,
            PathCost = ushort.MaxValue,
            masterPriority = data.masterPriority,
            freshness = data.freshness,
            foodQuality = data.foodQuality
          });
      }
    }

    public void UpdateOffsetTables()
    {
      foreach (FetchManager.Fetchable data in this.fetchables.GetDataList())
        data.pickupable.GetOffsets(data.pickupable.cachedCell);
    }

    private void GatherReachablePickups(Navigator navigator)
    {
      this.cellCosts.Clear();
      this.finalPickups.Clear();
      foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
      {
        Pickupable pickupable = pickup.pickupable;
        int num = -1;
        if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
        {
          num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
          this.cellCosts[pickupable.cachedCell] = num;
        }
        if (num != -1)
          this.finalPickups.Add(new FetchManager.Pickup()
          {
            pickupable = pickupable,
            tagBitsHash = pickup.tagBitsHash,
            PathCost = (ushort) num,
            masterPriority = pickup.masterPriority,
            freshness = pickup.freshness,
            foodQuality = pickup.foodQuality
          });
      }
    }

    public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
    {
      FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
      byte num = 0;
      Pickupable pickupable = data.pickupable;
      if ((Object) pickupable.storage != (Object) null)
      {
        Prioritizable prioritizable = pickupable.storage.prioritizable;
        if ((Object) prioritizable != (Object) null)
          num = (byte) prioritizable.GetMasterPriority().priority_value;
      }
      data.masterPriority = num;
      this.fetchables.SetData(fetchable_handle, data);
    }

    public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
    {
      FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
      TagBits rhs = new TagBits(ref FetchManager.disallowedTagMask);
      data.pickupable.KPrefabID.AndTagBits(ref rhs);
      data.tagBitsHash = rhs.GetHashCode();
      this.fetchables.SetData(fetchable_handle, data);
    }

    public void Sim1000ms(float dt)
    {
      foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> rotUpdater in this.rotUpdaters)
      {
        HandleVector<int>.Handle key = rotUpdater.Key;
        Rottable.Instance instance = rotUpdater.Value;
        FetchManager.Fetchable data = this.fetchables.GetData(key);
        data.freshness = FetchManager.FetchablesByPrefabId.QuantizeRotValue(instance.RotValue);
        this.fetchables.SetData(key, data);
      }
    }

    private static byte QuantizeRotValue(float rot_value)
    {
      return (byte) (4.0 * (double) rot_value);
    }
  }

  private struct UpdatePickupWorkItem : IWorkItem<object>
  {
    public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
    public PathProber pathProber;
    public Navigator navigator;
    public GameObject worker;

    public void Run(object shared_data)
    {
      this.fetchablesByPrefabId.UpdatePickups(this.pathProber, this.navigator, this.worker);
    }
  }
}
