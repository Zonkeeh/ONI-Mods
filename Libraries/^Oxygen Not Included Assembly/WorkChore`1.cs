// Decompiled with JetBrains decompiler
// Type: WorkChore`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
  public Func<Chore.Precondition.Context, bool> preemption_cb;

  public WorkChore(
    ChoreType chore_type,
    IStateMachineTarget target,
    ChoreProvider chore_provider = null,
    bool run_until_complete = true,
    System.Action<Chore> on_complete = null,
    System.Action<Chore> on_begin = null,
    System.Action<Chore> on_end = null,
    bool allow_in_red_alert = true,
    ScheduleBlockType schedule_block = null,
    bool ignore_schedule_block = false,
    bool only_when_operational = true,
    KAnimFile override_anims = null,
    bool is_preemptable = false,
    bool allow_in_context_menu = true,
    bool allow_prioritization = true,
    PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic,
    int priority_class_value = 5,
    bool ignore_building_assignment = false,
    bool add_to_daily_report = true)
  {
    ChoreType chore_type1 = chore_type;
    IStateMachineTarget target1 = target;
    ChoreProvider chore_provider1 = chore_provider;
    bool run_until_complete1 = run_until_complete;
    System.Action<Chore> on_complete1 = on_complete;
    System.Action<Chore> on_begin1 = on_begin;
    System.Action<Chore> on_end1 = on_end;
    bool is_preemptable1 = is_preemptable;
    bool allow_in_context_menu1 = allow_in_context_menu;
    bool add_to_daily_report1 = add_to_daily_report;
    // ISSUE: explicit constructor call
    base.\u002Ector(chore_type1, target1, chore_provider1, run_until_complete1, on_complete1, on_begin1, on_end1, priority_class, priority_class_value, is_preemptable1, allow_in_context_menu1, 0, add_to_daily_report1, ReportManager.ReportType.WorkTime);
    this.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
    this.onlyWhenOperational = only_when_operational;
    if (allow_prioritization)
      this.SetPrioritizable(target.GetComponent<Prioritizable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, (object) null);
    if (!allow_in_red_alert)
      this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    if (schedule_block != null)
      this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) schedule_block);
    else if (!ignore_schedule_block)
      this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) this.smi.sm.workable.Get<WorkableType>(this.smi));
    Operational component1 = target.GetComponent<Operational>();
    if (only_when_operational && (UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) component1);
    if (only_when_operational)
    {
      Deconstructable component2 = target.GetComponent<Deconstructable>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) component2);
      BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) component3);
    }
    if (!ignore_building_assignment && (UnityEngine.Object) this.smi.sm.workable.Get(this.smi).GetComponent<Assignable>() != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, (object) this.smi.sm.workable.Get<Assignable>(this.smi));
    WorkableType workableType = target as WorkableType;
    if (!((UnityEngine.Object) workableType != (UnityEngine.Object) null) || string.IsNullOrEmpty(workableType.requiredSkillPerk))
      return;
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) (HashedString) workableType.requiredSkillPerk);
  }

  public bool onlyWhenOperational { get; private set; }

  public override string ToString()
  {
    return "WorkChore<" + typeof (WorkableType).ToString() + ">";
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.worker.Set(context.consumerState.gameObject, this.smi);
    base.Begin(context);
  }

  public bool IsOperationalValid()
  {
    if (this.onlyWhenOperational)
    {
      Operational component = this.smi.master.GetComponent<Operational>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.IsOperational)
        return false;
    }
    return true;
  }

  public override bool CanPreempt(Chore.Precondition.Context context)
  {
    if (!base.CanPreempt(context) || (UnityEngine.Object) context.chore.driver == (UnityEngine.Object) null || (UnityEngine.Object) context.chore.driver == (UnityEngine.Object) context.consumerState.choreDriver)
      return false;
    Workable workable = (Workable) this.smi.sm.workable.Get<WorkableType>(this.smi);
    if ((UnityEngine.Object) workable == (UnityEngine.Object) null)
      return false;
    if (this.preemption_cb != null)
    {
      if (!this.preemption_cb(context))
        return false;
    }
    else
    {
      int num = 4;
      int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost((IApproachable) workable);
      if (navigationCost == -1 || navigationCost < num || context.consumerState.navigator.GetNavigationCost((IApproachable) workable) * 2 > navigationCost)
        return false;
    }
    return true;
  }

  public class StatesInstance : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
  {
    private KAnimFile overrideAnims;

    public StatesInstance(
      WorkChore<WorkableType> master,
      GameObject workable,
      KAnimFile override_anims)
      : base(master)
    {
      this.overrideAnims = override_anims;
      this.sm.workable.Set(workable, this.smi);
    }

    public void EnableAnimOverrides()
    {
      if (!((UnityEngine.Object) this.overrideAnims != (UnityEngine.Object) null))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).AddAnimOverrides(this.overrideAnims, 0.0f);
    }

    public void DisableAnimOverrides()
    {
      if (!((UnityEngine.Object) this.overrideAnims != (UnityEngine.Object) null))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).RemoveAnimOverrides(this.overrideAnims);
    }
  }

  public class States : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
  {
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;
    public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;
    public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.worker);
      this.approach.InitializeStates(this.worker, this.workable, this.work, (GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State) null, (CellOffset[]) null, (NavTactic) null).Update("CheckOperational", (System.Action<WorkChore<WorkableType>.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsOperationalValid())
          return;
        smi.StopSM("Building not operational");
      }), UpdateRate.SIM_200ms, false);
      this.work.Enter((StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State.Callback) (smi => smi.EnableAnimOverrides())).ToggleWork<WorkableType>(this.workable, this.success, (GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State) null, (Func<WorkChore<WorkableType>.StatesInstance, bool>) (smi => smi.master.IsOperationalValid())).Exit((StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State.Callback) (smi => smi.DisableAnimOverrides()));
      this.success.ReturnSuccess();
    }
  }
}
