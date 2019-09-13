// Decompiled with JetBrains decompiler
// Type: GroupSelectorHeaderWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupSelectorHeaderWidget : MonoBehaviour
{
  [SerializeField]
  private int numExpectedPanelColumns = 3;
  public LocText label;
  [SerializeField]
  private GameObject itemTemplate;
  [SerializeField]
  private RectTransform itemsPanel;
  [SerializeField]
  private KButton addItemButton;
  [SerializeField]
  private KButton removeItemButton;
  [SerializeField]
  private KButton sortButton;
  private object widgetID;
  private GroupSelectorHeaderWidget.ItemCallbacks itemCallbacks;
  private IList<GroupSelectorWidget.ItemData> options;

  public void Initialize(
    object widget_id,
    IList<GroupSelectorWidget.ItemData> options,
    GroupSelectorHeaderWidget.ItemCallbacks item_callbacks)
  {
    this.widgetID = widget_id;
    this.options = options;
    this.itemCallbacks = item_callbacks;
    if (this.itemCallbacks.getTitleHoverText != null)
      this.label.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => this.itemCallbacks.getTitleHoverText(widget_id));
    bool adding_item1 = true;
    this.addItemButton.onClick += (System.Action) (() => this.RebuildSubPanel(this.addItemButton.transform.GetPosition(), (Func<object, IList<int>>) (widget_go => this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item1)), this.itemCallbacks.onItemAdded, (Func<object, object, string>) ((widget_go, item_data) => this.itemCallbacks.getItemHoverText(widget_go, adding_item1, item_data))));
    bool adding_item2 = false;
    this.removeItemButton.onClick += (System.Action) (() => this.RebuildSubPanel(this.removeItemButton.transform.GetPosition(), (Func<object, IList<int>>) (widget_go => this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item2)), this.itemCallbacks.onItemRemoved, (Func<object, object, string>) ((widget_go, item_data) => this.itemCallbacks.getItemHoverText(widget_go, adding_item2, item_data))));
    this.sortButton.onClick += (System.Action) (() => this.RebuildSubPanel(this.sortButton.transform.GetPosition(), this.itemCallbacks.getValidSortOptionIndices, (System.Action<object>) (item_data => this.itemCallbacks.onSort(this.widgetID, item_data)), (Func<object, object, string>) ((widget_go, item_data) => this.itemCallbacks.getSortHoverText(item_data))));
    if (this.itemCallbacks.getTitleButtonHoverText == null)
      return;
    this.addItemButton.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => this.itemCallbacks.getTitleButtonHoverText(widget_id, true));
    this.removeItemButton.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => this.itemCallbacks.getTitleButtonHoverText(widget_id, false));
  }

  private void RebuildSubPanel(
    Vector3 pos,
    Func<object, IList<int>> display_list_query,
    System.Action<object> on_item_selected,
    Func<object, object, string> get_item_hover_text)
  {
    this.itemsPanel.gameObject.transform.SetPosition(pos + new Vector3(2f, 2f, 0.0f));
    IList<int> intList = display_list_query(this.widgetID);
    if (intList.Count > 0)
    {
      this.ClearSubPanelOptions();
      foreach (int num in (IEnumerable<int>) intList)
      {
        int idx = num;
        GroupSelectorWidget.ItemData option = this.options[idx];
        GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, this.itemsPanel.gameObject, true);
        KButton component = gameObject.GetComponent<KButton>();
        component.fgImage.sprite = this.options[idx].sprite;
        component.onClick += (System.Action) (() =>
        {
          on_item_selected(this.options[idx].userData);
          this.RebuildSubPanel(pos, display_list_query, on_item_selected, get_item_hover_text);
        });
        if (get_item_hover_text != null)
          gameObject.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => get_item_hover_text(this.widgetID, this.options[idx].userData));
      }
      this.itemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.itemsPanel.childCount);
      this.itemsPanel.gameObject.SetActive(true);
      this.itemsPanel.GetComponent<Selectable>().Select();
    }
    else
      this.CloseSubPanel();
  }

  public void CloseSubPanel()
  {
    this.ClearSubPanelOptions();
    this.itemsPanel.gameObject.SetActive(false);
  }

  private void ClearSubPanelOptions()
  {
    IEnumerator enumerator = this.itemsPanel.transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
        Util.KDestroyGameObject(((Component) enumerator.Current).gameObject);
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public struct ItemCallbacks
  {
    public Func<object, string> getTitleHoverText;
    public Func<object, bool, string> getTitleButtonHoverText;
    public Func<object, bool, IList<int>> getHeaderButtonOptions;
    public System.Action<object> onItemAdded;
    public System.Action<object> onItemRemoved;
    public Func<object, bool, object, string> getItemHoverText;
    public Func<object, IList<int>> getValidSortOptionIndices;
    public Func<object, string> getSortHoverText;
    public System.Action<object, object> onSort;
  }
}
