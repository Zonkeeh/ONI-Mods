// Decompiled with JetBrains decompiler
// Type: UtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class UtilityNetworkLink : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>((System.Action<UtilityNetworkLink, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>((System.Action<UtilityNetworkLink, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  [MyCmpGet]
  private Rotatable rotatable;
  [SerializeField]
  public CellOffset link1;
  [SerializeField]
  public CellOffset link2;
  [SerializeField]
  public bool visualizeOnly;
  private bool connected;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
    this.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
    this.Connect();
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
    this.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
    this.Disconnect();
    base.OnCleanUp();
  }

  private void Connect()
  {
    if (this.visualizeOnly || this.connected)
      return;
    this.connected = true;
    int linked_cell1;
    int linked_cell2;
    this.GetCells(out linked_cell1, out linked_cell2);
    this.OnConnect(linked_cell1, linked_cell2);
  }

  protected virtual void OnConnect(int cell1, int cell2)
  {
  }

  private void Disconnect()
  {
    if (this.visualizeOnly || !this.connected)
      return;
    this.connected = false;
    int linked_cell1;
    int linked_cell2;
    this.GetCells(out linked_cell1, out linked_cell2);
    this.OnDisconnect(linked_cell1, linked_cell2);
  }

  protected virtual void OnDisconnect(int cell1, int cell2)
  {
  }

  public void GetCells(out int linked_cell1, out int linked_cell2)
  {
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      this.GetCells(Grid.PosToCell(this.transform.GetPosition()), component.Orientation, out linked_cell1, out linked_cell2);
    }
    else
    {
      linked_cell1 = -1;
      linked_cell2 = -1;
    }
  }

  public void GetCells(
    int cell,
    Orientation orientation,
    out int linked_cell1,
    out int linked_cell2)
  {
    CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(this.link1, orientation);
    CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
    linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset1);
    linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
  }

  public bool AreCellsValid(int cell, Orientation orientation)
  {
    CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(this.link1, orientation);
    CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
    if (Grid.IsCellOffsetValid(cell, rotatedCellOffset1))
      return Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
    return false;
  }

  private void OnBuildingBroken(object data)
  {
    this.Disconnect();
  }

  private void OnBuildingFullyRepaired(object data)
  {
    this.Connect();
  }

  public int GetNetworkCell()
  {
    int linked_cell1;
    int linked_cell2;
    this.GetCells(out linked_cell1, out linked_cell2);
    return linked_cell1;
  }
}
