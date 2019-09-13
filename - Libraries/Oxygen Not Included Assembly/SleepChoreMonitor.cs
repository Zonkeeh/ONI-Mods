// Decompiled with JetBrains decompiler
// Type: SleepChoreMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance>
{
  public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State checkforbed;
  public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout;
  public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State sleeponfloor;
  public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State bedassigned;
  public StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.TargetParameter bed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = false;
    this.root.EventHandler(GameHashes.AssignablesChanged, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.UpdateBed()));
    this.satisfied.EventTransition(GameHashes.AddUrge, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasSleepUrge()));
    this.checkforbed.Enter("SetBed", (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.UpdateBed();
      if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep())
        smi.GoTo((StateMachine.BaseState) this.passingout);
      else if ((UnityEngine.Object) this.bed.Get(smi) == (UnityEngine.Object) null || !smi.IsBedReachable())
        smi.GoTo((StateMachine.BaseState) this.sleeponfloor);
      else
        smi.GoTo((StateMachine.BaseState) this.bedassigned);
    }));
    this.passingout.ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreatePassingOutChore), this.satisfied, this.satisfied);
    this.sleeponfloor.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsBedReachable())).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepOnFloorChore), this.satisfied, this.satisfied);
    this.bedassigned.ParamTransition<GameObject>((StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Parameter<GameObject>) this.bed, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p == (UnityEngine.Object) null)).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsBedReachable())).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepChore), this.satisfied, this.satisfied);
  }

  private Chore CreatePassingOutChore(SleepChoreMonitor.Instance smi)
  {
    GameObject passedOutLocator = smi.CreatePassedOutLocator();
    return (Chore) new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, passedOutLocator, true, false);
  }

  private Chore CreateSleepOnFloorChore(SleepChoreMonitor.Instance smi)
  {
    GameObject floorLocator = smi.CreateFloorLocator();
    return (Chore) new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, floorLocator, true, true);
  }

  private Chore CreateSleepChore(SleepChoreMonitor.Instance smi)
  {
    return (Chore) new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true);
  }

  public class Instance : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private int locatorCell;
    public GameObject locator;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
    }

    public void UpdateBed()
    {
      Ownables soleOwner = this.sm.masterTarget.Get(this.smi).GetComponent<MinionIdentity>().GetSoleOwner();
      Assignable assignable1 = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
      Assignable assignable2;
      if ((UnityEngine.Object) assignable1 != (UnityEngine.Object) null && assignable1.CanAutoAssignTo((IAssignableIdentity) this.sm.masterTarget.Get(this.smi).GetComponent<MinionIdentity>().assignableProxy.Get()))
      {
        assignable2 = assignable1;
      }
      else
      {
        assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
        if ((UnityEngine.Object) assignable2 == (UnityEngine.Object) null)
        {
          assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
          if ((UnityEngine.Object) assignable2 != (UnityEngine.Object) null)
            this.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>().Update();
        }
      }
      this.smi.sm.bed.Set((KMonoBehaviour) assignable2, this.smi);
    }

    public bool HasSleepUrge()
    {
      return this.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep);
    }

    public bool IsBedReachable()
    {
      AssignableReachabilitySensor sensor = this.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
      return sensor.IsReachable(Db.Get().AssignableSlots.Bed) || sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
    }

    public GameObject CreatePassedOutLocator()
    {
      Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(this.master.gameObject);
      safeFloorLocator.effectName = "PassedOutSleep";
      safeFloorLocator.wakeEffects = new List<string>()
      {
        "SoreBack"
      };
      safeFloorLocator.stretchOnWake = false;
      return safeFloorLocator.gameObject;
    }

    public GameObject CreateFloorLocator()
    {
      Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(this.master.gameObject);
      safeFloorLocator.effectName = "FloorSleep";
      safeFloorLocator.wakeEffects = new List<string>()
      {
        "SoreBack"
      };
      safeFloorLocator.stretchOnWake = false;
      return safeFloorLocator.gameObject;
    }
  }
}
