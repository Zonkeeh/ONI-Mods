// Decompiled with JetBrains decompiler
// Type: ResetSkillsStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetSkillsStation : Workable
{
  [MyCmpReq]
  public Assignable assignable;
  private Notification notification;
  private Chore chore;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnAssign(this.assignable.assignee);
    this.assignable.OnAssign += new System.Action<IAssignableIdentity>(this.OnAssign);
  }

  private void OnAssign(IAssignableIdentity obj)
  {
    if (obj != null)
    {
      this.CreateChore();
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Unassigned");
      this.chore = (Chore) null;
    }
  }

  private void CreateChore()
  {
    this.chore = (Chore) new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.Train, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<Operational>().SetActive(true, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.assignable.Unassign();
    MinionResume component = worker.GetComponent<MinionResume>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.ResetSkillLevels(true);
    component.SetHats(component.CurrentHat, (string) null);
    component.ApplyTargetHat();
    this.notification = new Notification((string) MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false)), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
    worker.GetComponent<Notifier>().Add(this.notification, string.Empty);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<Operational>().SetActive(false, false);
    this.chore = (Chore) null;
  }
}
