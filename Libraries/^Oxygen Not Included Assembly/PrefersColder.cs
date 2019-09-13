// Decompiled with JetBrains decompiler
// Type: PrefersColder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
  {
    public StatesInstance(PrefersColder master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
  {
    private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", 0.005f, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.ToggleAttributeModifier((string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (Func<PrefersColder.StatesInstance, AttributeModifier>) (smi => this.modifier), (Func<PrefersColder.StatesInstance, bool>) null);
    }
  }
}
