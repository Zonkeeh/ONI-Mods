// Decompiled with JetBrains decompiler
// Type: CrewRationsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrewRationsScreen : CrewListScreen<CrewRationsEntry>
{
  [SerializeField]
  private KButton closebutton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.closebutton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshCrewPortraitContent();
    this.SortByPreviousSelected();
  }

  private void SortByPreviousSelected()
  {
    if ((UnityEngine.Object) this.sortToggleGroup == (UnityEngine.Object) null || (UnityEngine.Object) this.lastSortToggle == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < this.ColumnTitlesContainer.childCount; ++index)
    {
      OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(index).GetComponent<OverviewColumnIdentity>();
      if ((UnityEngine.Object) this.ColumnTitlesContainer.GetChild(index).GetComponent<Toggle>() == (UnityEngine.Object) this.lastSortToggle)
      {
        if (component.columnID == "name")
          this.SortByName(this.lastSortReversed);
        if (component.columnID == "health")
          this.SortByAmount("HitPoints", this.lastSortReversed);
        if (component.columnID == "stress")
          this.SortByAmount("Stress", this.lastSortReversed);
        if (component.columnID == "calories")
          this.SortByAmount("Calories", this.lastSortReversed);
      }
    }
  }

  protected override void PositionColumnTitles()
  {
    base.PositionColumnTitles();
    for (int index = 0; index < this.ColumnTitlesContainer.childCount; ++index)
    {
      OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(index).GetComponent<OverviewColumnIdentity>();
      if (component.Sortable)
      {
        Toggle toggle = this.ColumnTitlesContainer.GetChild(index).GetComponent<Toggle>();
        toggle.group = this.sortToggleGroup;
        ImageToggleState toggleImage = toggle.GetComponentInChildren<ImageToggleState>(true);
        if (component.columnID == "name")
          toggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
          {
            this.SortByName(!toggle.isOn);
            this.lastSortToggle = toggle;
            this.lastSortReversed = !toggle.isOn;
            this.ResetSortToggles(toggle);
            if (toggle.isOn)
              toggleImage.SetActive();
            else
              toggleImage.SetInactive();
          }));
        if (component.columnID == "health")
          toggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
          {
            this.SortByAmount("HitPoints", !toggle.isOn);
            this.lastSortToggle = toggle;
            this.lastSortReversed = !toggle.isOn;
            this.ResetSortToggles(toggle);
            if (toggle.isOn)
              toggleImage.SetActive();
            else
              toggleImage.SetInactive();
          }));
        if (component.columnID == "stress")
          toggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
          {
            this.SortByAmount("Stress", !toggle.isOn);
            this.lastSortToggle = toggle;
            this.lastSortReversed = !toggle.isOn;
            this.ResetSortToggles(toggle);
            if (toggle.isOn)
              toggleImage.SetActive();
            else
              toggleImage.SetInactive();
          }));
        if (component.columnID == "calories")
          toggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
          {
            this.SortByAmount("Calories", !toggle.isOn);
            this.lastSortToggle = toggle;
            this.lastSortReversed = !toggle.isOn;
            this.ResetSortToggles(toggle);
            if (toggle.isOn)
              toggleImage.SetActive();
            else
              toggleImage.SetInactive();
          }));
      }
    }
  }

  protected override void SpawnEntries()
  {
    base.SpawnEntries();
    foreach (MinionIdentity _identity in Components.LiveMinionIdentities.Items)
    {
      CrewRationsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewRationsEntry>();
      component.Populate(_identity);
      this.EntryObjects.Add(component);
    }
    this.SortByPreviousSelected();
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    foreach (CrewListEntry entryObject in this.EntryObjects)
      entryObject.Refresh();
  }

  private void SortByAmount(string amount_id, bool reverse)
  {
    List<CrewRationsEntry> sortedEntries = new List<CrewRationsEntry>((IEnumerable<CrewRationsEntry>) this.EntryObjects);
    sortedEntries.Sort((Comparison<CrewRationsEntry>) ((a, b) => a.Identity.GetAmounts().GetValue(amount_id).CompareTo(b.Identity.GetAmounts().GetValue(amount_id))));
    this.ReorderEntries(sortedEntries, reverse);
  }

  private void ResetSortToggles(Toggle exceptToggle)
  {
    for (int index = 0; index < this.ColumnTitlesContainer.childCount; ++index)
    {
      Toggle component = this.ColumnTitlesContainer.GetChild(index).GetComponent<Toggle>();
      ImageToggleState componentInChildren = component.GetComponentInChildren<ImageToggleState>(true);
      if ((UnityEngine.Object) component != (UnityEngine.Object) exceptToggle)
        componentInChildren.SetDisabled();
    }
  }
}
