// Decompiled with JetBrains decompiler
// Type: TableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableScreen : KScreen
{
  protected bool has_default_duplicant_row = true;
  private HandleVector<int>.Handle current_looping_sound = HandleVector<int>.InvalidHandle;
  protected Dictionary<string, TableColumn> columns = new Dictionary<string, TableColumn>();
  public List<TableRow> rows = new List<TableRow>();
  public List<TableRow> sortable_rows = new List<TableRow>();
  public List<string> column_scrollers = new List<string>();
  private Dictionary<GameObject, TableRow> known_widget_rows = new Dictionary<GameObject, TableRow>();
  private Dictionary<GameObject, TableColumn> known_widget_columns = new Dictionary<GameObject, TableColumn>();
  private string cascade_sound_path = GlobalAssets.GetSound("Placers_Unfurl_LP", false);
  protected string title;
  private bool rows_dirty;
  protected Comparison<IAssignableIdentity> active_sort_method;
  protected TableColumn active_sort_column;
  protected bool sort_is_reversed;
  private int active_cascade_coroutine_count;
  private bool incubating;
  public GameObject prefab_row_empty;
  public GameObject prefab_row_header;
  public GameObject prefab_scroller_border;
  public KButton CloseButton;
  [MyCmpGet]
  private VerticalLayoutGroup VLG;
  protected GameObject header_row;
  protected GameObject default_row;
  public LocText title_bar;
  public Transform header_content_transform;
  public Transform scroll_content_transform;
  public Transform scroller_borders_transform;

  protected override void OnActivate()
  {
    base.OnActivate();
    this.title_bar.text = this.title;
    this.ConsumeMouseScroll = true;
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.incubating = true;
    this.transform.rectTransform().localScale = Vector3.zero;
    Components.LiveMinionIdentities.OnAdd += (System.Action<MinionIdentity>) (param => this.MarkRowsDirty());
    Components.LiveMinionIdentities.OnRemove += (System.Action<MinionIdentity>) (param => this.MarkRowsDirty());
  }

  protected override void OnShow(bool show)
  {
    if (!show)
    {
      this.active_cascade_coroutine_count = 0;
      this.StopAllCoroutines();
      this.StopLoopingCascadeSound();
    }
    this.ZeroScrollers();
    base.OnShow(show);
  }

  private void ZeroScrollers()
  {
    if (this.rows.Count <= 0)
      return;
    foreach (string columnScroller in this.column_scrollers)
    {
      foreach (TableRow row in this.rows)
        row.GetScroller(columnScroller).transform.parent.GetComponent<KScrollRect>().horizontalNormalizedPosition = 0.0f;
    }
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if (this.incubating)
    {
      this.ZeroScrollers();
      this.transform.rectTransform().localScale = Vector3.one;
      this.incubating = false;
    }
    if (this.rows_dirty)
      this.RefreshRows();
    foreach (TableRow row in this.rows)
      row.RefreshScrollers();
    foreach (TableColumn tableColumn in this.columns.Values)
    {
      if (tableColumn.isDirty)
      {
        foreach (KeyValuePair<TableRow, GameObject> keyValuePair in tableColumn.widgets_by_row)
        {
          tableColumn.on_load_action(keyValuePair.Key.GetIdentity(), keyValuePair.Value);
          tableColumn.MarkClean();
        }
      }
    }
  }

  protected void MarkRowsDirty()
  {
    this.rows_dirty = true;
  }

  protected virtual void RefreshRows()
  {
    this.ClearRows();
    this.AddRow((IAssignableIdentity) null);
    if (this.has_default_duplicant_row)
      this.AddDefaultRow();
    for (int index = 0; index < Components.LiveMinionIdentities.Count; ++index)
      this.AddRow((IAssignableIdentity) Components.LiveMinionIdentities[index]);
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
          this.AddRow((IAssignableIdentity) info.serializedMinion.Get<StoredMinionIdentity>());
      }
    }
    this.SortRows();
    this.rows_dirty = false;
  }

  public virtual void SetSortComparison(
    Comparison<IAssignableIdentity> comparison,
    TableColumn sort_column)
  {
    if (comparison == null)
      return;
    if (this.active_sort_column == sort_column)
    {
      if (this.sort_is_reversed)
      {
        this.sort_is_reversed = false;
        this.active_sort_method = (Comparison<IAssignableIdentity>) null;
        this.active_sort_column = (TableColumn) null;
      }
      else
        this.sort_is_reversed = true;
    }
    else
    {
      this.active_sort_column = sort_column;
      this.active_sort_method = comparison;
      this.sort_is_reversed = false;
    }
  }

  public void SortRows()
  {
    foreach (TableColumn tableColumn in this.columns.Values)
    {
      if (!((UnityEngine.Object) tableColumn.column_sort_toggle == (UnityEngine.Object) null))
      {
        if (tableColumn == this.active_sort_column)
        {
          if (this.sort_is_reversed)
            tableColumn.column_sort_toggle.ChangeState(2);
          else
            tableColumn.column_sort_toggle.ChangeState(1);
        }
        else
          tableColumn.column_sort_toggle.ChangeState(0);
      }
    }
    if (this.active_sort_method == null)
      return;
    Dictionary<IAssignableIdentity, TableRow> dictionary = new Dictionary<IAssignableIdentity, TableRow>();
    foreach (TableRow sortableRow in this.sortable_rows)
      dictionary.Add(sortableRow.GetIdentity(), sortableRow);
    List<IAssignableIdentity> assignableIdentityList = new List<IAssignableIdentity>();
    foreach (KeyValuePair<IAssignableIdentity, TableRow> keyValuePair in dictionary)
      assignableIdentityList.Add(keyValuePair.Key);
    assignableIdentityList.Sort(this.active_sort_method);
    if (this.sort_is_reversed)
      assignableIdentityList.Reverse();
    this.sortable_rows.Clear();
    for (int index = 0; index < assignableIdentityList.Count; ++index)
      this.sortable_rows.Add(dictionary[assignableIdentityList[index]]);
    for (int index = 0; index < this.sortable_rows.Count; ++index)
      this.sortable_rows[index].gameObject.transform.SetSiblingIndex(index);
    if (!this.has_default_duplicant_row)
      return;
    this.default_row.transform.SetAsFirstSibling();
  }

  protected int compare_rows_alphabetical(IAssignableIdentity a, IAssignableIdentity b)
  {
    if (a == null && b == null)
      return 0;
    if (a == null)
      return -1;
    if (b == null)
      return 1;
    return a.GetProperName().CompareTo(b.GetProperName());
  }

  protected int default_sort(TableRow a, TableRow b)
  {
    return 0;
  }

  protected void ClearRows()
  {
    for (int index = this.rows.Count - 1; index >= 0; --index)
      this.rows[index].Clear();
    this.rows.Clear();
    this.sortable_rows.Clear();
  }

  protected void AddRow(IAssignableIdentity minion)
  {
    bool flag = minion == null;
    GameObject gameObject = Util.KInstantiateUI(!flag ? this.prefab_row_empty : this.prefab_row_header, minion != null ? this.scroll_content_transform.gameObject : this.header_content_transform.gameObject, true);
    TableRow component = gameObject.GetComponent<TableRow>();
    component.rowType = !flag ? (!((UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null) ? TableRow.RowType.StoredMinon : TableRow.RowType.Minion) : TableRow.RowType.Header;
    this.rows.Add(component);
    component.ConfigureContent(minion, this.columns);
    if (!flag)
      this.sortable_rows.Add(component);
    else
      this.header_row = gameObject;
  }

  protected void AddDefaultRow()
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_row_empty, this.scroll_content_transform.gameObject, true);
    this.default_row = gameObject;
    TableRow component = gameObject.GetComponent<TableRow>();
    component.rowType = TableRow.RowType.Default;
    component.isDefault = true;
    this.rows.Add(component);
    component.ConfigureContent((IAssignableIdentity) null, this.columns);
  }

  protected TableRow GetWidgetRow(GameObject widget_go)
  {
    if ((UnityEngine.Object) widget_go == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Widget is null");
      return (TableRow) null;
    }
    if (this.known_widget_rows.ContainsKey(widget_go))
      return this.known_widget_rows[widget_go];
    foreach (TableRow row in this.rows)
    {
      if (row.ContainsWidget(widget_go))
      {
        this.known_widget_rows.Add(widget_go, row);
        return row;
      }
    }
    Debug.LogWarning((object) ("Row is null for widget: " + widget_go.name + " parent is " + widget_go.transform.parent.name));
    return (TableRow) null;
  }

  protected void StartScrollableContent(string scrollablePanelID)
  {
    if (this.column_scrollers.Contains(scrollablePanelID))
      return;
    DividerColumn dividerColumn = new DividerColumn((Func<bool>) (() => true), string.Empty);
    this.RegisterColumn("scroller_spacer_" + scrollablePanelID, (TableColumn) dividerColumn);
    this.column_scrollers.Add(scrollablePanelID);
  }

  protected PortraitTableColumn AddPortraitColumn(
    string id,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Comparison<IAssignableIdentity> sort_comparison,
    bool double_click_to_target = true)
  {
    PortraitTableColumn portraitTableColumn = new PortraitTableColumn(on_load_action, sort_comparison, double_click_to_target);
    if (this.RegisterColumn(id, (TableColumn) portraitTableColumn))
      return portraitTableColumn;
    return (PortraitTableColumn) null;
  }

  protected ButtonLabelColumn AddButtonLabelColumn(
    string id,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    System.Action<GameObject> on_click_action,
    System.Action<GameObject> on_double_click_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    bool whiteText = false)
  {
    ButtonLabelColumn buttonLabelColumn = new ButtonLabelColumn(on_load_action, get_value_action, on_click_action, on_double_click_action, sort_comparison, on_tooltip, on_sort_tooltip, whiteText);
    if (this.RegisterColumn(id, (TableColumn) buttonLabelColumn))
      return buttonLabelColumn;
    return (ButtonLabelColumn) null;
  }

  protected LabelTableColumn AddLabelColumn(
    string id,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    int widget_width = 128,
    bool should_refresh_columns = false)
  {
    LabelTableColumn labelTableColumn = new LabelTableColumn(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, widget_width, should_refresh_columns);
    if (this.RegisterColumn(id, (TableColumn) labelTableColumn))
      return labelTableColumn;
    return (LabelTableColumn) null;
  }

  protected CheckboxTableColumn AddCheckboxColumn(
    string id,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    System.Action<GameObject> on_press_action,
    System.Action<GameObject, TableScreen.ResultValues> set_value_function,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
  {
    CheckboxTableColumn checkboxTableColumn = new CheckboxTableColumn(on_load_action, get_value_action, on_press_action, set_value_function, sort_comparison, on_tooltip, on_sort_tooltip, (Func<bool>) null);
    if (this.RegisterColumn(id, (TableColumn) checkboxTableColumn))
      return checkboxTableColumn;
    return (CheckboxTableColumn) null;
  }

  protected SuperCheckboxTableColumn AddSuperCheckboxColumn(
    string id,
    CheckboxTableColumn[] columns_affected,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    System.Action<GameObject> on_press_action,
    System.Action<GameObject, TableScreen.ResultValues> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip)
  {
    SuperCheckboxTableColumn checkboxTableColumn1 = new SuperCheckboxTableColumn(columns_affected, on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip);
    if (this.RegisterColumn(id, (TableColumn) checkboxTableColumn1))
    {
      foreach (CheckboxTableColumn checkboxTableColumn2 in columns_affected)
        checkboxTableColumn2.on_set_action += new System.Action<GameObject, TableScreen.ResultValues>(((TableColumn) checkboxTableColumn1).MarkDirty);
      checkboxTableColumn1.MarkDirty((GameObject) null, TableScreen.ResultValues.False);
      return checkboxTableColumn1;
    }
    Debug.LogWarning((object) "SuperCheckbox column registration failed");
    return (SuperCheckboxTableColumn) null;
  }

  protected NumericDropDownTableColumn AddNumericDropDownColumn(
    string id,
    object user_data,
    List<TMP_Dropdown.OptionData> options,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    System.Action<GameObject, int> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    NumericDropDownTableColumn.ToolTipCallbacks tooltip_callbacks)
  {
    NumericDropDownTableColumn dropDownTableColumn = new NumericDropDownTableColumn(user_data, options, on_load_action, set_value_action, sort_comparison, tooltip_callbacks, (Func<bool>) null);
    if (this.RegisterColumn(id, (TableColumn) dropDownTableColumn))
      return dropDownTableColumn;
    return (NumericDropDownTableColumn) null;
  }

  protected bool RegisterColumn(string id, TableColumn new_column)
  {
    if (this.columns.ContainsKey(id))
    {
      Debug.LogWarning((object) string.Format("Column with id {0} already in dictionary", (object) id));
      return false;
    }
    new_column.screen = this;
    this.columns.Add(id, new_column);
    this.MarkRowsDirty();
    return true;
  }

  protected TableColumn GetWidgetColumn(GameObject widget_go)
  {
    if (this.known_widget_columns.ContainsKey(widget_go))
      return this.known_widget_columns[widget_go];
    foreach (KeyValuePair<string, TableColumn> column in this.columns)
    {
      if (column.Value.ContainsWidget(widget_go))
      {
        this.known_widget_columns.Add(widget_go, column.Value);
        return column.Value;
      }
    }
    Debug.LogWarning((object) ("No column found for widget gameobject " + widget_go.name));
    return (TableColumn) null;
  }

  protected void on_load_portrait(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    CrewPortrait component = widget_go.GetComponent<CrewPortrait>();
    if (minion != null)
      component.SetIdentityObject(minion, false);
    else
      component.targetImage.enabled = widgetRow.rowType == TableRow.RowType.Default;
  }

  protected void on_load_name_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText locText = (LocText) null;
    HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
    LocText reference = component.GetReference("Label") as LocText;
    if (component.HasReference("SubLabel"))
      locText = component.GetReference("SubLabel") as LocText;
    if (minion != null)
    {
      reference.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
      if (!((UnityEngine.Object) locText != (UnityEngine.Object) null))
        return;
      MinionIdentity minionIdentity = minion as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
        locText.text = minionIdentity.gameObject.GetComponent<MinionResume>().GetSkillsSubtitle();
      else
        locText.text = string.Empty;
      locText.enableWordWrapping = false;
    }
    else
    {
      if (widgetRow.isDefault)
      {
        reference.text = (string) STRINGS.UI.JOBSCREEN_DEFAULT;
        if ((UnityEngine.Object) locText != (UnityEngine.Object) null)
          locText.gameObject.SetActive(false);
      }
      else
        reference.text = (string) STRINGS.UI.JOBSCREEN_EVERYONE;
      if (!((UnityEngine.Object) locText != (UnityEngine.Object) null))
        return;
      locText.text = string.Empty;
    }
  }

  protected string get_value_name_label(IAssignableIdentity minion, GameObject widget_go)
  {
    return minion.GetProperName();
  }

  protected void on_load_value_checkbox_column_super(
    IAssignableIdentity minion,
    GameObject widget_go)
  {
    MultiToggle component = widget_go.GetComponent<MultiToggle>();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
      case TableRow.RowType.Default:
      case TableRow.RowType.Minion:
        component.ChangeState((int) this.get_value_checkbox_column_super(minion, widget_go));
        break;
    }
  }

  public virtual TableScreen.ResultValues get_value_checkbox_column_super(
    IAssignableIdentity minion,
    GameObject widget_go)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    foreach (CheckboxTableColumn checkboxTableColumn in widgetColumn.columns_affected)
    {
      if (checkboxTableColumn.isRevealed)
      {
        switch (checkboxTableColumn.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget((TableColumn) checkboxTableColumn)))
        {
          case TableScreen.ResultValues.False:
            flag2 = false;
            if (!flag1)
            {
              flag5 = true;
              break;
            }
            break;
          case TableScreen.ResultValues.Partial:
            flag4 = true;
            flag5 = true;
            break;
          case TableScreen.ResultValues.True:
            flag4 = true;
            flag1 = false;
            if (!flag2)
            {
              flag5 = true;
              break;
            }
            break;
          case TableScreen.ResultValues.ConditionalGroup:
            flag3 = true;
            flag2 = false;
            flag1 = false;
            break;
        }
        if (!flag5)
          ;
      }
    }
    TableScreen.ResultValues resultValues = TableScreen.ResultValues.Partial;
    if (flag3 && !flag4 && (!flag2 && !flag1))
      resultValues = TableScreen.ResultValues.ConditionalGroup;
    else if (flag2)
      resultValues = TableScreen.ResultValues.True;
    else if (flag1)
      resultValues = TableScreen.ResultValues.False;
    else if (flag4)
      resultValues = TableScreen.ResultValues.Partial;
    return resultValues;
  }

  protected void set_value_checkbox_column_super(
    GameObject widget_go,
    TableScreen.ResultValues new_value)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, this.default_row.GetComponent<TableRow>(), new_value, widget_go));
        this.StartCoroutine(this.CascadeSetColumnCheckBoxes(this.sortable_rows, (CheckboxTableColumn) widgetColumn, new_value, widget_go));
        break;
      case TableRow.RowType.Default:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, widgetRow, new_value, widget_go));
        break;
      case TableRow.RowType.Minion:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, widgetRow, new_value, widget_go));
        break;
    }
  }

  [DebuggerHidden]
  protected IEnumerator CascadeSetRowCheckBoxes(
    CheckboxTableColumn[] checkBoxToggleColumns,
    TableRow row,
    TableScreen.ResultValues state,
    GameObject ignore_widget = null)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new TableScreen.\u003CCascadeSetRowCheckBoxes\u003Ec__Iterator0()
    {
      checkBoxToggleColumns = checkBoxToggleColumns,
      row = row,
      ignore_widget = ignore_widget,
      state = state,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  protected IEnumerator CascadeSetColumnCheckBoxes(
    List<TableRow> rows,
    CheckboxTableColumn checkBoxToggleColumn,
    TableScreen.ResultValues state,
    GameObject header_widget_go = null)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new TableScreen.\u003CCascadeSetColumnCheckBoxes\u003Ec__Iterator1()
    {
      rows = rows,
      checkBoxToggleColumn = checkBoxToggleColumn,
      header_widget_go = header_widget_go,
      state = state,
      \u0024this = this
    };
  }

  private void StopLoopingCascadeSound()
  {
    if (!this.current_looping_sound.IsValid())
      return;
    LoopingSoundManager.StopSound(this.current_looping_sound);
    this.current_looping_sound.Clear();
  }

  protected void on_press_checkbox_column_super(GameObject widget_go)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (this.get_value_checkbox_column_super(widgetRow.GetIdentity(), widget_go))
    {
      case TableScreen.ResultValues.False:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
        break;
      case TableScreen.ResultValues.Partial:
      case TableScreen.ResultValues.ConditionalGroup:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
        break;
      case TableScreen.ResultValues.True:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
        break;
    }
    widgetColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
  }

  protected void on_tooltip_sort_alphabetically(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) STRINGS.UI.TABLESCREENS.COLUMN_SORT_BY_NAME, (ScriptableObject) null);
        break;
    }
  }

  public enum ResultValues
  {
    False,
    Partial,
    True,
    ConditionalGroup,
  }
}
