// Decompiled with JetBrains decompiler
// Type: LightShapePreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class LightShapePreview : KMonoBehaviour
{
  private int previousCell = -1;
  public float radius;
  public int lux;
  public LightShape shape;
  public CellOffset offset;

  private void Update()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.previousCell)
      return;
    this.previousCell = cell;
    LightGridManager.DestroyPreview();
    LightGridManager.CreatePreview(Grid.OffsetCell(cell, this.offset), this.radius, this.shape, this.lux);
  }

  protected override void OnCleanUp()
  {
    LightGridManager.DestroyPreview();
  }
}
