// Decompiled with JetBrains decompiler
// Type: CellOffsetQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellOffsetQuery : CellArrayQuery
{
  public CellArrayQuery Reset(int cell, CellOffset[] offsets)
  {
    int[] target_cells = new int[offsets.Length];
    for (int index = 0; index < offsets.Length; ++index)
      target_cells[index] = Grid.OffsetCell(cell, offsets[index]);
    this.Reset(target_cells);
    return (CellArrayQuery) this;
  }
}
