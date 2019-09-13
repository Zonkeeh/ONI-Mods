// Decompiled with JetBrains decompiler
// Type: FleeChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FleeChore : Chore<FleeChore.StatesInstance>
{
  private Navigator nav;

  public FleeChore(IStateMachineTarget target, GameObject enemy)
    : base(Db.Get().ChoreTypes.Flee, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new FleeChore.StatesInstance(this);
    this.smi.sm.self.Set(this.gameObject, this.smi);
    this.nav = this.gameObject.GetComponent<Navigator>();
    this.smi.sm.fleeFromTarget.Set(enemy, this.smi);
  }

  private bool isInFavoredDirection(int cell, int fleeFromCell)
  {
    return (double) Grid.CellToPos(fleeFromCell).x < (double) this.gameObject.transform.GetPosition().x == (double) Grid.CellToPos(fleeFromCell).x < (double) Grid.CellToPos(cell).x;
  }

  private bool CanFleeTo(int cell)
  {
    if (!this.nav.CanReach(cell) && !this.nav.CanReach(Grid.OffsetCell(cell, -1, -1)) && (!this.nav.CanReach(Grid.OffsetCell(cell, 1, -1)) && !this.nav.CanReach(Grid.OffsetCell(cell, -1, 1))))
      return this.nav.CanReach(Grid.OffsetCell(cell, 1, 1));
    return true;
  }

  public GameObject CreateLocator(Vector3 pos)
  {
    return ChoreHelpers.CreateLocator("GoToLocator", pos);
  }

  protected override void OnStateMachineStop(string reason, StateMachine.Status status)
  {
    if ((UnityEngine.Object) this.smi.sm.fleeToTarget.Get(this.smi) != (UnityEngine.Object) null)
      ChoreHelpers.DestroyLocator(this.smi.sm.fleeToTarget.Get(this.smi));
    base.OnStateMachineStop(reason, status);
  }

  public class StatesInstance : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.GameInstance
  {
    public StatesInstance(FleeChore master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore>
  {
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeFromTarget;
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeToTarget;
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter self;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State planFleeRoute;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.ApproachSubState<IApproachable> flee;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State cower;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State end;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.planFleeRoute;
      this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fleeing, (Func<FleeChore.StatesInstance, object>) null);
      this.planFleeRoute.Enter((StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State.Callback) (smi =>
      {
        int cell1 = Grid.PosToCell(this.fleeFromTarget.Get(smi));
        HashSet<int> intSet = GameUtil.FloodCollectCells(Grid.PosToCell(smi.master.gameObject), new Func<int, bool>(smi.master.CanFleeTo), 300, (HashSet<int>) null, true);
        int num1 = -1;
        int num2 = -1;
        foreach (int num3 in intSet)
        {
          if (smi.master.nav.CanReach(num3))
          {
            int num4 = Grid.GetCellDistance(num3, cell1) - 1;
            if (smi.master.isInFavoredDirection(num3, cell1))
              num4 += 8;
            if (num4 > num2)
            {
              num2 = num4;
              num1 = num3;
            }
          }
        }
        int cell2 = num1;
        if (cell2 != -1)
        {
          smi.sm.fleeToTarget.Set(smi.master.CreateLocator(Grid.CellToPos(cell2)), smi);
          smi.sm.fleeToTarget.Get(smi).name = "FleeLocator";
          if (cell2 == cell1)
            smi.GoTo((StateMachine.BaseState) this.cower);
          else
            smi.GoTo((StateMachine.BaseState) this.flee);
        }
        else
          smi.GoTo((StateMachine.BaseState) this.cower);
      }));
      this.flee.InitializeStates(this.self, this.fleeToTarget, this.cower, this.cower, (CellOffset[]) null, NavigationTactics.ReduceTravelDistance).ToggleAnims("anim_loco_run_insane_kanim", 2f);
      this.cower.ToggleAnims("anim_cringe_kanim", 4f).PlayAnim("cringe_pre").QueueAnim("cringe_loop", false, (Func<FleeChore.StatesInstance, string>) null).QueueAnim("cringe_pst", false, (Func<FleeChore.StatesInstance, string>) null).OnAnimQueueComplete(this.end);
      this.end.Enter((StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State.Callback) (smi => smi.StopSM("stopped")));
    }
  }
}
