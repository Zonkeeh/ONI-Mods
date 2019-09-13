// Decompiled with JetBrains decompiler
// Type: StampTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class StampTool : InterfaceTool
{
  private bool ready = true;
  private int placementCell = Grid.InvalidCell;
  public static StampTool Instance;
  public TemplateContainer stampTemplate;
  public GameObject PlacerPrefab;
  private bool selectAffected;
  private bool deactivateOnStamp;

  public static void DestroyInstance()
  {
    StampTool.Instance = (StampTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    StampTool.Instance = this;
  }

  public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
  {
    this.stampTemplate = template;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    this.selectAffected = SelectAffected;
    this.deactivateOnStamp = DeactivateOnStamp;
  }

  private void Update()
  {
    this.RefreshPreview(Grid.PosToCell(this.GetCursorPos()));
  }

  private Vector3 GetCursorPos()
  {
    return PlayerController.GetCursorPos(KInputManager.GetMousePos());
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    this.Stamp((Vector2) cursor_pos);
  }

  private void Stamp(Vector2 pos)
  {
    if (!this.ready)
      return;
    int cell1 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt((float) (-(double) this.stampTemplate.info.size.X / 2.0)), 0);
    int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
    int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt((float) (-(double) this.stampTemplate.info.size.Y / 2.0)));
    int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
    if (!Grid.IsValidBuildingCell(cell1) || !Grid.IsValidBuildingCell(cell2) || (!Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3)))
      return;
    this.ready = false;
    bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(true);
    List<GameObject> objects_to_destroy = new List<GameObject>();
    for (int index1 = 0; index1 < this.stampTemplate.cells.Count; ++index1)
    {
      for (int index2 = 0; index2 < 34; ++index2)
      {
        GameObject gameObject = Grid.Objects[Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index1].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index1].location_y)), index2];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && !objects_to_destroy.Contains(gameObject))
          objects_to_destroy.Add(gameObject);
      }
    }
    TemplateLoader.Stamp(this.stampTemplate, pos, (System.Action) (() => this.CompleteStamp(pauseOnComplete, objects_to_destroy)));
    if (this.selectAffected)
    {
      DebugBaseTemplateButton.Instance.ClearSelection();
      for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
        DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index].location_y)));
    }
    if (!this.deactivateOnStamp)
      return;
    this.DeactivateTool((InterfaceTool) null);
  }

  private void CompleteStamp(bool pause, List<GameObject> objects_to_destroy = null)
  {
    if (objects_to_destroy != null)
    {
      foreach (GameObject original in objects_to_destroy)
      {
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
    }
    if (pause)
      SpeedControlScreen.Instance.Pause(true);
    this.ready = true;
    this.OnDeactivateTool((InterfaceTool) null);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.RefreshPreview(Grid.InvalidCell);
  }

  public void RefreshPreview(int new_placement_cell)
  {
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (TemplateClasses.Cell cell1 in this.stampTemplate.cells)
    {
      if (this.placementCell != Grid.InvalidCell)
      {
        int cell2 = Grid.OffsetCell(this.placementCell, new CellOffset(cell1.location_x, cell1.location_y));
        if (Grid.IsValidCell(cell2))
          intList1.Add(cell2);
      }
      if (new_placement_cell != Grid.InvalidCell)
      {
        int cell2 = Grid.OffsetCell(new_placement_cell, new CellOffset(cell1.location_x, cell1.location_y));
        if (Grid.IsValidCell(cell2))
          intList2.Add(cell2);
      }
    }
    this.placementCell = new_placement_cell;
    foreach (int index in intList1)
    {
      if (!intList2.Contains(index))
      {
        GameObject go = Grid.Objects[index, 6];
        if ((UnityEngine.Object) go != (UnityEngine.Object) null)
          go.DeleteObject();
      }
    }
    foreach (int cell in intList2)
    {
      if (!intList1.Contains(cell) && (UnityEngine.Object) Grid.Objects[cell, 6] == (UnityEngine.Object) null)
      {
        GameObject gameObject = Util.KInstantiate(this.PlacerPrefab, (GameObject) null, (string) null);
        Grid.Objects[cell, 6] = gameObject;
        Vector3 posCbc = Grid.CellToPosCBC(cell, this.visualizerLayer);
        float num = -0.15f;
        posCbc.z += num;
        gameObject.transform.SetPosition(posCbc);
      }
    }
  }
}
