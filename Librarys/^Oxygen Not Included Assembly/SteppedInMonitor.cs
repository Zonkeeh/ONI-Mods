// Decompiled with JetBrains decompiler
// Type: SteppedInMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;
  public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied1 = this.satisfied;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor1 = this.wetFloor;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state1 = satisfied1.Transition(wetFloor1, fMgCache0, UpdateRate.SIM_200ms);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody1 = this.wetBody;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache1;
    state1.Transition(wetBody1, fMgCache1, UpdateRate.SIM_200ms);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor2 = this.wetFloor;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache2 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state2 = wetFloor2.Enter(fMgCache2);
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache3 = new System.Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SteppedInMonitor.Instance, float> fMgCache3 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state3 = state2.Update(fMgCache3, UpdateRate.SIM_1000ms, false);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied2 = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition1 = GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(SteppedInMonitor.\u003C\u003Ef__mg\u0024cache4);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state4 = state3.Transition(satisfied2, condition1, UpdateRate.SIM_200ms);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody2 = this.wetBody;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache5 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache5 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache5;
    state4.Transition(wetBody2, fMgCache5, UpdateRate.SIM_200ms);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody3 = this.wetBody;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache6 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache6;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state5 = wetBody3.Enter(fMgCache6);
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache7 = new System.Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<SteppedInMonitor.Instance, float> fMgCache7 = SteppedInMonitor.\u003C\u003Ef__mg\u0024cache7;
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State state6 = state5.Update(fMgCache7, UpdateRate.SIM_1000ms, false);
    GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor3 = this.wetFloor;
    // ISSUE: reference to a compiler-generated field
    if (SteppedInMonitor.\u003C\u003Ef__mg\u0024cache8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SteppedInMonitor.\u003C\u003Ef__mg\u0024cache8 = new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback condition2 = GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(SteppedInMonitor.\u003C\u003Ef__mg\u0024cache8);
    state6.Transition(wetFloor3, condition2, UpdateRate.SIM_200ms);
  }

  private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
  {
    SteppedInMonitor.GetWetFeet(smi);
  }

  private static void GetWetFeet(SteppedInMonitor.Instance smi)
  {
    if (smi.effects.HasEffect("SoakingWet"))
      return;
    smi.effects.Add("WetFeet", true);
  }

  private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
  {
    SteppedInMonitor.GetSoaked(smi);
  }

  private static void GetSoaked(SteppedInMonitor.Instance smi)
  {
    if (smi.effects.HasEffect("WetFeet"))
      smi.effects.Remove("WetFeet");
    smi.effects.Add("SoakingWet", true);
  }

  private static bool IsFloorWet(SteppedInMonitor.Instance smi)
  {
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    if (Grid.IsValidCell(cell))
      return Grid.Element[cell].IsLiquid;
    return false;
  }

  private static bool IsSubmerged(SteppedInMonitor.Instance smi)
  {
    int cell = Grid.CellAbove(Grid.PosToCell((StateMachine.Instance) smi));
    if (Grid.IsValidCell(cell))
      return Grid.Element[cell].IsLiquid;
    return false;
  }

  public class Instance : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Effects effects;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.effects = this.GetComponent<Effects>();
    }
  }
}
