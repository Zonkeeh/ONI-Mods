// Decompiled with JetBrains decompiler
// Type: HappinessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
  private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;
  private HappinessMonitor.HappyState happy;
  private HappinessMonitor.UnhappyState unhappy;
  private Effect happyWildEffect;
  private Effect happyTameEffect;
  private Effect unhappyWildEffect;
  private Effect unhappyTameEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied1 = this.satisfied;
    HappinessMonitor.HappyState happy = this.happy;
    // ISSUE: reference to a compiler-generated field
    if (HappinessMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      HappinessMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback fMgCache0 = HappinessMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State state1 = satisfied1.Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) happy, fMgCache0, UpdateRate.SIM_1000ms);
    HappinessMonitor.UnhappyState unhappy = this.unhappy;
    // ISSUE: reference to a compiler-generated field
    if (HappinessMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      HappinessMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback condition1 = GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(HappinessMonitor.\u003C\u003Ef__mg\u0024cache1);
    state1.Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) unhappy, condition1, UpdateRate.SIM_1000ms);
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State state2 = this.happy.DefaultState(this.happy.wild);
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied2 = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (HappinessMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      HappinessMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback condition2 = GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(HappinessMonitor.\u003C\u003Ef__mg\u0024cache2);
    state2.Transition(satisfied2, condition2, UpdateRate.SIM_1000ms);
    this.happy.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.happyWildEffect)).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
    this.happy.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.happyTameEffect)).TagTransition(GameTags.Creatures.Wild, this.happy.wild, false);
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State state3 = this.unhappy.DefaultState(this.unhappy.wild);
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied3 = this.satisfied;
    // ISSUE: reference to a compiler-generated field
    if (HappinessMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      HappinessMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback fMgCache3 = HappinessMonitor.\u003C\u003Ef__mg\u0024cache3;
    state3.Transition(satisfied3, fMgCache3, UpdateRate.SIM_1000ms);
    this.unhappy.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.unhappyWildEffect)).TagTransition(GameTags.Creatures.Wild, this.unhappy.tame, true);
    this.unhappy.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.unhappyTameEffect)).TagTransition(GameTags.Creatures.Wild, this.unhappy.wild, false);
    this.happyWildEffect = new Effect("Happy", (string) CREATURES.MODIFIERS.HAPPY.NAME, (string) CREATURES.MODIFIERS.HAPPY.TOOLTIP, 0.0f, true, false, false, (string) null, 0.0f, (string) null);
    this.happyTameEffect = new Effect("Happy", (string) CREATURES.MODIFIERS.HAPPY.NAME, (string) CREATURES.MODIFIERS.HAPPY.TOOLTIP, 0.0f, true, false, false, (string) null, 0.0f, (string) null);
    this.happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, (string) CREATURES.MODIFIERS.HAPPY.NAME, true, false, true));
    this.unhappyWildEffect = new Effect("Unhappy", (string) CREATURES.MODIFIERS.UNHAPPY.NAME, (string) CREATURES.MODIFIERS.UNHAPPY.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
    this.unhappyWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, (string) CREATURES.MODIFIERS.UNHAPPY.NAME, false, false, true));
    this.unhappyTameEffect = new Effect("Unhappy", (string) CREATURES.MODIFIERS.UNHAPPY.NAME, (string) CREATURES.MODIFIERS.UNHAPPY.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
    this.unhappyTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, (string) CREATURES.MODIFIERS.UNHAPPY.NAME, false, false, true));
  }

  private static bool IsHappy(HappinessMonitor.Instance smi)
  {
    return (double) smi.happiness.GetTotalValue() >= (double) smi.def.threshold;
  }

  public class Def : StateMachine.BaseDef
  {
    public float threshold;
  }

  public class UnhappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public class HappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public class Instance : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
  {
    public AttributeInstance happiness;

    public Instance(IStateMachineTarget master, HappinessMonitor.Def def)
      : base(master, def)
    {
      this.happiness = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
    }
  }
}
