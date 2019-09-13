// Decompiled with JetBrains decompiler
// Type: FixedCaptureChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class FixedCaptureChore : Chore<FixedCaptureChore.FixedCaptureChoreStates.Instance>
{
  public Chore.Precondition IsCreatureAvailableForFixedCapture = new Chore.Precondition()
  {
    id = nameof (IsCreatureAvailableForFixedCapture),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as FixedCapturePoint.Instance).IsCreatureAvailableForFixedCapture())
  };

  public FixedCaptureChore(KPrefabID capture_point)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) capture_point, (ChoreProvider) null, false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.AddPrecondition(this.IsCreatureAvailableForFixedCapture, (object) capture_point.GetSMI<FixedCapturePoint.Instance>());
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanWrangleCreatures.Id);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) capture_point.GetComponent<Building>());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) capture_point.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) capture_point.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) capture_point.GetComponent<BuildingEnabledButton>());
    this.smi = new FixedCaptureChore.FixedCaptureChoreStates.Instance(capture_point);
    this.SetPrioritizable(capture_point.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi);
    this.smi.sm.creature.Set(this.smi.fixedCapturePoint.targetCapturable.gameObject, this.smi);
    base.Begin(context);
  }

  public class FixedCaptureChoreStates : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance>
  {
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter creature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State movetopoint;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State capturecreature;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed;
    private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.movetopoint;
      this.Target(this.rancher);
      this.root.Exit("ResetCapturePoint", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.ResetCapturePoint()));
      this.movetopoint.MoveTo((Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, int>) (smi => Grid.PosToCell(smi.transform.GetPosition())), this.waitforcreature_pre, (GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null, false).Target(this.masterTarget).EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
      GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State state1 = this.waitforcreature_pre.EnterTransition((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.IsNullOrStopped()));
      GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed1 = this.failed;
      // ISSUE: reference to a compiler-generated field
      if (FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache0;
      state1.EnterTransition(failed1, fMgCache0).EnterTransition(this.waitforcreature, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => true));
      GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State state2 = this.waitforcreature.ToggleAnims("anim_interacts_rancherstation_kanim", 0.0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop);
      GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed2 = this.failed;
      // ISSUE: reference to a compiler-generated field
      if (FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = FixedCaptureChore.FixedCaptureChoreStates.\u003C\u003Ef__mg\u0024cache1;
      state2.Transition(failed2, fMgCache1, UpdateRate.SIM_200ms).Face(this.creature, 0.0f).Enter("SetRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.SetRancherIsAvailableForCapturing())).Exit("ClearRancherIsAvailableForCapturing", (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.fixedCapturePoint.ClearRancherIsAvailableForCapturing())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtCapturePoint, this.capturecreature, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
      this.capturecreature.EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).EnterTransition(this.failed, (StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.fixedCapturePoint.targetCapturable.IsNullOrStopped())).ToggleWork<Capturable>(this.creature, this.success, this.failed, (Func<FixedCaptureChore.FixedCaptureChoreStates.Instance, bool>) null);
      this.failed.GoTo((GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State) null);
      this.success.ReturnSuccess();
    }

    private static bool HasCreatureLeft(
      FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
    {
      if (!smi.fixedCapturePoint.targetCapturable.IsNullOrStopped())
        return !smi.fixedCapturePoint.targetCapturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>();
      return true;
    }

    public class Instance : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      public FixedCapturePoint.Instance fixedCapturePoint;

      public Instance(KPrefabID capture_point)
        : base((IStateMachineTarget) capture_point)
      {
        this.fixedCapturePoint = capture_point.GetSMI<FixedCapturePoint.Instance>();
      }
    }
  }
}
