// Decompiled with JetBrains decompiler
// Type: GrowUpStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;
  public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grow_up_pre;
    GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.GROWINGUP.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, GrowUpStates.Instance, string>) null, (Func<string, GrowUpStates.Instance, string>) null, category);
    this.grow_up_pre.QueueAnim("growup_pre", false, (Func<GrowUpStates.Instance, string>) null).OnAnimQueueComplete(this.spawn_adult);
    GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawnAdult = this.spawn_adult;
    // ISSUE: reference to a compiler-generated field
    if (GrowUpStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      GrowUpStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback fMgCache0 = GrowUpStates.\u003C\u003Ef__mg\u0024cache0;
    spawnAdult.Enter(fMgCache0);
  }

  private static void SpawnAdult(GrowUpStates.Instance smi)
  {
    smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
  {
    public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.GrowUpBehaviour);
    }
  }
}
