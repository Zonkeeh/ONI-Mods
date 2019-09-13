// Decompiled with JetBrains decompiler
// Type: RanchedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class RanchedStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
  private RanchedStates.RanchStates ranch;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State wavegoodbye;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State runaway;
  private GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.ranch;
    this.root.Exit("AbandonedRanchStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.AbandonedRanchStation()));
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state1 = this.ranch.EventTransition(GameHashes.RanchStationNoLongerAvailable, (GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) null, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) null).DefaultState((GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) this.ranch.cheer);
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback fMgCache0 = RanchedStates.\u003C\u003Ef__mg\u0024cache0;
    state1.Exit(fMgCache0);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state2 = this.ranch.cheer.DefaultState(this.ranch.cheer.pre);
    string name1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty1 = string.Empty;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state2.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, RanchedStates.Instance, string>) null, (Func<string, RanchedStates.Instance, string>) null, category1);
    this.ranch.cheer.pre.ScheduleGoTo(0.9f, (StateMachine.BaseState) this.ranch.cheer.cheer);
    this.ranch.cheer.cheer.Enter("FaceRancher", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition()))).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.cheer.pst);
    this.ranch.cheer.pst.ScheduleGoTo(0.2f, (StateMachine.BaseState) this.ranch.move);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state3 = this.ranch.move.DefaultState(this.ranch.move.movetoranch);
    string name3 = (string) CREATURES.STATUSITEMS.GETTING_RANCHED.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.GETTING_RANCHED.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    string name4 = name3;
    string tooltip4 = tooltip3;
    string empty2 = string.Empty;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state3.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, RanchedStates.Instance, string>) null, (Func<string, RanchedStates.Instance, string>) null, category2);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state4 = this.ranch.move.movetoranch.Enter("Speedup", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f));
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache1 = new Func<RanchedStates.Instance, int>(RanchedStates.GetTargetRanchCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<RanchedStates.Instance, int> fMgCache1 = RanchedStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State getontable1 = this.ranch.move.getontable;
    state4.MoveTo(fMgCache1, getontable1, (GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) null, false).Exit("RestoreSpeed", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed));
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State getontable2 = this.ranch.move.getontable;
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback fMgCache2 = RanchedStates.\u003C\u003Ef__mg\u0024cache2;
    getontable2.Enter(fMgCache2).OnAnimQueueComplete(this.ranch.move.waitforranchertobeready);
    this.ranch.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetRanchStation().Trigger(-1357116271, (object) null))).EventTransition(GameHashes.RancherReadyAtRanchStation, this.ranch.ranching, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) null);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State ranching = this.ranch.ranching;
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.PlayGroomingLoopAnim);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback fMgCache3 = RanchedStates.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state5 = ranching.Enter(fMgCache3).EventTransition(GameHashes.RanchingComplete, this.wavegoodbye, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) null);
    string name5 = (string) CREATURES.STATUSITEMS.GETTING_RANCHED.NAME;
    string tooltip5 = (string) CREATURES.STATUSITEMS.GETTING_RANCHED.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    string name6 = name5;
    string tooltip6 = tooltip5;
    string empty3 = string.Empty;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    state5.ToggleStatusItem(name6, tooltip6, empty3, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay3, 0, (Func<string, RanchedStates.Instance, string>) null, (Func<string, RanchedStates.Instance, string>) null, category3);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State wavegoodbye = this.wavegoodbye;
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.PlayGroomingPstAnim);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback fMgCache4 = RanchedStates.\u003C\u003Ef__mg\u0024cache4;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state6 = wavegoodbye.Enter(fMgCache4).OnAnimQueueComplete(this.runaway);
    string name7 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
    string tooltip7 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
    StatusItemCategory main4 = Db.Get().StatusItemCategories.Main;
    string name8 = name7;
    string tooltip8 = tooltip7;
    string empty4 = string.Empty;
    HashedString render_overlay4 = new HashedString();
    StatusItemCategory category4 = main4;
    state6.ToggleStatusItem(name8, tooltip8, empty4, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay4, 0, (Func<string, RanchedStates.Instance, string>) null, (Func<string, RanchedStates.Instance, string>) null, category4);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State runaway = this.runaway;
    // ISSUE: reference to a compiler-generated field
    if (RanchedStates.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      RanchedStates.\u003C\u003Ef__mg\u0024cache5 = new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<RanchedStates.Instance, int> fMgCache5 = RanchedStates.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State behaviourcomplete1 = this.behaviourcomplete;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State behaviourcomplete2 = this.behaviourcomplete;
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state7 = runaway.MoveTo(fMgCache5, behaviourcomplete1, behaviourcomplete2, false);
    string name9 = (string) CREATURES.STATUSITEMS.IDLE.NAME;
    string tooltip9 = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
    StatusItemCategory main5 = Db.Get().StatusItemCategories.Main;
    string name10 = name9;
    string tooltip10 = tooltip9;
    string empty5 = string.Empty;
    HashedString render_overlay5 = new HashedString();
    StatusItemCategory category5 = main5;
    state7.ToggleStatusItem(name10, tooltip10, empty5, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay5, 0, (Func<string, RanchedStates.Instance, string>) null, (Func<string, RanchedStates.Instance, string>) null, category5);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetRanched, false);
  }

  private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
  {
    return smi.GetSMI<RanchableMonitor.Instance>().targetRanchStation;
  }

  private static void ClearLayerOverride(RanchedStates.Instance smi)
  {
    smi.Get<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
  }

  private static void GetOnTable(RanchedStates.Instance smi)
  {
    Navigator navigator = smi.Get<Navigator>();
    if (navigator.IsValidNavType(NavType.Floor))
      navigator.SetCurrentNavType(NavType.Floor);
    smi.Get<Facing>().SetFacing(false);
    smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedPreAnim, KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private static void PlayGroomingLoopAnim(RanchedStates.Instance smi)
  {
    smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedLoopAnim, KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  private static void PlayGroomingPstAnim(RanchedStates.Instance smi)
  {
    smi.Get<KBatchedAnimController>().Queue(RanchedStates.GetRanchStation(smi).def.ranchedPstAnim, KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private static int GetTargetRanchCell(RanchedStates.Instance smi)
  {
    return RanchedStates.GetRanchStation(smi).GetTargetRanchCell();
  }

  private static int GetRunawayCell(RanchedStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    int index = Grid.OffsetCell(cell, 2, 0);
    if (Grid.Solid[index])
      index = Grid.OffsetCell(cell, -2, 0);
    return index;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
  {
    public float originalSpeed;

    public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.originalSpeed = this.GetComponent<Navigator>().defaultSpeed;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToGetRanched);
    }

    public RanchStation.Instance GetRanchStation()
    {
      return this.GetSMI<RanchableMonitor.Instance>().targetRanchStation;
    }

    public void AbandonedRanchStation()
    {
      if (this.GetRanchStation() == null)
        return;
      this.GetRanchStation().Trigger(-364750427, (object) null);
    }
  }

  public class RanchStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public RanchedStates.RanchStates.CheerStates cheer;
    public RanchedStates.RanchStates.MoveStates move;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State ranching;

    public class CheerStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
    {
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State pre;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State cheer;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State pst;
    }

    public class MoveStates : GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
    {
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State movetoranch;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State getontable;
      public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State waitforranchertobeready;
    }
  }
}
