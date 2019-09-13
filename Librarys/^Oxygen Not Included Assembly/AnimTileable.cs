// Decompiled with JetBrains decompiler
// Type: AnimTileable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class AnimTileable : KMonoBehaviour
{
  private static readonly KAnimHashedString[] leftSymbols = new KAnimHashedString[3]
  {
    new KAnimHashedString("cap_left"),
    new KAnimHashedString("cap_left_fg"),
    new KAnimHashedString("cap_left_place")
  };
  private static readonly KAnimHashedString[] rightSymbols = new KAnimHashedString[3]
  {
    new KAnimHashedString("cap_right"),
    new KAnimHashedString("cap_right_fg"),
    new KAnimHashedString("cap_right_place")
  };
  private static readonly KAnimHashedString[] topSymbols = new KAnimHashedString[3]
  {
    new KAnimHashedString("cap_top"),
    new KAnimHashedString("cap_top_fg"),
    new KAnimHashedString("cap_top_place")
  };
  private static readonly KAnimHashedString[] bottomSymbols = new KAnimHashedString[3]
  {
    new KAnimHashedString("cap_bottom"),
    new KAnimHashedString("cap_bottom_fg"),
    new KAnimHashedString("cap_bottom_place")
  };
  public ObjectLayer objectLayer = ObjectLayer.Building;
  private HandleVector<int>.Handle partitionerEntry;
  public Tag[] tags;
  private Extents extents;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.tags != null && this.tags.Length != 0)
      return;
    this.tags = new Tag[1]
    {
      this.GetComponent<KPrefabID>().PrefabTag
    };
  }

  protected override void OnSpawn()
  {
    OccupyArea component = this.GetComponent<OccupyArea>();
    this.extents = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? this.GetComponent<Building>().GetExtents() : component.GetExtents();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", (object) this.gameObject, new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new System.Action<object>(this.OnNeighbourCellsUpdated));
    this.UpdateEndCaps();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void UpdateEndCaps()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    bool is_visible1 = true;
    bool is_visible2 = true;
    bool is_visible3 = true;
    bool is_visible4 = true;
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    CellOffset offset1 = new CellOffset(this.extents.x - x - 1, 0);
    CellOffset offset2 = new CellOffset(this.extents.x - x + this.extents.width, 0);
    CellOffset offset3 = new CellOffset(0, this.extents.y - y + this.extents.height);
    CellOffset offset4 = new CellOffset(0, this.extents.y - y - 1);
    Rotatable component = this.GetComponent<Rotatable>();
    if ((bool) ((UnityEngine.Object) component))
    {
      offset1 = component.GetRotatedCellOffset(offset1);
      offset2 = component.GetRotatedCellOffset(offset2);
      offset3 = component.GetRotatedCellOffset(offset3);
      offset4 = component.GetRotatedCellOffset(offset4);
    }
    int num1 = Grid.OffsetCell(cell, offset1);
    int num2 = Grid.OffsetCell(cell, offset2);
    int num3 = Grid.OffsetCell(cell, offset3);
    int num4 = Grid.OffsetCell(cell, offset4);
    if (Grid.IsValidCell(num1))
      is_visible1 = !this.HasTileableNeighbour(num1);
    if (Grid.IsValidCell(num2))
      is_visible2 = !this.HasTileableNeighbour(num2);
    if (Grid.IsValidCell(num3))
      is_visible3 = !this.HasTileableNeighbour(num3);
    if (Grid.IsValidCell(num4))
      is_visible4 = !this.HasTileableNeighbour(num4);
    foreach (KBatchedAnimController componentsInChild in this.GetComponentsInChildren<KBatchedAnimController>())
    {
      foreach (KAnimHashedString leftSymbol in AnimTileable.leftSymbols)
        componentsInChild.SetSymbolVisiblity(leftSymbol, is_visible1);
      foreach (KAnimHashedString rightSymbol in AnimTileable.rightSymbols)
        componentsInChild.SetSymbolVisiblity(rightSymbol, is_visible2);
      foreach (KAnimHashedString topSymbol in AnimTileable.topSymbols)
        componentsInChild.SetSymbolVisiblity(topSymbol, is_visible3);
      foreach (KAnimHashedString bottomSymbol in AnimTileable.bottomSymbols)
        componentsInChild.SetSymbolVisiblity(bottomSymbol, is_visible4);
    }
  }

  private bool HasTileableNeighbour(int neighbour_cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[neighbour_cell, (int) this.objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasAnyTags(this.tags))
        flag = true;
    }
    return flag;
  }

  private void OnNeighbourCellsUpdated(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.partitionerEntry.IsValid())
      return;
    this.UpdateEndCaps();
  }
}
