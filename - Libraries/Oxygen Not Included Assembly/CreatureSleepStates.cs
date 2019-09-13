// Decompiled with JetBrains decompiler
// Type: CreatureSleepStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class CreatureSleepStates : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>
{
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pre;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State loop;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst;
  public GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.pre;
    GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.SLEEPING.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, CreatureSleepStates.Instance, string>) null, (Func<string, CreatureSleepStates.Instance, string>) null, category);
    this.pre.QueueAnim("sleep_pre", false, (Func<CreatureSleepStates.Instance, string>) null).OnAnimQueueComplete(this.loop);
    GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State state = this.loop.QueueAnim("sleep_loop", true, (Func<CreatureSleepStates.Instance, string>) null);
    GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.State pst = this.pst;
    // ISSUE: reference to a compiler-generated field
    if (CreatureSleepStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureSleepStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback(CreatureSleepStates.ShouldWakeUp);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.Transition.ConditionCallback fMgCache0 = CreatureSleepStates.\u003C\u003Ef__mg\u0024cache0;
    state.Transition(pst, fMgCache0, UpdateRate.SIM_1000ms);
    this.pst.QueueAnim("sleep_pst", false, (Func<CreatureSleepStates.Instance, string>) null).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.SleepBehaviour, false);
  }

  public static bool ShouldWakeUp(CreatureSleepStates.Instance smi)
  {
    return !GameClock.Instance.IsNighttime();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<CreatureSleepStates, CreatureSleepStates.Instance, IStateMachineTarget, CreatureSleepStates.Def>.GameInstance
  {
    public Instance(Chore<CreatureSleepStates.Instance> chore, CreatureSleepStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.SleepBehaviour);
    }
  }
}
