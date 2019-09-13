// Decompiled with JetBrains decompiler
// Type: UtilityNetworkManager`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilityNetworkManager<NetworkType, ItemType> : IUtilityNetworkMgr
  where NetworkType : UtilityNetwork, new()
  where ItemType : MonoBehaviour
{
  private Dictionary<int, object> items = new Dictionary<int, object>();
  private Dictionary<int, object> endpoints = new Dictionary<int, object>();
  private Dictionary<int, int> links = new Dictionary<int, int>();
  private Queue<int> queued = new Queue<int>();
  private int tileLayer = -1;
  private List<UtilityNetwork> networks;
  private HashSet<int> visitedCells;
  private System.Action<IList<UtilityNetwork>, ICollection<int>> onNetworksRebuilt;
  protected UtilityNetworkGridNode[] visualGrid;
  private UtilityNetworkGridNode[] stashedVisualGrid;
  protected UtilityNetworkGridNode[] physicalGrid;
  protected HashSet<int> physicalNodes;
  protected HashSet<int> visualNodes;
  private bool dirty;

  public UtilityNetworkManager(int game_width, int game_height, int tile_layer)
  {
    this.tileLayer = tile_layer;
    this.networks = new List<UtilityNetwork>();
    this.Initialize(game_width, game_height);
  }

  public bool IsDirty
  {
    get
    {
      return this.dirty;
    }
  }

  public void Initialize(int game_width, int game_height)
  {
    this.networks.Clear();
    this.physicalGrid = new UtilityNetworkGridNode[game_width * game_height];
    this.visualGrid = new UtilityNetworkGridNode[game_width * game_height];
    this.stashedVisualGrid = new UtilityNetworkGridNode[game_width * game_height];
    this.physicalNodes = new HashSet<int>();
    this.visualNodes = new HashSet<int>();
    this.visitedCells = new HashSet<int>();
    for (int index = 0; index < this.visualGrid.Length; ++index)
    {
      this.visualGrid[index] = new UtilityNetworkGridNode()
      {
        networkIdx = -1,
        connections = (UtilityConnections) 0
      };
      this.physicalGrid[index] = new UtilityNetworkGridNode()
      {
        networkIdx = -1,
        connections = (UtilityConnections) 0
      };
    }
  }

  public void Update()
  {
    if (!this.dirty)
      return;
    this.dirty = false;
    for (int index = 0; index < this.networks.Count; ++index)
      this.networks[index].Reset(this.physicalGrid);
    this.networks.Clear();
    this.RebuildNetworks(this.tileLayer, false);
    this.RebuildNetworks(this.tileLayer, true);
    if (this.onNetworksRebuilt == null)
      return;
    this.onNetworksRebuilt((IList<UtilityNetwork>) this.networks, (ICollection<int>) this.GetNodes(true));
  }

  protected UtilityNetworkGridNode[] GetGrid(bool is_physical_building)
  {
    if (is_physical_building)
      return this.physicalGrid;
    return this.visualGrid;
  }

  private HashSet<int> GetNodes(bool is_physical_building)
  {
    if (is_physical_building)
      return this.physicalNodes;
    return this.visualNodes;
  }

  public void ClearCell(int cell, bool is_physical_building)
  {
    if (Game.IsQuitting())
      return;
    UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
    HashSet<int> nodes = this.GetNodes(is_physical_building);
    UtilityConnections connections = grid[cell].connections;
    grid[cell].connections = (UtilityConnections) 0;
    Vector2I xy = Grid.CellToXY(cell);
    if (xy.x > 1 && (connections & UtilityConnections.Left) != (UtilityConnections) 0)
      grid[Grid.CellLeft(cell)].connections &= ~UtilityConnections.Right;
    if (xy.x < Grid.WidthInCells - 1 && (connections & UtilityConnections.Right) != (UtilityConnections) 0)
      grid[Grid.CellRight(cell)].connections &= ~UtilityConnections.Left;
    if (xy.y > 1 && (connections & UtilityConnections.Down) != (UtilityConnections) 0)
      grid[Grid.CellBelow(cell)].connections &= ~UtilityConnections.Up;
    if (xy.y < Grid.HeightInCells - 1 && (connections & UtilityConnections.Up) != (UtilityConnections) 0)
      grid[Grid.CellAbove(cell)].connections &= ~UtilityConnections.Down;
    nodes.Remove(cell);
    if (!is_physical_building)
      return;
    this.dirty = true;
    this.ClearCell(cell, false);
  }

  private void QueueCellForVisit(
    UtilityNetworkGridNode[] grid,
    int dest_cell,
    UtilityConnections direction)
  {
    if (!Grid.IsValidCell(dest_cell) || this.visitedCells.Contains(dest_cell) || direction != (UtilityConnections) 0 && (grid[dest_cell].connections & direction.InverseDirection()) == (UtilityConnections) 0 || !((UnityEngine.Object) Grid.Objects[dest_cell, this.tileLayer] != (UnityEngine.Object) null))
      return;
    this.visitedCells.Add(dest_cell);
    this.queued.Enqueue(dest_cell);
  }

  public void ForceRebuildNetworks()
  {
    this.dirty = true;
  }

  public void AddToNetworks(int cell, object item, bool is_endpoint)
  {
    this.dirty = true;
    if (item == null)
      return;
    if (is_endpoint)
    {
      if (this.endpoints.ContainsKey(cell))
        DebugUtil.LogWarningArgs((object) "Cell", (object) cell, (object) "already has a utility network endpoint assigned. Adding", (object) item.ToString(), (object) "will stomp previous endpoint");
      this.endpoints[cell] = item;
    }
    else
    {
      if (this.items.ContainsKey(cell))
        DebugUtil.LogWarningArgs((object) "Cell", (object) cell, (object) "already has a utility network connector assigned. Adding", (object) item.ToString(), (object) "will stomp previous item");
      this.items[cell] = item;
    }
  }

  private unsafe void Reconnect(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    // ISSUE: untyped stack allocation
    int* numPtr1 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    // ISSUE: untyped stack allocation
    int* numPtr2 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    // ISSUE: untyped stack allocation
    int* numPtr3 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    int index1 = 0;
    if (xy.y < Grid.HeightInCells - 1)
    {
      numPtr1[index1] = Grid.CellAbove(cell);
      numPtr2[index1] = 4;
      numPtr3[index1] = 8;
      ++index1;
    }
    if (xy.y > 1)
    {
      numPtr1[index1] = Grid.CellBelow(cell);
      numPtr2[index1] = 8;
      numPtr3[index1] = 4;
      ++index1;
    }
    if (xy.x > 1)
    {
      numPtr1[index1] = Grid.CellLeft(cell);
      numPtr2[index1] = 1;
      numPtr3[index1] = 2;
      ++index1;
    }
    if (xy.x < Grid.WidthInCells - 1)
    {
      numPtr1[index1] = Grid.CellRight(cell);
      numPtr2[index1] = 2;
      numPtr3[index1] = 1;
      ++index1;
    }
    UtilityConnections connections1 = this.physicalGrid[cell].connections;
    UtilityConnections connections2 = this.visualGrid[cell].connections;
    for (int index2 = 0; index2 < index1; ++index2)
    {
      int index3 = numPtr1[index2];
      UtilityConnections utilityConnections1 = (UtilityConnections) numPtr2[index2];
      UtilityConnections utilityConnections2 = (UtilityConnections) numPtr3[index2];
      if ((connections1 & utilityConnections1) != (UtilityConnections) 0)
      {
        if (this.physicalNodes.Contains(index3))
          this.physicalGrid[index3].connections |= utilityConnections2;
        if (this.visualNodes.Contains(index3))
          this.visualGrid[index3].connections |= utilityConnections2;
      }
      else if ((connections2 & utilityConnections1) != (UtilityConnections) 0 && (this.physicalNodes.Contains(index3) || this.visualNodes.Contains(index3)))
        this.visualGrid[index3].connections |= utilityConnections2;
    }
  }

  public void RemoveFromNetworks(int cell, object item, bool is_endpoint)
  {
    if (Game.IsQuitting())
      return;
    this.dirty = true;
    if (item == null)
      return;
    if (is_endpoint)
    {
      this.endpoints.Remove(cell);
      int networkIdx = this.physicalGrid[cell].networkIdx;
      if (networkIdx == -1)
        return;
      this.networks[networkIdx].RemoveItem(cell, item);
    }
    else
    {
      int networkIdx = this.physicalGrid[cell].networkIdx;
      this.physicalGrid[cell].connections = (UtilityConnections) 0;
      this.physicalGrid[cell].networkIdx = -1;
      this.items.Remove(cell);
      this.Disconnect(cell);
      object obj;
      if (!this.endpoints.TryGetValue(cell, out obj) || networkIdx == -1)
        return;
      this.networks[networkIdx].DisconnectItem(cell, obj);
    }
  }

  private unsafe void Disconnect(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    int index1 = 0;
    // ISSUE: untyped stack allocation
    int* numPtr1 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    // ISSUE: untyped stack allocation
    int* numPtr2 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    if (xy.y < Grid.HeightInCells - 1)
    {
      numPtr1[index1] = Grid.CellAbove(cell);
      numPtr2[index1] = -9;
      ++index1;
    }
    if (xy.y > 1)
    {
      numPtr1[index1] = Grid.CellBelow(cell);
      numPtr2[index1] = -5;
      ++index1;
    }
    if (xy.x > 1)
    {
      numPtr1[index1] = Grid.CellLeft(cell);
      numPtr2[index1] = -3;
      ++index1;
    }
    if (xy.x < Grid.WidthInCells - 1)
    {
      numPtr1[index1] = Grid.CellRight(cell);
      numPtr2[index1] = -2;
      ++index1;
    }
    for (int index2 = 0; index2 < index1; ++index2)
    {
      int index3 = numPtr1[index2];
      int num1 = numPtr2[index2];
      int num2 = (int) (this.physicalGrid[index3].connections & (UtilityConnections) num1);
      this.physicalGrid[index3].connections = (UtilityConnections) num2;
    }
  }

  private unsafe void RebuildNetworks(int layer, bool is_physical)
  {
    UtilityNetworkGridNode[] grid = this.GetGrid(is_physical);
    HashSet<int> nodes = this.GetNodes(is_physical);
    this.visitedCells.Clear();
    this.queued.Clear();
    // ISSUE: untyped stack allocation
    int* numPtr1 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    // ISSUE: untyped stack allocation
    int* numPtr2 = (int*) __untypedstackalloc((int) checked (4U * 4U));
    foreach (int index1 in nodes)
    {
      UtilityNetworkGridNode utilityNetworkGridNode1 = grid[index1];
      if (!this.visitedCells.Contains(index1))
      {
        this.queued.Enqueue(index1);
        this.visitedCells.Add(index1);
        NetworkType networkType = new NetworkType();
        networkType.id = this.networks.Count;
        this.networks.Add((UtilityNetwork) networkType);
        while (this.queued.Count > 0)
        {
          int index2 = this.queued.Dequeue();
          UtilityNetworkGridNode utilityNetworkGridNode2 = grid[index2];
          object obj1 = (object) null;
          object obj2 = (object) null;
          if (is_physical)
          {
            if (this.items.TryGetValue(index2, out obj1))
            {
              if (!(obj1 is IDisconnectable) || !(obj1 as IDisconnectable).IsDisconnected())
              {
                if (obj1 != null)
                  networkType.AddItem(index2, obj1);
              }
              else
                continue;
            }
            if (this.endpoints.TryGetValue(index2, out obj2) && obj2 != null)
              networkType.AddItem(index2, obj2);
          }
          grid[index2].networkIdx = networkType.id;
          if (obj1 != null && obj2 != null)
            networkType.ConnectItem(index2, obj2);
          Vector2I xy = Grid.CellToXY(index2);
          int index3 = 0;
          if (xy.x >= 0)
          {
            numPtr1[index3] = Grid.CellLeft(index2);
            numPtr2[index3] = 1;
            ++index3;
          }
          if (xy.x < Grid.WidthInCells)
          {
            numPtr1[index3] = Grid.CellRight(index2);
            numPtr2[index3] = 2;
            ++index3;
          }
          if (xy.y >= 0)
          {
            numPtr1[index3] = Grid.CellBelow(index2);
            numPtr2[index3] = 8;
            ++index3;
          }
          if (xy.y < Grid.HeightInCells)
          {
            numPtr1[index3] = Grid.CellAbove(index2);
            numPtr2[index3] = 4;
            ++index3;
          }
          for (int index4 = 0; index4 < index3; ++index4)
          {
            int num = numPtr2[index4];
            if ((utilityNetworkGridNode2.connections & (UtilityConnections) num) != (UtilityConnections) 0)
            {
              int dest_cell = numPtr1[index4];
              this.QueueCellForVisit(grid, dest_cell, (UtilityConnections) num);
            }
          }
          int dest_cell1;
          if (this.links.TryGetValue(index2, out dest_cell1))
            this.QueueCellForVisit(grid, dest_cell1, (UtilityConnections) 0);
        }
      }
    }
  }

  public UtilityNetwork GetNetworkByID(int id)
  {
    UtilityNetwork utilityNetwork = (UtilityNetwork) null;
    if (0 <= id && id < this.networks.Count)
      utilityNetwork = this.networks[id];
    return utilityNetwork;
  }

  public UtilityNetwork GetNetworkForCell(int cell)
  {
    UtilityNetwork utilityNetwork = (UtilityNetwork) null;
    if (Grid.IsValidCell(cell) && 0 <= this.physicalGrid[cell].networkIdx && this.physicalGrid[cell].networkIdx < this.networks.Count)
      utilityNetwork = this.networks[this.physicalGrid[cell].networkIdx];
    return utilityNetwork;
  }

  public UtilityNetwork GetNetworkForDirection(int cell, Direction direction)
  {
    cell = Grid.GetCellInDirection(cell, direction);
    if (!Grid.IsValidCell(cell))
      return (UtilityNetwork) null;
    UtilityNetworkGridNode utilityNetworkGridNode = this.GetGrid(true)[cell];
    UtilityNetwork utilityNetwork = (UtilityNetwork) null;
    if (utilityNetworkGridNode.networkIdx != -1 && utilityNetworkGridNode.networkIdx < this.networks.Count)
      utilityNetwork = this.networks[utilityNetworkGridNode.networkIdx];
    return utilityNetwork;
  }

  private UtilityConnections GetNeighboursAsConnections(
    int cell,
    HashSet<int> nodes)
  {
    UtilityConnections utilityConnections = (UtilityConnections) 0;
    Vector2I xy = Grid.CellToXY(cell);
    if (xy.x > 1 && nodes.Contains(Grid.CellLeft(cell)))
      utilityConnections |= UtilityConnections.Left;
    if (xy.x < Grid.WidthInCells - 1 && nodes.Contains(Grid.CellRight(cell)))
      utilityConnections |= UtilityConnections.Right;
    if (xy.y > 1 && nodes.Contains(Grid.CellBelow(cell)))
      utilityConnections |= UtilityConnections.Down;
    if (xy.y < Grid.HeightInCells - 1 && nodes.Contains(Grid.CellAbove(cell)))
      utilityConnections |= UtilityConnections.Up;
    return utilityConnections;
  }

  public virtual void SetConnections(
    UtilityConnections connections,
    int cell,
    bool is_physical_building)
  {
    HashSet<int> nodes = this.GetNodes(is_physical_building);
    nodes.Add(cell);
    this.visualGrid[cell].connections = connections;
    if (is_physical_building)
    {
      this.dirty = true;
      UtilityConnections utilityConnections = !is_physical_building ? connections : connections & this.GetNeighboursAsConnections(cell, nodes);
      this.physicalGrid[cell].connections = utilityConnections;
    }
    this.Reconnect(cell);
  }

  public UtilityConnections GetConnections(int cell, bool is_physical_building)
  {
    UtilityConnections connections = this.GetGrid(is_physical_building)[cell].connections;
    if (!is_physical_building)
    {
      UtilityNetworkGridNode[] grid = this.GetGrid(true);
      connections |= grid[cell].connections;
    }
    return connections;
  }

  public UtilityConnections GetDisplayConnections(int cell)
  {
    return (UtilityConnections) 0 | this.GetGrid(false)[cell].connections | this.GetGrid(true)[cell].connections;
  }

  public virtual bool CanAddConnection(
    UtilityConnections new_connection,
    int cell,
    bool is_physical_building,
    out string fail_reason)
  {
    fail_reason = (string) null;
    return true;
  }

  public void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building)
  {
    string fail_reason;
    if (!this.CanAddConnection(new_connection, cell, is_physical_building, out fail_reason))
      return;
    if (is_physical_building)
      this.dirty = true;
    UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
    UtilityConnections connections = grid[cell].connections;
    grid[cell].connections = connections | new_connection;
  }

  public void StashVisualGrids()
  {
    Array.Copy((Array) this.visualGrid, (Array) this.stashedVisualGrid, this.visualGrid.Length);
  }

  public void UnstashVisualGrids()
  {
    Array.Copy((Array) this.stashedVisualGrid, (Array) this.visualGrid, this.visualGrid.Length);
  }

  public string GetVisualizerString(int cell)
  {
    return this.GetVisualizerString(this.GetDisplayConnections(cell));
  }

  public string GetVisualizerString(UtilityConnections connections)
  {
    string str = string.Empty;
    if ((connections & UtilityConnections.Left) != (UtilityConnections) 0)
      str += "L";
    if ((connections & UtilityConnections.Right) != (UtilityConnections) 0)
      str += "R";
    if ((connections & UtilityConnections.Up) != (UtilityConnections) 0)
      str += "U";
    if ((connections & UtilityConnections.Down) != (UtilityConnections) 0)
      str += "D";
    if (str == string.Empty)
      str = "None";
    return str;
  }

  public object GetEndpoint(int cell)
  {
    object obj = (object) null;
    this.endpoints.TryGetValue(cell, out obj);
    return obj;
  }

  public void AddLink(int cell1, int cell2)
  {
    this.links[cell1] = cell2;
    this.links[cell2] = cell1;
    this.dirty = true;
  }

  public void RemoveLink(int cell1, int cell2)
  {
    this.links.Remove(cell1);
    this.links.Remove(cell2);
    this.dirty = true;
  }

  public void AddNetworksRebuiltListener(
    System.Action<IList<UtilityNetwork>, ICollection<int>> listener)
  {
    this.onNetworksRebuilt += listener;
  }

  public void RemoveNetworksRebuiltListener(
    System.Action<IList<UtilityNetwork>, ICollection<int>> listener)
  {
    this.onNetworksRebuilt -= listener;
  }

  public IList<UtilityNetwork> GetNetworks()
  {
    return (IList<UtilityNetwork>) this.networks;
  }
}
