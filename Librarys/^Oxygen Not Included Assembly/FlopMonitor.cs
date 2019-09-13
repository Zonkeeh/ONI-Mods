// Decompiled with JetBrains decompiler
// Type: FlopMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (StateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldBeginFlopping()), (System.Action<FlopMonitor.Instance>) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, FlopMonitor.Def def)
      : base(master, def)
    {
    }

    public bool ShouldBeginFlopping()
    {
      Vector3 position = this.transform.GetPosition();
      position.y += CreatureFallMonitor.FLOOR_DISTANCE;
      int cell1 = Grid.PosToCell(this.transform.GetPosition());
      int cell2 = Grid.PosToCell(position);
      if (Grid.IsValidCell(cell2) && Grid.Solid[cell2] && !Grid.IsSubstantialLiquid(cell1, 0.35f))
        return !Grid.IsLiquid(Grid.CellAbove(cell1));
      return false;
    }
  }
}
