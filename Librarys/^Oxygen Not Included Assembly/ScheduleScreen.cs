// Decompiled with JetBrains decompiler
// Type: ScheduleScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ScheduleScreen : KScreen
{
  [SerializeField]
  private SchedulePaintButton paintButtonPrefab;
  [SerializeField]
  private GameObject paintButtonContainer;
  [SerializeField]
  private ScheduleScreenEntry scheduleEntryPrefab;
  [SerializeField]
  private GameObject scheduleEntryContainer;
  [SerializeField]
  private KButton addScheduleButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private ColorStyleSetting hygene_color;
  [SerializeField]
  private ColorStyleSetting work_color;
  [SerializeField]
  private ColorStyleSetting recreation_color;
  [SerializeField]
  private ColorStyleSetting sleep_color;
  private Dictionary<string, ColorStyleSetting> paintStyles;
  private List<ScheduleScreenEntry> entries;
  private List<SchedulePaintButton> paintButtons;
  private SchedulePaintButton selectedPaint;

  public override float GetSortKey()
  {
    return 100f;
  }

  protected override void OnPrefabInit()
  {
    this.ConsumeMouseScroll = true;
    this.entries = new List<ScheduleScreenEntry>();
    this.paintStyles = new Dictionary<string, ColorStyleSetting>();
    this.paintStyles["Hygene"] = this.hygene_color;
    this.paintStyles["Worktime"] = this.work_color;
    this.paintStyles["Recreation"] = this.recreation_color;
    this.paintStyles["Sleep"] = this.sleep_color;
  }

  protected override void OnSpawn()
  {
    this.paintButtons = new List<SchedulePaintButton>();
    foreach (ScheduleGroup allGroup in Db.Get().ScheduleGroups.allGroups)
      this.AddPaintButton(allGroup);
    foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
      this.AddScheduleEntry(schedule);
    this.addScheduleButton.onClick += new System.Action(this.OnAddScheduleClick);
    this.closeButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    ScheduleManager.Instance.onSchedulesChanged += new System.Action<List<Schedule>>(this.OnSchedulesChanged);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    ScheduleManager.Instance.onSchedulesChanged -= new System.Action<List<Schedule>>(this.OnSchedulesChanged);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.Activate();
  }

  private void AddPaintButton(ScheduleGroup group)
  {
    SchedulePaintButton schedulePaintButton = Util.KInstantiateUI<SchedulePaintButton>(this.paintButtonPrefab.gameObject, this.paintButtonContainer, true);
    schedulePaintButton.SetGroup(group, this.paintStyles, new System.Action<SchedulePaintButton>(this.OnPaintButtonClick));
    schedulePaintButton.SetToggle(false);
    this.paintButtons.Add(schedulePaintButton);
  }

  private void OnAddScheduleClick()
  {
    ScheduleManager.Instance.AddSchedule(Db.Get().ScheduleGroups.allGroups, (string) null, false);
  }

  private void OnPaintButtonClick(SchedulePaintButton clicked)
  {
    if ((UnityEngine.Object) this.selectedPaint != (UnityEngine.Object) clicked)
    {
      foreach (SchedulePaintButton paintButton in this.paintButtons)
        paintButton.SetToggle((UnityEngine.Object) paintButton == (UnityEngine.Object) clicked);
      this.selectedPaint = clicked;
    }
    else
    {
      clicked.SetToggle(false);
      this.selectedPaint = (SchedulePaintButton) null;
    }
  }

  private void OnPaintDragged(ScheduleScreenEntry entry, float ratio)
  {
    if ((UnityEngine.Object) this.selectedPaint == (UnityEngine.Object) null)
      return;
    int idx = Mathf.FloorToInt(ratio * (float) entry.schedule.GetBlocks().Count);
    entry.schedule.SetGroup(idx, this.selectedPaint.group);
  }

  private void AddScheduleEntry(Schedule schedule)
  {
    ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
    scheduleScreenEntry.Setup(schedule, this.paintStyles, new System.Action<ScheduleScreenEntry, float>(this.OnPaintDragged));
    this.entries.Add(scheduleScreenEntry);
  }

  private void OnSchedulesChanged(List<Schedule> schedules)
  {
    foreach (Component entry in this.entries)
      Util.KDestroyGameObject(entry);
    this.entries.Clear();
    foreach (Schedule schedule in schedules)
      this.AddScheduleEntry(schedule);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.CheckBlockedInput())
    {
      if (e.Consumed)
        return;
      e.Consumed = true;
    }
    else
      base.OnKeyDown(e);
  }

  private bool CheckBlockedInput()
  {
    bool flag = false;
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
    {
      GameObject selectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject != (UnityEngine.Object) null)
      {
        foreach (ScheduleScreenEntry entry in this.entries)
        {
          if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) entry.GetNameInputField())
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }
}
