// Decompiled with JetBrains decompiler
// Type: DragTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System;
using UnityEngine;

public class DragTool : InterfaceTool
{
  [SerializeField]
  private Color32 areaColour = (Color32) new Color(1f, 1f, 1f, 0.5f);
  private DragTool.Mode mode = DragTool.Mode.Box;
  private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;
  protected bool canChangeDragAxis = true;
  [SerializeField]
  private Texture2D boxCursor;
  [SerializeField]
  private GameObject areaVisualizer;
  [SerializeField]
  private GameObject areaVisualizerTextPrefab;
  protected SpriteRenderer areaVisualizerSpriteRenderer;
  protected Guid areaVisualizerText;
  protected Vector3 placementPivot;
  protected bool interceptNumberKeysForPriority;
  private bool dragging;
  private Vector3 previousCursorPos;
  protected Vector3 downPos;

  public bool Dragging
  {
    get
    {
      return this.dragging;
    }
  }

  protected virtual DragTool.Mode GetMode()
  {
    return this.mode;
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.dragging = false;
    this.SetMode(this.mode);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if (this.areaVisualizerText != Guid.Empty)
    {
      NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
      this.areaVisualizerText = Guid.Empty;
    }
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
    this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
    this.areaVisualizer.transform.SetParent(this.transform);
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
    this.previousCursorPos = cursor_pos;
    KScreenManager.Instance.SetEventSystemEnabled(false);
    if ((UnityEngine.Object) this.areaVisualizerTextPrefab != (UnityEngine.Object) null)
    {
      this.areaVisualizerText = NameDisplayScreen.Instance.AddWorldText(string.Empty, this.areaVisualizerTextPrefab);
      NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = (Color) this.areaColour;
    }
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
          break;
        this.AddDragPoint(cursor_pos);
        break;
      case DragTool.Mode.Box:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(false);
        if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
          break;
        this.areaVisualizer.SetActive(true);
        this.areaVisualizer.transform.SetPosition(cursor_pos);
        this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
        break;
    }
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    cursor_pos -= this.placementPivot;
    KScreenManager.Instance.SetEventSystemEnabled(true);
    this.dragAxis = DragTool.DragAxis.Invalid;
    if (!this.dragging)
      return;
    this.dragging = false;
    DragTool.Mode mode = this.GetMode();
    if (this.areaVisualizerText != Guid.Empty)
    {
      NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
      this.areaVisualizerText = Guid.Empty;
    }
    if (mode != DragTool.Mode.Box || !((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer.SetActive(false);
    int x1;
    int y1;
    Grid.PosToXY(this.downPos, out x1, out y1);
    int num1 = x1;
    int num2 = y1;
    int x2;
    int y2;
    Grid.PosToXY(cursor_pos, out x2, out y2);
    if (x2 < x1)
      Util.Swap<int>(ref x1, ref x2);
    if (y2 < y1)
      Util.Swap<int>(ref y1, ref y2);
    for (int y3 = y1; y3 <= y2; ++y3)
    {
      for (int x3 = x1; x3 <= x2; ++x3)
      {
        int cell = Grid.XYToCell(x3, y3);
        if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
        {
          int num3 = y3 - num2;
          int num4 = x3 - num1;
          int num5 = Mathf.Abs(num3);
          int num6 = Mathf.Abs(num4);
          this.OnDragTool(cell, num5 + num6);
        }
      }
    }
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound(), false));
    this.OnDragComplete(this.downPos, cursor_pos);
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

  public override void OnMouseMove(Vector3 cursorPos)
  {
    if (this.dragging)
    {
      if (Input.GetKey((KeyCode) Global.Instance.GetInputManager().GetDefaultController().GetInputForAction(Action.DragStraight)))
      {
        Vector3 vector3 = cursorPos - this.downPos;
        if ((this.canChangeDragAxis || this.dragAxis == DragTool.DragAxis.Invalid) && (double) vector3.sqrMagnitude > 0.707000017166138)
          this.dragAxis = (double) Mathf.Abs(vector3.x) >= (double) Mathf.Abs(vector3.y) ? DragTool.DragAxis.Horizontal : DragTool.DragAxis.Vertical;
      }
      else
        this.dragAxis = DragTool.DragAxis.Invalid;
      switch (this.dragAxis)
      {
        case DragTool.DragAxis.Horizontal:
          cursorPos.y = this.downPos.y;
          break;
        case DragTool.DragAxis.Vertical:
          cursorPos.x = this.downPos.x;
          break;
      }
    }
    base.OnMouseMove(cursorPos);
    if (!this.dragging)
      return;
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        this.AddDragPoints(cursorPos, this.previousCursorPos);
        if (this.areaVisualizerText != Guid.Empty)
        {
          int dragLength = this.GetDragLength();
          LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
          component.text = string.Format((string) UI.TOOLS.TOOL_LENGTH_FMT, (object) dragLength);
          Vector3 position = Grid.CellToPos(Grid.PosToCell(cursorPos)) + new Vector3(0.0f, 1f, 0.0f);
          component.transform.SetPosition(position);
          break;
        }
        break;
      case DragTool.Mode.Box:
        Vector2 input1 = (Vector2) Vector3.Max(this.downPos, cursorPos);
        Vector2 input2 = (Vector2) Vector3.Min(this.downPos, cursorPos);
        Vector2 regularizedPos1 = this.GetRegularizedPos(input1, false);
        Vector2 regularizedPos2 = this.GetRegularizedPos(input2, true);
        Vector2 vector2_1 = regularizedPos1 - regularizedPos2;
        Vector2 vector2_2 = (regularizedPos1 + regularizedPos2) * 0.5f;
        this.areaVisualizer.transform.SetPosition((Vector3) new Vector2(vector2_2.x, vector2_2.y));
        int num1 = (int) ((double) regularizedPos1.x - (double) regularizedPos2.x + ((double) regularizedPos1.y - (double) regularizedPos2.y) - 1.0);
        if (this.areaVisualizerSpriteRenderer.size != vector2_1)
        {
          string sound = GlobalAssets.GetSound(this.GetDragSound(), false);
          if (sound != null)
          {
            EventInstance instance = SoundEvent.BeginOneShot(sound, this.areaVisualizer.transform.GetPosition());
            int num2 = (int) instance.setParameterValue("tileCount", (float) num1);
            SoundEvent.EndOneShot(instance);
          }
        }
        this.areaVisualizerSpriteRenderer.size = vector2_1;
        if (this.areaVisualizerText != Guid.Empty)
        {
          Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector2_1.x), Mathf.RoundToInt(vector2_1.y));
          LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
          component.text = string.Format((string) UI.TOOLS.TOOL_AREA_FMT, (object) vector2I.x, (object) vector2I.y);
          Vector2 vector2_3 = vector2_2;
          component.transform.SetPosition((Vector3) vector2_3);
          break;
        }
        break;
    }
    this.previousCursorPos = cursorPos;
  }

  protected virtual void OnDragTool(int cell, int distFromOrigin)
  {
  }

  protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
  {
  }

  protected virtual int GetDragLength()
  {
    return 0;
  }

  private void AddDragPoint(Vector3 cursorPos)
  {
    int cell = Grid.PosToCell(cursorPos);
    if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
      return;
    this.OnDragTool(cell, 0);
  }

  private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
  {
    Vector3 vector3 = cursorPos - previousCursorPos;
    float magnitude = vector3.magnitude;
    float num1 = Grid.CellSizeInMeters * 0.25f;
    int num2 = 1 + (int) ((double) magnitude / (double) num1);
    vector3.Normalize();
    for (int index = 0; index < num2; ++index)
      this.AddDragPoint(previousCursorPos + vector3 * ((float) index * num1));
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.interceptNumberKeysForPriority)
      this.HandlePriortyKeysDown(e);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.interceptNumberKeysForPriority)
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

  protected void SetMode(DragTool.Mode newMode)
  {
    this.mode = newMode;
    switch (this.mode)
    {
      case DragTool.Mode.Brush:
        if ((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null)
          this.areaVisualizer.SetActive(false);
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(true);
        this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
        break;
      case DragTool.Mode.Box:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(true);
        this.mode = DragTool.Mode.Box;
        this.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
        break;
    }
  }

  public override void OnFocus(bool focus)
  {
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(focus);
        this.hasFocus = focus;
        break;
      case DragTool.Mode.Box:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null && !this.dragging)
          this.visualizer.SetActive(focus);
        this.hasFocus = focus || this.dragging;
        break;
    }
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

  public enum Mode
  {
    Brush,
    Box,
  }
}
