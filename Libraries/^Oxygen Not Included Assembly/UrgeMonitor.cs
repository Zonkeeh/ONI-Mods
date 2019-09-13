// Decompiled with JetBrains decompiler
// Type: UrgeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

public class UrgeMonitor : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance>
{
  public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.State hasurge;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.Transition(this.hasurge, (StateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasUrge()), UpdateRate.SIM_200ms);
    this.hasurge.Transition(this.satisfied, (StateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.HasUrge()), UpdateRate.SIM_200ms).ToggleUrge((Func<UrgeMonitor.Instance, Urge>) (smi => smi.GetUrge()));
  }

  public class Instance : GameStateMachine<UrgeMonitor, UrgeMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private AmountInstance amountInstance;
    private Urge urge;
    private ScheduleBlockType scheduleBlock;
    private Schedulable schedulable;
    private float inScheduleThreshold;
    private float outOfScheduleThreshold;
    private bool isThresholdMinimum;

    public Instance(
      IStateMachineTarget master,
      Urge urge,
      Amount amount,
      ScheduleBlockType schedule_block,
      float in_schedule_threshold,
      float out_of_schedule_threshold,
      bool is_threshold_minimum)
      : base(master)
    {
      this.urge = urge;
      this.scheduleBlock = schedule_block;
      this.schedulable = this.GetComponent<Schedulable>();
      this.amountInstance = this.gameObject.GetAmounts().Get(amount);
      this.isThresholdMinimum = is_threshold_minimum;
      this.inScheduleThreshold = in_schedule_threshold;
      this.outOfScheduleThreshold = out_of_schedule_threshold;
    }

    private float GetThreshold()
    {
      if (this.schedulable.IsAllowed(this.scheduleBlock))
        return this.inScheduleThreshold;
      return this.outOfScheduleThreshold;
    }

    public Urge GetUrge()
    {
      return this.urge;
    }

    public bool HasUrge()
    {
      if (this.isThresholdMinimum)
        return (double) this.amountInstance.value >= (double) this.GetThreshold();
      return (double) this.amountInstance.value <= (double) this.GetThreshold();
    }
  }
}
