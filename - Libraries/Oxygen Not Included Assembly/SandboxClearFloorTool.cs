// Decompiled with JetBrains decompiler
// Type: SandboxClearFloorTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SandboxClearFloorTool : BrushTool
{
  public static SandboxClearFloorTool instance;

  public static void DestroyInstance()
  {
    SandboxClearFloorTool.instance = (SandboxClearFloorTool) null;
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
    SandboxClearFloorTool.instance = this;
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
    SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(1f);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
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
    bool flag = false;
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      Pickupable pickup = pickupable;
      if (!((UnityEngine.Object) pickup.storage != (UnityEngine.Object) null) && Grid.PosToCell((KMonoBehaviour) pickup) == cell && (UnityEngine.Object) Components.LiveMinionIdentities.Items.Find((Predicate<MinionIdentity>) (match => (UnityEngine.Object) match.gameObject == (UnityEngine.Object) pickup.gameObject)) == (UnityEngine.Object) null)
      {
        if (!flag)
        {
          UISounds.PlaySound(UISounds.Sound.Negative);
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
          flag = true;
        }
        Util.KDestroyGameObject(pickup.gameObject);
      }
    }
  }
}
