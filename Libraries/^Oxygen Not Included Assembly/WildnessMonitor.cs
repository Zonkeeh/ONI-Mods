// Decompiled with JetBrains decompiler
// Type: WildnessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
  private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[2]
  {
    (KAnimHashedString) "tag",
    (KAnimHashedString) "snapto_tag"
  };
  public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;
  public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.tame;
    this.serializable = true;
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild1 = this.wild;
    // ISSUE: reference to a compiler-generated field
    if (WildnessMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WildnessMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback fMgCache0 = WildnessMonitor.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State state1 = wild1.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (WildnessMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WildnessMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback fMgCache1 = WildnessMonitor.\u003C\u003Ef__mg\u0024cache1;
    state1.Enter(fMgCache1).Transition(this.tame, (StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback) (smi => !WildnessMonitor.IsWild(smi)), UpdateRate.SIM_1000ms).ToggleEffect((Func<WildnessMonitor.Instance, Effect>) (smi => smi.def.wildEffect)).ToggleTag(GameTags.Creatures.Wild);
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame = this.tame;
    // ISSUE: reference to a compiler-generated field
    if (WildnessMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WildnessMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback fMgCache2 = WildnessMonitor.\u003C\u003Ef__mg\u0024cache2;
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State state2 = tame.Enter(fMgCache2);
    // ISSUE: reference to a compiler-generated field
    if (WildnessMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WildnessMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback fMgCache3 = WildnessMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State state3 = state2.Enter(fMgCache3);
    GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild2 = this.wild;
    // ISSUE: reference to a compiler-generated field
    if (WildnessMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      WildnessMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback fMgCache4 = WildnessMonitor.\u003C\u003Ef__mg\u0024cache4;
    state3.Transition(wild2, fMgCache4, UpdateRate.SIM_1000ms).ToggleEffect((Func<WildnessMonitor.Instance, Effect>) (smi => smi.def.tameEffect));
  }

  private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
  {
    foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
      smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
  }

  private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
  {
    foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
      smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
  }

  private static bool IsWild(WildnessMonitor.Instance smi)
  {
    return (double) smi.wildness.value > 0.0;
  }

  private static void RefreshAmounts(WildnessMonitor.Instance smi)
  {
    bool flag = WildnessMonitor.IsWild(smi);
    smi.wildness.hide = !flag;
    Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject).hide = flag;
    Db.Get().Amounts.Calories.Lookup(smi.gameObject).hide = flag;
    Db.Get().Amounts.Temperature.Lookup(smi.gameObject).hide = flag;
    AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
    if (amountInstance == null)
      return;
    amountInstance.hide = flag;
  }

  public class Def : StateMachine.BaseDef
  {
    public Effect wildEffect;
    public Effect tameEffect;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
    }
  }

  public class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
  {
    public AmountInstance wildness;

    public Instance(IStateMachineTarget master, WildnessMonitor.Def def)
      : base(master, def)
    {
      this.wildness = Db.Get().Amounts.Wildness.Lookup(this.gameObject);
      this.wildness.value = this.wildness.GetMax();
    }
  }
}
