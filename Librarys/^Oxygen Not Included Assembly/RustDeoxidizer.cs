// Decompiled with JetBrains decompiler
// Type: RustDeoxidizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
  [SerializeField]
  public float maxMass = 2.5f;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter emitter;
  [MyCmpReq]
  private Operational operational;
  private MeterController meter;

  protected override void OnSpawn()
  {
    this.smi.StartSM();
    Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
    base.OnCleanUp();
  }

  private bool RoomForPressure
  {
    get
    {
      int start_cell = Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition()));
      if (RustDeoxidizer.\u003C\u003Ef__mg\u0024cache0 == null)
        RustDeoxidizer.\u003C\u003Ef__mg\u0024cache0 = new Func<int, RustDeoxidizer, bool>(RustDeoxidizer.OverPressure);
      return !GameUtil.FloodFillCheck<RustDeoxidizer>(RustDeoxidizer.\u003C\u003Ef__mg\u0024cache0, this, start_cell, 3, true, true);
    }
  }

  private static bool OverPressure(int cell, RustDeoxidizer rustDeoxidizer)
  {
    return (double) Grid.Mass[cell] > (double) rustDeoxidizer.maxMass;
  }

  public class StatesInstance : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance
  {
    public StatesInstance(RustDeoxidizer smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
  {
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.Enter("Waiting", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter("Ready", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Transition(this.waiting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()), UpdateRate.SIM_200ms).Transition(this.overpressure, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.RoomForPressure), UpdateRate.SIM_200ms);
      this.overpressure.Enter("OverPressure", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, (object) null).Transition(this.converting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.RoomForPressure), UpdateRate.SIM_200ms);
    }
  }
}
