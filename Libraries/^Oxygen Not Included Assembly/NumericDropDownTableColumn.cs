// Decompiled with JetBrains decompiler
// Type: NumericDropDownTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumericDropDownTableColumn : TableColumn
{
  public object userData;
  private NumericDropDownTableColumn.ToolTipCallbacks callbacks;
  private System.Action<GameObject, int> set_value_action;
  private List<TMP_Dropdown.OptionData> options;

  public NumericDropDownTableColumn(
    object user_data,
    List<TMP_Dropdown.OptionData> options,
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    System.Action<GameObject, int> set_value_action,
    Comparison<IAssignableIdentity> sort_comparer,
    NumericDropDownTableColumn.ToolTipCallbacks callbacks,
    Func<bool> revealed = null)
    : base(on_load_action, sort_comparer, callbacks.headerTooltip, callbacks.headerSortTooltip, revealed, false, string.Empty)
  {
    this.userData = user_data;
    this.set_value_action = set_value_action;
    this.options = options;
    this.callbacks = callbacks;
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    return this.GetWidget(parent);
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    return this.GetWidget(parent);
  }

  private GameObject GetWidget(GameObject parent)
  {
    GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.NumericDropDown, parent, true);
    TMP_Dropdown componentInChildren = widget_go.transform.GetComponentInChildren<TMP_Dropdown>();
    componentInChildren.options = this.options;
    componentInChildren.onValueChanged.AddListener((UnityAction<int>) (new_value => this.set_value_action(widget_go, new_value)));
    ToolTip tt = widget_go.transform.GetComponentInChildren<ToolTip>();
    if ((UnityEngine.Object) tt != (UnityEngine.Object) null)
      tt.OnToolTip = (Func<string>) (() => this.GetTooltip(tt));
    return widget_go;
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    GameObject widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.DropDownHeader, parent, true);
    HierarchyReferences component1 = widget_go.GetComponent<HierarchyReferences>();
    Component reference1 = component1.GetReference("Label");
    MultiToggle componentInChildren1 = reference1.GetComponentInChildren<MultiToggle>(true);
    this.column_sort_toggle = componentInChildren1;
    componentInChildren1.onClick += (System.Action) (() =>
    {
      this.screen.SetSortComparison(this.sort_comparer, (TableColumn) this);
      this.screen.SortRows();
    });
    ToolTip tt1 = reference1.GetComponent<ToolTip>();
    tt1.enabled = true;
    tt1.OnToolTip = (Func<string>) (() =>
    {
      this.callbacks.headerTooltip((IAssignableIdentity) null, widget_go, tt1);
      return string.Empty;
    });
    ToolTip tt2 = componentInChildren1.transform.GetComponent<ToolTip>();
    tt2.OnToolTip = (Func<string>) (() =>
    {
      this.callbacks.headerSortTooltip((IAssignableIdentity) null, widget_go, tt2);
      return string.Empty;
    });
    Component reference2 = component1.GetReference("DropDown");
    TMP_Dropdown componentInChildren2 = reference2.GetComponentInChildren<TMP_Dropdown>();
    componentInChildren2.options = this.options;
    componentInChildren2.onValueChanged.AddListener((UnityAction<int>) (new_value => this.set_value_action(widget_go, new_value)));
    ToolTip tt3 = reference2.GetComponent<ToolTip>();
    tt3.OnToolTip = (Func<string>) (() =>
    {
      this.callbacks.headerDropdownTooltip((IAssignableIdentity) null, widget_go, tt3);
      return string.Empty;
    });
    LayoutElement component2 = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
    LayoutElement layoutElement = component2;
    float num1 = 83f;
    component2.minWidth = num1;
    double num2 = (double) num1;
    layoutElement.preferredWidth = (float) num2;
    return widget_go;
  }

  public class ToolTipCallbacks
  {
    public System.Action<IAssignableIdentity, GameObject, ToolTip> headerTooltip;
    public System.Action<IAssignableIdentity, GameObject, ToolTip> headerSortTooltip;
    public System.Action<IAssignableIdentity, GameObject, ToolTip> headerDropdownTooltip;
  }
}
