// Decompiled with JetBrains decompiler
// Type: FliesFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class FliesFX : GameStateMachine<FliesFX, FliesFX.Instance>
{
  public StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.Target(this.fx);
    this.root.PlayAnim("swarm_pre").QueueAnim("swarm_loop", true, (Func<FliesFX.Instance, string>) null).Exit("DestroyFX", (StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public class Instance : GameStateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, Vector3 offset)
      : base(master)
    {
      this.sm.fx.Set(FXHelpers.CreateEffect("fly_swarm_kanim", this.smi.master.transform.GetPosition() + offset, this.smi.master.transform, false, Grid.SceneLayer.Front, false).gameObject, this.smi);
    }

    public void DestroyFX()
    {
      Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
    }
  }
}
