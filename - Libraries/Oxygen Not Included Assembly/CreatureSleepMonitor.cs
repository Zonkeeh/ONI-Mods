// Decompiled with JetBrains decompiler
// Type: CreatureSleepMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.State root = this.root;
    Tag sleepBehaviour = GameTags.Creatures.Behaviours.SleepBehaviour;
    // ISSUE: reference to a compiler-generated field
    if (CreatureSleepMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureSleepMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback fMgCache0 = CreatureSleepMonitor.\u003C\u003Ef__mg\u0024cache0;
    root.ToggleBehaviour(sleepBehaviour, fMgCache0, (System.Action<CreatureSleepMonitor.Instance>) null);
  }

  public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
  {
    return GameClock.Instance.IsNighttime();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
