// Decompiled with JetBrains decompiler
// Type: PutOnHatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PutOnHatChore : Chore<PutOnHatChore.StatesInstance>
{
  public PutOnHatChore(IStateMachineTarget target, ChoreType chore_type)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new PutOnHatChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.GameInstance
  {
    public StatesInstance(PutOnHatChore master, GameObject duplicant)
      : base(master)
    {
      this.sm.duplicant.Set(duplicant, this.smi);
    }
  }

  public class States : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore>
  {
    public StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.TargetParameter duplicant;
    public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat_pre;
    public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat;
    public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State complete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.applyHat_pre;
      this.Target(this.duplicant);
      this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0.0f).Enter((StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State.Callback) (smi => this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat())).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
      this.applyHat.ToggleAnims("anim_hat_kanim", 0.0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
      this.complete.ReturnSuccess();
    }
  }
}
