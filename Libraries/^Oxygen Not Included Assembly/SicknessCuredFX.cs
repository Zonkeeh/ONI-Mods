// Decompiled with JetBrains decompiler
// Type: SicknessCuredFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SicknessCuredFX : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance>
{
  public StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.Target(this.fx);
    this.root.PlayAnim("upgrade").OnAnimQueueComplete((GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.State) null).Exit("DestroyFX", (StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public class Instance : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, Vector3 offset)
      : base(master)
    {
      this.sm.fx.Set(FXHelpers.CreateEffect("recentlyhealed_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false).gameObject, this.smi);
    }

    public void DestroyFX()
    {
      Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
    }
  }
}
