// Decompiled with JetBrains decompiler
// Type: OffsetTableTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class OffsetTableTracker : OffsetTracker
{
  private readonly CellOffset[][] table;
  public HandleVector<int>.Handle solidPartitionerEntry;
  public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;
  private static NavGrid navGridImpl;
  private KMonoBehaviour cmp;

  public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
  {
    this.table = table;
    this.cmp = cmp;
  }

  private static NavGrid navGrid
  {
    get
    {
      if (OffsetTableTracker.navGridImpl == null)
        OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
      Debug.Assert(OffsetTableTracker.navGridImpl == Pathfinding.Instance.GetNavGrid("MinionNavGrid"), (object) "Cached NavGrid reference is invalid");
      return OffsetTableTracker.navGridImpl;
    }
  }

  protected override void UpdateCell(int previous_cell, int current_cell)
  {
    if (previous_cell == current_cell)
      return;
    base.UpdateCell(previous_cell, current_cell);
    if (!this.solidPartitionerEntry.IsValid())
    {
      Extents extents = new Extents(current_cell, this.table);
      extents.height += 2;
      --extents.y;
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", (object) this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnCellChanged));
      this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", (object) this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new System.Action<object>(this.OnCellChanged));
    }
    else
    {
      GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, current_cell);
      GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, current_cell);
    }
    this.offsets = (CellOffset[]) null;
  }

  private static bool IsValidRow(int current_cell, CellOffset[] row)
  {
    for (int index = 1; index < row.Length; ++index)
    {
      int cell = Grid.OffsetCell(current_cell, row[index]);
      if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
        return false;
    }
    return true;
  }

  private unsafe void UpdateOffsets(int cell, CellOffset[][] table)
  {
    Debug.Assert(table.Length <= 192, (object) string.Format("validRowIndices[{0}] isn't big enough < {1}", (object) 192, (object) table.Length));
    // ISSUE: untyped stack allocation
    int* numPtr = (int*) __untypedstackalloc((int) checked (192U * 4U));
    int length = 0;
    if (Grid.IsValidCell(cell))
    {
      for (int index1 = 0; index1 < table.Length; ++index1)
      {
        CellOffset[] row = table[index1];
        int cell1 = Grid.OffsetCell(cell, row[0]);
        for (int index2 = 0; index2 < OffsetTableTracker.navGrid.ValidNavTypes.Length; ++index2)
        {
          NavType validNavType = OffsetTableTracker.navGrid.ValidNavTypes[index2];
          if (validNavType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell1, validNavType) && OffsetTableTracker.IsValidRow(cell, row))
          {
            numPtr[length] = index1;
            ++length;
            break;
          }
        }
      }
    }
    if (this.offsets == null || this.offsets.Length != length)
      this.offsets = new CellOffset[length];
    for (int index = 0; index != length; ++index)
      this.offsets[index] = table[numPtr[index]][0];
  }

  protected override void UpdateOffsets(int current_cell)
  {
    base.UpdateOffsets(current_cell);
    this.UpdateOffsets(current_cell, this.table);
  }

  private void OnCellChanged(object data)
  {
    this.offsets = (CellOffset[]) null;
  }

  public override void Clear()
  {
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
  }

  public static void OnPathfindingInvalidated()
  {
    OffsetTableTracker.navGridImpl = (NavGrid) null;
  }
}
