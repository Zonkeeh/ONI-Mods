// Decompiled with JetBrains decompiler
// Type: DoctorStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoctorStation : Workable
{
  private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>((System.Action<DoctorStation, object>) ((component, data) => component.OnStorageChange(data)));
  public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition()
  {
    id = nameof (TreatmentAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((DoctorStation) data).IsTreatmentAvailable(context.consumerState.gameObject))
  };
  public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition()
  {
    id = nameof (DoctorAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((DoctorStation) data).IsDoctorAvailable(context.consumerState.gameObject))
  };
  private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();
  [MyCmpReq]
  public Storage storage;
  [MyCmpReq]
  public Operational operational;
  private DoctorStationDoctorWorkable doctor_workable;
  [SerializeField]
  public Tag supplyTag;
  private DoctorStation.StatesInstance smi;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    this.doctor_workable = this.GetComponent<DoctorStationDoctorWorkable>();
    this.SetWorkTime(float.PositiveInfinity);
    this.smi = new DoctorStation.StatesInstance(this);
    this.smi.StartSM();
    this.OnStorageChange((object) null);
    this.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
  }

  protected override void OnCleanUp()
  {
    Prioritizable.RemoveRef(this.gameObject);
    if (this.smi != null)
    {
      this.smi.StopSM(nameof (OnCleanUp));
      this.smi = (DoctorStation.StatesInstance) null;
    }
    base.OnCleanUp();
  }

  private void OnStorageChange(object data = null)
  {
    this.treatments_available.Clear();
    foreach (GameObject go in this.storage.items)
    {
      if (go.HasTag(GameTags.MedicalSupplies))
      {
        Tag tag = go.PrefabID();
        if (tag == (Tag) "IntermediateCure")
          this.AddTreatment("SlimeSickness", tag);
        if (tag == (Tag) "AdvancedCure")
          this.AddTreatment("ZombieSickness", tag);
      }
    }
    this.smi.sm.hasSupplies.Set(this.treatments_available.Count > 0, this.smi);
  }

  private void AddTreatment(string id, Tag tag)
  {
    if (this.treatments_available.ContainsKey((HashedString) id))
      return;
    this.treatments_available.Add((HashedString) id, tag);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.smi.sm.hasPatient.Set(true, this.smi);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.smi.sm.hasPatient.Set(false, this.smi);
  }

  public void SetHasDoctor(bool has)
  {
    this.smi.sm.hasDoctor.Set(has, this.smi);
  }

  public void CompleteDoctoring()
  {
    if (!(bool) ((UnityEngine.Object) this.worker))
      return;
    this.CompleteDoctoring(this.worker.gameObject);
  }

  private void CompleteDoctoring(GameObject target)
  {
    Sicknesses sicknesses = target.GetSicknesses();
    if (sicknesses == null)
      return;
    bool flag = false;
    foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
    {
      Tag tag;
      if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
      {
        Game.Instance.savedInfo.curedDisease = true;
        sicknessInstance.Cure();
        this.storage.ConsumeIgnoringDisease(tag, 1f);
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "Failed to treat any disease for {0}", (object) target);
  }

  public bool IsDoctorAvailable(GameObject target)
  {
    return string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk) || MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, target.GetComponent<MinionResume>());
  }

  public bool IsTreatmentAvailable(GameObject target)
  {
    Sicknesses sicknesses = target.GetSicknesses();
    if (sicknesses != null)
    {
      foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
      {
        Tag tag;
        if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
          return true;
      }
    }
    return false;
  }

  public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
  {
    public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;
    public DoctorStation.States.OperationalStates operational;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;
    public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = false;
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.operational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).DefaultState(this.operational.not_ready);
      this.operational.not_ready.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasSupplies, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational.ready, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore), (Func<DoctorStation.StatesInstance, bool>) null).ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasSupplies, this.operational.not_ready, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p));
      this.operational.ready.idle.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasPatient, (GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State) this.operational.ready.has_patient, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.has_patient.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasPatient, this.operational.ready.idle, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p)).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore), (Func<DoctorStation.StatesInstance, bool>) null);
      this.operational.ready.has_patient.waiting.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasDoctor, this.operational.ready.has_patient.being_treated, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => p));
      this.operational.ready.has_patient.being_treated.ParamTransition<bool>((StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>) this.hasDoctor, this.operational.ready.has_patient.waiting, (StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.Parameter<bool>.Callback) ((smi, p) => !p));
    }

    private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
    {
      WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, (IStateMachineTarget) smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
      workChore.AddPrecondition(DoctorStation.TreatmentAvailable, (object) smi.master);
      workChore.AddPrecondition(DoctorStation.DoctorAvailable, (object) smi.master);
      return (Chore) workChore;
    }

    private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
    {
      return (Chore) new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, (IStateMachineTarget) smi.master.GetComponent<DoctorStationDoctorWorkable>(), (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
    }

    public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;
      public DoctorStation.States.ReadyStates ready;
    }

    public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;
      public DoctorStation.States.PatientStates has_patient;
    }

    public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
    {
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;
      public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
    }
  }

  public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
  {
    public StatesInstance(DoctorStation master)
      : base(master)
    {
    }
  }
}
