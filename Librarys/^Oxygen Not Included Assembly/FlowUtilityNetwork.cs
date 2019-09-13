// Decompiled with JetBrains decompiler
// Type: FlowUtilityNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class FlowUtilityNetwork : UtilityNetwork
{
  public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();
  public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();
  private List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

  public bool HasSinks
  {
    get
    {
      return this.sinks.Count > 0;
    }
  }

  public int GetActiveCount()
  {
    return this.sinks.Count;
  }

  public override void AddItem(int cell, object generic_item)
  {
    FlowUtilityNetwork.IItem obj = (FlowUtilityNetwork.IItem) generic_item;
    if (obj == null)
      return;
    switch (obj.EndpointType)
    {
      case Endpoint.Source:
        if (this.sources.Contains(obj))
          break;
        this.sources.Add(obj);
        obj.Network = this;
        break;
      case Endpoint.Sink:
        if (this.sinks.Contains(obj))
          break;
        this.sinks.Add(obj);
        obj.Network = this;
        break;
      case Endpoint.Conduit:
        break;
      default:
        obj.Network = this;
        break;
    }
  }

  public override void Reset(UtilityNetworkGridNode[] grid)
  {
    for (int index = 0; index < this.sinks.Count; ++index)
    {
      FlowUtilityNetwork.IItem sink = this.sinks[index];
      sink.Network = (FlowUtilityNetwork) null;
      UtilityNetworkGridNode utilityNetworkGridNode = grid[sink.Cell];
      utilityNetworkGridNode.networkIdx = -1;
      grid[sink.Cell] = utilityNetworkGridNode;
    }
    for (int index = 0; index < this.sources.Count; ++index)
    {
      FlowUtilityNetwork.IItem source = this.sources[index];
      source.Network = (FlowUtilityNetwork) null;
      UtilityNetworkGridNode utilityNetworkGridNode = grid[source.Cell];
      utilityNetworkGridNode.networkIdx = -1;
      grid[source.Cell] = utilityNetworkGridNode;
    }
    for (int index = 0; index < this.conduits.Count; ++index)
    {
      FlowUtilityNetwork.IItem conduit = this.conduits[index];
      conduit.Network = (FlowUtilityNetwork) null;
      UtilityNetworkGridNode utilityNetworkGridNode = grid[conduit.Cell];
      utilityNetworkGridNode.networkIdx = -1;
      grid[conduit.Cell] = utilityNetworkGridNode;
    }
  }

  public interface IItem
  {
    int Cell { get; }

    FlowUtilityNetwork Network { set; }

    Endpoint EndpointType { get; }

    ConduitType ConduitType { get; }

    GameObject GameObject { get; }
  }

  public class NetworkItem : FlowUtilityNetwork.IItem
  {
    private int cell;
    private FlowUtilityNetwork network;
    private Endpoint endpointType;
    private ConduitType conduitType;
    private GameObject parent;

    public NetworkItem(
      ConduitType conduit_type,
      Endpoint endpoint_type,
      int cell,
      GameObject parent)
    {
      this.conduitType = conduit_type;
      this.endpointType = endpoint_type;
      this.cell = cell;
      this.parent = parent;
    }

    public Endpoint EndpointType
    {
      get
      {
        return this.endpointType;
      }
    }

    public ConduitType ConduitType
    {
      get
      {
        return this.conduitType;
      }
    }

    public int Cell
    {
      get
      {
        return this.cell;
      }
    }

    public FlowUtilityNetwork Network
    {
      get
      {
        return this.network;
      }
      set
      {
        this.network = value;
      }
    }

    public GameObject GameObject
    {
      get
      {
        return this.parent;
      }
    }
  }
}
