// Decompiled with JetBrains decompiler
// Type: DieChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class DieChore : Chore<DieChore.StatesInstance>
{
  public DieChore(IStateMachineTarget master, Death death)
    : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new DieChore.StatesInstance(this, death);
  }

  public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
  {
    public StatesInstance(DieChore master, Death death)
      : base(master)
    {
      this.sm.death.Set(death, this.smi);
    }

    public void PlayPreAnim()
    {
      this.GetComponent<KAnimControllerBase>().Play((HashedString) this.sm.death.Get(this.smi).preAnim, KAnim.PlayMode.Once, 1f, 0.0f);
    }
  }

  public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
  {
    public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;
    public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;
    public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.dying;
      this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", (StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State.Callback) (smi => smi.PlayPreAnim()));
      this.dead.ReturnSuccess();
    }
  }
}
