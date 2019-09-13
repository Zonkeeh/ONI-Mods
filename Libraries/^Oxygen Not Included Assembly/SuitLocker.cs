// Decompiled with JetBrains decompiler
// Type: SuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
  [MyCmpGet]
  private Building building;
  public Tag[] OutfitTags;
  private FetchChore fetchChore;
  [MyCmpAdd]
  public SuitLocker.ReturnSuitWorkable returnSuitWorkable;
  private MeterController meter;
  private SuitLocker.SuitMarkerState suitMarkerState;

  public float OxygenAvailable
  {
    get
    {
      GameObject oxygen = this.GetOxygen();
      float num = 0.0f;
      if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
        num = Math.Min(oxygen.GetComponent<PrimaryElement>().Mass / this.GetComponent<ConduitConsumer>().capacityKG, 1f);
      return num;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.smi.StartSM();
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
  }

  public KPrefabID GetStoredOutfit()
  {
    foreach (GameObject gameObject in this.GetComponent<Storage>().items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.HasAnyTags(this.OutfitTags))
          return component;
      }
    }
    return (KPrefabID) null;
  }

  public float GetSuitScore()
  {
    float num = -1f;
    KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
    if ((bool) ((UnityEngine.Object) partiallyChargedOutfit))
    {
      num = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
      JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
      if ((bool) ((UnityEngine.Object) component) && (double) component.PercentFull() < (double) num)
        num = component.PercentFull();
    }
    return num;
  }

  public KPrefabID GetPartiallyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) ((UnityEngine.Object) storedOutfit))
      return (KPrefabID) null;
    if ((double) storedOutfit.GetComponent<SuitTank>().PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    if ((bool) ((UnityEngine.Object) component) && (double) component.PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
      return (KPrefabID) null;
    return storedOutfit;
  }

  public KPrefabID GetFullyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) ((UnityEngine.Object) storedOutfit))
      return (KPrefabID) null;
    if (!storedOutfit.GetComponent<SuitTank>().IsFull())
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    if ((bool) ((UnityEngine.Object) component) && !component.IsFull())
      return (KPrefabID) null;
    return storedOutfit;
  }

  private void CreateFetchChore()
  {
    this.fetchChore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, this.GetComponent<Storage>(), 1f, this.OutfitTags, (Tag[]) null, new Tag[1]
    {
      GameTags.Assigned
    }, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, FetchOrder2.OperationalRequirement.None, 0);
    this.fetchChore.allowMultifetch = false;
  }

  private void CancelFetchChore()
  {
    if (this.fetchChore == null)
      return;
    this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
    this.fetchChore = (FetchChore) null;
  }

  public bool HasOxygen()
  {
    GameObject oxygen = this.GetOxygen();
    if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
      return (double) oxygen.GetComponent<PrimaryElement>().Mass > 0.0;
    return false;
  }

  private void RefreshMeter()
  {
    GameObject oxygen = this.GetOxygen();
    float percent_full = 0.0f;
    if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
      percent_full = Math.Min(oxygen.GetComponent<PrimaryElement>().Mass / this.GetComponent<ConduitConsumer>().capacityKG, 1f);
    this.meter.SetPositionPercent(percent_full);
  }

  public bool IsSuitFullyCharged()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component1 = storedOutfit.GetComponent<SuitTank>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (double) component1.PercentFull() < 1.0)
      return false;
    JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
    return !((UnityEngine.Object) component2 != (UnityEngine.Object) null) || (double) component2.PercentFull() >= 1.0;
  }

  public bool IsOxygenTankFull()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return true;
    return (double) component.PercentFull() >= 1.0;
  }

  private void OnRequestOutfit()
  {
    this.smi.sm.isWaitingForSuit.Set(true, this.smi);
  }

  private void OnCancelRequest()
  {
    this.smi.sm.isWaitingForSuit.Set(false, this.smi);
  }

  public void DropSuit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
  }

  public void EquipTo(Equipment equipment)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
    storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
    storedOutfit.GetComponent<EquippableWorkable>().CancelChore();
    equipment.Equip(storedOutfit.GetComponent<Equippable>());
    this.returnSuitWorkable.CreateChore();
  }

  public void UnequipFrom(Equipment equipment)
  {
    Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
    assignable.Unassign();
    this.GetComponent<Storage>().Store(assignable.gameObject, false, false, true, false);
  }

  public void ConfigRequestSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(true, this.smi);
  }

  public void ConfigNoSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(false, this.smi);
  }

  public bool CanDropOffSuit()
  {
    if (this.smi.sm.isConfigured.Get(this.smi) && !this.smi.sm.isWaitingForSuit.Get(this.smi))
      return (UnityEngine.Object) this.GetStoredOutfit() == (UnityEngine.Object) null;
    return false;
  }

  private GameObject GetOxygen()
  {
    return this.GetComponent<Storage>().FindFirst(GameTags.Oxygen);
  }

  private void ChargeSuit(float dt)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    GameObject oxygen = this.GetOxygen();
    if ((UnityEngine.Object) oxygen == (UnityEngine.Object) null)
      return;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    float b = Mathf.Min((float) ((double) component.capacity * 15.0 * (double) dt / 600.0), component.capacity - component.amount);
    float num = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, b);
    oxygen.GetComponent<PrimaryElement>().Mass -= num;
    component.amount += num;
  }

  public void SetSuitMarker(SuitMarker suit_marker)
  {
    SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
    if ((UnityEngine.Object) suit_marker == (UnityEngine.Object) null)
      suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
    else if ((double) suit_marker.transform.GetPosition().x > (double) this.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if ((double) suit_marker.transform.GetPosition().x < (double) this.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if (!suit_marker.GetComponent<Operational>().IsOperational)
      suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
    if (suitMarkerState == this.suitMarkerState)
      return;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, false);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, false);
    switch (suitMarkerState)
    {
      case SuitLocker.SuitMarkerState.NoMarker:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, (object) null);
        break;
      case SuitLocker.SuitMarkerState.WrongSide:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, (object) null);
        break;
    }
    this.suitMarkerState = suitMarkerState;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private static void GatherSuitBuildings(
    int cell,
    int dir,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    int x = dir;
    while (true)
    {
      int cell1 = Grid.OffsetCell(cell, x, 0);
      if (!Grid.IsValidCell(cell1) || SuitLocker.GatherSuitBuildingsOnCell(cell1, suit_lockers, suit_markers))
        x += dir;
      else
        break;
    }
  }

  private static bool GatherSuitBuildingsOnCell(
    int cell,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    SuitMarker component1 = gameObject.GetComponent<SuitMarker>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      suit_markers.Add(new SuitLocker.SuitMarkerEntry()
      {
        suitMarker = component1,
        cell = cell
      });
      return true;
    }
    SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return false;
    suit_lockers.Add(new SuitLocker.SuitLockerEntry()
    {
      suitLocker = component2,
      cell = cell
    });
    return true;
  }

  private static SuitMarker FindSuitMarker(
    int cell,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    if (!Grid.IsValidCell(cell))
      return (SuitMarker) null;
    foreach (SuitLocker.SuitMarkerEntry suitMarker in suit_markers)
    {
      if (suitMarker.cell == cell)
        return suitMarker.suitMarker;
    }
    return (SuitMarker) null;
  }

  public static void UpdateSuitMarkerStates(int cell, GameObject self)
  {
    ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList1 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
    ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList2 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
    if ((UnityEngine.Object) self != (UnityEngine.Object) null)
    {
      SuitLocker component1 = self.GetComponent<SuitLocker>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        pooledList1.Add(new SuitLocker.SuitLockerEntry()
        {
          suitLocker = component1,
          cell = cell
        });
      SuitMarker component2 = self.GetComponent<SuitMarker>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        pooledList2.Add(new SuitLocker.SuitMarkerEntry()
        {
          suitMarker = component2,
          cell = cell
        });
    }
    SuitLocker.GatherSuitBuildings(cell, 1, (List<SuitLocker.SuitLockerEntry>) pooledList1, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
    SuitLocker.GatherSuitBuildings(cell, -1, (List<SuitLocker.SuitLockerEntry>) pooledList1, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
    pooledList1.Sort((IComparer<SuitLocker.SuitLockerEntry>) SuitLocker.SuitLockerEntry.comparer);
    for (int index1 = 0; index1 < pooledList1.Count; ++index1)
    {
      SuitLocker.SuitLockerEntry suitLockerEntry1 = pooledList1[index1];
      SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry1;
      ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList3 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
      pooledList3.Add(suitLockerEntry1);
      for (int index2 = index1 + 1; index2 < pooledList1.Count; ++index2)
      {
        SuitLocker.SuitLockerEntry suitLockerEntry3 = pooledList1[index2];
        if (Grid.CellRight(suitLockerEntry2.cell) == suitLockerEntry3.cell)
        {
          ++index1;
          suitLockerEntry2 = suitLockerEntry3;
          pooledList3.Add(suitLockerEntry3);
        }
        else
          break;
      }
      int cell1 = Grid.CellLeft(suitLockerEntry1.cell);
      int cell2 = Grid.CellRight(suitLockerEntry2.cell);
      SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell1, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
      if ((UnityEngine.Object) suitMarker == (UnityEngine.Object) null)
        suitMarker = SuitLocker.FindSuitMarker(cell2, (List<SuitLocker.SuitMarkerEntry>) pooledList2);
      foreach (SuitLocker.SuitLockerEntry suitLockerEntry3 in (List<SuitLocker.SuitLockerEntry>) pooledList3)
        suitLockerEntry3.suitLocker.SetSuitMarker(suitMarker);
      pooledList3.Recycle();
    }
    pooledList1.Recycle();
    pooledList2.Recycle();
  }

  public class ReturnSuitWorkable : Workable
  {
    public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingUrgent),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        if ((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null)
          return false;
        SuitTank component1 = slot.assignable.GetComponent<SuitTank>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
          return false;
        if (component1.NeedsRecharging())
          return true;
        JetSuitTank component2 = slot.assignable.GetComponent<JetSuitTank>();
        return !((UnityEngine.Object) component2 == (UnityEngine.Object) null) && component2.NeedsRecharging();
      })
    };
    public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingIdle),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        return !((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null) && !((UnityEngine.Object) slot.assignable.GetComponent<SuitTank>() == (UnityEngine.Object) null);
      })
    };
    public Chore.Precondition HasSuitMarker = new Chore.Precondition()
    {
      id = "IsValid",
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((SuitLocker) data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker)
    };
    public Chore.Precondition SuitTypeMatchesLocker = new Chore.Precondition()
    {
      id = "IsValid",
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        SuitLocker suitLocker = (SuitLocker) data;
        AssignableSlotInstance slot = context.consumerState.equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        if ((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null)
          return false;
        return (UnityEngine.Object) slot.assignable.GetComponent<JetSuitTank>() != (UnityEngine.Object) null == ((UnityEngine.Object) suitLocker.GetComponent<JetSuitLocker>() != (UnityEngine.Object) null);
      })
    };
    private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;
    private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.resetProgressOnStop = true;
      this.workTime = 0.25f;
      this.synchronizeAnims = false;
    }

    public void CreateChore()
    {
      if (this.urgentChore != null)
        return;
      SuitLocker component = this.GetComponent<SuitLocker>();
      this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
      this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent, (object) null);
      this.urgentChore.AddPrecondition(this.HasSuitMarker, (object) component);
      this.urgentChore.AddPrecondition(this.SuitTypeMatchesLocker, (object) component);
      this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.idle, 5, false, false);
      this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle, (object) null);
      this.idleChore.AddPrecondition(this.HasSuitMarker, (object) component);
      this.idleChore.AddPrecondition(this.SuitTypeMatchesLocker, (object) component);
    }

    public void CancelChore()
    {
      if (this.urgentChore != null)
      {
        this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
        this.urgentChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
      }
      if (this.idleChore == null)
        return;
      this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
      this.idleChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
    }

    protected override void OnStartWork(Worker worker)
    {
      this.ShowProgressBar(false);
    }

    protected override bool OnWorkTick(Worker worker, float dt)
    {
      return true;
    }

    protected override void OnCompleteWork(Worker worker)
    {
      Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
      if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
      {
        if (this.GetComponent<SuitLocker>().CanDropOffSuit())
          this.GetComponent<SuitLocker>().UnequipFrom(equipment);
        else
          equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
      }
      if (this.urgentChore == null)
        return;
      this.CancelChore();
      this.CreateChore();
    }

    public override HashedString[] GetWorkAnims(Worker worker)
    {
      return new HashedString[1]{ new HashedString("none") };
    }
  }

  public class StatesInstance : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance
  {
    public StatesInstance(SuitLocker suit_locker)
      : base(suit_locker)
    {
    }
  }

  public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
  {
    public SuitLocker.States.EmptyStates empty;
    public SuitLocker.States.ChargingStates charging;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = true;
      this.root.Update("RefreshMeter", (System.Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms, false);
      this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CreateChore())).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CancelChore())).PlayAnim("no_suit_pre").QueueAnim("no_suit", false, (Func<SuitLocker.StatesInstance, string>) null);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state1 = this.empty.notconfigured.ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue);
      string name1 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME;
      string tooltip1 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP;
      string str1 = "status_item_no_filter_set";
      StatusItem.IconType iconType1 = StatusItem.IconType.Custom;
      NotificationType notificationType1 = NotificationType.BadMinor;
      StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
      string name2 = name1;
      string tooltip2 = tooltip1;
      string icon1 = str1;
      int num1 = (int) iconType1;
      int num2 = (int) notificationType1;
      HashedString render_overlay1 = new HashedString();
      StatusItemCategory category1 = main1;
      state1.ToggleStatusItem(name2, tooltip2, icon1, (StatusItem.IconType) num1, (NotificationType) num2, false, render_overlay1, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category1);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state2 = this.empty.configured.RefreshUserMenuOnEnter();
      string name3 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME;
      string tooltip3 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP;
      StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
      string name4 = name3;
      string tooltip4 = tooltip3;
      string empty1 = string.Empty;
      HashedString render_overlay2 = new HashedString();
      StatusItemCategory category2 = main2;
      state2.ToggleStatusItem(name4, tooltip4, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category2);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state3 = this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).Enter("CreateFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CreateFetchChore())).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit", false, (Func<SuitLocker.StatesInstance, string>) null).Exit("ClearIsWaitingForSuit", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => this.isWaitingForSuit.Set(false, smi))).Exit("CancelFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CancelFetchChore()));
      string name5 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME;
      string tooltip5 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP;
      StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
      string name6 = name5;
      string tooltip6 = tooltip5;
      string empty2 = string.Empty;
      HashedString render_overlay3 = new HashedString();
      StatusItemCategory category3 = main3;
      state3.ToggleStatusItem(name6, tooltip6, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay3, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category3);
      this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null));
      this.charging.pre.Enter((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.IsSuitFullyCharged())
        {
          smi.GoTo((StateMachine.BaseState) this.suitfullycharged);
        }
        else
        {
          smi.GetComponent<KBatchedAnimController>().Play((HashedString) "no_suit_pst", KAnim.PlayMode.Once, 1f, 0.0f);
          smi.GetComponent<KBatchedAnimController>().Queue((HashedString) "charging_pre", KAnim.PlayMode.Once, 1f, 0.0f);
        }
      })).OnAnimQueueComplete(this.charging.operational);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state4 = this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.HasOxygen()), UpdateRate.SIM_200ms).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(true, false))).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()), UpdateRate.SIM_200ms).Update("ChargeSuit", (System.Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.ChargeSuit(dt)), UpdateRate.SIM_200ms, false).Exit("ClearActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(false, false)));
      string name7 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME;
      string tooltip7 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP;
      StatusItemCategory main4 = Db.Get().StatusItemCategories.Main;
      string name8 = name7;
      string tooltip8 = tooltip7;
      string empty3 = string.Empty;
      HashedString render_overlay4 = new HashedString();
      StatusItemCategory category4 = main4;
      state4.ToggleStatusItem(name8, tooltip8, empty3, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay4, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category4);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state5 = this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.HasOxygen()), UpdateRate.SIM_200ms).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()), UpdateRate.SIM_200ms).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop);
      string name9 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME;
      string tooltip9 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP;
      string str2 = "status_item_suit_locker_no_oxygen";
      StatusItem.IconType iconType2 = StatusItem.IconType.Custom;
      NotificationType notificationType2 = NotificationType.BadMinor;
      StatusItemCategory main5 = Db.Get().StatusItemCategories.Main;
      string name10 = name9;
      string tooltip10 = tooltip9;
      string icon2 = str2;
      int num3 = (int) iconType2;
      int num4 = (int) notificationType2;
      HashedString render_overlay5 = new HashedString();
      StatusItemCategory category5 = main5;
      state5.ToggleStatusItem(name10, tooltip10, icon2, (StatusItem.IconType) num3, (NotificationType) num4, false, render_overlay5, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category5);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state6 = this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()), UpdateRate.SIM_200ms);
      string name11 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME;
      string tooltip11 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP;
      StatusItemCategory main6 = Db.Get().StatusItemCategories.Main;
      string name12 = name11;
      string tooltip12 = tooltip11;
      string empty4 = string.Empty;
      HashedString render_overlay6 = new HashedString();
      StatusItemCategory category6 = main6;
      state6.ToggleStatusItem(name12, tooltip12, empty4, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay6, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category6);
      this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state7 = this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).PlayAnim("has_suit").RefreshUserMenuOnEnter();
      string name13 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME;
      string tooltip13 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP;
      StatusItemCategory main7 = Db.Get().StatusItemCategories.Main;
      string name14 = name13;
      string tooltip14 = tooltip13;
      string empty5 = string.Empty;
      HashedString render_overlay7 = new HashedString();
      StatusItemCategory category7 = main7;
      state7.ToggleStatusItem(name14, tooltip14, empty5, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay7, 0, (Func<string, SuitLocker.StatesInstance, string>) null, (Func<string, SuitLocker.StatesInstance, string>) null, category7);
    }

    public class ChargingStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
    }

    public class EmptyStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
    }
  }

  private enum SuitMarkerState
  {
    HasMarker,
    NoMarker,
    WrongSide,
    NotOperational,
  }

  private struct SuitLockerEntry
  {
    public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();
    public SuitLocker suitLocker;
    public int cell;

    public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
    {
      public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b)
      {
        return a.cell - b.cell;
      }
    }
  }

  private struct SuitMarkerEntry
  {
    public SuitMarker suitMarker;
    public int cell;
  }
}
