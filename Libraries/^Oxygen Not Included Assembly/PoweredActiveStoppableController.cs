// Decompiled with JetBrains decompiler
// Type: PoweredActiveStoppableController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PoweredActiveStoppableController : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance>
{
  public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pre;
  public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_loop;
  public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State working_pst;
  public GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.State stop;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("off").EventTransition(GameHashes.ActiveChanged, this.working_pre, (StateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
    this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
    this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.stop, (StateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.working_pst, (StateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
    this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
    this.stop.PlayAnim("stop").OnAnimQueueComplete(this.off);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<PoweredActiveStoppableController, PoweredActiveStoppableController.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, PoweredActiveStoppableController.Def def)
      : base(master, (object) def)
    {
    }
  }
}
