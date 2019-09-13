// Decompiled with JetBrains decompiler
// Type: AgeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
  private const float OLD_WARNING = 5f;
  public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;
  public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;
  private AttributeModifier aging;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State state1 = this.alive.ToggleAttributeModifier("Aging", (Func<AgeMonitor.Instance, AttributeModifier>) (smi => this.aging), (Func<AgeMonitor.Instance, bool>) null);
    GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State timeToDie1 = this.time_to_die;
    // ISSUE: reference to a compiler-generated field
    if (AgeMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AgeMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback fMgCache0 = AgeMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State state2 = state1.Transition(timeToDie1, fMgCache0, UpdateRate.SIM_1000ms);
    // ISSUE: reference to a compiler-generated field
    if (AgeMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AgeMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<AgeMonitor.Instance, float> fMgCache1 = AgeMonitor.\u003C\u003Ef__mg\u0024cache1;
    state2.Update(fMgCache1, UpdateRate.SIM_1000ms, false);
    GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State timeToDie2 = this.time_to_die;
    // ISSUE: reference to a compiler-generated field
    if (AgeMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AgeMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback fMgCache2 = AgeMonitor.\u003C\u003Ef__mg\u0024cache2;
    timeToDie2.Enter(fMgCache2);
    this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 1f / 600f, (string) CREATURES.MODIFIERS.AGE.NAME, false, false, true);
  }

  private static void Die(AgeMonitor.Instance smi)
  {
    smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
  }

  private static bool TimeToDie(AgeMonitor.Instance smi)
  {
    return (double) smi.age.value >= (double) smi.age.GetMax();
  }

  private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
  {
    KSelectable component = smi.GetComponent<KSelectable>();
    if ((double) smi.age.value > (double) smi.age.GetMax() - 5.0)
      component.AddStatusItem(Db.Get().CreatureStatusItems.Old, (object) smi);
    else
      component.RemoveStatusItem(Db.Get().CreatureStatusItems.Old, false);
  }

  public class Def : StateMachine.BaseDef
  {
    public float maxAgePercentOnSpawn = 0.75f;

    public override void Configure(GameObject prefab)
    {
      prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
    }
  }

  public class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
  {
    public AmountInstance age;

    public Instance(IStateMachineTarget master, AgeMonitor.Def def)
      : base(master, def)
    {
      this.age = Db.Get().Amounts.Age.Lookup(this.gameObject);
      this.Subscribe(1119167081, (System.Action<object>) (data => this.RandomizeAge()));
    }

    public void RandomizeAge()
    {
      this.age.value = UnityEngine.Random.value * this.age.GetMax() * this.def.maxAgePercentOnSpawn;
      AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(this.gameObject);
      if (amountInstance == null)
        return;
      amountInstance.value = (float) ((double) this.age.value / (double) this.age.GetMax() * (double) amountInstance.GetMax() * 1.75);
      amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
    }

    public float CyclesUntilDeath
    {
      get
      {
        return this.age.GetMax() - this.age.value;
      }
    }
  }
}
