// Decompiled with JetBrains decompiler
// Type: AggressiveChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class AggressiveChore : Chore<AggressiveChore.StatesInstance>
{
  public AggressiveChore(IStateMachineTarget target, System.Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.StressActingOut, target, target.GetComponent<ChoreProvider>(), false, on_complete, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new AggressiveChore.StatesInstance(this, target.gameObject);
  }

  public override void Cleanup()
  {
    base.Cleanup();
  }

  public void PunchWallDamage(float dt)
  {
    if (!Grid.Solid[this.smi.sm.wallCellToBreak] || Grid.StrengthInfo[this.smi.sm.wallCellToBreak] >= (byte) 100)
      return;
    WorldDamage instance = WorldDamage.Instance;
    int wallCellToBreak1 = this.smi.sm.wallCellToBreak;
    float num1 = 0.06f * dt;
    int wallCellToBreak2 = this.smi.sm.wallCellToBreak;
    string minionDestruction1 = (string) BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION;
    int cell = wallCellToBreak1;
    double num2 = (double) num1;
    int src_cell = wallCellToBreak2;
    string source_name = minionDestruction1;
    string minionDestruction2 = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION;
    double num3 = (double) instance.ApplyDamage(cell, (float) num2, src_cell, -1, source_name, minionDestruction2);
  }

  public class StatesInstance : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.GameInstance
  {
    public StatesInstance(AggressiveChore master, GameObject breaker)
      : base(master)
    {
      this.sm.breaker.Set(breaker, this.smi);
    }

    public void FindBreakable()
    {
      Navigator navigator = this.GetComponent<Navigator>();
      int num = int.MaxValue;
      Breakable breakable1 = (Breakable) null;
      if (UnityEngine.Random.Range(0, 100) >= 50)
      {
        foreach (Breakable breakable2 in Components.Breakables.Items)
        {
          if (!((UnityEngine.Object) breakable2 == (UnityEngine.Object) null) && !breakable2.isBroken())
          {
            int navigationCost = navigator.GetNavigationCost((IApproachable) breakable2);
            if (navigationCost != -1 && navigationCost < num)
            {
              num = navigationCost;
              breakable1 = breakable2;
            }
          }
        }
      }
      if ((UnityEngine.Object) breakable1 == (UnityEngine.Object) null)
      {
        this.sm.moveToWallTarget.Set(GameUtil.FloodFillFind<object>((Func<int, object, bool>) ((cell, arg) => !Grid.Solid[cell] && navigator.CanReach(cell) && (Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellLeft(cell)] || Grid.IsValidCell(Grid.CellRight(cell)) && Grid.Solid[Grid.CellRight(cell)] || (Grid.IsValidCell(Grid.OffsetCell(cell, 1, 1)) && Grid.Solid[Grid.OffsetCell(cell, 1, 1)] || Grid.IsValidCell(Grid.OffsetCell(cell, -1, 1)) && Grid.Solid[Grid.OffsetCell(cell, -1, 1)]))), (object) null, Grid.PosToCell(navigator.gameObject), 128, true, true), this.smi);
        this.GoTo((StateMachine.BaseState) this.sm.move_notarget);
      }
      else
      {
        this.sm.breakable.Set((KMonoBehaviour) breakable1, this.smi);
        this.GoTo((StateMachine.BaseState) this.sm.move_target);
      }
    }
  }

  public class States : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore>
  {
    public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.TargetParameter breaker;
    public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.TargetParameter breakable;
    public StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.IntParameter moveToWallTarget;
    public int wallCellToBreak;
    public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.ApproachSubState<Breakable> move_target;
    public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State move_notarget;
    public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State findbreakable;
    public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State noTarget;
    public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State breaking;
    public AggressiveChore.States.BreakingWall breaking_wall;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.findbreakable;
      this.Target(this.breaker);
      this.root.ToggleAnims("anim_loco_destructive_kanim", 0.0f);
      this.noTarget.Enter((StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State.Callback) (smi => smi.StopSM("complete/no more food")));
      this.findbreakable.Enter("FindBreakable", (StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State.Callback) (smi => smi.FindBreakable()));
      this.move_notarget.MoveTo((Func<AggressiveChore.StatesInstance, int>) (smi => smi.sm.moveToWallTarget.Get(smi)), (GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State) this.breaking_wall, this.noTarget, false);
      this.move_target.InitializeStates(this.breaker, this.breakable, this.breaking, this.findbreakable, (CellOffset[]) null, (NavTactic) null).ToggleStatusItem(Db.Get().DuplicantStatusItems.LashingOut, (object) null);
      this.breaking_wall.DefaultState(this.breaking_wall.Pre).Enter((StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State.Callback) (smi =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (Grid.Solid[Grid.OffsetCell(cell, 1, 0)])
        {
          smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_low_kanim"), 0.0f);
          this.wallCellToBreak = Grid.OffsetCell(cell, 1, 0);
        }
        else if (Grid.Solid[Grid.OffsetCell(cell, -1, 0)])
        {
          smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_low_kanim"), 0.0f);
          this.wallCellToBreak = Grid.OffsetCell(cell, -1, 0);
        }
        else if (Grid.Solid[Grid.OffsetCell(cell, 1, 1)])
        {
          smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_high_kanim"), 0.0f);
          this.wallCellToBreak = Grid.OffsetCell(cell, 1, 1);
        }
        else if (Grid.Solid[Grid.OffsetCell(cell, -1, 1)])
        {
          smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_high_kanim"), 0.0f);
          this.wallCellToBreak = Grid.OffsetCell(cell, -1, 1);
        }
        smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.wallCellToBreak));
      })).Exit((StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State.Callback) (smi =>
      {
        smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_high_kanim"));
        smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_out_of_reach_destructive_low_kanim"));
      }));
      this.breaking_wall.Pre.PlayAnim("working_pre").OnAnimQueueComplete(this.breaking_wall.Loop);
      this.breaking_wall.Loop.ScheduleGoTo(26f, (StateMachine.BaseState) this.breaking_wall.Pst).Update("PunchWallDamage", (System.Action<AggressiveChore.StatesInstance, float>) ((smi, dt) => smi.master.PunchWallDamage(dt)), UpdateRate.SIM_1000ms, false).Enter((StateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State.Callback) (smi => smi.Play("working_loop", KAnim.PlayMode.Loop))).Update((System.Action<AggressiveChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (Grid.Solid[smi.sm.wallCellToBreak])
          return;
        smi.GoTo((StateMachine.BaseState) this.breaking_wall.Pst);
      }), UpdateRate.SIM_200ms, false);
      this.breaking_wall.Pst.QueueAnim("working_pst", false, (Func<AggressiveChore.StatesInstance, string>) null).OnAnimQueueComplete(this.noTarget);
      this.breaking.ToggleWork<Breakable>(this.breakable, (GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State) null, (GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State) null, (Func<AggressiveChore.StatesInstance, bool>) null);
    }

    public class BreakingWall : GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State
    {
      public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Pre;
      public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Loop;
      public GameStateMachine<AggressiveChore.States, AggressiveChore.StatesInstance, AggressiveChore, object>.State Pst;
    }
  }
}
