// Decompiled with JetBrains decompiler
// Type: RedAlertMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
  public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = true;
    this.off.EventTransition(GameHashes.EnteredRedAlert, (Func<RedAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.on, (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => VignetteManager.Instance.Get().IsRedAlert()));
    this.on.EventTransition(GameHashes.ExitedRedAlert, (Func<RedAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.off, (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !VignetteManager.Instance.Get().IsRedAlert())).Enter("EnableRedAlert", (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.EnableRedAlert())).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert, (Func<RedAlertMonitor.Instance, bool>) null);
  }

  public class Instance : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void EnableRedAlert()
    {
      ChoreDriver component = this.GetComponent<ChoreDriver>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      Chore currentChore = component.GetCurrentChore();
      if (currentChore == null)
        return;
      bool flag = false;
      for (int index = 0; index < currentChore.GetPreconditions().Count; ++index)
      {
        if (currentChore.GetPreconditions()[index].id == ChorePreconditions.instance.IsNotRedAlert.id)
          flag = true;
      }
      if (!flag)
        return;
      component.StopChore();
    }
  }
}
