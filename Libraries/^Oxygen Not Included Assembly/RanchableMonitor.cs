// Decompiled with JetBrains decompiler
// Type: RanchableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (StateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldGoGetRanched()), (System.Action<RanchableMonitor.Instance>) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
  {
    public RanchStation.Instance targetRanchStation;

    public Instance(IStateMachineTarget master, RanchableMonitor.Def def)
      : base(master, def)
    {
    }

    public bool ShouldGoGetRanched()
    {
      if (this.targetRanchStation != null && this.targetRanchStation.IsRunning())
        return this.targetRanchStation.shouldCreatureGoGetRanched;
      return false;
    }
  }
}
