// Decompiled with JetBrains decompiler
// Type: CancelTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CancelTool : FilteredDragTool
{
  public static CancelTool Instance;

  public static void DestroyInstance()
  {
    CancelTool.Instance = (CancelTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    CancelTool.Instance = this;
  }

  protected override void GetDefaultFilters(
    Dictionary<string, ToolParameterMenu.ToggleState> filters)
  {
    base.GetDefaultFilters(filters);
    filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
    filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
  }

  protected override string GetConfirmSound()
  {
    return "Tile_Confirm_NegativeTool";
  }

  protected override string GetDragSound()
  {
    return "Tile_Drag_NegativeTool";
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int index = 0; index < 39; ++index)
    {
      GameObject gameObject = Grid.Objects[cell, index];
      if ((Object) gameObject != (Object) null && this.IsActiveLayer(this.GetFilterLayerFromGameObject(gameObject)))
        gameObject.Trigger(2127324410, (object) null);
    }
  }

  protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
  {
    Vector2 regularizedPos1 = this.GetRegularizedPos(Vector2.Min((Vector2) downPos, (Vector2) upPos), true);
    Vector2 regularizedPos2 = this.GetRegularizedPos(Vector2.Max((Vector2) downPos, (Vector2) upPos), false);
    AttackTool.MarkForAttack(regularizedPos1, regularizedPos2, false);
    CaptureTool.MarkForCapture(regularizedPos1, regularizedPos2, false);
  }
}
