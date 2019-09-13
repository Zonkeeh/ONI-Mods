// Decompiled with JetBrains decompiler
// Type: JetSuitMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;
  public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.Target(this.owner);
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off1 = this.off;
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying1 = this.flying;
    // ISSUE: reference to a compiler-generated field
    if (JetSuitMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      JetSuitMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = JetSuitMonitor.\u003C\u003Ef__mg\u0024cache0;
    off1.EventTransition(GameHashes.PathAdvanced, flying1, fMgCache0);
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying2 = this.flying;
    // ISSUE: reference to a compiler-generated field
    if (JetSuitMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      JetSuitMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache1 = JetSuitMonitor.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State state1 = flying2.Enter(fMgCache1);
    // ISSUE: reference to a compiler-generated field
    if (JetSuitMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      JetSuitMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback fMgCache2 = JetSuitMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State state2 = state1.Exit(fMgCache2);
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off2 = this.off;
    // ISSUE: reference to a compiler-generated field
    if (JetSuitMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      JetSuitMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache3 = JetSuitMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State state3 = state2.EventTransition(GameHashes.PathAdvanced, off2, fMgCache3);
    // ISSUE: reference to a compiler-generated field
    if (JetSuitMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      JetSuitMonitor.\u003C\u003Ef__mg\u0024cache4 = new System.Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<JetSuitMonitor.Instance, float> fMgCache4 = JetSuitMonitor.\u003C\u003Ef__mg\u0024cache4;
    state3.Update(fMgCache4, UpdateRate.SIM_200ms, false);
  }

  public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
  {
    if ((bool) ((UnityEngine.Object) smi.navigator))
      return smi.navigator.CurrentNavType == NavType.Hover;
    return false;
  }

  public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
  {
    if ((bool) ((UnityEngine.Object) smi.navigator))
      return smi.navigator.CurrentNavType != NavType.Hover;
    return true;
  }

  public static void StartFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void StopFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void Emit(JetSuitMonitor.Instance smi, float dt)
  {
    if (!(bool) ((UnityEngine.Object) smi.navigator))
      return;
    GameObject gameObject = smi.sm.owner.Get(smi);
    if (!(bool) ((UnityEngine.Object) gameObject))
      return;
    int cell = Grid.PosToCell(gameObject.transform.GetPosition());
    float num = Mathf.Min(0.1f * dt, smi.jet_suit_tank.amount);
    smi.jet_suit_tank.amount -= num;
    float mass = num * 3f;
    if ((double) mass > 1.40129846432482E-45)
      SimMessages.AddRemoveSubstance(cell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, 473.15f, byte.MaxValue, 0, true, -1);
    if ((double) smi.jet_suit_tank.amount != 0.0)
      return;
    smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
    smi.navigator.SetCurrentNavType(NavType.Floor);
  }

  public class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Navigator navigator;
    public JetSuitTank jet_suit_tank;

    public Instance(IStateMachineTarget master, GameObject owner)
      : base(master)
    {
      this.sm.owner.Set(owner, this.smi);
      this.navigator = owner.GetComponent<Navigator>();
      this.jet_suit_tank = master.GetComponent<JetSuitTank>();
    }
  }
}
