// Decompiled with JetBrains decompiler
// Type: DeconstructTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DeconstructTool : FilteredDragTool
{
  public static DeconstructTool Instance;

  public static void DestroyInstance()
  {
    DeconstructTool.Instance = (DeconstructTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DeconstructTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
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
    this.DeconstructCell(cell);
  }

  public void DeconstructCell(int cell)
  {
    for (int index = 0; index < 39; ++index)
    {
      GameObject gameObject = Grid.Objects[cell, index];
      if ((Object) gameObject != (Object) null && this.IsActiveLayer(this.GetFilterLayerFromGameObject(gameObject)))
      {
        gameObject.Trigger(-790448070, (object) null);
        Prioritizable component = gameObject.GetComponent<Prioritizable>();
        if ((Object) component != (Object) null)
          component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
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
}
