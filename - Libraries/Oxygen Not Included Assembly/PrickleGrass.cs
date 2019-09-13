// Decompiled with JetBrains decompiler
// Type: PrickleGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

public class PrickleGrass : StateMachineComponent<PrickleGrass.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<PrickleGrass> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<PrickleGrass>((System.Action<PrickleGrass, object>) ((component, data) => component.replanted = true));
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
    this.Subscribe<PrickleGrass>(1309017699, PrickleGrass.SetReplantedTrueDelegate);
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

  public class StatesInstance : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.GameInstance
  {
    public StatesInstance(PrickleGrass smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass>
  {
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State grow;
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State blocked_from_growing;
    public PrickleGrass.States.AliveStates alive;
    public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grow;
      this.serializable = true;
      GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State dead = this.dead;
      string name1 = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip1 = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
      string name2 = name1;
      string tooltip2 = tooltip1;
      string empty1 = string.Empty;
      HashedString render_overlay1 = new HashedString();
      StatusItemCategory category1 = main1;
      dead.ToggleStatusItem(name2, tooltip2, empty1, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay1, 0, (Func<string, PrickleGrass.StatesInstance, string>) null, (Func<string, PrickleGrass.StatesInstance, string>) null, category1).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, (string) null, 0).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, (object) null).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead, false);
      this.grow.Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        if (!smi.master.replanted || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) null);
      GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State state = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
      string name3 = (string) CREATURES.STATUSITEMS.IDLE.NAME;
      string tooltip3 = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
      StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
      string name4 = name3;
      string tooltip4 = tooltip3;
      string empty2 = string.Empty;
      HashedString render_overlay2 = new HashedString();
      StatusItemCategory category2 = main2;
      state.ToggleStatusItem(name4, tooltip4, empty2, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay2, 0, (Func<string, PrickleGrass.StatesInstance, string>) null, (Func<string, PrickleGrass.StatesInstance, string>) null, category2);
      this.alive.idle.EventTransition(GameHashes.Wilt, (GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State) this.alive.wilting, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.AddTag(GameTags.Decoration);
      }));
      this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.Transition.ConditionCallback) null).ToggleTag(GameTags.PreventEmittingDisease).Enter((StateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
        smi.master.GetComponent<DecorProvider>().Refresh();
        smi.master.RemoveTag(GameTags.Decoration);
      }));
    }

    public class AliveStates : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.PlantAliveSubState
    {
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State idle;
      public PrickleGrass.States.WiltingState wilting;
    }

    public class WiltingState : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State
    {
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pre;
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting;
      public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pst;
    }
  }
}
