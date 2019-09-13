// Decompiled with JetBrains decompiler
// Type: PoweredController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PoweredController : GameStateMachine<PoweredController, PoweredController.Instance>
{
  public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<PoweredController, PoweredController.Instance, IStateMachineTarget, object>.GameInstance
  {
    public bool ShowWorkingStatus;

    public Instance(IStateMachineTarget master, PoweredController.Def def)
      : base(master, (object) def)
    {
    }
  }
}
