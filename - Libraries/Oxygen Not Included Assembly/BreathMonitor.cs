// Decompiled with JetBrains decompiler
// Type: BreathMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
  public BreathMonitor.SatisfiedState satisfied;
  public BreathMonitor.LowBreathState lowbreath;
  public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state1 = this.satisfied.DefaultState(this.satisfied.full);
    BreathMonitor.LowBreathState lowbreath = this.lowbreath;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = BreathMonitor.\u003C\u003Ef__mg\u0024cache0;
    state1.Transition((GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State) lowbreath, fMgCache0, UpdateRate.SIM_200ms);
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full1 = this.satisfied.full;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull1 = this.satisfied.notfull;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = BreathMonitor.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state2 = full1.Transition(notfull1, fMgCache1, UpdateRate.SIM_200ms);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache2 = BreathMonitor.\u003C\u003Ef__mg\u0024cache2;
    state2.Enter(fMgCache2);
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull2 = this.satisfied.notfull;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full2 = this.satisfied.full;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache3 = BreathMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state3 = notfull2.Transition(full2, fMgCache3, UpdateRate.SIM_200ms);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache4 = BreathMonitor.\u003C\u003Ef__mg\u0024cache4;
    state3.Enter(fMgCache4);
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state4 = this.lowbreath.DefaultState(this.lowbreath.nowheretorecover);
    BreathMonitor.SatisfiedState satisfied = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache5 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache5 = BreathMonitor.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state5 = state4.Transition((GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State) satisfied, fMgCache5, UpdateRate.SIM_200ms);
    Expression recoverBreath = Db.Get().Expressions.RecoverBreath;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache6 = new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsNotInBreathableArea);
    }
    // ISSUE: reference to a compiler-generated field
    Func<BreathMonitor.Instance, bool> fMgCache6 = BreathMonitor.\u003C\u003Ef__mg\u0024cache6;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state6 = state5.ToggleExpression(recoverBreath, fMgCache6).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating, (Func<BreathMonitor.Instance, bool>) null).ToggleTag(GameTags.HoldingBreath);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache7 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache7 = BreathMonitor.\u003C\u003Ef__mg\u0024cache7;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state7 = state6.Enter(fMgCache7);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache8 = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache8 = BreathMonitor.\u003C\u003Ef__mg\u0024cache8;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state8 = state7.Enter(fMgCache8);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cache9 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cache9 = new System.Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<BreathMonitor.Instance, float> fMgCache9 = BreathMonitor.\u003C\u003Ef__mg\u0024cache9;
    state8.Update(fMgCache9, UpdateRate.RENDER_1000ms, true);
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover1 = this.lowbreath.nowheretorecover;
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell1 = this.recoverBreathCell;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable1 = this.lowbreath.recoveryavailable;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cacheA == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cacheA = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback fMgCacheA = BreathMonitor.\u003C\u003Ef__mg\u0024cacheA;
    nowheretorecover1.ParamTransition<int>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>) recoverBreathCell1, recoveryavailable1, fMgCacheA);
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable2 = this.lowbreath.recoveryavailable;
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell2 = this.recoverBreathCell;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover2 = this.lowbreath.nowheretorecover;
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cacheB == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cacheB = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback fMgCacheB = BreathMonitor.\u003C\u003Ef__mg\u0024cacheB;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state9 = recoveryavailable2.ParamTransition<int>((StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>) recoverBreathCell2, nowheretorecover2, fMgCacheB);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cacheC == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cacheC = new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCacheC = BreathMonitor.\u003C\u003Ef__mg\u0024cacheC;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State state10 = state9.Enter(fMgCacheC);
    // ISSUE: reference to a compiler-generated field
    if (BreathMonitor.\u003C\u003Ef__mg\u0024cacheD == null)
    {
      // ISSUE: reference to a compiler-generated field
      BreathMonitor.\u003C\u003Ef__mg\u0024cacheD = new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore);
    }
    // ISSUE: reference to a compiler-generated field
    Func<BreathMonitor.Instance, Chore> fMgCacheD = BreathMonitor.\u003C\u003Ef__mg\u0024cacheD;
    GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover3 = this.lowbreath.nowheretorecover;
    state10.ToggleChore(fMgCacheD, nowheretorecover3);
  }

  private static bool IsLowBreath(BreathMonitor.Instance smi)
  {
    if (!VignetteManager.Instance.Get().IsRedAlert())
      return (double) smi.breath.value < 72.7272720336914;
    return (double) smi.breath.value < 45.4545440673828;
  }

  private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
  {
    return (Chore) new RecoverBreathChore(smi.master);
  }

  private static bool IsNotFullBreath(BreathMonitor.Instance smi)
  {
    return !BreathMonitor.IsFullBreath(smi);
  }

  private static bool IsFullBreath(BreathMonitor.Instance smi)
  {
    return (double) smi.breath.value >= (double) smi.breath.GetMax();
  }

  private static bool IsNotInBreathableArea(BreathMonitor.Instance smi)
  {
    return !smi.breather.IsBreathableElementAtCell(Grid.PosToCell((StateMachine.Instance) smi), (CellOffset[]) null);
  }

  private static void ShowBreathBar(BreathMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
  }

  private static void HideBreathBar(BreathMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, (Func<float>) null, false);
  }

  private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
  {
    return cell != Grid.InvalidCell;
  }

  private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
  {
    return !BreathMonitor.IsValidRecoverCell(smi, cell);
  }

  private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
  {
    BreathMonitor.UpdateRecoverBreathCell(smi);
  }

  private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
  {
    smi.query.Reset();
    smi.navigator.RunQuery((PathFinderQuery) smi.query);
    int cell = smi.query.GetResultCell();
    if (!smi.breather.IsBreathableElementAtCell(cell, (CellOffset[]) null))
      cell = PathFinder.InvalidCell;
    smi.sm.recoverBreathCell.Set(cell, smi);
  }

  public class LowBreathState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable;
  }

  public class SatisfiedState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;
    public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
  }

  public class Instance : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public AmountInstance breath;
    public SafetyQuery query;
    public Navigator navigator;
    public OxygenBreather breather;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
      this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, this.GetComponent<KMonoBehaviour>(), int.MaxValue);
      this.navigator = this.GetComponent<Navigator>();
      this.breather = this.GetComponent<OxygenBreather>();
    }

    public int GetRecoverCell()
    {
      return this.sm.recoverBreathCell.Get(this.smi);
    }

    public float GetBreath()
    {
      return this.breath.value / this.breath.GetMax();
    }
  }
}
