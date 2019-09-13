// Decompiled with JetBrains decompiler
// Type: PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : KMonoBehaviour, IInputHandler
{
  private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0.0f, 0.0f);
  public InterfaceTool[] tools;
  private InterfaceTool activeTool;
  private bool DebugHidingCursor;
  private const float MIN_DRAG_DIST = 6f;
  private const float MIN_DRAG_TIME = 0.3f;
  private Action dragAction;
  private bool dragging;
  private bool queueStopDrag;
  private Vector3 startDragPos;
  private float startDragTime;
  private Vector3 dragDelta;
  private Vector3 worldDragDelta;

  public string handlerName
  {
    get
    {
      return nameof (PlayerController);
    }
  }

  public KInputHandler inputHandler { get; set; }

  public InterfaceTool ActiveTool
  {
    get
    {
      return this.activeTool;
    }
  }

  public static PlayerController Instance { get; private set; }

  public static void DestroyInstance()
  {
    PlayerController.Instance = (PlayerController) null;
  }

  protected override void OnPrefabInit()
  {
    PlayerController.Instance = this;
    for (int index = 0; index < this.tools.Length; ++index)
    {
      GameObject gameObject = Util.KInstantiate(this.tools[index].gameObject, this.gameObject, (string) null);
      this.tools[index] = gameObject.GetComponent<InterfaceTool>();
      this.tools[index].gameObject.SetActive(true);
      this.tools[index].gameObject.SetActive(false);
    }
  }

  protected override void OnSpawn()
  {
    this.ActivateTool(this.tools[0]);
  }

  private Vector3 GetCursorPos()
  {
    return PlayerController.GetCursorPos(KInputManager.GetMousePos());
  }

  public static Vector3 GetCursorPos(Vector3 mouse_pos)
  {
    RaycastHit hitInfo;
    Vector3 vector3;
    if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out hitInfo, float.PositiveInfinity, Game.BlockSelectionLayerMask))
    {
      vector3 = hitInfo.point;
    }
    else
    {
      mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
      vector3 = Camera.main.ScreenToWorldPoint(mouse_pos);
    }
    float x = vector3.x;
    float y = vector3.y;
    float num1 = Mathf.Min(Mathf.Max(x, 0.0f), Grid.WidthInMeters);
    float num2 = Mathf.Min(Mathf.Max(y, 0.0f), Grid.HeightInMeters);
    vector3.x = num1;
    vector3.y = num2;
    return vector3;
  }

  private void UpdateHover()
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if (!((Object) current != (Object) null))
      return;
    this.activeTool.OnFocus(!current.IsPointerOverGameObject());
  }

  private void Update()
  {
    this.UpdateDrag();
    if ((bool) ((Object) this.activeTool) && this.activeTool.enabled)
    {
      this.UpdateHover();
      Vector3 cursorPos = this.GetCursorPos();
      if (cursorPos != this.prevMousePos)
      {
        this.prevMousePos = cursorPos;
        this.activeTool.OnMouseMove(cursorPos);
      }
    }
    if (!Input.GetKeyDown(KeyCode.F12) || !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
      return;
    this.DebugHidingCursor = !this.DebugHidingCursor;
    Cursor.visible = !this.DebugHidingCursor;
    HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
  }

  private void LateUpdate()
  {
    if (!this.queueStopDrag)
      return;
    this.queueStopDrag = false;
    this.dragging = false;
    this.dragAction = Action.Invalid;
    this.dragDelta = Vector3.zero;
    this.worldDragDelta = Vector3.zero;
  }

  public void ActivateTool(InterfaceTool tool)
  {
    if ((Object) this.activeTool == (Object) tool)
      return;
    this.DeactivateTool(tool);
    this.activeTool = tool;
    this.activeTool.enabled = true;
    this.activeTool.gameObject.SetActive(true);
    this.activeTool.ActivateTool();
    this.UpdateHover();
  }

  public void ToolDeactivated(InterfaceTool tool)
  {
    if ((Object) this.activeTool == (Object) tool && (Object) this.activeTool != (Object) null)
      this.DeactivateTool((InterfaceTool) null);
    if (!((Object) this.activeTool == (Object) null))
      return;
    this.ActivateTool((InterfaceTool) SelectTool.Instance);
  }

  private void DeactivateTool(InterfaceTool new_tool = null)
  {
    if (!((Object) this.activeTool != (Object) null))
      return;
    this.activeTool.enabled = false;
    this.activeTool.gameObject.SetActive(false);
    InterfaceTool activeTool = this.activeTool;
    this.activeTool = (InterfaceTool) null;
    activeTool.DeactivateTool(new_tool);
  }

  public bool IsUsingDefaultTool()
  {
    return (Object) this.activeTool == (Object) this.tools[0];
  }

  private void StartDrag(Action action)
  {
    if (this.dragAction != Action.Invalid)
      return;
    this.dragAction = action;
    this.startDragPos = KInputManager.GetMousePos();
    this.startDragTime = Time.unscaledTime;
  }

  private void UpdateDrag()
  {
    this.dragDelta = (Vector3) Vector2.zero;
    Vector3 mousePos = KInputManager.GetMousePos();
    if (!this.dragging && this.dragAction != Action.Invalid && ((double) (mousePos - this.startDragPos).magnitude > 6.0 || (double) Time.unscaledTime - (double) this.startDragTime > 0.300000011920929))
      this.dragging = true;
    if (!this.dragging)
      return;
    this.dragDelta = mousePos - this.startDragPos;
    this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
    this.startDragPos = mousePos;
  }

  private void StopDrag(Action action)
  {
    if (this.dragAction != action)
      return;
    this.queueStopDrag = true;
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.ToggleScreenshotMode))
      DebugHandler.ToggleScreenshotMode();
    else if (DebugHandler.HideUI && e.TryConsume(Action.Escape))
    {
      DebugHandler.ToggleScreenshotMode();
    }
    else
    {
      if ((Object) this.activeTool == (Object) null || !this.activeTool.enabled)
        return;
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
      eventData.position = (Vector2) KInputManager.GetMousePos();
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      if ((Object) current != (Object) null)
      {
        current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
          return;
      }
      if (e.TryConsume(Action.MouseLeft) || e.TryConsume(Action.ShiftMouseLeft))
      {
        this.StartDrag(Action.MouseLeft);
        this.activeTool.OnLeftClickDown(this.GetCursorPos());
      }
      else if (e.IsAction(Action.MouseRight))
      {
        this.StartDrag(Action.MouseRight);
        this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
      }
      else if (e.IsAction(Action.MouseMiddle))
        this.StartDrag(Action.MouseMiddle);
      else
        this.activeTool.OnKeyDown(e);
    }
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
      this.StopDrag(Action.MouseLeft);
    else if (e.IsAction(Action.MouseRight))
      this.StopDrag(Action.MouseRight);
    else if (e.IsAction(Action.MouseMiddle))
      this.StopDrag(Action.MouseMiddle);
    if ((Object) this.activeTool == (Object) null || !this.activeTool.enabled || !this.activeTool.hasFocus)
      return;
    if (e.TryConsume(Action.MouseLeft) || e.TryConsume(Action.ShiftMouseLeft))
      this.activeTool.OnLeftClickUp(this.GetCursorPos());
    else if (e.IsAction(Action.MouseRight))
      this.activeTool.OnRightClickUp(this.GetCursorPos());
    else
      this.activeTool.OnKeyUp(e);
  }

  public bool ConsumeIfNotDragging(KButtonEvent e, Action action)
  {
    if (this.dragAction != action || !this.dragging)
      return e.TryConsume(action);
    return false;
  }

  public bool IsDragging()
  {
    return this.dragAction != Action.Invalid;
  }

  public Vector3 GetDragDelta()
  {
    return this.dragDelta;
  }

  public Vector3 GetWorldDragDelta()
  {
    return this.worldDragDelta;
  }
}
