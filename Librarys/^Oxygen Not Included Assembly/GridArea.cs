// Decompiled with JetBrains decompiler
// Type: GridArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public struct GridArea
{
  private Vector2I min;
  private Vector2I max;
  private int MinCell;
  private int MaxCell;

  public Vector2I Min
  {
    get
    {
      return this.min;
    }
  }

  public Vector2I Max
  {
    get
    {
      return this.max;
    }
  }

  public void SetArea(int cell, int width, int height)
  {
    Vector2I xy = Grid.CellToXY(cell);
    Vector2I vector2I = new Vector2I(xy.x + width, xy.y + height);
    this.SetExtents(xy.x, xy.y, vector2I.x, vector2I.y);
  }

  public void SetExtents(int min_x, int min_y, int max_x, int max_y)
  {
    this.min.x = Math.Max(min_x, 0);
    this.min.y = Math.Max(min_y, 0);
    this.max.x = Math.Min(max_x, Grid.WidthInCells);
    this.max.y = Math.Min(max_y, Grid.HeightInCells);
    this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
    this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
  }

  public bool Contains(int cell)
  {
    if (cell < this.MinCell || cell >= this.MaxCell)
      return false;
    int num = cell % Grid.WidthInCells;
    if (num >= this.Min.x)
      return num < this.Max.x;
    return false;
  }

  public bool Contains(int x, int y)
  {
    if (x >= this.min.x && x < this.max.x && y >= this.min.y)
      return y < this.max.y;
    return false;
  }

  public bool Contains(Vector3 pos)
  {
    if ((double) this.min.x <= (double) pos.x && (double) pos.x < (double) this.max.x && (double) this.min.y <= (double) pos.y)
      return (double) pos.y <= (double) this.max.y;
    return false;
  }

  public void RunIfInside(int cell, System.Action<int> action)
  {
    if (!this.Contains(cell))
      return;
    action(cell);
  }

  public void Run(System.Action<int> action)
  {
    for (int y = this.min.y; y < this.max.y; ++y)
    {
      for (int x = this.min.x; x < this.max.x; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        action(cell);
      }
    }
  }

  public void RunOnDifference(GridArea subtract_area, System.Action<int> action)
  {
    for (int y = this.min.y; y < this.max.y; ++y)
    {
      for (int x = this.min.x; x < this.max.x; ++x)
      {
        if (!subtract_area.Contains(x, y))
        {
          int cell = Grid.XYToCell(x, y);
          action(cell);
        }
      }
    }
  }

  public int GetCellCount()
  {
    return (this.max.x - this.min.x) * (this.max.y - this.min.y);
  }
}
