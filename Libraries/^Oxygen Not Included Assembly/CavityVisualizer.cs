// Decompiled with JetBrains decompiler
// Type: CavityVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGenGame;
using System.Collections.Generic;
using UnityEngine;

public class CavityVisualizer : KMonoBehaviour
{
  public List<int> cavityCells = new List<int>();
  public List<int> spawnCells = new List<int>();
  public bool drawCavity = true;
  public bool drawSpawnCells = true;
  public static CavityVisualizer Instance;

  protected override void OnPrefabInit()
  {
    Debug.Assert((Object) CavityVisualizer.Instance == (Object) null);
    CavityVisualizer.Instance = this;
    base.OnPrefabInit();
    foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
    {
      foreach (HashSet<int> intSet in MobSpawning.NaturalCavities[key])
      {
        foreach (int num in intSet)
          this.cavityCells.Add(num);
      }
    }
  }

  private void OnDrawGizmosSelected()
  {
    if (this.drawCavity)
    {
      Color[] colorArray = new Color[2]
      {
        Color.blue,
        Color.yellow
      };
      int num = 0;
      foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
      {
        Gizmos.color = colorArray[num % colorArray.Length];
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
        ++num;
        foreach (HashSet<int> intSet in MobSpawning.NaturalCavities[key])
        {
          foreach (int cell in intSet)
            Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
        }
      }
    }
    if (this.spawnCells == null || !this.drawSpawnCells)
      return;
    Gizmos.color = new Color(0.0f, 1f, 0.0f, 0.15f);
    foreach (int spawnCell in this.spawnCells)
      Gizmos.DrawCube(Grid.CellToPos(spawnCell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
  }
}
