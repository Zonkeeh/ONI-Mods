// Decompiled with JetBrains decompiler
// Type: SandboxDestroyerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxDestroyerTool : BrushTool
{
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  public static SandboxDestroyerTool instance;

  public static void DestroyInstance()
  {
    SandboxDestroyerTool.instance = (SandboxDestroyerTool) null;
  }

  private SandboxSettings settings
  {
    get
    {
      return SandboxToolParameterMenu.instance.settings;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxDestroyerTool.instance = this;
    this.affectFoundation = true;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, this.recentlyAffectedCellColor));
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    this.recentlyAffectedCells.Add(cell);
    int index = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)), false)).index;
    SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SandBoxTool, 0.0f, 0.0f, Db.Get().Diseases.GetIndex(this.settings.Disease.IdHash), 0, index);
    HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) pickupable) == cell)
        pooledHashSet.Add(pickupable.gameObject);
    }
    foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) buildingComplete) == cell)
        pooledHashSet.Add(buildingComplete.gameObject);
    }
    if ((UnityEngine.Object) Grid.Objects[cell, 1] != (UnityEngine.Object) null)
      pooledHashSet.Add(Grid.Objects[cell, 1]);
    foreach (Crop crop in Components.Crops.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) crop) == cell)
        pooledHashSet.Add(crop.gameObject);
    }
    foreach (Health health in Components.Health.Items)
    {
      if (Grid.PosToCell((KMonoBehaviour) health) == cell)
        pooledHashSet.Add(health.gameObject);
    }
    foreach (GameObject original in (HashSet<GameObject>) pooledHashSet)
      Util.KDestroyGameObject(original);
    pooledHashSet.Recycle();
  }
}
