// Decompiled with JetBrains decompiler
// Type: PressureVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class PressureVulnerable : StateMachineComponent<PressureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
  private static Func<int, object, bool> testAreaCB = (Func<int, object, bool>) ((test_cell, data) =>
  {
    if (Grid.IsGas(test_cell))
    {
      PressureVulnerable.testAreaPressure += Grid.Mass[test_cell];
      ++PressureVulnerable.testAreaCount;
    }
    return true;
  });
  private HandleVector<int>.Handle pressureAccumulator = HandleVector<int>.InvalidHandle;
  private HandleVector<int>.Handle elementAccumulator = HandleVector<int>.InvalidHandle;
  public bool pressure_sensitive = true;
  public HashSet<Element> safe_atmospheres = new HashSet<Element>();
  private PressureVulnerable.PressureState pressureState = PressureVulnerable.PressureState.Normal;
  private OccupyArea _occupyArea;
  public float pressureLethal_Low;
  public float pressureWarning_Low;
  public float pressureWarning_High;
  public float pressureLethal_High;
  private static float testAreaPressure;
  private static int testAreaCount;
  private AmountInstance displayPressureAmount;
  private int cell;

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  public PressureVulnerable.PressureState ExternalPressureState
  {
    get
    {
      return this.pressureState;
    }
  }

  public Element ExternalElement
  {
    get
    {
      return Grid.Element[this.cell];
    }
  }

  public bool IsLethal
  {
    get
    {
      if (this.pressureState != PressureVulnerable.PressureState.LethalHigh && this.pressureState != PressureVulnerable.PressureState.LethalLow)
        return !this.IsSafeElement(this.ExternalElement);
      return true;
    }
  }

  public bool IsNormal
  {
    get
    {
      if (this.IsSafeElement(this.ExternalElement))
        return this.pressureState == PressureVulnerable.PressureState.Normal;
      return false;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.displayPressureAmount = this.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.AirPressure, this.gameObject));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.cell = Grid.PosToCell((KMonoBehaviour) this);
    double num = (double) this.smi.sm.pressure.Set(1f, this.smi);
    this.smi.sm.safe_element.Set(this.IsSafeElement(this.ExternalElement), this.smi);
    this.smi.master.pressureAccumulator = Game.Instance.accumulators.Add("pressureAccumulator", (KMonoBehaviour) this);
    this.smi.master.elementAccumulator = Game.Instance.accumulators.Add("elementAccumulator", (KMonoBehaviour) this);
    this.smi.StartSM();
  }

  public void Configure(SimHashes[] safeAtmospheres = null)
  {
    this.pressure_sensitive = false;
    this.pressureWarning_Low = float.MinValue;
    this.pressureLethal_Low = float.MinValue;
    this.pressureLethal_High = float.MaxValue;
    this.pressureWarning_High = float.MaxValue;
    this.safe_atmospheres = new HashSet<Element>();
    if (safeAtmospheres == null)
      return;
    foreach (SimHashes safeAtmosphere in safeAtmospheres)
      this.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeAtmosphere));
  }

  public void Configure(
    float pressureWarningLow = 0.25f,
    float pressureLethalLow = 0.01f,
    float pressureWarningHigh = 10f,
    float pressureLethalHigh = 30f,
    SimHashes[] safeAtmospheres = null)
  {
    this.pressure_sensitive = true;
    this.pressureWarning_Low = pressureWarningLow;
    this.pressureLethal_Low = pressureLethalLow;
    this.pressureLethal_High = pressureLethalHigh;
    this.pressureWarning_High = pressureWarningHigh;
    this.safe_atmospheres = new HashSet<Element>();
    if (safeAtmospheres == null)
      return;
    foreach (SimHashes safeAtmosphere in safeAtmospheres)
      this.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeAtmosphere));
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[2]
      {
        WiltCondition.Condition.Pressure,
        WiltCondition.Condition.AtmosphereElement
      };
    }
  }

  public string WiltStateString
  {
    get
    {
      string empty = string.Empty;
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.warningLow) || this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.lethalLow))
        empty += Db.Get().CreatureStatusItems.AtmosphericPressureTooLow.resolveStringCallback((string) CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOLOW.NAME, (object) this);
      else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.warningHigh) || this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.lethalHigh))
        empty += Db.Get().CreatureStatusItems.AtmosphericPressureTooHigh.resolveStringCallback((string) CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOHIGH.NAME, (object) this);
      else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.unsafeElement))
        empty += Db.Get().CreatureStatusItems.WrongAtmosphere.resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGATMOSPHERE.NAME, (object) this);
      return empty;
    }
  }

  public bool IsCellSafe(int cell)
  {
    if (this.IsSafeElement(Grid.Element[cell]))
      return this.IsSafePressure(this.GetPressureOverArea(cell));
    return false;
  }

  public bool IsSafeElement(Element element)
  {
    return this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_atmospheres.Contains(element);
  }

  public bool IsSafePressure(float pressure)
  {
    if (!this.pressure_sensitive)
      return true;
    if ((double) pressure > (double) this.pressureLethal_Low)
      return (double) pressure < (double) this.pressureLethal_High;
    return false;
  }

  public void Sim1000ms(float dt)
  {
    Game.Instance.accumulators.Accumulate(this.smi.master.pressureAccumulator, this.GetPressureOverArea(this.cell));
    float averageRate = Game.Instance.accumulators.GetAverageRate(this.smi.master.pressureAccumulator);
    this.displayPressureAmount.value = averageRate;
    Game.Instance.accumulators.Accumulate(this.smi.master.elementAccumulator, !this.IsSafeElement(this.ExternalElement) ? 0.0f : 1f);
    this.smi.sm.safe_element.Set((double) Game.Instance.accumulators.GetAverageRate(this.smi.master.elementAccumulator) > 0.0, this.smi);
    double num = (double) this.smi.sm.pressure.Set(averageRate, this.smi);
  }

  public float GetExternalPressure()
  {
    return this.GetPressureOverArea(this.cell);
  }

  private float GetPressureOverArea(int cell)
  {
    PressureVulnerable.testAreaPressure = 0.0f;
    PressureVulnerable.testAreaCount = 0;
    this.occupyArea.TestArea(cell, (object) null, PressureVulnerable.testAreaCB);
    this.occupyArea.TestAreaAbove(cell, (object) null, PressureVulnerable.testAreaCB);
    PressureVulnerable.testAreaPressure = PressureVulnerable.testAreaCount <= 0 ? 0.0f : PressureVulnerable.testAreaPressure / (float) PressureVulnerable.testAreaCount;
    return PressureVulnerable.testAreaPressure;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.pressure_sensitive)
      descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.REQUIRES_PRESSURE, (object) GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_PRESSURE, (object) GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
    if (this.safe_atmospheres != null && this.safe_atmospheres.Count > 0)
    {
      string str = string.Empty;
      foreach (Element safeAtmosphere in this.safe_atmospheres)
        str = str + "\n        • " + safeAtmosphere.name;
      descriptorList.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, (object) str), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE, (object) str), Descriptor.DescriptorType.Requirement, false));
    }
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.GameInstance
  {
    public bool hasMaturity;

    public StatesInstance(PressureVulnerable master)
      : base(master)
    {
      if (Db.Get().Amounts.Maturity.Lookup(this.gameObject) == null)
        return;
      this.hasMaturity = true;
    }
  }

  public class States : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable>
  {
    public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.FloatParameter pressure;
    public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.BoolParameter safe_element;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State unsafeElement;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalLow;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalHigh;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningLow;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningHigh;
    public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State normal;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.normal;
      this.lethalLow.Enter((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State.Callback) (smi => smi.master.pressureState = PressureVulnerable.PressureState.LethalLow)).TriggerOnEnter(GameHashes.LowPressureFatal, (Func<PressureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.warningLow, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.pressureLethal_Low)).ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse);
      this.lethalHigh.Enter((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State.Callback) (smi => smi.master.pressureState = PressureVulnerable.PressureState.LethalHigh)).TriggerOnEnter(GameHashes.HighPressureFatal, (Func<PressureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.warningHigh, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.pressureLethal_High)).ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse);
      this.warningLow.Enter((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State.Callback) (smi => smi.master.pressureState = PressureVulnerable.PressureState.WarningLow)).TriggerOnEnter(GameHashes.LowPressureWarning, (Func<PressureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.lethalLow, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.pressureLethal_Low)).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.normal, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.pressureWarning_Low)).ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse);
      this.unsafeElement.ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.normal, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsTrue).TriggerOnExit(GameHashes.CorrectAtmosphere).TriggerOnEnter(GameHashes.WrongAtmosphere, (Func<PressureVulnerable.StatesInstance, object>) null);
      this.warningHigh.Enter((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State.Callback) (smi => smi.master.pressureState = PressureVulnerable.PressureState.WarningHigh)).TriggerOnEnter(GameHashes.HighPressureWarning, (Func<PressureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.lethalHigh, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.pressureLethal_High)).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.normal, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.pressureWarning_High)).ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse);
      this.normal.Enter((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State.Callback) (smi => smi.master.pressureState = PressureVulnerable.PressureState.Normal)).TriggerOnEnter(GameHashes.OptimalPressureAchieved, (Func<PressureVulnerable.StatesInstance, object>) null).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.warningHigh, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.master.pressureWarning_High)).ParamTransition<float>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>) this.pressure, this.warningLow, (StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<float>.Callback) ((smi, p) => (double) p < (double) smi.master.pressureWarning_Low)).ParamTransition<bool>((StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.Parameter<bool>) this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse);
    }
  }

  public enum PressureState
  {
    LethalLow,
    WarningLow,
    Normal,
    WarningHigh,
    LethalHigh,
  }
}
