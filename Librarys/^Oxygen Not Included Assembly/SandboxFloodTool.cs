// Decompiled with JetBrains decompiler
// Type: SandboxFloodTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SandboxFloodTool : FloodTool
{
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected HashSet<int> cellsToAffect = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  public static SandboxFloodTool instance;

  public static void DestroyInstance()
  {
    SandboxFloodTool.instance = (SandboxFloodTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxFloodTool.instance = this;
    this.floodCriteria = (Func<int, bool>) (cell =>
    {
      if (Grid.IsValidCell(cell))
        return Grid.Element[cell] == Grid.Element[this.mouseCell];
      return false;
    });
    this.paintArea = (System.Action<HashSet<int>>) (cells =>
    {
      foreach (int cell in cells)
        this.PaintCell(cell);
    });
  }

  private void PaintCell(int cell)
  {
    this.recentlyAffectedCells.Add(cell);
    int index = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)), false)).index;
    SimMessages.ReplaceElement(cell, this.settings.Element.id, CellEventLogger.Instance.SandBoxTool, this.settings.Mass, this.settings.temperature, Db.Get().Diseases.GetIndex(this.settings.Disease.IdHash), this.settings.diseaseCount, index);
  }

  private SandboxSettings settings
  {
    get
    {
      return SandboxToolParameterMenu.instance.settings;
    }
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
    SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
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
    foreach (int cell in this.cellsToAffect)
      colors.Add(new ToolMenu.CellColorData(cell, (Color) this.areaColour));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.cellsToAffect = this.Flood(Grid.PosToCell(cursorPos));
  }
}
