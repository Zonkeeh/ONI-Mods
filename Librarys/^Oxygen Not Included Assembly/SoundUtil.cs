// Decompiled with JetBrains decompiler
// Type: SoundUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class SoundUtil
{
  public static float GetLiquidDepth(int cell)
  {
    float num = (float) (0.0 + (double) Grid.Mass[cell] * (!Grid.Element[cell].IsLiquid ? 0.0 : 1.0));
    int cell1 = Grid.CellBelow(cell);
    if (Grid.IsValidCell(cell1))
      num += Grid.Mass[cell1] * (!Grid.Element[cell1].IsLiquid ? 0.0f : 1f);
    return Mathf.Min(num / 1000f, 1f);
  }

  public static float GetLiquidVolume(float mass)
  {
    return Mathf.Min(mass / 100f, 1f);
  }
}
