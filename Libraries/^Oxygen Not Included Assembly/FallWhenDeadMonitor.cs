// Decompiled with JetBrains decompiler
// Type: FallWhenDeadMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FallWhenDeadMonitor : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance>
{
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State standing;
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State falling;
  public GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.State entombed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.standing;
    this.standing.Transition(this.entombed, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsEntombed()), UpdateRate.SIM_200ms).Transition(this.falling, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsFalling()), UpdateRate.SIM_200ms);
    this.falling.ToggleGravity(this.standing);
    this.entombed.Transition(this.standing, (StateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsEntombed()), UpdateRate.SIM_200ms);
  }

  public class Instance : GameStateMachine<FallWhenDeadMonitor, FallWhenDeadMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public bool IsEntombed()
    {
      Pickupable component = this.GetComponent<Pickupable>();
      if ((Object) component != (Object) null)
        return component.IsEntombed;
      return false;
    }

    public bool IsFalling()
    {
      int cell = Grid.CellBelow(Grid.PosToCell(this.master.transform.GetPosition()));
      if (!Grid.IsValidCell(cell))
        return false;
      return !Grid.Solid[cell];
    }
  }
}
