// Decompiled with JetBrains decompiler
// Type: SandboxHeatTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxHeatTool : BrushTool
{
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  public static SandboxHeatTool instance;

  public static void DestroyInstance()
  {
    SandboxHeatTool.instance = (SandboxHeatTool) null;
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
    SandboxHeatTool.instance = this;
    this.viewMode = OverlayModes.Temperature.ID;
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
    SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureAdditiveSlider.SetValue(5f);
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
    if (this.recentlyAffectedCells.Contains(cell))
      return;
    this.recentlyAffectedCells.Add(cell);
    int index = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)), false)).index;
    float num = Mathf.Clamp(Grid.Temperature[cell] + SandboxToolParameterMenu.instance.settings.temperatureAdditive, 1f, 9999f);
    int gameCell = cell;
    SimHashes id = Grid.Element[cell].id;
    CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
    float mass = Grid.Mass[cell];
    float temperature = num;
    int callbackIdx = index;
    SimMessages.ReplaceElement(gameCell, id, sandBoxTool, mass, temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], callbackIdx);
  }
}
