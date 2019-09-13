// Decompiled with JetBrains decompiler
// Type: EmptyPipeTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EmptyPipeTool : FilteredDragTool
{
  public static EmptyPipeTool Instance;

  public static void DestroyInstance()
  {
    EmptyPipeTool.Instance = (EmptyPipeTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    EmptyPipeTool.Instance = this;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int index = 0; index < 39; ++index)
    {
      if (this.IsActiveLayer((ObjectLayer) index))
      {
        GameObject gameObject = Grid.Objects[cell, index];
        if (!((Object) gameObject == (Object) null))
        {
          EmptyConduitWorkable component1 = gameObject.GetComponent<EmptyConduitWorkable>();
          if (!((Object) component1 == (Object) null))
          {
            if (DebugHandler.InstantBuildMode)
            {
              component1.EmptyPipeContents();
            }
            else
            {
              component1.MarkForEmptying();
              Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
              if ((Object) component2 != (Object) null)
                component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
            }
          }
        }
      }
    }
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    ToolMenu.Instance.PriorityScreen.Show(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    ToolMenu.Instance.PriorityScreen.Show(false);
  }

  protected override void GetDefaultFilters(
    Dictionary<string, ToolParameterMenu.ToggleState> filters)
  {
    filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.On);
    filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.On);
    filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.On);
  }
}
