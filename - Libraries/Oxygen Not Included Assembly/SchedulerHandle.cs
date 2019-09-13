// Decompiled with JetBrains decompiler
// Type: SchedulerHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public struct SchedulerHandle
{
  public SchedulerEntry entry;
  private Scheduler scheduler;

  public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
  {
    this.entry = entry;
    this.scheduler = scheduler;
  }

  public float TimeRemaining
  {
    get
    {
      if (!this.IsValid)
        return -1f;
      return this.entry.time - this.scheduler.GetTime();
    }
  }

  public void FreeResources()
  {
    this.entry.FreeResources();
    this.scheduler = (Scheduler) null;
  }

  public void ClearScheduler()
  {
    if (this.scheduler == null)
      return;
    this.scheduler.Clear(this);
    this.scheduler = (Scheduler) null;
  }

  public bool IsValid
  {
    get
    {
      return this.scheduler != null;
    }
  }
}
