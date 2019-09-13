// Decompiled with JetBrains decompiler
// Type: MoveToLureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class MoveToLureStates : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>
{
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arrive_at_lure;
  public GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.move;
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.CONSIDERINGLURE.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.CONSIDERINGLURE.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, MoveToLureStates.Instance, string>) null, (Func<string, MoveToLureStates.Instance, string>) null, category);
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State move = this.move;
    // ISSUE: reference to a compiler-generated field
    if (MoveToLureStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MoveToLureStates.\u003C\u003Ef__mg\u0024cache0 = new Func<MoveToLureStates.Instance, int>(MoveToLureStates.GetLureCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<MoveToLureStates.Instance, int> fMgCache0 = MoveToLureStates.\u003C\u003Ef__mg\u0024cache0;
    // ISSUE: reference to a compiler-generated field
    if (MoveToLureStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MoveToLureStates.\u003C\u003Ef__mg\u0024cache1 = new Func<MoveToLureStates.Instance, CellOffset[]>(MoveToLureStates.GetLureOffsets);
    }
    // ISSUE: reference to a compiler-generated field
    Func<MoveToLureStates.Instance, CellOffset[]> fMgCache1 = MoveToLureStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State arriveAtLure = this.arrive_at_lure;
    GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State behaviourcomplete = this.behaviourcomplete;
    move.MoveTo(fMgCache0, fMgCache1, arriveAtLure, behaviourcomplete, false);
    this.arrive_at_lure.Enter((StateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.State.Callback) (smi =>
    {
      Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
      if (targetLure == null || !targetLure.HasTag(GameTags.OneTimeUseLure))
        return;
      targetLure.GetComponent<KPrefabID>().AddTag(GameTags.LureUsed, false);
    })).GoTo(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.MoveToLure, false);
  }

  private static Lure.Instance GetTargetLure(MoveToLureStates.Instance smi)
  {
    GameObject targetLure = smi.GetSMI<LureableMonitor.Instance>().GetTargetLure();
    if ((UnityEngine.Object) targetLure == (UnityEngine.Object) null)
      return (Lure.Instance) null;
    return targetLure.GetSMI<Lure.Instance>();
  }

  private static int GetLureCell(MoveToLureStates.Instance smi)
  {
    Lure.Instance targetLure = MoveToLureStates.GetTargetLure(smi);
    if (targetLure == null)
      return Grid.InvalidCell;
    return Grid.PosToCell((StateMachine.Instance) targetLure);
  }

  private static CellOffset[] GetLureOffsets(MoveToLureStates.Instance smi)
  {
    return MoveToLureStates.GetTargetLure(smi)?.def.lurePoints;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<MoveToLureStates, MoveToLureStates.Instance, IStateMachineTarget, MoveToLureStates.Def>.GameInstance
  {
    public Instance(Chore<MoveToLureStates.Instance> chore, MoveToLureStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.MoveToLure);
    }
  }
}
