// Decompiled with JetBrains decompiler
// Type: ReceptacleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
  private bool replanted;

  public bool Replanted
  {
    get
    {
      return this.replanted;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public PlantablePlot GetReceptacle()
  {
    return (PlantablePlot) this.smi.sm.receptacle.Get(this.smi);
  }

  public void SetReceptacle(PlantablePlot plot = null)
  {
    if ((UnityEngine.Object) plot == (UnityEngine.Object) null)
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) null, this.smi);
      this.replanted = false;
    }
    else
    {
      this.smi.sm.receptacle.Set((SingleEntityReceptacle) plot, this.smi);
      this.replanted = true;
    }
  }

  public void Sim1000ms(float dt)
  {
    if ((UnityEngine.Object) this.smi.sm.receptacle.Get(this.smi) == (UnityEngine.Object) null)
    {
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.wild);
    }
    else
    {
      Operational component = this.smi.sm.receptacle.Get(this.smi).GetComponent<Operational>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational);
      else if (component.IsOperational)
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational);
      else
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.inoperational);
    }
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.Receptacle
      };
    }
  }

  public string WiltStateString
  {
    get
    {
      string empty = string.Empty;
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.inoperational))
        empty += (string) CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
      return empty;
    }
  }

  public bool HasReceptacle()
  {
    return !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.wild);
  }

  public bool HasOperationalReceptacle()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.operational);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
    };
  }

  public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
  {
    public StatesInstance(ReceptacleMonitor master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
  {
    public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;
    public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.wild;
      this.serializable = true;
      this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational, (Func<ReceptacleMonitor.StatesInstance, object>) null);
      this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational, (Func<ReceptacleMonitor.StatesInstance, object>) null);
      this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational, (Func<ReceptacleMonitor.StatesInstance, object>) null);
    }
  }
}
