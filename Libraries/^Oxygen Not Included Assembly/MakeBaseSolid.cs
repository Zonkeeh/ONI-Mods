// Decompiled with JetBrains decompiler
// Type: MakeBaseSolid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MakeBaseSolid : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (MakeBaseSolid.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MakeBaseSolid.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToSolid);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback fMgCache0 = MakeBaseSolid.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State state = root.Enter(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (MakeBaseSolid.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      MakeBaseSolid.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToVacuum);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback fMgCache1 = MakeBaseSolid.\u003C\u003Ef__mg\u0024cache1;
    state.Exit(fMgCache1);
  }

  private static void ConvertToSolid(MakeBaseSolid.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    PrimaryElement component = smi.GetComponent<PrimaryElement>();
    SimMessages.ReplaceAndDisplaceElement(cell, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature, byte.MaxValue, 0, -1);
    Grid.Objects[cell, 9] = smi.gameObject;
    Grid.Foundation[cell] = true;
    Grid.SetSolid(cell, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
    Grid.RenderedByWorld[cell] = false;
    World.Instance.OnSolidChanged(cell);
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
  }

  private static void ConvertToVacuum(MakeBaseSolid.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    SimMessages.ReplaceAndDisplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0.0f, -1f, byte.MaxValue, 0, -1);
    Grid.Objects[cell, 9] = (GameObject) null;
    Grid.Foundation[cell] = false;
    Grid.SetSolid(cell, false, CellEventLogger.Instance.SimCellOccupierDestroy);
    Grid.RenderedByWorld[cell] = true;
    World.Instance.OnSolidChanged(cell);
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, MakeBaseSolid.Def def)
      : base(master, def)
    {
    }
  }
}
