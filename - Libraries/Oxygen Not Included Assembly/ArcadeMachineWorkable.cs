// Decompiled with JetBrains decompiler
// Type: ArcadeMachineWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;

public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
  private static string specificEffect = "PlayedArcade";
  private static string trackingEffect = "RecentlyPlayedArcade";
  public int basePriority = RELAXATION.PRIORITY.TIER4;
  public ArcadeMachine owner;

  private ArcadeMachineWorkable()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.SetWorkTime(15f);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
      component.Add(ArcadeMachineWorkable.trackingEffect, true);
    if (string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
      return;
    component.Add(ArcadeMachineWorkable.specificEffect, true);
  }

  public bool GetWorkerPriority(Worker worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
