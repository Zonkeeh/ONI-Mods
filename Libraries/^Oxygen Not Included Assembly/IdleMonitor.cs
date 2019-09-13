// Decompiled with JetBrains decompiler
// Type: IdleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class IdleMonitor : GameStateMachine<IdleMonitor, IdleMonitor.Instance>
{
  public GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.State idle;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.serializable = false;
    this.idle.ToggleRecurringChore(new Func<IdleMonitor.Instance, Chore>(this.CreateIdleChore), (Func<IdleMonitor.Instance, bool>) null);
  }

  private Chore CreateIdleChore(IdleMonitor.Instance smi)
  {
    return (Chore) new IdleChore(smi.master);
  }

  public class Instance : GameStateMachine<IdleMonitor, IdleMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }
  }
}
