// Decompiled with JetBrains decompiler
// Type: ColonyRationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ColonyRationMonitor : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance>
{
  private StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isOutOfRations = new StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter();
  public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State outofrations;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Update("UpdateOutOfRations", (System.Action<ColonyRationMonitor.Instance, float>) ((smi, dt) => smi.UpdateIsOutOfRations()), UpdateRate.SIM_200ms, false);
    this.satisfied.ParamTransition<bool>((StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isOutOfRations, this.outofrations, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsTrue).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, (Func<ColonyRationMonitor.Instance, object>) null);
    this.outofrations.ParamTransition<bool>((StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isOutOfRations, this.satisfied, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsFalse).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, (Func<ColonyRationMonitor.Instance, object>) null);
  }

  public class Instance : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.UpdateIsOutOfRations();
    }

    public void UpdateIsOutOfRations()
    {
      bool flag = true;
      foreach (Component component in Components.Edibles.Items)
      {
        if ((double) component.GetComponent<Pickupable>().UnreservedAmount > 0.0)
        {
          flag = false;
          break;
        }
      }
      this.smi.sm.isOutOfRations.Set(flag, this.smi);
    }

    public bool IsOutOfRations()
    {
      return this.smi.sm.isOutOfRations.Get(this.smi);
    }
  }
}
