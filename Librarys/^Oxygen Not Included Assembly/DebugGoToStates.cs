// Decompiled with JetBrains decompiler
// Type: DebugGoToStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class DebugGoToStates : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moving;
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving = this.moving;
    // ISSUE: reference to a compiler-generated field
    if (DebugGoToStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DebugGoToStates.\u003C\u003Ef__mg\u0024cache0 = new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<DebugGoToStates.Instance, int> fMgCache0 = DebugGoToStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete1 = this.behaviourcomplete;
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete2 = this.behaviourcomplete;
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State state = moving.MoveTo(fMgCache0, behaviourcomplete1, behaviourcomplete2, true);
    string name1 = (string) CREATURES.STATUSITEMS.DEBUGGOTO.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, DebugGoToStates.Instance, string>) null, (Func<string, DebugGoToStates.Instance, string>) null, category);
    this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination, false);
  }

  private static int GetTargetCell(DebugGoToStates.Instance smi)
  {
    return smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
  {
    public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.HasDebugDestination);
    }
  }
}
