// Decompiled with JetBrains decompiler
// Type: TableRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TableRow : KMonoBehaviour
{
  private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();
  private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();
  public TableRow.RowType rowType;
  private IAssignableIdentity minion;
  public bool isDefault;
  public KButton selectMinionButton;
  [SerializeField]
  private ColorStyleSetting style_setting_default;
  [SerializeField]
  private ColorStyleSetting style_setting_minion;
  [SerializeField]
  private GameObject scrollerPrefab;
  [SerializeField]
  private GameObject scrollbarPrefab;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (!((UnityEngine.Object) this.selectMinionButton != (UnityEngine.Object) null))
      return;
    this.selectMinionButton.onClick += new System.Action(this.SelectMinion);
    this.selectMinionButton.onDoubleClick += new System.Action(this.SelectAndFocusMinion);
  }

  public GameObject GetScroller(string scrollerID)
  {
    return this.scrollers[scrollerID];
  }

  public GameObject GetScrollerBorder(string scrolledID)
  {
    return this.scrollerBorders[scrolledID];
  }

  public void SelectMinion()
  {
    MinionIdentity minion = this.minion as MinionIdentity;
    if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      return;
    SelectTool.Instance.Select(minion.GetComponent<KSelectable>(), false);
  }

  public void SelectAndFocusMinion()
  {
    MinionIdentity minion = this.minion as MinionIdentity;
    if ((UnityEngine.Object) minion == (UnityEngine.Object) null)
      return;
    SelectTool.Instance.SelectAndFocus(minion.transform.GetPosition(), minion.GetComponent<KSelectable>(), new Vector3(8f, 0.0f, 0.0f));
  }

  public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns)
  {
    this.minion = minion;
    KImage componentInChildren1 = this.GetComponentInChildren<KImage>(true);
    componentInChildren1.colorStyleSetting = minion != null ? this.style_setting_minion : this.style_setting_default;
    componentInChildren1.ColorState = KImage.ColorSelector.Inactive;
    CanvasGroup component = this.GetComponent<CanvasGroup>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) (minion as StoredMinionIdentity) != (UnityEngine.Object) null)
      component.alpha = 0.6f;
    foreach (KeyValuePair<string, TableColumn> column1 in columns)
    {
      KeyValuePair<string, TableColumn> column = column1;
      GameObject gameObject = minion != null ? column.Value.GetMinionWidget(this.gameObject) : (!this.isDefault ? column.Value.GetHeaderWidget(this.gameObject) : column.Value.GetDefaultWidget(this.gameObject));
      this.widgets.Add(column.Value, gameObject);
      column.Value.widgets_by_row.Add(this, gameObject);
      if (column.Key.Contains("scroller_spacer_") && (minion != null || this.isDefault))
        gameObject.GetComponentInChildren<LayoutElement>().minWidth += 3f;
      if (column.Value.scrollerID != string.Empty)
      {
        foreach (string columnScroller in column.Value.screen.column_scrollers)
        {
          if (columnScroller == column.Value.scrollerID)
          {
            if (!this.scrollers.ContainsKey(columnScroller))
            {
              KScrollRect scroll_rect = Util.KInstantiateUI(this.scrollerPrefab, this.gameObject, true).GetComponent<KScrollRect>();
              scroll_rect.onValueChanged.AddListener((UnityAction<Vector2>) (_param1 =>
              {
                foreach (Component row in column.Value.screen.rows)
                {
                  KScrollRect componentInChildren2 = row.GetComponentInChildren<KScrollRect>();
                  if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
                    componentInChildren2.horizontalNormalizedPosition = scroll_rect.horizontalNormalizedPosition;
                }
              }));
              this.scrollers.Add(columnScroller, scroll_rect.content.gameObject);
              if ((UnityEngine.Object) scroll_rect.content.transform.parent.Find("Border") != (UnityEngine.Object) null)
                this.scrollerBorders.Add(columnScroller, scroll_rect.content.transform.parent.Find("Border").gameObject);
            }
            gameObject.transform.SetParent(this.scrollers[columnScroller].transform);
            this.scrollers[columnScroller].transform.parent.GetComponent<KScrollRect>().horizontalNormalizedPosition = 0.0f;
          }
        }
      }
    }
    foreach (KeyValuePair<string, TableColumn> column in columns)
    {
      if (column.Value.on_load_action != null)
        column.Value.on_load_action(minion, column.Value.widgets_by_row[this]);
    }
    if (minion != null)
      this.gameObject.name = minion.GetProperName();
    else if (this.isDefault)
      this.gameObject.name = "defaultRow";
    if ((bool) ((UnityEngine.Object) this.selectMinionButton))
      this.selectMinionButton.transform.SetAsLastSibling();
    foreach (KeyValuePair<string, GameObject> scrollerBorder in this.scrollerBorders)
    {
      RectTransform transform = scrollerBorder.Value.rectTransform();
      float width = transform.rect.width;
      scrollerBorder.Value.transform.SetParent(this.gameObject.transform);
      RectTransform rectTransform1 = transform;
      Vector2 vector2_1 = new Vector2(0.0f, 1f);
      transform.anchorMax = vector2_1;
      Vector2 vector2_2 = vector2_1;
      rectTransform1.anchorMin = vector2_2;
      transform.sizeDelta = new Vector2(width, transform.sizeDelta.y);
      RectTransform rectTransform2 = this.scrollers[scrollerBorder.Key].transform.parent.rectTransform();
      Vector3 vector3 = this.scrollers[scrollerBorder.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, (float) (-1.0 * ((double) rectTransform2.sizeDelta.y / 2.0)), 0.0f);
      vector3.y = 0.0f;
      transform.sizeDelta = new Vector2(transform.sizeDelta.x, 374f);
      transform.SetLocalPosition(vector3 + Vector3.up * transform.GetLocalPosition().y + Vector3.up * -transform.anchoredPosition.y);
    }
  }

  public void RefreshScrollers()
  {
    foreach (KeyValuePair<string, GameObject> scroller in this.scrollers)
    {
      KScrollRect component = scroller.Value.transform.parent.GetComponent<KScrollRect>();
      component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
    }
    foreach (KeyValuePair<string, GameObject> scrollerBorder in this.scrollerBorders)
    {
      RectTransform rectTransform = scrollerBorder.Value.rectTransform();
      rectTransform.sizeDelta = new Vector2(this.scrollers[scrollerBorder.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
    }
  }

  public GameObject GetWidget(TableColumn column)
  {
    if (this.widgets.ContainsKey(column) && (UnityEngine.Object) this.widgets[column] != (UnityEngine.Object) null)
      return this.widgets[column];
    Debug.LogWarning((object) ("Widget is null or row does not contain widget for column " + (object) column));
    return (GameObject) null;
  }

  public IAssignableIdentity GetIdentity()
  {
    return this.minion;
  }

  public bool ContainsWidget(GameObject widget)
  {
    return this.widgets.ContainsValue(widget);
  }

  public void Clear()
  {
    foreach (KeyValuePair<TableColumn, GameObject> widget in this.widgets)
      widget.Key.widgets_by_row.Remove(this);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum RowType
  {
    Header,
    Default,
    Minion,
    StoredMinon,
  }
}
