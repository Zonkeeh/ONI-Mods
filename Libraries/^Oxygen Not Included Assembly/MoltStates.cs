// Decompiled with JetBrains decompiler
// Type: MoltStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using UnityEngine;

public class MoltStates : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moltpre;
    GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.MOLTING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.MOLTING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, MoltStates.Instance, string>) null, (Func<string, MoltStates.Instance, string>) null, category);
    GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre = this.moltpre;
    // ISSUE: reference to a compiler-generated field
    if (MoltStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MoltStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback fMgCache0 = MoltStates.\u003C\u003Ef__mg\u0024cache0;
    moltpre.Enter(fMgCache0).QueueAnim("lay_egg_pre", false, (Func<MoltStates.Instance, string>) null).OnAnimQueueComplete(this.moltpst);
    this.moltpst.QueueAnim("lay_egg_pst", false, (Func<MoltStates.Instance, string>) null).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown, false);
  }

  private static void Molt(MoltStates.Instance smi)
  {
    smi.eggPos = smi.transform.GetPosition();
    smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
  }

  private static int GetMoveAsideCell(MoltStates.Instance smi)
  {
    int x = 1;
    if (GenericGameSettings.instance.acceleratedLifecycle)
      x = 8;
    int cell1 = Grid.PosToCell((StateMachine.Instance) smi);
    if (Grid.IsValidCell(cell1))
    {
      int cell2 = Grid.OffsetCell(cell1, x, 0);
      if (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
        return cell2;
      int cell3 = Grid.OffsetCell(cell1, -x, 0);
      if (Grid.IsValidCell(cell3))
        return cell3;
    }
    return Grid.InvalidCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
  {
    public Vector3 eggPos;

    public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.ScalesGrown);
    }
  }
}
