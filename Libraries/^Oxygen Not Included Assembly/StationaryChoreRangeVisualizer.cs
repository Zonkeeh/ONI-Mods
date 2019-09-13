// Decompiled with JetBrains decompiler
// Type: StationaryChoreRangeVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
  private static readonly string AnimName = "transferarmgrid_kanim";
  private static readonly HashedString[] PreAnims = new HashedString[2]
  {
    (HashedString) "grid_pre",
    (HashedString) "grid_loop"
  };
  private static readonly HashedString PostAnim = (HashedString) "grid_pst";
  private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>((System.Action<StationaryChoreRangeVisualizer, object>) ((component, data) => component.OnSelect(data)));
  private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>((System.Action<StationaryChoreRangeVisualizer, object>) ((component, data) => component.OnRotated(data)));
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private Rotatable rotatable;
  public int x;
  public int y;
  public int width;
  public int height;
  public bool movable;
  public Grid.SceneLayer sceneLayer;
  public CellOffset vision_offset;
  public Func<int, bool> blocking_cb;
  public bool blocking_tile_visible;
  private List<StationaryChoreRangeVisualizer.VisData> visualizers;
  private List<int> newCells;

  public StationaryChoreRangeVisualizer()
  {
    // ISSUE: reference to a compiler-generated field
    if (StationaryChoreRangeVisualizer.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      StationaryChoreRangeVisualizer.\u003C\u003Ef__mg\u0024cache0 = new Func<int, bool>(Grid.PhysicalBlockingCB);
    }
    // ISSUE: reference to a compiler-generated field
    this.blocking_cb = StationaryChoreRangeVisualizer.\u003C\u003Ef__mg\u0024cache0;
    this.blocking_tile_visible = true;
    this.visualizers = new List<StationaryChoreRangeVisualizer.VisData>();
    this.newCells = new List<int>();
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
    if (!this.movable)
      return;
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
    this.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    this.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
    this.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
    this.ClearVisualizers();
    base.OnCleanUp();
  }

  private void OnSelect(object data)
  {
    if ((bool) data)
    {
      SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), this.transform.position);
      this.UpdateVisualizers();
    }
    else
    {
      SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), this.transform.position);
      this.ClearVisualizers();
    }
  }

  private void OnRotated(object data)
  {
    this.UpdateVisualizers();
  }

  private void OnCellChange()
  {
    this.UpdateVisualizers();
  }

  private void UpdateVisualizers()
  {
    this.newCells.Clear();
    CellOffset offset1 = this.vision_offset;
    if ((bool) ((UnityEngine.Object) this.rotatable))
      offset1 = this.rotatable.GetRotatedCellOffset(this.vision_offset);
    int cell1 = Grid.PosToCell(this.transform.gameObject);
    int x1;
    int y1;
    Grid.CellToXY(Grid.OffsetCell(cell1, offset1), out x1, out y1);
    for (int index1 = 0; index1 < this.height; ++index1)
    {
      for (int index2 = 0; index2 < this.width; ++index2)
      {
        CellOffset offset2 = new CellOffset(this.x + index2, this.y + index1);
        if ((bool) ((UnityEngine.Object) this.rotatable))
          offset2 = this.rotatable.GetRotatedCellOffset(offset2);
        int cell2 = Grid.OffsetCell(cell1, offset2);
        if (Grid.IsValidCell(cell2))
        {
          int x2;
          int y2;
          Grid.CellToXY(cell2, out x2, out y2);
          if (Grid.TestLineOfSight(x1, y1, x2, y2, this.blocking_cb, this.blocking_tile_visible))
            this.newCells.Add(cell2);
        }
      }
    }
    for (int index = this.visualizers.Count - 1; index >= 0; --index)
    {
      if (this.newCells.Contains(this.visualizers[index].cell))
      {
        this.newCells.Remove(this.visualizers[index].cell);
      }
      else
      {
        this.DestroyEffect(this.visualizers[index].controller);
        this.visualizers.RemoveAt(index);
      }
    }
    for (int index = 0; index < this.newCells.Count; ++index)
    {
      KBatchedAnimController effect = this.CreateEffect(this.newCells[index]);
      this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData()
      {
        cell = this.newCells[index],
        controller = effect
      });
    }
  }

  private void ClearVisualizers()
  {
    for (int index = 0; index < this.visualizers.Count; ++index)
      this.DestroyEffect(this.visualizers[index].controller);
    this.visualizers.Clear();
  }

  private KBatchedAnimController CreateEffect(int cell)
  {
    KBatchedAnimController effect = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), (Transform) null, false, this.sceneLayer, true);
    effect.destroyOnAnimComplete = false;
    effect.visibilityType = KAnimControllerBase.VisibilityType.Always;
    effect.gameObject.SetActive(true);
    effect.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
    return effect;
  }

  private void DestroyEffect(KBatchedAnimController controller)
  {
    controller.destroyOnAnimComplete = true;
    controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private struct VisData
  {
    public int cell;
    public KBatchedAnimController controller;
  }
}
