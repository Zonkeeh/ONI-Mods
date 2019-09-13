// Decompiled with JetBrains decompiler
// Type: ScenePartitioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ScenePartitioner : ISim1000ms
{
  private static readonly Predicate<ScenePartitionerEntry> removeCallback = (Predicate<ScenePartitionerEntry>) (entry =>
  {
    if (entry != null)
      return entry.obj == null;
    return true;
  });
  public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();
  private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();
  private int nodeSize;
  private ScenePartitioner.ScenePartitionerNode[,,] nodes;
  private int queryId;

  public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
  {
    this.nodeSize = node_size;
    int length1 = scene_width / node_size;
    int length2 = scene_height / node_size;
    this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, length2, length1];
    SimAndRenderScheduler.instance.Add((object) this, false);
  }

  public void FreeResources()
  {
    for (int index1 = 0; index1 < this.nodes.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < this.nodes.GetLength(1); ++index2)
      {
        for (int index3 = 0; index3 < this.nodes.GetLength(2); ++index3)
        {
          for (int index4 = 0; index4 < this.nodes[index1, index2, index3].entries.Count; ++index4)
          {
            ScenePartitionerEntry entry = this.nodes[index1, index2, index3].entries[index4];
            if (entry != null)
            {
              entry.partitioner = (ScenePartitioner) null;
              entry.obj = (object) null;
            }
          }
          this.nodes[index1, index2, index3].entries.Clear();
        }
      }
    }
    this.nodes = (ScenePartitioner.ScenePartitionerNode[,,]) null;
  }

  public ScenePartitionerLayer CreateMask(HashedString name)
  {
    foreach (ScenePartitionerLayer layer in this.layers)
    {
      if (layer.name == name)
        return layer;
    }
    ScenePartitionerLayer partitionerLayer = new ScenePartitionerLayer(name, this.layers.Count);
    this.layers.Add(partitionerLayer);
    DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
    return partitionerLayer;
  }

  private int ClampNodeX(int x)
  {
    return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
  }

  private int ClampNodeY(int y)
  {
    return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
  }

  private Extents GetNodeExtents(int x, int y, int width, int height)
  {
    Extents extents = new Extents()
    {
      x = this.ClampNodeX(x / this.nodeSize),
      y = this.ClampNodeY(y / this.nodeSize)
    };
    extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
    extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
    return extents;
  }

  private Extents GetNodeExtents(ScenePartitionerEntry entry)
  {
    return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
  }

  private void Insert(ScenePartitionerEntry entry)
  {
    if (entry.obj == null)
    {
      Debug.LogWarning((object) "Trying to put null go into scene partitioner");
    }
    else
    {
      Extents nodeExtents = this.GetNodeExtents(entry);
      if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
        Debug.LogError((object) (entry.obj.ToString() + " x/w " + (object) nodeExtents.x + "/" + (object) nodeExtents.width + " < " + (object) this.nodes.GetLength(2)));
      if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
        Debug.LogError((object) (entry.obj.ToString() + " y/h " + (object) nodeExtents.y + "/" + (object) nodeExtents.height + " < " + (object) this.nodes.GetLength(1)));
      int layer = entry.layer;
      for (int y = nodeExtents.y; y < nodeExtents.y + nodeExtents.height; ++y)
      {
        for (int x = nodeExtents.x; x < nodeExtents.x + nodeExtents.width; ++x)
        {
          if (!this.nodes[layer, y, x].dirty)
          {
            this.nodes[layer, y, x].dirty = true;
            this.dirtyNodes.Add(new ScenePartitioner.DirtyNode()
            {
              layer = layer,
              x = x,
              y = y
            });
          }
          this.nodes[layer, y, x].entries.Add(entry);
        }
      }
    }
  }

  private void Widthdraw(ScenePartitionerEntry entry)
  {
    Extents nodeExtents = this.GetNodeExtents(entry);
    if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
      Debug.LogError((object) (" x/w " + (object) nodeExtents.x + "/" + (object) nodeExtents.width + " < " + (object) this.nodes.GetLength(2)));
    if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
      Debug.LogError((object) (" y/h " + (object) nodeExtents.y + "/" + (object) nodeExtents.height + " < " + (object) this.nodes.GetLength(1)));
    int layer = entry.layer;
    for (int y = nodeExtents.y; y < nodeExtents.y + nodeExtents.height; ++y)
    {
      for (int x = nodeExtents.x; x < nodeExtents.x + nodeExtents.width; ++x)
      {
        int count = this.nodes[layer, y, x].entries.Count;
        for (int index = 0; index < count; ++index)
        {
          if (this.nodes[layer, y, x].entries[index] == entry)
          {
            if (!this.nodes[layer, y, x].dirty)
            {
              this.nodes[layer, y, x].dirty = true;
              this.dirtyNodes.Add(new ScenePartitioner.DirtyNode()
              {
                layer = layer,
                x = x,
                y = y
              });
            }
            this.nodes[layer, y, x].entries[index] = (ScenePartitionerEntry) null;
            break;
          }
        }
      }
    }
  }

  public ScenePartitionerEntry Add(ScenePartitionerEntry entry)
  {
    this.Insert(entry);
    return entry;
  }

  public void UpdatePosition(int x, int y, ScenePartitionerEntry entry)
  {
    this.Widthdraw(entry);
    entry.x = x;
    entry.y = y;
    this.Insert(entry);
  }

  public void Remove(ScenePartitionerEntry entry)
  {
    Extents nodeExtents = this.GetNodeExtents(entry);
    if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
      Debug.LogError((object) (" x/w " + (object) nodeExtents.x + "/" + (object) nodeExtents.width + " < " + (object) this.nodes.GetLength(2)));
    if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
      Debug.LogError((object) (" y/h " + (object) nodeExtents.y + "/" + (object) nodeExtents.height + " < " + (object) this.nodes.GetLength(1)));
    int layer = entry.layer;
    for (int y = nodeExtents.y; y < nodeExtents.y + nodeExtents.height; ++y)
    {
      for (int x = nodeExtents.x; x < nodeExtents.x + nodeExtents.width; ++x)
      {
        if (!this.nodes[layer, y, x].dirty)
        {
          this.nodes[layer, y, x].dirty = true;
          this.dirtyNodes.Add(new ScenePartitioner.DirtyNode()
          {
            layer = layer,
            x = x,
            y = y
          });
        }
      }
    }
    entry.obj = (object) null;
  }

  public void Sim1000ms(float dt)
  {
    foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
    {
      this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries.RemoveAllSwap(ScenePartitioner.removeCallback);
      this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
    }
    this.dirtyNodes.Clear();
  }

  public void TriggerEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
  {
    ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
    ++this.queryId;
    for (int index = 0; index < cells.Count; ++index)
    {
      int x = 0;
      int y = 0;
      Grid.CellToXY(cells[index], out x, out y);
      this.GatherEntries(x, y, 1, 1, layer, event_data, (List<ScenePartitionerEntry>) pooledList, this.queryId);
    }
    this.RunLayerGlobalEvent(cells, layer, event_data);
    this.RunEntries((List<ScenePartitionerEntry>) pooledList, event_data);
    pooledList.Recycle();
  }

  public void TriggerEvent(HashSet<int> cells, ScenePartitionerLayer layer, object event_data)
  {
    ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
    ++this.queryId;
    foreach (int cell in cells)
    {
      int x = 0;
      int y = 0;
      Grid.CellToXY(cell, out x, out y);
      this.GatherEntries(x, y, 1, 1, layer, event_data, (List<ScenePartitionerEntry>) pooledList, this.queryId);
    }
    this.RunLayerGlobalEvent(cells, layer, event_data);
    this.RunEntries((List<ScenePartitionerEntry>) pooledList, event_data);
    pooledList.Recycle();
  }

  public void TriggerEvent(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data)
  {
    ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
    this.GatherEntries(x, y, width, height, layer, event_data, (List<ScenePartitionerEntry>) pooledList);
    this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
    this.RunEntries((List<ScenePartitionerEntry>) pooledList, event_data);
    pooledList.Recycle();
  }

  private void RunLayerGlobalEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
  {
    if (layer.OnEvent == null)
      return;
    for (int index = 0; index < cells.Count; ++index)
      layer.OnEvent(cells[index], event_data);
  }

  private void RunLayerGlobalEvent(
    HashSet<int> cells,
    ScenePartitionerLayer layer,
    object event_data)
  {
    if (layer.OnEvent == null)
      return;
    foreach (int cell in cells)
      layer.OnEvent(cell, event_data);
  }

  private void RunLayerGlobalEvent(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data)
  {
    if (layer.OnEvent == null)
      return;
    for (int y1 = y; y1 < y + height; ++y1)
    {
      for (int x1 = x; x1 < x + width; ++x1)
      {
        int cell = Grid.XYToCell(x1, y1);
        if (Grid.IsValidCell(cell))
          layer.OnEvent(cell, event_data);
      }
    }
  }

  private void RunEntries(List<ScenePartitionerEntry> gathered_entries, object event_data)
  {
    for (int index = 0; index < gathered_entries.Count; ++index)
    {
      ScenePartitionerEntry gatheredEntry = gathered_entries[index];
      if (gatheredEntry.obj != null && gatheredEntry.eventCallback != null)
        gatheredEntry.eventCallback(event_data);
    }
  }

  public void GatherEntries(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data,
    List<ScenePartitionerEntry> gathered_entries)
  {
    this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, ++this.queryId);
  }

  public void GatherEntries(
    int x,
    int y,
    int width,
    int height,
    ScenePartitionerLayer layer,
    object event_data,
    List<ScenePartitionerEntry> gathered_entries,
    int query_id)
  {
    Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
    int num1 = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
    int num2 = Math.Max(nodeExtents.y, 0);
    int num3 = Math.Max(nodeExtents.x, 0);
    int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
    int layer1 = layer.layer;
    for (int index1 = num2; index1 < num1; ++index1)
    {
      for (int index2 = num3; index2 < num4; ++index2)
      {
        int count = this.nodes[layer1, index1, index2].entries.Count;
        for (int index3 = 0; index3 < count; ++index3)
        {
          ScenePartitionerEntry entry = this.nodes[layer1, index1, index2].entries[index3];
          if (entry != null && entry.queryId != this.queryId)
          {
            if (entry.obj == null)
              this.nodes[layer1, index1, index2].entries[index3] = (ScenePartitionerEntry) null;
            else if (x + width - 1 >= entry.x && x <= entry.x + entry.width - 1 && (y + height - 1 >= entry.y && y <= entry.y + entry.height - 1))
            {
              entry.queryId = this.queryId;
              gathered_entries.Add(entry);
            }
          }
        }
      }
    }
  }

  public void Cleanup()
  {
    SimAndRenderScheduler.instance.Remove((object) this);
  }

  private struct ScenePartitionerNode
  {
    public ArrayRef<ScenePartitionerEntry> entries;
    public bool dirty;
  }

  private struct DirtyNode
  {
    public int layer;
    public int x;
    public int y;
  }
}
