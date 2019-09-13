// Decompiled with JetBrains decompiler
// Type: Uprootable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

public class Uprootable : Workable
{
  private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>((System.Action<Uprootable, object>) ((component, data) => component.OnPlanterStorage(data)));
  private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>((System.Action<Uprootable, object>) ((component, data) => component.ForceCancelUproot(data)));
  private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>((System.Action<Uprootable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>((System.Action<Uprootable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [Serialize]
  protected bool canBeUprooted = true;
  public bool deselectOnUproot = true;
  public bool showUserMenuButtons = true;
  [Serialize]
  protected bool isMarkedForUproot;
  protected bool uprootComplete;
  [MyCmpReq]
  private Prioritizable prioritizable;
  protected Chore chore;
  private string buttonLabel;
  private string buttonTooltip;
  private string cancelButtonLabel;
  private string cancelButtonTooltip;
  private StatusItem pendingStatusItem;
  public OccupyArea area;
  private Storage planterStorage;
  public HandleVector<int>.Handle partitionerEntry;

  protected Uprootable()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.buttonLabel = (string) UI.USERMENUACTIONS.UPROOT.NAME;
    this.buttonTooltip = (string) UI.USERMENUACTIONS.UPROOT.TOOLTIP;
    this.cancelButtonLabel = (string) UI.USERMENUACTIONS.CANCELUPROOT.NAME;
    this.cancelButtonTooltip = (string) UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
    this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
  }

  public bool IsMarkedForUproot
  {
    get
    {
      return this.isMarkedForUproot;
    }
  }

  public Storage GetPlanterStorage
  {
    get
    {
      return this.planterStorage;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
    this.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
    this.SetWorkTime(12.5f);
    this.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
    this.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
    this.faceTargetWhenWorking = true;
    Components.Uprootables.Add(this);
    this.area = this.GetComponent<OccupyArea>();
    Prioritizable.AddRef(this.gameObject);
    this.gameObject.AddTag(GameTags.Plant);
    this.partitionerEntry = GameScenePartitioner.Instance.Add(this.gameObject.name, (object) this.gameObject.GetComponent<KPrefabID>(), new Extents(Grid.PosToCell(this.gameObject), this.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets), GameScenePartitioner.Instance.plants, (System.Action<object>) null);
    if (!this.isMarkedForUproot)
      return;
    this.MarkForUproot(true);
  }

  private void OnPlanterStorage(object data)
  {
    this.planterStorage = (Storage) data;
    Prioritizable component = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.showIcon = (UnityEngine.Object) this.planterStorage == (UnityEngine.Object) null;
  }

  public bool IsInPlanterBox()
  {
    return (UnityEngine.Object) this.planterStorage != (UnityEngine.Object) null;
  }

  public void Uproot()
  {
    this.isMarkedForUproot = false;
    this.chore = (Chore) null;
    this.uprootComplete = true;
    this.Trigger(-216549700, (object) this);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public void SetCanBeUprooted(bool state)
  {
    this.canBeUprooted = state;
    if (this.canBeUprooted)
      this.SetUprootedComplete(false);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public void SetUprootedComplete(bool state)
  {
    this.uprootComplete = state;
  }

  public void MarkForUproot(bool instantOnDebug = true)
  {
    if (!this.canBeUprooted)
      return;
    if (DebugHandler.InstantBuildMode && instantOnDebug)
      this.Uproot();
    else if (this.chore == null)
    {
      this.chore = (Chore) new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
      this.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, (object) this);
    }
    this.isMarkedForUproot = true;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.Uproot();
  }

  private void OnCancel(object data)
  {
    if (this.chore != null)
    {
      this.chore.Cancel("Cancel uproot");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
    }
    this.isMarkedForUproot = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public bool HasChore()
  {
    return this.chore != null;
  }

  private void OnClickUproot()
  {
    this.MarkForUproot(true);
  }

  protected void OnClickCancelUproot()
  {
    this.OnCancel((object) null);
  }

  public virtual void ForceCancelUproot(object data = null)
  {
    this.OnCancel((object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.showUserMenuButtons)
      return;
    if (this.uprootComplete)
    {
      if (!this.deselectOnUproot)
        return;
      KSelectable component = this.GetComponent<KSelectable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component))
        return;
      SelectTool.Instance.Select((KSelectable) null, false);
    }
    else
    {
      if (!this.canBeUprooted)
        return;
      Game.Instance.userMenu.AddButton(this.gameObject, this.chore == null ? new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, this.buttonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, this.cancelButtonTooltip, true), 1f);
    }
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    Components.Uprootables.Remove(this);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
  }
}
