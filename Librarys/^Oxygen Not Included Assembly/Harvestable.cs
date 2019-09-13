// Decompiled with JetBrains decompiler
// Type: Harvestable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TUNING;

[SerializationConfig(MemberSerialization.OptIn)]
public class Harvestable : Workable
{
  private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>((System.Action<Harvestable, object>) ((component, data) => component.ForceCancelHarvest(data)));
  private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>((System.Action<Harvestable, object>) ((component, data) => component.OnCancel(data)));
  public HarvestDesignatable harvestDesignatable;
  [Serialize]
  protected bool canBeHarvested;
  protected Chore chore;

  protected Harvestable()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  public Worker completed_by { get; protected set; }

  public bool CanBeHarvested
  {
    get
    {
      return this.canBeHarvested;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
  }

  protected override void OnSpawn()
  {
    this.harvestDesignatable = this.GetComponent<HarvestDesignatable>();
    this.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
    this.SetWorkTime(10f);
    this.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
    this.faceTargetWhenWorking = true;
    Components.Harvestables.Add(this);
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  }

  public void OnUprooted(object data)
  {
    if (!this.canBeHarvested)
      return;
    this.Harvest();
  }

  public void Harvest()
  {
    this.harvestDesignatable.MarkedForHarvest = false;
    this.chore = (Chore) null;
    this.Trigger(1272413801, (object) this);
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
    component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  public void OnMarkedForHarvest()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.chore == null)
    {
      this.chore = (Chore) new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
      component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, (object) this);
    }
    component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
  }

  public void SetCanBeHarvested(bool state)
  {
    this.canBeHarvested = state;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.canBeHarvested)
    {
      component.AddStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, (object) null);
      if (this.harvestDesignatable.HarvestWhenReady)
        this.harvestDesignatable.MarkForHarvest();
      else if (this.harvestDesignatable.InPlanterBox)
        component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, (object) this);
    }
    else
    {
      component.RemoveStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, false);
      component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
    }
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.completed_by = worker;
    this.Harvest();
  }

  protected virtual void OnCancel(object data)
  {
    if (this.chore != null)
    {
      this.chore.Cancel("Cancel harvest");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
      this.harvestDesignatable.SetHarvestWhenReady(false);
    }
    this.harvestDesignatable.MarkedForHarvest = false;
  }

  public bool HasChore()
  {
    return this.chore != null;
  }

  public virtual void ForceCancelHarvest(object data = null)
  {
    this.OnCancel((object) null);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Harvestables.Remove(this);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
  }
}
