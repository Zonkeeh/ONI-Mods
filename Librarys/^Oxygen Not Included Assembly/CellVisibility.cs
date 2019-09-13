// Decompiled with JetBrains decompiler
// Type: CellVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellVisibility
{
  private int MinX;
  private int MinY;
  private int MaxX;
  private int MaxY;

  public CellVisibility()
  {
    Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
  }

  public bool IsVisible(int cell)
  {
    int num1 = Grid.CellColumn(cell);
    if (num1 < this.MinX || num1 > this.MaxX)
      return false;
    int num2 = Grid.CellRow(cell);
    return num2 >= this.MinY && num2 <= this.MaxY;
  }
}
