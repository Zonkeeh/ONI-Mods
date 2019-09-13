// Decompiled with JetBrains decompiler
// Type: LayEggStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using UnityEngine;

public class LayEggStates : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>
{
  public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpre;
  public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpst;
  public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State moveaside;
  public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State lookategg;
  public GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.layeggpre;
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.LAYINGANEGG.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.LAYINGANEGG.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, LayEggStates.Instance, string>) null, (Func<string, LayEggStates.Instance, string>) null, category);
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State layeggpre = this.layeggpre;
    // ISSUE: reference to a compiler-generated field
    if (LayEggStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      LayEggStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.LayEgg);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback fMgCache0 = LayEggStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State state = layeggpre.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (LayEggStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      LayEggStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.ShowEgg);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback fMgCache1 = LayEggStates.\u003C\u003Ef__mg\u0024cache1;
    state.Exit(fMgCache1).PlayAnim("lay_egg_pre").OnAnimQueueComplete(this.layeggpst);
    this.layeggpst.PlayAnim("lay_egg_pst").OnAnimQueueComplete(this.moveaside);
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State moveaside = this.moveaside;
    // ISSUE: reference to a compiler-generated field
    if (LayEggStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      LayEggStates.\u003C\u003Ef__mg\u0024cache2 = new Func<LayEggStates.Instance, int>(LayEggStates.GetMoveAsideCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<LayEggStates.Instance, int> fMgCache2 = LayEggStates.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State lookategg1 = this.lookategg;
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State behaviourcomplete = this.behaviourcomplete;
    moveaside.MoveTo(fMgCache2, lookategg1, behaviourcomplete, false);
    GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State lookategg2 = this.lookategg;
    // ISSUE: reference to a compiler-generated field
    if (LayEggStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      LayEggStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback(LayEggStates.FaceEgg);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.State.Callback fMgCache3 = LayEggStates.\u003C\u003Ef__mg\u0024cache3;
    lookategg2.Enter(fMgCache3).GoTo(this.behaviourcomplete);
    this.behaviourcomplete.QueueAnim("idle_loop", true, (Func<LayEggStates.Instance, string>) null).BehaviourComplete(GameTags.Creatures.Fertile, false);
  }

  private static void LayEgg(LayEggStates.Instance smi)
  {
    smi.eggPos = smi.transform.GetPosition();
    smi.GetSMI<FertilityMonitor.Instance>().LayEgg();
  }

  private static void ShowEgg(LayEggStates.Instance smi)
  {
    smi.GetSMI<FertilityMonitor.Instance>().ShowEgg();
  }

  private static void FaceEgg(LayEggStates.Instance smi)
  {
    smi.Get<Facing>().Face(smi.eggPos);
  }

  private static int GetMoveAsideCell(LayEggStates.Instance smi)
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

  public class Instance : GameStateMachine<LayEggStates, LayEggStates.Instance, IStateMachineTarget, LayEggStates.Def>.GameInstance
  {
    public Vector3 eggPos;

    public Instance(Chore<LayEggStates.Instance> chore, LayEggStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Fertile);
    }
  }
}
