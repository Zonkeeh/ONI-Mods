// Decompiled with JetBrains decompiler
// Type: IncapacitationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.healthy;
    this.serializable = true;
    this.healthy.TagTransition(GameTags.CaloriesDepleted, this.incapacitated, false).TagTransition(GameTags.HitPointsDepleted, this.incapacitated, false).Update((System.Action<IncapacitationMonitor.Instance, float>) ((smi, dt) => smi.RecoverStamina(dt, smi)), UpdateRate.SIM_200ms, false);
    this.start_recovery.TagTransition(new Tag[2]
    {
      GameTags.CaloriesDepleted,
      GameTags.HitPointsDepleted
    }, this.healthy, true);
    this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.start_recovery, (StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((Func<IncapacitationMonitor.Instance, Chore>) (smi => (Chore) new BeIncapacitatedChore(smi.master)), (Func<IncapacitationMonitor.Instance, bool>) null).ParamTransition<float>((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Enter((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.master.Trigger(-1506500077, (object) null))).Update((System.Action<IncapacitationMonitor.Instance, float>) ((smi, dt) => smi.Bleed(dt, smi)), UpdateRate.SIM_200ms, false);
    this.die.Enter((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation())));
  }

  public class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      Health component = master.GetComponent<Health>();
      if (!(bool) ((UnityEngine.Object) component))
        return;
      component.CanBeIncapacitated = true;
    }

    public void Bleed(float dt, IncapacitationMonitor.Instance smi)
    {
      double num = (double) smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
    }

    public void RecoverStamina(float dt, IncapacitationMonitor.Instance smi)
    {
      double num = (double) smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
    }

    public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
    {
      return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
    }

    public Death GetCauseOfIncapacitation()
    {
      KPrefabID component = this.GetComponent<KPrefabID>();
      if (component.HasTag(GameTags.CaloriesDepleted))
        return Db.Get().Deaths.Starvation;
      if (component.HasTag(GameTags.HitPointsDepleted))
        return Db.Get().Deaths.Slain;
      return Db.Get().Deaths.Generic;
    }
  }
}
