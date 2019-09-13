// Decompiled with JetBrains decompiler
// Type: CallAdultMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.State root = this.root;
    Tag callAdultBehaviour = GameTags.Creatures.Behaviours.CallAdultBehaviour;
    // ISSUE: reference to a compiler-generated field
    if (CallAdultMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CallAdultMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback fMgCache0 = CallAdultMonitor.\u003C\u003Ef__mg\u0024cache0;
    System.Action<CallAdultMonitor.Instance> on_complete = (System.Action<CallAdultMonitor.Instance>) (smi => smi.RefreshCallTime());
    root.ToggleBehaviour(callAdultBehaviour, fMgCache0, on_complete);
  }

  public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
  {
    return (double) Time.time >= (double) smi.nextCallTime;
  }

  public class Def : StateMachine.BaseDef
  {
    public float callMinInterval = 120f;
    public float callMaxInterval = 240f;
  }

  public class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
  {
    public float nextCallTime;

    public Instance(IStateMachineTarget master, CallAdultMonitor.Def def)
      : base(master, def)
    {
      this.RefreshCallTime();
    }

    public void RefreshCallTime()
    {
      this.nextCallTime = Time.time + UnityEngine.Random.value * (this.def.callMaxInterval - this.def.callMinInterval) + this.def.callMinInterval;
    }
  }
}
