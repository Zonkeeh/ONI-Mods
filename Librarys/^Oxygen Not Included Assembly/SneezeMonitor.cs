// Decompiled with JetBrains decompiler
// Type: SneezeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
  public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;
  public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;
  public const float SINGLE_SNEEZE_TIME = 70f;
  public const float SNEEZE_TIME_VARIANCE = 0.3f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.ParamTransition<bool>((StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isSneezy, this.sneezy, (StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback) ((smi, p) => p));
    this.sneezy.ParamTransition<bool>((StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isSneezy, this.idle, (StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback) ((smi, p) => !p)).ToggleReactable((Func<SneezeMonitor.Instance, Reactable>) (smi => smi.GetReactable()));
  }

  public class Instance : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private StatusItem statusItem;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      AttributeInstance attributeInstance = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
      this.OnSneezyChange();
      attributeInstance.OnDirty += new System.Action(this.OnSneezyChange);
    }

    public override void StopSM(string reason)
    {
      Db.Get().Attributes.Sneezyness.Lookup(this.master.gameObject).OnDirty -= new System.Action(this.OnSneezyChange);
      base.StopSM(reason);
    }

    public float NextSneezeInterval()
    {
      AttributeInstance attributeInstance = Db.Get().Attributes.Sneezyness.Lookup(this.master.gameObject);
      if ((double) attributeInstance.GetTotalValue() <= 0.0)
        return 70f;
      float num = 70f / attributeInstance.GetTotalValue();
      return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
    }

    private void OnSneezyChange()
    {
      this.smi.sm.isSneezy.Set((double) Db.Get().Attributes.Sneezyness.Lookup(this.master.gameObject).GetTotalValue() > 0.0, this.smi);
    }

    public Reactable GetReactable()
    {
      return (Reactable) new SelfEmoteReactable(this.master.gameObject, (HashedString) "Sneeze", Db.Get().ChoreTypes.Cough, (HashedString) "anim_sneeze_kanim", 0.0f, this.NextSneezeInterval(), float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "sneeze",
        startcb = new System.Action<GameObject>(this.TriggerDisurbance)
      }).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "sneeze_pst",
        finishcb = new System.Action<GameObject>(this.ResetSneeze)
      });
    }

    private void TriggerDisurbance(GameObject go)
    {
      AcousticDisturbance.Emit((object) go, 3);
    }

    private void ResetSneeze(GameObject go)
    {
      this.smi.GoTo((StateMachine.BaseState) this.sm.idle);
    }
  }
}
