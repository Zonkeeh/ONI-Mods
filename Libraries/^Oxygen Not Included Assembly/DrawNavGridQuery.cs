// Decompiled with JetBrains decompiler
// Type: DrawNavGridQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DrawNavGridQuery : PathFinderQuery
{
  public DrawNavGridQuery Reset(MinionBrain brain)
  {
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (parent_cell == Grid.InvalidCell)
      return false;
    GL.Color(Color.white);
    GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
    GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
    return false;
  }
}
