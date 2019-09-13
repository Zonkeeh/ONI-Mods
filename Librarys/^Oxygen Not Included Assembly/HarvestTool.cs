// Decompiled with JetBrains decompiler
// Type: HarvestTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class HarvestTool : DragTool
{
  private Dictionary<string, ToolParameterMenu.ToggleState> options = new Dictionary<string, ToolParameterMenu.ToggleState>();
  public GameObject Placer;
  public static HarvestTool Instance;
  public Texture2D[] visualizerTextures;

  public static void DestroyInstance()
  {
    HarvestTool.Instance = (HarvestTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    HarvestTool.Instance = this;
    this.options.Add("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On);
    this.options.Add("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off);
    this.viewMode = OverlayModes.Harvest.ID;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.IsValidCell(cell))
      return;
    foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items)
    {
      OccupyArea area = harvestDesignatable.area;
      if (Grid.PosToCell((KMonoBehaviour) harvestDesignatable) == cell || (Object) area != (Object) null && area.CheckIsOccupying(cell))
      {
        if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
          harvestDesignatable.SetHarvestWhenReady(true);
        else if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
          harvestDesignatable.SetHarvestWhenReady(false);
        Prioritizable component = harvestDesignatable.GetComponent<Prioritizable>();
        if ((Object) component != (Object) null)
          component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
      }
    }
  }

  public void Update()
  {
    MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
    if (!((Object) componentInChildren != (Object) null))
      return;
    if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
    {
      componentInChildren.material.mainTexture = (Texture) this.visualizerTextures[0];
    }
    else
    {
      if (this.options["DO_NOT_HARVEST"] != ToolParameterMenu.ToggleState.On)
        return;
      componentInChildren.material.mainTexture = (Texture) this.visualizerTextures[1];
    }
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    base.OnLeftClickUp(cursor_pos);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show(true);
    ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
    ToolMenu.Instance.toolParameterMenu.ClearMenu();
  }
}
