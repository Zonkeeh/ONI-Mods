// Decompiled with JetBrains decompiler
// Type: MinionTodoSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionTodoSideScreen : SideScreenContent
{
  private List<Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>> priorityGroups = new List<Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>>();
  private List<MinionTodoChoreEntry> choreEntries = new List<MinionTodoChoreEntry>();
  private List<GameObject> choreTargets = new List<GameObject>();
  private bool useOffscreenIndicators;
  public MinionTodoChoreEntry taskEntryPrefab;
  public GameObject priorityGroupPrefab;
  public GameObject taskEntryContainer;
  public MinionTodoChoreEntry currentTask;
  public LocText currentScheduleBlockLabel;
  private SchedulerHandle refreshHandle;
  private ChoreConsumer choreConsumer;
  [SerializeField]
  private ColorStyleSetting buttonColorSettingCurrent;
  [SerializeField]
  private ColorStyleSetting buttonColorSettingStandard;
  private static List<JobsTableScreen.PriorityInfo> _priorityInfo;
  private int activeChoreEntries;

  public static List<JobsTableScreen.PriorityInfo> priorityInfo
  {
    get
    {
      if (MinionTodoSideScreen._priorityInfo == null)
        MinionTodoSideScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>()
        {
          new JobsTableScreen.PriorityInfo(4, Assets.GetSprite((HashedString) "ic_dupe"), STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY),
          new JobsTableScreen.PriorityInfo(3, Assets.GetSprite((HashedString) "notification_exclamation"), STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY),
          new JobsTableScreen.PriorityInfo(2, Assets.GetSprite((HashedString) "status_item_room_required"), STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS),
          new JobsTableScreen.PriorityInfo(1, Assets.GetSprite((HashedString) "status_item_prioritized"), STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.HIGH),
          new JobsTableScreen.PriorityInfo(0, (Sprite) null, STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.BASIC),
          new JobsTableScreen.PriorityInfo(-1, Assets.GetSprite((HashedString) "icon_gear"), STRINGS.UI.JOBSSCREEN.PRIORITY_CLASS.IDLE)
        };
      return MinionTodoSideScreen._priorityInfo;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    foreach (JobsTableScreen.PriorityInfo priorityInfo1 in MinionTodoSideScreen.priorityInfo)
    {
      PriorityScreen.PriorityClass priority = (PriorityScreen.PriorityClass) priorityInfo1.priority;
      if (priority == PriorityScreen.PriorityClass.basic)
      {
        for (int b = 5; b >= 0; --b)
        {
          Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple = new Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, b, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
          tuple.third.name = "PriorityGroup_" + (string) priorityInfo1.name + "_" + (object) b;
          tuple.third.gameObject.SetActive(true);
          JobsTableScreen.PriorityInfo priorityInfo2 = JobsTableScreen.priorityInfo[b];
          tuple.third.GetReference<LocText>("Title").text = priorityInfo2.name.text.ToUpper();
          tuple.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo2.sprite;
          this.priorityGroups.Add(tuple);
        }
      }
      else
      {
        Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple = new Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, 3, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
        tuple.third.name = "PriorityGroup_" + (string) priorityInfo1.name;
        tuple.third.gameObject.SetActive(true);
        tuple.third.GetReference<LocText>("Title").text = priorityInfo1.name.text.ToUpper();
        tuple.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo1.sprite;
        this.priorityGroups.Add(tuple);
      }
    }
  }

  public override bool IsValidForTarget(GameObject target)
  {
    if ((UnityEngine.Object) target.GetComponent<MinionIdentity>() != (UnityEngine.Object) null)
      return !target.HasTag(GameTags.Dead);
    return false;
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    this.refreshHandle.ClearScheduler();
  }

  public override void SetTarget(GameObject target)
  {
    this.refreshHandle.ClearScheduler();
    base.SetTarget(target);
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    this.PopulateElements((object) null);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.refreshHandle.ClearScheduler();
    if (!show)
    {
      if (!this.useOffscreenIndicators)
        return;
      foreach (GameObject choreTarget in this.choreTargets)
        OffscreenIndicator.Instance.DeactivateIndicator(choreTarget);
    }
    else
    {
      if ((UnityEngine.Object) DetailsScreen.Instance.target == (UnityEngine.Object) null)
        return;
      this.choreConsumer = DetailsScreen.Instance.target.GetComponent<ChoreConsumer>();
      this.PopulateElements((object) null);
    }
  }

  private void PopulateElements(object data = null)
  {
    this.refreshHandle.ClearScheduler();
    this.refreshHandle = UIScheduler.Instance.Schedule("RefreshToDoList", 0.1f, new System.Action<object>(this.PopulateElements), (object) null, (SchedulerGroup) null);
    ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
    ChoreConsumer.PreconditionSnapshot preconditionSnapshot = this.choreConsumer.GetLastPreconditionSnapshot();
    if (preconditionSnapshot.doFailedContextsNeedSorting)
    {
      preconditionSnapshot.failedContexts.Sort();
      preconditionSnapshot.doFailedContextsNeedSorting = false;
    }
    pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.failedContexts);
    pooledList.AddRange((IEnumerable<Chore.Precondition.Context>) preconditionSnapshot.succeededContexts);
    Chore.Precondition.Context choreB = new Chore.Precondition.Context();
    MinionTodoChoreEntry minionTodoChoreEntry = (MinionTodoChoreEntry) null;
    int amount = 0;
    Schedulable component = DetailsScreen.Instance.target.GetComponent<Schedulable>();
    string str = string.Empty;
    Schedule schedule = component.GetSchedule();
    if (schedule != null)
      str = schedule.GetBlock(Schedule.GetBlockIdx()).name;
    this.currentScheduleBlockLabel.SetText(string.Format((string) STRINGS.UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CURRENT_SCHEDULE_BLOCK, (object) str));
    this.choreTargets.Clear();
    bool flag = false;
    this.activeChoreEntries = 0;
    for (int index = pooledList.Count - 1; index >= 0; --index)
    {
      if (pooledList[index].chore != null && !pooledList[index].chore.target.isNull && (!((UnityEngine.Object) pooledList[index].chore.target.gameObject == (UnityEngine.Object) null) && pooledList[index].IsPotentialSuccess()))
      {
        if ((UnityEngine.Object) pooledList[index].chore.driver == (UnityEngine.Object) this.choreConsumer.choreDriver)
        {
          this.currentTask.Apply(pooledList[index]);
          minionTodoChoreEntry = this.currentTask;
          choreB = pooledList[index];
          amount = 0;
          flag = true;
        }
        else if (!flag && this.activeChoreEntries != 0 && GameUtil.AreChoresUIMergeable(pooledList[index], choreB))
        {
          ++amount;
          minionTodoChoreEntry.SetMoreAmount(amount);
        }
        else
        {
          MinionTodoChoreEntry choreEntry = this.GetChoreEntry(this.PriorityGroupForPriority(this.choreConsumer, pooledList[index].chore).GetReference<RectTransform>("EntriesContainer"));
          choreEntry.Apply(pooledList[index]);
          minionTodoChoreEntry = choreEntry;
          choreB = pooledList[index];
          amount = 0;
          flag = false;
        }
      }
    }
    pooledList.Recycle();
    for (int index = this.choreEntries.Count - 1; index >= this.activeChoreEntries; --index)
      this.choreEntries[index].gameObject.SetActive(false);
    foreach (Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> priorityGroup in this.priorityGroups)
    {
      RectTransform reference = priorityGroup.third.GetReference<RectTransform>("EntriesContainer");
      priorityGroup.third.gameObject.SetActive(reference.childCount > 0);
    }
  }

  private MinionTodoChoreEntry GetChoreEntry(RectTransform parent)
  {
    MinionTodoChoreEntry minionTodoChoreEntry;
    if (this.activeChoreEntries >= this.choreEntries.Count - 1)
    {
      minionTodoChoreEntry = Util.KInstantiateUI<MinionTodoChoreEntry>(this.taskEntryPrefab.gameObject, parent.gameObject, false);
      this.choreEntries.Add(minionTodoChoreEntry);
    }
    else
    {
      minionTodoChoreEntry = this.choreEntries[this.activeChoreEntries];
      minionTodoChoreEntry.transform.SetParent((Transform) parent);
      minionTodoChoreEntry.transform.SetAsLastSibling();
    }
    ++this.activeChoreEntries;
    minionTodoChoreEntry.gameObject.SetActive(true);
    return minionTodoChoreEntry;
  }

  private HierarchyReferences PriorityGroupForPriority(
    ChoreConsumer choreConsumer,
    Chore chore)
  {
    foreach (Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> priorityGroup in this.priorityGroups)
    {
      if (priorityGroup.first == chore.masterPriority.priority_class && (chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic || priorityGroup.second == choreConsumer.GetPersonalPriority(chore.choreType)))
        return priorityGroup.third;
    }
    return (HierarchyReferences) null;
  }

  private void Button_onPointerEnter()
  {
    throw new NotImplementedException();
  }
}
