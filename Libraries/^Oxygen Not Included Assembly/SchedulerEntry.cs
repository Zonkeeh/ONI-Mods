// Decompiled with JetBrains decompiler
// Type: SchedulerEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct SchedulerEntry
{
  public float time;

  public SchedulerEntry(
    string name,
    float time,
    float time_interval,
    System.Action<object> callback,
    object callback_data,
    GameObject profiler_obj)
  {
    this.time = time;
    this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
  }

  public SchedulerEntry.Details details { get; private set; }

  public void FreeResources()
  {
    this.details = (SchedulerEntry.Details) null;
  }

  public System.Action<object> callback
  {
    get
    {
      return this.details.callback;
    }
  }

  public object callbackData
  {
    get
    {
      return this.details.callbackData;
    }
  }

  public float timeInterval
  {
    get
    {
      return this.details.timeInterval;
    }
  }

  public override string ToString()
  {
    return this.time.ToString();
  }

  public void Clear()
  {
    this.details.callback = (System.Action<object>) null;
  }

  public class Details
  {
    public System.Action<object> callback;
    public object callbackData;
    public float timeInterval;

    public Details(
      string name,
      System.Action<object> callback,
      object callback_data,
      float time_interval,
      GameObject profiler_obj)
    {
      this.timeInterval = time_interval;
      this.callback = callback;
      this.callbackData = callback_data;
    }
  }
}
