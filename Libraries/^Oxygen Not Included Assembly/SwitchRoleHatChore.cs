// Decompiled with JetBrains decompiler
// Type: SwitchRoleHatChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class SwitchRoleHatChore : Chore<SwitchRoleHatChore.StatesInstance>
{
  public SwitchRoleHatChore(IStateMachineTarget target, ChoreType chore_type)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new SwitchRoleHatChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.GameInstance
  {
    public StatesInstance(SwitchRoleHatChore master, GameObject duplicant)
      : base(master)
    {
      this.sm.duplicant.Set(duplicant, this.smi);
    }
  }

  public class States : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore>
  {
    public StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.TargetParameter duplicant;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State remove_hat;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State start;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay_pst;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat_pre;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat;
    public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State complete;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.start;
      this.Target(this.duplicant);
      this.start.Enter((StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State.Callback) (smi =>
      {
        if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat == null)
          smi.GoTo((StateMachine.BaseState) this.delay);
        else
          smi.GoTo((StateMachine.BaseState) this.remove_hat);
      }));
      this.remove_hat.ToggleAnims("anim_hat_kanim", 0.0f).PlayAnim("hat_off").OnAnimQueueComplete(this.delay);
      this.delay.ToggleThought(Db.Get().Thoughts.NewRole, (Func<SwitchRoleHatChore.StatesInstance, bool>) null).ToggleExpression(Db.Get().Expressions.Happy, (Func<SwitchRoleHatChore.StatesInstance, bool>) null).ToggleAnims("anim_selfish_kanim", 0.0f).QueueAnim("working_pre", false, (Func<SwitchRoleHatChore.StatesInstance, string>) null).QueueAnim("working_loop", false, (Func<SwitchRoleHatChore.StatesInstance, string>) null).QueueAnim("working_pst", false, (Func<SwitchRoleHatChore.StatesInstance, string>) null).OnAnimQueueComplete(this.applyHat_pre);
      this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0.0f).Enter((StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State.Callback) (smi => this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat())).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
      this.applyHat.ToggleAnims("anim_hat_kanim", 0.0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
      this.complete.ReturnSuccess();
    }
  }
}
