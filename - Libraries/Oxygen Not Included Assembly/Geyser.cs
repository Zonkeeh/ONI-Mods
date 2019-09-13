// Decompiled with JetBrains decompiler
// Type: Geyser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpAdd]
  private ElementEmitter emitter;
  [Serialize]
  public GeyserConfigurator.GeyserInstanceConfiguration configuration;
  public Vector2I outputOffset;
  private const float PRE_PCT = 0.1f;
  private const float POST_PCT = 0.05f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    if (this.configuration == null || this.configuration.typeId == HashedString.Invalid)
      this.configuration = this.GetComponent<GeyserConfigurator>().MakeConfiguration();
    this.emitter.emitRange = (byte) 2;
    this.emitter.maxPressure = this.configuration.GetMaxPressure();
    this.emitter.outputElement = new ElementConverter.OutputElement(this.configuration.GetEmitRate(), this.configuration.GetElement(), this.configuration.GetTemperature(), false, false, (float) this.outputOffset.x, (float) this.outputOffset.y, 1f, this.configuration.GetDiseaseIdx(), Mathf.RoundToInt((float) this.configuration.GetDiseaseCount() * this.configuration.GetEmitRate()));
    this.smi.StartSM();
    Workable component = (Workable) this.GetComponent<Studyable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.alwaysShowProgressBar = true;
  }

  public float RemainingPhaseTimeFrom2(
    float onDuration,
    float offDuration,
    float time,
    Geyser.Phase expectedPhase)
  {
    float num1 = onDuration + offDuration;
    float num2 = time % num1;
    float num3;
    Geyser.Phase phase;
    if ((double) num2 < (double) onDuration)
    {
      num3 = Mathf.Max(onDuration - num2, 0.0f);
      phase = Geyser.Phase.On;
    }
    else
    {
      num3 = Mathf.Max(onDuration + offDuration - num2, 0.0f);
      phase = Geyser.Phase.Off;
    }
    if (expectedPhase == Geyser.Phase.Any || phase == expectedPhase)
      return num3;
    return 0.0f;
  }

  public float RemainingPhaseTimeFrom4(
    float onDuration,
    float pstDuration,
    float offDuration,
    float preDuration,
    float time,
    Geyser.Phase expectedPhase)
  {
    float num1 = onDuration + pstDuration + offDuration + preDuration;
    float num2 = time % num1;
    float num3;
    Geyser.Phase phase;
    if ((double) num2 < (double) onDuration)
    {
      num3 = onDuration - num2;
      phase = Geyser.Phase.On;
    }
    else if ((double) num2 < (double) onDuration + (double) pstDuration)
    {
      num3 = onDuration + pstDuration - num2;
      phase = Geyser.Phase.Pst;
    }
    else if ((double) num2 < (double) onDuration + (double) pstDuration + (double) offDuration)
    {
      num3 = onDuration + pstDuration + offDuration - num2;
      phase = Geyser.Phase.Off;
    }
    else
    {
      num3 = onDuration + pstDuration + offDuration + preDuration - num2;
      phase = Geyser.Phase.Pre;
    }
    if (expectedPhase == Geyser.Phase.Any || phase == expectedPhase)
      return num3;
    return 0.0f;
  }

  private float IdleDuration()
  {
    return this.configuration.GetOffDuration() * 0.85f;
  }

  private float PreDuration()
  {
    return this.configuration.GetOffDuration() * 0.1f;
  }

  private float PostDuration()
  {
    return this.configuration.GetOffDuration() * 0.05f;
  }

  private float EruptDuration()
  {
    return this.configuration.GetOnDuration();
  }

  public bool ShouldGoDormant()
  {
    return (double) this.RemainingActiveTime() <= 0.0;
  }

  public float RemainingIdleTime()
  {
    return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), GameClock.Instance.GetTime(), Geyser.Phase.Off);
  }

  public float RemainingEruptPreTime()
  {
    return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), GameClock.Instance.GetTime(), Geyser.Phase.Pre);
  }

  public float RemainingEruptTime()
  {
    return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), GameClock.Instance.GetTime(), Geyser.Phase.On);
  }

  public float RemainingEruptPostTime()
  {
    return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), GameClock.Instance.GetTime(), Geyser.Phase.Pst);
  }

  public float RemainingNonEruptTime()
  {
    return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), GameClock.Instance.GetTime(), Geyser.Phase.Off);
  }

  public float RemainingDormantTime()
  {
    return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), GameClock.Instance.GetTime(), Geyser.Phase.Off);
  }

  public float RemainingActiveTime()
  {
    return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), GameClock.Instance.GetTime(), Geyser.Phase.On);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(this.configuration.GetElement()).tag.ProperName();
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, (object) str, (object) GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION, (object) this.configuration.GetElement().ToString(), (object) GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
    if (this.configuration.GetDiseaseIdx() != byte.MaxValue)
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_DISEASE, (object) GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE, (object) GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), Descriptor.DescriptorType.Effect, false));
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_PERIOD, (object) GameUtil.GetFormattedTime(this.configuration.GetOnDuration()), (object) GameUtil.GetFormattedTime(this.configuration.GetIterationLength())), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD, (object) GameUtil.GetFormattedTime(this.configuration.GetOnDuration()), (object) GameUtil.GetFormattedTime(this.configuration.GetIterationLength())), Descriptor.DescriptorType.Effect, false));
    Studyable component = this.GetComponent<Studyable>();
    if ((bool) ((UnityEngine.Object) component) && !component.Studied)
    {
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED), Descriptor.DescriptorType.Effect, false));
    }
    else
    {
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD, (object) GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1"), (object) GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD, (object) GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1"), (object) GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1")), Descriptor.DescriptorType.Effect, false));
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.dormant))
        descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE, (object) GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE, (object) GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1")), Descriptor.DescriptorType.Effect, false));
      else
        descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT, (object) GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT, (object) GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1")), Descriptor.DescriptorType.Effect, false));
    }
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.GameInstance
  {
    public StatesInstance(Geyser smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser>
  {
    public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State dormant;
    public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State idle;
    public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State pre_erupt;
    public Geyser.States.EruptState erupt;
    public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State post_erupt;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = true;
      this.root.DefaultState(this.idle).Enter((StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State.Callback) (smi => smi.master.emitter.SetEmitting(false)));
      this.dormant.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant).ScheduleGoTo((Func<Geyser.StatesInstance, float>) (smi => smi.master.RemainingDormantTime()), (StateMachine.BaseState) this.pre_erupt);
      this.idle.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle).Enter((StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State.Callback) (smi =>
      {
        if (!smi.master.ShouldGoDormant())
          return;
        smi.GoTo((StateMachine.BaseState) this.dormant);
      })).ScheduleGoTo((Func<Geyser.StatesInstance, float>) (smi => smi.master.RemainingIdleTime()), (StateMachine.BaseState) this.pre_erupt);
      this.pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding).ScheduleGoTo((Func<Geyser.StatesInstance, float>) (smi => smi.master.RemainingEruptPreTime()), (StateMachine.BaseState) this.erupt);
      this.erupt.DefaultState(this.erupt.erupting).ScheduleGoTo((Func<Geyser.StatesInstance, float>) (smi => smi.master.RemainingEruptTime()), (StateMachine.BaseState) this.post_erupt).Enter((StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State.Callback) (smi => smi.master.emitter.SetEmitting(true))).Exit((StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State.Callback) (smi => smi.master.emitter.SetEmitting(false)));
      this.erupt.erupting.EventTransition(GameHashes.EmitterBlocked, this.erupt.overpressure, (StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.Transition.ConditionCallback) (smi => smi.GetComponent<ElementEmitter>().isEmitterBlocked)).PlayAnim("erupt", KAnim.PlayMode.Loop);
      this.erupt.overpressure.EventTransition(GameHashes.EmitterUnblocked, this.erupt.erupting, (StateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<ElementEmitter>().isEmitterBlocked)).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure).PlayAnim("inactive", KAnim.PlayMode.Loop);
      this.post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle).ScheduleGoTo((Func<Geyser.StatesInstance, float>) (smi => smi.master.RemainingEruptPostTime()), (StateMachine.BaseState) this.idle);
    }

    public class EruptState : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State
    {
      public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State erupting;
      public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State overpressure;
    }
  }

  public enum Phase
  {
    Pre,
    On,
    Pst,
    Off,
    Any,
  }
}
