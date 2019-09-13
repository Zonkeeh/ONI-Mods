// Decompiled with JetBrains decompiler
// Type: SuitSuffocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

public class SuitSuffocationMonitor : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance>
{
  public SuitSuffocationMonitor.SatisfiedState satisfied;
  public SuitSuffocationMonitor.NoOxygenState nooxygen;
  public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (Func<SuitSuffocationMonitor.Instance, AttributeModifier>) (smi => smi.breathing), (Func<SuitSuffocationMonitor.Instance, bool>) null).Transition((GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State) this.nooxygen, (StateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsTankEmpty()), UpdateRate.SIM_200ms);
    this.satisfied.normal.Transition(this.satisfied.low, (StateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.suitTank.NeedsRecharging()), UpdateRate.SIM_200ms);
    this.satisfied.low.DoNothing();
    this.nooxygen.ToggleExpression(Db.Get().Expressions.Suffocate, (Func<SuitSuffocationMonitor.Instance, bool>) null).ToggleAttributeModifier("Holding Breath", (Func<SuitSuffocationMonitor.Instance, AttributeModifier>) (smi => smi.holdingbreath), (Func<SuitSuffocationMonitor.Instance, bool>) null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
    this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, (object) null).Transition(this.nooxygen.suffocating, (StateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSuffocating()), UpdateRate.SIM_200ms);
    this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, (object) null).Transition(this.death, (StateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasSuffocated()), UpdateRate.SIM_200ms);
    this.death.Enter("SuffocationDeath", (StateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Kill()));
  }

  public class NoOxygenState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;
    public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
  }

  public class SatisfiedState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;
    public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
  }

  public class Instance : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private AmountInstance breath;
    public AttributeModifier breathing;
    public AttributeModifier holdingbreath;
    private OxygenBreather masterOxygenBreather;

    public Instance(IStateMachineTarget master, SuitTank suit_tank)
      : base(master)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
      Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
      float num = 0.9090909f;
      this.breathing = new AttributeModifier(deltaAttribute.Id, num, (string) DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
      this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -num, (string) DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
      this.suitTank = suit_tank;
    }

    public SuitTank suitTank { get; private set; }

    public bool IsTankEmpty()
    {
      return this.suitTank.IsEmpty();
    }

    public bool HasSuffocated()
    {
      return (double) this.breath.value <= 0.0;
    }

    public bool IsSuffocating()
    {
      return (double) this.breath.value <= 45.4545440673828;
    }

    public void Kill()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
    }
  }
}
