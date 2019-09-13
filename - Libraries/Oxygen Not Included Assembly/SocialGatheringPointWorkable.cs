// Decompiled with JetBrains decompiler
// Type: SocialGatheringPointWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

public class SocialGatheringPointWorkable : Workable, IWorkerPrioritizable
{
  private GameObject lastTalker;
  public int basePriority;
  public string specificEffect;

  private SocialGatheringPointWorkable()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_generic_convo_kanim")
    };
    this.workAnims = new HashedString[1]
    {
      (HashedString) "idle"
    };
    this.faceTargetWhenWorking = true;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
    this.synchronizeAnims = false;
    this.showProgressBar = false;
    this.resetProgressOnStop = true;
    this.lightEfficiencyBonus = false;
  }

  public override Vector3 GetFacingTarget()
  {
    if ((UnityEngine.Object) this.lastTalker != (UnityEngine.Object) null)
      return this.lastTalker.transform.GetPosition();
    return base.GetFacingTarget();
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    if (!worker.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation))
    {
      Effects component = worker.GetComponent<Effects>();
      if (string.IsNullOrEmpty(this.specificEffect) || component.HasEffect(this.specificEffect))
        return true;
    }
    return false;
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
    worker.Subscribe(-594200555, new System.Action<object>(this.OnStartedTalking));
    worker.Subscribe(25860745, new System.Action<object>(this.OnStoppedTalking));
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
    worker.Unsubscribe(-594200555, new System.Action<object>(this.OnStartedTalking));
    worker.Unsubscribe(25860745, new System.Action<object>(this.OnStoppedTalking));
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (string.IsNullOrEmpty(this.specificEffect))
      return;
    component.Add(this.specificEffect, true);
  }

  private void OnStartedTalking(object data)
  {
    ConversationManager.StartedTalkingEvent startedTalkingEvent = (ConversationManager.StartedTalkingEvent) data;
    GameObject talker = startedTalkingEvent.talker;
    if ((UnityEngine.Object) talker == (UnityEngine.Object) this.worker.gameObject)
    {
      KBatchedAnimController component = this.worker.GetComponent<KBatchedAnimController>();
      string str = startedTalkingEvent.anim + UnityEngine.Random.Range(1, 9).ToString();
      component.Play((HashedString) str, KAnim.PlayMode.Once, 1f, 0.0f);
      component.Queue((HashedString) "idle", KAnim.PlayMode.Loop, 1f, 0.0f);
    }
    else
    {
      this.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
      this.lastTalker = talker;
    }
  }

  private void OnStoppedTalking(object data)
  {
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
