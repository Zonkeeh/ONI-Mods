// Decompiled with JetBrains decompiler
// Type: NestingPoopState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;

internal class NestingPoopState : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>
{
  public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State goingtopoop;
  public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State pooping;
  public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State behaviourcomplete;
  public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State failedtonest;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtopoop;
    this.goingtopoop.MoveTo((Func<NestingPoopState.Instance, int>) (smi => smi.GetPoopPosition()), this.pooping, this.failedtonest, false);
    this.failedtonest.Enter((StateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State.Callback) (smi => smi.SetLastPoopCell())).GoTo(this.pooping);
    GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State state = this.pooping.Enter((StateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State.Callback) (smi => smi.master.GetComponent<Facing>().SetFacing(Grid.PosToCell(smi.master.gameObject) > smi.targetPoopCell)));
    string name1 = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, NestingPoopState.Instance, string>) null, (Func<string, NestingPoopState.Instance, string>) null, category).PlayAnim("poop").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.Enter((StateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State.Callback) (smi => smi.SetLastPoopCell())).PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag nestingPoopElement = Tag.Invalid;

    public Def(Tag tag)
    {
      this.nestingPoopElement = tag;
    }
  }

  public class Instance : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.GameInstance
  {
    [Serialize]
    private int lastPoopCell = -1;
    public int targetPoopCell = -1;
    private Tag currentlyPoopingElement = Tag.Invalid;

    public Instance(Chore<NestingPoopState.Instance> chore, NestingPoopState.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Poop);
    }

    private static bool IsValidNestingCell(int cell, object arg)
    {
      if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || !Grid.Solid[Grid.CellBelow(cell)])
        return false;
      if (!NestingPoopState.Instance.IsValidPoopFromCell(cell, true))
        return NestingPoopState.Instance.IsValidPoopFromCell(cell, false);
      return true;
    }

    private static bool IsValidPoopFromCell(int cell, bool look_left)
    {
      if (look_left)
      {
        int cell1 = Grid.CellDownLeft(cell);
        int cell2 = Grid.CellLeft(cell);
        if (Grid.IsValidCell(cell1) && Grid.Solid[cell1] && Grid.IsValidCell(cell2))
          return !Grid.Solid[cell2];
        return false;
      }
      int cell3 = Grid.CellDownRight(cell);
      int cell4 = Grid.CellRight(cell);
      if (Grid.IsValidCell(cell3) && Grid.Solid[cell3] && Grid.IsValidCell(cell4))
        return !Grid.Solid[cell4];
      return false;
    }

    public int GetPoopPosition()
    {
      this.targetPoopCell = this.GetTargetPoopCell();
      List<Direction> directionList = new List<Direction>();
      if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, true))
        directionList.Add(Direction.Left);
      if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, false))
        directionList.Add(Direction.Right);
      if (directionList.Count > 0)
      {
        int cellInDirection = Grid.GetCellInDirection(this.targetPoopCell, directionList[UnityEngine.Random.Range(0, directionList.Count)]);
        if (Grid.IsValidCell(cellInDirection))
          return cellInDirection;
      }
      if (Grid.IsValidCell(this.targetPoopCell))
        return this.targetPoopCell;
      if (!Grid.IsValidCell(Grid.PosToCell((StateMachine.Instance) this)))
        Debug.LogWarning((object) "This is bad, how is Mole occupying an invalid cell?");
      return Grid.PosToCell((StateMachine.Instance) this);
    }

    private int GetTargetPoopCell()
    {
      this.currentlyPoopingElement = this.smi.GetSMI<CreatureCalorieMonitor.Instance>().stomach.GetNextPoopEntry();
      int start_cell = !(this.currentlyPoopingElement == this.smi.def.nestingPoopElement) || !(this.smi.def.nestingPoopElement != Tag.Invalid) || this.lastPoopCell == -1 ? Grid.PosToCell((StateMachine.Instance) this) : this.lastPoopCell;
      // ISSUE: reference to a compiler-generated field
      if (NestingPoopState.Instance.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        NestingPoopState.Instance.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(NestingPoopState.Instance.IsValidNestingCell);
      }
      // ISSUE: reference to a compiler-generated field
      int cell1 = GameUtil.FloodFillFind<object>(NestingPoopState.Instance.\u003C\u003Ef__mg\u0024cache0, (object) null, start_cell, 8, false, true);
      if (cell1 == -1)
      {
        CellOffset[] cellOffsetArray = new CellOffset[5]
        {
          new CellOffset(0, 0),
          new CellOffset(-1, 0),
          new CellOffset(1, 0),
          new CellOffset(-1, -1),
          new CellOffset(1, -1)
        };
        cell1 = Grid.OffsetCell(this.lastPoopCell, cellOffsetArray[UnityEngine.Random.Range(0, cellOffsetArray.Length)]);
        for (int cell2 = Grid.CellAbove(cell1); Grid.IsValidCell(cell2) && Grid.Solid[cell2]; cell2 = Grid.CellAbove(cell1))
          cell1 = cell2;
      }
      return cell1;
    }

    public void SetLastPoopCell()
    {
      if (!(this.currentlyPoopingElement == this.smi.def.nestingPoopElement))
        return;
      this.lastPoopCell = Grid.PosToCell((StateMachine.Instance) this);
    }
  }
}
