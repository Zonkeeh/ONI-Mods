// Decompiled with JetBrains decompiler
// Type: NavTactic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class NavTactic
{
  private int _overlapPenalty = 3;
  private int _rangePenalty = 2;
  private int _pathCostPenalty = 1;
  private int _preferredRange;

  public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
  {
    this._overlapPenalty = overlapPenalty;
    this._preferredRange = preferredRange;
    this._rangePenalty = rangePenalty;
    this._pathCostPenalty = pathCostPenalty;
  }

  public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
  {
    int num1 = NavigationReservations.InvalidReservation;
    int num2 = int.MaxValue;
    for (int index = 0; index < offsets.Length; ++index)
    {
      int num3 = Grid.OffsetCell(root, offsets[index]);
      int num4 = 0 + this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num3) + this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num3)) + this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num3), 0);
      if (num4 < num2 && navigator.CanReach(num3))
      {
        num2 = num4;
        num1 = num3;
      }
    }
    return num1;
  }
}
