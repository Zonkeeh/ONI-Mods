// Decompiled with JetBrains decompiler
// Type: PrefersWarmer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
  {
    public StatesInstance(PrefersWarmer master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
  {
    private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", -0.005f, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.ToggleAttributeModifier((string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (Func<PrefersWarmer.StatesInstance, AttributeModifier>) (smi => this.modifier), (Func<PrefersWarmer.StatesInstance, bool>) null);
    }
  }
}
