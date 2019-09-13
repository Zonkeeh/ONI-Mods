// Decompiled with JetBrains decompiler
// Type: StressMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using System;

public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
  public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public StressMonitor.Stressed stressed;
  private const float StressThreshold_One = 60f;
  private const float StressThreshold_Two = 100f;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = true;
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Update(nameof (StressMonitor), (System.Action<StressMonitor.Instance, float>) ((smi, dt) => smi.ReportStress(dt)), UpdateRate.SIM_200ms, false);
    this.satisfied.Transition(this.stressed.tier1, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) smi.stress.value >= 60.0), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Neutral, (Func<StressMonitor.Instance, bool>) null);
    this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed, (object) null).Transition(this.satisfied, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => (double) smi.stress.value < 60.0), UpdateRate.SIM_200ms).ToggleReactable((Func<StressMonitor.Instance, Reactable>) (smi => smi.CreateConcernReactable())).TriggerOnEnter(GameHashes.Stressed, (Func<StressMonitor.Instance, object>) null);
    this.stressed.tier1.Transition(this.stressed.tier2, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.HasHadEnough()), UpdateRate.SIM_200ms);
    this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough, (Func<StressMonitor.Instance, object>) null).Transition(this.stressed.tier1, (StateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.HasHadEnough()), UpdateRate.SIM_200ms);
  }

  public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;
    public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
  }

  public class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private bool allowStressBreak = true;
    public AmountInstance stress;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.stress = Db.Get().Amounts.Stress.Lookup(this.gameObject);
      this.allowStressBreak = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id].IsDefaultLevel(CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks).id);
    }

    public bool IsStressed()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.stressed);
    }

    public bool HasHadEnough()
    {
      if (this.allowStressBreak)
        return (double) this.stress.value >= 100.0;
      return false;
    }

    public void ReportStress(float dt)
    {
      for (int index = 0; index != this.stress.deltaAttribute.Modifiers.Count; ++index)
      {
        AttributeModifier modifier = this.stress.deltaAttribute.Modifiers[index];
        DebugUtil.DevAssert(!modifier.IsMultiplier, "Reporting stress for multipliers not supported yet.");
        ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, modifier.Value * dt, modifier.GetDescription(), this.gameObject.GetProperName());
      }
    }

    public Reactable CreateConcernReactable()
    {
      return (Reactable) new EmoteReactable(this.master.gameObject, (HashedString) "StressConcern", Db.Get().ChoreTypes.Emote, (HashedString) "anim_react_concern_kanim", 15, 8, 0.0f, 30f, float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
      {
        anim = (HashedString) "react"
      });
    }
  }
}
