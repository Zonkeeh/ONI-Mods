// Decompiled with JetBrains decompiler
// Type: IUtilityNetworkMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IUtilityNetworkMgr
{
  bool CanAddConnection(
    UtilityConnections new_connection,
    int cell,
    bool is_physical_building,
    out string fail_reason);

  void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

  void StashVisualGrids();

  void UnstashVisualGrids();

  string GetVisualizerString(int cell);

  string GetVisualizerString(UtilityConnections connections);

  UtilityConnections GetConnections(int cell, bool is_physical_building);

  UtilityConnections GetDisplayConnections(int cell);

  void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

  void ClearCell(int cell, bool is_physical_building);

  void ForceRebuildNetworks();

  void AddToNetworks(int cell, object item, bool is_endpoint);

  void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

  object GetEndpoint(int cell);

  UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

  UtilityNetwork GetNetworkForCell(int cell);

  void AddNetworksRebuiltListener(
    System.Action<IList<UtilityNetwork>, ICollection<int>> listener);

  void RemoveNetworksRebuiltListener(
    System.Action<IList<UtilityNetwork>, ICollection<int>> listener);

  IList<UtilityNetwork> GetNetworks();
}
