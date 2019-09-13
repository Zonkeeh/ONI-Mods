// Decompiled with JetBrains decompiler
// Type: UglyCryChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
  public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, System.Action<Chore> on_complete = null)
    : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
  {
    this.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
  {
    private AmountInstance bodyTemperature;

    public StatesInstance(UglyCryChore master, GameObject crier)
      : base(master)
    {
      this.sm.crier.Set(crier, this.smi);
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
    }

    public void ProduceTears(float dt)
    {
      if ((double) dt <= 0.0)
        return;
      int cell = Grid.PosToCell(this.smi.master.gameObject);
      Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
      if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
        equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, 1f * TUNING.STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, false, true);
      else
        SimMessages.AddRemoveSubstance(cell, SimHashes.Water, CellEventLogger.Instance.Tears, 1f * TUNING.STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, true, -1);
    }
  }

  public class States : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
  {
    public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;
    public UglyCryChore.States.Cry cry;
    public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;
    private Effect uglyCryingEffect;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.cry;
      this.Target(this.crier);
      this.uglyCryingEffect = new Effect("UglyCrying", (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
      this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, (string) DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, false, false, true));
      Db.Get().effects.Add(this.uglyCryingEffect);
      this.cry.defaultState = (StateMachine.BaseState) this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim", 0.0f);
      this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, (StateMachine.BaseState) this.cry.cry_loop);
      this.cry.cry_loop.ToggleAnims("anim_cry_kanim", 0.0f).Enter((StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State.Callback) (smi => smi.Play("working_loop", KAnim.PlayMode.Loop))).ScheduleGoTo(18f, (StateMachine.BaseState) this.cry.cry_pst).ToggleEffect((Func<UglyCryChore.StatesInstance, Effect>) (smi => this.uglyCryingEffect)).Update((System.Action<UglyCryChore.StatesInstance, float>) ((smi, dt) => smi.ProduceTears(dt)), UpdateRate.SIM_200ms, false);
      this.cry.cry_pst.QueueAnim("working_pst", false, (Func<UglyCryChore.StatesInstance, string>) null).OnAnimQueueComplete(this.complete);
      this.complete.AddEffect("CryFace").Enter((StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State.Callback) (smi => smi.StopSM("complete")));
    }

    public class Cry : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
    {
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;
      public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
    }
  }
}
