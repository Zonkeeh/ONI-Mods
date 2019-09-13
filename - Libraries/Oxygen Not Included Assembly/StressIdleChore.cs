// Decompiled with JetBrains decompiler
// Type: StressIdleChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
  public StressIdleChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
  {
    public StatesInstance(StressIdleChore master, GameObject idler)
      : base(master)
    {
      this.sm.idler.Set(idler, this.smi);
    }
  }

  public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
  {
    public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;
    public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.Target(this.idler);
      this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
    }
  }
}
