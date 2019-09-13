// Decompiled with JetBrains decompiler
// Type: PlantableCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlantableCellQuery : PathFinderQuery
{
  public List<int> result_cells = new List<int>();
  private int plantDetectionRadius = 6;
  private int maxPlantsInRadius = 2;
  private PlantableSeed seed;
  private int max_results;

  public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
  {
    this.seed = seed;
    this.max_results = max_results;
    this.result_cells.Clear();
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
      this.result_cells.Add(cell);
    return this.result_cells.Count >= this.max_results;
  }

  private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
  {
    if (!Grid.IsValidCell(plant_cell))
      return false;
    int cell = seed.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(plant_cell) : Grid.CellAbove(plant_cell);
    if (!Grid.IsValidCell(cell) || !Grid.Solid[cell] || ((bool) ((Object) Grid.Objects[plant_cell, 5]) || (bool) ((Object) Grid.Objects[plant_cell, 1])))
      return false;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((bool) ((Object) gameObject))
    {
      PlantablePlot component = gameObject.GetComponent<PlantablePlot>();
      if ((Object) component == (Object) null || component.Direction != seed.Direction || (Object) component.Occupant != (Object) null)
        return false;
    }
    else if (!seed.TestSuitableGround(plant_cell) || PlantableCellQuery.CountNearbyPlants(cell, this.plantDetectionRadius) > this.maxPlantsInRadius)
      return false;
    return true;
  }

  private static int CountNearbyPlants(int cell, int radius)
  {
    int x = 0;
    int y = 0;
    Grid.PosToXY(Grid.CellToPos(cell), out x, out y);
    int num1 = radius * 2;
    int x_bottomLeft = x - radius;
    int y_bottomLeft = y - radius;
    ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, num1, num1, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) pooledList);
    int num2 = 0;
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) pooledList)
    {
      if (!(bool) ((Object) ((Component) partitionerEntry.obj).GetComponent<TreeBud>()))
        ++num2;
    }
    pooledList.Recycle();
    return num2;
  }
}
