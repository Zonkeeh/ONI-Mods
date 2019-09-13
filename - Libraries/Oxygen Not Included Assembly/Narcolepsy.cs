// Decompiled with JetBrains decompiler
// Type: Narcolepsy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
  public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition()
  {
    id = nameof (IsNarcolepsingPrecondition),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsNarcolepsing();
    })
  };
  private static readonly EventSystem.IntraObjectHandler<Narcolepsy> OnDeathDelegate = new EventSystem.IntraObjectHandler<Narcolepsy>((System.Action<Narcolepsy, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Narcolepsy> OnRevivedDelegate = new EventSystem.IntraObjectHandler<Narcolepsy>((System.Action<Narcolepsy, object>) ((component, data) => component.OnRevived(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<Narcolepsy>(1623392196, Narcolepsy.OnDeathDelegate);
    this.Subscribe<Narcolepsy>(-1117766961, Narcolepsy.OnRevivedDelegate);
  }

  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  private void OnDeath(object data)
  {
    this.enabled = false;
  }

  private void OnRevived(object data)
  {
    this.enabled = true;
  }

  public bool IsNarcolepsing()
  {
    return this.smi.IsNarcolepsing();
  }

  public void ModifyTrait(Trait t)
  {
  }

  public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
  {
    public StatesInstance(Narcolepsy master)
      : base(master)
    {
    }

    public bool IsSleeping()
    {
      StaminaMonitor.Instance smi = this.master.GetSMI<StaminaMonitor.Instance>();
      if (smi != null)
        return smi.IsSleeping();
      return false;
    }

    public bool IsNarcolepsing()
    {
      return this.GetCurrentState() == this.sm.sleepy;
    }

    public GameObject CreateFloorLocator()
    {
      Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(this.master.gameObject);
      safeFloorLocator.effectName = "NarcolepticSleep";
      safeFloorLocator.stretchOnWake = false;
      return safeFloorLocator.gameObject;
    }
  }

  public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
  {
    public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;
    public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
    public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.TagTransition(GameTags.Dead, this.dead, false);
      this.idle.Enter("ScheduleNextSleep", (StateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State.Callback) (smi => smi.ScheduleGoTo(this.GetNewInterval(TUNING.TRAITS.NARCOLEPSY_INTERVAL_MIN, TUNING.TRAITS.NARCOLEPSY_INTERVAL_MAX), (StateMachine.BaseState) this.sleepy)));
      this.sleepy.Enter("Is Already Sleeping Check", (StateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State.Callback) (smi =>
      {
        if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
          smi.GoTo((StateMachine.BaseState) this.idle);
        else
          smi.ScheduleGoTo(this.GetNewInterval(TUNING.TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TUNING.TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), (StateMachine.BaseState) this.idle);
      })).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
      this.dead.DoNothing();
    }

    private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
    {
      GameObject floorLocator = smi.CreateFloorLocator();
      SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, (IStateMachineTarget) smi.master, floorLocator, true, false);
      sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, (object) null);
      return (Chore) sleepChore;
    }

    private float GetNewInterval(float min, float max)
    {
      Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
      return UnityEngine.Random.Range(min, max);
    }
  }
}
