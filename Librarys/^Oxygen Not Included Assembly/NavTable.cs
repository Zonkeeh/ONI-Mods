// Decompiled with JetBrains decompiler
// Type: NavTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class NavTable
{
  public System.Action<int, NavType> OnValidCellChanged;
  private short[] NavTypeMasks;
  private short[] ValidCells;

  public NavTable(int cell_count)
  {
    this.ValidCells = new short[cell_count];
    this.NavTypeMasks = new short[10];
    for (short index = 0; index < (short) 10; ++index)
      this.NavTypeMasks[(int) index] = (short) (1 << (int) index);
  }

  public bool IsValid(int cell, NavType nav_type = NavType.Floor)
  {
    if (Grid.IsValidCell(cell))
      return ((int) this.NavTypeMasks[(int) nav_type] & (int) this.ValidCells[cell]) != 0;
    return false;
  }

  public void SetValid(int cell, NavType nav_type, bool is_valid)
  {
    short navTypeMask = this.NavTypeMasks[(int) nav_type];
    short validCell = this.ValidCells[cell];
    if (((int) validCell & (int) navTypeMask) != 0 == is_valid)
      return;
    this.ValidCells[cell] = !is_valid ? (short) ((int) ~navTypeMask & (int) validCell) : (short) ((int) navTypeMask | (int) validCell);
    if (this.OnValidCellChanged == null)
      return;
    this.OnValidCellChanged(cell, nav_type);
  }
}
