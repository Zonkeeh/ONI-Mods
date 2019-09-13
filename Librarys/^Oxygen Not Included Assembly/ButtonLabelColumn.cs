// Decompiled with JetBrains decompiler
// Type: ButtonLabelColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ButtonLabelColumn : LabelTableColumn
{
  private System.Action<GameObject> on_click_action;
  private System.Action<GameObject> on_double_click_action;
  private bool whiteText;

  public ButtonLabelColumn(
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    System.Action<GameObject> on_click_action,
    System.Action<GameObject> on_double_click_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    bool whiteText = false)
    : base(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, 128, false)
  {
    this.on_click_action = on_click_action;
    this.on_double_click_action = on_double_click_action;
    this.whiteText = whiteText;
  }

  public override GameObject GetDefaultWidget(GameObject parent)
  {
    GameObject widget_go = Util.KInstantiateUI(!this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabel : Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite, parent, true);
    if (this.on_click_action != null)
      widget_go.GetComponent<KButton>().onClick += (System.Action) (() => this.on_click_action(widget_go));
    if (this.on_double_click_action != null)
      widget_go.GetComponent<KButton>().onDoubleClick += (System.Action) (() => this.on_double_click_action(widget_go));
    return widget_go;
  }

  public override GameObject GetHeaderWidget(GameObject parent)
  {
    return base.GetHeaderWidget(parent);
  }

  public override GameObject GetMinionWidget(GameObject parent)
  {
    GameObject widget_go = Util.KInstantiateUI(!this.whiteText ? Assets.UIPrefabs.TableScreenWidgets.ButtonLabel : Assets.UIPrefabs.TableScreenWidgets.ButtonLabelWhite, parent, true);
    ToolTip tt = widget_go.GetComponent<ToolTip>();
    tt.OnToolTip = (Func<string>) (() => this.GetTooltip(tt));
    if (this.on_click_action != null)
      widget_go.GetComponent<KButton>().onClick += (System.Action) (() => this.on_click_action(widget_go));
    if (this.on_double_click_action != null)
      widget_go.GetComponent<KButton>().onDoubleClick += (System.Action) (() => this.on_double_click_action(widget_go));
    return widget_go;
  }
}
