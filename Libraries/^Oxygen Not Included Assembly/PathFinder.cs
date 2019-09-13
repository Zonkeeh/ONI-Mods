// Decompiled with JetBrains decompiler
// Type: PathFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PathFinder
{
  public static int InvalidHandle = -1;
  public static int InvalidIdx = -1;
  public static int InvalidCell = -1;
  private static readonly Func<int, bool> allowPathfindingFloodFillCb = (Func<int, bool>) (cell =>
  {
    if (Grid.Solid[cell] || Grid.AllowPathfinding[cell])
      return false;
    Grid.AllowPathfinding[cell] = true;
    return true;
  });
  public static PathGrid PathGrid;

  public static void Initialize()
  {
    NavType[] valid_nav_types = new NavType[10];
    for (int index = 0; index < valid_nav_types.Length; ++index)
      valid_nav_types[index] = (NavType) index;
    PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, valid_nav_types);
    for (int start_cell = 0; start_cell < Grid.CellCount; ++start_cell)
    {
      if (Grid.Visible[start_cell] > (byte) 0 || Grid.Spawnable[start_cell] > (byte) 0)
      {
        ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
        GameUtil.FloodFillConditional(start_cell, PathFinder.allowPathfindingFloodFillCb, (ICollection<int>) pooledList, (ICollection<int>) null);
        Grid.AllowPathfinding[start_cell] = true;
        pooledList.Recycle();
      }
    }
    System.Action<int> onReveal = Grid.OnReveal;
    // ISSUE: reference to a compiler-generated field
    if (PathFinder.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PathFinder.\u003C\u003Ef__mg\u0024cache0 = new System.Action<int>(PathFinder.OnReveal);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<int> fMgCache0 = PathFinder.\u003C\u003Ef__mg\u0024cache0;
    Grid.OnReveal = onReveal + fMgCache0;
  }

  private static void OnReveal(int cell)
  {
  }

  public static void UpdatePath(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    ref PathFinder.Path path)
  {
    PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
  }

  public static bool ValidatePath(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    ref PathFinder.Path path)
  {
    if (!path.IsValid())
      return false;
    for (int index1 = 0; index1 < path.nodes.Count; ++index1)
    {
      PathFinder.Path.Node node1 = path.nodes[index1];
      if (index1 < path.nodes.Count - 1)
      {
        PathFinder.Path.Node node2 = path.nodes[index1 + 1];
        int index2 = node1.cell * nav_grid.maxLinksPerCell;
        bool flag = false;
        for (NavGrid.Link link = nav_grid.Links[index2]; link.link != PathFinder.InvalidHandle; link = nav_grid.Links[index2])
        {
          if (link.link == node2.cell && node2.navType == link.endNavType && node1.navType == link.startNavType)
          {
            PathFinder.PotentialPath path1 = new PathFinder.PotentialPath(node1.cell, node1.navType, PathFinder.PotentialPath.Flags.None);
            flag = abilities.TraversePath(ref path1, node1.cell, node1.navType, 0, (int) link.transitionId, 0);
            if (flag)
              break;
          }
          ++index2;
        }
        if (!flag)
          return false;
      }
    }
    return true;
  }

  public static void Run(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query)
  {
    int invalidCell = PathFinder.InvalidCell;
    NavType result_nav_type = NavType.NumNavTypes;
    query.ClearResult();
    if (!Grid.IsValidCell(potential_path.cell))
      return;
    PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref result_nav_type);
    if (invalidCell == PathFinder.InvalidCell)
      return;
    bool is_cell_in_range = false;
    PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, result_nav_type, out is_cell_in_range);
    query.SetResult(invalidCell, cell.cost, result_nav_type);
  }

  public static void Run(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    ref PathFinder.Path path)
  {
    PathFinder.Run(nav_grid, abilities, potential_path, query);
    if (query.GetResultCell() != PathFinder.InvalidCell)
      PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
    else
      path.Clear();
  }

  private static void BuildResultPath(
    int path_cell,
    NavType path_nav_type,
    ref PathFinder.Path path)
  {
    if (path_cell == PathFinder.InvalidCell)
      return;
    bool is_cell_in_range = false;
    PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out is_cell_in_range);
    path.Clear();
    path.cost = cell.cost;
    while (path_cell != PathFinder.InvalidCell)
    {
      path.AddNode(new PathFinder.Path.Node()
      {
        cell = path_cell,
        navType = cell.navType,
        transitionId = cell.transitionId
      });
      path_cell = cell.parent;
      if (path_cell != PathFinder.InvalidCell)
        cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out is_cell_in_range);
    }
    if (path.nodes == null)
      return;
    for (int index = 0; index < path.nodes.Count / 2; ++index)
    {
      PathFinder.Path.Node node = path.nodes[index];
      path.nodes[index] = path.nodes[path.nodes.Count - index - 1];
      path.nodes[path.nodes.Count - index - 1] = node;
    }
  }

  private static void FindPaths(
    NavGrid nav_grid,
    ref PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    PathFinder.PotentialList potentials,
    ref int result_cell,
    ref NavType result_nav_type)
  {
    potentials.Clear();
    PathFinder.PathGrid.ResetUpdate();
    PathFinder.PathGrid.BeginUpdate(potential_path.cell, false);
    bool is_cell_in_range;
    PathFinder.Cell cell1 = PathFinder.PathGrid.GetCell(potential_path, out is_cell_in_range);
    PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, -1, potentials, PathFinder.PathGrid, ref cell1);
    int maxValue = int.MaxValue;
    while (potentials.Count > 0)
    {
      KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
      PathFinder.Cell cell2 = PathFinder.PathGrid.GetCell(keyValuePair.Value, out is_cell_in_range);
      if (cell2.cost == keyValuePair.Key)
      {
        if (cell2.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell2.parent, cell2.cost) && cell2.cost < maxValue)
        {
          result_cell = keyValuePair.Value.cell;
          int cost = cell2.cost;
          result_nav_type = cell2.navType;
          break;
        }
        PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell2.cost, (int) cell2.underwaterCost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell2.parent, cell2.parentNavType);
      }
    }
    PathFinder.PathGrid.EndUpdate(true);
  }

  public static void AddPotential(
    PathFinder.PotentialPath potential_path,
    int parent_cell,
    NavType parent_nav_type,
    int cost,
    int underwater_cost,
    int transition_id,
    PathFinder.PotentialList potentials,
    PathGrid path_grid,
    ref PathFinder.Cell cell_data)
  {
    cell_data.cost = cost;
    cell_data.underwaterCost = (byte) Math.Min(underwater_cost, (int) byte.MaxValue);
    cell_data.parent = parent_cell;
    cell_data.navType = potential_path.navType;
    cell_data.parentNavType = parent_nav_type;
    cell_data.transitionId = transition_id;
    potentials.Add(cost, potential_path);
    path_grid.SetCell(potential_path, ref cell_data);
  }

  [Conditional("ENABLE_PATH_DETAILS")]
  private static void BeginDetailSample(string region_name)
  {
  }

  [Conditional("ENABLE_PATH_DETAILS")]
  private static void EndDetailSample()
  {
  }

  public static bool IsSubmerged(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int cell1 = Grid.CellAbove(cell);
    return Grid.IsValidCell(cell1) && Grid.Element[cell1].IsLiquid || Grid.Element[cell].IsLiquid && Grid.IsValidCell(cell1) && Grid.Element[cell1].IsSolid;
  }

  public static void AddPotentials(
    PathFinder.PotentialScratchPad potential_scratch_pad,
    PathFinder.PotentialPath potential,
    int cost,
    int underwater_cost,
    ref PathFinderAbilities abilities,
    PathFinderQuery query,
    int max_links_per_cell,
    NavGrid.Link[] links,
    PathFinder.PotentialList potentials,
    PathGrid path_grid,
    int parent_cell,
    NavType parent_nav_type)
  {
    int num1 = 0;
    NavGrid.Link[] withCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
    int index1 = potential.cell * max_links_per_cell;
    NavGrid.Link link1 = links[index1];
    for (int link2 = link1.link; link2 != PathFinder.InvalidHandle; link2 = link1.link)
    {
      if (link1.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link1.startNavType))
        withCorrectNavType[num1++] = link1;
      ++index1;
      link1 = links[index1];
    }
    int num2 = 0;
    PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
    for (int index2 = 0; index2 < num1; ++index2)
    {
      NavGrid.Link link2 = withCorrectNavType[index2];
      int link3 = link2.link;
      bool is_cell_in_range = false;
      PathFinder.Cell cell = path_grid.GetCell(link3, link2.endNavType, out is_cell_in_range);
      if (is_cell_in_range)
      {
        int num3 = cost + (int) link2.cost;
        bool flag1 = cell.cost == -1;
        bool flag2 = num3 < cell.cost;
        if (flag1 || flag2)
          linksInCellRange[num2++] = new PathFinder.PotentialScratchPad.PathGridCellData()
          {
            pathGridCell = cell,
            link = link2
          };
      }
    }
    for (int index2 = 0; index2 < num2; ++index2)
    {
      PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[index2];
      int link2 = pathGridCellData.link.link;
      pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link2);
      linksInCellRange[index2] = pathGridCellData;
    }
    for (int index2 = 0; index2 < num2; ++index2)
    {
      PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[index2];
      NavGrid.Link link2 = pathGridCellData.link;
      int link3 = link2.link;
      PathFinder.Cell pathGridCell = pathGridCellData.pathGridCell;
      int cost1 = cost + (int) link2.cost;
      PathFinder.PotentialPath path = potential;
      path.cell = link3;
      path.navType = link2.endNavType;
      int underwater_cost1;
      if (pathGridCellData.isSubmerged)
      {
        underwater_cost1 = underwater_cost + 1;
        int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(path, link2);
        cost1 += submergedPathCostPenalty;
      }
      else
        underwater_cost1 = 0;
      PathFinder.PotentialPath.Flags flags = path.flags;
      bool flag = abilities.TraversePath(ref path, potential.cell, potential.navType, cost1, (int) link2.transitionId, underwater_cost1);
      if (path.flags != flags)
        KProfiler.AddEvent("NavChange");
      if (flag)
        PathFinder.AddPotential(path, potential.cell, potential.navType, cost1, underwater_cost1, (int) link2.transitionId, potentials, path_grid, ref pathGridCell);
    }
  }

  public struct Cell
  {
    public int queryId;
    public int cost;
    public int parent;
    public byte underwaterCost;
    public NavType navType;
    public NavType parentNavType;
    public int transitionId;
  }

  public struct PotentialPath
  {
    public int cell;
    public NavType navType;

    public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
    {
      this.cell = cell;
      this.navType = nav_type;
      this.flags = flags;
    }

    public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
    {
      this.flags |= new_flags;
    }

    public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
    {
      this.flags &= ~new_flags;
    }

    public bool HasFlag(PathFinder.PotentialPath.Flags flag)
    {
      return this.HasAnyFlag(flag);
    }

    public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
    {
      return (this.flags & mask) != PathFinder.PotentialPath.Flags.None;
    }

    public PathFinder.PotentialPath.Flags flags { get; private set; }

    [System.Flags]
    public enum Flags : byte
    {
      None = 0,
      HasAtmoSuit = 1,
      HasJetPack = 2,
      PerformSuitChecks = 4,
    }
  }

  public struct Path
  {
    public int cost;
    public List<PathFinder.Path.Node> nodes;

    public void AddNode(PathFinder.Path.Node node)
    {
      if (this.nodes == null)
        this.nodes = new List<PathFinder.Path.Node>();
      this.nodes.Add(node);
    }

    public bool IsValid()
    {
      if (this.nodes != null)
        return this.nodes.Count > 1;
      return false;
    }

    public bool HasArrived()
    {
      if (this.nodes != null)
        return this.nodes.Count > 0;
      return false;
    }

    public void Clear()
    {
      this.cost = 0;
      if (this.nodes == null)
        return;
      this.nodes.Clear();
    }

    public struct Node
    {
      public int cell;
      public NavType navType;
      public int transitionId;
    }
  }

  public class PotentialList
  {
    private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

    public KeyValuePair<int, PathFinder.PotentialPath> Next()
    {
      return this.queue.Dequeue();
    }

    public int Count
    {
      get
      {
        return this.queue.Count;
      }
    }

    public void Add(int cost, PathFinder.PotentialPath path)
    {
      this.queue.Enqueue(cost, path);
    }

    public void Clear()
    {
      this.queue.Clear();
    }

    public class PriorityQueue<TValue>
    {
      private List<KeyValuePair<int, TValue>> _baseHeap;

      public PriorityQueue()
      {
        this._baseHeap = new List<KeyValuePair<int, TValue>>();
      }

      public void Enqueue(int priority, TValue value)
      {
        this.Insert(priority, value);
      }

      public KeyValuePair<int, TValue> Dequeue()
      {
        KeyValuePair<int, TValue> keyValuePair = this._baseHeap[0];
        this.DeleteRoot();
        return keyValuePair;
      }

      public KeyValuePair<int, TValue> Peek()
      {
        if (this.Count > 0)
          return this._baseHeap[0];
        throw new InvalidOperationException("Priority queue is empty");
      }

      private void ExchangeElements(int pos1, int pos2)
      {
        KeyValuePair<int, TValue> keyValuePair = this._baseHeap[pos1];
        this._baseHeap[pos1] = this._baseHeap[pos2];
        this._baseHeap[pos2] = keyValuePair;
      }

      private void Insert(int priority, TValue value)
      {
        this._baseHeap.Add(new KeyValuePair<int, TValue>(priority, value));
        this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
      }

      private int HeapifyFromEndToBeginning(int pos)
      {
        if (pos >= this._baseHeap.Count)
          return -1;
        int pos1;
        for (; pos > 0; pos = pos1)
        {
          pos1 = (pos - 1) / 2;
          if (this._baseHeap[pos1].Key - this._baseHeap[pos].Key > 0)
            this.ExchangeElements(pos1, pos);
          else
            break;
        }
        return pos;
      }

      private void DeleteRoot()
      {
        if (this._baseHeap.Count <= 1)
        {
          this._baseHeap.Clear();
        }
        else
        {
          this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
          this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
          this.HeapifyFromBeginningToEnd(0);
        }
      }

      private void HeapifyFromBeginningToEnd(int pos)
      {
        int count = this._baseHeap.Count;
        if (pos >= count)
          return;
        while (true)
        {
          int pos1 = pos;
          int index1 = 2 * pos + 1;
          int index2 = 2 * pos + 2;
          if (index1 < count && this._baseHeap[pos1].Key - this._baseHeap[index1].Key > 0)
            pos1 = index1;
          if (index2 < count && this._baseHeap[pos1].Key - this._baseHeap[index2].Key > 0)
            pos1 = index2;
          if (pos1 != pos)
          {
            this.ExchangeElements(pos1, pos);
            pos = pos1;
          }
          else
            break;
        }
      }

      public void Clear()
      {
        this._baseHeap.Clear();
      }

      public int Count
      {
        get
        {
          return this._baseHeap.Count;
        }
      }
    }

    private class HOTQueue<TValue>
    {
      private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();
      private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();
      private int hotThreshold = int.MinValue;
      private int coldThreshold = int.MinValue;
      private int count;

      public KeyValuePair<int, TValue> Dequeue()
      {
        if (this.hotQueue.Count == 0)
        {
          PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = this.hotQueue;
          this.hotQueue = this.coldQueue;
          this.coldQueue = hotQueue;
          this.hotThreshold = this.coldThreshold;
        }
        --this.count;
        return this.hotQueue.Dequeue();
      }

      public void Enqueue(int priority, TValue value)
      {
        if (priority <= this.hotThreshold)
        {
          this.hotQueue.Enqueue(priority, value);
        }
        else
        {
          this.coldQueue.Enqueue(priority, value);
          this.coldThreshold = Math.Max(this.coldThreshold, priority);
        }
        ++this.count;
      }

      public KeyValuePair<int, TValue> Peek()
      {
        if (this.hotQueue.Count == 0)
        {
          PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = this.hotQueue;
          this.hotQueue = this.coldQueue;
          this.coldQueue = hotQueue;
          this.hotThreshold = this.coldThreshold;
        }
        return this.hotQueue.Peek();
      }

      public void Clear()
      {
        this.count = 0;
        this.hotThreshold = int.MinValue;
        this.hotQueue.Clear();
        this.coldThreshold = int.MinValue;
        this.coldQueue.Clear();
      }

      public int Count
      {
        get
        {
          return this.count;
        }
      }
    }
  }

  private class Temp
  {
    public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
  }

  public class PotentialScratchPad
  {
    public NavGrid.Link[] linksWithCorrectNavType;
    public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

    public PotentialScratchPad(int max_links_per_cell)
    {
      this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
      this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
    }

    public struct PathGridCellData
    {
      public PathFinder.Cell pathGridCell;
      public NavGrid.Link link;
      public bool isSubmerged;
    }
  }
}
