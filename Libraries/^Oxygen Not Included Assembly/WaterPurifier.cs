// Decompiled with JetBrains decompiler
// Type: WaterPurifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class WaterPurifier : StateMachineComponent<WaterPurifier.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<WaterPurifier> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<WaterPurifier>((System.Action<WaterPurifier, object>) ((component, data) => component.OnConduitConnectionChanged(data)));
  [MyCmpGet]
  private Operational operational;
  private ManualDeliveryKG[] deliveryComponents;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.deliveryComponents = this.GetComponents<ManualDeliveryKG>();
    this.OnConduitConnectionChanged((object) this.GetComponent<ConduitConsumer>().IsConnected);
    this.Subscribe<WaterPurifier>(-2094018600, WaterPurifier.OnConduitConnectionChangedDelegate);
    this.smi.StartSM();
  }

  private void OnConduitConnectionChanged(object data)
  {
    bool pause = (bool) data;
    foreach (ManualDeliveryKG deliveryComponent in this.deliveryComponents)
    {
      Element element = ElementLoader.GetElement(deliveryComponent.requestedItemTag);
      if (element != null && element.IsLiquid)
        deliveryComponent.Pause(pause, "pipe connected");
    }
  }

  public class StatesInstance : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.GameInstance
  {
    public StatesInstance(WaterPurifier smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier>
  {
    public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State off;
    public WaterPurifier.States.OnStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State) this.on, (StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).DefaultState(this.on.waiting);
      this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
      this.on.working.Enter((StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).QueueAnim("working_loop", true, (Func<WaterPurifier.StatesInstance, string>) null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll())).Exit((StateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false)));
      this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
    }

    public class OnStates : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State
    {
      public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State waiting;
      public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pre;
      public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working;
      public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pst;
    }
  }
}
