// Decompiled with JetBrains decompiler
// Type: BladderMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
  public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public BladderMonitor.WantsToPeeStates urgentwant;
  public BladderMonitor.WantsToPeeStates breakwant;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.Transition((GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State) this.urgentwant, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.NeedsToPee()), UpdateRate.SIM_200ms).Transition((GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State) this.breakwant, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.WantsToPee()), UpdateRate.SIM_200ms);
    this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, (Func<BladderMonitor.Instance, bool>) null).ToggleExpression(Db.Get().Expressions.FullBladder, (Func<BladderMonitor.Instance, bool>) null).ToggleStateMachine((Func<BladderMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new PeeChoreMonitor.Instance(smi.master))).ToggleEffect("FullBladder");
    this.breakwant.InitializeStates(this.satisfied);
    this.breakwant.wanting.Transition((GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State) this.urgentwant, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.NeedsToPee()), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.WantsToPee()));
    this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, (Func<BladderMonitor.Instance, bool>) null);
  }

  public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;
    public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;

    public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(
      GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
    {
      this.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((Func<BladderMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ToiletMonitor.Instance(smi.master)));
      this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsPeeing()));
      this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (StateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsPeeing()));
      return (GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State) this;
    }
  }

  public class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private AmountInstance bladder;
    private ChoreDriver choreDriver;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
      this.choreDriver = this.GetComponent<ChoreDriver>();
    }

    public bool NeedsToPee()
    {
      DebugUtil.DevAssert(this.master != null, "master ref null");
      DebugUtil.DevAssert(!this.master.isNull, "master isNull");
      KPrefabID component = this.master.GetComponent<KPrefabID>();
      DebugUtil.DevAssert((bool) ((UnityEngine.Object) component), "kpid was null");
      if (component.HasTag(GameTags.Asleep))
        return false;
      return (double) this.bladder.value >= 100.0;
    }

    public bool WantsToPee()
    {
      if (this.NeedsToPee())
        return true;
      if (this.IsPeeTime())
        return (double) this.bladder.value >= 40.0;
      return false;
    }

    public bool IsPeeing()
    {
      if (this.choreDriver.HasChore())
        return this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
      return false;
    }

    public bool IsPeeTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
    }
  }
}
