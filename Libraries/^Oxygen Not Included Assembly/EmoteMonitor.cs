// Decompiled with JetBrains decompiler
// Type: EmoteMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
  public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = true;
    this.satisfied.ScheduleGoTo((float) Random.Range(30, 90), (StateMachine.BaseState) this.ready);
    this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, (GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, o) => smi.OnStartChore(o)));
  }

  public class Instance : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void OnStartChore(object o)
    {
      if (!((Chore) o).SatisfiesUrge(Db.Get().Urges.Emote))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.satisfied);
    }
  }
}
