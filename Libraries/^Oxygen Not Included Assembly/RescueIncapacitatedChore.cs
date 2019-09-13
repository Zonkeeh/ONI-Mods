// Decompiled with JetBrains decompiler
// Type: RescueIncapacitatedChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class RescueIncapacitatedChore : Chore<RescueIncapacitatedChore.StatesInstance>
{
  public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition()
  {
    id = nameof (CanReachIncapacitated),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      GameObject gameObject = (GameObject) data;
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return false;
      int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(gameObject.transform.GetPosition()));
      if (navigationCost == -1)
        return false;
      context.cost += navigationCost;
      return true;
    })
  };

  public RescueIncapacitatedChore(IStateMachineTarget master, GameObject incapacitatedDuplicant)
    : base(Db.Get().ChoreTypes.RescueIncapacitated, master, (ChoreProvider) null, false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new RescueIncapacitatedChore.StatesInstance(this);
    this.runUntilComplete = true;
    this.AddPrecondition(ChorePreconditions.instance.NotChoreCreator, (object) incapacitatedDuplicant.gameObject);
    this.AddPrecondition(RescueIncapacitatedChore.CanReachIncapacitated, (object) incapacitatedDuplicant);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rescuer.Set(context.consumerState.gameObject, this.smi);
    this.smi.sm.rescueTarget.Set(this.gameObject, this.smi);
    this.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<BeIncapacitatedChore.StatesInstance>().master.GetChosenClinic(), this.smi);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    this.DropIncapacitatedDuplicant();
    base.End(reason);
  }

  private void DropIncapacitatedDuplicant()
  {
    if (!((UnityEngine.Object) this.smi.sm.rescuer.Get(this.smi) != (UnityEngine.Object) null) || !((UnityEngine.Object) this.smi.sm.rescueTarget.Get(this.smi) != (UnityEngine.Object) null))
      return;
    this.smi.sm.rescuer.Get(this.smi).GetComponent<Storage>().Drop(this.smi.sm.rescueTarget.Get(this.smi), true);
  }

  public class StatesInstance : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.GameInstance
  {
    public StatesInstance(RescueIncapacitatedChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore>
  {
    public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.ApproachSubState<Chattable> approachIncapacitated;
    public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State failure;
    public RescueIncapacitatedChore.States.HoldingIncapacitated holding;
    public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter rescueTarget;
    public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter deliverTarget;
    public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter rescuer;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approachIncapacitated;
      this.approachIncapacitated.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset, (NavTactic) null).Enter((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        DeathMonitor.Instance smi1 = this.rescueTarget.GetSMI<DeathMonitor.Instance>(smi);
        if (smi1 != null && !smi1.IsDead())
          return;
        smi.StopSM("target died");
      }));
      this.holding.Target(this.rescuer).Enter((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        smi.sm.rescueTarget.Get(smi).Subscribe(1623392196, (System.Action<object>) (d => smi.GoTo((StateMachine.BaseState) this.holding.ditch)));
        KAnimFile anim = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
        smi.master.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
        smi.master.GetComponent<KAnimControllerBase>().AddAnimOverrides(anim, 0.0f);
      })).Exit((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        KAnimFile anim = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
        smi.master.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
      }));
      this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi => this.rescueTarget.Get(smi).gameObject.GetComponent<KBatchedAnimController>().Play((HashedString) "pickup", KAnim.PlayMode.Once, 1f, 0.0f))).Exit((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi), false, false, true, false);
        this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
        KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
        component.symbol = new HashedString("snapTo_pivot");
        component.offset = new Vector3(0.0f, 0.0f, 1f);
      })).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State) this.holding.delivering, (StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.Transition.ConditionCallback) null);
      this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch, (CellOffset[]) null, (NavTactic) null).Enter((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        DeathMonitor.Instance smi1 = this.rescueTarget.GetSMI<DeathMonitor.Instance>(smi);
        if (smi1 != null && !smi1.IsDead())
          return;
        smi.StopSM("target died");
      })).Update((System.Action<RescueIncapacitatedChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!((UnityEngine.Object) this.deliverTarget.Get(smi) == (UnityEngine.Object) null))
          return;
        smi.GoTo((StateMachine.BaseState) this.holding.ditch);
      }), UpdateRate.SIM_200ms, false);
      this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        smi.master.DropIncapacitatedDuplicant();
        smi.SetStatus(StateMachine.Status.Success);
        smi.StopSM("complete");
      }));
      this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, (StateMachine.BaseState) this.failure).Exit((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi => smi.master.DropIncapacitatedDuplicant()));
      this.failure.Enter((StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State.Callback) (smi =>
      {
        smi.SetStatus(StateMachine.Status.Failed);
        smi.StopSM("failed");
      }));
    }

    public class HoldingIncapacitated : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State
    {
      public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State pickup;
      public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.ApproachSubState<IApproachable> delivering;
      public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State deposit;
      public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State ditch;
    }
  }
}
