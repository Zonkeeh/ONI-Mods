// Decompiled with JetBrains decompiler
// Type: MingleChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

public class MingleChore : Chore<MingleChore.StatesInstance>, IWorkerPrioritizable
{
  private int basePriority = RELAXATION.PRIORITY.TIER1;
  private Chore.Precondition HasMingleCell = new Chore.Precondition()
  {
    id = nameof (HasMingleCell),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_MINGLE_CELL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((Chore<MingleChore.StatesInstance>) data).smi.HasMingleCell())
  };

  public MingleChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Relax, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new MingleChore.StatesInstance(this, target.gameObject);
    this.AddPrecondition(this.HasMingleCell, (object) this);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Recreation);
    this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) this);
  }

  protected override StatusItem GetStatusItem()
  {
    return Db.Get().DuplicantStatusItems.Mingling;
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    return true;
  }

  public class States : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore>
  {
    public StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.TargetParameter mingler;
    public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State mingle;
    public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State move;
    public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State walk;
    public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State onfloor;
    public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.mingle;
      this.Target(this.mingler);
      this.root.EventTransition(GameHashes.ScheduleBlocksChanged, (GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State) null, (StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime()));
      this.mingle.Transition(this.walk, (StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.Transition.ConditionCallback) (smi => smi.IsSameRoom()), UpdateRate.SIM_200ms).Transition(this.move, (StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.Transition.ConditionCallback) (smi => !smi.IsSameRoom()), UpdateRate.SIM_200ms);
      this.move.Transition((GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State) null, (StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.Transition.ConditionCallback) (smi => !smi.HasMingleCell()), UpdateRate.SIM_200ms).MoveTo((Func<MingleChore.StatesInstance, int>) (smi => smi.GetMingleCell()), this.onfloor, (GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State) null, false);
      this.walk.Transition((GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State) null, (StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.Transition.ConditionCallback) (smi => !smi.HasMingleCell()), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, (Func<MingleChore.StatesInstance, object>) null).TriggerOnExit(GameHashes.EndWalk).ToggleAnims("anim_loco_walk_kanim", 0.0f).MoveTo((Func<MingleChore.StatesInstance, int>) (smi => smi.GetMingleCell()), this.onfloor, (GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State) null, false);
      this.onfloor.ToggleAnims("anim_generic_convo_kanim", 0.0f).PlayAnim("idle", KAnim.PlayMode.Loop).ScheduleGoTo((Func<MingleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 10)), (StateMachine.BaseState) this.success).ToggleTag(GameTags.AlwaysConverse);
      this.success.ReturnSuccess();
    }
  }

  public class StatesInstance : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.GameInstance
  {
    private MingleCellSensor mingleCellSensor;
    private GameObject mingler;

    public StatesInstance(MingleChore master, GameObject mingler)
      : base(master)
    {
      this.mingler = mingler;
      this.sm.mingler.Set(mingler, this.smi);
      this.mingleCellSensor = this.GetComponent<Sensors>().GetSensor<MingleCellSensor>();
    }

    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public int GetMingleCell()
    {
      return this.mingleCellSensor.GetCell();
    }

    public bool HasMingleCell()
    {
      return this.mingleCellSensor.GetCell() != Grid.InvalidCell;
    }

    public bool IsSameRoom()
    {
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.mingler));
      CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetMingleCell());
      if (cavityForCell1 != null && cavityForCell2 != null)
        return cavityForCell1.handle == cavityForCell2.handle;
      return false;
    }
  }
}
