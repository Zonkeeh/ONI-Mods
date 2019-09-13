// Decompiled with JetBrains decompiler
// Type: SolidLogicValve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>, ISim200ms
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private SolidConduitBridge bridge;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  public void Sim200ms(float dt)
  {
    if (this.operational.IsOperational && this.bridge.IsDispensing)
      this.operational.SetActive(true, false);
    else
      this.operational.SetActive(false, false);
  }

  public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
  {
    public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;
    public SolidLogicValve.States.ReadyStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.DoNothing();
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State) this.on, (StateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (StateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.on.idle, (StateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive));
    }

    public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
    {
      public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;
      public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
    }
  }

  public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
  {
    public StatesInstance(SolidLogicValve master)
      : base(master)
    {
    }
  }
}
