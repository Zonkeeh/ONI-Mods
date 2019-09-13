// Decompiled with JetBrains decompiler
// Type: TiredMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
  public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventTransition(GameHashes.SleepFail, this.tired, (StateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null);
    this.tired.Enter((StateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.SetInterruptDay())).EventTransition(GameHashes.NewDay, (Func<TiredMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.root, (StateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.AllowInterruptClear())).ToggleExpression(Db.Get().Expressions.Tired, (Func<TiredMonitor.Instance, bool>) null).ToggleAnims("anim_loco_walk_slouch_kanim", 0.0f).ToggleAnims("anim_idle_slouch_kanim", 0.0f);
  }

  public class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public int disturbedDay = -1;
    public int interruptedDay = -1;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void SetInterruptDay()
    {
      this.interruptedDay = GameClock.Instance.GetCycle();
    }

    public bool AllowInterruptClear()
    {
      bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
      if (flag)
        this.interruptedDay = -1;
      return flag;
    }
  }
}
