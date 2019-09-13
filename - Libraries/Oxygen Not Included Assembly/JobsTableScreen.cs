// Decompiled with JetBrains decompiler
// Type: JobsTableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JobsTableScreen : TableScreen
{
  [SerializeField]
  private Color32 skillOutlineColourLow = (Color32) Color.white;
  [SerializeField]
  private Color32 skillOutlineColourHigh = (Color32) new Color(0.7215686f, 0.4431373f, 0.5803922f);
  [SerializeField]
  private int skillLevelLow = 1;
  [SerializeField]
  private int skillLevelHigh = 10;
  [SerializeField]
  private bool dynamicRowSpacing = true;
  private HashSet<MinionIdentity> dirty_single_minion_rows = new HashSet<MinionIdentity>();
  private List<KeyValuePair<GameObject, JobsTableScreen.SkillEventHandlerID>> EffectListeners = new List<KeyValuePair<GameObject, JobsTableScreen.SkillEventHandlerID>>();
  [SerializeField]
  private KButton settingsButton;
  [SerializeField]
  private KButton resetSettingsButton;
  [SerializeField]
  private KButton toggleAdvancedModeButton;
  [SerializeField]
  private KImage optionsPanel;
  public static JobsTableScreen Instance;
  public TextStyleSetting TooltipTextStyle_Ability;
  public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;
  public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;
  private static List<JobsTableScreen.PriorityInfo> _priorityInfo;
  private List<Sprite> prioritySprites;

  public override float GetSortKey()
  {
    return 101f;
  }

  public static List<JobsTableScreen.PriorityInfo> priorityInfo
  {
    get
    {
      if (JobsTableScreen._priorityInfo == null)
        JobsTableScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>()
        {
          new JobsTableScreen.PriorityInfo(0, Assets.GetSprite((HashedString) "icon_priority_disabled"), STRINGS.UI.JOBSSCREEN.PRIORITY.DISABLED),
          new JobsTableScreen.PriorityInfo(1, Assets.GetSprite((HashedString) "icon_priority_down_2"), STRINGS.UI.JOBSSCREEN.PRIORITY.VERYLOW),
          new JobsTableScreen.PriorityInfo(2, Assets.GetSprite((HashedString) "icon_priority_down"), STRINGS.UI.JOBSSCREEN.PRIORITY.LOW),
          new JobsTableScreen.PriorityInfo(3, Assets.GetSprite((HashedString) "icon_priority_flat"), STRINGS.UI.JOBSSCREEN.PRIORITY.STANDARD),
          new JobsTableScreen.PriorityInfo(4, Assets.GetSprite((HashedString) "icon_priority_up"), STRINGS.UI.JOBSSCREEN.PRIORITY.HIGH),
          new JobsTableScreen.PriorityInfo(5, Assets.GetSprite((HashedString) "icon_priority_up_2"), STRINGS.UI.JOBSSCREEN.PRIORITY.VERYHIGH),
          new JobsTableScreen.PriorityInfo(5, Assets.GetSprite((HashedString) "icon_priority_automatic"), STRINGS.UI.JOBSSCREEN.PRIORITY.VERYHIGH)
        };
      return JobsTableScreen._priorityInfo;
    }
  }

  protected override void OnActivate()
  {
    JobsTableScreen.Instance = this;
    this.title = (string) STRINGS.UI.JOBSSCREEN.TITLE;
    base.OnActivate();
    this.resetSettingsButton.onClick += new System.Action(this.OnResetSettingsClicked);
    this.prioritySprites = new List<Sprite>();
    foreach (JobsTableScreen.PriorityInfo priorityInfo in JobsTableScreen.priorityInfo)
      this.prioritySprites.Add(priorityInfo.sprite);
    this.AddPortraitColumn("Portrait", new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_portrait), (Comparison<IAssignableIdentity>) null, true);
    this.AddButtonLabelColumn("Names", new System.Action<IAssignableIdentity, GameObject>(this.ConfigureNameLabel), new Func<IAssignableIdentity, GameObject, string>(((TableScreen) this).get_value_name_label), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectMinion()), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectAndFocusMinion()), new Comparison<IAssignableIdentity>(((TableScreen) this).compare_rows_alphabetical), (System.Action<IAssignableIdentity, GameObject, ToolTip>) null, new System.Action<IAssignableIdentity, GameObject, ToolTip>(((TableScreen) this).on_tooltip_sort_alphabetically), false);
    List<ChoreGroup> source = new List<ChoreGroup>((IEnumerable<ChoreGroup>) Db.Get().ChoreGroups.resources);
    source.OrderByDescending<ChoreGroup, int>((Func<ChoreGroup, int>) (group => group.DefaultPersonalPriority)).ThenBy<ChoreGroup, string>((Func<ChoreGroup, string>) (group => group.Name));
    foreach (ChoreGroup choreGroup in source)
    {
      PrioritizationGroupTableColumn groupTableColumn = new PrioritizationGroupTableColumn((object) choreGroup, new System.Action<IAssignableIdentity, GameObject>(this.LoadValue), new System.Action<object, int>(this.ChangePersonalPriority), new Func<object, string>(this.HoverPersonalPriority), new System.Action<object, int>(this.ChangeColumnPriority), new Func<object, string>(this.HoverChangeColumnPriorityButton), new System.Action<object>(this.OnSortClicked), new Func<object, string>(this.OnSortHovered));
      this.RegisterColumn(choreGroup.Id, (TableColumn) groupTableColumn);
    }
    this.RegisterColumn("prioritize_row", (TableColumn) new PrioritizeRowTableColumn((object) null, new System.Action<object, int>(this.ChangeRowPriority), new Func<object, int, string>(this.HoverChangeRowPriorityButton)));
    this.settingsButton.onClick += new System.Action(this.OnSettingsButtonClicked);
    this.resetSettingsButton.onClick += new System.Action(this.OnResetSettingsClicked);
    this.toggleAdvancedModeButton.onClick += new System.Action(this.OnAdvancedModeToggleClicked);
    this.toggleAdvancedModeButton.fgImage.gameObject.SetActive(Game.Instance.advancedPersonalPriorities);
    this.RefreshEffectListeners();
  }

  private string HoverPersonalPriority(object widget_go_obj)
  {
    GameObject widget_go = widget_go_obj as GameObject;
    ChoreGroup userData = (this.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
    string str1 = (string) null;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        string str2 = STRINGS.UI.JOBSSCREEN.HEADER_TOOLTIP.ToString().Replace("{Job}", userData.Name);
        string str3 = STRINGS.UI.JOBSSCREEN.HEADER_DETAILS_TOOLTIP.ToString().Replace("{Description}", userData.description);
        HashSet<string> stringSet = new HashSet<string>();
        foreach (ChoreType choreType in userData.choreTypes)
          stringSet.Add(choreType.Name);
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (string str4 in stringSet)
        {
          stringBuilder.Append(str4);
          if (num < stringSet.Count - 1)
            stringBuilder.Append(", ");
          ++num;
        }
        string newValue = str3.Replace("{ChoreList}", stringBuilder.ToString());
        return str2.Replace("{Details}", newValue);
      case TableRow.RowType.Default:
        str1 = STRINGS.UI.JOBSSCREEN.NEW_MINION_ITEM_TOOLTIP.ToString();
        break;
      case TableRow.RowType.Minion:
      case TableRow.RowType.StoredMinon:
        str1 = STRINGS.UI.JOBSSCREEN.ITEM_TOOLTIP.ToString().Replace("{Name}", widgetRow.name);
        break;
    }
    ToolTip componentInChildren = widget_go.GetComponentInChildren<ToolTip>();
    IAssignableIdentity identity = widgetRow.GetIdentity();
    MinionIdentity cmp = identity as MinionIdentity;
    if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
    {
      IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
      int personalPriority = priorityManager.GetPersonalPriority(userData);
      string priorityStr = (string) this.GetPriorityStr(personalPriority);
      string priorityValue = this.GetPriorityValue(personalPriority);
      if (priorityManager.IsChoreGroupDisabled(userData))
      {
        Trait trait1 = (Trait) null;
        foreach (Trait trait2 in cmp.GetComponent<Traits>().TraitList)
        {
          if (trait2.disabledChoreGroups != null)
          {
            foreach (Resource disabledChoreGroup in trait2.disabledChoreGroups)
            {
              if (disabledChoreGroup.IdHash == userData.IdHash)
              {
                trait1 = trait2;
                break;
              }
            }
            if (trait1 != null)
              break;
          }
        }
        string newString = STRINGS.UI.JOBSSCREEN.TRAIT_DISABLED.ToString().Replace("{Name}", cmp.GetProperName()).Replace("{Job}", userData.Name).Replace("{Trait}", trait1.Name);
        componentInChildren.ClearMultiStringTooltip();
        componentInChildren.AddMultiStringTooltip(newString, (ScriptableObject) null);
      }
      else
      {
        string newString1 = str1.Replace("{Job}", userData.Name).Replace("{Priority}", priorityStr).Replace("{PriorityValue}", priorityValue);
        componentInChildren.ClearMultiStringTooltip();
        componentInChildren.AddMultiStringTooltip(newString1, (ScriptableObject) null);
        if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
        {
          string str4 = ("\n" + STRINGS.UI.JOBSSCREEN.MINION_SKILL_TOOLTIP.ToString()).Replace("{Name}", cmp.GetProperName()).Replace("{Attribute}", userData.attribute.Name);
          float totalValue = cmp.GetAttributes().Get(userData.attribute).GetTotalValue();
          TextStyleSetting textStyleAbility = this.TooltipTextStyle_Ability;
          string newString2 = str4 + GameUtil.ColourizeString((Color32) textStyleAbility.textColor, totalValue.ToString());
          componentInChildren.AddMultiStringTooltip(newString2, (ScriptableObject) null);
        }
        componentInChildren.AddMultiStringTooltip(STRINGS.UI.HORIZONTAL_RULE + "\n" + this.GetUsageString(), (ScriptableObject) null);
      }
    }
    else if ((UnityEngine.Object) (identity as StoredMinionIdentity) != (UnityEngine.Object) null)
      componentInChildren.AddMultiStringTooltip(string.Format((string) STRINGS.UI.JOBSSCREEN.CANNOT_ADJUST_PRIORITY, (object) identity.GetProperName(), (object) (identity as StoredMinionIdentity).GetStorageReason()), (ScriptableObject) null);
    return string.Empty;
  }

  private string HoverChangeColumnPriorityButton(object widget_go_obj)
  {
    ChoreGroup userData = (this.GetWidgetColumn(widget_go_obj as GameObject) as PrioritizationGroupTableColumn).userData as ChoreGroup;
    return STRINGS.UI.JOBSSCREEN.HEADER_CHANGE_TOOLTIP.ToString().Replace("{Job}", userData.Name);
  }

  private string GetUsageString()
  {
    return GameUtil.ReplaceHotkeyString((string) STRINGS.UI.JOBSSCREEN.INCREASE_PRIORITY_TUTORIAL, Action.MouseLeft) + "\n" + GameUtil.ReplaceHotkeyString((string) STRINGS.UI.JOBSSCREEN.DECREASE_PRIORITY_TUTORIAL, Action.MouseRight);
  }

  private string HoverChangeRowPriorityButton(object widget_go_obj, int delta)
  {
    GameObject widget_go = widget_go_obj as GameObject;
    LocString locString1 = (LocString) null;
    LocString locString2 = (LocString) null;
    string newValue = (string) null;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        Debug.Assert(false);
        return (string) null;
      case TableRow.RowType.Default:
        locString1 = STRINGS.UI.JOBSSCREEN.INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP;
        locString2 = STRINGS.UI.JOBSSCREEN.DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP;
        break;
      case TableRow.RowType.Minion:
        locString1 = STRINGS.UI.JOBSSCREEN.INCREASE_ROW_PRIORITY_MINION_TOOLTIP;
        locString2 = STRINGS.UI.JOBSSCREEN.DECREASE_ROW_PRIORITY_MINION_TOOLTIP;
        newValue = widgetRow.GetIdentity().GetProperName();
        break;
      case TableRow.RowType.StoredMinon:
        StoredMinionIdentity identity = widgetRow.GetIdentity() as StoredMinionIdentity;
        if ((UnityEngine.Object) identity != (UnityEngine.Object) null)
          return string.Format((string) STRINGS.UI.JOBSSCREEN.CANNOT_ADJUST_PRIORITY, (object) identity.GetProperName(), (object) identity.GetStorageReason());
        break;
    }
    string str = (delta <= 0 ? (object) locString2 : (object) locString1).ToString();
    if (newValue != null)
      str = str.Replace("{Name}", newValue);
    return str;
  }

  private void OnSortClicked(object widget_go_obj)
  {
    PrioritizationGroupTableColumn widgetColumn = this.GetWidgetColumn(widget_go_obj as GameObject) as PrioritizationGroupTableColumn;
    ChoreGroup chore_group = widgetColumn.userData as ChoreGroup;
    if (this.active_sort_column == widgetColumn)
      this.sort_is_reversed = !this.sort_is_reversed;
    this.active_sort_column = (TableColumn) widgetColumn;
    this.active_sort_method = (Comparison<IAssignableIdentity>) ((a, b) =>
    {
      MinionIdentity minionIdentity1 = a as MinionIdentity;
      MinionIdentity minionIdentity2 = b as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
        return 0;
      if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
        return -1;
      if ((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
        return 1;
      ChoreConsumer component1 = minionIdentity1.GetComponent<ChoreConsumer>();
      ChoreConsumer component2 = minionIdentity2.GetComponent<ChoreConsumer>();
      if (component1.IsChoreGroupDisabled(chore_group))
        return 1;
      if (component2.IsChoreGroupDisabled(chore_group))
        return -1;
      int personalPriority1 = component1.GetPersonalPriority(chore_group);
      int personalPriority2 = component2.GetPersonalPriority(chore_group);
      if (personalPriority1 == personalPriority2)
        return minionIdentity1.name.CompareTo(minionIdentity2.name);
      return personalPriority2 - personalPriority1;
    });
    this.SortRows();
  }

  private string OnSortHovered(object widget_go_obj)
  {
    ChoreGroup userData = (this.GetWidgetColumn(widget_go_obj as GameObject) as PrioritizationGroupTableColumn).userData as ChoreGroup;
    return STRINGS.UI.JOBSSCREEN.SORT_TOOLTIP.ToString().Replace("{Job}", userData.Name);
  }

  private IPersonalPriorityManager GetPriorityManager(TableRow row)
  {
    IPersonalPriorityManager personalPriorityManager = (IPersonalPriorityManager) null;
    switch (row.rowType)
    {
      case TableRow.RowType.Default:
        personalPriorityManager = (IPersonalPriorityManager) Immigration.Instance;
        break;
      case TableRow.RowType.Minion:
        MinionIdentity identity = row.GetIdentity() as MinionIdentity;
        if ((UnityEngine.Object) identity != (UnityEngine.Object) null)
        {
          personalPriorityManager = (IPersonalPriorityManager) identity.GetComponent<ChoreConsumer>();
          break;
        }
        break;
      case TableRow.RowType.StoredMinon:
        personalPriorityManager = (IPersonalPriorityManager) (row.GetIdentity() as StoredMinionIdentity);
        break;
    }
    return personalPriorityManager;
  }

  private LocString GetPriorityStr(int priority)
  {
    priority = Mathf.Clamp(priority, 0, 5);
    LocString locString = (LocString) null;
    foreach (JobsTableScreen.PriorityInfo priorityInfo in JobsTableScreen.priorityInfo)
    {
      if (priorityInfo.priority == priority)
        locString = priorityInfo.name;
    }
    return locString;
  }

  private string GetPriorityValue(int priority)
  {
    return (priority * 10).ToString();
  }

  private void LoadValue(IAssignableIdentity minion, GameObject widget_go)
  {
    if ((UnityEngine.Object) widget_go == (UnityEngine.Object) null)
      return;
    ChoreGroup userData = (this.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        this.InitializeHeader(userData, widget_go);
        break;
      case TableRow.RowType.Default:
      case TableRow.RowType.Minion:
      case TableRow.RowType.StoredMinon:
        bool flag = this.GetPriorityManager(widgetRow).IsChoreGroupDisabled(userData);
        HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
        (component.GetReference("FG") as KImage).raycastTarget = flag;
        (component.GetReference("FGToolTip") as ToolTip).enabled = flag;
        break;
    }
    IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
    if (priorityManager == null)
      return;
    this.UpdateWidget(widget_go, userData, priorityManager);
  }

  private JobsTableScreen.PriorityInfo GetPriorityInfo(int priority)
  {
    JobsTableScreen.PriorityInfo priorityInfo = new JobsTableScreen.PriorityInfo();
    for (int index = 0; index < JobsTableScreen.priorityInfo.Count; ++index)
    {
      if (JobsTableScreen.priorityInfo[index].priority == priority)
      {
        priorityInfo = JobsTableScreen.priorityInfo[index];
        break;
      }
    }
    return priorityInfo;
  }

  private void ChangePersonalPriority(object widget_go_obj, int delta)
  {
    GameObject widget_go = widget_go_obj as GameObject;
    if (widget_go_obj == null)
      return;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Header)
      Debug.Assert(false);
    ChoreGroup userData = (this.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn).userData as ChoreGroup;
    IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
    this.ChangePersonalPriority(priorityManager, userData, delta, true);
    this.UpdateWidget(widget_go, userData, priorityManager);
  }

  private void ChangeColumnPriority(object widget_go_obj, int new_priority)
  {
    GameObject widget_go = widget_go_obj as GameObject;
    if (widget_go_obj == null)
      return;
    if (this.GetWidgetRow(widget_go).rowType != TableRow.RowType.Header)
      Debug.Assert(false);
    PrioritizationGroupTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as PrioritizationGroupTableColumn;
    ChoreGroup userData = widgetColumn.userData as ChoreGroup;
    foreach (TableRow row in this.rows)
    {
      IPersonalPriorityManager priorityManager = this.GetPriorityManager(row);
      if (priorityManager != null)
      {
        priorityManager.SetPersonalPriority(userData, new_priority);
        this.UpdateWidget(row.GetWidget((TableColumn) widgetColumn), userData, priorityManager);
      }
    }
  }

  private void ChangeRowPriority(object widget_go_obj, int delta)
  {
    GameObject widget_go = widget_go_obj as GameObject;
    if (widget_go_obj == null)
      return;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Header)
    {
      Debug.Assert(false);
    }
    else
    {
      IPersonalPriorityManager priorityManager = this.GetPriorityManager(widgetRow);
      foreach (TableColumn tableColumn in this.columns.Values)
      {
        PrioritizationGroupTableColumn groupTableColumn = tableColumn as PrioritizationGroupTableColumn;
        if (groupTableColumn != null)
        {
          ChoreGroup userData = groupTableColumn.userData as ChoreGroup;
          GameObject widget = widgetRow.GetWidget((TableColumn) groupTableColumn);
          this.ChangePersonalPriority(priorityManager, userData, delta, false);
          this.UpdateWidget(widget, userData, priorityManager);
        }
      }
    }
  }

  private void ChangePersonalPriority(
    IPersonalPriorityManager priority_mgr,
    ChoreGroup chore_group,
    int delta,
    bool wrap_around)
  {
    if (priority_mgr.IsChoreGroupDisabled(chore_group))
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
    }
    else
    {
      int num1 = priority_mgr.GetPersonalPriority(chore_group) + delta;
      if (wrap_around)
      {
        num1 %= 6;
        if (num1 < 0)
          num1 += 6;
      }
      int num2 = Mathf.Clamp(num1, 0, 5);
      priority_mgr.SetPersonalPriority(chore_group, num2);
      if (delta > 0)
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
      else
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
    }
  }

  private void UpdateWidget(
    GameObject widget_go,
    ChoreGroup chore_group,
    IPersonalPriorityManager priority_mgr)
  {
    int num1 = 0;
    int num2 = 0;
    bool flag = priority_mgr.IsChoreGroupDisabled(chore_group);
    if (!flag)
      num2 = priority_mgr.GetPersonalPriority(chore_group);
    int num3 = Mathf.Clamp(num2, 0, 5);
    for (int index = 0; index < JobsTableScreen.priorityInfo.Count - 1; ++index)
    {
      if (JobsTableScreen.priorityInfo[index].priority == num3)
      {
        num1 = index;
        break;
      }
    }
    OptionSelector component = widget_go.GetComponent<OptionSelector>();
    int num4 = priority_mgr == null ? 0 : priority_mgr.GetAssociatedSkillLevel(chore_group);
    Color32 color32 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 128);
    if (num4 > 0)
      color32 = Color32.Lerp(this.skillOutlineColourLow, this.skillOutlineColourHigh, (float) (num4 - this.skillLevelLow) / (float) (this.skillLevelHigh - this.skillLevelLow));
    component.ConfigureItem((flag ? 1 : 0) != 0, new OptionSelector.DisplayOptionInfo()
    {
      bgOptions = (IList<Sprite>) null,
      fgOptions = (IList<Sprite>) this.prioritySprites,
      bgIndex = 0,
      fgIndex = num1,
      fillColour = color32
    });
    ToolTip componentInChildren = widget_go.transform.GetComponentInChildren<ToolTip>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    componentInChildren.toolTip = this.HoverPersonalPriority((object) widget_go);
    componentInChildren.forceRefresh = true;
  }

  public void ToggleColumnSortWidgets(bool show)
  {
    foreach (KeyValuePair<string, TableColumn> column in this.columns)
    {
      if ((UnityEngine.Object) column.Value.column_sort_toggle != (UnityEngine.Object) null)
        column.Value.column_sort_toggle.gameObject.SetActive(show);
    }
  }

  public void Refresh(MinionResume minion_resume)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    foreach (TableRow row in this.rows)
    {
      IAssignableIdentity identity = row.GetIdentity();
      if (!((UnityEngine.Object) (identity as MinionIdentity) == (UnityEngine.Object) null) && !((UnityEngine.Object) (identity as MinionIdentity).gameObject != (UnityEngine.Object) minion_resume.gameObject))
      {
        foreach (TableColumn tableColumn in this.columns.Values)
        {
          PrioritizationGroupTableColumn groupTableColumn = tableColumn as PrioritizationGroupTableColumn;
          if (groupTableColumn != null)
            this.UpdateWidget(row.GetWidget((TableColumn) groupTableColumn), groupTableColumn.userData as ChoreGroup, (IPersonalPriorityManager) (identity as MinionIdentity).GetComponent<ChoreConsumer>());
        }
      }
    }
  }

  protected override void RefreshRows()
  {
    base.RefreshRows();
    this.RefreshEffectListeners();
    if (!this.dynamicRowSpacing)
      return;
    this.SizeRows();
  }

  private void SizeRows()
  {
    float num1 = 0.0f;
    int num2 = 0;
    for (int index = 0; index < this.header_row.transform.childCount; ++index)
    {
      Transform child = this.header_row.transform.GetChild(index);
      LayoutElement component1 = child.GetComponent<LayoutElement>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !component1.ignoreLayout)
      {
        ++num2;
        num1 += component1.minWidth;
      }
      else
      {
        HorizontalOrVerticalLayoutGroup component2 = child.GetComponent<HorizontalOrVerticalLayoutGroup>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          float x = component2.rectTransform().sizeDelta.x;
          num1 += x;
          ++num2;
        }
      }
    }
    float width = this.gameObject.rectTransform().rect.width;
    float num3 = 0.0f;
    HorizontalLayoutGroup component = this.header_row.GetComponent<HorizontalLayoutGroup>();
    component.spacing = num3;
    component.childAlignment = TextAnchor.MiddleLeft;
    foreach (KMonoBehaviour row in this.rows)
      row.transform.GetComponentInChildren<HorizontalLayoutGroup>().spacing = num3;
  }

  private void RefreshEffectListeners()
  {
    for (int index = 0; index < this.EffectListeners.Count; ++index)
    {
      this.EffectListeners[index].Key.Unsubscribe(this.EffectListeners[index].Value.level_up);
      this.EffectListeners[index].Key.Unsubscribe(this.EffectListeners[index].Value.effect_added);
      this.EffectListeners[index].Key.Unsubscribe(this.EffectListeners[index].Value.effect_removed);
      this.EffectListeners[index].Key.Unsubscribe(this.EffectListeners[index].Value.disease_added);
      this.EffectListeners[index].Key.Unsubscribe(this.EffectListeners[index].Value.effect_added);
    }
    this.EffectListeners.Clear();
    for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
    {
      JobsTableScreen.SkillEventHandlerID skillEventHandlerId = new JobsTableScreen.SkillEventHandlerID();
      MinionIdentity id = Components.LiveMinionIdentities[index];
      System.Action<object> handler = (System.Action<object>) (o => this.MarkSingleMinionRowDirty(id));
      skillEventHandlerId.level_up = Components.LiveMinionIdentities[index].gameObject.Subscribe(-110704193, handler);
      skillEventHandlerId.effect_added = Components.LiveMinionIdentities[index].gameObject.Subscribe(-1901442097, handler);
      skillEventHandlerId.effect_removed = Components.LiveMinionIdentities[index].gameObject.Subscribe(-1157678353, handler);
      skillEventHandlerId.disease_added = Components.LiveMinionIdentities[index].gameObject.Subscribe(1592732331, handler);
      skillEventHandlerId.disease_cured = Components.LiveMinionIdentities[index].gameObject.Subscribe(77635178, handler);
    }
    for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
    {
      MinionIdentity id = Components.LiveMinionIdentities[index];
      Components.LiveMinionIdentities[index].gameObject.Subscribe(540773776, (System.Action<object>) (new_role => this.MarkSingleMinionRowDirty(id)));
    }
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if (this.dirty_single_minion_rows.Count == 0)
      return;
    foreach (MinionIdentity dirtySingleMinionRow in this.dirty_single_minion_rows)
    {
      if (!((UnityEngine.Object) dirtySingleMinionRow == (UnityEngine.Object) null))
        this.RefreshSingleMinionRow((IAssignableIdentity) dirtySingleMinionRow);
    }
    this.dirty_single_minion_rows.Clear();
  }

  protected void MarkSingleMinionRowDirty(MinionIdentity id)
  {
    this.dirty_single_minion_rows.Add(id);
  }

  private void RefreshSingleMinionRow(IAssignableIdentity id)
  {
    foreach (KeyValuePair<string, TableColumn> column in this.columns)
    {
      if (column.Value != null && column.Value.on_load_action != null)
      {
        foreach (KeyValuePair<TableRow, GameObject> keyValuePair in column.Value.widgets_by_row)
        {
          if (!((UnityEngine.Object) keyValuePair.Value == (UnityEngine.Object) null) && keyValuePair.Key.GetIdentity() == id)
            column.Value.on_load_action(id, keyValuePair.Value);
        }
        column.Value.on_load_action((IAssignableIdentity) null, this.rows[0].GetWidget(column.Value));
      }
    }
  }

  protected override void OnCmpDisable()
  {
    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject((GameObject) null);
    base.OnCmpDisable();
    foreach (TableColumn column in this.columns.Values)
    {
      foreach (TableRow row in this.rows)
      {
        GameObject widget = row.GetWidget(column);
        if (!((UnityEngine.Object) widget == (UnityEngine.Object) null))
        {
          GroupSelectorWidget[] componentsInChildren1 = widget.GetComponentsInChildren<GroupSelectorWidget>();
          if (componentsInChildren1 != null)
          {
            foreach (GroupSelectorWidget groupSelectorWidget in componentsInChildren1)
              groupSelectorWidget.CloseSubPanel();
          }
          GroupSelectorHeaderWidget[] componentsInChildren2 = widget.GetComponentsInChildren<GroupSelectorHeaderWidget>();
          if (componentsInChildren2 != null)
          {
            foreach (GroupSelectorHeaderWidget selectorHeaderWidget in componentsInChildren2)
              selectorHeaderWidget.CloseSubPanel();
          }
          SelectablePanel[] componentsInChildren3 = widget.GetComponentsInChildren<SelectablePanel>();
          if (componentsInChildren3 != null)
          {
            foreach (Component component in componentsInChildren3)
              component.gameObject.SetActive(false);
          }
        }
      }
    }
    this.optionsPanel.gameObject.SetActive(false);
  }

  private void GetMouseHoverInfo(out bool is_hovering_screen, out bool is_hovering_button)
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if ((UnityEngine.Object) current == (UnityEngine.Object) null)
    {
      is_hovering_button = false;
      is_hovering_screen = false;
    }
    else
    {
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      current.RaycastAll(new PointerEventData(current)
      {
        position = (Vector2) KInputManager.GetMousePos()
      }, raycastResults);
      bool flag1 = false;
      bool flag2 = false;
      foreach (RaycastResult raycastResult in raycastResults)
      {
        if ((UnityEngine.Object) raycastResult.gameObject.GetComponent<OptionSelector>() != (UnityEngine.Object) null || (UnityEngine.Object) raycastResult.gameObject.transform.parent != (UnityEngine.Object) null && (UnityEngine.Object) raycastResult.gameObject.transform.parent.GetComponent<OptionSelector>() != (UnityEngine.Object) null)
        {
          flag1 = true;
          flag2 = true;
          break;
        }
        if (this.HasParent(raycastResult.gameObject, this.gameObject))
          flag2 = true;
      }
      is_hovering_screen = flag2;
      is_hovering_button = flag1;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    bool flag = false;
    if (e.IsAction(Action.MouseRight))
    {
      bool is_hovering_screen;
      bool is_hovering_button;
      this.GetMouseHoverInfo(out is_hovering_screen, out is_hovering_button);
      if (is_hovering_screen)
      {
        flag = true;
        if (!e.Consumed)
          e.TryConsume(Action.MouseRight);
      }
    }
    if (flag)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    bool flag = false;
    if (e.IsAction(Action.MouseRight))
    {
      bool is_hovering_screen;
      bool is_hovering_button;
      this.GetMouseHoverInfo(out is_hovering_screen, out is_hovering_button);
      if (is_hovering_screen)
      {
        flag = true;
        if (!is_hovering_button)
          UISounds.PlaySound(UISounds.Sound.Negative);
        if (!e.Consumed)
          e.TryConsume(Action.MouseRight);
      }
    }
    if (flag)
      return;
    base.OnKeyUp(e);
  }

  private bool HasParent(GameObject obj, GameObject parent)
  {
    bool flag = false;
    Transform transform1 = parent.transform;
    for (Transform transform2 = obj.transform; (UnityEngine.Object) transform2 != (UnityEngine.Object) null; transform2 = transform2.parent)
    {
      if ((UnityEngine.Object) transform2 == (UnityEngine.Object) transform1)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private void ConfigureNameLabel(IAssignableIdentity identity, GameObject widget_go)
  {
    this.on_load_name_label(identity, widget_go);
    if (identity == null)
      return;
    string result = string.Empty;
    ToolTip component = widget_go.GetComponent<ToolTip>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.OnToolTip += (Func<string>) (() =>
    {
      MinionIdentity cmp = identity as MinionIdentity;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<b>" + (string) STRINGS.UI.DETAILTABS.STATS.NAME + "</b>");
        foreach (AttributeInstance attribute in cmp.GetAttributes())
        {
          if (attribute.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
          {
            string str = UIConstants.ColorPrefixWhite;
            if ((double) attribute.GetTotalValue() > 0.0)
              str = UIConstants.ColorPrefixGreen;
            else if ((double) attribute.GetTotalValue() < 0.0)
              str = UIConstants.ColorPrefixRed;
            stringBuilder.Append("\n    • " + attribute.Name + ": " + str + (object) attribute.GetTotalValue() + UIConstants.ColorSuffix);
          }
        }
        result = stringBuilder.ToString();
      }
      else if ((UnityEngine.Object) (identity as StoredMinionIdentity) != (UnityEngine.Object) null)
        result = string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) (identity as StoredMinionIdentity).GetStorageReason(), (object) identity.GetProperName());
      return result;
    });
  }

  private void InitializeHeader(ChoreGroup chore_group, GameObject widget_go)
  {
    HierarchyReferences component1 = widget_go.GetComponent<HierarchyReferences>();
    HierarchyReferences reference1 = component1.GetReference("PrioritizationWidget") as HierarchyReferences;
    GameObject items_root = reference1.GetReference("ItemPanel").gameObject;
    if (items_root.transform.childCount > 0)
      return;
    items_root.SetActive(false);
    (component1.GetReference("Label") as LocText).text = chore_group.Name;
    KButton reference2 = component1.GetReference("PrioritizeButton") as KButton;
    Selectable selectable = items_root.GetComponent<Selectable>();
    reference2.onClick += (System.Action) (() =>
    {
      selectable.Select();
      items_root.SetActive(true);
    });
    GameObject gameObject1 = reference1.GetReference("ItemTemplate").gameObject;
    for (int priority = 5; priority >= 0; --priority)
    {
      JobsTableScreen.PriorityInfo priorityInfo = this.GetPriorityInfo(priority);
      if (priorityInfo.name != null)
      {
        GameObject gameObject2 = Util.KInstantiateUI(gameObject1, items_root, true);
        KButton component2 = gameObject2.GetComponent<KButton>();
        HierarchyReferences component3 = gameObject2.GetComponent<HierarchyReferences>();
        KImage reference3 = component3.GetReference("Icon") as KImage;
        LocText reference4 = component3.GetReference("Label") as LocText;
        int new_priority = priority;
        component2.onClick += (System.Action) (() =>
        {
          this.ChangeColumnPriority((object) widget_go, new_priority);
          UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject((GameObject) null);
        });
        reference3.sprite = priorityInfo.sprite;
        reference4.text = (string) priorityInfo.name;
      }
    }
  }

  private void OnSettingsButtonClicked()
  {
    this.optionsPanel.gameObject.SetActive(true);
    this.optionsPanel.GetComponent<Selectable>().Select();
  }

  private void OnResetSettingsClicked()
  {
    if (Game.Instance.advancedPersonalPriorities)
    {
      if ((UnityEngine.Object) Immigration.Instance != (UnityEngine.Object) null)
        Immigration.Instance.ResetPersonalPriorities();
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        if (!((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null))
          Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
      }
    }
    else
    {
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        if (!((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null))
        {
          ChoreConsumer component = minionIdentity.GetComponent<ChoreConsumer>();
          foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
            component.SetPersonalPriority(resource, 3);
        }
      }
    }
    this.MarkRowsDirty();
  }

  private void OnAdvancedModeToggleClicked()
  {
    Game.Instance.advancedPersonalPriorities = !Game.Instance.advancedPersonalPriorities;
    this.toggleAdvancedModeButton.fgImage.gameObject.SetActive(Game.Instance.advancedPersonalPriorities);
  }

  public struct PriorityInfo
  {
    public int priority;
    public Sprite sprite;
    public LocString name;

    public PriorityInfo(int priority, Sprite sprite, LocString name)
    {
      this.priority = priority;
      this.sprite = sprite;
      this.name = name;
    }
  }

  private struct SkillEventHandlerID
  {
    public int level_up;
    public int effect_added;
    public int effect_removed;
    public int disease_added;
    public int disease_cured;
  }
}
