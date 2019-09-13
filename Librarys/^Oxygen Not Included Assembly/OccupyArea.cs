// Decompiled with JetBrains decompiler
// Type: OccupyArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SkipSaveFileSerialization]
public class OccupyArea : KMonoBehaviour
{
  public ObjectLayer[] objectLayers = new ObjectLayer[0];
  [SerializeField]
  private bool applyToCells = true;
  public CellOffset[] OccupiedCellsOffsets;
  private CellOffset[] AboveOccupiedCellOffsets;
  private int[] occupiedGridCells;

  public bool ApplyToCells
  {
    get
    {
      return this.applyToCells;
    }
    set
    {
      if (value == this.applyToCells)
        return;
      if (value)
        this.UpdateOccupiedArea();
      else
        this.ClearOccupiedArea();
      this.applyToCells = value;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.applyToCells)
      return;
    this.UpdateOccupiedArea();
  }

  private void ValidatePosition()
  {
    if (Grid.IsValidCell(Grid.PosToCell((KMonoBehaviour) this)))
      return;
    Debug.LogWarning((object) (this.name + " is outside the grid! DELETING!"));
    Util.KDestroyGameObject(this.gameObject);
  }

  [OnSerializing]
  private void OnSerializing()
  {
    this.ValidatePosition();
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    this.ValidatePosition();
  }

  public void SetCellOffsets(CellOffset[] cells)
  {
    this.OccupiedCellsOffsets = cells;
  }

  public bool CheckIsOccupying(int checkCell)
  {
    int cell = Grid.PosToCell(this.gameObject);
    if (checkCell == cell)
      return true;
    foreach (CellOffset occupiedCellsOffset in this.OccupiedCellsOffsets)
    {
      if (Grid.OffsetCell(cell, occupiedCellsOffset) == checkCell)
        return true;
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.ClearOccupiedArea();
  }

  private void ClearOccupiedArea()
  {
    if (this.occupiedGridCells == null)
      return;
    foreach (ObjectLayer objectLayer in this.objectLayers)
    {
      if (objectLayer != ObjectLayer.NumLayers)
      {
        foreach (int occupiedGridCell in this.occupiedGridCells)
        {
          if ((UnityEngine.Object) Grid.Objects[occupiedGridCell, (int) objectLayer] == (UnityEngine.Object) this.gameObject)
            Grid.Objects[occupiedGridCell, (int) objectLayer] = (GameObject) null;
        }
      }
    }
  }

  public void UpdateOccupiedArea()
  {
    if (this.objectLayers.Length == 0)
      return;
    if (this.occupiedGridCells == null)
      this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
    this.ClearOccupiedArea();
    int cell = Grid.PosToCell(this.gameObject);
    foreach (ObjectLayer objectLayer in this.objectLayers)
    {
      if (objectLayer != ObjectLayer.NumLayers)
      {
        for (int index1 = 0; index1 < this.OccupiedCellsOffsets.Length; ++index1)
        {
          CellOffset occupiedCellsOffset = this.OccupiedCellsOffsets[index1];
          int index2 = Grid.OffsetCell(cell, occupiedCellsOffset);
          Grid.Objects[index2, (int) objectLayer] = this.gameObject;
          this.occupiedGridCells[index1] = index2;
        }
      }
    }
  }

  public int GetWidthInCells()
  {
    int val1_1 = int.MaxValue;
    int val1_2 = int.MinValue;
    foreach (CellOffset occupiedCellsOffset in this.OccupiedCellsOffsets)
    {
      val1_1 = Math.Min(val1_1, occupiedCellsOffset.x);
      val1_2 = Math.Max(val1_2, occupiedCellsOffset.x);
    }
    return val1_2 - val1_1 + 1;
  }

  public int GetHeightInCells()
  {
    int val1_1 = int.MaxValue;
    int val1_2 = int.MinValue;
    foreach (CellOffset occupiedCellsOffset in this.OccupiedCellsOffsets)
    {
      val1_1 = Math.Min(val1_1, occupiedCellsOffset.y);
      val1_2 = Math.Max(val1_2, occupiedCellsOffset.y);
    }
    return val1_2 - val1_1 + 1;
  }

  public Extents GetExtents()
  {
    return new Extents(Grid.PosToCell(this.gameObject), this.OccupiedCellsOffsets);
  }

  public Extents GetExtents(Orientation orientation)
  {
    return new Extents(Grid.PosToCell(this.gameObject), this.OccupiedCellsOffsets, orientation);
  }

  private void OnDrawGizmosSelected()
  {
    int cell = Grid.PosToCell(this.gameObject);
    if (this.OccupiedCellsOffsets != null)
    {
      foreach (CellOffset occupiedCellsOffset in this.OccupiedCellsOffsets)
      {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, occupiedCellsOffset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
      }
    }
    if (this.AboveOccupiedCellOffsets == null)
      return;
    foreach (CellOffset occupiedCellOffset in this.AboveOccupiedCellOffsets)
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, occupiedCellOffset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
    }
  }

  public bool CanOccupyArea(int rootCell, ObjectLayer layer)
  {
    for (int index1 = 0; index1 < this.OccupiedCellsOffsets.Length; ++index1)
    {
      CellOffset occupiedCellsOffset = this.OccupiedCellsOffsets[index1];
      int index2 = Grid.OffsetCell(rootCell, occupiedCellsOffset);
      if ((UnityEngine.Object) Grid.Objects[index2, (int) layer] != (UnityEngine.Object) null)
        return false;
    }
    return true;
  }

  public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
  {
    for (int index = 0; index < this.OccupiedCellsOffsets.Length; ++index)
    {
      CellOffset occupiedCellsOffset = this.OccupiedCellsOffsets[index];
      int num = Grid.OffsetCell(rootCell, occupiedCellsOffset);
      if (!testDelegate(num, data))
        return false;
    }
    return true;
  }

  public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
  {
    if (this.AboveOccupiedCellOffsets == null)
    {
      List<CellOffset> cellOffsetList = new List<CellOffset>();
      for (int index = 0; index < this.OccupiedCellsOffsets.Length; ++index)
      {
        CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[index].x, this.OccupiedCellsOffsets[index].y + 1);
        if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
          cellOffsetList.Add(cellOffset);
      }
      this.AboveOccupiedCellOffsets = cellOffsetList.ToArray();
    }
    for (int index = 0; index < this.AboveOccupiedCellOffsets.Length; ++index)
    {
      int num = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[index]);
      if (!testDelegate(num, data))
        return false;
    }
    return true;
  }
}
