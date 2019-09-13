// Decompiled with JetBrains decompiler
// Type: BuildingChoresPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingChoresPanel : TargetScreen
{
  private List<HierarchyReferences> choreEntries = new List<HierarchyReferences>();
  private List<BuildingChoresPanelDupeRow> dupeEntries = new List<BuildingChoresPanelDupeRow>();
  private List<BuildingChoresPanel.DupeEntryData> DupeEntryDatas = new List<BuildingChoresPanel.DupeEntryData>();
  public GameObject choreGroupPrefab;
  public GameObject chorePrefab;
  public BuildingChoresPanelDupeRow dupePrefab;
  private GameObject detailsPanel;
  private DetailsPanelDrawer drawer;
  private HierarchyReferences choreGroup;
  private int activeChoreEntries;
  private int activeDupeEntries;

  public override bool IsValidForTarget(GameObject target)
  {
    KPrefabID component = target.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasTag(GameTags.HasChores))
      return !component.HasTag(GameTags.Minion);
    return false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreGroup = Util.KInstantiateUI<HierarchyReferences>(this.choreGroupPrefab, this.gameObject, false);
    this.choreGroup.gameObject.SetActive(true);
  }

  private void Update()
  {
    this.Refresh();
  }

  public override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  public override void OnDeselectTarget(GameObject target)
  {
    base.OnDeselectTarget(target);
  }

  private void Refresh()
  {
    this.RefreshDetails();
  }

  private void RefreshDetails()
  {
    foreach (Chore chore in GlobalChoreProvider.Instance.chores)
    {
      if (!chore.isNull && (UnityEngine.Object) chore.gameObject == (UnityEngine.Object) this.selectedTarget)
        this.AddChoreEntry(chore);
    }
    for (int activeDupeEntries = this.activeDupeEntries; activeDupeEntries < this.dupeEntries.Count; ++activeDupeEntries)
      this.dupeEntries[activeDupeEntries].gameObject.SetActive(false);
    this.activeDupeEntries = 0;
    for (int activeChoreEntries = this.activeChoreEntries; activeChoreEntries < this.choreEntries.Count; ++activeChoreEntries)
      this.choreEntries[activeChoreEntries].gameObject.SetActive(false);
    this.activeChoreEntries = 0;
  }

  private void AddChoreEntry(Chore chore)
  {
    HierarchyReferences choreEntry = this.GetChoreEntry(GameUtil.GetChoreName(chore, (object) null), chore.choreType, this.choreGroup.GetReference<RectTransform>("EntriesContainer"));
    FetchChore fetch = chore as FetchChore;
    ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      pooledList.Clear();
      ChoreConsumer component = minionIdentity.GetComponent<ChoreConsumer>();
      Chore.Precondition.Context context = new Chore.Precondition.Context();
      ChoreConsumer.PreconditionSnapshot preconditionSnapshot = component.GetLastPreconditionSnapshot();
      if (preconditionSnapshot.doFailedContextsNeedSorting)
      {
        preconditionSnapshot.failedContexts.Sort();
        preconditionSnapshot.doFailedContextsNeedSorting = false;
      }
      pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.failedContexts);
      pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.succeededContexts);
      int num1 = -1;
      int num2 = 0;
      for (int index = pooledList.Count - 1; index >= 0; --index)
      {
        if (!((UnityEngine.Object) pooledList[index].chore.driver != (UnityEngine.Object) null) || !((UnityEngine.Object) pooledList[index].chore.driver != (UnityEngine.Object) component.choreDriver))
        {
          bool flag = pooledList[index].IsPotentialSuccess();
          if (flag)
            ++num2;
          FetchAreaChore chore1 = pooledList[index].chore as FetchAreaChore;
          if (pooledList[index].chore == chore || fetch != null && chore1 != null && chore1.smi.SameDestination(fetch))
          {
            num1 = !flag ? int.MaxValue : num2;
            context = pooledList[index];
            break;
          }
        }
      }
      if (num1 >= 0)
        this.DupeEntryDatas.Add(new BuildingChoresPanel.DupeEntryData()
        {
          consumer = component,
          context = context,
          personalPriority = component.GetPersonalPriority(chore.choreType),
          rank = num1
        });
    }
    pooledList.Recycle();
    this.DupeEntryDatas.Sort();
    foreach (BuildingChoresPanel.DupeEntryData dupeEntryData in this.DupeEntryDatas)
      this.GetDupeEntry(dupeEntryData, choreEntry.GetReference<RectTransform>("DupeContainer"));
    this.DupeEntryDatas.Clear();
  }

  private HierarchyReferences GetChoreEntry(
    string label,
    ChoreType choreType,
    RectTransform parent)
  {
    HierarchyReferences hierarchyReferences;
    if (this.activeChoreEntries >= this.choreEntries.Count)
    {
      hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.chorePrefab, parent.gameObject, false);
      this.choreEntries.Add(hierarchyReferences);
    }
    else
    {
      hierarchyReferences = this.choreEntries[this.activeChoreEntries];
      hierarchyReferences.transform.SetParent((Transform) parent);
      hierarchyReferences.transform.SetAsLastSibling();
    }
    ++this.activeChoreEntries;
    hierarchyReferences.GetReference<LocText>("ChoreLabel").text = label;
    hierarchyReferences.GetReference<LocText>("ChoreSubLabel").text = GameUtil.ChoreGroupsForChoreType(choreType);
    Image reference1 = hierarchyReferences.GetReference<Image>("Icon");
    if (choreType.groups.Length > 0)
    {
      Sprite sprite = Assets.GetSprite((HashedString) choreType.groups[0].sprite);
      reference1.sprite = sprite;
      reference1.gameObject.SetActive(true);
      reference1.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, (object) choreType.groups[0].Name);
    }
    else
      reference1.gameObject.SetActive(false);
    Image reference2 = hierarchyReferences.GetReference<Image>("Icon2");
    if (choreType.groups.Length > 1)
    {
      Sprite sprite = Assets.GetSprite((HashedString) choreType.groups[1].sprite);
      reference2.sprite = sprite;
      reference2.gameObject.SetActive(true);
      reference2.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, (object) choreType.groups[1].Name);
    }
    else
      reference2.gameObject.SetActive(false);
    hierarchyReferences.gameObject.SetActive(true);
    return hierarchyReferences;
  }

  private BuildingChoresPanelDupeRow GetDupeEntry(
    BuildingChoresPanel.DupeEntryData data,
    RectTransform parent)
  {
    BuildingChoresPanelDupeRow choresPanelDupeRow;
    if (this.activeDupeEntries >= this.dupeEntries.Count)
    {
      choresPanelDupeRow = Util.KInstantiateUI<BuildingChoresPanelDupeRow>(this.dupePrefab.gameObject, parent.gameObject, false);
      this.dupeEntries.Add(choresPanelDupeRow);
    }
    else
    {
      choresPanelDupeRow = this.dupeEntries[this.activeDupeEntries];
      choresPanelDupeRow.transform.SetParent((Transform) parent);
      choresPanelDupeRow.transform.SetAsLastSibling();
    }
    ++this.activeDupeEntries;
    choresPanelDupeRow.Init(data);
    choresPanelDupeRow.gameObject.SetActive(true);
    return choresPanelDupeRow;
  }

  public class DupeEntryData : IComparable<BuildingChoresPanel.DupeEntryData>
  {
    public ChoreConsumer consumer;
    public Chore.Precondition.Context context;
    public int personalPriority;
    public int rank;

    public int CompareTo(BuildingChoresPanel.DupeEntryData other)
    {
      if (this.personalPriority != other.personalPriority)
        return other.personalPriority.CompareTo(this.personalPriority);
      if (this.rank != other.rank)
        return this.rank.CompareTo(other.rank);
      if (this.consumer.GetProperName() != other.consumer.GetProperName())
        return this.consumer.GetProperName().CompareTo(other.consumer.GetProperName());
      return this.consumer.GetInstanceID().CompareTo(other.consumer.GetInstanceID());
    }
  }
}
