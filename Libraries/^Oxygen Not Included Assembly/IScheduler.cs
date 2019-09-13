// Decompiled with JetBrains decompiler
// Type: IScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IScheduler
{
  SchedulerHandle Schedule(
    string name,
    float time,
    System.Action<object> callback,
    object callback_data = null,
    SchedulerGroup group = null);
}
