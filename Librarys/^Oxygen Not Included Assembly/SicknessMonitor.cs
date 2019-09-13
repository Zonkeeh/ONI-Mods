// Decompiled with JetBrains decompiler
// Type: SicknessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SicknessMonitor : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance>
{
  private static readonly HashedString SickPostKAnim = (HashedString) "anim_cheer_kanim";
  private static readonly HashedString[] SickPostAnims = new HashedString[3]
  {
    (HashedString) "cheer_pre",
    (HashedString) "cheer_loop",
    (HashedString) "cheer_pst"
  };
  public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State healthy;
  public SicknessMonitor.SickStates sick;
  public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post;
  public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post_nocheer;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = true;
    default_state = (StateMachine.BaseState) this.healthy;
    this.healthy.EventTransition(GameHashes.SicknessAdded, (GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State) this.sick, (StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSick()));
    this.sick.DefaultState(this.sick.minor).EventTransition(GameHashes.SicknessCured, this.post_nocheer, (StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsSick())).ToggleThought(Db.Get().Thoughts.GotInfected, (Func<SicknessMonitor.Instance, bool>) null);
    this.sick.minor.EventTransition(GameHashes.SicknessAdded, this.sick.major, (StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasMajorDisease()));
    this.sick.major.EventTransition(GameHashes.SicknessCured, this.sick.minor, (StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.HasMajorDisease())).ToggleUrge(Db.Get().Urges.RestDueToDisease).Update("AutoAssignClinic", (System.Action<SicknessMonitor.Instance, float>) ((smi, dt) => smi.AutoAssignClinic()), UpdateRate.SIM_4000ms, false).Exit((StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.UnassignClinic()));
    this.post_nocheer.Enter((StateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      new SicknessCuredFX.Instance(smi.master, new Vector3(0.0f, 0.0f, -0.1f)).StartSM();
      if (smi.IsSleepingOrSleepSchedule())
        smi.GoTo((StateMachine.BaseState) this.healthy);
      else
        smi.GoTo((StateMachine.BaseState) this.post);
    }));
    this.post.ToggleChore((Func<SicknessMonitor.Instance, Chore>) (smi => (Chore) new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, SicknessMonitor.SickPostKAnim, SicknessMonitor.SickPostAnims, KAnim.PlayMode.Once, false)), this.healthy);
  }

  public class SickStates : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State minor;
    public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State major;
  }

  public class Instance : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private Sicknesses sicknesses;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
    }

    private string OnGetToolTip(List<Notification> notifications, object data)
    {
      return (string) DUPLICANTS.STATUSITEMS.HASDISEASE.TOOLTIP;
    }

    public bool IsSick()
    {
      return this.sicknesses.Count > 0;
    }

    public bool HasMajorDisease()
    {
      foreach (ModifierInstance<Sickness> sickness in (Modifications<Sickness, SicknessInstance>) this.sicknesses)
      {
        if (sickness.modifier.severity >= Sickness.Severity.Major)
          return true;
      }
      return false;
    }

    public void AutoAssignClinic()
    {
      Ownables soleOwner = this.sm.masterTarget.Get(this.smi).GetComponent<MinionIdentity>().GetSoleOwner();
      AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
      AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
      if (slot == null || (UnityEngine.Object) slot.assignable != (UnityEngine.Object) null)
        return;
      soleOwner.AutoAssignSlot(clinic);
    }

    public void UnassignClinic()
    {
      this.sm.masterTarget.Get(this.smi).GetComponent<MinionIdentity>().GetSoleOwner().GetSlot(Db.Get().AssignableSlots.Clinic)?.Unassign(true);
    }

    public bool IsSleepingOrSleepSchedule()
    {
      Schedulable component1 = this.GetComponent<Schedulable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
        return true;
      KPrefabID component2 = this.GetComponent<KPrefabID>();
      return (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasTag(GameTags.Asleep);
    }
  }
}
