// Decompiled with JetBrains decompiler
// Type: DrowningStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;
  public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.drown;
    GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.DROWNING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, DrowningStates.Instance, string>) null, (Func<string, DrowningStates.Instance, string>) null, category).TagTransition(GameTags.Creatures.Drowning, (GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State) null, true);
    this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, (Func<DrowningStates.Instance, string>) null).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
    this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
    this.move_to_safe.MoveTo((Func<DrowningStates.Instance, int>) (smi => smi.safeCell), (GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State) null, (GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State) null, false);
  }

  public bool UpdateSafeCell(DrowningStates.Instance smi)
  {
    Navigator component = smi.GetComponent<Navigator>();
    DrowningStates.EscapeCellQuery escapeCellQuery = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
    component.RunQuery((PathFinderQuery) escapeCellQuery);
    smi.safeCell = escapeCellQuery.GetResultCell();
    return smi.safeCell != Grid.InvalidCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
  {
    public int safeCell = Grid.InvalidCell;

    public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.HasTag, (object) GameTags.Creatures.Drowning);
    }
  }

  public class EscapeCellQuery : PathFinderQuery
  {
    private DrowningMonitor monitor;

    public EscapeCellQuery(DrowningMonitor monitor)
    {
      this.monitor = monitor;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      return this.monitor.IsCellSafe(cell);
    }
  }
}
