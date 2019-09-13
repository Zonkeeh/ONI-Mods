// Decompiled with JetBrains decompiler
// Type: Rendering.World.TileCells
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace Rendering.World
{
  public struct TileCells
  {
    public int Cell0;
    public int Cell1;
    public int Cell2;
    public int Cell3;

    public TileCells(int tile_x, int tile_y)
    {
      int val2_1 = Grid.WidthInCells - 1;
      int val2_2 = Grid.HeightInCells - 1;
      this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val2_1), Math.Min(Math.Max(tile_y - 1, 0), val2_2));
      this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val2_1), Math.Min(Math.Max(tile_y - 1, 0), val2_2));
      this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val2_1), Math.Min(tile_y, val2_2));
      this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val2_1), Math.Min(tile_y, val2_2));
    }
  }
}
