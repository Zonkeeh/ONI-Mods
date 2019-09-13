// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

public class ComplexFabricatorWorkable : Workable
{
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private ComplexFabricator fabricator;
  public System.Action<Worker, float> OnWorkTickActions;
  public MeterController meter;
  protected GameObject visualizer;
  protected KAnimLink visualizerLink;

  public StatusItem WorkerStatusItem
  {
    get
    {
      return this.workerStatusItem;
    }
    set
    {
      this.workerStatusItem = value;
    }
  }

  public AttributeConverter AttributeConverter
  {
    get
    {
      return this.attributeConverter;
    }
    set
    {
      this.attributeConverter = value;
    }
  }

  public float AttributeExperienceMultiplier
  {
    get
    {
      return this.attributeExperienceMultiplier;
    }
    set
    {
      this.attributeExperienceMultiplier = value;
    }
  }

  public string SkillExperienceSkillGroup
  {
    set
    {
      this.skillExperienceSkillGroup = value;
    }
  }

  public float SkillExperienceMultiplier
  {
    set
    {
      this.skillExperienceMultiplier = value;
    }
  }

  public ComplexRecipe CurrentWorkingOrder
  {
    get
    {
      if ((UnityEngine.Object) this.fabricator != (UnityEngine.Object) null)
        return this.fabricator.CurrentWorkingOrder;
      return (ComplexRecipe) null;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
  }

  public override string GetConversationTopic()
  {
    return this.fabricator.GetConversationTopic() ?? base.GetConversationTopic();
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    if (!this.operational.IsOperational)
      return;
    if (this.fabricator.CurrentWorkingOrder != null)
      this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
    else
      DebugUtil.DevAssertArgs(false, (object) "ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null", (object) this.gameObject);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (this.OnWorkTickActions != null)
      this.OnWorkTickActions(worker, dt);
    this.UpdateOrderProgress(worker, dt);
    return base.OnWorkTick(worker, dt);
  }

  public override float GetWorkTime()
  {
    ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
    if (currentWorkingOrder == null)
      return -1f;
    this.workTime = currentWorkingOrder.time;
    return this.workTime;
  }

  public Chore CreateWorkChore(ChoreType choreType, float order_progress)
  {
    WorkChore<ComplexFabricatorWorkable> workChore = new WorkChore<ComplexFabricatorWorkable>(choreType, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
    return (Chore) workChore;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.fabricator.CompleteWorkingOrder();
    this.DestroyVisualizer();
  }

  private void InstantiateVisualizer(ComplexRecipe recipe)
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.DestroyVisualizer();
    if (this.visualizerLink != null)
    {
      this.visualizerLink.Unregister();
      this.visualizerLink = (KAnimLink) null;
    }
    if ((UnityEngine.Object) recipe.FabricationVisualizer == (UnityEngine.Object) null)
      return;
    this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, (GameObject) null, (string) null);
    this.visualizer.transform.parent = this.meter.meterController.transform;
    this.visualizer.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, 1f));
    this.visualizer.SetActive(true);
    this.visualizerLink = new KAnimLink((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), (KAnimControllerBase) this.visualizer.GetComponent<KBatchedAnimController>());
  }

  private void UpdateOrderProgress(Worker worker, float dt)
  {
    float workTime = this.GetWorkTime();
    float percent_full = Mathf.Clamp01((workTime - this.WorkTimeRemaining) / workTime);
    if ((bool) ((UnityEngine.Object) this.fabricator))
      this.fabricator.OrderProgress = percent_full;
    if (this.meter == null)
      return;
    this.meter.SetPositionPercent(percent_full);
  }

  private void DestroyVisualizer()
  {
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    if (this.visualizerLink != null)
    {
      this.visualizerLink.Unregister();
      this.visualizerLink = (KAnimLink) null;
    }
    Util.KDestroyGameObject(this.visualizer);
    this.visualizer = (GameObject) null;
  }
}
