// Decompiled with JetBrains decompiler
// Type: Electrolyzer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
  [SerializeField]
  public float maxMass = 2.5f;
  [SerializeField]
  public bool hasMeter = true;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter emitter;
  [MyCmpReq]
  private Operational operational;
  private MeterController meter;

  protected override void OnSpawn()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (this.hasMeter)
      this.meter = new MeterController((KAnimControllerBase) component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[4]
      {
        "U2H_meter_target",
        "U2H_meter_tank",
        "U2H_meter_waterbody",
        "U2H_meter_level"
      });
    this.smi.StartSM();
    this.UpdateMeter();
    Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
    base.OnCleanUp();
  }

  public void UpdateMeter()
  {
    if (!this.hasMeter)
      return;
    this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
  }

  private bool RoomForPressure
  {
    get
    {
      int start_cell = Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition()));
      if (Electrolyzer.\u003C\u003Ef__mg\u0024cache0 == null)
        Electrolyzer.\u003C\u003Ef__mg\u0024cache0 = new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure);
      return !GameUtil.FloodFillCheck<Electrolyzer>(Electrolyzer.\u003C\u003Ef__mg\u0024cache0, this, start_cell, 3, true, true);
    }
  }

  private static bool OverPressure(int cell, Electrolyzer electrolyzer)
  {
    return (double) Grid.Mass[cell] > (double) electrolyzer.maxMass;
  }

  public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
  {
    public StatesInstance(Electrolyzer smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
  {
    public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;
    public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;
    public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;
    public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).EventHandler(GameHashes.OnStorageChange, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State.Callback) (smi => smi.master.UpdateMeter()));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.Enter("Waiting", (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter("Ready", (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Transition(this.waiting, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()), UpdateRate.SIM_200ms).Transition(this.overpressure, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => !smi.master.RoomForPressure), UpdateRate.SIM_200ms);
      this.overpressure.Enter("OverPressure", (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, (object) null).Transition(this.converting, (StateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.Transition.ConditionCallback) (smi => smi.master.RoomForPressure), UpdateRate.SIM_200ms);
    }
  }
}
