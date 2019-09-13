// Decompiled with JetBrains decompiler
// Type: SleepChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

public class SleepChore : Chore<SleepChore.StatesInstance>
{
  public static readonly Chore.Precondition IsOkayTimeToSleep = new Chore.Precondition()
  {
    id = nameof (IsOkayTimeToSleep),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OKAY_TIME_TO_SLEEP,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
      bool flag1 = (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsNarcolepsing();
      StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
      bool flag2 = smi != null && smi.NeedsToSleep();
      bool flag3 = ChorePreconditions.instance.IsScheduledTime.fn(ref context, (object) Db.Get().ScheduleBlockTypes.Sleep);
      if (!flag1 && !flag3)
        return flag2;
      return true;
    })
  };

  public SleepChore(
    ChoreType choreType,
    IStateMachineTarget target,
    GameObject bed,
    bool bedIsLocator,
    bool isInterruptable)
    : base(choreType, target, target.GetComponent<ChoreProvider>(), false, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
  {
    this.smi = new SleepChore.StatesInstance(this, target.gameObject, bed, bedIsLocator, isInterruptable);
    if (isInterruptable)
      this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(SleepChore.IsOkayTimeToSleep, (object) null);
    Operational component = bed.GetComponent<Operational>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) component);
  }

  public static Sleepable GetSafeFloorLocator(GameObject sleeper)
  {
    int cell = sleeper.GetComponent<Sensors>().GetSensor<SafeCellSensor>().GetSleepCellQuery();
    if (cell == Grid.InvalidCell)
      cell = Grid.PosToCell(sleeper.transform.GetPosition());
    return ChoreHelpers.CreateSleepLocator(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move)).GetComponent<Sleepable>();
  }

  public static bool IsLightLevelOk(int cell)
  {
    return Grid.LightIntensity[cell] <= 0;
  }

  public class StatesInstance : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.GameInstance
  {
    public int lastEvaluatedDay = -1;
    public float wakeUpBuffer = 2f;
    public bool hadPeacefulSleep;
    public bool hadNormalSleep;
    public bool hadBadSleep;
    public bool hadTerribleSleep;
    public string stateChangeNoiseSource;
    private GameObject locator;

    public StatesInstance(
      SleepChore master,
      GameObject sleeper,
      GameObject bed,
      bool bedIsLocator,
      bool isInterruptable)
      : base(master)
    {
      this.sm.sleeper.Set(sleeper, this.smi);
      this.sm.isInterruptable.Set(isInterruptable, this.smi);
      if (bedIsLocator)
        this.AddLocator(bed);
      else
        this.sm.bed.Set(bed, this.smi);
    }

    public void CheckLightLevel()
    {
      GameObject go = this.sm.sleeper.Get(this.smi);
      int cell = Grid.PosToCell(go);
      if (!Grid.IsValidCell(cell) || SleepChore.IsLightLevelOk(cell) || this.IsLoudSleeper())
        return;
      go.Trigger(-1063113160, (object) null);
    }

    public bool IsLoudSleeper()
    {
      return (UnityEngine.Object) this.sm.sleeper.Get(this.smi).GetComponent<Snorer>() != (UnityEngine.Object) null;
    }

    public void EvaluateSleepQuality()
    {
    }

    public void AddLocator(GameObject sleepable)
    {
      this.locator = sleepable;
      int cell = Grid.PosToCell(this.locator);
      Grid.Reserved[cell] = true;
      this.sm.bed.Set(this.locator, this);
    }

    public void DestroyLocator()
    {
      if (!((UnityEngine.Object) this.locator != (UnityEngine.Object) null))
        return;
      Grid.Reserved[Grid.PosToCell(this.locator)] = false;
      ChoreHelpers.DestroyLocator(this.locator);
      this.sm.bed.Set((KMonoBehaviour) null, this);
      this.locator = (GameObject) null;
    }

    public void SetAnim()
    {
      Sleepable sleepable = this.sm.bed.Get<Sleepable>(this.smi);
      if (!((UnityEngine.Object) sleepable.GetComponent<Building>() == (UnityEngine.Object) null))
        return;
      string str;
      switch (this.sm.sleeper.Get<Navigator>(this.smi).CurrentNavType)
      {
        case NavType.Ladder:
          str = "anim_sleep_ladder_kanim";
          break;
        case NavType.Pole:
          str = "anim_sleep_pole_kanim";
          break;
        default:
          str = "anim_sleep_floor_kanim";
          break;
      }
      sleepable.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) str)
      };
    }
  }

  public class States : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore>
  {
    public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter sleeper;
    public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter bed;
    public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isInterruptable;
    public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByNoise;
    public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByLight;
    public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.ApproachSubState<IApproachable> approach;
    public SleepChore.States.SleepStates sleep;
    public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.sleeper);
      this.root.Exit("DestroyLocator", (StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi => smi.DestroyLocator()));
      this.approach.InitializeStates(this.sleeper, this.bed, (GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State) this.sleep, (GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State) null, (CellOffset[]) null, (NavTactic) null);
      this.sleep.Enter("SetAnims", (StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi => smi.SetAnim())).DefaultState(this.sleep.normal).ToggleTag(GameTags.Asleep).DoSleep(this.sleeper, this.bed, this.success, (GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State) null).TriggerOnExit(GameHashes.SleepFinished).EventHandler(GameHashes.SleepDisturbedByLight, (StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi => this.isDisturbedByLight.Set(true, smi))).EventHandler(GameHashes.SleepDisturbedByNoise, (StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi => this.isDisturbedByNoise.Set(true, smi)));
      this.sleep.uninterruptable.DoNothing();
      this.sleep.normal.ParamTransition<bool>((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>) this.isInterruptable, this.sleep.uninterruptable, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsFalse).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Sleeping, (object) null).QueueAnim("working_loop", true, (Func<SleepChore.StatesInstance, string>) null).ParamTransition<bool>((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>) this.isDisturbedByNoise, this.sleep.interrupt_noise, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>) this.isDisturbedByLight, this.sleep.interrupt_light, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).Update((System.Action<SleepChore.StatesInstance, float>) ((smi, dt) => smi.CheckLightLevel()), UpdateRate.SIM_200ms, false);
      this.sleep.interrupt_noise.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByNoise, (object) null).QueueAnim("interrupt_light", false, (Func<SleepChore.StatesInstance, string>) null).OnAnimQueueComplete(this.sleep.interrupt_noise_transition);
      this.sleep.interrupt_noise_transition.Enter((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi =>
      {
        Effects component = smi.master.GetComponent<Effects>();
        component.Add(Db.Get().effects.Get("TerribleSleep"), true);
        if (component.HasEffect(Db.Get().effects.Get("BadSleep")))
          component.Remove(Db.Get().effects.Get("BadSleep"));
        this.isDisturbedByNoise.Set(false, smi);
        GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = !smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.success : this.sleep.normal;
        smi.GoTo((StateMachine.BaseState) state);
      }));
      this.sleep.interrupt_light.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByLight, (object) null).QueueAnim("interrupt", false, (Func<SleepChore.StatesInstance, string>) null).OnAnimQueueComplete(this.sleep.interrupt_light_transition);
      this.sleep.interrupt_light_transition.Enter((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi =>
      {
        if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
          smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleep"), true);
        GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = !smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.success : this.sleep.normal;
        this.isDisturbedByLight.Set(false, smi);
        smi.GoTo((StateMachine.BaseState) state);
      }));
      this.success.Enter((StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback) (smi => smi.EvaluateSleepQuality())).ReturnSuccess();
    }

    public class SleepStates : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State
    {
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition_pre;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State uninterruptable;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State normal;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise_transition;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light;
      public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light_transition;
    }
  }
}
