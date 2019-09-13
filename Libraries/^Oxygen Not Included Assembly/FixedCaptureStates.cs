// Decompiled with JetBrains decompiler
// Type: FixedCaptureStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class FixedCaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>
{
  private FixedCaptureStates.CaptureStates capture;
  private GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.capture;
    this.root.Exit("AbandonedCapturePoint", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.AbandonedCapturePoint()));
    this.capture.EventTransition(GameHashes.CapturePointNoLongerAvailable, (GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State) null, (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.Transition.ConditionCallback) null).DefaultState((GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State) this.capture.cheer);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state1 = this.capture.cheer.DefaultState(this.capture.cheer.pre);
    string name1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty1 = string.Empty;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, FixedCaptureStates.Instance, string>) null, (Func<string, FixedCaptureStates.Instance, string>) null, category1);
    this.capture.cheer.pre.ScheduleGoTo(0.9f, (StateMachine.BaseState) this.capture.cheer.cheer);
    this.capture.cheer.cheer.Enter("FaceRancher", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Facing>().Face(smi.GetCapturePoint().transform.GetPosition()))).PlayAnim("excited_loop").OnAnimQueueComplete(this.capture.cheer.pst);
    this.capture.cheer.pst.ScheduleGoTo(0.2f, (StateMachine.BaseState) this.capture.move);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state2 = this.capture.move.DefaultState(this.capture.move.movetoranch);
    string name3 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    string name4 = name3;
    string tooltip4 = tooltip3;
    string empty2 = string.Empty;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, FixedCaptureStates.Instance, string>) null, (Func<string, FixedCaptureStates.Instance, string>) null, category2);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state3 = this.capture.move.movetoranch.Enter("Speedup", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f));
    // ISSUE: reference to a compiler-generated field
    if (FixedCaptureStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FixedCaptureStates.\u003C\u003Ef__mg\u0024cache0 = new Func<FixedCaptureStates.Instance, int>(FixedCaptureStates.GetTargetCaptureCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<FixedCaptureStates.Instance, int> fMgCache0 = FixedCaptureStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready = this.capture.move.waitforranchertobeready;
    state3.MoveTo(fMgCache0, waitforranchertobeready, (GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State) null, false).Exit("RestoreSpeed", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed));
    this.capture.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State.Callback) (smi => smi.GetCapturePoint().Trigger(-1992722293, (object) null))).EventTransition(GameHashes.RancherReadyAtCapturePoint, this.capture.ranching, (StateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.Transition.ConditionCallback) null);
    GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching = this.capture.ranching;
    string name5 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
    string tooltip5 = (string) CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    string name6 = name5;
    string tooltip6 = tooltip5;
    string empty3 = string.Empty;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    ranching.ToggleStatusItem(name6, tooltip6, empty3, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay3, 0, (Func<string, FixedCaptureStates.Instance, string>) null, (Func<string, FixedCaptureStates.Instance, string>) null, category3);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetCaptured, false);
  }

  private static FixedCapturePoint.Instance GetCapturePoint(
    FixedCaptureStates.Instance smi)
  {
    return smi.GetSMI<FixedCapturableMonitor.Instance>().targetCapturePoint;
  }

  private static int GetTargetCaptureCell(FixedCaptureStates.Instance smi)
  {
    FixedCapturePoint.Instance capturePoint = FixedCaptureStates.GetCapturePoint(smi);
    return capturePoint.def.getTargetCapturePoint(capturePoint);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.GameInstance
  {
    public float originalSpeed;

    public Instance(Chore<FixedCaptureStates.Instance> chore, FixedCaptureStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.originalSpeed = this.GetComponent<Navigator>().defaultSpeed;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToGetCaptured);
    }

    public FixedCapturePoint.Instance GetCapturePoint()
    {
      return this.GetSMI<FixedCapturableMonitor.Instance>()?.targetCapturePoint;
    }

    public void AbandonedCapturePoint()
    {
      if (this.GetCapturePoint() == null)
        return;
      this.GetCapturePoint().Trigger(-1000356449, (object) null);
    }
  }

  public class CaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
  {
    public FixedCaptureStates.CaptureStates.CheerStates cheer;
    public FixedCaptureStates.CaptureStates.MoveStates move;
    public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching;

    public class CheerStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
    {
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pre;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State cheer;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pst;
    }

    public class MoveStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
    {
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State movetoranch;
      public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready;
    }
  }
}
