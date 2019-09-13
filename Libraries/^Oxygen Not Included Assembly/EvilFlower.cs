// Decompiled with JetBrains decompiler
// Type: EvilFlower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class EvilFlower : StateMachineComponent<EvilFlower.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<EvilFlower> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<EvilFlower>((System.Action<EvilFlower, object>) ((component, data) => component.replanted = true));
  public EffectorValues positive_decor_effect = new EffectorValues()
  {
    amount = 1,
    radius = 5
  };
  public EffectorValues negative_decor_effect = new EffectorValues()
  {
    amount = -1,
    radius = 5
  };
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private EntombVulnerable entombVulnerable;
  public bool replanted;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<EvilFlower>(1309017699, EvilFlower.SetReplantedTrueDelegate);
  }

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

  public class StatesInstance : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.GameInstance
  {
    public StatesInstance(EvilFlower smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower>
  {
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State grow;
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State blocked_from_growing;
    public EvilFlower.States.AliveStates alive;
    public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      this.serializable = true;
      GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead = this.dead;
      string name1 = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip1 = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
      string name2 = name1;
      string tooltip2 = tooltip1;
      string empty1 = string.Empty;
      HashedString render_overlay1 = new HashedString();
      StatusItemCategory category1 = main1;
      dead.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, EvilFlower.StatesInstance, string>) null, (Func<string, EvilFlower.StatesInstance, string>) null, category1).TriggerOnEnter(GameHashes.BurstEmitDisease, (Func<EvilFlower.StatesInstance, object>) null).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, (string) null, 0).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, (object) null).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead, false);
      this.grow.Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        if (!smi.master.replanted || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) null);
      GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
      string name3 = (string) CREATURES.STATUSITEMS.IDLE.NAME;
      string tooltip3 = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
      StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
      string name4 = name3;
      string tooltip4 = tooltip3;
      string empty2 = string.Empty;
      HashedString render_overlay2 = new HashedString();
      StatusItemCategory category2 = main2;
      state.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, EvilFlower.StatesInstance, string>) null, (Func<string, EvilFlower.StatesInstance, string>) null, category2);
      this.alive.idle.EventTransition(GameHashes.Wilt, (GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State) this.alive.wilting, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.AddTag(GameTags.Decoration);
      }));
      this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.Transition.ConditionCallback) null).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.RemoveTag(GameTags.Decoration);
      }));
    }

    public class AliveStates : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.PlantAliveSubState
    {
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State idle;
      public EvilFlower.States.WiltingState wilting;
    }

    public class WiltingState : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State
    {
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pre;
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting;
      public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pst;
    }
  }
}
