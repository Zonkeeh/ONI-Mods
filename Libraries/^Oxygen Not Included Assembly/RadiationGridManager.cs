// Decompiled with JetBrains decompiler
// Type: RadiationGridManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class RadiationGridManager
{
  public static List<Tuple<int, int>> previewLightCells = new List<Tuple<int, int>>();
  public static int[] previewLux;

  public static int CalculateFalloff(float falloffRate, int cell, int origin)
  {
    return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float) Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
  }

  public static void Initialise()
  {
    RadiationGridManager.previewLux = new int[Grid.CellCount];
  }

  public static void Shutdown()
  {
    RadiationGridManager.previewLux = (int[]) null;
    RadiationGridManager.previewLightCells.Clear();
  }

  public static void DestroyPreview()
  {
    foreach (Tuple<int, int> previewLightCell in RadiationGridManager.previewLightCells)
      RadiationGridManager.previewLux[previewLightCell.first] = 0;
    RadiationGridManager.previewLightCells.Clear();
  }

  public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux)
  {
    RadiationGridManager.previewLightCells.Clear();
    ListPool<int, RadiationGridEmitter>.PooledList pooledList = ListPool<int, RadiationGridEmitter>.Allocate();
    pooledList.Add(origin_cell);
    DiscreteShadowCaster.GetVisibleCells(origin_cell, (List<int>) pooledList, (int) radius, shape);
    foreach (int index in (List<int>) pooledList)
    {
      if (Grid.IsValidCell(index))
      {
        int b = lux / RadiationGridManager.CalculateFalloff(0.5f, index, origin_cell);
        RadiationGridManager.previewLightCells.Add(new Tuple<int, int>(index, b));
        RadiationGridManager.previewLux[index] = b;
      }
    }
    pooledList.Recycle();
  }
}
