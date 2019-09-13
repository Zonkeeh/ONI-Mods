// Decompiled with JetBrains decompiler
// Type: NavigationReservations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class NavigationReservations : KMonoBehaviour
{
  public static int InvalidReservation = -1;
  private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
  public static NavigationReservations Instance;

  public static void DestroyInstance()
  {
    NavigationReservations.Instance = (NavigationReservations) null;
  }

  public int GetOccupancyCount(int cell)
  {
    if (this.cellOccupancyDensity.ContainsKey(cell))
      return this.cellOccupancyDensity[cell];
    return 0;
  }

  public void AddOccupancy(int cell)
  {
    if (!this.cellOccupancyDensity.ContainsKey(cell))
    {
      this.cellOccupancyDensity.Add(cell, 1);
    }
    else
    {
      Dictionary<int, int> occupancyDensity;
      int index;
      (occupancyDensity = this.cellOccupancyDensity)[index = cell] = occupancyDensity[index] + 1;
    }
  }

  public void RemoveOccupancy(int cell)
  {
    int num = 0;
    if (!this.cellOccupancyDensity.TryGetValue(cell, out num))
      return;
    if (num == 1)
      this.cellOccupancyDensity.Remove(cell);
    else
      this.cellOccupancyDensity[cell] = num - 1;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NavigationReservations.Instance = this;
  }
}
