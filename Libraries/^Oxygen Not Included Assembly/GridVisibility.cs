// Decompiled with JetBrains decompiler
// Type: GridVisibility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GridVisibility : KMonoBehaviour
{
  public float radius = 18f;
  public float innerRadius = 16.5f;

  protected override void OnSpawn()
  {
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "GridVisibility.OnSpawn");
    this.OnCellChange();
  }

  private void OnCellChange()
  {
    if (this.gameObject.HasTag(GameTags.Dead))
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell))
      return;
    if (!Grid.Revealed[cell])
    {
      int x;
      int y;
      Grid.PosToXY(this.transform.GetPosition(), out x, out y);
      GridVisibility.Reveal(x, y, this.radius, this.innerRadius);
      Grid.Revealed[cell] = true;
    }
    FogOfWarMask.ClearMask(cell);
  }

  public static void Reveal(int baseX, int baseY, float radius, float innerRadius)
  {
    for (float y = -radius; (double) y <= (double) radius; ++y)
    {
      for (float x = -radius; (double) x <= (double) radius; ++x)
      {
        float num1 = (float) baseY + y;
        float num2 = (float) baseX + x;
        if ((double) num1 >= 0.0 && (double) (Grid.HeightInCells - 1) >= (double) num1 && ((double) num2 >= 0.0 && (double) (Grid.WidthInCells - 1) >= (double) num2))
        {
          int cell = (int) ((double) num1 * (double) Grid.WidthInCells + (double) num2);
          if (Grid.Visible[cell] < byte.MaxValue)
          {
            float num3 = Mathf.Lerp(1f, 0.0f, (float) (((double) new Vector2(x, y).magnitude - (double) innerRadius) / ((double) radius - (double) innerRadius)));
            Grid.Reveal(cell, (byte) ((double) byte.MaxValue * (double) num3));
          }
        }
      }
    }
    int num = Mathf.CeilToInt(radius);
    Game.Instance.UpdateGameActiveRegion(baseX - num, baseY - num, baseX + num, baseY + num);
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
  }
}
