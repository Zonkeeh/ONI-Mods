// Decompiled with JetBrains decompiler
// Type: ScheduleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;

public class ScheduleManager : KMonoBehaviour, ISim33ms
{
  [Serialize]
  private List<Schedule> schedules;
  [Serialize]
  private int lastIdx;
  [Serialize]
  private int scheduleNameIncrementor;
  public static ScheduleManager Instance;

  public event System.Action<List<Schedule>> onSchedulesChanged;

  public static void DestroyInstance()
  {
    ScheduleManager.Instance = (ScheduleManager) null;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if (this.schedules.Count != 0)
      return;
    this.SetupDefaultSchedule();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.schedules = new List<Schedule>();
    ScheduleManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    if (this.schedules.Count == 0)
      this.SetupDefaultSchedule();
    foreach (Schedule schedule in this.schedules)
      schedule.ClearNullReferences();
    foreach (Component component1 in Components.LiveMinionIdentities.Items)
    {
      Schedulable component2 = component1.GetComponent<Schedulable>();
      if (this.GetSchedule(component2) == null)
        this.schedules[0].Assign(component2);
    }
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnAddDupe);
    Components.LiveMinionIdentities.OnRemove += new System.Action<MinionIdentity>(this.OnRemoveDupe);
  }

  private void OnAddDupe(MinionIdentity minion)
  {
    Schedulable component = minion.GetComponent<Schedulable>();
    if (this.GetSchedule(component) != null)
      return;
    this.schedules[0].Assign(component);
  }

  private void OnRemoveDupe(MinionIdentity minion)
  {
    Schedulable component = minion.GetComponent<Schedulable>();
    this.GetSchedule(component)?.Unassign(component);
  }

  private void SetupDefaultSchedule()
  {
    this.AddSchedule(Db.Get().ScheduleGroups.allGroups, (string) UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT, true);
  }

  public void AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
  {
    ++this.scheduleNameIncrementor;
    if (name == null)
      name = string.Format((string) UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, (object) this.scheduleNameIncrementor.ToString());
    this.schedules.Add(new Schedule(name, groups, alarmOn));
    if (this.onSchedulesChanged == null)
      return;
    this.onSchedulesChanged(this.schedules);
  }

  public void DeleteSchedule(Schedule schedule)
  {
    if (this.schedules.Count == 1)
      return;
    List<Ref<Schedulable>> assigned = schedule.GetAssigned();
    this.schedules.Remove(schedule);
    foreach (Ref<Schedulable> @ref in assigned)
      this.schedules[0].Assign(@ref.Get());
    if (this.onSchedulesChanged == null)
      return;
    this.onSchedulesChanged(this.schedules);
  }

  public Schedule GetSchedule(Schedulable schedulable)
  {
    foreach (Schedule schedule in this.schedules)
    {
      if (schedule.IsAssigned(schedulable))
        return schedule;
    }
    return (Schedule) null;
  }

  public List<Schedule> GetSchedules()
  {
    return this.schedules;
  }

  public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
  {
    int blockIdx = Schedule.GetBlockIdx();
    return this.GetSchedule(schedulable).GetBlock(blockIdx).IsAllowed(schedule_block_type);
  }

  public void Sim33ms(float dt)
  {
    int blockIdx = Schedule.GetBlockIdx();
    if (blockIdx == this.lastIdx)
      return;
    foreach (Schedule schedule in this.schedules)
      schedule.Tick();
    this.lastIdx = blockIdx;
  }

  public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
  {
    this.GetComponent<Notifier>().Add(new Notification(string.Format((string) MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, (object) schedule.name, (object) block.name), NotificationType.Good, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => string.Format((string) MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP, (object) schedule.name, (object) block.name, (object) Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip)), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null), string.Empty);
    this.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
  }

  [DebuggerHidden]
  private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ScheduleManager.\u003CPlayScheduleTone\u003Ec__Iterator0()
    {
      schedule = schedule,
      forwards = forwards,
      \u0024this = this
    };
  }

  private void PlayTone(int pitch, bool forwards)
  {
    FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero);
    int num1 = (int) instance.setParameterValue("WorkChime_pitch", (float) pitch);
    int num2 = (int) instance.setParameterValue("WorkChime_start", !forwards ? 0.0f : 1f);
    KFMOD.EndOneShot(instance);
  }

  public class Tuning : TuningData<ScheduleManager.Tuning>
  {
    public float toneSpacingSeconds;
    public int minToneIndex;
    public int maxToneIndex;
    public int firstLastToneSpacing;
  }
}
