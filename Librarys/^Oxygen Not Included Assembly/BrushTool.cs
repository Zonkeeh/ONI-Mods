// Decompiled with JetBrains decompiler
// Type: BrushTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : InterfaceTool
{
  [SerializeField]
  private Color32 areaColour = (Color32) new Color(1f, 1f, 1f, 0.5f);
  protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
  protected List<Vector2> brushOffsets = new List<Vector2>();
  protected int brushRadius = -1;
  private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;
  protected HashSet<int> cellsInRadius = new HashSet<int>();
  [SerializeField]
  private Texture2D brushCursor;
  [SerializeField]
  private GameObject areaVisualizer;
  protected Vector3 placementPivot;
  protected bool interceptNumberKeysForPriority;
  protected bool affectFoundation;
  private bool dragging;
  protected Vector3 downPos;
  protected int currentCell;

  public bool Dragging
  {
    get
    {
      return this.dragging;
    }
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.dragging = false;
    this.SetBrushSize(5);
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public virtual void SetBrushSize(int radius)
  {
    if (radius == this.brushRadius)
      return;
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

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    KScreenManager.Instance.SetEventSystemEnabled(true);
    base.OnDeactivateTool(new_tool);
  }

  protected override void OnPrefabInit()
  {
    Game.Instance.Subscribe(1634669191, new System.Action<object>(this.OnTutorialOpened));
    base.OnPrefabInit();
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer = Util.KInstantiate(this.visualizer, (GameObject) null, (string) null);
    if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer = Util.KInstantiate(this.areaVisualizer, (GameObject) null, (string) null);
    this.areaVisualizer.SetActive(false);
    this.areaVisualizer.GetComponent<RectTransform>().SetParent(this.transform);
    this.areaVisualizer.GetComponent<Renderer>().material.color = (Color) this.areaColour;
  }

  protected override void OnCmpEnable()
  {
    this.dragging = false;
  }

  protected override void OnCmpDisable()
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer.SetActive(false);
    if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer.SetActive(false);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    cursor_pos -= this.placementPivot;
    this.dragging = true;
    this.downPos = cursor_pos;
    KScreenManager.Instance.SetEventSystemEnabled(false);
    this.Paint();
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    cursor_pos -= this.placementPivot;
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if (!this.dragging)
      return;
    this.dragging = false;
    switch (this.dragAxis)
    {
      case BrushTool.DragAxis.Horizontal:
        cursor_pos.y = this.downPos.y;
        this.dragAxis = BrushTool.DragAxis.None;
        break;
      case BrushTool.DragAxis.Vertical:
        cursor_pos.x = this.downPos.x;
        this.dragAxis = BrushTool.DragAxis.None;
        break;
    }
  }

  protected virtual string GetConfirmSound()
  {
    return "Tile_Confirm";
  }

  protected virtual string GetDragSound()
  {
    return "Tile_Drag";
  }

  public override string GetDeactivateSound()
  {
    return "Tile_Cancel";
  }

  private static int GetGridDistance(int cell, int center_cell)
  {
    Vector2I vector2I = Grid.CellToXY(cell) - Grid.CellToXY(center_cell);
    return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
  }

  private void Paint()
  {
    foreach (int cellsInRadiu in this.cellsInRadius)
    {
      if (Grid.IsValidCell(cellsInRadiu) && (!Grid.Foundation[cellsInRadiu] || this.affectFoundation))
        this.OnPaintCell(cellsInRadiu, Grid.GetCellDistance(this.currentCell, cellsInRadiu));
    }
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    this.currentCell = Grid.PosToCell(cursorPos);
    base.OnMouseMove(cursorPos);
    this.cellsInRadius.Clear();
    foreach (Vector2 brushOffset in this.brushOffsets)
      this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int) brushOffset.x, (int) brushOffset.y)));
    if (!this.dragging)
      return;
    this.Paint();
  }

  protected virtual void OnPaintCell(int cell, int distFromOrigin)
  {
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.DragStraight))
      this.dragAxis = BrushTool.DragAxis.None;
    else if (this.interceptNumberKeysForPriority)
      this.HandlePriortyKeysDown(e);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.DragStraight))
      this.dragAxis = BrushTool.DragAxis.Invalid;
    else if (this.interceptNumberKeysForPriority)
      this.HandlePriorityKeysUp(e);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void HandlePriortyKeysDown(KButtonEvent e)
  {
    Action action = e.GetAction();
    if (Action.Plan1 > action || action > Action.Plan10 || !e.TryConsume(action))
      return;
    int priority_value = (int) (action - 36 + 1);
    if (priority_value <= 9)
      ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, priority_value), true);
    else
      ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
  }

  private void HandlePriorityKeysUp(KButtonEvent e)
  {
    Action action = e.GetAction();
    if (Action.Plan1 > action || action > Action.Plan10)
      return;
    e.TryConsume(action);
  }

  public override void OnFocus(bool focus)
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer.SetActive(focus);
    this.hasFocus = focus;
    base.OnFocus(focus);
  }

  private void OnTutorialOpened(object data)
  {
    this.dragging = false;
  }

  public override bool ShowHoverUI()
  {
    return this.dragging || base.ShowHoverUI();
  }

  public override void LateUpdate()
  {
    base.LateUpdate();
  }

  private enum DragAxis
  {
    Invalid = -1, // 0xFFFFFFFF
    None = 0,
    Horizontal = 1,
    Vertical = 2,
  }
}
