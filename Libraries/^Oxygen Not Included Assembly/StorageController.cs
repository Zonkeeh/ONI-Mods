// Decompiled with JetBrains decompiler
// Type: StorageController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class StorageController : GameStateMachine<StorageController, StorageController.Instance>
{
  public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State on;
  public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State working;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.root.EventTransition(GameHashes.OnStorageInteracted, this.working, (StateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
    this.working.PlayAnim("working").OnAnimQueueComplete(this.off);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, StorageController.Def def)
      : base(master)
    {
    }
  }
}
