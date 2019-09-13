// Decompiled with JetBrains decompiler
// Type: RancherChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class RancherChore : Chore<RancherChore.RancherChoreStates.Instance>
{
  public Chore.Precondition IsCreatureAvailableForRanching = new Chore.Precondition()
  {
    id = nameof (IsCreatureAvailableForRanching),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_RANCHING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as RanchStation.Instance).IsCreatureAvailableForRanching())
  };

  public RancherChore(KPrefabID rancher_station)
    : base(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) rancher_station, (ChoreProvider) null, false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.AddPrecondition(this.IsCreatureAvailableForRanching, (object) rancher_station.GetSMI<RanchStation.Instance>());
    this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanUseRanchStation.Id);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) rancher_station.GetComponent<Building>());
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) rancher_station.GetComponent<Operational>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) rancher_station.GetComponent<Deconstructable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) rancher_station.GetComponent<BuildingEnabledButton>());
    this.smi = new RancherChore.RancherChoreStates.Instance(rancher_station);
    this.SetPrioritizable(rancher_station.GetComponent<Prioritizable>());
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rancher.Set(context.consumerState.gameObject, this.smi);
    base.Begin(context);
  }

  public class RancherChoreStates : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance>
  {
    public StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State movetoranch;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;
    private RancherChore.RancherChoreStates.RanchState ranchcreature;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State wavegoodbye;
    private GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.movetoranch;
      this.Target(this.rancher);
      this.root.Exit("TriggerRanchStationNoLongerAvailable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TriggerRanchStationNoLongerAvailable()));
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state1 = this.movetoranch.MoveTo((Func<RancherChore.RancherChoreStates.Instance, int>) (smi => Grid.PosToCell(smi.transform.GetPosition())), this.waitforcreature_pre, (GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) null, false);
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables1 = this.checkformoreranchables;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache0 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache0;
      state1.Transition(checkformoreranchables1, fMgCache0, UpdateRate.SIM_1000ms);
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state2 = this.waitforcreature_pre.EnterTransition((GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) null, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.ranchStation.IsNullOrStopped()));
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables2 = this.checkformoreranchables;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache1 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache1;
      state2.Transition(checkformoreranchables2, fMgCache1, UpdateRate.SIM_1000ms).EnterTransition(this.waitforcreature, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => true));
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature = this.waitforcreature;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables3 = this.checkformoreranchables;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache2 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache2;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state3 = waitforcreature.Transition(checkformoreranchables3, fMgCache2, UpdateRate.SIM_1000ms).ToggleAnims("anim_interacts_rancherstation_kanim", 0.0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.FaceCreature);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCache3 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache3;
      state3.Enter(fMgCache3).Enter("TellCreatureToGoGetRanched", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ranchStation.SetRancherIsAvailableForRanching())).Exit("ClearRancherIsAvailableForRanching", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ranchStation.ClearRancherIsAvailableForRanching())).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, (GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State) this.ranchcreature, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
      RancherChore.RancherChoreStates.RanchState ranchcreature = this.ranchcreature;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State checkformoreranchables4 = this.checkformoreranchables;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RancherChore.RancherChoreStates.HasCreatureLeft);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback fMgCache4 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache4;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state4 = ranchcreature.Transition(checkformoreranchables4, fMgCache4, UpdateRate.SIM_1000ms);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache5 = new Func<RancherChore.RancherChoreStates.Instance, HashedString>(RancherChore.RancherChoreStates.GetRancherInteractAnim);
      }
      // ISSUE: reference to a compiler-generated field
      Func<RancherChore.RancherChoreStates.Instance, HashedString> fMgCache5 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache5;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state5 = state4.ToggleAnims(fMgCache5).DefaultState(this.ranchcreature.pre).EventTransition(GameHashes.CreatureAbandonedRanchStation, this.checkformoreranchables, (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.SetCreatureLayer);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCache6 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache6;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state6 = state5.Enter(fMgCache6);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache7 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.ClearCreatureLayer);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCache7 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache7;
      state6.Exit(fMgCache7);
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pre = this.ranchcreature.pre;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache8 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.FaceCreature);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCache8 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache8;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state7 = pre.Enter(fMgCache8);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache9 = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingPre);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCache9 = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cache9;
      state7.Enter(fMgCache9).QueueAnim("working_pre", false, (Func<RancherChore.RancherChoreStates.Instance, string>) null).OnAnimQueueComplete(this.ranchcreature.loop);
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state8 = this.ranchcreature.loop.Enter("TellCreatureRancherIsReady", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TellCreatureRancherIsReady()));
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheA == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheA = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingLoop);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCacheA = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheA;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state9 = state8.Enter(fMgCacheA);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheB == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheB = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayRancherWorkingLoops);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCacheB = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheB;
      state9.Enter(fMgCacheB).Target(this.rancher).OnAnimQueueComplete(this.ranchcreature.pst);
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst = this.ranchcreature.pst;
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheC == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheC = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.RanchCreature);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCacheC = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheC;
      GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State state10 = pst.Enter(fMgCacheC);
      // ISSUE: reference to a compiler-generated field
      if (RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheD == null)
      {
        // ISSUE: reference to a compiler-generated field
        RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheD = new StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback(RancherChore.RancherChoreStates.PlayBuildingWorkingPst);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback fMgCacheD = RancherChore.RancherChoreStates.\u003C\u003Ef__mg\u0024cacheD;
      state10.Enter(fMgCacheD).QueueAnim("working_pst", false, (Func<RancherChore.RancherChoreStates.Instance, string>) null).QueueAnim("wipe_brow", false, (Func<RancherChore.RancherChoreStates.Instance, string>) null).OnAnimQueueComplete(this.checkformoreranchables);
      this.checkformoreranchables.Enter("FindRanchable", (StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.CheckForMoreRanchables())).Update("FindRanchable", (System.Action<RancherChore.RancherChoreStates.Instance, float>) ((smi, dt) => smi.CheckForMoreRanchables()), UpdateRate.SIM_200ms, false);
    }

    private static bool HasCreatureLeft(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!smi.ranchStation.targetRanchable.IsNullOrStopped())
        return !smi.ranchStation.targetRanchable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>();
      return true;
    }

    private static void SetCreatureLayer(RancherChore.RancherChoreStates.Instance smi)
    {
      if (smi.ranchStation.targetRanchable.IsNullOrStopped())
        return;
      smi.ranchStation.targetRanchable.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    }

    private static void ClearCreatureLayer(RancherChore.RancherChoreStates.Instance smi)
    {
      if (smi.ranchStation.targetRanchable.IsNullOrStopped())
        return;
      smi.ranchStation.targetRanchable.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
    }

    private static HashedString GetRancherInteractAnim(
      RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.rancherInteractAnim;
    }

    private static void FaceCreature(RancherChore.RancherChoreStates.Instance smi)
    {
      smi.sm.rancher.Get<Facing>(smi).Face(smi.ranchStation.targetRanchable.transform.GetPosition());
    }

    private static void RanchCreature(RancherChore.RancherChoreStates.Instance smi)
    {
      Debug.Assert(smi.ranchStation != null, (object) "smi.ranchStation was null");
      RanchableMonitor.Instance targetRanchable = smi.ranchStation.targetRanchable;
      if (targetRanchable.IsNullOrStopped())
        return;
      KPrefabID component = targetRanchable.GetComponent<KPrefabID>();
      smi.sm.rancher.Get(smi).Trigger(937885943, (object) component.PrefabTag.Name);
      smi.ranchStation.RanchCreature();
    }

    private static bool ShouldSynchronizeBuilding(RancherChore.RancherChoreStates.Instance smi)
    {
      return smi.ranchStation.def.synchronizeBuilding;
    }

    private static void PlayBuildingWorkingPre(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_pre", KAnim.PlayMode.Once, 1f, 0.0f);
    }

    private static void PlayRancherWorkingLoops(RancherChore.RancherChoreStates.Instance smi)
    {
      KBatchedAnimController kbatchedAnimController = smi.sm.rancher.Get<KBatchedAnimController>(smi);
      for (int index = 0; index < smi.ranchStation.def.interactLoopCount; ++index)
        kbatchedAnimController.Queue((HashedString) "working_loop", KAnim.PlayMode.Once, 1f, 0.0f);
    }

    private static void PlayBuildingWorkingLoop(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_loop", KAnim.PlayMode.Loop, 1f, 0.0f);
    }

    private static void PlayBuildingWorkingPst(RancherChore.RancherChoreStates.Instance smi)
    {
      if (!RancherChore.RancherChoreStates.ShouldSynchronizeBuilding(smi))
        return;
      smi.ranchStation.GetComponent<KBatchedAnimController>().Queue((HashedString) "working_pst", KAnim.PlayMode.Once, 1f, 0.0f);
    }

    private class RanchState : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State
    {
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pre;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State loop;
      public GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.State pst;
    }

    public class Instance : GameStateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.GameInstance
    {
      public RanchStation.Instance ranchStation;

      public Instance(KPrefabID rancher_station)
        : base((IStateMachineTarget) rancher_station)
      {
        this.ranchStation = rancher_station.GetSMI<RanchStation.Instance>();
      }

      public void CheckForMoreRanchables()
      {
        this.ranchStation.FindRanchable();
        if (this.ranchStation.IsCreatureAvailableForRanching())
          this.GoTo((StateMachine.BaseState) this.sm.movetoranch);
        else
          this.GoTo((StateMachine.BaseState) null);
      }

      public void TriggerRanchStationNoLongerAvailable()
      {
        this.ranchStation.TriggerRanchStationNoLongerAvailable();
      }

      public void TellCreatureRancherIsReady()
      {
        if (this.ranchStation.targetRanchable.IsNullOrStopped())
          return;
        this.ranchStation.targetRanchable.Trigger(1084749845, (object) null);
      }
    }
  }
}
