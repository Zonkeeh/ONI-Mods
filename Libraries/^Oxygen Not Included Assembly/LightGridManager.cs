// Decompiled with JetBrains decompiler
// Type: LightGridManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public static class LightGridManager
{
  public static List<Tuple<int, int>> previewLightCells = new List<Tuple<int, int>>();
  public const float DEFAULT_FALLOFF_RATE = 0.5f;
  public static int[] previewLux;

  private static int CalculateFalloff(float falloffRate, int cell, int origin)
  {
    return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float) Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
  }

  public static void Initialise()
  {
    LightGridManager.previewLux = new int[Grid.CellCount];
  }

  public static void Shutdown()
  {
    LightGridManager.previewLux = (int[]) null;
    LightGridManager.previewLightCells.Clear();
  }

  public static void DestroyPreview()
  {
    foreach (Tuple<int, int> previewLightCell in LightGridManager.previewLightCells)
      LightGridManager.previewLux[previewLightCell.first] = 0;
    LightGridManager.previewLightCells.Clear();
  }

  public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux)
  {
    LightGridManager.previewLightCells.Clear();
    ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
    pooledList.Add(origin_cell);
    DiscreteShadowCaster.GetVisibleCells(origin_cell, (List<int>) pooledList, (int) radius, shape);
    foreach (int index in (List<int>) pooledList)
    {
      if (Grid.IsValidCell(index))
      {
        int b = lux / LightGridManager.CalculateFalloff(0.5f, index, origin_cell);
        LightGridManager.previewLightCells.Add(new Tuple<int, int>(index, b));
        LightGridManager.previewLux[index] = b;
      }
    }
    pooledList.Recycle();
  }

  public class LightGridEmitter
  {
    private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;
    private List<int> litCells = new List<int>();

    public void UpdateLitCells()
    {
      DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int) this.state.radius, this.state.shape);
    }

    public void AddToGrid(bool update_lit_cells)
    {
      DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter");
      if (update_lit_cells)
        this.UpdateLitCells();
      foreach (int litCell in this.litCells)
      {
        int num = Mathf.Max(0, Grid.LightCount[litCell] + this.ComputeLux(litCell));
        Grid.LightCount[litCell] = num;
        LightGridManager.previewLux[litCell] = num;
      }
    }

    public void RemoveFromGrid()
    {
      foreach (int litCell in this.litCells)
      {
        Grid.LightCount[litCell] = Mathf.Max(0, Grid.LightCount[litCell] - this.ComputeLux(litCell));
        LightGridManager.previewLux[litCell] = 0;
      }
      this.litCells.Clear();
    }

    public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
    {
      if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
        return false;
      this.RemoveFromGrid();
      this.state = state;
      this.AddToGrid(true);
      return true;
    }

    private int ComputeLux(int cell)
    {
      return this.state.intensity / this.ComputeFalloff(cell);
    }

    private int ComputeFalloff(int cell)
    {
      return LightGridManager.CalculateFalloff(this.state.falloffRate, this.state.origin, cell);
    }

    [Serializable]
    public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
    {
      public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State()
      {
        origin = Grid.InvalidCell,
        shape = LightShape.Circle,
        radius = 4f,
        intensity = 1,
        falloffRate = 0.5f,
        colour = Color.white
      };
      public int origin;
      public LightShape shape;
      public float radius;
      public int intensity;
      public float falloffRate;
      public Color colour;

      public bool Equals(LightGridManager.LightGridEmitter.State rhs)
      {
        if (this.origin == rhs.origin && this.shape == rhs.shape && ((double) this.radius == (double) rhs.radius && this.intensity == rhs.intensity) && (double) this.falloffRate == (double) rhs.falloffRate)
          return this.colour == rhs.colour;
        return false;
      }
    }
  }
}
