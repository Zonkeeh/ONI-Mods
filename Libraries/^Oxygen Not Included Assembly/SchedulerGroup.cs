// Decompiled with JetBrains decompiler
// Type: SchedulerGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SchedulerGroup
{
  private List<SchedulerHandle> handles = new List<SchedulerHandle>();

  public SchedulerGroup(Scheduler scheduler)
  {
    this.scheduler = scheduler;
    this.Reset();
  }

  public Scheduler scheduler { get; private set; }

  public void FreeResources()
  {
    if (this.scheduler != null)
      this.scheduler.FreeResources();
    this.scheduler = (Scheduler) null;
    if (this.handles != null)
      this.handles.Clear();
    this.handles = (List<SchedulerHandle>) null;
  }

  public void Reset()
  {
    foreach (SchedulerHandle handle in this.handles)
      handle.ClearScheduler();
    this.handles.Clear();
  }

  public void Add(SchedulerHandle handle)
  {
    this.handles.Add(handle);
  }
}
