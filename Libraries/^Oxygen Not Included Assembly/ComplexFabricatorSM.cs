// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
  [MyCmpGet]
  private ComplexFabricator fabricator;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
  {
    public StatesInstance(ComplexFabricatorSM master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
  {
    public ComplexFabricatorSM.States.IdleStates off;
    public ComplexFabricatorSM.States.IdleStates idle;
    public ComplexFabricatorSM.States.OperatingStates operating;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.idle.DefaultState(this.idle.idleQueue).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.off, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, (GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.operating, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.idle.idleQueue.ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorIdle, (object) null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.master.fabricator.HasAnyOrder));
      this.idle.waitingForMaterial.ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorEmpty, (object) null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.HasAnyOrder)).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => smi.master.fabricator.WaitingForWorker));
      this.idle.waitingForWorker.ToggleStatusItem(Db.Get().BuildingStatusItems.PendingWork, (object) null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.WaitingForWorker)).EnterTransition((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.operating, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.master.fabricator.duplicantOperated));
      this.operating.DefaultState(this.operating.working_pre);
      this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
      this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
      this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((Func<ComplexFabricatorSM.StatesInstance, Workable>) (smi => (Workable) smi.master.fabricator.Workable), this.operating.working_pst_complete).OnAnimQueueComplete((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle);
      this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete((GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State) this.idle);
    }

    public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
    {
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
    }

    public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
    {
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;
      public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
    }
  }
}
