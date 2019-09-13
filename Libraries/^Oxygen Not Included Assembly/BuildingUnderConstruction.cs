// Decompiled with JetBrains decompiler
// Type: BuildingUnderConstruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BuildingUnderConstruction : Building
{
  [MyCmpAdd]
  private KSelectable selectable;
  [MyCmpAdd]
  private SaveLoadRoot saveLoadRoot;
  [MyCmpAdd]
  private KPrefabID kPrefabID;
  [MyCmpAdd]
  private Cancellable cancellable;

  protected override void OnPrefabInit()
  {
    Vector3 position = this.transform.GetPosition();
    position.z = Grid.GetLayerZ(this.Def.SceneLayer);
    this.transform.SetPosition(position);
    this.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    Rotatable component2 = this.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      component1.Offset = this.Def.GetVisualizerOffset() + this.Def.placementPivot;
    KBoxCollider2D component3 = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
      component3.offset = component3.offset + new Vector2(visualizerOffset.x, visualizerOffset.y);
    }
    if (this.Def.IsTilePiece)
      this.Def.RunOnArea(Grid.PosToCell(this.transform.GetPosition()), this.Orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer)));
    this.RegisterBlockTileRenderer();
  }

  protected override void OnCleanUp()
  {
    this.UnregisterBlockTileRenderer();
    base.OnCleanUp();
  }
}
