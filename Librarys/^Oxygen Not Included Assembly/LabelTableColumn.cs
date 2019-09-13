// Decompiled with JetBrains decompiler
// Type: LabelTableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class LabelTableColumn : TableColumn
{
  private int widget_width = 128;
  public Func<IAssignableIdentity, GameObject, string> get_value_action;

  public LabelTableColumn(
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    int widget_width = 128,
    bool should_refresh_columns = false)
    : base(on_load_action, sort_comparison, on_tooltip, on_sort_tooltip, (Func<bool>) null, should_refresh_columns, string.Empty)
  {
    this.get_value_action = get_value_action;
    this.widget_width = widget_width;
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
    LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
    LayoutElement layoutElement = component;
    float widgetWidth = (float) this.widget_width;
    component.minWidth = widgetWidth;
    double num = (double) widgetWidth;
    layoutElement.preferredWidth = (float) num;
    return gameObject;
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
    ToolTip tt = gameObject.GetComponent<ToolTip>();
    tt.OnToolTip = (Func<string>) (() => this.GetTooltip(tt));
    LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
    LayoutElement layoutElement = component;
    float widgetWidth = (float) this.widget_width;
    component.minWidth = widgetWidth;
    double num = (double) widgetWidth;
    layoutElement.preferredWidth = (float) num;
    return gameObject;
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    GameObject widget_go = (GameObject) null;
    widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.LabelHeader, parent, true);
    MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>(true);
    this.column_sort_toggle = componentInChildren;
    componentInChildren.onClick += (System.Action) (() =>
    {
      this.screen.SetSortComparison(this.sort_comparer, (TableColumn) this);
      this.screen.SortRows();
    });
    ToolTip tt = widget_go.GetComponent<ToolTip>();
    tt.OnToolTip = (Func<string>) (() =>
    {
      this.on_tooltip((IAssignableIdentity) null, widget_go, tt);
      return string.Empty;
    });
    tt = widget_go.GetComponentInChildren<MultiToggle>().GetComponent<ToolTip>();
    tt.OnToolTip = (Func<string>) (() =>
    {
      this.on_sort_tooltip((IAssignableIdentity) null, widget_go, tt);
      return string.Empty;
    });
    LayoutElement component = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
    LayoutElement layoutElement = component;
    float widgetWidth = (float) this.widget_width;
    component.minWidth = widgetWidth;
    double num = (double) widgetWidth;
    layoutElement.preferredWidth = (float) num;
    return widget_go;
  }
}
