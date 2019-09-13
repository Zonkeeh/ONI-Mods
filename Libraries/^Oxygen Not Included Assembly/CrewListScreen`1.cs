// Decompiled with JetBrains decompiler
// Type: CrewListScreen`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry
{
  public List<EntryType> EntryObjects = new List<EntryType>();
  protected Vector2 EntryRectSize = new Vector2(750f, 64f);
  public int maxEntriesBeforeScroll = 5;
  public GameObject Prefab_CrewEntry;
  public Transform ScrollRectTransform;
  public Transform EntriesPanelTransform;
  public Scrollbar PanelScrollbar;
  protected ToggleGroup sortToggleGroup;
  protected Toggle lastSortToggle;
  protected bool lastSortReversed;
  public GameObject Prefab_ColumnTitle;
  public Transform ColumnTitlesContainer;
  public bool autoColumn;
  public float columnTitleHorizontalOffset;

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ClearEntries();
    this.SpawnEntries();
    this.PositionColumnTitles();
    if (this.autoColumn)
      this.UpdateColumnTitles();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnCmpEnable()
  {
    if (this.autoColumn)
      this.UpdateColumnTitles();
    this.Reconstruct();
  }

  private void ClearEntries()
  {
    for (int index = this.EntryObjects.Count - 1; index > -1; --index)
      Util.KDestroyGameObject((Component) this.EntryObjects[index]);
    this.EntryObjects.Clear();
  }

  protected void RefreshCrewPortraitContent()
  {
    this.EntryObjects.ForEach((System.Action<EntryType>) (eo => eo.RefreshCrewPortraitContent()));
  }

  protected virtual void SpawnEntries()
  {
    if (this.EntryObjects.Count == 0)
      return;
    this.ClearEntries();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if (this.autoColumn)
      this.UpdateColumnTitles();
    bool flag = false;
    List<MinionIdentity> liveIdentities = new List<MinionIdentity>((IEnumerable<MinionIdentity>) Components.LiveMinionIdentities.Items);
    if (this.EntryObjects.Count != liveIdentities.Count || this.EntryObjects.FindAll((Predicate<EntryType>) (o => liveIdentities.Contains(o.Identity))).Count != this.EntryObjects.Count)
      flag = true;
    if (flag)
      this.Reconstruct();
    this.UpdateScroll();
  }

  public void Reconstruct()
  {
    this.ClearEntries();
    this.SpawnEntries();
  }

  private void UpdateScroll()
  {
    if (!(bool) ((UnityEngine.Object) this.PanelScrollbar))
      return;
    if (this.EntryObjects.Count <= this.maxEntriesBeforeScroll)
    {
      this.PanelScrollbar.value = Mathf.Lerp(this.PanelScrollbar.value, 1f, 10f);
      this.PanelScrollbar.gameObject.SetActive(false);
    }
    else
      this.PanelScrollbar.gameObject.SetActive(true);
  }

  private void SetHeadersActive(bool state)
  {
    for (int index = 0; index < this.ColumnTitlesContainer.childCount; ++index)
      this.ColumnTitlesContainer.GetChild(index).gameObject.SetActive(state);
  }

  protected virtual void PositionColumnTitles()
  {
    if ((UnityEngine.Object) this.ColumnTitlesContainer == (UnityEngine.Object) null)
      return;
    if (this.EntryObjects.Count <= 0)
    {
      this.SetHeadersActive(false);
    }
    else
    {
      this.SetHeadersActive(true);
      int childCount = this.EntryObjects[0].transform.childCount;
      for (int index = 0; index < childCount; ++index)
      {
        OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(index).GetComponent<OverviewColumnIdentity>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          GameObject go = Util.KInstantiate(this.Prefab_ColumnTitle, (GameObject) null, (string) null);
          go.name = component.Column_DisplayName;
          LocText componentInChildren = go.GetComponentInChildren<LocText>();
          go.transform.SetParent(this.ColumnTitlesContainer);
          componentInChildren.text = !component.StringLookup ? component.Column_DisplayName : (string) Strings.Get(component.Column_DisplayName);
          go.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.TOOLTIPS.SORTCOLUMN, (object) componentInChildren.text);
          go.rectTransform().anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x, 0.0f);
          OverviewColumnIdentity overviewColumnIdentity = go.GetComponent<OverviewColumnIdentity>();
          if ((UnityEngine.Object) overviewColumnIdentity == (UnityEngine.Object) null)
            overviewColumnIdentity = go.AddComponent<OverviewColumnIdentity>();
          overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
          overviewColumnIdentity.columnID = component.columnID;
          overviewColumnIdentity.xPivot = component.xPivot;
          overviewColumnIdentity.Sortable = component.Sortable;
          if (overviewColumnIdentity.Sortable)
            overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
        }
      }
      this.UpdateColumnTitles();
      this.sortToggleGroup = this.gameObject.AddComponent<ToggleGroup>();
      this.sortToggleGroup.allowSwitchOff = true;
    }
  }

  protected void SortByName(bool reverse)
  {
    List<EntryType> sortedEntries = new List<EntryType>((IEnumerable<EntryType>) this.EntryObjects);
    sortedEntries.Sort((Comparison<EntryType>) ((a, b) => (a.Identity.GetProperName() + (object) a.gameObject.GetInstanceID()).CompareTo(b.Identity.GetProperName() + (object) b.gameObject.GetInstanceID())));
    this.ReorderEntries(sortedEntries, reverse);
  }

  protected void UpdateColumnTitles()
  {
    if (this.EntryObjects.Count <= 0 || !this.EntryObjects[0].gameObject.activeSelf)
    {
      this.SetHeadersActive(false);
    }
    else
    {
      this.SetHeadersActive(true);
      for (int index1 = 0; index1 < this.ColumnTitlesContainer.childCount; ++index1)
      {
        RectTransform rectTransform = this.ColumnTitlesContainer.GetChild(index1).rectTransform();
        for (int index2 = 0; index2 < this.EntryObjects[0].transform.childCount; ++index2)
        {
          OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(index2).GetComponent<OverviewColumnIdentity>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Column_DisplayName == rectTransform.name)
          {
            rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
            rectTransform.anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x + this.columnTitleHorizontalOffset, 0.0f);
            rectTransform.sizeDelta = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);
            if ((double) rectTransform.anchoredPosition.x == 0.0)
              rectTransform.gameObject.SetActive(false);
            else
              rectTransform.gameObject.SetActive(true);
          }
        }
      }
    }
  }

  protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse)
  {
    for (int index = 0; index < sortedEntries.Count; ++index)
    {
      if (reverse)
        sortedEntries[index].transform.SetSiblingIndex(sortedEntries.Count - 1 - index);
      else
        sortedEntries[index].transform.SetSiblingIndex(index);
    }
  }
}
