// Decompiled with JetBrains decompiler
// Type: LiquidCooledFan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LiquidCooledFan : StateMachineComponent<LiquidCooledFan.StatesInstance>, IEffectDescriptor
{
  private float flowRate = 0.3f;
  private HandleVector<int>.Handle waterConsumptionAccumulator = HandleVector<int>.InvalidHandle;
  [SerializeField]
  public float coolingKilowatts;
  [SerializeField]
  public float minCooledTemperature;
  [SerializeField]
  public float minEnvironmentMass;
  [SerializeField]
  public float waterKGConsumedPerKJ;
  [SerializeField]
  public Vector2I minCoolingRange;
  [SerializeField]
  public Vector2I maxCoolingRange;
  [SerializeField]
  public Storage gasStorage;
  [SerializeField]
  public Storage liquidStorage;
  [MyCmpAdd]
  private LiquidCooledFanWorkable workable;
  [MyCmpGet]
  private Operational operational;
  private MeterController meter;

  public bool HasMaterial()
  {
    ListPool<GameObject, LiquidCooledFan>.PooledList pooledList = ListPool<GameObject, LiquidCooledFan>.Allocate();
    this.smi.master.gasStorage.Find(GameTags.Water, (List<GameObject>) pooledList);
    if (pooledList.Count > 0)
    {
      Debug.LogWarning((object) "Liquid Cooled fan Gas storage contains water - A duplicant probably delivered to the wrong storage - moving it to liquid storage.");
      foreach (GameObject go in (List<GameObject>) pooledList)
        this.smi.master.gasStorage.Transfer(go, this.smi.master.liquidStorage, false, false);
    }
    pooledList.Recycle();
    this.UpdateMeter();
    return (double) this.liquidStorage.MassStored() > 0.0;
  }

  public void CheckWorking()
  {
    if (!((UnityEngine.Object) this.smi.master.workable.worker == (UnityEngine.Object) null))
      return;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.unworkable);
  }

  private void UpdateUnworkableStatusItems()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (!this.smi.EnvironmentNeedsCooling())
    {
      if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
        component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, (object) null);
    }
    else if (component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, false);
    if (!this.smi.EnvironmentHighEnoughPressure())
    {
      if (component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
        return;
      component.AddStatusItem(Db.Get().BuildingStatusItems.UnderPressure, (object) null);
    }
    else
    {
      if (!component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
        return;
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.UnderPressure, false);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_waterbody",
      "meter_waterlevel"
    });
    this.GetComponent<ElementConsumer>().EnableConsumption(true);
    this.smi.StartSM();
    this.smi.master.waterConsumptionAccumulator = Game.Instance.accumulators.Add("waterConsumptionAccumulator", (KMonoBehaviour) this);
    this.GetComponent<ElementConsumer>().storage = this.gasStorage;
    this.GetComponent<ManualDeliveryKG>().SetStorage(this.liquidStorage);
  }

  private void UpdateMeter()
  {
    this.meter.SetPositionPercent(Mathf.Clamp01(this.liquidStorage.MassStored() / this.liquidStorage.capacityKg));
  }

  private void EmitContents()
  {
    if (this.gasStorage.items.Count == 0)
      return;
    float num = 0.1f;
    PrimaryElement primaryElement = (PrimaryElement) null;
    for (int index = 0; index < this.gasStorage.items.Count; ++index)
    {
      PrimaryElement component = this.gasStorage.items[index].GetComponent<PrimaryElement>();
      if ((double) component.Mass > (double) num && component.Element.IsGas)
      {
        primaryElement = component;
        num = primaryElement.Mass;
      }
    }
    if (!((UnityEngine.Object) primaryElement != (UnityEngine.Object) null))
      return;
    SimMessages.AddRemoveSubstance(Grid.CellRight(Grid.CellAbove(Grid.PosToCell(this.gameObject))), ElementLoader.GetElementIndex(primaryElement.ElementID), CellEventLogger.Instance.ExhaustSimUpdate, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
    this.gasStorage.ConsumeIgnoringDisease(primaryElement.gameObject);
  }

  private void CoolContents(float dt)
  {
    if (this.gasStorage.items.Count == 0)
      return;
    float a = float.PositiveInfinity;
    float num1 = 0.0f;
    foreach (GameObject gameObject in this.gasStorage.items)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && (double) component.Mass >= 0.100000001490116 && (double) component.Temperature >= (double) this.minCooledTemperature)
      {
        float thermalEnergy = GameUtil.GetThermalEnergy(component);
        if ((double) a > (double) thermalEnergy)
          a = thermalEnergy;
      }
    }
    foreach (GameObject gameObject in this.gasStorage.items)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && (double) component.Mass >= 0.100000001490116 && (double) component.Temperature >= (double) this.minCooledTemperature)
      {
        float num2 = Mathf.Min(a, 10f);
        GameUtil.DeltaThermalEnergy(component, -num2, this.minCooledTemperature);
        num1 += num2;
      }
    }
    float amount = Mathf.Abs(num1 * this.waterKGConsumedPerKJ);
    Game.Instance.accumulators.Accumulate(this.smi.master.waterConsumptionAccumulator, amount);
    if ((double) amount == 0.0)
      return;
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.liquidStorage.ConsumeAndGetDisease(GameTags.Water, amount, out disease_info, out aggregate_temperature);
    SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this.gameObject), disease_info.idx, disease_info.count);
    this.UpdateMeter();
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.HEATCONSUMED, (object) GameUtil.GetFormattedHeatEnergy(this.coolingKilowatts, GameUtil.HeatEnergyFormatterUnit.Automatic)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.HEATCONSUMED, (object) GameUtil.GetFormattedHeatEnergy(this.coolingKilowatts, GameUtil.HeatEnergyFormatterUnit.Automatic)), Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.GameInstance
  {
    public StatesInstance(LiquidCooledFan smi)
      : base(smi)
    {
    }

    public bool IsWorkable()
    {
      bool flag = false;
      if (this.master.operational.IsOperational && this.EnvironmentNeedsCooling() && (this.smi.master.HasMaterial() && this.smi.EnvironmentHighEnoughPressure()))
        flag = true;
      return flag;
    }

    public bool EnvironmentNeedsCooling()
    {
      bool flag = false;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      for (int y = this.master.minCoolingRange.y; y < this.master.maxCoolingRange.y; ++y)
      {
        for (int x = this.master.minCoolingRange.x; x < this.master.maxCoolingRange.x; ++x)
        {
          CellOffset offset = new CellOffset(x, y);
          int index = Grid.OffsetCell(cell, offset);
          if ((double) Grid.Temperature[index] > (double) this.master.minCooledTemperature)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public bool EnvironmentHighEnoughPressure()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      for (int y = this.master.minCoolingRange.y; y < this.master.maxCoolingRange.y; ++y)
      {
        for (int x = this.master.minCoolingRange.x; x < this.master.maxCoolingRange.x; ++x)
        {
          CellOffset offset = new CellOffset(x, y);
          int index = Grid.OffsetCell(cell, offset);
          if ((double) Grid.Mass[index] >= (double) this.master.minEnvironmentMass)
            return true;
        }
      }
      return false;
    }
  }

  public class States : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan>
  {
    public LiquidCooledFan.States.Workable workable;
    public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State unworkable;
    public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State work_pst;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unworkable;
      this.root.Enter((StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi => smi.master.workable.SetWorkTime(float.PositiveInfinity)));
      this.workable.ToggleChore(new Func<LiquidCooledFan.StatesInstance, Chore>(this.CreateUseChore), this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.consuming, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.workable.worker != (UnityEngine.Object) null)).EventTransition(GameHashes.OperationalChanged, this.workable.consuming, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.workable.worker != (UnityEngine.Object) null)).Transition(this.unworkable, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => !smi.IsWorkable()), UpdateRate.SIM_200ms);
      this.work_pst.Update("LiquidFanEmitCooledContents", (System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.EmitContents()), UpdateRate.SIM_200ms, false).ScheduleGoTo(2f, (StateMachine.BaseState) this.unworkable);
      this.unworkable.Update("LiquidFanEmitCooledContents", (System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.EmitContents()), UpdateRate.SIM_200ms, false).Update("LiquidFanUnworkableStatusItems", (System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.UpdateUnworkableStatusItems()), UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => smi.IsWorkable()), UpdateRate.SIM_200ms).Enter((StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi => smi.master.UpdateUnworkableStatusItems())).Exit((StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi => smi.master.UpdateUnworkableStatusItems()));
      this.workable.consuming.EventTransition(GameHashes.OperationalChanged, this.unworkable, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.workable.worker == (UnityEngine.Object) null)).EventHandler(GameHashes.ActiveChanged, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi => smi.master.CheckWorking())).Enter((StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi =>
      {
        if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
          smi.GoTo((StateMachine.BaseState) this.unworkable);
        ElementConsumer component = smi.master.GetComponent<ElementConsumer>();
        component.consumptionRate = smi.master.flowRate;
        component.RefreshConsumptionRate();
      })).Update((System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.CoolContents(dt)), UpdateRate.SIM_200ms, false).ScheduleGoTo(12f, (StateMachine.BaseState) this.workable.emitting).Exit((StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State.Callback) (smi =>
      {
        ElementConsumer component = smi.master.GetComponent<ElementConsumer>();
        component.consumptionRate = 0.0f;
        component.RefreshConsumptionRate();
      }));
      this.workable.emitting.EventTransition(GameHashes.ActiveChanged, this.unworkable, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.workable.worker == (UnityEngine.Object) null)).EventTransition(GameHashes.OperationalChanged, this.unworkable, (StateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.workable.worker == (UnityEngine.Object) null)).ScheduleGoTo(3f, (StateMachine.BaseState) this.workable.consuming).Update((System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.CoolContents(dt)), UpdateRate.SIM_200ms, false).Update("LiquidFanEmitCooledContents", (System.Action<LiquidCooledFan.StatesInstance, float>) ((smi, dt) => smi.master.EmitContents()), UpdateRate.SIM_200ms, false);
    }

    private Chore CreateUseChore(LiquidCooledFan.StatesInstance smi)
    {
      return (Chore) new WorkChore<LiquidCooledFanWorkable>(Db.Get().ChoreTypes.LiquidCooledFan, (IStateMachineTarget) smi.master.workable, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }

    public class Workable : GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State
    {
      public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State waiting;
      public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State consuming;
      public GameStateMachine<LiquidCooledFan.States, LiquidCooledFan.StatesInstance, LiquidCooledFan, object>.State emitting;
    }
  }
}
