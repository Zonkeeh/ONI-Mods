// Decompiled with JetBrains decompiler
// Type: PathGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class PathGrid
{
  public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell()
  {
    cost = -1
  };
  private List<int> freshlyOccupiedCells = new List<int>();
  private PathFinder.Cell[] Cells;
  private PathGrid.ProberCell[] ProberCells;
  private NavType[] ValidNavTypes;
  private int[] NavTypeTable;
  private int widthInCells;
  private int heightInCells;
  private bool applyOffset;
  private int rootX;
  private int rootY;
  private int serialNo;
  private int previousSerialNo;
  private bool isUpdating;
  private IGroupProber groupProber;

  public PathGrid(
    int width_in_cells,
    int height_in_cells,
    bool apply_offset,
    NavType[] valid_nav_types)
  {
    this.applyOffset = apply_offset;
    this.widthInCells = width_in_cells;
    this.heightInCells = height_in_cells;
    this.ValidNavTypes = valid_nav_types;
    int num = 0;
    this.NavTypeTable = new int[10];
    for (int index1 = 0; index1 < this.NavTypeTable.Length; ++index1)
    {
      this.NavTypeTable[index1] = -1;
      for (int index2 = 0; index2 < this.ValidNavTypes.Length; ++index2)
      {
        if (this.ValidNavTypes[index2] == (NavType) index1)
        {
          this.NavTypeTable[index1] = num++;
          break;
        }
      }
    }
    this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
    this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
    this.serialNo = 0;
    this.previousSerialNo = -1;
    this.isUpdating = false;
  }

  public void SetGroupProber(IGroupProber group_prober)
  {
    this.groupProber = group_prober;
  }

  public void OnCleanUp()
  {
    if (this.groupProber == null)
      return;
    this.groupProber.ReleaseProber((object) this);
  }

  public void ResetUpdate()
  {
    this.previousSerialNo = -1;
  }

  public void BeginUpdate(int root_cell, bool isContinuation)
  {
    this.isUpdating = true;
    this.freshlyOccupiedCells.Clear();
    if (isContinuation)
      return;
    KProfiler.AddEvent("PathGrid.BeginUpdate");
    if (this.applyOffset)
    {
      Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
      this.rootX -= this.widthInCells / 2;
      this.rootY -= this.heightInCells / 2;
    }
    ++this.serialNo;
    if (this.groupProber == null)
      return;
    this.groupProber.SetValidSerialNos((object) this, this.previousSerialNo, this.serialNo);
  }

  public void EndUpdate(bool isComplete)
  {
    this.isUpdating = false;
    if (this.groupProber != null)
      this.groupProber.Occupy((object) this, this.serialNo, (IEnumerable<int>) this.freshlyOccupiedCells);
    if (!isComplete)
      return;
    if (this.groupProber != null)
      this.groupProber.SetValidSerialNos((object) this, this.serialNo, this.serialNo);
    this.previousSerialNo = this.serialNo;
    KProfiler.AddEvent("PathGrid.EndUpdate");
  }

  private bool IsValidSerialNo(int serialNo)
  {
    if (serialNo == this.serialNo)
      return true;
    if (!this.isUpdating && this.previousSerialNo != -1)
      return serialNo == this.previousSerialNo;
    return false;
  }

  public PathFinder.Cell GetCell(
    PathFinder.PotentialPath potential_path,
    out bool is_cell_in_range)
  {
    return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
  }

  public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
  {
    int num = this.OffsetCell(cell);
    is_cell_in_range = -1 != num;
    if (!is_cell_in_range)
      return PathGrid.InvalidCell;
    PathFinder.Cell cell1 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int) nav_type]];
    if (this.IsValidSerialNo(cell1.queryId))
      return cell1;
    return PathGrid.InvalidCell;
  }

  public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
  {
    int index = this.OffsetCell(potential_path.cell);
    if (index == -1)
      return;
    cell_data.queryId = this.serialNo;
    int num = this.NavTypeTable[(int) potential_path.navType];
    this.Cells[index * this.ValidNavTypes.Length + num] = cell_data;
    if (potential_path.navType == NavType.Tube)
      return;
    PathGrid.ProberCell proberCell = this.ProberCells[index];
    if (cell_data.queryId == proberCell.queryId && cell_data.cost >= proberCell.cost)
      return;
    proberCell.queryId = cell_data.queryId;
    proberCell.cost = cell_data.cost;
    this.ProberCells[index] = proberCell;
    this.freshlyOccupiedCells.Add(potential_path.cell);
  }

  public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
  {
    int num = -1;
    foreach (CellOffset offset in offsets)
    {
      int cell1 = Grid.OffsetCell(cell, offset);
      if (Grid.IsValidCell(cell1))
      {
        PathGrid.ProberCell proberCell = this.ProberCells[cell1];
        if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
          num = proberCell.cost;
      }
    }
    return num;
  }

  public int GetCost(int cell)
  {
    int index = this.OffsetCell(cell);
    if (index == -1)
      return -1;
    PathGrid.ProberCell proberCell = this.ProberCells[index];
    if (this.IsValidSerialNo(proberCell.queryId))
      return proberCell.cost;
    return -1;
  }

  private int OffsetCell(int cell)
  {
    if (!this.applyOffset)
      return cell;
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    if (x < this.rootX || x >= this.rootX + this.widthInCells || (y < this.rootY || y >= this.rootY + this.heightInCells))
      return -1;
    int num = x - this.rootX;
    return (y - this.rootY) * this.widthInCells + num;
  }

  private struct ProberCell
  {
    public int cost;
    public int queryId;
  }
}
