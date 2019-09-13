// Decompiled with JetBrains decompiler
// Type: PathProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

[SkipSaveFileSerialization]
public class PathProber : KMonoBehaviour
{
  private PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
  public int updateCount = -1;
  public int potentialCellsPerUpdate = -1;
  public const int InvalidHandle = -1;
  public const int InvalidIdx = -1;
  public const int InvalidCell = -1;
  public const int InvalidCost = -1;
  private PathGrid PathGrid;
  private const int updateCountThreshold = 25;
  private PathFinder.PotentialScratchPad scratchPad;

  protected override void OnCleanUp()
  {
    if (this.PathGrid != null)
      this.PathGrid.OnCleanUp();
    base.OnCleanUp();
  }

  public void SetGroupProber(IGroupProber group_prober)
  {
    this.PathGrid.SetGroupProber(group_prober);
  }

  public void SetValidNavTypes(NavType[] nav_types, int max_probing_radius)
  {
    if (max_probing_radius != 0)
      this.PathGrid = new PathGrid(max_probing_radius * 2, max_probing_radius * 2, true, nav_types);
    else
      this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, nav_types);
  }

  public int GetCost(int cell)
  {
    return this.PathGrid.GetCost(cell);
  }

  public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
  {
    return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
  }

  public PathGrid GetPathGrid()
  {
    return this.PathGrid;
  }

  public void UpdateProbe(
    NavGrid nav_grid,
    int cell,
    NavType nav_type,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath.Flags flags)
  {
    if (this.scratchPad == null)
      this.scratchPad = new PathFinder.PotentialScratchPad(nav_grid.maxLinksPerCell);
    bool flag1 = this.updateCount == -1;
    bool flag2 = this.Potentials.Count == 0 || flag1;
    this.PathGrid.BeginUpdate(cell, !flag2);
    bool is_cell_in_range;
    if (flag2)
    {
      this.updateCount = 0;
      PathFinder.Cell cell1 = this.PathGrid.GetCell(cell, nav_type, out is_cell_in_range);
      PathFinder.AddPotential(new PathFinder.PotentialPath(cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, -1, this.Potentials, this.PathGrid, ref cell1);
    }
    int num = this.potentialCellsPerUpdate <= 0 || flag1 ? int.MaxValue : this.potentialCellsPerUpdate;
    ++this.updateCount;
    while (this.Potentials.Count > 0 && num > 0)
    {
      KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = this.Potentials.Next();
      --num;
      PathFinder.Cell cell1 = this.PathGrid.GetCell(keyValuePair.Value, out is_cell_in_range);
      if (cell1.cost == keyValuePair.Key)
        PathFinder.AddPotentials(this.scratchPad, keyValuePair.Value, cell1.cost, (int) cell1.underwaterCost, ref abilities, (PathFinderQuery) null, nav_grid.maxLinksPerCell, nav_grid.Links, this.Potentials, this.PathGrid, cell1.parent, cell1.parentNavType);
    }
    bool isComplete = this.Potentials.Count == 0;
    this.PathGrid.EndUpdate(isComplete);
    if (!isComplete || this.updateCount <= 25)
      return;
    KProfiler.AddEvent("PathProberUpdateCountExceeded");
  }
}
