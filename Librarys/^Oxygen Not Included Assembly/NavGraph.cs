// Decompiled with JetBrains decompiler
// Type: NavGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class NavGraph
{
  private HandleVector<NavGraph.NavGraphEdge>.Handle[] grid;
  private HandleVector<NavGraph.NavGraphEdge> edges;

  public NavGraph(int cell_count, NavGrid nav_grid)
  {
    this.grid = new HandleVector<NavGraph.NavGraphEdge>.Handle[nav_grid.maxLinksPerCell * cell_count];
    for (int index = 0; index < this.grid.Length; ++index)
      this.grid[index] = HandleVector<NavGraph.NavGraphEdge>.InvalidHandle;
    this.edges = new HandleVector<NavGraph.NavGraphEdge>(cell_count);
    for (int index1 = 0; index1 < cell_count; ++index1)
    {
      int index2 = index1 * nav_grid.maxLinksPerCell;
      for (NavGrid.Link link = nav_grid.Links[index2]; link.link != NavGrid.InvalidHandle; link = nav_grid.Links[index2])
      {
        HandleVector<NavGraph.NavGraphEdge>.Handle handle = this.edges.Add(new NavGraph.NavGraphEdge()
        {
          startNavType = link.startNavType,
          endNavType = link.endNavType,
          endCell = link.link,
          startCell = index1
        });
        this.grid[index2] = handle;
        ++index2;
      }
      this.grid[index2] = HandleVector<NavGraph.NavGraphEdge>.InvalidHandle;
    }
  }

  public void Cleanup()
  {
  }

  private struct NavGraphEdge
  {
    public NavType startNavType;
    public NavType endNavType;
    public int startCell;
    public int endCell;
  }
}
