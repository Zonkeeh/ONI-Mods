// Decompiled with JetBrains decompiler
// Type: FixedCapturableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (StateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldGoGetCaptured()), (System.Action<FixedCapturableMonitor.Instance>) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
  {
    public FixedCapturePoint.Instance targetCapturePoint;

    public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def)
      : base(master, def)
    {
    }

    public bool ShouldGoGetCaptured()
    {
      if (this.targetCapturePoint != null && this.targetCapturePoint.IsRunning())
        return this.targetCapturePoint.shouldCreatureGoGetCaptured;
      return false;
    }
  }
}
