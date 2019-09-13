// Decompiled with JetBrains decompiler
// Type: PumpingStationGuide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
  private int previousDepthAvailable = -1;
  public GameObject parent;
  public bool occupyTiles;
  private KBatchedAnimController parentController;
  private KBatchedAnimController guideController;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.parentController = this.parent.GetComponent<KBatchedAnimController>();
    this.guideController = this.GetComponent<KBatchedAnimController>();
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  private void RefreshTint()
  {
    this.guideController.TintColour = this.parentController.TintColour;
  }

  private void RefreshDepthAvailable()
  {
    int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell((KMonoBehaviour) this), this.parent);
    if (depthAvailable == this.previousDepthAvailable)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (depthAvailable == 0)
    {
      component.enabled = false;
    }
    else
    {
      component.enabled = true;
      component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0.0f);
    }
    if (this.occupyTiles)
      PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
    this.previousDepthAvailable = depthAvailable;
  }

  public void RenderEveryTick(float dt)
  {
    this.RefreshTint();
    this.RefreshDepthAvailable();
  }

  public static void OccupyArea(GameObject go, int depth_available)
  {
    int cell = Grid.PosToCell(go.transform.GetPosition());
    for (int index1 = 1; index1 <= depth_available; ++index1)
    {
      int index2 = Grid.OffsetCell(cell, 0, -index1);
      int index3 = Grid.OffsetCell(cell, 1, -index1);
      Grid.ObjectLayers[1][index2] = go;
      Grid.ObjectLayers[1][index3] = go;
    }
  }

  public static int GetDepthAvailable(int root_cell, GameObject pump)
  {
    int num1 = 4;
    int num2 = 0;
    for (int index1 = 1; index1 <= num1; ++index1)
    {
      int index2 = Grid.OffsetCell(root_cell, 0, -index1);
      int index3 = Grid.OffsetCell(root_cell, 1, -index1);
      if (Grid.IsValidCell(index2) && !Grid.Solid[index2] && (Grid.IsValidCell(index3) && !Grid.Solid[index3]) && (!Grid.ObjectLayers[1].ContainsKey(index2) || (Object) Grid.ObjectLayers[1][index2] == (Object) null || (Object) Grid.ObjectLayers[1][index2] == (Object) pump) && (!Grid.ObjectLayers[1].ContainsKey(index3) || (Object) Grid.ObjectLayers[1][index3] == (Object) null || (Object) Grid.ObjectLayers[1][index3] == (Object) pump))
        num2 = index1;
      else
        break;
    }
    return num2;
  }
}
