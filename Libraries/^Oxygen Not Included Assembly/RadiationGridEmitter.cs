// Decompiled with JetBrains decompiler
// Type: RadiationGridEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RadiationGridEmitter
{
  public int cell = -1;
  public float radius = 4f;
  public int intensity = 1;
  public float falloffRate = 0.5f;
  public LightShape shape;
  private List<int> litCells;

  public RadiationGridEmitter(
    int cell,
    List<int> lit_cells,
    int intensity,
    float radius,
    LightShape shape,
    float falloffRate = 0.5f)
  {
    this.cell = cell;
    this.radius = radius;
    this.intensity = intensity;
    this.shape = shape;
    this.litCells = lit_cells;
    this.falloffRate = falloffRate;
  }

  public void Add()
  {
    this.Remove();
    DiscreteShadowCaster.GetVisibleCells(this.cell, this.litCells, (int) this.radius, this.shape);
    for (int index = 0; index < this.litCells.Count; ++index)
    {
      int litCell = this.litCells[index];
      int num1 = Mathf.Max(1, Mathf.RoundToInt(this.falloffRate * (float) Mathf.Max(Grid.GetCellDistance(litCell, this.cell), 1)));
      int num2 = Mathf.Max(0, Grid.RadiationCount[litCell] + this.intensity / num1);
      Grid.RadiationCount[litCell] = num2;
      RadiationGridManager.previewLux[litCell] = num2;
    }
  }

  public void Remove()
  {
    for (int index = 0; index < this.litCells.Count; ++index)
    {
      int litCell = this.litCells[index];
      int falloff = RadiationGridManager.CalculateFalloff(this.falloffRate, litCell, this.cell);
      Grid.RadiationCount[litCell] = Mathf.Max(0, Grid.RadiationCount[litCell] - this.intensity / falloff);
      RadiationGridManager.previewLux[litCell] = 0;
    }
    this.litCells.Clear();
  }
}
