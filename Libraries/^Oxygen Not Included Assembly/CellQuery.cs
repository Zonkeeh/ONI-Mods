// Decompiled with JetBrains decompiler
// Type: CellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class CellQuery : PathFinderQuery
{
  private int targetCell;

  public CellQuery Reset(int target_cell)
  {
    this.targetCell = target_cell;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    return cell == this.targetCell;
  }
}
