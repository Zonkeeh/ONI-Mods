// Decompiled with JetBrains decompiler
// Type: SighChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SighChore : Chore<SighChore.StatesInstance>
{
  public SighChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new SighChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
  {
    public StatesInstance(SighChore master, GameObject sigher)
      : base(master)
    {
      this.sm.sigher.Set(sigher, this.smi);
    }
  }

  public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
  {
    public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.sigher);
      this.root.PlayAnim("emote_depressed").OnAnimQueueComplete((GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.State) null);
    }
  }
}
