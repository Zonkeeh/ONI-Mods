// Decompiled with JetBrains decompiler
// Type: SuffocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance>
{
  public SuffocationMonitor.SatisfiedState satisfied;
  public SuffocationMonitor.NoOxygenState nooxygen;
  public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State death;
  public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Update("CheckOverPressure", (System.Action<SuffocationMonitor.Instance, float>) ((smi, dt) => smi.CheckOverPressure()), UpdateRate.SIM_200ms, false).TagTransition(GameTags.Dead, this.dead, false);
    this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (Func<SuffocationMonitor.Instance, AttributeModifier>) (smi => smi.breathing), (Func<SuffocationMonitor.Instance, bool>) null).EventTransition(GameHashes.ExitedBreathableArea, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State) this.nooxygen, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsInBreathableArea()));
    this.satisfied.normal.Transition(this.satisfied.low, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.oxygenBreather.IsLowOxygen()), UpdateRate.SIM_200ms);
    this.satisfied.low.Transition(this.satisfied.normal, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.oxygenBreather.IsLowOxygen()), UpdateRate.SIM_200ms).Transition((GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State) this.nooxygen, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsInBreathableArea()), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
    this.nooxygen.EventTransition(GameHashes.EnteredBreathableArea, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State) this.satisfied, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsInBreathableArea())).TagTransition(GameTags.RecoveringBreath, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State) this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, (Func<SuffocationMonitor.Instance, bool>) null).ToggleAttributeModifier("Holding Breath", (Func<SuffocationMonitor.Instance, AttributeModifier>) (smi => smi.holdingbreath), (Func<SuffocationMonitor.Instance, bool>) null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
    this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, (object) null).Transition(this.nooxygen.suffocating, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSuffocating()), UpdateRate.SIM_200ms);
    this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, (object) null).Transition(this.death, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasSuffocated()), UpdateRate.SIM_200ms);
    this.death.Enter("SuffocationDeath", (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Kill()));
    this.dead.DoNothing();
  }

  public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
  }

  public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
  }

  public class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private static CellOffset[] pressureTestOffsets = new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    };
    private AmountInstance breath;
    public AttributeModifier breathing;
    public AttributeModifier holdingbreath;
    private const float HIGH_PRESSURE_DELAY = 3f;
    private bool wasInHighPressure;
    private float highPressureTime;

    public Instance(OxygenBreather oxygen_breather)
      : base((IStateMachineTarget) oxygen_breather)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(this.master.gameObject);
      Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
      float num = 0.9090909f;
      this.breathing = new AttributeModifier(deltaAttribute.Id, num, (string) DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
      this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -num, (string) DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
      this.oxygenBreather = oxygen_breather;
    }

    public OxygenBreather oxygenBreather { get; private set; }

    public bool IsInBreathableArea()
    {
      if (!this.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath))
        return this.master.GetComponent<Sensors>().GetSensor<BreathableAreaSensor>().IsBreathable();
      return true;
    }

    public bool HasSuffocated()
    {
      return (double) this.breath.value <= 0.0;
    }

    public bool IsSuffocating()
    {
      if ((double) this.breath.deltaAttribute.GetTotalValue() <= 0.0)
        return (double) this.breath.value <= 45.4545440673828;
      return false;
    }

    public void Kill()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
    }

    public void CheckOverPressure()
    {
      if (this.IsInHighPressure())
      {
        if (!this.wasInHighPressure)
        {
          this.wasInHighPressure = true;
          this.highPressureTime = Time.time;
        }
        else
        {
          if ((double) Time.time - (double) this.highPressureTime <= 3.0)
            return;
          this.master.GetComponent<Effects>().Add("PoppedEarDrums", true);
        }
      }
      else
        this.wasInHighPressure = false;
    }

    private bool IsInHighPressure()
    {
      int cell1 = Grid.PosToCell(this.gameObject);
      for (int index = 0; index < SuffocationMonitor.Instance.pressureTestOffsets.Length; ++index)
      {
        int cell2 = Grid.OffsetCell(cell1, SuffocationMonitor.Instance.pressureTestOffsets[index]);
        if (Grid.IsValidCell(cell2) && Grid.Element[cell2].IsGas && (double) Grid.Mass[cell2] > 4.0)
          return true;
      }
      return false;
    }
  }
}
