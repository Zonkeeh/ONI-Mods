// Decompiled with JetBrains decompiler
// Type: CreatureDebugGoToMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.State root = this.root;
    Tag debugDestination = GameTags.HasDebugDestination;
    // ISSUE: reference to a compiler-generated field
    if (CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback fMgCache0 = CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache0;
    // ISSUE: reference to a compiler-generated field
    if (CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<CreatureDebugGoToMonitor.Instance> fMgCache1 = CreatureDebugGoToMonitor.\u003C\u003Ef__mg\u0024cache1;
    root.ToggleBehaviour(debugDestination, fMgCache0, fMgCache1);
  }

  private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
  {
    return smi.targetCell != Grid.InvalidCell;
  }

  private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
  {
    smi.targetCell = Grid.InvalidCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
  {
    public int targetCell = Grid.InvalidCell;

    public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def)
      : base(target, def)
    {
    }

    public void GoToCursor()
    {
      this.targetCell = DebugHandler.GetMouseCell();
    }
  }
}
