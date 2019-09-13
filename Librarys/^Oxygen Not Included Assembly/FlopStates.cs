// Decompiled with JetBrains decompiler
// Type: FlopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class FlopStates : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>
{
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_pre;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_cycle;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.flop_pre;
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.FLOPPING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.FLOPPING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, FlopStates.Instance, string>) null, (Func<string, FlopStates.Instance, string>) null, category);
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flopPre = this.flop_pre;
    // ISSUE: reference to a compiler-generated field
    if (FlopStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FlopStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback(FlopStates.ChooseDirection);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback fMgCache0 = FlopStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State state1 = flopPre.Enter(fMgCache0);
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flopCycle = this.flop_cycle;
    // ISSUE: reference to a compiler-generated field
    if (FlopStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FlopStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback fMgCache1 = FlopStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State state2 = state1.Transition(flopCycle, fMgCache1, UpdateRate.SIM_200ms);
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst1 = this.pst;
    // ISSUE: reference to a compiler-generated field
    if (FlopStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FlopStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback condition = GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Not(FlopStates.\u003C\u003Ef__mg\u0024cache2);
    state2.Transition(pst1, condition, UpdateRate.SIM_200ms);
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State state3 = this.flop_cycle.PlayAnim("flop_loop", KAnim.PlayMode.Once);
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst2 = this.pst;
    // ISSUE: reference to a compiler-generated field
    if (FlopStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FlopStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.IsSubstantialLiquid);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback fMgCache3 = FlopStates.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State state4 = state3.Transition(pst2, fMgCache3, UpdateRate.SIM_200ms);
    // ISSUE: reference to a compiler-generated field
    if (FlopStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FlopStates.\u003C\u003Ef__mg\u0024cache4 = new System.Action<FlopStates.Instance, float>(FlopStates.FlopForward);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<FlopStates.Instance, float> fMgCache4 = FlopStates.\u003C\u003Ef__mg\u0024cache4;
    state4.Update("Flop", fMgCache4, UpdateRate.SIM_33ms, false).OnAnimQueueComplete(this.flop_pre);
    this.pst.QueueAnim("flop_loop", true, (Func<FlopStates.Instance, string>) null).BehaviourComplete(GameTags.Creatures.Flopping, false);
  }

  public static bool ShouldFlop(FlopStates.Instance smi)
  {
    int cell = Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition()));
    if (Grid.IsValidCell(cell))
      return Grid.Solid[cell];
    return false;
  }

  public static void ChooseDirection(FlopStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    if (FlopStates.SearchForLiquid(cell, 1))
      smi.currentDir = 1f;
    else if (FlopStates.SearchForLiquid(cell, -1))
      smi.currentDir = -1f;
    else if ((double) UnityEngine.Random.value > 0.5)
      smi.currentDir = 1f;
    else
      smi.currentDir = -1f;
  }

  private static bool SearchForLiquid(int cell, int delta_x)
  {
    while (Grid.IsValidCell(cell))
    {
      if (Grid.IsSubstantialLiquid(cell, 0.35f))
        return true;
      if (Grid.Solid[cell] || Grid.CritterImpassable[cell])
        return false;
      int cell1 = Grid.CellBelow(cell);
      if (Grid.IsValidCell(cell1) && Grid.Solid[cell1])
        cell += delta_x;
      else
        cell = cell1;
    }
    return false;
  }

  public static void FlopForward(FlopStates.Instance smi, float dt)
  {
    int currentFrame = smi.GetComponent<KBatchedAnimController>().currentFrame;
    if (currentFrame < 23 || currentFrame > 36)
      return;
    Vector3 position = smi.transform.GetPosition();
    Vector3 vector3 = position;
    vector3.x = position.x + (float) ((double) smi.currentDir * (double) dt * 1.0);
    int cell = Grid.PosToCell(vector3);
    if (Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.CritterImpassable[cell])
      smi.transform.SetPosition(vector3);
    else
      smi.currentDir = -smi.currentDir;
  }

  public static bool IsSubstantialLiquid(FlopStates.Instance smi)
  {
    return Grid.IsSubstantialLiquid(Grid.PosToCell(smi.transform.GetPosition()), 0.35f);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.GameInstance
  {
    public float currentDir = 1f;

    public Instance(Chore<FlopStates.Instance> chore, FlopStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Flopping);
    }
  }
}
