// Decompiled with JetBrains decompiler
// Type: BuildingAttachPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class BuildingAttachPoint : KMonoBehaviour
{
  public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.BuildingAttachPoints.Add(this);
    this.TryAttachEmptyHardpoints();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  private void TryAttachEmptyHardpoints()
  {
    for (int index1 = 0; index1 < this.points.Length; ++index1)
    {
      if (!((UnityEngine.Object) this.points[index1].attachedBuilding != (UnityEngine.Object) null))
      {
        bool flag = false;
        for (int index2 = 0; index2 < Components.AttachableBuildings.Count && !flag; ++index2)
        {
          if (Components.AttachableBuildings[index2].attachableToTag == this.points[index1].attachableType && Grid.OffsetCell(Grid.PosToCell(this.gameObject), this.points[index1].position) == Grid.PosToCell((KMonoBehaviour) Components.AttachableBuildings[index2]))
          {
            this.points[index1].attachedBuilding = Components.AttachableBuildings[index2];
            flag = true;
          }
        }
      }
    }
  }

  public bool AcceptsAttachment(Tag type, int cell)
  {
    int cell1 = Grid.PosToCell(this.gameObject);
    for (int index = 0; index < this.points.Length; ++index)
    {
      if (Grid.OffsetCell(cell1, this.points[index].position) == cell && this.points[index].attachableType == type)
        return true;
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.BuildingAttachPoints.Remove(this);
  }

  [Serializable]
  public struct HardPoint
  {
    public CellOffset position;
    public Tag attachableType;
    public AttachableBuilding attachedBuilding;

    public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
    {
      this.position = position;
      this.attachableType = attachableType;
      this.attachedBuilding = attachedBuilding;
    }
  }
}
