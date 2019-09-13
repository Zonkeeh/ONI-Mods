// Decompiled with JetBrains decompiler
// Type: RationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class RationMonitor : GameStateMachine<RationMonitor, RationMonitor.Instance>
{
  public StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.FloatParameter rationsAteToday;
  public RationMonitor.RationsAvailableState rationsavailable;
  public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState outofrations;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.rationsavailable;
    this.serializable = true;
    this.root.EventHandler(GameHashes.EatCompleteEater, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, d) => smi.OnEatComplete(d))).EventHandler(GameHashes.NewDay, (Func<RationMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), (StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.OnNewDay())).ParamTransition<float>((StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.rationsAteToday, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) this.rationsavailable, (StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback) ((smi, p) => smi.HasRationsAvailable())).ParamTransition<float>((StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.rationsAteToday, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) this.outofrations, (StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback) ((smi, p) => !smi.HasRationsAvailable()));
    this.rationsavailable.DefaultState((GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) this.rationsavailable.noediblesavailable);
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state1 = this.rationsavailable.noediblesavailable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.NoRationsAvailable);
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache0 = new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame);
    }
    // ISSUE: reference to a compiler-generated field
    Func<RationMonitor.Instance, KMonoBehaviour> fMgCache0 = RationMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable1 = this.rationsavailable.ediblesunreachable;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereAnyEdibles);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = RationMonitor.\u003C\u003Ef__mg\u0024cache1;
    state1.EventTransition(GameHashes.ColonyHasRationsChanged, fMgCache0, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) ediblesunreachable1, fMgCache1);
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state2 = this.rationsavailable.ediblereachablebutnotpermitted.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsNotPermitted);
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache2 = new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame);
    }
    // ISSUE: reference to a compiler-generated field
    Func<RationMonitor.Instance, KMonoBehaviour> fMgCache2 = RationMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable1 = this.rationsavailable.noediblesavailable;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache3 = RationMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state3 = state2.EventTransition(GameHashes.ColonyHasRationsChanged, fMgCache2, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) noediblesavailable1, fMgCache3);
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable2 = this.rationsavailable.ediblesunreachable;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.NotIsEdibleInReachButNotPermitted);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache4 = RationMonitor.\u003C\u003Ef__mg\u0024cache4;
    state3.EventTransition(GameHashes.ClosestEdibleChanged, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) ediblesunreachable2, fMgCache4);
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state4 = this.rationsavailable.ediblesunreachable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsUnreachable);
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache5 = new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame);
    }
    // ISSUE: reference to a compiler-generated field
    Func<RationMonitor.Instance, KMonoBehaviour> fMgCache5 = RationMonitor.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable2 = this.rationsavailable.noediblesavailable;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache6 = RationMonitor.\u003C\u003Ef__mg\u0024cache6;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state5 = state4.EventTransition(GameHashes.ColonyHasRationsChanged, fMgCache5, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) noediblesavailable2, fMgCache6);
    RationMonitor.EdibleAvailablestate edibleavailable = this.rationsavailable.edibleavailable;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache7 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleAvailable);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache7 = RationMonitor.\u003C\u003Ef__mg\u0024cache7;
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State state6 = state5.EventTransition(GameHashes.ClosestEdibleChanged, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) edibleavailable, fMgCache7);
    GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblereachablebutnotpermitted = this.rationsavailable.ediblereachablebutnotpermitted;
    // ISSUE: reference to a compiler-generated field
    if (RationMonitor.\u003C\u003Ef__mg\u0024cache8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RationMonitor.\u003C\u003Ef__mg\u0024cache8 = new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleInReachButNotPermitted);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache8 = RationMonitor.\u003C\u003Ef__mg\u0024cache8;
    state6.EventTransition(GameHashes.ClosestEdibleChanged, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) ediblereachablebutnotpermitted, fMgCache8);
    this.rationsavailable.edibleavailable.ToggleChore((Func<RationMonitor.Instance, Chore>) (smi => (Chore) new EatChore(smi.master)), (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) this.rationsavailable.noediblesavailable).DefaultState(this.rationsavailable.edibleavailable.readytoeat);
    this.rationsavailable.edibleavailable.readytoeat.EventTransition(GameHashes.ClosestEdibleChanged, (GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State) this.rationsavailable.noediblesavailable, (StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.BeginChore, this.rationsavailable.edibleavailable.eating, (StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsEating()));
    this.rationsavailable.edibleavailable.eating.DoNothing();
    this.outofrations.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.DailyRationLimitReached);
  }

  private static bool AreThereNoEdibles(RationMonitor.Instance smi)
  {
    return !RationMonitor.AreThereAnyEdibles(smi);
  }

  private static bool AreThereAnyEdibles(RationMonitor.Instance smi)
  {
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      ColonyRationMonitor.Instance smi1 = SaveGame.Instance.GetSMI<ColonyRationMonitor.Instance>();
      if (smi1 != null)
        return !smi1.IsOutOfRations();
    }
    return false;
  }

  private static KMonoBehaviour GetSaveGame(RationMonitor.Instance smi)
  {
    return (KMonoBehaviour) SaveGame.Instance;
  }

  private static bool IsEdibleAvailable(RationMonitor.Instance smi)
  {
    return (UnityEngine.Object) smi.GetEdible() != (UnityEngine.Object) null;
  }

  private static bool NotIsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
  {
    return !RationMonitor.IsEdibleInReachButNotPermitted(smi);
  }

  private static bool IsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
  {
    return smi.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().edibleInReachButNotPermitted;
  }

  public class EdibleAvailablestate : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State readytoeat;
    public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State eating;
  }

  public class RationsAvailableState : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable;
    public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblereachablebutnotpermitted;
    public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable;
    public RationMonitor.EdibleAvailablestate edibleavailable;
  }

  public class Instance : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private ChoreDriver choreDriver;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.choreDriver = master.GetComponent<ChoreDriver>();
    }

    public Edible GetEdible()
    {
      return this.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible();
    }

    public bool HasRationsAvailable()
    {
      return true;
    }

    public float GetRationsAteToday()
    {
      return this.sm.rationsAteToday.Get(this.smi);
    }

    public float GetRationsRemaining()
    {
      return 1f;
    }

    public bool IsEating()
    {
      if (this.choreDriver.HasChore())
        return this.choreDriver.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
      return false;
    }

    public void OnNewDay()
    {
      double num = (double) this.smi.sm.rationsAteToday.Set(0.0f, this.smi);
    }

    public void OnEatComplete(object data)
    {
      Edible edible = (Edible) data;
      double num = (double) this.sm.rationsAteToday.Delta(edible.caloriesConsumed, this.smi);
      RationTracker.Get().RegisterRationsConsumed(edible);
    }
  }
}
