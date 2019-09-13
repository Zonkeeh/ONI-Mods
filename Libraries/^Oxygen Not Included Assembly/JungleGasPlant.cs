// Decompiled with JetBrains decompiler
// Type: JungleGasPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class JungleGasPlant : StateMachineComponent<JungleGasPlant.StatesInstance>
{
  [MyCmpReq]
  private ReceptacleMonitor rm;
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ElementEmitter elementEmitter;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public class StatesInstance : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.GameInstance
  {
    public StatesInstance(JungleGasPlant master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant>
  {
    public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State blocked_from_growing;
    public JungleGasPlant.States.AliveStates alive;
    public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.alive.seed_grow;
      this.serializable = true;
      this.root.Enter((StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State.Callback) (smi =>
      {
        if (smi.master.rm.Replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
          smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
        else
          smi.GoTo((StateMachine.BaseState) this.alive.seed_grow);
      }));
      GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State dead = this.dead;
      string name1 = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip1 = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main = Db.Get().StatusItemCategories.Main;
      string name2 = name1;
      string tooltip2 = tooltip1;
      string empty = string.Empty;
      HashedString render_overlay = new HashedString();
      StatusItemCategory category = main;
      dead.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, JungleGasPlant.StatesInstance, string>) null, (Func<string, JungleGasPlant.StatesInstance, string>) null, category).Enter((StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, (string) null, 0).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, (object) null).TagTransition(GameTags.Entombed, this.alive.seed_grow, true).EventTransition(GameHashes.TooColdWarning, this.alive.seed_grow, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.TooHotWarning, this.alive.seed_grow, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) null).TagTransition(GameTags.Uprooted, this.dead, false);
      this.alive.InitializeStates(this.masterTarget, this.dead);
      this.alive.seed_grow.QueueAnim("seed_grow", false, (Func<JungleGasPlant.StatesInstance, string>) null).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.Wilt, (GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State) this.alive.wilting, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting()));
      this.alive.idle.EventTransition(GameHashes.Wilt, (GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State) this.alive.wilting, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.Grow, (GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State) this.alive.grown, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => smi.master.growing.IsGrown())).PlayAnim("idle_loop", KAnim.PlayMode.Loop);
      this.alive.grown.DefaultState(this.alive.grown.pre).EventTransition(GameHashes.Wilt, (GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State) this.alive.wilting, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).Enter((StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State.Callback) (smi => smi.master.elementEmitter.SetEmitting(true))).Exit((StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State.Callback) (smi => smi.master.elementEmitter.SetEmitting(false)));
      this.alive.grown.pre.PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.grown.idle);
      this.alive.grown.idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop);
      this.alive.wilting.pre.DefaultState(this.alive.wilting.pre).PlayAnim("wilt_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.wilting.idle).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
      this.alive.wilting.idle.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (StateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting()));
      this.alive.wilting.pst.PlayAnim("wilt_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.idle);
    }

    public class AliveStates : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.PlantAliveSubState
    {
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State seed_grow;
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
      public JungleGasPlant.States.WiltingState wilting;
      public JungleGasPlant.States.GrownState grown;
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State destroy;
    }

    public class GrownState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
    {
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
    }

    public class WiltingState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
    {
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
      public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pst;
    }
  }
}
