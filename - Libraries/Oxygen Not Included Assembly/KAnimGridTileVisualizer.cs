// Decompiled with JetBrains decompiler
// Type: KAnimGridTileVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Rendering;
using UnityEngine;

[SkipSaveFileSerialization]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
  private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>((System.Action<KAnimGridTileVisualizer, object>) ((component, data) => component.OnSelectionChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>((System.Action<KAnimGridTileVisualizer, object>) ((component, data) => component.OnHighlightChanged(data)));
  [SerializeField]
  public int blockTileConnectorID;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
    this.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
  }

  protected override void OnCleanUp()
  {
    Building component = this.GetComponent<Building>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      ObjectLayer tileLayer = component.Def.TileLayer;
      if ((UnityEngine.Object) Grid.Objects[cell, (int) tileLayer] == (UnityEngine.Object) this.gameObject)
        Grid.Objects[cell, (int) tileLayer] = (GameObject) null;
      TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
    }
    base.OnCleanUp();
  }

  private void OnSelectionChanged(object data)
  {
    World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(this.transform.GetPosition()), (bool) data);
  }

  private void OnHighlightChanged(object data)
  {
    World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(this.transform.GetPosition()), (bool) data);
  }

  public int GetBlockTileConnectorID()
  {
    return this.blockTileConnectorID;
  }
}
