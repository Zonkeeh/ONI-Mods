// Decompiled with JetBrains decompiler
// Type: StateMachineManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StateMachineManager : Singleton<StateMachineManager>, IScheduler
{
  private static object[] parameters = new object[2];
  private Dictionary<System.Type, StateMachine> stateMachines = new Dictionary<System.Type, StateMachine>();
  private Dictionary<System.Type, List<System.Action<StateMachine>>> stateMachineCreatedCBs = new Dictionary<System.Type, List<System.Action<StateMachine>>>();
  private Scheduler scheduler;

  public void RegisterScheduler(Scheduler scheduler)
  {
    this.scheduler = scheduler;
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

  public SchedulerGroup CreateSchedulerGroup()
  {
    return new SchedulerGroup(this.scheduler);
  }

  public StateMachine CreateStateMachine(System.Type type)
  {
    StateMachine stateMachine = (StateMachine) null;
    if (!this.stateMachines.TryGetValue(type, out stateMachine))
    {
      stateMachine = (StateMachine) Activator.CreateInstance(type);
      stateMachine.CreateStates((object) stateMachine);
      stateMachine.BindStates();
      stateMachine.InitializeStateMachine();
      this.stateMachines[type] = stateMachine;
      List<System.Action<StateMachine>> actionList;
      if (this.stateMachineCreatedCBs.TryGetValue(type, out actionList))
      {
        foreach (System.Action<StateMachine> action in actionList)
          action(stateMachine);
      }
    }
    return stateMachine;
  }

  public T CreateStateMachine<T>()
  {
    return (T) this.CreateStateMachine(typeof (T));
  }

  public static void ResetParameters()
  {
    for (int index = 0; index < StateMachineManager.parameters.Length; ++index)
      StateMachineManager.parameters[index] = (object) null;
  }

  public StateMachine.Instance CreateSMIFromDef(
    IStateMachineTarget master,
    StateMachine.BaseDef def)
  {
    StateMachineManager.parameters[0] = (object) master;
    StateMachineManager.parameters[1] = (object) def;
    return (StateMachine.Instance) Activator.CreateInstance(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType(), StateMachineManager.parameters);
  }

  public void Clear()
  {
    if (this.scheduler != null)
      this.scheduler.FreeResources();
    if (this.stateMachines == null)
      return;
    this.stateMachines.Clear();
  }

  public void AddStateMachineCreatedCallback(System.Type sm_type, System.Action<StateMachine> cb)
  {
    List<System.Action<StateMachine>> actionList;
    if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out actionList))
    {
      actionList = new List<System.Action<StateMachine>>();
      this.stateMachineCreatedCBs[sm_type] = actionList;
    }
    actionList.Add(cb);
  }

  public void RemoveStateMachineCreatedCallback(System.Type sm_type, System.Action<StateMachine> cb)
  {
    List<System.Action<StateMachine>> actionList;
    if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out actionList))
      return;
    actionList.Remove(cb);
  }
}
