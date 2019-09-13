// Decompiled with JetBrains decompiler
// Type: Clinic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Clinic : Workable, IEffectDescriptor, ISingleSliderControl, ISliderControl
{
  private static readonly string[] EffectsRemoved = new string[1]
  {
    "SoreBack"
  };
  public static readonly Chore.Precondition IsOverSicknessThreshold = new Chore.Precondition()
  {
    id = nameof (IsOverSicknessThreshold),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((Clinic) data).IsHealthBelowThreshold(context.consumerState.gameObject))
  };
  public float doctorVisitInterval = 300f;
  [Serialize]
  private float sicknessSliderValue = 100f;
  [MyCmpReq]
  private Assignable assignable;
  private const int MAX_RANGE = 10;
  private const float CHECK_RANGE_INTERVAL = 10f;
  public KAnimFile[] workerInjuredAnims;
  public KAnimFile[] workerDiseasedAnims;
  public string diseaseEffect;
  public string healthEffect;
  public string doctoredDiseaseEffect;
  public string doctoredHealthEffect;
  public string doctoredPlaceholderEffect;
  private Clinic.ClinicSM.Instance clinicSMI;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = false;
    this.assignable.subSlots = new AssignableSlot[1]
    {
      Db.Get().AssignableSlots.MedicalBed
    };
    this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAutoAssignTo));
    this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    Components.Clinics.Add(this);
    this.SetWorkTime(float.PositiveInfinity);
    this.clinicSMI = new Clinic.ClinicSM.Instance(this);
    this.clinicSMI.StartSM();
  }

  protected override void OnCleanUp()
  {
    Prioritizable.RemoveRef(this.gameObject);
    Components.Clinics.Remove(this);
    base.OnCleanUp();
  }

  private KAnimFile[] GetAppropriateOverrideAnims(Worker worker)
  {
    KAnimFile[] kanimFileArray = (KAnimFile[]) null;
    if (!worker.GetSMI<WoundMonitor.Instance>().ShouldExitInfirmary())
      kanimFileArray = this.workerInjuredAnims;
    else if (this.workerDiseasedAnims != null && this.IsValidEffect(this.diseaseEffect) && worker.GetSMI<SicknessMonitor.Instance>().IsSick())
      kanimFileArray = this.workerDiseasedAnims;
    return kanimFileArray;
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    this.overrideAnims = this.GetAppropriateOverrideAnims(worker);
    return base.GetAnim(worker);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<Effects>().Add("Sleep", false);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    KAnimFile[] appropriateOverrideAnims = this.GetAppropriateOverrideAnims(worker);
    if (appropriateOverrideAnims == null || appropriateOverrideAnims != this.overrideAnims)
      return true;
    base.OnWorkTick(worker, dt);
    return false;
  }

  protected override void OnStopWork(Worker worker)
  {
    worker.GetComponent<Effects>().Remove("Sleep");
    base.OnStopWork(worker);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.assignable.Unassign();
    base.OnCompleteWork(worker);
    Effects component = worker.GetComponent<Effects>();
    for (int index = 0; index < Clinic.EffectsRemoved.Length; ++index)
    {
      string effect_id = Clinic.EffectsRemoved[index];
      component.Remove(effect_id);
    }
  }

  private Chore CreateWorkChore(
    ChoreType chore_type,
    bool allow_prioritization,
    bool allow_in_red_alert,
    PriorityScreen.PriorityClass priority_class,
    bool ignore_schedule_block = false)
  {
    ChoreType chore_type1 = chore_type;
    Clinic clinic = this;
    bool allow_prioritization1 = allow_prioritization;
    bool allow_in_red_alert1 = allow_in_red_alert;
    PriorityScreen.PriorityClass priority_class1 = priority_class;
    bool ignore_schedule_block1 = ignore_schedule_block;
    return (Chore) new WorkChore<Clinic>(chore_type1, (IStateMachineTarget) clinic, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, allow_in_red_alert1, (ScheduleBlockType) null, ignore_schedule_block1, true, (KAnimFile) null, false, true, allow_prioritization1, priority_class1, 5, false, false);
  }

  private bool CanAutoAssignTo(MinionAssignablesProxy worker)
  {
    bool flag = false;
    MinionIdentity target = worker.target as MinionIdentity;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
    {
      if (this.IsValidEffect(this.healthEffect))
      {
        Health component = target.GetComponent<Health>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.hitPoints < (double) component.maxHitPoints)
          flag = true;
      }
      if (!flag && this.IsValidEffect(this.diseaseEffect))
        flag = target.GetComponent<MinionModifiers>().sicknesses.Count > 0;
    }
    return flag;
  }

  private bool CanManuallyAssignTo(MinionAssignablesProxy worker)
  {
    bool flag = false;
    MinionIdentity target = worker.target as MinionIdentity;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      flag = this.IsHealthBelowThreshold(target.gameObject);
    return flag;
  }

  private bool IsHealthBelowThreshold(GameObject minion)
  {
    Health health = !((UnityEngine.Object) minion != (UnityEngine.Object) null) ? (Health) null : minion.GetComponent<Health>();
    if ((UnityEngine.Object) health != (UnityEngine.Object) null)
    {
      float num = health.hitPoints / health.maxHitPoints;
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        return (double) num < (double) this.MedicalAttentionMinimum;
    }
    return false;
  }

  private bool IsValidEffect(string effect)
  {
    if (effect != null)
      return effect != string.Empty;
    return false;
  }

  private bool AllowDoctoring()
  {
    if (!this.IsValidEffect(this.doctoredDiseaseEffect))
      return this.IsValidEffect(this.doctoredHealthEffect);
    return true;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descs = new List<Descriptor>();
    if (this.IsValidEffect(this.healthEffect))
      Effect.AddModifierDescriptions(this.gameObject, descs, this.healthEffect, false);
    if (this.diseaseEffect != this.healthEffect && this.IsValidEffect(this.diseaseEffect))
      Effect.AddModifierDescriptions(this.gameObject, descs, this.diseaseEffect, false);
    if (this.AllowDoctoring())
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.DOCTORING, (string) UI.BUILDINGEFFECTS.TOOLTIPS.DOCTORING, Descriptor.DescriptorType.Effect);
      descs.Add(descriptor);
      if (this.IsValidEffect(this.doctoredHealthEffect))
        Effect.AddModifierDescriptions(this.gameObject, descs, this.doctoredHealthEffect, true);
      if (this.doctoredDiseaseEffect != this.doctoredHealthEffect && this.IsValidEffect(this.doctoredDiseaseEffect))
        Effect.AddModifierDescriptions(this.gameObject, descs, this.doctoredDiseaseEffect, true);
    }
    return descs;
  }

  public float MedicalAttentionMinimum
  {
    get
    {
      return this.sicknessSliderValue / 100f;
    }
  }

  string ISliderControl.SliderTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TITLE";
    }
  }

  string ISliderControl.SliderUnits
  {
    get
    {
      return (string) UI.UNITSUFFIXES.PERCENT;
    }
  }

  int ISliderControl.SliderDecimalPlaces(int index)
  {
    return 0;
  }

  float ISliderControl.GetSliderMin(int index)
  {
    return 0.0f;
  }

  float ISliderControl.GetSliderMax(int index)
  {
    return 100f;
  }

  float ISliderControl.GetSliderValue(int index)
  {
    return this.sicknessSliderValue;
  }

  void ISliderControl.SetSliderValue(float percent, int index)
  {
    if ((double) percent == (double) this.sicknessSliderValue)
      return;
    this.sicknessSliderValue = (float) Mathf.RoundToInt(percent);
    Game.Instance.Trigger(875045922, (object) null);
  }

  string ISliderControl.GetSliderTooltip()
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP"), (object) this.sicknessSliderValue);
  }

  string ISliderControl.GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP";
  }

  public class ClinicSM : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic>
  {
    public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State unoperational;
    public Clinic.ClinicSM.OperationalStates operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = false;
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State) this.operational, (StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).Enter((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi => smi.master.GetComponent<Assignable>().Unassign()));
      this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.unoperational, (StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.AssigneeChanged, this.unoperational, (StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.Transition.ConditionCallback) null).ToggleRecurringChore((Func<Clinic.ClinicSM.Instance, Chore>) (smi => smi.master.CreateWorkChore(Db.Get().ChoreTypes.Heal, false, true, PriorityScreen.PriorityClass.personalNeeds, false)), (Func<Clinic.ClinicSM.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.healthEffect))).ToggleRecurringChore((Func<Clinic.ClinicSM.Instance, Chore>) (smi => smi.master.CreateWorkChore(Db.Get().ChoreTypes.HealCritical, false, true, PriorityScreen.PriorityClass.personalNeeds, false)), (Func<Clinic.ClinicSM.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.healthEffect))).ToggleRecurringChore((Func<Clinic.ClinicSM.Instance, Chore>) (smi => smi.master.CreateWorkChore(Db.Get().ChoreTypes.RestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true)), (Func<Clinic.ClinicSM.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.diseaseEffect))).ToggleRecurringChore((Func<Clinic.ClinicSM.Instance, Chore>) (smi => smi.master.CreateWorkChore(Db.Get().ChoreTypes.SleepDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true)), (Func<Clinic.ClinicSM.Instance, bool>) (smi => !string.IsNullOrEmpty(smi.master.diseaseEffect)));
      this.operational.idle.WorkableStartTransition((Func<Clinic.ClinicSM.Instance, Workable>) (smi => (Workable) smi.master), (GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State) this.operational.healing);
      this.operational.healing.DefaultState(this.operational.healing.undoctored).WorkableStopTransition((Func<Clinic.ClinicSM.Instance, Workable>) (smi => (Workable) smi.GetComponent<Clinic>()), this.operational.idle).Enter((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(true, false))).Exit((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(false, false)));
      this.operational.healing.undoctored.Enter((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi =>
      {
        smi.StartEffect(smi.master.healthEffect, false);
        smi.StartEffect(smi.master.diseaseEffect, false);
        bool flag = false;
        if ((UnityEngine.Object) smi.master.worker != (UnityEngine.Object) null)
          flag = smi.HasEffect(smi.master.doctoredHealthEffect) || smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredPlaceholderEffect);
        if (!smi.master.AllowDoctoring())
          return;
        if (flag)
          smi.GoTo((StateMachine.BaseState) this.operational.healing.doctored);
        else
          smi.StartDoctorChore();
      })).Exit((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi =>
      {
        smi.StopEffect(smi.master.healthEffect);
        smi.StopEffect(smi.master.diseaseEffect);
        smi.StopDoctorChore();
      }));
      this.operational.healing.newlyDoctored.Enter((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi =>
      {
        smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
        smi.StartEffect(smi.master.doctoredHealthEffect, true);
        smi.GoTo((StateMachine.BaseState) this.operational.healing.doctored);
      }));
      this.operational.healing.doctored.Enter((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi =>
      {
        Effects component = smi.master.worker.GetComponent<Effects>();
        if (!smi.HasEffect(smi.master.doctoredPlaceholderEffect))
          return;
        EffectInstance effectInstance1 = component.Get(smi.master.doctoredPlaceholderEffect);
        EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
        if (effectInstance2 != null)
        {
          float num = effectInstance1.effect.duration - effectInstance1.timeRemaining;
          effectInstance2.timeRemaining = effectInstance2.effect.duration - num;
        }
        EffectInstance effectInstance3 = smi.StartEffect(smi.master.doctoredHealthEffect, true);
        if (effectInstance3 != null)
        {
          float num = effectInstance1.effect.duration - effectInstance1.timeRemaining;
          effectInstance3.timeRemaining = effectInstance3.effect.duration - num;
        }
        component.Remove(smi.master.doctoredPlaceholderEffect);
      })).ScheduleGoTo((Func<Clinic.ClinicSM.Instance, float>) (smi =>
      {
        Effects component = smi.master.worker.GetComponent<Effects>();
        float a = smi.master.doctorVisitInterval;
        if (smi.HasEffect(smi.master.doctoredHealthEffect))
        {
          EffectInstance effectInstance = component.Get(smi.master.doctoredHealthEffect);
          a = Mathf.Min(a, effectInstance.GetTimeRemaining());
        }
        if (smi.HasEffect(smi.master.doctoredDiseaseEffect))
        {
          EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
          a = Mathf.Min(a, effectInstance.GetTimeRemaining());
        }
        return a;
      }), (StateMachine.BaseState) this.operational.healing.undoctored).Exit((StateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State.Callback) (smi =>
      {
        Effects component = smi.master.worker.GetComponent<Effects>();
        if (!smi.HasEffect(smi.master.doctoredDiseaseEffect) && !smi.HasEffect(smi.master.doctoredHealthEffect))
          return;
        EffectInstance effectInstance1 = component.Get(smi.master.doctoredDiseaseEffect) ?? component.Get(smi.master.doctoredHealthEffect);
        EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredPlaceholderEffect, true);
        effectInstance2.timeRemaining = effectInstance2.effect.duration - (effectInstance1.effect.duration - effectInstance1.timeRemaining);
        component.Remove(smi.master.doctoredDiseaseEffect);
        component.Remove(smi.master.doctoredHealthEffect);
      }));
    }

    public class OperationalStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
    {
      public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State idle;
      public Clinic.ClinicSM.HealingStates healing;
    }

    public class HealingStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
    {
      public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State undoctored;
      public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State doctored;
      public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State newlyDoctored;
    }

    public class Instance : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.GameInstance
    {
      private WorkChore<DoctorChoreWorkable> doctorChore;

      public Instance(Clinic master)
        : base(master)
      {
      }

      public void StartDoctorChore()
      {
        if (!this.master.IsValidEffect(this.master.doctoredHealthEffect) && !this.master.IsValidEffect(this.master.doctoredDiseaseEffect))
          return;
        this.doctorChore = new WorkChore<DoctorChoreWorkable>(Db.Get().ChoreTypes.Doctor, (IStateMachineTarget) this.smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
        WorkChore<DoctorChoreWorkable> doctorChore = this.doctorChore;
        doctorChore.onComplete = doctorChore.onComplete + (System.Action<Chore>) (chore => this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational.healing.newlyDoctored));
      }

      public void StopDoctorChore()
      {
        if (this.doctorChore == null)
          return;
        this.doctorChore.Cancel(nameof (StopDoctorChore));
        this.doctorChore = (WorkChore<DoctorChoreWorkable>) null;
      }

      public bool HasEffect(string effect)
      {
        bool flag = false;
        if (this.master.IsValidEffect(effect))
          flag = this.smi.master.worker.GetComponent<Effects>().HasEffect(effect);
        return flag;
      }

      public EffectInstance StartEffect(string effect, bool should_save)
      {
        if (this.master.IsValidEffect(effect))
        {
          Worker worker = this.smi.master.worker;
          if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
          {
            Effects component = worker.GetComponent<Effects>();
            if (!component.HasEffect(effect))
              return component.Add(effect, should_save);
          }
        }
        return (EffectInstance) null;
      }

      public void StopEffect(string effect)
      {
        if (!this.master.IsValidEffect(effect))
          return;
        Worker worker = this.smi.master.worker;
        if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
          return;
        Effects component = worker.GetComponent<Effects>();
        if (!component.HasEffect(effect))
          return;
        component.Remove(effect);
      }
    }
  }
}
