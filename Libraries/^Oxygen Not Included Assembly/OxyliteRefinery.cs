// Decompiled with JetBrains decompiler
// Type: OxyliteRefinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
  [MyCmpAdd]
  private Storage storage;
  [MyCmpReq]
  private Operational operational;
  public Tag emitTag;
  public float emitMass;
  public Vector3 dropOffset;

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
  {
    public StatesInstance(OxyliteRefinery smi)
      : base(smi)
    {
    }

    public void TryEmit()
    {
      Storage storage = this.smi.master.storage;
      GameObject first = storage.FindFirst(this.smi.master.emitTag);
      if (!((Object) first != (Object) null) || (double) first.GetComponent<PrimaryElement>().Mass < (double) this.master.emitMass)
        return;
      Vector3 position = this.transform.GetPosition() + this.master.dropOffset;
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      first.transform.SetPosition(position);
      storage.Drop(first, true);
    }
  }

  public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
  {
    public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;
    public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;
    public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter((StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Exit((StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).Transition(this.waiting, (StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, (StateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State.Callback) (smi => smi.TryEmit()));
    }
  }
}
