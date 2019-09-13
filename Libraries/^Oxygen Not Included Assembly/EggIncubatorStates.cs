// Decompiled with JetBrains decompiler
// Type: EggIncubatorStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class EggIncubatorStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance>
{
  public StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.BoolParameter readyToHatch;
  public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty;
  public EggIncubatorStates.EggStates egg;
  public EggIncubatorStates.BabyStates baby;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.empty;
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state1 = this.empty.PlayAnim("off", KAnim.PlayMode.Loop);
    EggIncubatorStates.EggStates egg = this.egg;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasEgg);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = EggIncubatorStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state2 = state1.EventTransition(GameHashes.OccupantChanged, (GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State) egg, fMgCache0);
    EggIncubatorStates.BabyStates baby1 = this.baby;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = EggIncubatorStates.\u003C\u003Ef__mg\u0024cache1;
    state2.EventTransition(GameHashes.OccupantChanged, (GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State) baby1, fMgCache1);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state3 = this.egg.DefaultState(this.egg.unpowered);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty1 = this.empty;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasAny);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition1 = GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(EggIncubatorStates.\u003C\u003Ef__mg\u0024cache2);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state4 = state3.EventTransition(GameHashes.OccupantChanged, empty1, condition1);
    EggIncubatorStates.BabyStates baby2 = this.baby;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache3 = EggIncubatorStates.\u003C\u003Ef__mg\u0024cache3;
    state4.EventTransition(GameHashes.OccupantChanged, (GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State) baby2, fMgCache3).ToggleStatusItem(Db.Get().BuildingStatusItems.IncubatorProgress, (Func<EggIncubatorStates.Instance, object>) (smi => (object) smi.master.GetComponent<EggIncubator>()));
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state5 = this.egg.lose_power.PlayAnim("no_power_pre");
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating1 = this.egg.incubating;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache4 = EggIncubatorStates.\u003C\u003Ef__mg\u0024cache4;
    state5.EventTransition(GameHashes.OperationalChanged, incubating1, fMgCache4).OnAnimQueueComplete(this.egg.unpowered);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state6 = this.egg.unpowered.PlayAnim("no_power_loop", KAnim.PlayMode.Loop);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating2 = this.egg.incubating;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache5 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache5 = EggIncubatorStates.\u003C\u003Ef__mg\u0024cache5;
    state6.EventTransition(GameHashes.OperationalChanged, incubating2, fMgCache5);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state7 = this.egg.incubating.PlayAnim("no_power_pst").QueueAnim("working_loop", true, (Func<EggIncubatorStates.Instance, string>) null);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State losePower = this.egg.lose_power;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition2 = GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(EggIncubatorStates.\u003C\u003Ef__mg\u0024cache6);
    state7.EventTransition(GameHashes.OperationalChanged, losePower, condition2);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State state8 = this.baby.DefaultState(this.baby.idle);
    GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty2 = this.empty;
    // ISSUE: reference to a compiler-generated field
    if (EggIncubatorStates.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EggIncubatorStates.\u003C\u003Ef__mg\u0024cache7 = new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition3 = GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(EggIncubatorStates.\u003C\u003Ef__mg\u0024cache7);
    state8.EventTransition(GameHashes.OccupantChanged, empty2, condition3);
    this.baby.idle.PlayAnim("no_power_pre").QueueAnim("no_power_loop", true, (Func<EggIncubatorStates.Instance, string>) null);
  }

  public static bool IsOperational(EggIncubatorStates.Instance smi)
  {
    return smi.GetComponent<Operational>().IsOperational;
  }

  public static bool HasEgg(EggIncubatorStates.Instance smi)
  {
    GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
    if ((bool) ((UnityEngine.Object) occupant))
      return occupant.HasTag(GameTags.Egg);
    return false;
  }

  public static bool HasBaby(EggIncubatorStates.Instance smi)
  {
    GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
    if ((bool) ((UnityEngine.Object) occupant))
      return occupant.HasTag(GameTags.Creature);
    return false;
  }

  public static bool HasAny(EggIncubatorStates.Instance smi)
  {
    return (bool) ((UnityEngine.Object) smi.GetComponent<EggIncubator>().Occupant);
  }

  public class EggStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating;
    public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State lose_power;
    public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State unpowered;
  }

  public class BabyStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State idle;
  }

  public class Instance : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
