// Decompiled with JetBrains decompiler
// Type: EntityPreview
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class EntityPreview : KMonoBehaviour
{
  public ObjectLayer objectLayer = ObjectLayer.NumLayers;
  [MyCmpReq]
  private OccupyArea occupyArea;
  [MyCmpReq]
  private KBatchedAnimController animController;
  [MyCmpGet]
  private Storage storage;
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle objectPartitionerEntry;

  public bool Valid { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.solidPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnAreaChanged));
    if (this.objectLayer != ObjectLayer.NumLayers)
      this.objectPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (EntityPreview), (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int) this.objectLayer], new System.Action<object>(this.OnAreaChanged));
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
    this.OnAreaChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    base.OnCleanUp();
  }

  private void OnCellChange()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, cell);
    GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, cell);
    this.OnAreaChanged((object) null);
  }

  public void SetSolid()
  {
    this.occupyArea.ApplyToCells = true;
  }

  private void OnAreaChanged(object obj)
  {
    this.UpdateValidity();
  }

  public void UpdateValidity()
  {
    bool valid = this.Valid;
    OccupyArea occupyArea = this.occupyArea;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    // ISSUE: reference to a compiler-generated field
    if (EntityPreview.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      EntityPreview.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(EntityPreview.ValidTest);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = EntityPreview.\u003C\u003Ef__mg\u0024cache0;
    this.Valid = occupyArea.TestArea(cell, (object) this, fMgCache0);
    if (this.Valid)
      this.animController.TintColour = (Color32) Color.white;
    else
      this.animController.TintColour = (Color32) Color.red;
    if (valid == this.Valid)
      return;
    this.Trigger(-1820564715, (object) this.Valid);
  }

  private static bool ValidTest(int cell, object data)
  {
    EntityPreview entityPreview = (EntityPreview) data;
    if (Grid.Solid[cell])
      return false;
    if (entityPreview.objectLayer != ObjectLayer.NumLayers && !((UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) entityPreview.gameObject))
      return (UnityEngine.Object) Grid.Objects[cell, (int) entityPreview.objectLayer] == (UnityEngine.Object) null;
    return true;
  }
}
