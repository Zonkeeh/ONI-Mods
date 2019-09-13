// Decompiled with JetBrains decompiler
// Type: FallMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;
  public FallMonitor.EntombedStates entombed;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;
  public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;
  public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.standing;
    this.root.EventTransition(GameHashes.OnStore, this.instorage, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).Update("CheckLanded", (System.Action<FallMonitor.Instance, float>) ((smi, dt) => smi.UpdateFalling()), UpdateRate.SIM_33ms, true);
    this.standing.ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, (GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State) this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
    this.falling_pre.Enter("StopNavigator", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Stop(false))).Enter("AttemptInitialRecovery", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.AttemptInitialRecovery())).GoTo(this.falling).ToggleBrain("falling_pre");
    this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true, (Func<FallMonitor.Instance, string>) null).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, (GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State) this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.CanRecoverToLadder()), UpdateRate.SIM_33ms).Transition(this.recoverpole, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.CanRecoverToPole()), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
    this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Recover())).EventTransition(GameHashes.DestinationReached, this.standing, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.NavigationFailed, this.standing, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).Exit((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RecoverEmote()));
    this.landfloor.Enter("Land", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.LandFloor())).GoTo(this.standing);
    this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.MountLadder())).OnAnimQueueComplete(this.standing);
    this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.MountPole())).OnAnimQueueComplete(this.standing);
    this.instorage.EventTransition(GameHashes.OnStore, this.standing, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
    this.entombed.DefaultState(this.entombed.recovering);
    this.entombed.recovering.Enter("TryEntombedEscape", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TryEntombedEscape()));
    this.entombed.stuck.Enter("StopNavigator", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Stop(false))).ToggleChore((Func<FallMonitor.Instance, Chore>) (smi => (Chore) new EntombedChore(smi.master)), this.standing).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
  }

  public class EntombedStates : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;
    public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
  }

  public class Instance : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private CellOffset[] entombedEscapeOffsets = new CellOffset[9]
    {
      new CellOffset(0, 1),
      new CellOffset(0, -1),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1),
      new CellOffset(1, -1),
      new CellOffset(-1, -1),
      new CellOffset(0, 2)
    };
    private Navigator navigator;
    private bool flipRecoverEmote;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.navigator = this.GetComponent<Navigator>();
      Pathfinding.Instance.FlushNavGridsOnLoad();
    }

    public void Recover()
    {
      int cell1 = Grid.PosToCell((KMonoBehaviour) this.navigator);
      foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
      {
        if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
        {
          int cell2 = transition.IsValid(cell1, this.navigator.NavGrid.NavTable);
          if (Grid.InvalidCell != cell2)
          {
            Vector2I xy = Grid.CellToXY(cell1);
            this.flipRecoverEmote = Grid.CellToXY(cell2).x < xy.x;
            this.navigator.BeginTransition(transition);
            break;
          }
        }
      }
    }

    public void RecoverEmote()
    {
      if (UnityEngine.Random.Range(0, 9) != 8)
        return;
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) this.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_react_floor_missing_kanim", new HashedString[1]
      {
        (HashedString) "react"
      }, KAnim.PlayMode.Once, (this.flipRecoverEmote ? 1 : 0) != 0);
    }

    public void LandFloor()
    {
      this.navigator.SetCurrentNavType(NavType.Floor);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public void AttemptInitialRecovery()
    {
      if (this.gameObject.HasTag(GameTags.Incapacitated))
        return;
      int cell = Grid.PosToCell((KMonoBehaviour) this.navigator);
      foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
      {
        if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
        {
          int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
          if (Grid.InvalidCell != num)
          {
            this.smi.GoTo((StateMachine.BaseState) this.smi.sm.recoverinitialfall);
            break;
          }
        }
      }
    }

    public bool CanRecoverToLadder()
    {
      if (this.navigator.NavGrid.NavTable.IsValid(Grid.PosToCell(this.master.transform.GetPosition()), NavType.Ladder))
        return !this.gameObject.HasTag(GameTags.Incapacitated);
      return false;
    }

    public void MountLadder()
    {
      this.navigator.SetCurrentNavType(NavType.Ladder);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public bool CanRecoverToPole()
    {
      if (this.navigator.NavGrid.NavTable.IsValid(Grid.PosToCell(this.master.transform.GetPosition()), NavType.Pole))
        return !this.gameObject.HasTag(GameTags.Incapacitated);
      return false;
    }

    public void MountPole()
    {
      this.navigator.SetCurrentNavType(NavType.Pole);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public bool IsFalling()
    {
      if (this.navigator.IsMoving())
        return false;
      int cell = Grid.PosToCell(this.master.transform.GetPosition());
      if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellBelow(cell)))
        return false;
      return !this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType);
    }

    public void UpdateFalling()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (!this.navigator.IsMoving())
      {
        int cell1 = Grid.PosToCell(this.transform.GetPosition());
        int cell2 = Grid.CellAbove(cell1);
        bool flag3 = this.navigator.NavGrid.NavTable.IsValid(cell1, this.navigator.CurrentNavType) && (!this.gameObject.HasTag(GameTags.Incapacitated) ? 0 : (this.navigator.CurrentNavType == NavType.Ladder ? 1 : (this.navigator.CurrentNavType == NavType.Pole ? 1 : 0))) == 0;
        flag2 = !flag3 && (Grid.IsValidCell(cell1) && Grid.Solid[cell1] || Grid.IsValidCell(cell2) && Grid.Solid[cell2]);
        flag1 = !flag3 && !flag2;
      }
      this.sm.isFalling.Set(flag1, this.smi);
      this.sm.isEntombed.Set(flag2, this.smi);
    }

    private bool IsValidNavCell(int cell)
    {
      return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType);
    }

    public void TryEntombedEscape()
    {
      int cell1 = Grid.PosToCell(this.transform.GetPosition());
      foreach (CellOffset entombedEscapeOffset in this.entombedEscapeOffsets)
      {
        if (Grid.IsCellOffsetValid(cell1, entombedEscapeOffset))
        {
          int cell2 = Grid.OffsetCell(cell1, entombedEscapeOffset);
          if (this.IsValidNavCell(cell2))
          {
            this.transform.SetPosition(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Move));
            this.transform.GetComponent<Navigator>().Stop(false);
            this.UpdateFalling();
            this.GoTo((StateMachine.BaseState) this.sm.standing);
            return;
          }
        }
      }
      foreach (CellOffset entombedEscapeOffset in this.entombedEscapeOffsets)
      {
        if (Grid.IsCellOffsetValid(cell1, entombedEscapeOffset))
        {
          int cell2 = Grid.OffsetCell(cell1, entombedEscapeOffset);
          int cell3 = Grid.CellAbove(cell2);
          if (Grid.IsValidCell(cell3) && !Grid.Solid[cell2] && !Grid.Solid[cell3])
          {
            this.transform.SetPosition(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Move));
            this.transform.GetComponent<Navigator>().Stop(false);
            this.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
            this.UpdateFalling();
            this.GoTo((StateMachine.BaseState) this.sm.standing);
            return;
          }
        }
      }
      this.GoTo((StateMachine.BaseState) this.sm.entombed.stuck);
    }
  }
}
