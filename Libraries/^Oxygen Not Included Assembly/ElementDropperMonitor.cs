// Decompiled with JetBrains decompiler
// Type: ElementDropperMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ElementDropperMonitor : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
  public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;
  public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;
  public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventHandler(GameHashes.DeathAnimComplete, (StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State.Callback) (smi => smi.DropDeathElement()));
    this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (Func<ElementDropperMonitor.Instance, bool>) (smi => smi.ShouldDropElement()));
    this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<ElementDropperMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.satisfied))).EventHandler(GameHashes.ObjectMovementStateChanged, (GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameEvent.Callback) ((smi, d) =>
    {
      if ((GameHashes) d != GameHashes.ObjectMovementWakeUp)
        return;
      smi.GoTo((StateMachine.BaseState) this.satisfied);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
    public byte emitDiseaseIdx = byte.MaxValue;
    public SimHashes dirtyEmitElement;
    public float dirtyProbabilityPercent;
    public float dirtyCellToTargetMass;
    public float dirtyMassPerDirty;
    public float dirtyMassReleaseOnDeath;
    public float emitDiseasePerKg;
  }

  public class Instance : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def)
      : base(master, def)
    {
      Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "ElementDropperMonitor.Instance");
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    }

    private void OnCellChange()
    {
      this.sm.cellChangedSignal.Trigger(this);
    }

    public bool ShouldDropElement()
    {
      if (this.IsValidDropCell())
        return (double) UnityEngine.Random.Range(0.0f, 100f) < (double) this.def.dirtyProbabilityPercent;
      return false;
    }

    public void DropDeathElement()
    {
      this.DropElement(this.def.dirtyMassReleaseOnDeath, this.def.dirtyEmitElement, this.def.emitDiseaseIdx, Mathf.RoundToInt(this.def.dirtyMassReleaseOnDeath * this.def.dirtyMassPerDirty));
    }

    public void DropPeriodicElement()
    {
      this.DropElement(this.def.dirtyMassPerDirty, this.def.dirtyEmitElement, this.def.emitDiseaseIdx, Mathf.RoundToInt(this.def.emitDiseasePerKg * this.def.dirtyMassPerDirty));
    }

    public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
    {
      if ((double) mass <= 0.0)
        return;
      Element elementByHash = ElementLoader.FindElementByHash(element_id);
      float temperature = this.GetComponent<PrimaryElement>().Temperature;
      if (elementByHash.IsGas || elementByHash.IsLiquid)
        SimMessages.AddRemoveSubstance(Grid.PosToCell(this.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count, true, -1);
      else if (elementByHash.IsSolid)
        elementByHash.substance.SpawnResource(this.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f), mass, temperature, disease_idx, disease_count, false, true, false);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, elementByHash.name, this.gameObject.transform, 1.5f, false);
    }

    public bool IsValidDropCell()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return Grid.IsValidCell(cell) && Grid.IsGas(cell) && (double) Grid.Mass[cell] <= 1.0;
    }
  }
}
