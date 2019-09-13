// Decompiled with JetBrains decompiler
// Type: Workable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
  public Vector3 AnimOffset = Vector3.zero;
  protected bool showProgressBar = true;
  protected bool lightEfficiencyBonus = true;
  protected float minimumAttributeMultiplier = 0.5f;
  protected bool shouldTransferDiseaseWithWorker = true;
  [SerializeField]
  protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
  [SerializeField]
  protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  public bool triggerWorkReactions = true;
  public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;
  [SerializeField]
  [Tooltip("What layer does the dupe switch to when interacting with the building")]
  public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;
  [SerializeField]
  [Serialize]
  protected float workTimeRemaining = float.PositiveInfinity;
  [SerializeField]
  [Tooltip("Whether to user the KAnimSynchronizer or not")]
  public bool synchronizeAnims = true;
  private int skillsUpdateHandle = -1;
  [SerializeField]
  protected bool shouldShowSkillPerkStatusItem = true;
  public HashedString[] workAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  public HashedString workingPstComplete = (HashedString) "working_pst";
  public HashedString workingPstFailed = (HashedString) "working_pst";
  public float workTime;
  public bool alwaysShowProgressBar;
  protected StatusItem lightEfficiencyBonusStatusItem;
  protected Guid lightEfficiencyBonusStatusItemHandle;
  protected StatusItem workerStatusItem;
  protected StatusItem workingStatusItem;
  protected Guid workStatusItemHandle;
  protected OffsetTracker offsetTracker;
  [SerializeField]
  protected string attributeConverterId;
  protected AttributeConverter attributeConverter;
  public bool resetProgressOnStop;
  [SerializeField]
  protected string skillExperienceSkillGroup;
  [SerializeField]
  public KAnimFile[] overrideAnims;
  [SerializeField]
  protected HashedString multitoolContext;
  [SerializeField]
  protected Tag multitoolHitEffectTag;
  [SerializeField]
  [Tooltip("Whether to display number of uses in the details panel")]
  public bool trackUses;
  [Serialize]
  protected int numberOfUses;
  public System.Action<Workable.WorkableEvent> OnWorkableEventCB;
  public string requiredSkillPerk;
  protected StatusItem readyForSkillWorkStatusItem;
  public KAnim.PlayMode workAnimPlayMode;
  public bool faceTargetWhenWorking;
  protected ProgressBar progressBar;

  public Worker worker { get; protected set; }

  public float WorkTimeRemaining
  {
    get
    {
      return this.workTimeRemaining;
    }
    set
    {
      this.workTimeRemaining = value;
    }
  }

  public bool preferUnreservedCell { get; set; }

  public virtual float GetWorkTime()
  {
    return this.workTime;
  }

  public Worker GetWorker()
  {
    return this.worker;
  }

  public virtual float GetPercentComplete()
  {
    if ((double) this.workTimeRemaining <= (double) this.workTime)
      return (float) (1.0 - (double) this.workTimeRemaining / (double) this.workTime);
    return -1f;
  }

  public virtual Workable.AnimInfo GetAnim(Worker worker)
  {
    Workable.AnimInfo animInfo = new Workable.AnimInfo();
    if (this.overrideAnims != null && this.overrideAnims.Length > 0)
      animInfo.overrideAnims = this.overrideAnims;
    if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
      animInfo.smi = (StateMachine.Instance) new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
    return animInfo;
  }

  public virtual HashedString[] GetWorkAnims(Worker worker)
  {
    return this.workAnims;
  }

  public virtual KAnim.PlayMode GetWorkAnimPlayMode()
  {
    return this.workAnimPlayMode;
  }

  public virtual HashedString GetWorkPstAnim(Worker worker, bool successfully_completed)
  {
    if (successfully_completed)
      return this.workingPstComplete;
    return this.workingPstFailed;
  }

  public virtual Vector3 GetWorkOffset()
  {
    return Vector3.zero;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().MiscStatusItems.Using;
    this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
    this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
    this.workTime = this.GetWorkTime();
    this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
    {
      if (this.skillsUpdateHandle != -1)
        Game.Instance.Unsubscribe(this.skillsUpdateHandle);
      this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new System.Action<object>(this.UpdateStatusItem));
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
    this.lightEfficiencyBonusStatusItem = Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus;
    this.ShowProgressBar(this.alwaysShowProgressBar && (double) this.workTimeRemaining < (double) this.GetWorkTime());
    this.UpdateStatusItem((object) null);
  }

  protected virtual void UpdateStatusItem(object data = null)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.RemoveStatusItem(this.workStatusItemHandle, false);
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null)
    {
      if (!this.shouldShowSkillPerkStatusItem || string.IsNullOrEmpty(this.requiredSkillPerk))
        return;
      if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk))
        this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, (object) this.requiredSkillPerk);
      else
        this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, (object) this.requiredSkillPerk);
    }
    else
    {
      if (this.workingStatusItem == null)
        return;
      this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, (object) this);
    }
  }

  protected override void OnLoadLevel()
  {
    this.overrideAnims = (KAnimFile[]) null;
    base.OnLoadLevel();
  }

  public int GetCell()
  {
    return Grid.PosToCell((KMonoBehaviour) this);
  }

  public void StartWork(Worker worker_to_start)
  {
    Debug.Assert((UnityEngine.Object) worker_to_start != (UnityEngine.Object) null, (object) "How did we get a null worker?");
    this.worker = worker_to_start;
    this.UpdateStatusItem((object) null);
    if (this.showProgressBar)
      this.ShowProgressBar(true);
    this.OnStartWork(this.worker);
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
    {
      string conversationTopic = this.GetConversationTopic();
      if (conversationTopic != null)
        this.worker.Trigger(937885943, (object) conversationTopic);
    }
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(Workable.WorkableEvent.WorkStarted);
    ++this.numberOfUses;
  }

  public bool WorkTick(Worker worker, float dt)
  {
    bool flag = false;
    if ((double) dt > 0.0)
    {
      this.workTimeRemaining -= dt;
      flag = this.OnWorkTick(worker, dt);
    }
    if (!flag)
      return (double) this.workTimeRemaining < 0.0;
    return true;
  }

  public virtual float GetEfficiencyMultiplier(Worker worker)
  {
    float a = 1f;
    if (this.attributeConverter != null)
    {
      AttributeConverterInstance converter = worker.GetComponent<AttributeConverters>().GetConverter(this.attributeConverter.Id);
      a += converter.Evaluate();
    }
    if (this.lightEfficiencyBonus)
    {
      int cell = Grid.PosToCell(worker.gameObject);
      if (Grid.IsValidCell(cell))
      {
        if (Grid.LightIntensity[cell] > 0)
        {
          a += DUPLICANTSTATS.LIGHT.LIGHT_WORK_EFFICIENCY_BONUS;
          if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
            this.lightEfficiencyBonusStatusItemHandle = worker.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, (object) this);
        }
        else if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
          worker.GetComponent<KSelectable>().RemoveStatusItem(this.lightEfficiencyBonusStatusItemHandle, false);
      }
    }
    return Mathf.Max(a, this.minimumAttributeMultiplier);
  }

  public virtual Klei.AI.Attribute GetWorkAttribute()
  {
    if (this.attributeConverter != null)
      return this.attributeConverter.attribute;
    return (Klei.AI.Attribute) null;
  }

  public virtual string GetConversationTopic()
  {
    KPrefabID component = this.GetComponent<KPrefabID>();
    if (component.HasTag(GameTags.NotConversationTopic))
      return (string) null;
    return component.PrefabTag.Name;
  }

  public float GetAttributeExperienceMultiplier()
  {
    return this.attributeExperienceMultiplier;
  }

  public string GetSkillExperienceSkillGroup()
  {
    return this.skillExperienceSkillGroup;
  }

  public float GetSkillExperienceMultiplier()
  {
    return this.skillExperienceMultiplier;
  }

  protected virtual bool OnWorkTick(Worker worker, float dt)
  {
    return false;
  }

  public void StopWork(Worker workerToStop, bool aborted)
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) workerToStop && aborted)
      this.OnAbortWork(workerToStop);
    if (this.shouldTransferDiseaseWithWorker)
      this.TransferDiseaseWithWorker(workerToStop);
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(Workable.WorkableEvent.WorkStopped);
    this.OnStopWork(workerToStop);
    if (this.resetProgressOnStop)
      this.workTimeRemaining = this.GetWorkTime();
    this.ShowProgressBar(this.alwaysShowProgressBar && (double) this.workTimeRemaining < (double) this.GetWorkTime());
    if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
      this.lightEfficiencyBonusStatusItemHandle = workerToStop.GetComponent<KSelectable>().RemoveStatusItem(this.lightEfficiencyBonusStatusItemHandle, false);
    this.worker = (Worker) null;
    this.UpdateStatusItem((object) null);
  }

  public virtual StatusItem GetWorkerStatusItem()
  {
    return this.workerStatusItem;
  }

  public void SetWorkerStatusItem(StatusItem item)
  {
    this.workerStatusItem = item;
  }

  public void CompleteWork(Worker worker)
  {
    if (this.shouldTransferDiseaseWithWorker)
      this.TransferDiseaseWithWorker(worker);
    this.OnCompleteWork(worker);
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(Workable.WorkableEvent.WorkCompleted);
    if (this.OnWorkableEventCB != null)
      this.OnWorkableEventCB(Workable.WorkableEvent.WorkStopped);
    this.workTimeRemaining = this.GetWorkTime();
    this.ShowProgressBar(false);
  }

  public void SetReportType(ReportManager.ReportType report_type)
  {
    this.reportType = report_type;
  }

  public ReportManager.ReportType GetReportType()
  {
    return this.reportType;
  }

  protected virtual void OnStartWork(Worker worker)
  {
  }

  protected virtual void OnStopWork(Worker worker)
  {
  }

  protected virtual void OnCompleteWork(Worker worker)
  {
  }

  protected virtual void OnAbortWork(Worker worker)
  {
  }

  public void SetOffsets(CellOffset[] offsets)
  {
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    this.offsetTracker = (OffsetTracker) new StandardOffsetTracker(offsets);
  }

  public void SetOffsetTable(CellOffset[][] offset_table)
  {
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    this.offsetTracker = (OffsetTracker) new OffsetTableTracker(offset_table, (KMonoBehaviour) this);
  }

  public virtual CellOffset[] GetOffsets(int cell)
  {
    if (this.offsetTracker == null)
      this.offsetTracker = (OffsetTracker) new StandardOffsetTracker(new CellOffset[1]
      {
        new CellOffset()
      });
    return this.offsetTracker.GetOffsets(cell);
  }

  public CellOffset[] GetOffsets()
  {
    return this.GetOffsets(Grid.PosToCell((KMonoBehaviour) this));
  }

  public void SetWorkTime(float work_time)
  {
    this.workTime = work_time;
    this.workTimeRemaining = work_time;
  }

  public bool ShouldFaceTargetWhenWorking()
  {
    return this.faceTargetWhenWorking;
  }

  public virtual Vector3 GetFacingTarget()
  {
    return this.transform.GetPosition();
  }

  public void ShowProgressBar(bool show)
  {
    if (show)
    {
      if ((UnityEngine.Object) this.progressBar == (UnityEngine.Object) null)
        this.progressBar = ProgressBar.CreateProgressBar((KMonoBehaviour) this, new Func<float>(this.GetPercentComplete));
      this.progressBar.gameObject.SetActive(true);
    }
    else
    {
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.gameObject.DeleteObject();
      this.progressBar = (ProgressBar) null;
    }
  }

  protected override void OnCleanUp()
  {
    this.ShowProgressBar(false);
    if (this.offsetTracker != null)
      this.offsetTracker.Clear();
    if (this.skillsUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.skillsUpdateHandle);
    base.OnCleanUp();
    this.OnWorkableEventCB = (System.Action<Workable.WorkableEvent>) null;
  }

  public virtual Vector3 GetTargetPoint()
  {
    Vector3 vector3 = this.transform.GetPosition();
    float num = vector3.y + 0.65f;
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      vector3 = component.bounds.center;
    vector3.y = num;
    vector3.z = 0.0f;
    return vector3;
  }

  public int GetNavigationCost(Navigator navigator, int cell)
  {
    return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
  }

  public int GetNavigationCost(Navigator navigator)
  {
    return this.GetNavigationCost(navigator, Grid.PosToCell((KMonoBehaviour) this));
  }

  private void TransferDiseaseWithWorker(Worker worker)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    Workable.TransferDiseaseWithWorker(this.gameObject, worker.gameObject);
  }

  public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
  {
    if ((UnityEngine.Object) workable == (UnityEngine.Object) null || (UnityEngine.Object) worker == (UnityEngine.Object) null)
      return;
    PrimaryElement component1 = workable.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid;
    invalid1.idx = component2.DiseaseIdx;
    invalid1.count = (int) ((double) component2.DiseaseCount * 0.330000013113022);
    SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
    invalid2.idx = component1.DiseaseIdx;
    invalid2.count = (int) ((double) component1.DiseaseCount * 0.330000013113022);
    component2.ModifyDiseaseCount(-invalid1.count, "Workable.TransferDiseaseWithWorker");
    component1.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
    if (invalid1.count > 0)
      component1.AddDisease(invalid1.idx, invalid1.count, "Workable.TransferDiseaseWithWorker");
    if (invalid2.count <= 0)
      return;
    component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
  }

  public virtual List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.trackUses)
    {
      Descriptor descriptor = new Descriptor(string.Format((string) BUILDING.DETAILS.USE_COUNT, (object) this.numberOfUses), string.Format((string) BUILDING.DETAILS.USE_COUNT_TOOLTIP, (object) this.numberOfUses), Descriptor.DescriptorType.Detail, false);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  [ContextMenu("Refresh Reachability")]
  public void RefreshReachability()
  {
    if (this.offsetTracker == null)
      return;
    this.offsetTracker.ForceRefresh();
  }

  Transform IApproachable.get_transform()
  {
    return this.transform;
  }

  public enum WorkableEvent
  {
    WorkStarted,
    WorkCompleted,
    WorkStopped,
  }

  public struct AnimInfo
  {
    public KAnimFile[] overrideAnims;
    public StateMachine.Instance smi;
  }
}
