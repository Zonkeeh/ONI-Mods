// Decompiled with JetBrains decompiler
// Type: IncubatorMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
  public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;
  public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.not;
    GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not1 = this.not;
    GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State inIncubator = this.in_incubator;
    // ISSUE: reference to a compiler-generated field
    if (IncubatorMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      IncubatorMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback fMgCache0 = IncubatorMonitor.\u003C\u003Ef__mg\u0024cache0;
    not1.EventTransition(GameHashes.OnStore, inIncubator, fMgCache0);
    GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State state = this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator);
    GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not2 = this.not;
    // ISSUE: reference to a compiler-generated field
    if (IncubatorMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      IncubatorMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback condition = GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(IncubatorMonitor.\u003C\u003Ef__mg\u0024cache1);
    state.EventTransition(GameHashes.OnStore, not2, condition);
  }

  public static bool InIncubator(IncubatorMonitor.Instance smi)
  {
    if ((bool) ((Object) smi.gameObject.transform.parent))
      return (Object) smi.gameObject.transform.parent.GetComponent<EggIncubator>() != (Object) null;
    return false;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, IncubatorMonitor.Def def)
      : base(master, def)
    {
    }
  }
}
