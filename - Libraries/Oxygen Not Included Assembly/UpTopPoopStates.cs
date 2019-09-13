// Decompiled with JetBrains decompiler
// Type: UpTopPoopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class UpTopPoopStates : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>
{
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State goingtopoop;
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State pooping;
  public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State behaviourcomplete;
  public StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.IntParameter targetCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtopoop;
    this.root.Enter("SetTarget", (StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State.Callback) (smi => this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi)));
    this.goingtopoop.MoveTo((Func<UpTopPoopStates.Instance, int>) (smi => smi.GetPoopCell()), this.pooping, this.pooping, false);
    GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
    string name1 = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, UpTopPoopStates.Instance, string>) null, (Func<string, UpTopPoopStates.Instance, string>) null, category).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.GameInstance
  {
    public Instance(Chore<UpTopPoopStates.Instance> chore, UpTopPoopStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Poop);
    }

    public int GetPoopCell()
    {
      int num = this.master.gameObject.GetComponent<Navigator>().maxProbingRadius - 1;
      int cell = Grid.PosToCell(this.gameObject);
      for (int index = Grid.OffsetCell(cell, 0, 1); num > 0 && Grid.IsValidCell(index) && (!Grid.Solid[index] && !this.IsClosedDoor(index)); index = Grid.OffsetCell(cell, 0, 1))
      {
        --num;
        cell = index;
      }
      return cell;
    }

    public bool IsClosedDoor(int cellAbove)
    {
      if (!Grid.HasDoor[cellAbove])
        return false;
      Door component = Grid.Objects[cellAbove, 1].GetComponent<Door>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component.CurrentState != Door.ControlState.Opened;
      return false;
    }
  }
}
