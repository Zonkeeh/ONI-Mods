// Decompiled with JetBrains decompiler
// Type: ManualGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ManualGenerator : Workable, ISingleSliderControl, ISliderControl
{
  private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[6]
  {
    (KAnimHashedString) "meter",
    (KAnimHashedString) "meter_target",
    (KAnimHashedString) "meter_fill",
    (KAnimHashedString) "meter_frame",
    (KAnimHashedString) "meter_light",
    (KAnimHashedString) "meter_tubing"
  };
  private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>((System.Action<ManualGenerator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>((System.Action<ManualGenerator, object>) ((component, data) => component.OnActiveChanged(data)));
  [Serialize]
  [SerializeField]
  private float batteryRefillPercent = 0.5f;
  private const float batteryStopRunningPercent = 1f;
  [MyCmpReq]
  private Generator generator;
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private BuildingEnabledButton buildingEnabledButton;
  private Chore chore;
  private int powerCell;
  private ManualGenerator.GeneratePowerSM.Instance smi;

  private ManualGenerator()
  {
    this.showProgressBar = false;
  }

  public string SliderTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
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
    return this.batteryRefillPercent * 100f;
  }

  public void SetSliderValue(float value, int index)
  {
    this.batteryRefillPercent = value / 100f;
  }

  public string GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
  }

  string ISliderControl.GetSliderTooltip()
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), (object) (float) ((double) this.batteryRefillPercent * 100.0));
  }

  public bool IsPowered
  {
    get
    {
      return this.operational.IsActive;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
    this.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    EnergyGenerator.EnsureStatusItemAvailable();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(float.PositiveInfinity);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    foreach (KAnimHashedString symbolName in ManualGenerator.symbol_names)
      component.SetSymbolVisiblity(symbolName, false);
    this.powerCell = this.GetComponent<Building>().GetPowerOutputCell();
    this.OnActiveChanged((object) null);
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_generatormanual_kanim")
    };
    this.smi = new ManualGenerator.GeneratePowerSM.Instance((IStateMachineTarget) this);
    this.smi.StartSM();
    Game.Instance.energySim.AddManualGenerator(this);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.energySim.RemoveManualGenerator(this);
    this.smi.StopSM("cleanup");
    base.OnCleanUp();
  }

  protected void OnActiveChanged(object is_active)
  {
    if (!this.operational.IsActive)
      return;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, (object) null);
  }

  public void EnergySim200ms(float dt)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.operational.IsActive)
    {
      this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
      component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, (object) this.generator);
    }
    else
    {
      this.generator.ResetJoules();
      component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, (object) null);
      if (!this.operational.IsOperational)
        return;
      CircuitManager circuitManager = Game.Instance.circuitManager;
      if (circuitManager == null)
        return;
      ushort circuitId = circuitManager.GetCircuitID(this.powerCell);
      bool flag1 = circuitManager.HasBatteries(circuitId);
      bool flag2 = false;
      if (!flag1 && circuitManager.HasConsumers(circuitId))
        flag2 = true;
      else if (flag1)
      {
        if ((double) this.batteryRefillPercent <= 0.0 && (double) circuitManager.GetMinBatteryPercentFullOnCircuit(circuitId) <= 0.0)
          flag2 = true;
        else if ((double) circuitManager.GetMinBatteryPercentFullOnCircuit(circuitId) < (double) this.batteryRefillPercent)
          flag2 = true;
      }
      if (flag2)
      {
        if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
          this.chore = (Chore) new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
      }
      else if (this.chore != null)
      {
        this.chore.Cancel("No refill needed");
        this.chore = (Chore) null;
      }
      component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, (object) null);
    }
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.operational.SetActive(true, false);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    CircuitManager circuitManager = Game.Instance.circuitManager;
    bool flag1 = false;
    if (circuitManager != null)
    {
      ushort circuitId = circuitManager.GetCircuitID(this.powerCell);
      bool flag2 = circuitManager.HasBatteries(circuitId);
      flag1 = flag2 && (double) circuitManager.GetMinBatteryPercentFullOnCircuit(circuitId) < 1.0 || !flag2 && circuitManager.HasConsumers(circuitId);
    }
    AttributeLevels component = worker.GetComponent<AttributeLevels>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
    return !flag1;
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.operational.SetActive(false, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.operational.SetActive(false, false);
    if (this.chore == null)
      return;
    this.chore.Cancel("complete");
    this.chore = (Chore) null;
  }

  private void OnOperationalChanged(object data)
  {
    if (this.buildingEnabledButton.IsEnabled)
      return;
    this.generator.ResetJoules();
  }

  public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
  {
    public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;
    public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;
    public ManualGenerator.GeneratePowerSM.WorkingStates working;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.serializable = true;
      this.off.EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Operational>().IsOperational)).PlayAnim("off");
      this.on.EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.working.pre, (StateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Operational>().IsActive)).PlayAnim("on");
      this.working.DefaultState(this.working.pre);
      this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
      this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (StateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi =>
      {
        if ((UnityEngine.Object) this.masterTarget.Get(smi) != (UnityEngine.Object) null)
          return !smi.master.GetComponent<Operational>().IsActive;
        return false;
      }));
    }

    public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
    {
      public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;
      public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;
      public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
    }

    public class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
    {
      public Instance(IStateMachineTarget master)
        : base(master)
      {
      }
    }
  }
}
