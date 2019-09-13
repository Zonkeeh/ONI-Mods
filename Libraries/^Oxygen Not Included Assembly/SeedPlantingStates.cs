// Decompiled with JetBrains decompiler
// Type: SeedPlantingStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using UnityEngine;

public class SeedPlantingStates : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>
{
  private const int MAX_NAVIGATE_DISTANCE = 100;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State pickupSeed;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlantLocation;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlot;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToDirt;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting;
  public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.findSeed;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State root = this.root;
    string name1 = (string) CREATURES.STATUSITEMS.PLANTINGSEED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state1 = root.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, NotificationType.Neutral, false, render_overlay, 129022, (Func<string, SeedPlantingStates.Instance, string>) null, (Func<string, SeedPlantingStates.Instance, string>) null, category);
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.UnreserveSeed);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache0 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state2 = state1.Exit(fMgCache0);
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.DropAll);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache1 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state3 = state2.Exit(fMgCache1);
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache2 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache2;
    state3.Exit(fMgCache2);
    this.findSeed.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      SeedPlantingStates.FindSeed(smi);
      if ((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null)
      {
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
      {
        SeedPlantingStates.ReserveSeed(smi);
        smi.GoTo((StateMachine.BaseState) this.moveToSeed);
      }
    }));
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed = this.moveToSeed;
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache3 = new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetSeedCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SeedPlantingStates.Instance, int> fMgCache3 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation = this.findPlantLocation;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete1 = this.behaviourcomplete;
    moveToSeed.MoveTo(fMgCache3, findPlantLocation, behaviourcomplete1, false);
    this.findPlantLocation.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if ((bool) ((UnityEngine.Object) smi.targetSeed))
      {
        SeedPlantingStates.FindDirtPlot(smi);
        if ((UnityEngine.Object) smi.targetPlot != (UnityEngine.Object) null || smi.targetDirtPlotCell != Grid.InvalidCell)
          smi.GoTo((StateMachine.BaseState) this.pickupSeed);
        else
          smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state4 = this.pickupSeed.PlayAnim("gather");
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PickupComplete);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache4 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache4;
    state4.Enter(fMgCache4).OnAnimQueueComplete(this.moveToPlantLocation);
    this.moveToPlantLocation.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      else if ((UnityEngine.Object) smi.targetPlot != (UnityEngine.Object) null)
        smi.GoTo((StateMachine.BaseState) this.moveToPlot);
      else if (smi.targetDirtPlotCell != Grid.InvalidCell)
        smi.GoTo((StateMachine.BaseState) this.moveToDirt);
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.moveToDirt.MoveTo((Func<SeedPlantingStates.Instance, int>) (smi => smi.targetDirtPlotCell), this.planting, this.behaviourcomplete, false);
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state5 = this.moveToPlot.Enter((StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback) (smi =>
    {
      if (!((UnityEngine.Object) smi.targetPlot == (UnityEngine.Object) null) && !((UnityEngine.Object) smi.targetSeed == (UnityEngine.Object) null))
        return;
      smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache5 = new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetPlantableCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<SeedPlantingStates.Instance, int> fMgCache5 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting1 = this.planting;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete2 = this.behaviourcomplete;
    state5.MoveTo(fMgCache5, planting1, behaviourcomplete2, false);
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting2 = this.planting;
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache6 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache6;
    GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State state6 = planting2.Enter(fMgCache6).PlayAnim("plant");
    // ISSUE: reference to a compiler-generated field
    if (SeedPlantingStates.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SeedPlantingStates.\u003C\u003Ef__mg\u0024cache7 = new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PlantComplete);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback fMgCache7 = SeedPlantingStates.\u003C\u003Ef__mg\u0024cache7;
    state6.Exit(fMgCache7).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToPlantSeed, false);
  }

  private static void AddMouthOverride(SeedPlantingStates.Instance smi)
  {
    SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
    KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "sq_mouth_cheeks");
    if (symbol == null)
      return;
    component.AddSymbolOverride((HashedString) "sq_mouth", symbol, 0);
  }

  private static void RemoveMouthOverride(SeedPlantingStates.Instance smi)
  {
    smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) "sq_mouth", 0);
  }

  private static void PickupComplete(SeedPlantingStates.Instance smi)
  {
    if (!(bool) ((UnityEngine.Object) smi.targetSeed))
    {
      Debug.LogWarningFormat("PickupComplete seed {0} is null", (object) smi.targetSeed);
    }
    else
    {
      SeedPlantingStates.UnreserveSeed(smi);
      int cell = Grid.PosToCell((KMonoBehaviour) smi.targetSeed);
      if (smi.seed_cell != cell)
      {
        Debug.LogWarningFormat("PickupComplete seed {0} moved {1} != {2}", (object) smi.targetSeed, (object) cell, (object) smi.seed_cell);
        smi.targetSeed = (Pickupable) null;
      }
      else if (smi.targetSeed.HasTag(GameTags.Stored))
      {
        Debug.LogWarningFormat("PickupComplete seed {0} was stored by {1}", (object) smi.targetSeed, (object) smi.targetSeed.storage);
        smi.targetSeed = (Pickupable) null;
      }
      else
      {
        smi.targetSeed = EntitySplitter.Split(smi.targetSeed, 1f, (GameObject) null);
        smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject, false, false, true, false);
        SeedPlantingStates.AddMouthOverride(smi);
      }
    }
  }

  private static void PlantComplete(SeedPlantingStates.Instance smi)
  {
    PlantableSeed seed = !(bool) ((UnityEngine.Object) smi.targetSeed) ? (PlantableSeed) null : smi.targetSeed.GetComponent<PlantableSeed>();
    PlantablePlot plot;
    if ((bool) ((UnityEngine.Object) seed) && SeedPlantingStates.CheckValidPlotCell(smi, seed, smi.targetDirtPlotCell, out plot))
    {
      if ((bool) ((UnityEngine.Object) plot))
      {
        if ((UnityEngine.Object) plot.Occupant == (UnityEngine.Object) null)
          plot.ForceDepositPickupable(smi.targetSeed);
      }
      else
        seed.TryPlant(true);
    }
    smi.targetSeed = (Pickupable) null;
    smi.seed_cell = Grid.InvalidCell;
    smi.targetPlot = (PlantablePlot) null;
  }

  private static void DropAll(SeedPlantingStates.Instance smi)
  {
    smi.GetComponent<Storage>().DropAll(false, false, new Vector3(), true);
  }

  private static int GetPlantableCell(SeedPlantingStates.Instance smi)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) smi.targetPlot);
    if (Grid.IsValidCell(cell))
      return Grid.CellAbove(cell);
    return cell;
  }

  private static void FindDirtPlot(SeedPlantingStates.Instance smi)
  {
    smi.targetDirtPlotCell = Grid.InvalidCell;
    PlantableSeed component = smi.targetSeed.GetComponent<PlantableSeed>();
    PlantableCellQuery plantableCellQuery = PathFinderQueries.plantableCellQuery.Reset(component, 20);
    smi.GetComponent<Navigator>().RunQuery((PathFinderQuery) plantableCellQuery);
    if (plantableCellQuery.result_cells.Count <= 0)
      return;
    smi.targetDirtPlotCell = plantableCellQuery.result_cells[UnityEngine.Random.Range(0, plantableCellQuery.result_cells.Count)];
  }

  private static bool CheckValidPlotCell(
    SeedPlantingStates.Instance smi,
    PlantableSeed seed,
    int cell,
    out PlantablePlot plot)
  {
    plot = (PlantablePlot) null;
    if (!Grid.IsValidCell(cell))
      return false;
    int cell1 = seed.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(cell) : Grid.CellAbove(cell);
    if (!Grid.IsValidCell(cell1) || !Grid.Solid[cell1])
      return false;
    GameObject gameObject = Grid.Objects[cell1, 1];
    if (!(bool) ((UnityEngine.Object) gameObject))
      return seed.TestSuitableGround(cell);
    plot = gameObject.GetComponent<PlantablePlot>();
    return (UnityEngine.Object) plot != (UnityEngine.Object) null;
  }

  private static int GetSeedCell(SeedPlantingStates.Instance smi)
  {
    Debug.Assert((bool) ((UnityEngine.Object) smi.targetSeed));
    Debug.Assert(smi.seed_cell != Grid.InvalidCell);
    return smi.seed_cell;
  }

  private static void FindSeed(SeedPlantingStates.Instance smi)
  {
    Navigator component = smi.GetComponent<Navigator>();
    Pickupable pickupable = (Pickupable) null;
    int num = 100;
    IEnumerator enumerator = Components.Pickupables.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Pickupable current = (Pickupable) enumerator.Current;
        if ((current.HasTag(GameTags.Seed) || current.HasTag(GameTags.CropSeed)) && (!current.HasTag(GameTags.Creatures.ReservedByCreature) && (double) Vector2.Distance((Vector2) smi.transform.position, (Vector2) current.transform.position) <= 25.0))
        {
          int navigationCost = component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) current));
          if (navigationCost != -1 && navigationCost < num)
          {
            pickupable = current;
            num = navigationCost;
          }
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    smi.targetSeed = pickupable;
    smi.seed_cell = !(bool) ((UnityEngine.Object) smi.targetSeed) ? Grid.InvalidCell : Grid.PosToCell((KMonoBehaviour) smi.targetSeed);
  }

  private static void ReserveSeed(SeedPlantingStates.Instance smi)
  {
    GameObject go = !(bool) ((UnityEngine.Object) smi.targetSeed) ? (GameObject) null : smi.targetSeed.gameObject;
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void UnreserveSeed(SeedPlantingStates.Instance smi)
  {
    GameObject go = !(bool) ((UnityEngine.Object) smi.targetSeed) ? (GameObject) null : smi.targetSeed.gameObject;
    if (!((UnityEngine.Object) smi.targetSeed != (UnityEngine.Object) null))
      return;
    go.RemoveTag(GameTags.Creatures.ReservedByCreature);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.GameInstance
  {
    public int targetDirtPlotCell = Grid.InvalidCell;
    public Element plantElement = ElementLoader.FindElementByHash(SimHashes.Dirt);
    public int seed_cell = Grid.InvalidCell;
    public PlantablePlot targetPlot;
    public Pickupable targetSeed;

    public Instance(Chore<SeedPlantingStates.Instance> chore, SeedPlantingStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToPlantSeed);
    }
  }
}
