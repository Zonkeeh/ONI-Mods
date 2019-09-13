// Decompiled with JetBrains decompiler
// Type: AlgaeDistillery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
  [SerializeField]
  public Tag emitTag;
  [SerializeField]
  public float emitMass;
  [SerializeField]
  public Vector3 emitOffset;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter emitter;
  [MyCmpReq]
  private Operational operational;

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
  {
    public StatesInstance(AlgaeDistillery smi)
      : base(smi)
    {
    }

    public void TryEmit()
    {
      Storage storage = this.smi.master.storage;
      GameObject first = storage.FindFirst(this.smi.master.emitTag);
      if (!((Object) first != (Object) null) || (double) first.GetComponent<PrimaryElement>().Mass < (double) this.master.emitMass)
        return;
      storage.Drop(first, true).transform.SetPosition(this.transform.GetPosition() + this.master.emitOffset);
    }
  }

  public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
  {
    public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;
    public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;
    public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;
    public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.Enter("Waiting", (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter("Ready", (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Transition(this.waiting, (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, (StateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State.Callback) (smi => smi.TryEmit()));
    }
  }
}
