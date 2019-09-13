// Decompiled with JetBrains decompiler
// Type: StandardCropPlant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
  public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet()
  {
    grow = "grow",
    grow_pst = "grow_pst",
    idle_full = "idle_full",
    wilt_base = "wilt",
    harvest = "harvest"
  };
  public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;
  [MyCmpReq]
  private Crop crop;
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private ReceptacleMonitor rm;
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private KAnimControllerBase animController;
  [MyCmpGet]
  private Harvestable harvestable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.Get<KBatchedAnimController>().randomiseLoopedOffset = true;
    this.smi.StartSM();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public Notification CreateDeathNotification()
  {
    return new Notification((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + this.gameObject.GetProperName()), true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
  }

  public void RefreshPositionPercent()
  {
    this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
  }

  private static string ToolTipResolver(List<Notification> notificationList, object data)
  {
    string empty = string.Empty;
    for (int index = 0; index < notificationList.Count; ++index)
    {
      Notification notification = notificationList[index];
      empty += (string) notification.tooltipData;
      if (index < notificationList.Count - 1)
        empty += "\n";
    }
    return string.Format((string) CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, (object) empty);
  }

  public class AnimSet
  {
    public string grow;
    public string grow_pst;
    public string idle_full;
    public string wilt_base;
    public string harvest;
  }

  public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
  {
    public StandardCropPlant.States.AliveStates alive;
    public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = true;
      default_state = (StateMachine.BaseState) this.alive;
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead = this.dead;
      string name1 = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip1 = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main = Db.Get().StatusItemCategories.Main;
      string name2 = name1;
      string tooltip2 = tooltip1;
      string empty = string.Empty;
      HashedString render_overlay = new HashedString();
      StatusItemCategory category = main;
      dead.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, StandardCropPlant.StatesInstance, string>) null, (Func<string, StandardCropPlant.StatesInstance, string>) null, category).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
          smi.master.gameObject.AddOrGet<Notifier>().Add(smi.master.CreateDeathNotification(), string.Empty);
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, (string) null, 0).SetActive(true);
        Harvestable component = smi.master.GetComponent<Harvestable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.CanBeHarvested && (UnityEngine.Object) GameScheduler.Instance != (UnityEngine.Object) null)
          GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new System.Action<object>(smi.master.crop.SpawnFruit), (object) null, (SchedulerGroup) null);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new System.Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>();
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State state1 = this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => smi.master.growing.ReachedNextHarvest())).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping)).PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.grow), KAnim.PlayMode.Paused);
      // ISSUE: reference to a compiler-generated field
      if (StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback fMgCache0 = StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache0;
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State state2 = state1.Enter(fMgCache0);
      // ISSUE: reference to a compiler-generated field
      if (StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache1 = new System.Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<StandardCropPlant.StatesInstance, float> fMgCache1 = StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache1;
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State state3 = state2.Update(fMgCache1, UpdateRate.SIM_4000ms, false);
      // ISSUE: reference to a compiler-generated field
      if (StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback fMgCache2 = StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache2;
      state3.EventHandler(GameHashes.ConsumePlant, fMgCache2);
      this.alive.pre_fruiting.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.grow_pst), KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, (Func<StandardCropPlant.StatesInstance, object>) null).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State) this.alive.fruiting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null);
      this.alive.fruiting_lost.Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(false);
      })).GoTo(this.alive.idle);
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting = this.alive.wilting;
      // ISSUE: reference to a compiler-generated field
      if (StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache3 = new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim);
      }
      // ISSUE: reference to a compiler-generated field
      Func<StandardCropPlant.StatesInstance, string> fMgCache3 = StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache3;
      wilting.PlayAnim(fMgCache3, KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting())).EventTransition(GameHashes.Harvest, this.alive.harvest, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null);
      this.alive.sleeping.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.grow), KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null);
      this.alive.fruiting.DefaultState(this.alive.fruiting.fruiting_idle).EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.Harvest, this.alive.harvest, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback) (smi => !smi.master.growing.ReachedNextHarvest()));
      this.alive.fruiting.fruiting_idle.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.idle_full), KAnim.PlayMode.Loop).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(true);
      })).Transition(this.alive.fruiting.fruiting_old, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
      GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruitingOld = this.alive.fruiting.fruiting_old;
      // ISSUE: reference to a compiler-generated field
      if (StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache4 = new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim);
      }
      // ISSUE: reference to a compiler-generated field
      Func<StandardCropPlant.StatesInstance, string> fMgCache4 = StandardCropPlant.States.\u003C\u003Ef__mg\u0024cache4;
      fruitingOld.PlayAnim(fMgCache4, KAnim.PlayMode.Loop).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(true);
      })).Transition(this.alive.fruiting.fruiting_idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
      this.alive.harvest.PlayAnim((Func<StandardCropPlant.StatesInstance, string>) (smi => smi.master.anims.harvest), KAnim.PlayMode.Once).Enter((StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback) (smi =>
      {
        if ((UnityEngine.Object) GameScheduler.Instance != (UnityEngine.Object) null && (UnityEngine.Object) smi.master != (UnityEngine.Object) null)
          GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new System.Action<object>(smi.master.crop.SpawnFruit), (object) null, (SchedulerGroup) null);
        if (!((UnityEngine.Object) smi.master.harvestable != (UnityEngine.Object) null))
          return;
        smi.master.harvestable.SetCanBeHarvested(false);
      })).OnAnimQueueComplete(this.alive.idle);
    }

    private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
    {
      float num = smi.master.growing.PercentOfCurrentHarvest();
      string str = (double) num >= 0.75 ? ((double) num >= 1.0 ? "3" : "2") : "1";
      return smi.master.anims.wilt_base + str;
    }

    private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
    {
      smi.master.RefreshPositionPercent();
    }

    private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
    {
      smi.master.RefreshPositionPercent();
    }

    public bool IsOld(StandardCropPlant.StatesInstance smi)
    {
      return (double) smi.master.growing.PercentOldAge() > 0.5;
    }

    public bool IsSleeping(StandardCropPlant.StatesInstance smi)
    {
      CropSleepingMonitor.Instance smi1 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
      if (smi1 != null)
        return smi1.IsSleeping();
      return false;
    }

    public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
    {
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;
      public StandardCropPlant.States.FruitingState fruiting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State sleeping;
    }

    public class FruitingState : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State
    {
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_idle;
      public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_old;
    }
  }

  public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
  {
    public StatesInstance(StandardCropPlant master)
      : base(master)
    {
    }
  }
}
