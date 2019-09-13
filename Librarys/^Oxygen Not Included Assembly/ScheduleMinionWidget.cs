// Decompiled with JetBrains decompiler
// Type: ScheduleMinionWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Linq;
using UnityEngine;

public class ScheduleMinionWidget : KMonoBehaviour
{
  [SerializeField]
  private CrewPortrait portrait;
  [SerializeField]
  private DropDown dropDown;
  [SerializeField]
  private LocText label;
  [SerializeField]
  private GameObject nightOwlIcon;
  [SerializeField]
  private GameObject earlyBirdIcon;

  public Schedulable schedulable { get; private set; }

  public void ChangeAssignment(Schedule targetSchedule, Schedulable schedulable)
  {
    DebugUtil.LogArgs((object) "Assigning", (object) schedulable, (object) "from", (object) ScheduleManager.Instance.GetSchedule(schedulable).name, (object) "to", (object) targetSchedule.name);
    ScheduleManager.Instance.GetSchedule(schedulable).Unassign(schedulable);
    targetSchedule.Assign(schedulable);
  }

  public void Setup(Schedulable schedulable)
  {
    this.schedulable = schedulable;
    IAssignableIdentity component1 = schedulable.GetComponent<IAssignableIdentity>();
    this.portrait.SetIdentityObject(component1, true);
    this.label.text = component1.GetProperName();
    MinionIdentity minionIdentity = component1 as MinionIdentity;
    StoredMinionIdentity storedMinionIdentity = component1 as StoredMinionIdentity;
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      Traits component2 = minionIdentity.GetComponent<Traits>();
      if (component2.HasTrait("NightOwl"))
        this.nightOwlIcon.SetActive(true);
      else if (component2.HasTrait("EarlyBird"))
        this.earlyBirdIcon.SetActive(true);
    }
    else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
    {
      if (storedMinionIdentity.traitIDs.Contains("NightOwl"))
        this.nightOwlIcon.SetActive(true);
      else if (storedMinionIdentity.traitIDs.Contains("EarlyBird"))
        this.earlyBirdIcon.SetActive(true);
    }
    this.dropDown.Initialize(ScheduleManager.Instance.GetSchedules().Cast<IListableOption>(), new System.Action<IListableOption, object>(this.OnDropEntryClick), (Func<IListableOption, IListableOption, object, int>) null, new System.Action<DropDownEntry, object>(this.DropEntryRefreshAction), false, (object) schedulable);
  }

  private void OnDropEntryClick(IListableOption option, object obj)
  {
    this.ChangeAssignment((Schedule) option, this.schedulable);
  }

  private void DropEntryRefreshAction(DropDownEntry entry, object obj)
  {
    Schedule entryData = (Schedule) entry.entryData;
    if (((Schedulable) obj).GetSchedule() == entryData)
    {
      entry.label.text = string.Format((string) UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, (object) entryData.name);
      entry.button.isInteractable = false;
    }
    else
    {
      entry.label.text = entryData.name;
      entry.button.isInteractable = true;
    }
  }

  public void SetupBlank(Schedule schedule)
  {
    this.label.text = (string) UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_BLANK;
    this.dropDown.Initialize(Components.LiveMinionIdentities.Items.Cast<IListableOption>(), new System.Action<IListableOption, object>(this.OnBlankDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.BlankDropEntrySort), new System.Action<DropDownEntry, object>(this.BlankDropEntryRefreshAction), false, (object) schedule);
    Components.LiveMinionIdentities.OnAdd += new System.Action<MinionIdentity>(this.OnLivingMinionsChanged);
    Components.LiveMinionIdentities.OnRemove += new System.Action<MinionIdentity>(this.OnLivingMinionsChanged);
  }

  private void OnLivingMinionsChanged(MinionIdentity minion)
  {
    this.dropDown.ChangeContent(Components.LiveMinionIdentities.Items.Cast<IListableOption>());
  }

  private void OnBlankDropEntryClick(IListableOption option, object obj)
  {
    this.ChangeAssignment((Schedule) obj, ((Component) option).GetComponent<Schedulable>());
  }

  private void BlankDropEntryRefreshAction(DropDownEntry entry, object obj)
  {
    Schedule schedule = (Schedule) obj;
    MinionIdentity entryData = (MinionIdentity) entry.entryData;
    if (schedule.IsAssigned(entryData.GetComponent<Schedulable>()))
    {
      entry.label.text = string.Format((string) UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, (object) entryData.GetProperName());
      entry.button.isInteractable = false;
    }
    else
    {
      entry.label.text = entryData.GetProperName();
      entry.button.isInteractable = true;
    }
    Traits component = entryData.GetComponent<Traits>();
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightOwlIcon").gameObject.SetActive(component.HasTrait("NightOwl"));
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EarlyBirdIcon").gameObject.SetActive(component.HasTrait("EarlyBird"));
  }

  private int BlankDropEntrySort(IListableOption a, IListableOption b, object obj)
  {
    Schedule schedule = (Schedule) obj;
    MinionIdentity minionIdentity1 = (MinionIdentity) a;
    MinionIdentity minionIdentity2 = (MinionIdentity) b;
    bool flag1 = schedule.IsAssigned(minionIdentity1.GetComponent<Schedulable>());
    bool flag2 = schedule.IsAssigned(minionIdentity2.GetComponent<Schedulable>());
    if (flag1 && !flag2)
      return -1;
    return !flag1 && flag2 ? 1 : 0;
  }

  protected override void OnCleanUp()
  {
    Components.LiveMinionIdentities.OnAdd -= new System.Action<MinionIdentity>(this.OnLivingMinionsChanged);
    Components.LiveMinionIdentities.OnRemove -= new System.Action<MinionIdentity>(this.OnLivingMinionsChanged);
  }
}
