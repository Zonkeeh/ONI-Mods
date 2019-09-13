// Decompiled with JetBrains decompiler
// Type: CrewJobsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrewJobsScreen : CrewListScreen<CrewJobsEntry>
{
  private Dictionary<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState> EveryoneToggles = new Dictionary<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>();
  private List<ChoreGroup> choreGroups = new List<ChoreGroup>();
  public static CrewJobsScreen Instance;
  private KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState> EveryoneAllTaskToggle;
  public TextStyleSetting TextStyle_JobTooltip_Title;
  public TextStyleSetting TextStyle_JobTooltip_Description;
  public TextStyleSetting TextStyle_JobTooltip_RelevantAttributes;
  public Toggle SortEveryoneToggle;
  private bool dirty;
  private float screenWidth;

  protected override void OnActivate()
  {
    CrewJobsScreen.Instance = this;
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      this.choreGroups.Add(resource);
    base.OnActivate();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RefreshCrewPortraitContent();
    this.SortByPreviousSelected();
  }

  protected override void SpawnEntries()
  {
    base.SpawnEntries();
    foreach (MinionIdentity _identity in Components.LiveMinionIdentities.Items)
    {
      CrewJobsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewJobsEntry>();
      component.Populate(_identity);
      this.EntryObjects.Add(component);
    }
    this.SortEveryoneToggle.group = this.sortToggleGroup;
    ImageToggleState toggleImage = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
    this.SortEveryoneToggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
    {
      this.SortByName(!this.SortEveryoneToggle.isOn);
      this.lastSortToggle = this.SortEveryoneToggle;
      this.lastSortReversed = !this.SortEveryoneToggle.isOn;
      this.ResetSortToggles(this.SortEveryoneToggle);
      if (this.SortEveryoneToggle.isOn)
        toggleImage.SetActive();
      else
        toggleImage.SetInactive();
    }));
    this.SortByPreviousSelected();
    this.dirty = true;
  }

  private void SortByPreviousSelected()
  {
    if ((UnityEngine.Object) this.sortToggleGroup == (UnityEngine.Object) null || (UnityEngine.Object) this.lastSortToggle == (UnityEngine.Object) null)
      return;
    int childCount = this.ColumnTitlesContainer.childCount;
    for (int index = 0; index < childCount; ++index)
    {
      if (index < this.choreGroups.Count && (UnityEngine.Object) this.ColumnTitlesContainer.GetChild(index).Find("Title").GetComponentInChildren<Toggle>() == (UnityEngine.Object) this.lastSortToggle)
      {
        this.SortByEffectiveness(this.choreGroups[index], this.lastSortReversed, false);
        return;
      }
    }
    if (!((UnityEngine.Object) this.SortEveryoneToggle == (UnityEngine.Object) this.lastSortToggle))
      return;
    this.SortByName(this.lastSortReversed);
  }

  protected override void PositionColumnTitles()
  {
    base.PositionColumnTitles();
    int childCount = this.ColumnTitlesContainer.childCount;
    for (int index = 0; index < childCount; ++index)
    {
      if (index < this.choreGroups.Count)
      {
        Toggle sortToggle = this.ColumnTitlesContainer.GetChild(index).Find("Title").GetComponentInChildren<Toggle>();
        this.ColumnTitlesContainer.GetChild(index).rectTransform().localScale = Vector3.one;
        ChoreGroup chore_group = this.choreGroups[index];
        ImageToggleState toggleImage = sortToggle.GetComponentInChildren<ImageToggleState>(true);
        sortToggle.group = this.sortToggleGroup;
        sortToggle.onValueChanged.AddListener((UnityAction<bool>) (value =>
        {
          bool playSound = false;
          if ((UnityEngine.Object) this.lastSortToggle == (UnityEngine.Object) sortToggle)
            playSound = true;
          this.SortByEffectiveness(chore_group, !sortToggle.isOn, playSound);
          this.lastSortToggle = sortToggle;
          this.lastSortReversed = !sortToggle.isOn;
          this.ResetSortToggles(sortToggle);
          if (sortToggle.isOn)
            toggleImage.SetActive();
          else
            toggleImage.SetInactive();
        }));
      }
      ToolTip JobTooltip = this.ColumnTitlesContainer.GetChild(index).GetComponent<ToolTip>();
      JobTooltip.OnToolTip += (Func<string>) (() => this.GetJobTooltip(JobTooltip.gameObject));
      this.EveryoneToggles.Add(this.ColumnTitlesContainer.GetChild(index).GetComponentInChildren<UnityEngine.UI.Button>(), CrewJobsScreen.everyoneToggleState.on);
    }
    for (int index = 0; index < this.choreGroups.Count; ++index)
    {
      ChoreGroup chore_group = this.choreGroups[index];
      UnityEngine.UI.Button b = this.EveryoneToggles.Keys.ElementAt<UnityEngine.UI.Button>(index);
      this.EveryoneToggles.Keys.ElementAt<UnityEngine.UI.Button>(index).onClick.AddListener((UnityAction) (() => this.ToggleJobEveryone(b, chore_group)));
    }
    UnityEngine.UI.Button key = this.EveryoneToggles.ElementAt<KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>>(this.EveryoneToggles.Count - 1).Key;
    key.transform.parent.Find("Title").gameObject.GetComponentInChildren<Toggle>().gameObject.SetActive(false);
    key.onClick.AddListener((UnityAction) (() => this.ToggleAllTasksEveryone()));
    this.EveryoneAllTaskToggle = new KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>(key, this.EveryoneAllTaskToggle.Value);
  }

  private string GetJobTooltip(GameObject go)
  {
    ToolTip component1 = go.GetComponent<ToolTip>();
    component1.ClearMultiStringTooltip();
    OverviewColumnIdentity component2 = go.GetComponent<OverviewColumnIdentity>();
    if (component2.columnID != "AllTasks")
    {
      ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(component2.columnID);
      component1.AddMultiStringTooltip(component2.Column_DisplayName, (ScriptableObject) this.TextStyle_JobTooltip_Title);
      component1.AddMultiStringTooltip(choreGroup.description, (ScriptableObject) this.TextStyle_JobTooltip_Description);
      component1.AddMultiStringTooltip("\n", (ScriptableObject) this.TextStyle_JobTooltip_Description);
      component1.AddMultiStringTooltip((string) STRINGS.UI.TOOLTIPS.JOBSSCREEN_ATTRIBUTES, (ScriptableObject) this.TextStyle_JobTooltip_Description);
      component1.AddMultiStringTooltip("•  " + choreGroup.attribute.Name, (ScriptableObject) this.TextStyle_JobTooltip_RelevantAttributes);
    }
    return string.Empty;
  }

  private void ToggleAllTasksEveryone()
  {
    string name = "HUD_Click_Deselect";
    if (this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on)
      name = "HUD_Click";
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
    for (int index = 0; index < this.choreGroups.Count; ++index)
      this.SetJobEveryone(this.EveryoneAllTaskToggle.Value != CrewJobsScreen.everyoneToggleState.on, this.choreGroups[index]);
  }

  private void SetJobEveryone(UnityEngine.UI.Button button, ChoreGroup chore_group)
  {
    this.SetJobEveryone(this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on, chore_group);
  }

  private void SetJobEveryone(bool state, ChoreGroup chore_group)
  {
    foreach (CrewJobsEntry entryObject in this.EntryObjects)
      entryObject.consumer.SetPermittedByUser(chore_group, state);
  }

  private void ToggleJobEveryone(UnityEngine.UI.Button button, ChoreGroup chore_group)
  {
    string name = "HUD_Click_Deselect";
    if (this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on)
      name = "HUD_Click";
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
    foreach (CrewJobsEntry entryObject in this.EntryObjects)
      entryObject.consumer.SetPermittedByUser(chore_group, this.EveryoneToggles[button] != CrewJobsScreen.everyoneToggleState.on);
  }

  private void SortByEffectiveness(ChoreGroup chore_group, bool reverse, bool playSound)
  {
    if (playSound)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
    List<CrewJobsEntry> sortedEntries = new List<CrewJobsEntry>((IEnumerable<CrewJobsEntry>) this.EntryObjects);
    sortedEntries.Sort((Comparison<CrewJobsEntry>) ((a, b) => a.Identity.GetAttributes().GetValue(chore_group.attribute.Id).CompareTo(b.Identity.GetAttributes().GetValue(chore_group.attribute.Id))));
    this.ReorderEntries(sortedEntries, reverse);
  }

  private void ResetSortToggles(Toggle exceptToggle)
  {
    for (int index = 0; index < this.ColumnTitlesContainer.childCount; ++index)
    {
      Toggle componentInChildren1 = this.ColumnTitlesContainer.GetChild(index).Find("Title").GetComponentInChildren<Toggle>();
      if (!((UnityEngine.Object) componentInChildren1 == (UnityEngine.Object) null))
      {
        ImageToggleState componentInChildren2 = componentInChildren1.GetComponentInChildren<ImageToggleState>(true);
        if ((UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) exceptToggle)
          componentInChildren2.SetDisabled();
      }
    }
    ImageToggleState componentInChildren = this.SortEveryoneToggle.GetComponentInChildren<ImageToggleState>(true);
    if (!((UnityEngine.Object) this.SortEveryoneToggle != (UnityEngine.Object) exceptToggle))
      return;
    componentInChildren.SetDisabled();
  }

  private void Refresh()
  {
    if (!this.dirty)
      return;
    int childCount = this.ColumnTitlesContainer.childCount;
    bool flag1 = false;
    bool flag2 = false;
    for (int index1 = 0; index1 < childCount; ++index1)
    {
      bool flag3 = false;
      bool flag4 = false;
      if (this.choreGroups.Count - 1 >= index1)
      {
        ChoreGroup choreGroup = this.choreGroups[index1];
        for (int index2 = 0; index2 < this.EntryObjects.Count; ++index2)
        {
          ChoreConsumer consumer = this.EntryObjects[index2].GetComponent<CrewJobsEntry>().consumer;
          if (consumer.IsPermittedByTraits(choreGroup))
          {
            if (consumer.IsPermittedByUser(choreGroup))
            {
              flag3 = true;
              flag1 = true;
            }
            else
            {
              flag4 = true;
              flag2 = true;
            }
          }
        }
        if (flag3 && flag4)
          this.EveryoneToggles[this.EveryoneToggles.ElementAt<KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>>(index1).Key] = CrewJobsScreen.everyoneToggleState.mixed;
        else if (flag3)
          this.EveryoneToggles[this.EveryoneToggles.ElementAt<KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>>(index1).Key] = CrewJobsScreen.everyoneToggleState.on;
        else
          this.EveryoneToggles[this.EveryoneToggles.ElementAt<KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>>(index1).Key] = CrewJobsScreen.everyoneToggleState.off;
        UnityEngine.UI.Button componentInChildren = this.ColumnTitlesContainer.GetChild(index1).GetComponentInChildren<UnityEngine.UI.Button>();
        ImageToggleState component = componentInChildren.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
        switch (this.EveryoneToggles[componentInChildren])
        {
          case CrewJobsScreen.everyoneToggleState.off:
            component.SetDisabled();
            continue;
          case CrewJobsScreen.everyoneToggleState.mixed:
            component.SetInactive();
            continue;
          case CrewJobsScreen.everyoneToggleState.on:
            component.SetActive();
            continue;
          default:
            continue;
        }
      }
    }
    if (flag1 && flag2)
      this.EveryoneAllTaskToggle = new KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.mixed);
    else if (flag1)
      this.EveryoneAllTaskToggle = new KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.on);
    else if (flag2)
      this.EveryoneAllTaskToggle = new KeyValuePair<UnityEngine.UI.Button, CrewJobsScreen.everyoneToggleState>(this.EveryoneAllTaskToggle.Key, CrewJobsScreen.everyoneToggleState.off);
    ImageToggleState component1 = this.EveryoneAllTaskToggle.Key.GetComponentsInChildren<Image>(true)[1].GetComponent<ImageToggleState>();
    switch (this.EveryoneAllTaskToggle.Value)
    {
      case CrewJobsScreen.everyoneToggleState.off:
        component1.SetDisabled();
        break;
      case CrewJobsScreen.everyoneToggleState.mixed:
        component1.SetInactive();
        break;
      case CrewJobsScreen.everyoneToggleState.on:
        component1.SetActive();
        break;
    }
    this.screenWidth = this.EntriesPanelTransform.rectTransform().sizeDelta.x;
    this.ScrollRectTransform.GetComponent<LayoutElement>().minWidth = this.screenWidth;
    this.GetComponent<LayoutElement>().minWidth = this.screenWidth + 31f;
    this.dirty = false;
  }

  private void Update()
  {
    this.Refresh();
  }

  public void Dirty(object data = null)
  {
    this.dirty = true;
  }

  public enum everyoneToggleState
  {
    off,
    mixed,
    on,
  }
}
