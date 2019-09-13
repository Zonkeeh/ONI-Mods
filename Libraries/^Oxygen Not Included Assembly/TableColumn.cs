// Decompiled with JetBrains decompiler
// Type: TableColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TableColumn : IRender1000ms
{
  public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();
  public System.Action<IAssignableIdentity, GameObject> on_load_action;
  public System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;
  public System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;
  public Comparison<IAssignableIdentity> sort_comparer;
  public string scrollerID;
  public TableScreen screen;
  public MultiToggle column_sort_toggle;
  private Func<bool> revealed;
  protected bool dirty;

  public TableColumn(
    System.Action<IAssignableIdentity, GameObject> on_load_action,
    Comparison<IAssignableIdentity> sort_comparison,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null,
    System.Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null,
    Func<bool> revealed = null,
    bool should_refresh_columns = false,
    string scrollerID = "")
  {
    this.on_load_action = on_load_action;
    this.sort_comparer = sort_comparison;
    this.on_tooltip = on_tooltip;
    this.on_sort_tooltip = on_sort_tooltip;
    this.revealed = revealed;
    this.scrollerID = scrollerID;
    if (!should_refresh_columns)
      return;
    SimAndRenderScheduler.instance.Add((object) this, false);
  }

  public bool isRevealed
  {
    get
    {
      if (this.revealed != null)
        return this.revealed();
      return true;
    }
  }

  protected string GetTooltip(ToolTip tool_tip_instance)
  {
    GameObject gameObject = tool_tip_instance.gameObject;
    HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasReference("Widget"))
      gameObject = component.GetReference("Widget").gameObject;
    TableRow tableRow = (TableRow) null;
    foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
    {
      if ((UnityEngine.Object) keyValuePair.Value == (UnityEngine.Object) gameObject)
      {
        tableRow = keyValuePair.Key;
        break;
      }
    }
    if ((UnityEngine.Object) tableRow != (UnityEngine.Object) null && this.on_tooltip != null)
      this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
    return string.Empty;
  }

  protected string GetSortTooltip(ToolTip sort_tooltip_instance)
  {
    GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
    TableRow tableRow = (TableRow) null;
    foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
    {
      if ((UnityEngine.Object) keyValuePair.Value == (UnityEngine.Object) gameObject)
      {
        tableRow = keyValuePair.Key;
        break;
      }
    }
    if ((UnityEngine.Object) tableRow != (UnityEngine.Object) null && this.on_sort_tooltip != null)
      this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
    return string.Empty;
  }

  public bool isDirty
  {
    get
    {
      return this.dirty;
    }
  }

  public bool ContainsWidget(GameObject widget)
  {
    return this.widgets_by_row.ContainsValue(widget);
  }

  public virtual GameObject GetMinionWidget(GameObject parent)
  {
    Debug.LogError((object) "Table Column has no Widget prefab");
    return (GameObject) null;
  }

  public virtual GameObject GetHeaderWidget(GameObject parent)
  {
    Debug.LogError((object) "Table Column has no Widget prefab");
    return (GameObject) null;
  }

  public virtual GameObject GetDefaultWidget(GameObject parent)
  {
    Debug.LogError((object) "Table Column has no Widget prefab");
    return (GameObject) null;
  }

  public void Render1000ms(float dt)
  {
    this.MarkDirty((GameObject) null, TableScreen.ResultValues.False);
  }

  public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
  {
    this.dirty = true;
  }

  public void MarkClean()
  {
    this.dirty = false;
  }
}
