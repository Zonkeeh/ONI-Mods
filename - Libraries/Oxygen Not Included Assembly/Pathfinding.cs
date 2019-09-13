// Decompiled with JetBrains decompiler
// Type: Pathfinding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class Pathfinding : KMonoBehaviour
{
  private List<NavGrid> NavGrids = new List<NavGrid>();
  private int UpdateIdx;
  private bool navGridsHaveBeenFlushedOnLoad;
  public static Pathfinding Instance;

  public static void DestroyInstance()
  {
    Pathfinding.Instance = (Pathfinding) null;
    OffsetTableTracker.OnPathfindingInvalidated();
  }

  protected override void OnPrefabInit()
  {
    Pathfinding.Instance = this;
  }

  public void AddNavGrid(NavGrid nav_grid)
  {
    this.NavGrids.Add(nav_grid);
  }

  public NavGrid GetNavGrid(string id)
  {
    foreach (NavGrid navGrid in this.NavGrids)
    {
      if (navGrid.id == id)
        return navGrid;
    }
    Debug.LogError((object) ("Could not find nav grid: " + id));
    return (NavGrid) null;
  }

  public void ResetNavGrids()
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.InitializeGraph();
  }

  public void FlushNavGridsOnLoad()
  {
    if (this.navGridsHaveBeenFlushedOnLoad)
      return;
    this.navGridsHaveBeenFlushedOnLoad = true;
    this.UpdateNavGrids(true);
  }

  public void UpdateNavGrids(bool update_all = false)
  {
    update_all = true;
    if (update_all)
    {
      foreach (NavGrid navGrid in this.NavGrids)
        navGrid.UpdateGraph();
    }
    else
    {
      foreach (NavGrid navGrid in this.NavGrids)
      {
        if (navGrid.updateEveryFrame)
          navGrid.UpdateGraph();
      }
      this.NavGrids[this.UpdateIdx].UpdateGraph();
      this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
    }
  }

  public void RenderEveryTick()
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.DebugUpdate();
  }

  public void AddDirtyNavGridCell(int cell)
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.AddDirtyCell(cell);
  }

  public void RefreshNavCell(int cell)
  {
    HashSet<int> dirty_nav_cells = new HashSet<int>();
    dirty_nav_cells.Add(cell);
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.UpdateGraph(dirty_nav_cells);
  }

  protected override void OnCleanUp()
  {
    this.NavGrids.Clear();
    OffsetTableTracker.OnPathfindingInvalidated();
  }
}
