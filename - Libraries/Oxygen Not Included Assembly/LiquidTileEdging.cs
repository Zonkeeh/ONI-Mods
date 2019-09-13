// Decompiled with JetBrains decompiler
// Type: LiquidTileEdging
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class LiquidTileEdging
{
  private void Update()
  {
    int min_x;
    int min_y;
    int max_x;
    int max_y;
    Grid.GetVisibleExtents(out min_x, out min_y, out max_x, out max_y);
    int a1 = Math.Max(0, min_x);
    int a2 = Math.Max(0, min_y);
    int num1 = Mathf.Min(a1, Grid.WidthInCells - 1);
    int num2 = Mathf.Min(a2, Grid.HeightInCells - 1);
    int a3 = Mathf.CeilToInt((float) max_x);
    int a4 = Mathf.CeilToInt((float) max_y);
    int a5 = Mathf.Max(a3, 0);
    int a6 = Mathf.Max(a4, 0);
    int num3 = Mathf.Min(a5, Grid.WidthInCells - 1);
    int num4 = Mathf.Min(a6, Grid.HeightInCells - 1);
    int num5 = 0;
    int num6 = 0;
    int num7 = 0;
    for (int index1 = num2; index1 < num4; ++index1)
    {
      for (int index2 = num1; index2 < num3; ++index2)
      {
        int index3 = index1 * Grid.WidthInCells + index2;
        Element element = Grid.Element[index3];
        if (element.IsSolid)
          ++num5;
        else if (element.IsLiquid)
          ++num6;
        else
          ++num7;
      }
    }
  }
}
