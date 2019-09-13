// Decompiled with JetBrains decompiler
// Type: Schedulable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Schedulable : KMonoBehaviour
{
  public Schedule GetSchedule()
  {
    return ScheduleManager.Instance.GetSchedule(this);
  }

  public bool IsAllowed(ScheduleBlockType schedule_block_type)
  {
    if (!VignetteManager.Instance.Get().IsRedAlert())
      return ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
    return true;
  }

  public void OnScheduleChanged(Schedule schedule)
  {
    this.Trigger(467134493, (object) schedule);
  }

  public void OnScheduleBlocksTick(Schedule schedule)
  {
    this.Trigger(1714332666, (object) schedule);
  }

  public void OnScheduleBlocksChanged(Schedule schedule)
  {
    this.Trigger(-894023145, (object) schedule);
  }
}
