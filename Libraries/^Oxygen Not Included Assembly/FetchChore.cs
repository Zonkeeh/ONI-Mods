// Decompiled with JetBrains decompiler
// Type: FetchChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class FetchChore : Chore<FetchChore.StatesInstance>
{
  public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition()
  {
    id = nameof (IsFetchTargetAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      FetchChore chore = (FetchChore) context.chore;
      Pickupable pickupable = (Pickupable) context.data;
      bool flag;
      if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      {
        pickupable = chore.FindFetchTarget(context.consumerState);
        flag = (UnityEngine.Object) pickupable != (UnityEngine.Object) null;
      }
      else
        flag = FetchManager.IsFetchablePickup(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, ref chore.tagBits, ref chore.requiredTagBits, ref chore.forbiddenTagBits, context.consumerState.storage);
      if (flag)
      {
        if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
        {
          Debug.Log((object) string.Format("Failed to find fetch target for {0}", (object) chore.destination));
          return false;
        }
        context.data = (object) pickupable;
        int cost;
        if (context.consumerState.consumer.GetNavigationCost((IApproachable) pickupable, out cost))
        {
          context.cost += cost;
          return true;
        }
      }
      return false;
    })
  };
  public bool allowMultifetch = true;
  public Tag[] tags;
  public int tagBitsHash;
  public TagBits tagBits;
  public TagBits requiredTagBits;
  public TagBits forbiddenTagBits;
  public Automatable automatable;
  private HandleVector<int>.Handle partitionerEntry;

  public FetchChore(
    ChoreType choreType,
    Storage destination,
    float amount,
    Tag[] tags,
    Tag[] required_tags = null,
    Tag[] forbidden_tags = null,
    ChoreProvider chore_provider = null,
    bool run_until_complete = true,
    System.Action<Chore> on_complete = null,
    System.Action<Chore> on_begin = null,
    System.Action<Chore> on_end = null,
    FetchOrder2.OperationalRequirement operational_requirement = FetchOrder2.OperationalRequirement.Operational,
    int priority_mod = 0)
    : base(choreType, (IStateMachineTarget) destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
  {
    if (choreType == null)
      Debug.LogError((object) "You must specify a chore type for fetching!");
    if ((double) amount <= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
      DebugUtil.LogWarningArgs((object) string.Format("Chore {0} is requesting {1} {2} to {3}", (object) choreType.Id, (object) tags[0], (object) amount, !((UnityEngine.Object) destination != (UnityEngine.Object) null) ? (object) "to nowhere" : (object) destination.name));
    this.SetPrioritizable(!((UnityEngine.Object) destination.prioritizable != (UnityEngine.Object) null) ? destination.GetComponent<Prioritizable>() : destination.prioritizable);
    this.smi = new FetchChore.StatesInstance(this);
    double num = (double) this.smi.sm.requestedamount.Set(amount, this.smi);
    this.smi.sm.destination.Set((KMonoBehaviour) destination, this.smi);
    this.tags = tags;
    this.tagBits = new TagBits(tags);
    this.requiredTagBits = new TagBits(required_tags);
    this.forbiddenTagBits = new TagBits(forbidden_tags);
    this.tagBitsHash = this.tagBits.GetHashCode();
    DebugUtil.DevAssert(!this.tagBits.HasAny(ref FetchManager.disallowedTagBits), "Fetch chore fetching invalid tags.");
    if (destination.GetOnlyFetchMarkedItems())
      this.requiredTagBits.SetTag(GameTags.Garbage);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) destination);
    this.AddPrecondition(FetchChore.IsFetchTargetAvailable, (object) null);
    Deconstructable component1 = this.target.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) component1);
    BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) component2);
    if (operational_requirement != FetchOrder2.OperationalRequirement.None && (bool) ((UnityEngine.Object) destination.gameObject.GetComponent<Operational>()))
    {
      if (operational_requirement == FetchOrder2.OperationalRequirement.Operational)
      {
        Operational component3 = destination.GetComponent<Operational>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) component3);
      }
      if (operational_requirement == FetchOrder2.OperationalRequirement.Functional)
      {
        Operational component3 = destination.GetComponent<Operational>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          this.AddPrecondition(ChorePreconditions.instance.IsFunctional, (object) component3);
      }
    }
    this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, (object) this, Grid.PosToCell((KMonoBehaviour) destination), GameScenePartitioner.Instance.fetchChoreLayer, (System.Action<object>) null);
    destination.Subscribe(644822890, new System.Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
    this.automatable = destination.GetComponent<Automatable>();
    if (!(bool) ((UnityEngine.Object) this.automatable))
      return;
    this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, (object) this.automatable);
  }

  public float originalAmount
  {
    get
    {
      return this.smi.sm.requestedamount.Get(this.smi);
    }
  }

  public float amount
  {
    get
    {
      return this.smi.sm.actualamount.Get(this.smi);
    }
    set
    {
      double num = (double) this.smi.sm.actualamount.Set(value, this.smi);
    }
  }

  public Pickupable fetchTarget
  {
    get
    {
      return this.smi.sm.chunk.Get<Pickupable>(this.smi);
    }
    set
    {
      this.smi.sm.chunk.Set((KMonoBehaviour) value, this.smi);
    }
  }

  public GameObject fetcher
  {
    get
    {
      return this.smi.sm.fetcher.Get(this.smi);
    }
    set
    {
      this.smi.sm.fetcher.Set(value, this.smi);
    }
  }

  public Storage destination
  {
    get
    {
      return this.smi.sm.destination.Get<Storage>(this.smi);
    }
  }

  public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
  {
    this.amount = amount_to_be_fetched;
    this.smi.sm.fetcher.Set(context.consumerState.gameObject, this.smi);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName((Chore) this, context.data));
    base.Begin(context);
  }

  public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
  {
    if (is_success)
    {
      ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName((Chore) this, (object) pickupable));
      this.fetchTarget = pickupable;
      this.driver = driver;
      this.fetcher = driver.gameObject;
      this.Succeed(nameof (FetchAreaEnd));
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogFetchChore(this.fetcher, this.choreType);
    }
    else
    {
      this.SetOverrideTarget((ChoreConsumer) null);
      this.Fail("FetchAreaFail");
    }
  }

  public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
  {
    Pickupable target = (Pickupable) null;
    if ((UnityEngine.Object) this.destination != (UnityEngine.Object) null)
    {
      if (consumer_state.hasSolidTransferArm)
        consumer_state.solidTransferArm.FindFetchTarget(this.destination, this.tagBits, this.requiredTagBits, this.forbiddenTagBits, this.originalAmount, ref target);
      else
        target = Game.Instance.fetchManager.FindFetchTarget(this.destination, ref this.tagBits, ref this.requiredTagBits, ref this.forbiddenTagBits, this.originalAmount);
    }
    return target;
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    Pickupable pickupable = (Pickupable) context.data;
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      pickupable = this.FindFetchTarget(context.consumerState);
    this.smi.sm.source.Set(pickupable.gameObject, this.smi);
    pickupable.Subscribe(-1582839653, new System.Action<object>(this.OnTagsChanged));
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    Pickupable pickupable = this.smi.sm.source.Get<Pickupable>(this.smi);
    if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
      pickupable.Unsubscribe(-1582839653, new System.Action<object>(this.OnTagsChanged));
    base.End(reason);
  }

  private void OnTagsChanged(object data)
  {
    if (!((UnityEngine.Object) this.smi.sm.chunk.Get(this.smi) != (UnityEngine.Object) null))
      return;
    this.Fail("Tags changed");
  }

  public override void PrepareChore(ref Chore.Precondition.Context context)
  {
    context.chore = (Chore) new FetchAreaChore(context);
  }

  public float AmountWaitingToFetch()
  {
    if ((UnityEngine.Object) this.fetcher == (UnityEngine.Object) null)
      return this.originalAmount;
    return this.amount;
  }

  private void OnOnlyFetchMarkedItemsSettingChanged(object data)
  {
    if (this.smi.sm.destination.Get<Storage>(this.smi).GetOnlyFetchMarkedItems())
      this.requiredTagBits.SetTag(GameTags.Garbage);
    else
      this.requiredTagBits.Clear(GameTags.Garbage);
  }

  private void OnMasterPriorityChanged(
    PriorityScreen.PriorityClass priorityClass,
    int priority_value)
  {
    this.masterPriority.priority_class = priorityClass;
    this.masterPriority.priority_value = priority_value;
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
  }

  public void CollectChoresFromGlobalChoreProvider(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    base.CollectChores(consumer_state, succeeded_contexts, failed_contexts, is_attempting_override);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    Storage storage = this.smi.sm.destination.Get<Storage>(this.smi);
    if (!((UnityEngine.Object) storage != (UnityEngine.Object) null))
      return;
    storage.Unsubscribe(644822890, new System.Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
  }

  public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
  {
    public StatesInstance(FetchChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
  {
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter destination;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
    }
  }
}
