// Decompiled with JetBrains decompiler
// Type: PeeChoreMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class PeeChoreMonitor : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance>
{
  private StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter pee_fuse = new StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State building;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State critical;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State paused;
  public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State pee;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.building;
    this.serializable = true;
    double num1;
    this.building.Update((System.Action<PeeChoreMonitor.Instance, float>) ((smi, dt) => num1 = (double) this.pee_fuse.Delta(-dt, smi)), UpdateRate.SIM_200ms, false).Transition(this.paused, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => this.IsSleeping(smi)), UpdateRate.SIM_200ms).Transition(this.critical, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) this.pee_fuse.Get(smi) <= 60.0), UpdateRate.SIM_200ms);
    double num2;
    this.critical.Update((System.Action<PeeChoreMonitor.Instance, float>) ((smi, dt) => num2 = (double) this.pee_fuse.Delta(-dt, smi)), UpdateRate.SIM_200ms, false).Transition(this.paused, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => this.IsSleeping(smi)), UpdateRate.SIM_200ms).Transition(this.pee, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) this.pee_fuse.Get(smi) <= 0.0), UpdateRate.SIM_200ms);
    this.paused.Transition(this.building, (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !this.IsSleeping(smi)), UpdateRate.SIM_200ms);
    this.pee.ToggleChore(new Func<PeeChoreMonitor.Instance, Chore>(this.CreatePeeChore), this.building);
  }

  private bool IsSleeping(PeeChoreMonitor.Instance smi)
  {
    StaminaMonitor.Instance smi1 = smi.master.gameObject.GetSMI<StaminaMonitor.Instance>();
    if (smi1 == null || smi1.IsSleeping())
      ;
    return false;
  }

  private Chore CreatePeeChore(PeeChoreMonitor.Instance smi)
  {
    return (Chore) new PeeChore(smi.master);
  }

  public class Instance : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
