// Decompiled with JetBrains decompiler
// Type: FetchableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FetchableMonitor : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance>
{
  public StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter forceUnfetchable = new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);
  public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State fetchable;
  public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State unfetchable;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.unfetchable;
    this.serializable = false;
    this.fetchable.Enter("RegisterFetchable", (StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RegisterFetchable())).Exit("UnregisterFetchable", (StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.UnregisterFetchable())).EventTransition(GameHashes.ReachableChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.AssigneeChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.EntombedChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.TagsChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventHandler(GameHashes.OnStore, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.StoragePriorityChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.TagsChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateTags)).ParamTransition<bool>((StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.forceUnfetchable, this.unfetchable, (StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback) ((smi, p) => !smi.IsFetchable()));
    this.unfetchable.EventTransition(GameHashes.ReachableChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.AssigneeChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.EntombedChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.TagsChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).ParamTransition<bool>((StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.forceUnfetchable, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback(this.IsFetchable));
  }

  private bool IsFetchable(FetchableMonitor.Instance smi, bool param)
  {
    return this.IsFetchable(smi);
  }

  private bool IsFetchable(FetchableMonitor.Instance smi)
  {
    return smi.IsFetchable();
  }

  private void UpdateStorage(FetchableMonitor.Instance smi, object data)
  {
    Game.Instance.fetchManager.UpdateStorage(smi.pickupable.PrefabID(), smi.fetchable, data as Storage);
  }

  private void UpdateTags(FetchableMonitor.Instance smi, object data)
  {
    Game.Instance.fetchManager.UpdateTags(smi.pickupable.PrefabID(), smi.fetchable);
  }

  public class Instance : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Pickupable pickupable;
    private Equippable equippable;
    public HandleVector<int>.Handle fetchable;

    public Instance(Pickupable pickupable)
      : base((IStateMachineTarget) pickupable)
    {
      this.pickupable = pickupable;
      this.equippable = this.GetComponent<Equippable>();
    }

    public void RegisterFetchable()
    {
      Game.Instance.Trigger(-1588644844, (object) this.gameObject);
      this.fetchable = Game.Instance.fetchManager.Add(this.pickupable);
    }

    public void UnregisterFetchable()
    {
      Game.Instance.Trigger(-1491270284, (object) this.gameObject);
      Game.Instance.fetchManager.Remove(this.smi.pickupable.PrefabID(), this.fetchable);
    }

    public void SetForceUnfetchable(bool is_unfetchable)
    {
      this.sm.forceUnfetchable.Set(is_unfetchable, this.smi);
    }

    public bool IsFetchable()
    {
      return !this.sm.forceUnfetchable.Get(this) && !this.pickupable.IsEntombed && this.pickupable.IsReachable() && ((!((Object) this.equippable != (Object) null) || !this.equippable.isEquipped) && !this.pickupable.HasTag(GameTags.StoredPrivate));
    }
  }
}
