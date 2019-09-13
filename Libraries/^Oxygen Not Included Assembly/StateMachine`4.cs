// Decompiled with JetBrains decompiler
// Type: StateMachine`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine
  where StateMachineInstanceType : StateMachine.Instance
  where MasterType : IStateMachineTarget
{
  private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();
  public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;
  [StateMachine.DoNotAutoCreate]
  protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

  public override string[] GetStateNames()
  {
    List<string> stringList = new List<string>();
    foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
      stringList.Add(state.name);
    return stringList.ToArray();
  }

  public void Target(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
  {
    this.stateTarget = target;
  }

  public void BindState(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state,
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
    string state_name)
  {
    if (parent_state != null)
      state_name = parent_state.name + "." + state_name;
    state.name = state_name;
    state.longName = this.name + "." + state_name;
    state.debugPushName = "PuS: " + state.longName;
    state.debugPopName = "PoS: " + state.longName;
    state.debugExecuteName = "EA: " + state.longName;
    List<StateMachine.BaseState> baseStateList = parent_state == null ? new List<StateMachine.BaseState>() : new List<StateMachine.BaseState>((IEnumerable<StateMachine.BaseState>) parent_state.branch);
    baseStateList.Add((StateMachine.BaseState) state);
    state.parent = (StateMachine.BaseState) parent_state;
    state.branch = baseStateList.ToArray();
    this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
    this.states.Add(state);
  }

  public void BindStates(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state,
    object state_machine)
  {
    foreach (FieldInfo field in state_machine.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      if (field.FieldType.IsSubclassOf(typeof (StateMachine.BaseState)))
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) field.GetValue(state_machine);
        string name = field.Name;
        this.BindState(parent_state, state, name);
        this.BindStates(state, (object) state);
      }
    }
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    base.InitializeStates(out default_state);
  }

  public override void BindStates()
  {
    this.BindStates((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) null, (object) this);
  }

  public override System.Type GetStateMachineInstanceType()
  {
    return typeof (StateMachineInstanceType);
  }

  public override StateMachine.BaseState GetState(string state_name)
  {
    foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
    {
      if (state.name == state_name)
        return (StateMachine.BaseState) state;
    }
    return (StateMachine.BaseState) null;
  }

  public override void FreeResources()
  {
    for (int index = 0; index < this.states.Count; ++index)
      this.states[index].FreeResources();
    this.states.Clear();
    base.FreeResources();
  }

  public class GenericInstance : StateMachine.Instance
  {
    private int currentActionIdx = -1;
    private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();
    protected Stack<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context> transitionStack = new Stack<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context>();
    private float stateEnterTime;
    private int gotoId;
    private SchedulerHandle updateHandle;
    protected StateMachineController controller;
    private SchedulerGroup currentSchedulerGroup;
    private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

    public GenericInstance(MasterType master)
      : base((StateMachine) (object) Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>(), (IStateMachineTarget) master)
    {
      this.master = master;
      this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
      for (int index = 0; index < this.stateStack.Length; ++index)
        this.stateStack[index].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
      this.sm = (StateMachineType) this.stateMachine;
      this.dataTable = new object[this.GetStateMachine().dataTableSize];
      this.updateTable = new StateMachine.Instance.UpdateTableEntry[this.GetStateMachine().updateTableSize];
      this.controller = master.GetComponent<StateMachineController>();
      if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
        this.controller = master.gameObject.AddComponent<StateMachineController>();
      this.internalSm.masterTarget.Set(master.gameObject, this.smi);
      this.controller.AddStateMachineInstance((StateMachine.Instance) this);
    }

    public StateMachineType sm { get; private set; }

    protected StateMachineInstanceType smi
    {
      get
      {
        return (StateMachineInstanceType) this;
      }
    }

    public MasterType master { get; private set; }

    public DefType def { get; set; }

    public bool isMasterNull
    {
      get
      {
        return this.internalSm.masterTarget.IsNull((StateMachineInstanceType) this);
      }
    }

    private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
    {
      get
      {
        return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>) (object) this.sm;
      }
    }

    protected virtual void OnCleanUp()
    {
    }

    public override float timeinstate
    {
      get
      {
        return Time.time - this.stateEnterTime;
      }
    }

    public override void FreeResources()
    {
      this.updateHandle.FreeResources();
      this.updateHandle = new SchedulerHandle();
      this.controller = (StateMachineController) null;
      if (this.gotoStack != null)
        this.gotoStack.Clear();
      this.gotoStack = (Stack<StateMachine.BaseState>) null;
      if (this.transitionStack != null)
        this.transitionStack.Clear();
      this.transitionStack = (Stack<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context>) null;
      if (this.currentSchedulerGroup != null)
        this.currentSchedulerGroup.FreeResources();
      this.currentSchedulerGroup = (SchedulerGroup) null;
      if (this.stateStack != null)
      {
        for (int index = 0; index < this.stateStack.Length; ++index)
        {
          if (this.stateStack[index].schedulerGroup != null)
            this.stateStack[index].schedulerGroup.FreeResources();
        }
      }
      this.stateStack = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[]) null;
      base.FreeResources();
    }

    public override IStateMachineTarget GetMaster()
    {
      return (IStateMachineTarget) this.master;
    }

    private void PushEvent(StateEvent evt)
    {
      this.subscribedEvents.Push(evt.Subscribe((StateMachine.Instance) this));
    }

    private void PopEvent()
    {
      StateEvent.Context context = this.subscribedEvents.Pop();
      context.stateEvent.Unsubscribe((StateMachine.Instance) this, context);
    }

    private void PushTransition(
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition transition)
    {
      this.transitionStack.Push(transition.Register(this.smi));
    }

    private void PopTransition(
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
    {
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context context = this.transitionStack.Pop();
      ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition) state.transitions[context.idx]).Unregister(this.smi, context);
    }

    private void PushState(StateMachine.BaseState state)
    {
      int gotoId = this.gotoId;
      this.currentActionIdx = -1;
      if (state.events != null)
      {
        foreach (StateEvent evt in state.events)
          this.PushEvent(evt);
      }
      if (state.transitions != null)
      {
        foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition transition in state.transitions)
          this.PushTransition(transition);
      }
      if (state.parameterTransitions != null)
      {
        foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition parameterTransition in state.parameterTransitions)
          parameterTransition.Register(this.smi);
      }
      if (state.updateActions != null)
      {
        for (int index = 0; index < state.updateActions.Count; ++index)
        {
          StateMachine.UpdateAction updateAction = state.updateActions[index];
          int updateTableIdx = updateAction.updateTableIdx;
          int nextBucketIdx = updateAction.nextBucketIdx;
          updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
          UpdateBucketWithUpdater<StateMachineInstanceType> bucket = (UpdateBucketWithUpdater<StateMachineInstanceType>) updateAction.buckets[nextBucketIdx];
          this.smi.updateTable[updateTableIdx].bucket = (StateMachineUpdater.BaseUpdateBucket) bucket;
          this.smi.updateTable[updateTableIdx].handle = bucket.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, bucket.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater) updateAction.updater);
          state.updateActions[index] = updateAction;
        }
      }
      this.stateEnterTime = Time.time;
      this.stateStack[this.stackSize++].state = state;
      this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
      if (state.transitions != null)
      {
        foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition transition in state.transitions)
        {
          if (gotoId != this.gotoId)
            return;
          transition.Evaluate(this.smi);
        }
      }
      if (state.parameterTransitions != null)
      {
        foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition parameterTransition in state.parameterTransitions)
        {
          if (gotoId != this.gotoId)
            return;
          parameterTransition.Evaluate(this.smi);
        }
      }
      if (gotoId != this.gotoId)
        return;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state, state.enterActions);
      if (gotoId == this.gotoId)
        ;
    }

    private void ExecuteActions(
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
      List<StateMachine.Action> actions)
    {
      if (actions == null)
        return;
      int gotoId = this.gotoId;
      for (++this.currentActionIdx; this.currentActionIdx < actions.Count && gotoId == this.gotoId; ++this.currentActionIdx)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback) actions[this.currentActionIdx].callback;
        try
        {
          callback(this.smi);
        }
        catch (Exception ex)
        {
          if (!StateMachine.Instance.error)
          {
            this.Error();
            string str = "(NULL).";
            IStateMachineTarget master = this.GetMaster();
            if (!master.isNull)
            {
              KPrefabID component = master.GetComponent<KPrefabID>();
              str = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? "(" + this.gameObject.name + ")." : "(" + component.PrefabTag.ToString() + ").";
            }
            DebugUtil.LogErrorArgs((UnityEngine.Object) this.controller, (object) ("Exception in: " + str + this.stateMachine.ToString() + "." + state.name + "." + actions[this.currentActionIdx].name + "\n" + ex.ToString()));
          }
        }
      }
      this.currentActionIdx = 2147483646;
    }

    private void PopState()
    {
      this.currentActionIdx = -1;
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry state1 = this.stateStack[--this.stackSize];
      StateMachine.BaseState state2 = state1.state;
      if (state2.parameterTransitions != null)
      {
        foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition parameterTransition in state2.parameterTransitions)
          parameterTransition.Unregister(this.smi);
      }
      if (state2.transitions != null)
      {
        for (int index = 0; index < state2.transitions.Count; ++index)
          this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state2);
      }
      if (state2.events != null)
      {
        for (int index = 0; index < state2.events.Count; ++index)
          this.PopEvent();
      }
      if (state2.updateActions != null)
      {
        foreach (StateMachine.UpdateAction updateAction in state2.updateActions)
        {
          int updateTableIdx = updateAction.updateTableIdx;
          UpdateBucketWithUpdater<StateMachineInstanceType> bucket = (UpdateBucketWithUpdater<StateMachineInstanceType>) this.smi.updateTable[updateTableIdx].bucket;
          this.smi.updateTable[updateTableIdx].bucket = (StateMachineUpdater.BaseUpdateBucket) null;
          bucket.Remove(this.smi.updateTable[updateTableIdx].handle);
        }
      }
      state1.schedulerGroup.Reset();
      this.currentSchedulerGroup = state1.schedulerGroup;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state2, state2.exitActions);
    }

    public override SchedulerHandle Schedule(
      float time,
      System.Action<object> callback,
      object callback_data = null)
    {
      return Singleton<StateMachineManager>.Instance.Schedule(this.GetCurrentState().longName, time, callback, callback_data, this.currentSchedulerGroup);
    }

    public override void StartSM()
    {
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && !this.controller.HasStateMachineInstance((StateMachine.Instance) this))
        this.controller.AddStateMachineInstance((StateMachine.Instance) this);
      base.StartSM();
    }

    public override void StopSM(string reason)
    {
      if (StateMachine.Instance.error)
        return;
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
        this.controller.RemoveStateMachineInstance((StateMachine.Instance) this);
      if (!this.IsRunning())
        return;
      ++this.gotoId;
      while (this.stackSize > 0)
        this.PopState();
      if ((object) this.master != null && (UnityEngine.Object) this.controller != (UnityEngine.Object) null)
        this.controller.RemoveStateMachineInstance((StateMachine.Instance) this);
      if (this.status == StateMachine.Status.Running)
        this.SetStatus(StateMachine.Status.Failed);
      if (this.OnStop != null)
        this.OnStop(reason, this.status);
      for (int index = 0; index < this.parameterContexts.Length; ++index)
        this.parameterContexts[index].Cleanup();
      this.OnCleanUp();
    }

    private void FinishStateInProgress(StateMachine.BaseState state)
    {
      if (state.enterActions == null)
        return;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state, state.enterActions);
    }

    public override void GoTo(StateMachine.BaseState base_state)
    {
      if (App.IsExiting || StateMachine.Instance.error)
        return;
      if (this.isMasterNull)
        return;
      try
      {
        if (this.IsBreakOnGoToEnabled())
          Debugger.Break();
        if (base_state != null)
        {
          while (base_state.defaultState != null)
            base_state = base_state.defaultState;
        }
        if (this.GetCurrentState() == null)
          this.SetStatus(StateMachine.Status.Running);
        if (this.gotoStack.Count > 100)
        {
          string str = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
          foreach (StateMachine.BaseState baseState in this.gotoStack)
            str = str + "\n" + baseState.name;
          Debug.LogError((object) str);
          this.Error();
        }
        else
        {
          this.gotoStack.Push(base_state);
          if (base_state == null)
          {
            this.StopSM("StateMachine.GoTo(null)");
            this.gotoStack.Pop();
          }
          else
          {
            int num = ++this.gotoId;
            StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
            int index1 = 0;
            while (index1 < this.stackSize && (index1 < branch.Length && this.stateStack[index1].state == branch[index1]))
              ++index1;
            int index2 = this.stackSize - 1;
            if (index2 >= 0 && index2 == index1 - 1)
              this.FinishStateInProgress(this.stateStack[index2].state);
            while (this.stackSize > index1 && num == this.gotoId)
              this.PopState();
            for (int index3 = index1; index3 < branch.Length && num == this.gotoId; ++index3)
              this.PushState(branch[index3]);
            this.gotoStack.Pop();
          }
        }
      }
      catch (Exception ex)
      {
        if (StateMachine.Instance.error)
          return;
        this.Error();
        string str1 = "(Stop)";
        if (base_state != null)
          str1 = base_state.name;
        string str2 = "(NULL).";
        if (!this.GetMaster().isNull)
          str2 = "(" + this.gameObject.name + ").";
        DebugUtil.LogErrorArgs((UnityEngine.Object) this.controller, (object) ("Exception in: " + str2 + this.stateMachine.ToString() + ".GoTo(" + str1 + ")" + "\n" + ex.ToString()));
      }
    }

    public override StateMachine.BaseState GetCurrentState()
    {
      if (this.stackSize > 0)
        return this.stateStack[this.stackSize - 1].state;
      return (StateMachine.BaseState) null;
    }

    public struct StackEntry
    {
      public StateMachine.BaseState state;
      public SchedulerGroup schedulerGroup;
    }
  }

  public class State : StateMachine.BaseState
  {
    protected StateMachineType sm;

    public delegate void Callback(StateMachineInstanceType smi)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;
  }

  public abstract class ParameterTransition : StateMachine.ParameterTransition
  {
    public abstract void Evaluate(StateMachineInstanceType smi);

    public abstract void Register(StateMachineInstanceType smi);

    public abstract void Unregister(StateMachineInstanceType smi);
  }

  public class Transition : StateMachine.BaseTransition
  {
    public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;
    public int idx;

    public Transition(
      string name,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state,
      int idx,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition)
      : base(name, (StateMachine.BaseState) source_state, (StateMachine.BaseState) target_state)
    {
      this.condition = condition;
      this.idx = idx;
    }

    public virtual void Evaluate(StateMachineInstanceType smi)
    {
    }

    public virtual StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context Register(
      StateMachineInstanceType smi)
    {
      return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context(this);
    }

    public virtual void Unregister(
      StateMachineInstanceType smi,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.Context context)
    {
    }

    public delegate bool ConditionCallback(StateMachineInstanceType smi)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;

    public struct Context
    {
      public int idx;
      public int handlerId;

      public Context(
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition transition)
      {
        this.idx = transition.idx;
        this.handlerId = 0;
      }
    }
  }

  public abstract class Parameter<ParameterType> : StateMachine.Parameter
  {
    public ParameterType defaultValue;
    public bool isSignal;

    public Parameter()
    {
    }

    public Parameter(ParameterType default_value)
    {
      this.defaultValue = default_value;
    }

    public ParameterType Set(ParameterType value, StateMachineInstanceType smi)
    {
      ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).Set(value, smi);
      return value;
    }

    public ParameterType Get(StateMachineInstanceType smi)
    {
      return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).value;
    }

    public delegate bool Callback(StateMachineInstanceType smi, ParameterType p)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;

    public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
    {
      private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;
      private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
      private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state;

      public Transition(
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback)
      {
        this.parameter = parameter;
        this.callback = callback;
        this.state = state;
      }

      public override void Evaluate(StateMachineInstanceType smi)
      {
        if (this.parameter.isSignal && this.callback == null)
          return;
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (!this.callback(smi, parameterContext.value))
          return;
        smi.GoTo((StateMachine.BaseState) this.state);
      }

      private void Trigger(StateMachineInstanceType smi)
      {
        smi.GoTo((StateMachine.BaseState) this.state);
      }

      public override void Register(StateMachineInstanceType smi)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (this.parameter.isSignal && this.callback == null)
          parameterContext.onDirty += new System.Action<StateMachineInstanceType>(this.Trigger);
        else
          parameterContext.onDirty += new System.Action<StateMachineInstanceType>(((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition) this).Evaluate);
      }

      public override void Unregister(StateMachineInstanceType smi)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (this.parameter.isSignal && this.callback == null)
          parameterContext.onDirty -= new System.Action<StateMachineInstanceType>(this.Trigger);
        else
          parameterContext.onDirty -= new System.Action<StateMachineInstanceType>(((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition) this).Evaluate);
      }

      public override string ToString()
      {
        if (this.state != null)
          return this.parameter.name + "->" + this.state.name;
        return this.parameter.name + "->(Stop)";
      }
    }

    public abstract class Context : StateMachine.Parameter.Context
    {
      public ParameterType value;
      public System.Action<StateMachineInstanceType> onDirty;

      public Context(StateMachine.Parameter parameter, ParameterType default_value)
        : base(parameter)
      {
        this.value = default_value;
      }

      public virtual void Set(ParameterType value, StateMachineInstanceType smi)
      {
        if (EqualityComparer<ParameterType>.Default.Equals(value, this.value))
          return;
        this.value = value;
        if (this.onDirty == null)
          return;
        this.onDirty(smi);
      }
    }
  }

  public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
  {
    public BoolParameter()
    {
    }

    public BoolParameter(bool default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
    {
      public Context(StateMachine.Parameter parameter, bool default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(!this.value ? (byte) 0 : (byte) 1);
      }

      public override void Deserialize(IReader reader)
      {
        this.value = reader.ReadByte() != (byte) 0;
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
  {
    public Vector3Parameter()
    {
    }

    public Vector3Parameter(Vector3 default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
    {
      public Context(StateMachine.Parameter parameter, Vector3 default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value.x);
        writer.Write(this.value.y);
        writer.Write(this.value.z);
      }

      public override void Deserialize(IReader reader)
      {
        this.value.x = reader.ReadSingle();
        this.value.y = reader.ReadSingle();
        this.value.z = reader.ReadSingle();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
  {
    public EnumParameter(EnumType default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
    {
      public Context(StateMachine.Parameter parameter, EnumType default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        writer.Write((int) (object) this.value);
      }

      public override void Deserialize(IReader reader)
      {
        this.value = (EnumType) (ValueType) reader.ReadInt32();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
  {
    public FloatParameter()
    {
    }

    public FloatParameter(float default_value)
      : base(default_value)
    {
    }

    public float Delta(float delta_value, StateMachineInstanceType smi)
    {
      float num1 = this.Get(smi) + delta_value;
      double num2 = (double) this.Set(num1, smi);
      return num1;
    }

    public float DeltaClamp(
      float delta_value,
      float min_value,
      float max_value,
      StateMachineInstanceType smi)
    {
      float num1 = Mathf.Clamp(this.Get(smi) + delta_value, min_value, max_value);
      double num2 = (double) this.Set(num1, smi);
      return num1;
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
    {
      public Context(StateMachine.Parameter parameter, float default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value);
      }

      public override void Deserialize(IReader reader)
      {
        this.value = reader.ReadSingle();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
  {
    public IntParameter()
    {
    }

    public IntParameter(int default_value)
      : base(default_value)
    {
    }

    public int Delta(int delta_value, StateMachineInstanceType smi)
    {
      int num = this.Get(smi) + delta_value;
      this.Set(num, smi);
      return num;
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
    {
      public Context(StateMachine.Parameter parameter, int default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value);
      }

      public override void Deserialize(IReader reader)
      {
        this.value = reader.ReadInt32();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>
    where ResourceType : Resource
  {
    public ResourceParameter()
      : base((ResourceType) null)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
    {
      public Context(StateMachine.Parameter parameter, ResourceType default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
        string str = string.Empty;
        if ((object) this.value != null)
        {
          if (this.value.Guid == (ResourceGuid) null)
            Debug.LogError((object) ("Cannot serialize resource with invalid guid: " + this.value.Id));
          else
            str = this.value.Guid.Guid;
        }
        writer.WriteKleiString(str);
      }

      public override void Deserialize(IReader reader)
      {
        string id = reader.ReadKleiString();
        if (!(id != string.Empty))
          return;
        this.value = Db.Get().GetResource<ResourceType>(new ResourceGuid(id, (Resource) null));
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>
    where ObjectType : class
  {
    public ObjectParameter()
      : base((ObjectType) null)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
    {
      public Context(StateMachine.Parameter parameter, ObjectType default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
      }

      public override void Deserialize(IReader reader)
      {
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
  {
    public TargetParameter()
      : base((GameObject) null)
    {
    }

    public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
    {
      GameObject go = this.Get(smi);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        SMT smi1 = go.GetSMI<SMT>();
        if ((object) smi1 != null)
          return smi1;
        Debug.LogError((object) (go.name + " does not have state machine " + typeof (StateMachineType).Name));
      }
      return (SMT) null;
    }

    public bool IsNull(StateMachineInstanceType smi)
    {
      return (UnityEngine.Object) this.Get(smi) == (UnityEngine.Object) null;
    }

    public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
    {
      GameObject gameObject = this.Get(smi);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        ComponentType component = gameObject.GetComponent<ComponentType>();
        if ((object) component != null)
          return component;
        Debug.LogError((object) (gameObject.name + " does not have component " + typeof (ComponentType).Name));
      }
      return default (ComponentType);
    }

    public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
    {
      GameObject gameObject = (GameObject) null;
      if ((UnityEngine.Object) value != (UnityEngine.Object) null)
        gameObject = value.gameObject;
      this.Set(gameObject, smi);
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
    {
      private StateMachineInstanceType smi;
      private int objectDestroyedHandler;

      public Context(StateMachine.Parameter parameter, GameObject default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
      }

      public override void Deserialize(IReader reader)
      {
      }

      public override void Cleanup()
      {
        base.Cleanup();
        if (!((UnityEngine.Object) this.value != (UnityEngine.Object) null))
          return;
        this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
        this.objectDestroyedHandler = 0;
      }

      public override void Set(GameObject value, StateMachineInstanceType smi)
      {
        this.smi = smi;
        if ((UnityEngine.Object) this.value != (UnityEngine.Object) null)
        {
          this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
          this.objectDestroyedHandler = 0;
        }
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
          this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, new System.Action<object>(this.OnObjectDestroyed));
        base.Set(value, smi);
      }

      private void OnObjectDestroyed(object data)
      {
        this.Set((GameObject) null, this.smi);
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }

  public class SignalParameter
  {
  }

  public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
  {
    public Signal()
      : base((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter) null)
    {
      this.isSignal = true;
    }

    public void Trigger(StateMachineInstanceType smi)
    {
      ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).Set((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter) null, smi);
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
    {
      public Context(
        StateMachine.Parameter parameter,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value)
        : base(parameter, default_value)
      {
      }

      public override void Serialize(BinaryWriter writer)
      {
      }

      public override void Deserialize(IReader reader)
      {
      }

      public override void Set(
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value,
        StateMachineInstanceType smi)
      {
        if (this.onDirty == null)
          return;
        this.onDirty(smi);
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }
    }
  }
}
