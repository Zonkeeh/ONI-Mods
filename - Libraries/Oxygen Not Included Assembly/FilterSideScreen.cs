// Decompiled with JetBrains decompiler
// Type: FilterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterSideScreen : SideScreenContent
{
  public Dictionary<Element, FilterSideScreenRow> filterRowMap = new Dictionary<Element, FilterSideScreenRow>();
  public GameObject elementEntryPrefab;
  public GameObject elementEntryContainer;
  public Image outputIcon;
  public Image everythingElseIcon;
  public LocText outputElementHeaderLabel;
  public LocText everythingElseHeaderLabel;
  public LocText selectElementHeaderLabel;
  public LocText currentSelectionLabel;
  public bool isLogicFilter;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.filterRowMap.Clear();
    this.PopulateElements();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    if (!this.isLogicFilter ? (UnityEngine.Object) target.GetComponent<ElementFilter>() != (UnityEngine.Object) null : (UnityEngine.Object) target.GetComponent<ConduitElementSensor>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<LogicElementSensor>() != (UnityEngine.Object) null)
      return (UnityEngine.Object) target.GetComponent<Filterable>() != (UnityEngine.Object) null;
    return false;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    Filterable component = target.GetComponent<Filterable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.everythingElseHeaderLabel.text = (string) (component.filterElementState != Filterable.ElementState.Gas ? STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.LIQUID : STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.UNFILTEREDELEMENTS.GAS);
    this.SetFilterElement(!component.SelectedTag.IsValid ? ElementLoader.FindElementByHash(SimHashes.Void) : ElementLoader.GetElement(component.SelectedTag));
    this.Configure(component);
  }

  private void PopulateElements()
  {
    List<Element> elementList = new List<Element>((IEnumerable<Element>) ElementLoader.elements);
    elementList.Sort((Comparison<Element>) ((a, b) =>
    {
      if (a.id == SimHashes.Void)
        return -1;
      if (b.id == SimHashes.Void)
        return 1;
      return a.name.CompareTo(b.name);
    }));
    foreach (Element elem in elementList)
    {
      FilterSideScreenRow row = Util.KInstantiateUI(this.elementEntryPrefab, this.elementEntryContainer, false).GetComponent<FilterSideScreenRow>();
      row.SetElement(elem);
      row.button.onClick += (System.Action) (() => this.SetFilterElement(row.element));
      this.filterRowMap.Add(row.element, row);
    }
  }

  private void Configure(Filterable filterable)
  {
    IList<Tag> tagOptions = filterable.GetTagOptions();
    foreach (KeyValuePair<Element, FilterSideScreenRow> filterRow in this.filterRowMap)
    {
      Element key = filterRow.Key;
      bool flag = tagOptions.Contains(key.tag);
      filterRow.Value.gameObject.SetActive(flag);
    }
  }

  private void SetFilterElement(Element element)
  {
    Filterable component = DetailsScreen.Instance.target.GetComponent<Filterable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    LocString locString = component.filterElementState != Filterable.ElementState.Gas ? STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.LIQUID : STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.FILTEREDELEMENT.GAS;
    this.currentSelectionLabel.text = string.Format((string) locString, (object) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NOELEMENTSELECTED);
    if (element == null)
      return;
    component.SelectedTag = element.tag;
    foreach (KeyValuePair<Element, FilterSideScreenRow> filterRow in this.filterRowMap)
    {
      bool selected = filterRow.Key == element;
      filterRow.Value.SetSelected(selected);
      if (selected)
      {
        if (element.id != SimHashes.Void && element.id != SimHashes.Vacuum)
          this.currentSelectionLabel.text = string.Format((string) locString, (object) element.name);
        else
          this.currentSelectionLabel.text = (string) STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
      }
    }
  }
}
