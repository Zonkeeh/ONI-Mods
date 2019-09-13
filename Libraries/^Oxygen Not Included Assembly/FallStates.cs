// Decompiled with JetBrains decompiler
// Type: FallStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class FallStates : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>
{
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State loop;
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State snaptoground;
  private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.FALLING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.FALLING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, FallStates.Instance, string>) null, (Func<string, FallStates.Instance, string>) null, category);
    this.loop.PlayAnim((Func<FallStates.Instance, string>) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().anim), KAnim.PlayMode.Loop).ToggleGravity().EventTransition(GameHashes.Landed, this.snaptoground, (StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.Transition.ConditionCallback) null).Transition(this.pst, (StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.Transition.ConditionCallback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation(true)), UpdateRate.SIM_33ms);
    this.snaptoground.Enter((StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback) (smi => smi.GetSMI<CreatureFallMonitor.Instance>().SnapToGround())).GoTo(this.pst);
    GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State pst = this.pst;
    // ISSUE: reference to a compiler-generated field
    if (FallStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FallStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback(FallStates.PlayLandAnim);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback fMgCache0 = FallStates.\u003C\u003Ef__mg\u0024cache0;
    pst.Enter(fMgCache0).BehaviourComplete(GameTags.Creatures.Falling, false);
  }

  private static void PlayLandAnim(FallStates.Instance smi)
  {
    smi.GetComponent<KBatchedAnimController>().Queue((HashedString) smi.def.getLandAnim(smi), KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public class Def : StateMachine.BaseDef
  {
    public Func<FallStates.Instance, string> getLandAnim = (Func<FallStates.Instance, string>) (smi => "idle_loop");
  }

  public class Instance : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.GameInstance
  {
    public Instance(Chore<FallStates.Instance> chore, FallStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Falling);
    }
  }
}
