// Decompiled with JetBrains decompiler
// Type: ScheduleScreenEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleScreenEntry : KMonoBehaviour
{
  private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
  [SerializeField]
  private ScheduleBlockButton blockButtonPrefab;
  [SerializeField]
  private ScheduleBlockPainter blockButtonContainer;
  [SerializeField]
  private ScheduleMinionWidget minionWidgetPrefab;
  [SerializeField]
  private GameObject minionWidgetContainer;
  private ScheduleMinionWidget blankMinionWidget;
  [SerializeField]
  private EditableTitleBar title;
  [SerializeField]
  private LocText alarmField;
  [SerializeField]
  private KButton optionsButton;
  [SerializeField]
  private DialogPanel optionsPanel;
  [SerializeField]
  private LocText noteEntryLeft;
  [SerializeField]
  private LocText noteEntryRight;
  private List<ScheduleBlockButton> blockButtons;
  private List<ScheduleMinionWidget> minionWidgets;

  public Schedule schedule { get; private set; }

  public void Setup(
    Schedule schedule,
    Dictionary<string, ColorStyleSetting> paintStyles,
    System.Action<ScheduleScreenEntry, float> onPaintDragged)
  {
    this.schedule = schedule;
    this.gameObject.name = "Schedule_" + schedule.name;
    this.title.SetTitle(schedule.name);
    this.title.OnNameChanged += new System.Action<string>(this.OnNameChanged);
    this.blockButtonContainer.Setup((System.Action<float>) (f => onPaintDragged(this, f)));
    int num = 0;
    this.blockButtons = new List<ScheduleBlockButton>();
    int count = schedule.GetBlocks().Count;
    foreach (ScheduleBlock block in schedule.GetBlocks())
    {
      ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, this.blockButtonContainer.gameObject, true);
      scheduleBlockButton.Setup(num++, paintStyles, count);
      scheduleBlockButton.SetBlockTypes(block.allowed_types);
      this.blockButtons.Add(scheduleBlockButton);
    }
    this.minionWidgets = new List<ScheduleMinionWidget>();
    this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
    this.blankMinionWidget.SetupBlank(schedule);
    this.RebuildMinionWidgets();
    this.RefreshNotes();
    this.RefreshAlarmButton();
    this.optionsButton.onClick += new System.Action(this.OnOptionsClicked);
    HierarchyReferences component = this.optionsPanel.GetComponent<HierarchyReferences>();
    component.GetReference<MultiToggle>("AlarmButton").onClick += new System.Action(this.OnAlarmClicked);
    component.GetReference<KButton>("ResetButton").onClick += new System.Action(this.OnResetClicked);
    component.GetReference<KButton>("DeleteButton").onClick += new System.Action(this.OnDeleteClicked);
    schedule.onChanged += new System.Action<Schedule>(this.OnScheduleChanged);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.schedule == null)
      return;
    this.schedule.onChanged -= new System.Action<Schedule>(this.OnScheduleChanged);
  }

  public GameObject GetNameInputField()
  {
    return this.title.inputField.gameObject;
  }

  private void RebuildMinionWidgets()
  {
    if (!this.MinionWidgetsNeedRebuild())
      return;
    foreach (Component minionWidget in this.minionWidgets)
      Util.KDestroyGameObject(minionWidget);
    this.minionWidgets.Clear();
    foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
    {
      ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
      scheduleMinionWidget.Setup(@ref.Get());
      this.minionWidgets.Add(scheduleMinionWidget);
    }
    if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
    {
      this.blankMinionWidget.transform.SetAsLastSibling();
      this.blankMinionWidget.gameObject.SetActive(true);
    }
    else
      this.blankMinionWidget.gameObject.SetActive(false);
  }

  private bool MinionWidgetsNeedRebuild()
  {
    List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
    if (assigned.Count != this.minionWidgets.Count || assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
      return true;
    for (int index = 0; index < assigned.Count; ++index)
    {
      if ((UnityEngine.Object) assigned[index].Get() != (UnityEngine.Object) this.minionWidgets[index].schedulable)
        return true;
    }
    return false;
  }

  private void OnNameChanged(string newName)
  {
    this.schedule.name = newName;
    this.gameObject.name = "Schedule_" + this.schedule.name;
  }

  private void OnOptionsClicked()
  {
    this.optionsPanel.gameObject.SetActive(!this.optionsPanel.gameObject.activeSelf);
    this.optionsPanel.GetComponent<Selectable>().Select();
  }

  private void OnAlarmClicked()
  {
    this.schedule.alarmActivated = !this.schedule.alarmActivated;
    this.RefreshAlarmButton();
  }

  private void RefreshAlarmButton()
  {
    MultiToggle reference = this.optionsPanel.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("AlarmButton");
    reference.ChangeState(!this.schedule.alarmActivated ? 0 : 1);
    ToolTip component = reference.GetComponent<ToolTip>();
    component.SetSimpleTooltip((string) (!this.schedule.alarmActivated ? STRINGS.UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP : STRINGS.UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP));
    ToolTipScreen.Instance.MarkTooltipDirty(component);
    this.alarmField.text = (string) (!this.schedule.alarmActivated ? STRINGS.UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED : STRINGS.UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED);
  }

  private void OnResetClicked()
  {
    this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
  }

  private void OnDeleteClicked()
  {
    ScheduleManager.Instance.DeleteSchedule(this.schedule);
  }

  private void OnScheduleChanged(Schedule changedSchedule)
  {
    foreach (ScheduleBlockButton blockButton in this.blockButtons)
      blockButton.SetBlockTypes(changedSchedule.GetBlock(blockButton.idx).allowed_types);
    this.RefreshNotes();
    this.RebuildMinionWidgets();
  }

  private void RefreshNotes()
  {
    this.blockTypeCounts.Clear();
    foreach (Resource resource in Db.Get().ScheduleBlockTypes.resources)
      this.blockTypeCounts[resource.Id] = 0;
    foreach (ScheduleBlock block in this.schedule.GetBlocks())
    {
      foreach (ScheduleBlockType allowedType in block.allowed_types)
      {
        Dictionary<string, int> blockTypeCounts;
        string id;
        (blockTypeCounts = this.blockTypeCounts)[id = allowedType.Id] = blockTypeCounts[id] + 1;
      }
    }
    ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    int num = 0;
    foreach (KeyValuePair<string, int> blockTypeCount in this.blockTypeCounts)
    {
      if (blockTypeCount.Value == 0)
      {
        ++num;
        component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.SCHEDULEGROUPS.NOTIME, (object) Db.Get().ScheduleBlockTypes.Get(blockTypeCount.Key).Name), (ScriptableObject) null);
      }
    }
    if (num > 0)
      this.noteEntryRight.text = string.Format((string) STRINGS.UI.SCHEDULEGROUPS.MISSINGBLOCKS, (object) num);
    else
      this.noteEntryRight.text = string.Empty;
    string breakBonus = QualityOfLifeNeed.GetBreakBonus(this.blockTypeCounts[Db.Get().ScheduleBlockTypes.Recreation.Id]);
    if (breakBonus == null)
      return;
    Effect effect = Db.Get().effects.Get(breakBonus);
    if (effect == null)
      return;
    foreach (AttributeModifier selfModifier in effect.SelfModifiers)
    {
      if (selfModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
      {
        this.noteEntryLeft.text = string.Format((string) STRINGS.UI.SCHEDULESCREEN.DOWNTIME_MORALE, (object) selfModifier.GetFormattedString((GameObject) null));
        this.noteEntryLeft.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.SCHEDULESCREEN.SCHEDULE_DOWNTIME_MORALE, (object) selfModifier.GetFormattedString((GameObject) null)));
      }
    }
  }
}
