// Decompiled with JetBrains decompiler
// Type: ConditionFlightPathIsClear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class ConditionFlightPathIsClear : RocketFlightCondition
{
  private int obstructedTile = -1;
  private GameObject module;
  private int bufferWidth;
  private bool hasClearSky;

  public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
  {
    this.module = module;
    this.bufferWidth = bufferWidth;
  }

  public override bool EvaluateFlightCondition()
  {
    this.Update();
    return this.hasClearSky;
  }

  public override StatusItem GetFailureStatusItem()
  {
    return Db.Get().BuildingStatusItems.PathNotClear;
  }

  public void Update()
  {
    Extents extents = this.module.GetComponent<Building>().GetExtents();
    int x1 = extents.x - this.bufferWidth;
    int x2 = extents.x + extents.width - 1 + this.bufferWidth;
    int y = extents.y;
    int cell1 = Grid.XYToCell(x1, y);
    int cell2 = Grid.XYToCell(x2, y);
    this.hasClearSky = true;
    this.obstructedTile = -1;
    for (int startCell = cell1; startCell <= cell2; ++startCell)
    {
      if (!this.CanReachSpace(startCell))
      {
        this.hasClearSky = false;
        break;
      }
    }
  }

  private bool CanReachSpace(int startCell)
  {
    for (int cell = startCell; Grid.CellRow(cell) < Grid.HeightInCells; cell = Grid.CellAbove(cell))
    {
      if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
      {
        this.obstructedTile = cell;
        return false;
      }
    }
    return true;
  }

  public string GetObstruction()
  {
    if (this.obstructedTile == -1)
      return (string) null;
    if ((Object) Grid.Objects[this.obstructedTile, 1] != (Object) null)
      return Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name;
    return string.Format((string) BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, (object) Grid.Element[this.obstructedTile].tag.ProperName());
  }
}
