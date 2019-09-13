// Decompiled with JetBrains decompiler
// Type: PlantElementEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private KSelectable selectable;
  public SimHashes emittedElement;
  public float emitRate;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>();
  }

  public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
  {
    public StatesInstance(PlantElementEmitter master)
      : base(master)
    {
    }

    public bool IsWilting()
    {
      if ((UnityEngine.Object) this.master.wiltCondition == (UnityEngine.Object) null || !((UnityEngine.Object) this.master.wiltCondition != (UnityEngine.Object) null))
        return false;
      return this.master.wiltCondition.IsWilting();
    }
  }

  public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
  {
    public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;
    public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.healthy;
      this.serializable = true;
      this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (StateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.Transition.ConditionCallback) (smi => smi.IsWilting())).Update("PlantEmit", (System.Action<PlantElementEmitter.StatesInstance, float>) ((smi, dt) => SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1)), UpdateRate.SIM_4000ms, false);
      this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, (StateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.Transition.ConditionCallback) null);
    }
  }
}
