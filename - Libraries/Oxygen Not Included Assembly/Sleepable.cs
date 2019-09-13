// Decompiled with JetBrains decompiler
// Type: Sleepable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Sleepable : Workable
{
  private static readonly HashedString[] normalWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[2]
  {
    (HashedString) "hat_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString normalWorkPstAnim = (HashedString) "working_pst";
  private static readonly HashedString hatWorkPstAnim = (HashedString) "hat_pst";
  public string effectName = "Sleep";
  public bool stretchOnWake = true;
  private const float STRECH_CHANCE = 0.33f;
  [MyCmpGet]
  private Operational operational;
  public List<string> wakeEffects;
  private float wakeTime;
  private bool isDoneSleeping;

  private Sleepable()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = (StatusItem) null;
    this.synchronizeAnims = false;
    this.triggerWorkReactions = false;
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    Components.Sleepables.Add(this);
    this.SetWorkTime(float.PositiveInfinity);
  }

  public override HashedString[] GetWorkAnims(Worker worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null)
      return Sleepable.hatWorkAnims;
    return Sleepable.normalWorkAnims;
  }

  public override HashedString GetWorkPstAnim(
    Worker worker,
    bool successfully_completed)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null)
      return Sleepable.hatWorkPstAnim;
    return Sleepable.normalWorkPstAnim;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(true, false);
    worker.Trigger(-1283701846, (object) this);
    worker.GetComponent<Effects>().Add(this.effectName, false);
    this.isDoneSleeping = false;
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (this.isDoneSleeping)
    {
      if ((double) Time.time > (double) this.wakeTime)
        return true;
    }
    else if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
    {
      this.isDoneSleeping = true;
      this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
    }
    return false;
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(false, false);
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
      return;
    Effects component = worker.GetComponent<Effects>();
    component.Remove(this.effectName);
    if (this.wakeEffects != null)
    {
      foreach (string wakeEffect in this.wakeEffects)
        component.Add(wakeEffect, true);
    }
    if (this.stretchOnWake && (double) UnityEngine.Random.value < 0.330000013113022)
    {
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_react_morning_stretch_kanim", new HashedString[1]
      {
        (HashedString) "react"
      }, (Func<StatusItem>) null);
    }
    if ((double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value >= (double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
      return;
    worker.Trigger(1338475637, (object) this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Sleepables.Remove(this);
  }
}
