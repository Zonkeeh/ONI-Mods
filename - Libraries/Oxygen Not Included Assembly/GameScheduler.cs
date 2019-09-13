// Decompiled with JetBrains decompiler
// Type: GameScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GameScheduler : KMonoBehaviour, IScheduler
{
  private Scheduler scheduler = new Scheduler((SchedulerClock) new GameScheduler.GameSchedulerClock());
  public static GameScheduler Instance;

  public static void DestroyInstance()
  {
    GameScheduler.Instance = (GameScheduler) null;
  }

  protected override void OnPrefabInit()
  {
    GameScheduler.Instance = this;
    Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
  }

  public SchedulerHandle Schedule(
    string name,
    float time,
    System.Action<object> callback,
    object callback_data = null,
    SchedulerGroup group = null)
  {
    return this.scheduler.Schedule(name, time, callback, callback_data, group);
  }

  private void Update()
  {
    this.scheduler.Update();
  }

  protected override void OnLoadLevel()
  {
    this.scheduler.FreeResources();
    this.scheduler = (Scheduler) null;
  }

  public SchedulerGroup CreateGroup()
  {
    return new SchedulerGroup(this.scheduler);
  }

  public Scheduler GetScheduler()
  {
    return this.scheduler;
  }

  public class GameSchedulerClock : SchedulerClock
  {
    public override float GetTime()
    {
      return GameClock.Instance.GetTime();
    }
  }
}
