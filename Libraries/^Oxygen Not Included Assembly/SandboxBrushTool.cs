// Decompiled with JetBrains decompiler
// Type: SandboxBrushTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SandboxBrushTool : BrushTool
{
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();
  public static SandboxBrushTool instance;

  public static void DestroyInstance()
  {
    SandboxBrushTool.instance = (SandboxBrushTool) null;
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
    SandboxBrushTool.instance = this;
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
    {
      Color color = new Color(this.recentAffectedCellColor[recentlyAffectedCell].r, this.recentAffectedCellColor[recentlyAffectedCell].g, this.recentAffectedCellColor[recentlyAffectedCell].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, color));
    }
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void SetBrushSize(int radius)
  {
    this.brushRadius = radius;
    this.brushOffsets.Clear();
    for (int index1 = 0; index1 < this.brushRadius * 2; ++index1)
    {
      for (int index2 = 0; index2 < this.brushRadius * 2; ++index2)
      {
        if ((double) Vector2.Distance(new Vector2((float) index1, (float) index2), new Vector2((float) this.brushRadius, (float) this.brushRadius)) < (double) this.brushRadius - 0.800000011920929)
          this.brushOffsets.Add(new Vector2((float) (index1 - this.brushRadius), (float) (index2 - this.brushRadius)));
      }
    }
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    this.recentlyAffectedCells.Add(cell);
    if (!this.recentAffectedCellColor.ContainsKey(cell))
      this.recentAffectedCellColor.Add(cell, (Color) this.settings.Element.substance.uiColour);
    else
      this.recentAffectedCellColor[cell] = (Color) this.settings.Element.substance.uiColour;
    int index = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() =>
    {
      this.recentlyAffectedCells.Remove(cell);
      this.recentAffectedCellColor.Remove(cell);
    }), false)).index;
    SimMessages.ReplaceElement(cell, this.settings.Element.id, CellEventLogger.Instance.SandBoxTool, this.settings.Mass, this.settings.temperature, Db.Get().Diseases.GetIndex(this.settings.Disease.IdHash), this.settings.diseaseCount, index);
  }
}
