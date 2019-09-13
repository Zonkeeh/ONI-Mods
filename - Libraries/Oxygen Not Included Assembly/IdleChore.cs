// Decompiled with JetBrains decompiler
// Type: IdleChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class IdleChore : Chore<IdleChore.StatesInstance>
{
  public IdleChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.IdleTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new IdleChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
  {
    private IdleCellSensor idleCellSensor;

    public StatesInstance(IdleChore master, GameObject idler)
      : base(master)
    {
      this.sm.idler.Set(idler, this.smi);
      this.idleCellSensor = this.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
    }

    public void UpdateNavType()
    {
      NavType currentNavType = this.GetComponent<Navigator>().CurrentNavType;
      this.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, this);
      this.sm.isOnTube.Set(currentNavType == NavType.Tube, this);
    }

    public int GetIdleCell()
    {
      return this.idleCellSensor.GetCell();
    }

    public bool HasIdleCell()
    {
      return this.idleCellSensor.GetCell() != Grid.InvalidCell;
    }
  }

  public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
  {
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;
    public IdleChore.States.IdleState idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.Target(this.idler);
      this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => smi.UpdateNavType())).Update("UpdateNavType", (System.Action<IdleChore.StatesInstance, float>) ((smi, dt) => smi.UpdateNavType()), UpdateRate.SIM_200ms, false).ToggleStateMachine((Func<IdleChore.StatesInstance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TaskAvailabilityMonitor.Instance((IStateMachineTarget) smi.master))).ToggleTag(GameTags.Idle);
      this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (Func<IdleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 15)), (System.Action<IdleChore.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.idle.move)));
      this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (Func<IdleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 15)), (System.Action<IdleChore.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.idle.move)));
      this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", (System.Action<IdleChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.HasIdleCell())
          return;
        smi.GoTo((StateMachine.BaseState) this.idle.move);
      }), UpdateRate.SIM_1000ms, false);
      this.idle.move.Transition((GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle, (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Transition.ConditionCallback) (smi => !smi.HasIdleCell()), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, (Func<IdleChore.StatesInstance, object>) null).TriggerOnExit(GameHashes.EndWalk).ToggleAnims("anim_loco_walk_kanim", 0.0f).MoveTo((Func<IdleChore.StatesInstance, int>) (smi => smi.GetIdleCell()), (GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle, (GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle, false).Exit("UpdateNavType", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => smi.UpdateNavType())).Exit("ClearWalk", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => smi.GetComponent<KBatchedAnimController>().Play((HashedString) "idle_default", KAnim.PlayMode.Once, 1f, 0.0f)));
    }

    public class IdleState : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
    {
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
    }
  }
}
