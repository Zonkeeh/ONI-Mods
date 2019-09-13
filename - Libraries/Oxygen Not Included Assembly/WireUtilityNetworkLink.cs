// Decompiled with JetBrains decompiler
// Type: WireUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IUtilityNetworkItem, IBridgedNetworkItem
{
  [SerializeField]
  public Wire.WattageRating maxWattageRating;

  public Wire.WattageRating GetMaxWattageRating()
  {
    return this.maxWattageRating;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
    Game.Instance.circuitManager.Disconnect(this);
  }

  protected override void OnConnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
    Game.Instance.circuitManager.Connect(this);
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem;
  }

  public ushort NetworkID
  {
    get
    {
      int linked_cell1;
      int linked_cell2;
      this.GetCells(out linked_cell1, out linked_cell2);
      ElectricalUtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(linked_cell1) as ElectricalUtilityNetwork;
      if (networkForCell != null)
        return (ushort) networkForCell.id;
      return ushort.MaxValue;
    }
  }

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    int linked_cell1;
    int linked_cell2;
    this.GetCells(out linked_cell1, out linked_cell2);
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(linked_cell1);
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    int linked_cell1;
    int linked_cell2;
    this.GetCells(out linked_cell1, out linked_cell2);
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(linked_cell1);
    return networks.Contains(networkForCell);
  }
}
