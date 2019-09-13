// Decompiled with JetBrains decompiler
// Type: DecompositionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance>
{
  [SerializeField]
  public int remainingRotMonsters = 3;
  public StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.FloatParameter decomposition;
  public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public DecompositionMonitor.RottenState rotten;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = true;
    this.satisfied.Update("UpdateDecomposition", (System.Action<DecompositionMonitor.Instance, float>) ((smi, dt) => smi.UpdateDecomposition(dt)), UpdateRate.SIM_200ms, false).ParamTransition<float>((StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.decomposition, (GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State) this.rotten, GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.IsGTEOne).ToggleAttributeModifier("Dead", (Func<DecompositionMonitor.Instance, AttributeModifier>) (smi => smi.satisfiedDecorModifier), (Func<DecompositionMonitor.Instance, bool>) null).ToggleAttributeModifier("Dead", (Func<DecompositionMonitor.Instance, AttributeModifier>) (smi => smi.satisfiedDecorRadiusModifier), (Func<DecompositionMonitor.Instance, bool>) null);
    this.rotten.DefaultState((GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State) this.rotten.exposed).ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, (object) null).ToggleAttributeModifier("Rotten", (Func<DecompositionMonitor.Instance, AttributeModifier>) (smi => smi.rottenDecorModifier), (Func<DecompositionMonitor.Instance, bool>) null).ToggleAttributeModifier("Rotten", (Func<DecompositionMonitor.Instance, AttributeModifier>) (smi => smi.rottenDecorRadiusModifier), (Func<DecompositionMonitor.Instance, bool>) null);
    this.rotten.exposed.DefaultState(this.rotten.exposed.openair).EventTransition(GameHashes.OnStore, this.rotten.stored, (StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsExposed()));
    this.rotten.exposed.openair.Enter((StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!smi.spawnsRotMonsters)
        return;
      smi.ScheduleGoTo(UnityEngine.Random.Range(150f, 300f), (StateMachine.BaseState) this.rotten.spawningmonster);
    })).Transition((GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State) this.rotten.exposed.submerged, (StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsSubmerged()), UpdateRate.SIM_200ms).ToggleFX((Func<DecompositionMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) this.CreateFX(smi)));
    this.rotten.exposed.submerged.DefaultState(this.rotten.exposed.submerged.idle).Transition(this.rotten.exposed.openair, (StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsSubmerged()), UpdateRate.SIM_200ms);
    this.rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, (StateMachine.BaseState) this.rotten.exposed.submerged.dirtywater);
    this.rotten.exposed.submerged.dirtywater.Enter("DirtyWater", (StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.DirtyWater(smi.dirtyWaterMaxRange))).GoTo(this.rotten.exposed.submerged.idle);
    this.rotten.spawningmonster.Enter((StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (this.remainingRotMonsters > 0)
      {
        --this.remainingRotMonsters;
        GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), smi.transform.GetPosition(), Grid.SceneLayer.Creatures, (string) null, 0).SetActive(true);
      }
      smi.GoTo((StateMachine.BaseState) this.rotten.exposed);
    }));
    this.rotten.stored.EventTransition(GameHashes.OnStore, (GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State) this.rotten.exposed, (StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsExposed()));
  }

  private FliesFX.Instance CreateFX(DecompositionMonitor.Instance smi)
  {
    if (!smi.isMasterNull)
      return new FliesFX.Instance(smi.master, new Vector3(0.0f, 0.0f, -0.1f));
    return (FliesFX.Instance) null;
  }

  public class SubmergedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State dirtywater;
  }

  public class ExposedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
  {
    public DecompositionMonitor.SubmergedState submerged;
    public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State openair;
  }

  public class RottenState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
  {
    public DecompositionMonitor.ExposedState exposed;
    public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State stored;
    public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State spawningmonster;
  }

  public class Instance : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public int dirtyWaterMaxRange = 3;
    public bool spawnsRotMonsters = true;
    public AttributeModifier satisfiedDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, (string) DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);
    public AttributeModifier satisfiedDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, (string) DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);
    public AttributeModifier rottenDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, (string) DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
    public AttributeModifier rottenDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, (string) DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
    public float decompositionRate;
    public Klei.AI.Disease disease;

    public Instance(
      IStateMachineTarget master,
      Klei.AI.Disease disease,
      float decompositionRate = 0.0008333334f,
      bool spawnRotMonsters = true)
      : base(master)
    {
      this.gameObject.AddComponent<DecorProvider>();
      this.decompositionRate = decompositionRate;
      this.disease = disease;
      this.spawnsRotMonsters = spawnRotMonsters;
    }

    public void UpdateDecomposition(float dt)
    {
      double num = (double) this.sm.decomposition.Delta(dt * this.decompositionRate, this.smi);
    }

    public bool IsExposed()
    {
      KPrefabID component = this.smi.GetComponent<KPrefabID>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
        return !component.HasTag(GameTags.Preserved);
      return true;
    }

    public bool IsRotten()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.rotten);
    }

    public bool IsSubmerged()
    {
      return PathFinder.IsSubmerged(Grid.PosToCell(this.master.transform.GetPosition()));
    }

    public void DirtyWater(int maxCellRange = 3)
    {
      int cell = Grid.PosToCell(this.master.transform.GetPosition());
      if (Grid.Element[cell].id == SimHashes.Water)
      {
        SimMessages.ReplaceElement(cell, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[cell], Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], -1);
      }
      else
      {
        if (Grid.Element[cell].id != SimHashes.DirtyWater)
          return;
        int[] numArray = new int[4];
        for (int x = 0; x < maxCellRange; ++x)
        {
          for (int y = 0; y < maxCellRange; ++y)
          {
            numArray[0] = Grid.OffsetCell(cell, new CellOffset(-x, y));
            numArray[1] = Grid.OffsetCell(cell, new CellOffset(x, y));
            numArray[2] = Grid.OffsetCell(cell, new CellOffset(-x, -y));
            numArray[3] = Grid.OffsetCell(cell, new CellOffset(x, -y));
            ((IList<int>) numArray).Shuffle<int>();
            foreach (int index in numArray)
            {
              if (Grid.GetCellDistance(cell, index) < maxCellRange - 1 && Grid.IsValidCell(index) && Grid.Element[index].id == SimHashes.Water)
              {
                SimMessages.ReplaceElement(index, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[index], Grid.Temperature[index], Grid.DiseaseIdx[index], Grid.DiseaseCount[index], -1);
                return;
              }
            }
          }
        }
      }
    }
  }
}
