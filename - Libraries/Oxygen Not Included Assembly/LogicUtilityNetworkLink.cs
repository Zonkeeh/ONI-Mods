// Decompiled with JetBrains decompiler
// Type: LogicUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  protected override void OnConnect(int cell1, int cell2)
  {
    Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
  }

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem;
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
