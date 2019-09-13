// Decompiled with JetBrains decompiler
// Type: FetchAreaChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FetchAreaChore : Chore<FetchAreaChore.StatesInstance>
{
  public FetchAreaChore(Chore.Precondition.Context context)
    : base(context.chore.choreType, (IStateMachineTarget) context.consumerState.consumer, context.consumerState.choreProvider, false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, context.masterPriority.priority_class, context.masterPriority.priority_value, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new FetchAreaChore.StatesInstance(this, context);
  }

  public bool IsFetching
  {
    get
    {
      return this.smi.pickingup;
    }
  }

  public bool IsDelivering
  {
    get
    {
      return this.smi.delivering;
    }
  }

  public GameObject GetFetchTarget
  {
    get
    {
      return this.smi.sm.fetchTarget.Get(this.smi);
    }
  }

  public override void Cleanup()
  {
    base.Cleanup();
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.Begin(context);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    this.smi.End();
    base.End(reason);
  }

  private void OnTagsChanged(object data)
  {
    if (!((UnityEngine.Object) this.smi.sm.fetchTarget.Get(this.smi) != (UnityEngine.Object) null))
      return;
    this.Fail("Tags changed");
  }

  public static void GatherNearbyFetchChores(
    FetchChore root_chore,
    Chore.Precondition.Context context,
    int x,
    int y,
    int radius,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> failed_contexts)
  {
    ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x - radius, y - radius, radius * 2 + 1, radius * 2 + 1, GameScenePartitioner.Instance.fetchChoreLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
      (pooledList[index].obj as FetchChore).CollectChoresFromGlobalChoreProvider(context.consumerState, succeeded_contexts, failed_contexts, true);
    pooledList.Recycle();
  }

  public class StatesInstance : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.GameInstance
  {
    private List<FetchChore> chores = new List<FetchChore>();
    private List<Pickupable> fetchables = new List<Pickupable>();
    private List<FetchAreaChore.StatesInstance.Reservation> reservations = new List<FetchAreaChore.StatesInstance.Reservation>();
    private List<Pickupable> deliverables = new List<Pickupable>();
    public List<FetchAreaChore.StatesInstance.Delivery> deliveries = new List<FetchAreaChore.StatesInstance.Delivery>();
    private FetchChore rootChore;
    private Chore.Precondition.Context rootContext;
    private float fetchAmountRequested;
    public bool delivering;
    public bool pickingup;

    public StatesInstance(FetchAreaChore master, Chore.Precondition.Context context)
      : base(master)
    {
      this.rootContext = context;
      this.rootChore = context.chore as FetchChore;
    }

    public void Begin(Chore.Precondition.Context context)
    {
      this.sm.fetcher.Set(context.consumerState.gameObject, this.smi);
      this.chores.Clear();
      this.chores.Add(this.rootChore);
      int x1;
      int y1;
      Grid.CellToXY(Grid.PosToCell(this.rootChore.destination.transform.GetPosition()), out x1, out y1);
      ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList1 = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
      ListPool<Chore.Precondition.Context, FetchAreaChore>.PooledList pooledList2 = ListPool<Chore.Precondition.Context, FetchAreaChore>.Allocate();
      if (this.rootChore.allowMultifetch)
        FetchAreaChore.GatherNearbyFetchChores(this.rootChore, context, x1, y1, 3, (List<Chore.Precondition.Context>) pooledList1, (List<Chore.Precondition.Context>) pooledList2);
      float a1 = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup((Component) context.consumerState.consumer).GetTotalValue());
      Pickupable pickupable1 = context.data as Pickupable;
      if ((UnityEngine.Object) pickupable1 == (UnityEngine.Object) null)
      {
        Debug.Assert(pooledList1.Count > 0, (object) "succeeded_contexts was empty");
        FetchChore chore = (FetchChore) pooledList1[0].chore;
        Debug.Assert(chore != null, (object) "fetch_chore was null");
        DebugUtil.LogWarningArgs((object) "Missing root_fetchable for FetchAreaChore", (object) chore.destination, (object) chore.tags[0]);
        pickupable1 = chore.FindFetchTarget(context.consumerState);
      }
      Debug.Assert((UnityEngine.Object) pickupable1 != (UnityEngine.Object) null, (object) "root_fetchable was null");
      List<Pickupable> pickupableList = new List<Pickupable>();
      pickupableList.Add(pickupable1);
      float unreservedAmount1 = pickupable1.UnreservedAmount;
      float minTakeAmount = pickupable1.MinTakeAmount;
      int x2 = 0;
      int y2 = 0;
      Grid.CellToXY(Grid.PosToCell(pickupable1.transform.GetPosition()), out x2, out y2);
      int num1 = 6;
      int x_bottomLeft = x2 - num1 / 2;
      int y_bottomLeft = y2 - num1 / 2;
      ListPool<ScenePartitionerEntry, FetchAreaChore>.PooledList pooledList3 = ListPool<ScenePartitionerEntry, FetchAreaChore>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, num1, num1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList3);
      Tag prefabTag = pickupable1.GetComponent<KPrefabID>().PrefabTag;
      for (int index = 0; index < pooledList3.Count; ++index)
      {
        ScenePartitionerEntry partitionerEntry = pooledList3[index];
        if ((double) unreservedAmount1 <= (double) a1)
        {
          Pickupable pickupable2 = partitionerEntry.obj as Pickupable;
          KPrefabID component = pickupable2.GetComponent<KPrefabID>();
          if (!(component.PrefabTag != prefabTag) && (double) pickupable2.UnreservedAmount > 0.0)
          {
            component.UpdateTagBits();
            if (component.HasAllTags_AssumeLaundered(ref this.rootChore.requiredTagBits) && !component.HasAnyTags_AssumeLaundered(ref this.rootChore.forbiddenTagBits) && (!pickupableList.Contains(pickupable2) && this.rootContext.consumerState.consumer.CanReach((IApproachable) pickupable2)))
            {
              float unreservedAmount2 = pickupable2.UnreservedAmount;
              pickupableList.Add(pickupable2);
              unreservedAmount1 += unreservedAmount2;
              if (pickupableList.Count >= 10)
                break;
            }
          }
        }
        else
          break;
      }
      pooledList3.Recycle();
      float b = Mathf.Min(a1, unreservedAmount1);
      if ((double) minTakeAmount > 0.0)
        b -= b % minTakeAmount;
      this.deliveries.Clear();
      float amount_to_be_fetched1 = Mathf.Min(this.rootChore.originalAmount, b);
      if ((double) minTakeAmount > 0.0)
        amount_to_be_fetched1 -= amount_to_be_fetched1 % minTakeAmount;
      this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(this.rootContext, amount_to_be_fetched1, new System.Action<FetchChore>(this.OnFetchChoreCancelled)));
      float a2 = amount_to_be_fetched1;
      for (int index = 0; index < pooledList1.Count && (double) a2 < (double) b; ++index)
      {
        Chore.Precondition.Context context1 = pooledList1[index];
        FetchChore chore = context1.chore as FetchChore;
        if (chore != this.rootChore && context1.IsSuccess() && ((UnityEngine.Object) chore.overrideTarget == (UnityEngine.Object) null && (UnityEngine.Object) chore.driver == (UnityEngine.Object) null) && chore.tagBits.AreEqual(ref this.rootChore.tagBits))
        {
          float amount_to_be_fetched2 = Mathf.Min(chore.originalAmount, b - a2);
          if ((double) minTakeAmount > 0.0)
            amount_to_be_fetched2 -= amount_to_be_fetched2 % minTakeAmount;
          this.chores.Add(chore);
          this.deliveries.Add(new FetchAreaChore.StatesInstance.Delivery(context1, amount_to_be_fetched2, new System.Action<FetchChore>(this.OnFetchChoreCancelled)));
          a2 += amount_to_be_fetched2;
          if (this.deliveries.Count >= 10)
            break;
        }
      }
      float num2 = Mathf.Min(a2, b);
      float num3 = num2;
      this.fetchables.Clear();
      for (int index = 0; index < pickupableList.Count && (double) num3 > 0.0; ++index)
      {
        Pickupable pickupable2 = pickupableList[index];
        num3 -= pickupable2.UnreservedAmount;
        this.fetchables.Add(pickupable2);
      }
      this.fetchAmountRequested = num2;
      this.reservations.Clear();
      pooledList1.Recycle();
      pooledList2.Recycle();
    }

    public void End()
    {
      foreach (FetchAreaChore.StatesInstance.Delivery delivery in this.deliveries)
        delivery.Cleanup();
      this.deliveries.Clear();
    }

    public void SetupDelivery()
    {
      this.deliverables.RemoveAll((Predicate<Pickupable>) (x =>
      {
        if (!((UnityEngine.Object) x == (UnityEngine.Object) null))
          return (double) x.TotalAmount <= 0.0;
        return true;
      }));
      if (this.deliveries.Count > 0 && this.deliverables.Count > 0)
      {
        this.sm.deliveryDestination.Set((KMonoBehaviour) this.deliveries[0].destination, this.smi);
        this.sm.deliveryObject.Set((KMonoBehaviour) this.deliverables[0], this.smi);
        if ((UnityEngine.Object) this.deliveries[0].destination != (UnityEngine.Object) null)
        {
          if (this.rootContext.consumerState.hasSolidTransferArm)
          {
            if (this.rootContext.consumerState.consumer.IsWithinReach((IApproachable) this.deliveries[0].destination))
              this.GoTo((StateMachine.BaseState) this.sm.delivering.storing);
            else
              this.GoTo((StateMachine.BaseState) this.sm.delivering.deliverfail);
          }
          else
            this.GoTo((StateMachine.BaseState) this.sm.delivering.movetostorage);
        }
        else
          this.smi.GoTo((StateMachine.BaseState) this.sm.delivering.deliverfail);
      }
      else
        this.StopSM("FetchAreaChoreComplete");
    }

    public void SetupFetch()
    {
      if (this.reservations.Count > 0)
      {
        this.sm.fetchTarget.Set((KMonoBehaviour) this.reservations[0].pickupable, this.smi);
        this.sm.fetchResultTarget.Set((KMonoBehaviour) null, this.smi);
        double num = (double) this.sm.fetchAmount.Set(this.reservations[0].amount, this.smi);
        if ((UnityEngine.Object) this.reservations[0].pickupable != (UnityEngine.Object) null)
        {
          if (this.rootContext.consumerState.hasSolidTransferArm)
          {
            if (this.rootContext.consumerState.consumer.IsWithinReach((IApproachable) this.reservations[0].pickupable))
              this.GoTo((StateMachine.BaseState) this.sm.fetching.pickup);
            else
              this.GoTo((StateMachine.BaseState) this.sm.fetching.fetchfail);
          }
          else
            this.GoTo((StateMachine.BaseState) this.sm.fetching.movetopickupable);
        }
        else
          this.GoTo((StateMachine.BaseState) this.sm.fetching.fetchfail);
      }
      else
        this.GoTo((StateMachine.BaseState) this.sm.delivering.next);
    }

    public void DeliverFail()
    {
      if (this.deliveries.Count > 0)
      {
        this.deliveries[0].Cleanup();
        this.deliveries.RemoveAt(0);
      }
      this.GoTo((StateMachine.BaseState) this.sm.delivering.next);
    }

    public void DeliverComplete()
    {
      Pickupable pickupable = this.sm.deliveryObject.Get<Pickupable>(this.smi);
      if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null || (double) pickupable.TotalAmount <= 0.0)
      {
        if (this.deliveries.Count > 0 && (double) this.deliveries[0].chore.amount < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
        {
          FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
          Chore chore = (Chore) delivery.chore;
          delivery.Complete(this.deliverables);
          delivery.Cleanup();
          if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
            this.deliveries.RemoveAt(0);
          this.GoTo((StateMachine.BaseState) this.sm.delivering.next);
        }
        else
          this.smi.GoTo((StateMachine.BaseState) this.sm.delivering.deliverfail);
      }
      else
      {
        if (this.deliveries.Count > 0)
        {
          FetchAreaChore.StatesInstance.Delivery delivery = this.deliveries[0];
          Chore chore = (Chore) delivery.chore;
          delivery.Complete(this.deliverables);
          delivery.Cleanup();
          if (this.deliveries.Count > 0 && this.deliveries[0].chore == chore)
            this.deliveries.RemoveAt(0);
        }
        this.GoTo((StateMachine.BaseState) this.sm.delivering.next);
      }
    }

    public void FetchFail()
    {
      this.reservations[0].Cleanup();
      this.reservations.RemoveAt(0);
      this.GoTo((StateMachine.BaseState) this.sm.fetching.next);
    }

    public void FetchComplete()
    {
      this.reservations[0].Cleanup();
      this.reservations.RemoveAt(0);
      this.GoTo((StateMachine.BaseState) this.sm.fetching.next);
    }

    public void SetupDeliverables()
    {
      foreach (GameObject gameObject in this.sm.fetcher.Get<Storage>(this.smi).items)
      {
        if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        {
          KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
          if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
          {
            Pickupable component2 = component1.GetComponent<Pickupable>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              this.deliverables.Add(component2);
          }
        }
      }
    }

    public void ReservePickupables()
    {
      ChoreConsumer consumer = this.sm.fetcher.Get<ChoreConsumer>(this.smi);
      float fetchAmountRequested = this.fetchAmountRequested;
      foreach (Pickupable fetchable in this.fetchables)
      {
        if ((double) fetchAmountRequested <= 0.0)
          break;
        float reservation_amount = Math.Min(fetchAmountRequested, fetchable.UnreservedAmount);
        fetchAmountRequested -= reservation_amount;
        this.reservations.Add(new FetchAreaChore.StatesInstance.Reservation(consumer, fetchable, reservation_amount));
      }
    }

    private void OnFetchChoreCancelled(FetchChore chore)
    {
      for (int index = 0; index < this.deliveries.Count; ++index)
      {
        if (this.deliveries[index].chore == chore)
        {
          if (this.deliveries.Count == 1)
          {
            this.StopSM("AllDelivericesCancelled");
            break;
          }
          if (index == 0)
          {
            this.sm.currentdeliverycancelled.Trigger(this);
            break;
          }
          this.deliveries[index].Cleanup();
          this.deliveries.RemoveAt(index);
          break;
        }
      }
    }

    public void UnreservePickupables()
    {
      foreach (FetchAreaChore.StatesInstance.Reservation reservation in this.reservations)
        reservation.Cleanup();
      this.reservations.Clear();
    }

    public bool SameDestination(FetchChore fetch)
    {
      foreach (FetchChore chore in this.chores)
      {
        if ((UnityEngine.Object) chore.destination == (UnityEngine.Object) fetch.destination)
          return true;
      }
      return false;
    }

    public struct Delivery
    {
      private System.Action<FetchChore> onCancelled;
      private System.Action<Chore> onFetchChoreCleanup;

      public unsafe Delivery(
        Chore.Precondition.Context context,
        float amount_to_be_fetched,
        System.Action<FetchChore> on_cancelled)
      {
        *(FetchAreaChore.StatesInstance.Delivery*) ref this = new FetchAreaChore.StatesInstance.Delivery();
        this.chore = context.chore as FetchChore;
        this.amount = this.chore.originalAmount;
        this.destination = this.chore.destination;
        this.chore.SetOverrideTarget(context.consumerState.consumer);
        this.onCancelled = on_cancelled;
        this.onFetchChoreCleanup = new System.Action<Chore>(this.OnFetchChoreCleanup);
        this.chore.FetchAreaBegin(context, amount_to_be_fetched);
        FetchChore chore = this.chore;
        chore.onCleanup = chore.onCleanup + this.onFetchChoreCleanup;
      }

      public Storage destination { get; private set; }

      public float amount { get; private set; }

      public FetchChore chore { get; private set; }

      public void Complete(List<Pickupable> deliverables)
      {
        using (new KProfiler.Region("FAC.Delivery.Complete", (UnityEngine.Object) null))
        {
          if ((UnityEngine.Object) this.destination == (UnityEngine.Object) null || this.destination.IsEndOfLife())
            return;
          FetchChore chore = this.chore;
          chore.onCleanup = chore.onCleanup - this.onFetchChoreCleanup;
          float amount = this.amount;
          Pickupable pickupable1 = (Pickupable) null;
          for (int index = 0; index < deliverables.Count && (double) amount > 0.0; ++index)
          {
            if ((UnityEngine.Object) deliverables[index] == (UnityEngine.Object) null)
            {
              if ((double) amount < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
                this.destination.ForceStore(this.chore.tags[0], amount);
            }
            else
            {
              Pickupable pickupable2 = deliverables[index].Take(amount);
              if ((UnityEngine.Object) pickupable2 != (UnityEngine.Object) null && (double) pickupable2.TotalAmount > 0.0)
              {
                amount -= pickupable2.TotalAmount;
                this.destination.Store(pickupable2.gameObject, false, false, true, false);
                pickupable1 = pickupable2;
                if ((UnityEngine.Object) pickupable2 == (UnityEngine.Object) deliverables[index])
                  deliverables[index] = (Pickupable) null;
              }
            }
          }
          if ((UnityEngine.Object) this.chore.overrideTarget != (UnityEngine.Object) null)
            this.chore.FetchAreaEnd(this.chore.overrideTarget.GetComponent<ChoreDriver>(), pickupable1, true);
          this.chore = (FetchChore) null;
        }
      }

      private void OnFetchChoreCleanup(Chore chore)
      {
        if (this.onCancelled == null)
          return;
        this.onCancelled(chore as FetchChore);
      }

      public void Cleanup()
      {
        if (this.chore == null)
          return;
        FetchChore chore = this.chore;
        chore.onCleanup = chore.onCleanup - this.onFetchChoreCleanup;
        this.chore.FetchAreaEnd((ChoreDriver) null, (Pickupable) null, false);
      }
    }

    public struct Reservation
    {
      private int handle;

      public unsafe Reservation(
        ChoreConsumer consumer,
        Pickupable pickupable,
        float reservation_amount)
      {
        *(FetchAreaChore.StatesInstance.Reservation*) ref this = new FetchAreaChore.StatesInstance.Reservation();
        if ((double) reservation_amount <= 0.0)
          Debug.LogError((object) ("Invalid amount: " + (object) reservation_amount));
        this.amount = reservation_amount;
        this.pickupable = pickupable;
        this.handle = pickupable.Reserve(nameof (FetchAreaChore), consumer.gameObject, reservation_amount);
      }

      public float amount { get; private set; }

      public Pickupable pickupable { get; private set; }

      public void Cleanup()
      {
        if (!((UnityEngine.Object) this.pickupable != (UnityEngine.Object) null))
          return;
        this.pickupable.Unreserve(nameof (FetchAreaChore), this.handle);
      }
    }
  }

  public class States : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore>
  {
    public FetchAreaChore.States.FetchStates fetching;
    public FetchAreaChore.States.DeliverStates delivering;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchResultTarget;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter fetchAmount;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryObject;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.FloatParameter deliveryAmount;
    public StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.Signal currentdeliverycancelled;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetching;
      this.Target(this.fetcher);
      this.fetching.DefaultState(this.fetching.next).Enter("ReservePickupables", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.ReservePickupables())).Exit("UnreservePickupables", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.UnreservePickupables())).Enter("pickingup-on", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.pickingup = true)).Exit("pickingup-off", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.pickingup = false));
      this.fetching.next.Enter("SetupFetch", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.SetupFetch()));
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable = this.fetching.movetopickupable;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher1 = this.fetcher;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetchTarget = this.fetchTarget;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup = this.fetching.pickup;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail = this.fetching.fetchfail;
      NavTactic reduceTravelDistance1 = NavigationTactics.ReduceTravelDistance;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter mover1 = fetcher1;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter move_target1 = fetchTarget;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State success_state1 = pickup;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State failure_state1 = fetchfail;
      NavTactic tactic1 = reduceTravelDistance1;
      movetopickupable.InitializeStates(mover1, move_target1, success_state1, failure_state1, (CellOffset[]) null, tactic1);
      this.fetching.pickup.DoPickup(this.fetchTarget, this.fetchResultTarget, this.fetchAmount, this.fetching.fetchcomplete, this.fetching.fetchfail);
      this.fetching.fetchcomplete.Enter((StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.FetchComplete()));
      this.fetching.fetchfail.Enter((StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.FetchFail()));
      this.delivering.DefaultState(this.delivering.next).OnSignal(this.currentdeliverycancelled, this.delivering.deliverfail).Enter("SetupDeliverables", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.SetupDeliverables())).Enter("delivering-on", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.delivering = true)).Exit("delivering-off", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.delivering = false));
      this.delivering.next.Enter("SetupDelivery", (StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.SetupDelivery()));
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage = this.delivering.movetostorage;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter fetcher2 = this.fetcher;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter deliveryDestination = this.deliveryDestination;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing = this.delivering.storing;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail = this.delivering.deliverfail;
      NavTactic reduceTravelDistance2 = NavigationTactics.ReduceTravelDistance;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter mover2 = fetcher2;
      StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.TargetParameter move_target2 = deliveryDestination;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State success_state2 = storing;
      GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State failure_state2 = deliverfail;
      NavTactic tactic2 = reduceTravelDistance2;
      movetostorage.InitializeStates(mover2, move_target2, success_state2, failure_state2, (CellOffset[]) null, tactic2).Enter((StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) this.deliveryObject.Get(smi) != (UnityEngine.Object) null) || !((UnityEngine.Object) this.deliveryObject.Get(smi).GetComponent<MinionIdentity>() != (UnityEngine.Object) null))
          return;
        this.deliveryObject.Get(smi).transform.SetLocalPosition(Vector3.zero);
        KBatchedAnimTracker component = this.deliveryObject.Get(smi).GetComponent<KBatchedAnimTracker>();
        component.symbol = new HashedString("snapTo_chest");
        component.offset = new Vector3(0.0f, 0.0f, 1f);
      }));
      this.delivering.storing.DoDelivery(this.fetcher, this.deliveryDestination, this.delivering.delivercomplete, this.delivering.deliverfail);
      this.delivering.deliverfail.Enter((StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.DeliverFail()));
      this.delivering.delivercomplete.Enter((StateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State.Callback) (smi => smi.DeliverComplete()));
    }

    public class FetchStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
    {
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Pickupable> movetopickupable;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State pickup;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchfail;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State fetchcomplete;
    }

    public class DeliverStates : GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State
    {
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State next;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.ApproachSubState<Storage> movetostorage;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State storing;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State deliverfail;
      public GameStateMachine<FetchAreaChore.States, FetchAreaChore.StatesInstance, FetchAreaChore, object>.State delivercomplete;
    }
  }
}
