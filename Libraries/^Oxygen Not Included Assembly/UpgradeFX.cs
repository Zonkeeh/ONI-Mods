// Decompiled with JetBrains decompiler
// Type: UpgradeFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class UpgradeFX : GameStateMachine<UpgradeFX, UpgradeFX.Instance>
{
  public StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.Target(this.fx);
    this.root.PlayAnim("upgrade").OnAnimQueueComplete((GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.State) null).ToggleReactable((Func<UpgradeFX.Instance, Reactable>) (smi => smi.CreateReactable())).Exit("DestroyFX", (StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DestroyFX()));
  }

  public class Instance : GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master, Vector3 offset)
      : base(master)
    {
      this.sm.fx.Set(FXHelpers.CreateEffect("upgrade_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false).gameObject, this.smi);
    }

    public void DestroyFX()
    {
      Util.KDestroyGameObject(this.sm.fx.Get(this.smi));
    }

    public Reactable CreateReactable()
    {
      return (Reactable) new EmoteReactable(this.master.gameObject, (HashedString) nameof (UpgradeFX), Db.Get().ChoreTypes.Emote, (HashedString) "anim_cheer_kanim", 15, 8, 0.0f, 20f, float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "cheer_pre"
      }).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "cheer_loop"
      }).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "cheer_pst"
      });
    }
  }
}
