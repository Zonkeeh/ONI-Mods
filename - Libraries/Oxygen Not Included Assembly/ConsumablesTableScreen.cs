// Decompiled with JetBrains decompiler
// Type: ConsumablesTableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumablesTableScreen : TableScreen
{
  private const int CONSUMABLE_COLUMNS_BEFORE_SCROLL = 12;

  protected override void OnActivate()
  {
    this.title = (string) STRINGS.UI.CONSUMABLESSCREEN.TITLE;
    base.OnActivate();
    this.AddPortraitColumn("Portrait", new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_portrait), (Comparison<IAssignableIdentity>) null, true);
    this.AddButtonLabelColumn("Names", new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(((TableScreen) this).get_value_name_label), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectMinion()), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectAndFocusMinion()), new Comparison<IAssignableIdentity>(((TableScreen) this).compare_rows_alphabetical), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new System.Action<IAssignableIdentity, GameObject, ToolTip>(((TableScreen) this).on_tooltip_sort_alphabetically), false);
    this.AddLabelColumn("QOLExpectations", new System.Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 96, true);
    List<IConsumableUIItem> consumableUiItemList = new List<IConsumableUIItem>();
    for (int index = 0; index < TUNING.FOOD.FOOD_TYPES_LIST.Count; ++index)
      consumableUiItemList.Add((IConsumableUIItem) TUNING.FOOD.FOOD_TYPES_LIST[index]);
    List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(GameTags.Medicine);
    for (int index = 0; index < prefabsWithTag.Count; ++index)
    {
      MedicinalPill component = prefabsWithTag[index].GetComponent<MedicinalPill>();
      if ((bool) ((UnityEngine.Object) component))
        consumableUiItemList.Add((IConsumableUIItem) component);
      else
        DebugUtil.DevLogErrorFormat("Prefab tagged Medicine does not have MedicinalPill component: {0}", (object) prefabsWithTag[index]);
    }
    consumableUiItemList.Sort((Comparison<IConsumableUIItem>) ((a, b) =>
    {
      int num = a.MajorOrder.CompareTo(b.MajorOrder);
      if (num == 0)
        num = a.MinorOrder.CompareTo(b.MinorOrder);
      return num;
    }));
    ConsumerManager.instance.OnDiscover += new System.Action<Tag>(this.OnConsumableDiscovered);
    List<ConsumableInfoTableColumn> consumableInfoTableColumnList1 = new List<ConsumableInfoTableColumn>();
    List<DividerColumn> dividerColumnList = new List<DividerColumn>();
    List<ConsumableInfoTableColumn> consumableInfoTableColumnList2 = new List<ConsumableInfoTableColumn>();
    this.StartScrollableContent("consumableScroller");
    int num1 = 0;
    for (int index = 0; index < consumableUiItemList.Count; ++index)
    {
      if (consumableUiItemList[index].Display)
      {
        if (consumableUiItemList[index].MajorOrder != num1 && index != 0)
        {
          string id = "QualityDivider_" + (object) consumableUiItemList[index].MajorOrder;
          ConsumableInfoTableColumn[] quality_group_columns = consumableInfoTableColumnList2.ToArray();
          DividerColumn dividerColumn = new DividerColumn((Func<bool>) (() =>
          {
            if (quality_group_columns == null || quality_group_columns.Length == 0)
              return true;
            foreach (TableColumn tableColumn in quality_group_columns)
            {
              if (tableColumn.isRevealed)
                return true;
            }
            return false;
          }), "consumableScroller");
          dividerColumnList.Add(dividerColumn);
          this.RegisterColumn(id, (TableColumn) dividerColumn);
          consumableInfoTableColumnList2.Clear();
        }
        ConsumableInfoTableColumn consumableInfoTableColumn = this.AddConsumableInfoColumn(consumableUiItemList[index].ConsumableId, consumableUiItemList[index], new System.Action<IAssignableIdentity, GameObject>(this.on_load_consumable_info), new Func<IAssignableIdentity, GameObject, TableScreen.ResultValues>(this.get_value_consumable_info), new System.Action<GameObject>(this.on_click_consumable_info), new System.Action<GameObject, TableScreen.ResultValues>(this.set_value_consumable_info), new Comparison<IAssignableIdentity>(this.compare_consumable_info), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_consumable_info), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_consumable_info));
        consumableInfoTableColumnList1.Add(consumableInfoTableColumn);
        num1 = consumableUiItemList[index].MajorOrder;
        consumableInfoTableColumnList2.Add(consumableInfoTableColumn);
      }
    }
    this.AddSuperCheckboxColumn("SuperCheckConsumable", (CheckboxTableColumn[]) consumableInfoTableColumnList1.ToArray(), new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_value_checkbox_column_super), new Func<IAssignableIdentity, GameObject, TableScreen.ResultValues>(((TableScreen) this).get_value_checkbox_column_super), new System.Action<GameObject>(((TableScreen) this).on_press_checkbox_column_super), new System.Action<GameObject, TableScreen.ResultValues>(((TableScreen) this).set_value_checkbox_column_super), (Comparison<IAssignableIdentity>) null, new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_consumable_info_super));
  }

  private void refresh_scrollers()
  {
    int num = 0;
    foreach (EdiblesManager.FoodInfo foodTypes in TUNING.FOOD.FOOD_TYPES_LIST)
    {
      if (DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(foodTypes.ConsumableId.ToTag()))
        ++num;
    }
    foreach (TableRow row in this.rows)
    {
      GameObject scroller = row.GetScroller("consumableScroller");
      if ((UnityEngine.Object) scroller != (UnityEngine.Object) null)
      {
        KScrollRect component = scroller.transform.parent.GetComponent<KScrollRect>();
        if ((UnityEngine.Object) component.horizontalScrollbar != (UnityEngine.Object) null)
        {
          component.horizontalScrollbar.gameObject.SetActive(num >= 12);
          row.GetScrollerBorder("consumableScroller").gameObject.SetActive(num >= 12);
        }
        component.horizontal = num >= 12;
        component.enabled = num >= 12;
      }
    }
  }

  private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = !widgetRow.isDefault ? STRINGS.UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString() : string.Empty;
  }

  private string get_value_qualityoflife_label(IAssignableIdentity minion, GameObject widget_go)
  {
    string str = string.Empty;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
      str = Db.Get().Attributes.QualityOfLife.Lookup((Component) (minion as MinionIdentity)).GetFormattedValue();
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      str = (string) STRINGS.UI.TABLESCREENS.NA;
    return str;
  }

  private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minionIdentity1 = a as MinionIdentity;
    MinionIdentity minionIdentity2 = b as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 1;
    return Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) minionIdentity1).GetTotalValue().CompareTo(Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) minionIdentity2).GetTotalValue());
  }

  protected void on_tooltip_qualityoflife_expectations(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = minion as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup((Component) minionIdentity).GetAttributeValueTooltip(), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_qualityoflife_expectations(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) STRINGS.UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, (ScriptableObject) null);
        break;
    }
  }

  private TableScreen.ResultValues get_value_food_info_super(
    MinionIdentity minion,
    GameObject widget_go)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = false;
    bool flag4 = false;
    foreach (CheckboxTableColumn checkboxTableColumn in widgetColumn.columns_affected)
    {
      switch (checkboxTableColumn.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget((TableColumn) checkboxTableColumn)))
      {
        case TableScreen.ResultValues.False:
          flag2 = false;
          if (!flag1)
          {
            flag4 = true;
            break;
          }
          break;
        case TableScreen.ResultValues.Partial:
          flag3 = true;
          flag4 = true;
          break;
        case TableScreen.ResultValues.True:
          flag1 = false;
          if (!flag2)
          {
            flag4 = true;
            break;
          }
          break;
      }
      if (flag4)
        break;
    }
    if (flag3)
      return TableScreen.ResultValues.Partial;
    if (flag2)
      return TableScreen.ResultValues.True;
    return flag1 ? TableScreen.ResultValues.False : TableScreen.ResultValues.Partial;
  }

  private void set_value_consumable_info(GameObject widget_go, TableScreen.ResultValues new_value)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if ((UnityEngine.Object) widgetRow == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Row is null");
    }
    else
    {
      ConsumableInfoTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
      IAssignableIdentity identity = widgetRow.GetIdentity();
      IConsumableUIItem consumableInfo = widgetColumn.consumable_info;
      switch (widgetRow.rowType)
      {
        case TableRow.RowType.Header:
          this.set_value_consumable_info(this.default_row.GetComponent<TableRow>().GetWidget((TableColumn) widgetColumn), new_value);
          this.StartCoroutine(this.CascadeSetColumnCheckBoxes(this.sortable_rows, (CheckboxTableColumn) widgetColumn, new_value, widget_go));
          break;
        case TableRow.RowType.Default:
          if (new_value == TableScreen.ResultValues.True)
            ConsumerManager.instance.DefaultForbiddenTagsList.Remove(consumableInfo.ConsumableId.ToTag());
          else
            ConsumerManager.instance.DefaultForbiddenTagsList.Add(consumableInfo.ConsumableId.ToTag());
          widgetColumn.on_load_action(identity, widget_go);
          using (Dictionary<TableRow, GameObject>.Enumerator enumerator = widgetColumn.widgets_by_row.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<TableRow, GameObject> current = enumerator.Current;
              if (current.Key.rowType == TableRow.RowType.Header)
              {
                widgetColumn.on_load_action((IAssignableIdentity) null, current.Value);
                break;
              }
            }
            break;
          }
        case TableRow.RowType.Minion:
          MinionIdentity minionIdentity = identity as MinionIdentity;
          if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
            break;
          ConsumableConsumer component = minionIdentity.GetComponent<ConsumableConsumer>();
          if ((UnityEngine.Object) component == (UnityEngine.Object) null)
          {
            Debug.LogError((object) "Could not find minion identity / row associated with the widget");
            break;
          }
          switch (new_value)
          {
            case TableScreen.ResultValues.False:
            case TableScreen.ResultValues.Partial:
              component.SetPermitted(consumableInfo.ConsumableId, false);
              break;
            case TableScreen.ResultValues.True:
            case TableScreen.ResultValues.ConditionalGroup:
              component.SetPermitted(consumableInfo.ConsumableId, true);
              break;
          }
          widgetColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
          using (Dictionary<TableRow, GameObject>.Enumerator enumerator = widgetColumn.widgets_by_row.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<TableRow, GameObject> current = enumerator.Current;
              if (current.Key.rowType == TableRow.RowType.Header)
              {
                widgetColumn.on_load_action((IAssignableIdentity) null, current.Value);
                break;
              }
            }
            break;
          }
      }
    }
  }

  private void on_click_consumable_info(GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    IAssignableIdentity identity = widgetRow.GetIdentity();
    ConsumableInfoTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        switch (this.get_value_consumable_info((IAssignableIdentity) null, widget_go))
        {
          case TableScreen.ResultValues.False:
          case TableScreen.ResultValues.Partial:
          case TableScreen.ResultValues.ConditionalGroup:
            widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
            break;
          case TableScreen.ResultValues.True:
            widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
            break;
        }
        widgetColumn.on_load_action((IAssignableIdentity) null, widget_go);
        break;
      case TableRow.RowType.Default:
        IConsumableUIItem consumableInfo1 = widgetColumn.consumable_info;
        bool flag1 = !ConsumerManager.instance.DefaultForbiddenTagsList.Contains(consumableInfo1.ConsumableId.ToTag());
        widgetColumn.on_set_action(widget_go, !flag1 ? TableScreen.ResultValues.True : TableScreen.ResultValues.False);
        break;
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = identity as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        IConsumableUIItem consumableInfo2 = widgetColumn.consumable_info;
        ConsumableConsumer component = minionIdentity.GetComponent<ConsumableConsumer>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "Could not find minion identity / row associated with the widget");
          break;
        }
        bool flag2 = component.IsPermitted(consumableInfo2.ConsumableId);
        widgetColumn.on_set_action(widget_go, !flag2 ? TableScreen.ResultValues.True : TableScreen.ResultValues.False);
        break;
      case TableRow.RowType.StoredMinon:
        StoredMinionIdentity storedMinionIdentity = identity as StoredMinionIdentity;
        if (!((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null))
          break;
        IConsumableUIItem consumableInfo3 = widgetColumn.consumable_info;
        bool consume = storedMinionIdentity.IsPermittedToConsume(consumableInfo3.ConsumableId);
        widgetColumn.on_set_action(widget_go, !consume ? TableScreen.ResultValues.True : TableScreen.ResultValues.False);
        break;
    }
  }

  private void on_tooltip_consumable_info(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    ConsumableInfoTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    EdiblesManager.FoodInfo consumableInfo = widgetColumn.consumable_info as EdiblesManager.FoodInfo;
    int num = 0;
    if (consumableInfo != null)
    {
      int quality = consumableInfo.Quality;
      MinionIdentity cmp = minion as MinionIdentity;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        AttributeInstance attributeInstance = cmp.GetAttributes().Get(Db.Get().Attributes.FoodExpectation);
        quality += Mathf.RoundToInt(attributeInstance.GetTotalValue());
      }
      foreach (AttributeModifier selfModifier in Db.Get().effects.Get(Edible.GetEffectForFoodQuality(quality)).SelfModifiers)
      {
        if (selfModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
          num += Mathf.RoundToInt(selfModifier.Value);
      }
    }
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip(widgetColumn.consumable_info.ConsumableName, (ScriptableObject) null);
        if (consumableInfo != null)
        {
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_AVAILABLE, (object) GameUtil.GetFormattedCalories(WorldInventory.Instance.GetAmount(widgetColumn.consumable_info.ConsumableId.ToTag()) * consumableInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), (ScriptableObject) null);
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_QUALITY, (object) GameUtil.AddPositiveSign(num.ToString(), num > 0)), (ScriptableObject) null);
          tooltip.AddMultiStringTooltip("\n" + consumableInfo.Description, (ScriptableObject) null);
          break;
        }
        tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_AVAILABLE, (object) GameUtil.GetFormattedUnits(WorldInventory.Instance.GetAmount(widgetColumn.consumable_info.ConsumableId.ToTag()), GameUtil.TimeSlice.None, true)), (ScriptableObject) null);
        break;
      case TableRow.RowType.Default:
        if (widgetColumn.get_value_action(minion, widget_go) == TableScreen.ResultValues.True)
        {
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.NEW_MINIONS_FOOD_PERMISSION_ON, (object) widgetColumn.consumable_info.ConsumableName), (ScriptableObject) null);
          break;
        }
        tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.NEW_MINIONS_FOOD_PERMISSION_OFF, (object) widgetColumn.consumable_info.ConsumableName), (ScriptableObject) null);
        break;
      case TableRow.RowType.Minion:
      case TableRow.RowType.StoredMinon:
        if (minion == null)
          break;
        if (widgetColumn.get_value_action(minion, widget_go) == TableScreen.ResultValues.True)
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_PERMISSION_ON, (object) minion.GetProperName(), (object) widgetColumn.consumable_info.ConsumableName), (ScriptableObject) null);
        else
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_PERMISSION_OFF, (object) minion.GetProperName(), (object) widgetColumn.consumable_info.ConsumableName), (ScriptableObject) null);
        if (consumableInfo != null && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
        {
          tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.FOOD_QUALITY_VS_EXPECTATION, (object) GameUtil.AddPositiveSign(num.ToString(), num > 0), (object) minion.GetProperName()), (ScriptableObject) null);
          break;
        }
        if (!((UnityEngine.Object) (minion as StoredMinionIdentity) != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.CONSUMABLESSCREEN.CANNOT_ADJUST_PERMISSIONS, (object) (minion as StoredMinionIdentity).GetStorageReason()), (ScriptableObject) null);
        break;
    }
  }

  private void on_tooltip_sort_consumable_info(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
  }

  private void on_tooltip_consumable_info_super(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip(STRINGS.UI.CONSUMABLESSCREEN.TOOLTIP_TOGGLE_ALL.text, (ScriptableObject) null);
        break;
      case TableRow.RowType.Default:
        tooltip.AddMultiStringTooltip((string) STRINGS.UI.CONSUMABLESSCREEN.NEW_MINIONS_TOOLTIP_TOGGLE_ROW, (ScriptableObject) null);
        break;
      case TableRow.RowType.Minion:
        if (!((UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(string.Format(STRINGS.UI.CONSUMABLESSCREEN.TOOLTIP_TOGGLE_ROW.text, (object) (minion as MinionIdentity).gameObject.GetProperName()), (ScriptableObject) null);
        break;
    }
  }

  private void on_load_consumable_info(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    TableColumn widgetColumn = this.GetWidgetColumn(widget_go);
    IConsumableUIItem consumableInfo = (widgetColumn as ConsumableInfoTableColumn).consumable_info;
    EdiblesManager.FoodInfo foodInfo = consumableInfo as EdiblesManager.FoodInfo;
    MultiToggle component1 = widget_go.GetComponent<MultiToggle>();
    if (!widgetColumn.isRevealed)
    {
      widget_go.SetActive(false);
    }
    else
    {
      if (!widget_go.activeSelf)
        widget_go.SetActive(true);
      switch (widgetRow.rowType)
      {
        case TableRow.RowType.Header:
          GameObject prefab = Assets.GetPrefab(consumableInfo.ConsumableId.ToTag());
          if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
            return;
          KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
          Image reference = widget_go.GetComponent<HierarchyReferences>().GetReference("PortraitImage") as Image;
          if (component2.AnimFiles.Length > 0)
          {
            Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component2.AnimFiles[0], "ui", false, string.Empty);
            reference.sprite = fromMultiObjectAnim;
          }
          reference.color = Color.white;
          reference.material = (double) WorldInventory.Instance.GetAmount(consumableInfo.ConsumableId.ToTag()) <= 0.0 ? Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial;
          break;
        case TableRow.RowType.Default:
          switch (this.get_value_consumable_info(minion, widget_go))
          {
            case TableScreen.ResultValues.False:
              component1.ChangeState(0);
              break;
            case TableScreen.ResultValues.True:
              component1.ChangeState(1);
              break;
            case TableScreen.ResultValues.ConditionalGroup:
              component1.ChangeState(2);
              break;
          }
        case TableRow.RowType.Minion:
        case TableRow.RowType.StoredMinon:
          switch (this.get_value_consumable_info(minion, widget_go))
          {
            case TableScreen.ResultValues.False:
              component1.ChangeState(0);
              break;
            case TableScreen.ResultValues.True:
              component1.ChangeState(1);
              break;
            case TableScreen.ResultValues.ConditionalGroup:
              component1.ChangeState(2);
              break;
          }
          if (foodInfo != null && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
          {
            (widget_go.GetComponent<HierarchyReferences>().GetReference("BGImage") as Image).color = new Color(0.7215686f, 0.4431373f, 0.5803922f, Mathf.Max((float) ((double) foodInfo.Quality - (double) Db.Get().Attributes.FoodExpectation.Lookup((Component) (minion as MinionIdentity)).GetTotalValue() + 1.0), 0.0f) * 0.25f);
            break;
          }
          break;
      }
      this.refresh_scrollers();
    }
  }

  private int compare_consumable_info(IAssignableIdentity a, IAssignableIdentity b)
  {
    return 0;
  }

  private TableScreen.ResultValues get_value_consumable_info(
    IAssignableIdentity minion,
    GameObject widget_go)
  {
    ConsumableInfoTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as ConsumableInfoTableColumn;
    IConsumableUIItem consumableInfo = widgetColumn.consumable_info;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    TableScreen.ResultValues resultValues = TableScreen.ResultValues.Partial;
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        bool flag1 = true;
        bool flag2 = true;
        bool flag3 = false;
        bool flag4 = false;
        foreach (KeyValuePair<TableRow, GameObject> keyValuePair in widgetColumn.widgets_by_row)
        {
          GameObject gameObject = keyValuePair.Value;
          if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) widget_go) && !((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
          {
            switch (widgetColumn.get_value_action(keyValuePair.Key.GetIdentity(), gameObject))
            {
              case TableScreen.ResultValues.False:
                flag2 = false;
                if (!flag1)
                {
                  flag4 = true;
                  break;
                }
                break;
              case TableScreen.ResultValues.Partial:
                flag3 = true;
                flag4 = true;
                break;
              case TableScreen.ResultValues.True:
                flag1 = false;
                if (!flag2)
                {
                  flag4 = true;
                  break;
                }
                break;
            }
            if (flag4)
              break;
          }
        }
        resultValues = !flag3 ? (!flag2 ? (!flag1 ? TableScreen.ResultValues.Partial : TableScreen.ResultValues.False) : TableScreen.ResultValues.True) : TableScreen.ResultValues.Partial;
        break;
      case TableRow.RowType.Default:
        resultValues = !ConsumerManager.instance.DefaultForbiddenTagsList.Contains(consumableInfo.ConsumableId.ToTag()) ? TableScreen.ResultValues.True : TableScreen.ResultValues.False;
        break;
      case TableRow.RowType.Minion:
        resultValues = !((UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null) ? TableScreen.ResultValues.True : (!((Component) minion).GetComponent<ConsumableConsumer>().IsPermitted(consumableInfo.ConsumableId) ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
        break;
      case TableRow.RowType.StoredMinon:
        resultValues = !((UnityEngine.Object) (minion as StoredMinionIdentity) != (UnityEngine.Object) null) ? TableScreen.ResultValues.True : (!((StoredMinionIdentity) minion).IsPermittedToConsume(consumableInfo.ConsumableId) ? TableScreen.ResultValues.False : TableScreen.ResultValues.True);
        break;
    }
    return resultValues;
  }

  protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        if (minion == null)
          break;
        tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, (object) minion.GetProperName()), (ScriptableObject) null);
        break;
    }
  }

  protected ConsumableInfoTableColumn AddConsumableInfoColumn(
    string id,
    IConsumableUIItem consumable_info,
    System.Action<IAssignableIdentity, GameObject> load_value_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    System.Action<GameObject> on_press_action,
    System.Action<GameObject, TableScreen.ResultValues> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
  {
    ConsumableInfoTableColumn consumableInfoTableColumn = new ConsumableInfoTableColumn(consumable_info, load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, (Func<GameObject, string>) (widget_go => string.Empty));
    consumableInfoTableColumn.scrollerID = "consumableScroller";
    if (this.RegisterColumn(id, (TableColumn) consumableInfoTableColumn))
      return consumableInfoTableColumn;
    return (ConsumableInfoTableColumn) null;
  }

  private void OnConsumableDiscovered(Tag tag)
  {
    this.MarkRowsDirty();
  }

  private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
  {
    StoredMinionIdentity storedMinionIdentity = minion as StoredMinionIdentity;
    if (!((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null))
      return;
    tooltip.AddMultiStringTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) storedMinionIdentity.GetStorageReason(), (object) storedMinionIdentity.GetProperName()), (ScriptableObject) null);
  }
}
