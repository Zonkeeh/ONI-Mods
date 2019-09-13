// Decompiled with JetBrains decompiler
// Type: IceMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>
{
  [MyCmpGet]
  private Operational operational;
  public Storage waterStorage;
  public Storage iceStorage;
  public float targetTemperature;
  public float heatRemovalRate;
  private static StatusItem iceStorageFullStatusItem;

  public void SetStorages(Storage waterStorage, Storage iceStorage)
  {
    this.waterStorage = waterStorage;
    this.iceStorage = iceStorage;
  }

  private bool CanMakeIce()
  {
    bool flag1 = (UnityEngine.Object) this.waterStorage != (UnityEngine.Object) null && (double) this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.100000001490116;
    bool flag2 = (UnityEngine.Object) this.iceStorage != (UnityEngine.Object) null && this.iceStorage.IsFull();
    if (flag1)
      return !flag2;
    return false;
  }

  private void MakeIce(IceMachine.StatesInstance smi, float dt)
  {
    float num = this.heatRemovalRate * dt / (float) this.waterStorage.items.Count;
    foreach (GameObject gameObject in this.waterStorage.items)
      GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
    for (int count = this.waterStorage.items.Count; count > 0; --count)
    {
      GameObject item_go = this.waterStorage.items[count - 1];
      if ((bool) ((UnityEngine.Object) item_go) && (double) item_go.GetComponent<PrimaryElement>().Temperature < (double) item_go.GetComponent<PrimaryElement>().Element.lowTemp)
      {
        PrimaryElement component = item_go.GetComponent<PrimaryElement>();
        this.waterStorage.AddOre(component.Element.lowTempTransitionTarget, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
        this.waterStorage.ConsumeIgnoringDisease(item_go);
      }
    }
    smi.UpdateIceState();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
  {
    private MeterController meter;
    public Chore emptyChore;

    public StatesInstance(IceMachine smi)
      : base(smi)
    {
      this.meter = new MeterController((KAnimControllerBase) this.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
      {
        "meter_OL",
        "meter_frame",
        "meter_fill"
      });
      this.UpdateMeter();
      this.Subscribe(-1697596308, new System.Action<object>(this.OnStorageChange));
    }

    private void OnStorageChange(object data)
    {
      this.UpdateMeter();
    }

    public void UpdateMeter()
    {
      this.meter.SetPositionPercent(Mathf.Clamp01(this.smi.master.iceStorage.MassStored() / this.smi.master.iceStorage.Capacity()));
    }

    public void UpdateIceState()
    {
      bool flag = false;
      for (int count = this.smi.master.waterStorage.items.Count; count > 0; --count)
      {
        GameObject gameObject = this.smi.master.waterStorage.items[count - 1];
        if ((bool) ((UnityEngine.Object) gameObject) && (double) gameObject.GetComponent<PrimaryElement>().Temperature <= (double) this.smi.master.targetTemperature)
          flag = true;
      }
      this.sm.doneFreezingIce.Set(flag, this);
    }
  }

  public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
  {
    public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;
    public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;
    public IceMachine.States.OnStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.serializable = true;
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State) this.on, (StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).DefaultState(this.on.waiting);
      this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.Transition.ConditionCallback) (smi => smi.master.CanMakeIce()));
      this.on.working_pre.Enter((StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback) (smi => smi.UpdateIceState())).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
      this.on.working.QueueAnim("working_loop", true, (Func<IceMachine.StatesInstance, string>) null).Update("UpdateWorking", (System.Action<IceMachine.StatesInstance, float>) ((smi, dt) => smi.master.MakeIce(smi, dt)), UpdateRate.SIM_200ms, false).ParamTransition<bool>((StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.Parameter<bool>) this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter((StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback) (smi =>
      {
        smi.master.operational.SetActive(true, false);
        smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
      })).Exit((StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback) (smi =>
      {
        smi.master.operational.SetActive(false, false);
        smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
      }));
      this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State) this.on);
    }

    private void DoTransfer(IceMachine.StatesInstance smi)
    {
      for (int index = smi.master.waterStorage.items.Count - 1; index >= 0; --index)
      {
        GameObject go = smi.master.waterStorage.items[index];
        if ((bool) ((UnityEngine.Object) go) && (double) go.GetComponent<PrimaryElement>().Temperature <= (double) smi.master.targetTemperature)
          smi.master.waterStorage.Transfer(go, smi.master.iceStorage, false, true);
      }
      smi.UpdateMeter();
    }

    public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
    {
      public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;
      public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;
      public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;
      public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
    }
  }
}
