// Decompiled with JetBrains decompiler
// Type: AirFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter elementConverter;
  [MyCmpGet]
  private ElementConsumer elementConsumer;
  public Tag filterTag;

  public bool HasFilter()
  {
    return this.elementConverter.HasEnoughMass(this.filterTag);
  }

  public bool IsConvertable()
  {
    return this.elementConverter.HasEnoughMassToStartConverting();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }

  public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
  {
    public StatesInstance(AirFilter smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
  {
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State hasfilter;
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.waiting;
      this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasfilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.master.HasFilter())
          return smi.master.operational.IsOperational;
        return false;
      })).EventTransition(GameHashes.OperationalChanged, this.hasfilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.master.HasFilter())
          return smi.master.operational.IsOperational;
        return false;
      }));
      this.hasfilter.EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.IsConvertable())).EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).Enter("EnableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit("DisableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false)));
      this.converting.Enter("SetActive(true)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Exit("SetActive(false)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).Enter("EnableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit("DisableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false))).EventTransition(GameHashes.OnStorageChange, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.IsConvertable())).EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
    }
  }
}
