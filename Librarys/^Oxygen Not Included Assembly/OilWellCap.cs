// Decompiled with JetBrains decompiler
// Type: OilWellCap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class OilWellCap : Workable, ISingleSliderControl, IElementEmitter, ISliderControl
{
  private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>((System.Action<OilWellCap, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition()
  {
    id = nameof (AllowedToDepressurize),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((OilWellCap) data).NeedsDepressurizing())
  };
  public float addGasRate = 1f;
  public float maxGasPressure = 10f;
  public float releaseGasRate = 10f;
  [Serialize]
  private float depressurizePercent = 0.75f;
  private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
  private OilWellCap.StatesInstance smi;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Storage storage;
  public SimHashes gasElement;
  public float gasTemperature;
  private MeterController pressureMeter;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  public SimHashes Element
  {
    get
    {
      return this.gasElement;
    }
  }

  public float AverageEmitRate
  {
    get
    {
      return Game.Instance.accumulators.GetAverageRate(this.accumulator);
    }
  }

  public string SliderTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
    }
  }

  public string SliderUnits
  {
    get
    {
      return (string) UI.UNITSUFFIXES.PERCENT;
    }
  }

  public int SliderDecimalPlaces(int index)
  {
    return 0;
  }

  public float GetSliderMin(int index)
  {
    return 0.0f;
  }

  public float GetSliderMax(int index)
  {
    return 100f;
  }

  public float GetSliderValue(int index)
  {
    return this.depressurizePercent * 100f;
  }

  public void SetSliderValue(float value, int index)
  {
    this.depressurizePercent = value / 100f;
  }

  public string GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
  }

  string ISliderControl.GetSliderTooltip()
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), (object) (float) ((double) this.depressurizePercent * 100.0));
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    OilWellCap component = ((GameObject) data).GetComponent<OilWellCap>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.depressurizePercent = component.depressurizePercent;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    this.accumulator = Game.Instance.accumulators.Add("pressuregas", (KMonoBehaviour) this);
    this.showProgressBar = false;
    this.SetWorkTime(float.PositiveInfinity);
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_oil_cap_kanim")
    };
    this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.pressureMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0.0f, 0.0f, 0.0f), (string[]) null);
    this.smi = new OilWellCap.StatesInstance(this);
    this.smi.StartSM();
    this.UpdatePressurePercent();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.accumulators.Remove(this.accumulator);
    Prioritizable.RemoveRef(this.gameObject);
    base.OnCleanUp();
  }

  public void AddGasPressure(float dt)
  {
    this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, (byte) 0, 0, true, true);
    this.UpdatePressurePercent();
  }

  public void ReleaseGasPressure(float dt)
  {
    PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null && (double) primaryElement.Mass > 0.0)
    {
      float a = this.releaseGasRate * dt;
      if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
        a *= this.GetEfficiencyMultiplier(this.worker);
      float num = Mathf.Min(a, primaryElement.Mass);
      SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
      primaryElement.Mass -= num;
      Game.Instance.accumulators.Accumulate(this.accumulator, num);
      SimMessages.AddRemoveSubstance(Grid.PosToCell((KMonoBehaviour) this), ElementLoader.GetElementIndex(this.gasElement), (CellAddRemoveSubstanceEvent) null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
    }
    this.UpdatePressurePercent();
  }

  private void UpdatePressurePercent()
  {
    float percent_full = Mathf.Clamp01(this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure);
    double num = (double) this.smi.sm.pressurePercent.Set(percent_full, this.smi);
    this.pressureMeter.SetPositionPercent(percent_full);
  }

  public bool NeedsDepressurizing()
  {
    return (double) this.smi.GetPressurePercent() >= (double) this.depressurizePercent;
  }

  private WorkChore<OilWellCap> CreateWorkChore()
  {
    WorkChore<OilWellCap> workChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    workChore.AddPrecondition(OilWellCap.AllowedToDepressurize, (object) this);
    return workChore;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.smi.sm.working.Set(true, this.smi);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.smi.sm.working.Set(false, this.smi);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    return (double) this.smi.GetPressurePercent() <= 0.0;
  }

  public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
  {
    public StatesInstance(OilWellCap master)
      : base(master)
    {
    }

    public float GetPressurePercent()
    {
      return this.sm.pressurePercent.Get(this.smi);
    }
  }

  public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
  {
    public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;
    public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;
    public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;
    public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;
    public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;
    public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.ToggleRecurringChore((Func<OilWellCap.StatesInstance, Chore>) (smi => (Chore) smi.master.CreateWorkChore()), (Func<OilWellCap.StatesInstance, bool>) null);
      this.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, (object) null).ParamTransition<float>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<float>) this.pressurePercent, this.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<bool>) this.working, (GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State) this.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State) this.active, (StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.active.DefaultState(this.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, (object) null).EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).Enter((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Exit((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).Update((System.Action<OilWellCap.StatesInstance, float>) ((smi, dt) => smi.master.AddGasPressure(dt)), UpdateRate.SIM_200ms, false);
      this.active.pre.PlayAnim("working_pre").ParamTransition<float>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<float>) this.pressurePercent, this.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<bool>) this.working, (GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State) this.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.active.loop);
      this.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<float>) this.pressurePercent, this.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<bool>) this.working, this.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.active.pst, (StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
      this.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, (Func<OilWellCap.StatesInstance, string>) null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, (object) null).ParamTransition<float>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<float>) this.pressurePercent, this.idle, (StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0)).ParamTransition<bool>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<bool>) this.working, (GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State) this.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
      this.releasing_pressure.DefaultState(this.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (Func<OilWellCap.StatesInstance, object>) (smi => (object) smi.master)).ParamTransition<bool>((StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Parameter<bool>) this.working, this.idle, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update((System.Action<OilWellCap.StatesInstance, float>) ((smi, dt) => smi.master.ReleaseGasPressure(dt)), UpdateRate.SIM_200ms, false);
      this.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.releasing_pressure.loop);
      this.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.releasing_pressure.pst, (StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete((GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State) this.active);
    }
  }
}
