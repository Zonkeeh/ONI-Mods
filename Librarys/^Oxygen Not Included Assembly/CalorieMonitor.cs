// Decompiled with JetBrains decompiler
// Type: CalorieMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
  public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public CalorieMonitor.HungryState hungry;
  public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;
  public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;
  public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = true;
    this.satisfied.Transition((GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State) this.hungry, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsHungry()), UpdateRate.SIM_200ms);
    this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSatisfied()), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsEating()));
    this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsEatTime())).Transition(this.hungry.starving, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsStarving()), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, (object) null);
    this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsEatTime())).Transition(this.hungry.starving, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsStarving()), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, (object) null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, (Func<CalorieMonitor.Instance, bool>) null).ToggleThought(Db.Get().Thoughts.Starving, (Func<CalorieMonitor.Instance, bool>) null);
    this.hungry.starving.Transition(this.hungry.normal, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsStarving()), UpdateRate.SIM_200ms).Transition(this.depleted, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsDepleted()), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, (object) null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, (Func<CalorieMonitor.Instance, bool>) null).ToggleThought(Db.Get().Thoughts.Starving, (Func<CalorieMonitor.Instance, bool>) null);
    this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsEating()));
    this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter((StateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Kill()));
  }

  public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;
    public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;
    public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
  }

  public class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public AmountInstance calories;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.calories = Db.Get().Amounts.Calories.Lookup(this.gameObject);
    }

    private float GetCalories0to1()
    {
      return this.calories.value / this.calories.GetMax();
    }

    public bool IsEatTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
    }

    public bool IsHungry()
    {
      return (double) this.GetCalories0to1() < 0.824999988079071;
    }

    public bool IsStarving()
    {
      return (double) this.GetCalories0to1() < 0.25;
    }

    public bool IsSatisfied()
    {
      return (double) this.GetCalories0to1() > 0.949999988079071;
    }

    public bool IsEating()
    {
      ChoreDriver component = this.master.GetComponent<ChoreDriver>();
      if (component.HasChore())
        return component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
      return false;
    }

    public bool IsDepleted()
    {
      return (double) this.calories.value <= 0.0;
    }

    public bool ShouldExitInfirmary()
    {
      return !this.IsStarving();
    }

    public void Kill()
    {
      if (this.gameObject.GetSMI<DeathMonitor.Instance>() == null)
        return;
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
    }
  }
}
