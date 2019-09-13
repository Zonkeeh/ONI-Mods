// Decompiled with JetBrains decompiler
// Type: TemperatureVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class TemperatureVulnerable : StateMachineComponent<TemperatureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
  private TemperatureVulnerable.TemperatureState internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;
  private OccupyArea _occupyArea;
  public float internalTemperatureLethal_Low;
  public float internalTemperatureWarning_Low;
  public float internalTemperaturePerfect_Low;
  public float internalTemperaturePerfect_High;
  public float internalTemperatureWarning_High;
  public float internalTemperatureLethal_High;
  private const float minimumMassForReading = 0.1f;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private SimTemperatureTransfer temperatureTransfer;
  private AmountInstance displayTemperatureAmount;
  private float averageTemp;
  private int cellCount;

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public event System.Action<float, float> OnTemperature;

  public float InternalTemperature
  {
    get
    {
      return this.primaryElement.Temperature;
    }
  }

  public TemperatureVulnerable.TemperatureState GetInternalTemperatureState
  {
    get
    {
      return this.internalTemperatureState;
    }
  }

  public bool IsLethal
  {
    get
    {
      if (this.GetInternalTemperatureState != TemperatureVulnerable.TemperatureState.LethalHot)
        return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold;
      return true;
    }
  }

  public bool IsNormal
  {
    get
    {
      return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
    }
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[1];
    }
  }

  public string WiltStateString
  {
    get
    {
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.warningCold))
        return Db.Get().CreatureStatusItems.Cold_Crop.resolveStringCallback((string) CREATURES.STATUSITEMS.COLD_CROP.NAME, (object) this);
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.warningHot))
        return Db.Get().CreatureStatusItems.Hot_Crop.resolveStringCallback((string) CREATURES.STATUSITEMS.HOT_CROP.NAME, (object) this);
      return string.Empty;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.displayTemperatureAmount = this.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Temperature, this.gameObject));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    double num = (double) this.smi.sm.internalTemp.Set(this.primaryElement.Temperature, this.smi);
    this.smi.StartSM();
  }

  public void Configure(
    float tempWarningLow,
    float tempLethalLow,
    float tempWarningHigh,
    float tempLethalHigh)
  {
    this.internalTemperatureWarning_Low = tempWarningLow;
    this.internalTemperatureLethal_Low = tempLethalLow;
    this.internalTemperatureLethal_High = tempLethalHigh;
    this.internalTemperatureWarning_High = tempWarningHigh;
  }

  public bool IsCellSafe(int cell)
  {
    float averageTemperature = this.GetAverageTemperature(cell);
    if ((double) averageTemperature > -1.0 && (double) averageTemperature > (double) this.internalTemperatureLethal_Low)
      return (double) averageTemperature < (double) this.internalTemperatureLethal_High;
    return false;
  }

  public void Sim1000ms(float dt)
  {
    if (!Grid.IsValidCell(Grid.PosToCell(this.gameObject)))
      return;
    double num = (double) this.smi.sm.internalTemp.Set(this.InternalTemperature, this.smi);
    this.displayTemperatureAmount.value = this.InternalTemperature;
    if (this.OnTemperature == null)
      return;
    this.OnTemperature(dt, this.InternalTemperature);
  }

  private static bool GetAverageTemperatureCb(int cell, object data)
  {
    TemperatureVulnerable temperatureVulnerable = data as TemperatureVulnerable;
    if ((double) Grid.Mass[cell] > 0.100000001490116)
    {
      temperatureVulnerable.averageTemp += Grid.Temperature[cell];
      ++temperatureVulnerable.cellCount;
    }
    return true;
  }

  private float GetAverageTemperature(int cell)
  {
    this.averageTemp = 0.0f;
    this.cellCount = 0;
    OccupyArea occupyArea = this.occupyArea;
    int rootCell = cell;
    // ISSUE: reference to a compiler-generated field
    if (TemperatureVulnerable.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TemperatureVulnerable.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(TemperatureVulnerable.GetAverageTemperatureCb);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = TemperatureVulnerable.\u003C\u003Ef__mg\u0024cache0;
    occupyArea.TestArea(rootCell, (object) this, fMgCache0);
    if (this.cellCount > 0)
      return this.averageTemp / (float) this.cellCount;
    return -1f;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.REQUIRES_TEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), (object) GameUtil.GetFormattedTemperature(this.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_TEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), (object) GameUtil.GetFormattedTemperature(this.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false)
    };
  }

  public class StatesInstance : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.GameInstance
  {
    public bool hasMaturity;

    public StatesInstance(TemperatureVulnerable master)
      : base(master)
    {
      if (Db.Get().Amounts.Maturity.Lookup(this.gameObject) == null)
        return;
      this.hasMaturity = true;
    }
  }

  public class States : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable>
  {
    public StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.FloatParameter internalTemp;
    public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalCold;
    public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalHot;
    public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningCold;
    public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningHot;
    public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State normal;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.normal;
      GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State state1 = this.lethalCold.Enter((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback) (smi => smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalCold)).TriggerOnEnter(GameHashes.TooColdFatal, (Func<TemperatureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.warningCold, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.internalTemperatureLethal_Low));
      // ISSUE: reference to a compiler-generated field
      if (TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback fMgCache0 = TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache0;
      state1.Enter(fMgCache0);
      GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State state2 = this.lethalHot.Enter((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback) (smi => smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalHot)).TriggerOnEnter(GameHashes.TooHotFatal, (Func<TemperatureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.warningHot, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.internalTemperatureLethal_High));
      // ISSUE: reference to a compiler-generated field
      if (TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback fMgCache1 = TemperatureVulnerable.States.\u003C\u003Ef__mg\u0024cache1;
      state2.Enter(fMgCache1);
      this.warningCold.Enter((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback) (smi => smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningCold)).TriggerOnEnter(GameHashes.TooColdWarning, (Func<TemperatureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.lethalCold, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.internalTemperatureLethal_Low)).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.normal, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.internalTemperatureWarning_Low));
      this.warningHot.Enter((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback) (smi => smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningHot)).TriggerOnEnter(GameHashes.TooHotWarning, (Func<TemperatureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.lethalHot, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.internalTemperatureLethal_High)).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.normal, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.internalTemperatureWarning_High));
      this.normal.Enter((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback) (smi => smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal)).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, (Func<TemperatureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.warningHot, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.internalTemperatureWarning_High)).ParamTransition<float>((StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>) this.internalTemp, this.warningCold, (StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.internalTemperatureWarning_Low));
    }

    private static void Kill(StateMachine.Instance smi)
    {
      smi.GetSMI<DeathMonitor.Instance>()?.Kill(Db.Get().Deaths.Generic);
    }
  }

  public enum TemperatureState
  {
    LethalCold,
    WarningCold,
    Normal,
    WarningHot,
    LethalHot,
  }
}
