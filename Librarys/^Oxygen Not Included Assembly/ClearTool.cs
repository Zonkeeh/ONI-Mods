// Decompiled with JetBrains decompiler
// Type: ClearTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ClearTool : DragTool
{
  public static ClearTool Instance;

  public static void DestroyInstance()
  {
    ClearTool.Instance = (ClearTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ClearTool.Instance = this;
    this.interceptNumberKeysForPriority = true;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    GameObject gameObject1 = Grid.Objects[cell, 3];
    if ((Object) gameObject1 == (Object) null)
      return;
    ObjectLayerListItem objectLayerListItem = gameObject1.GetComponent<Pickupable>().objectLayerListItem;
    while (objectLayerListItem != null)
    {
      GameObject gameObject2 = objectLayerListItem.gameObject;
      objectLayerListItem = objectLayerListItem.nextItem;
      if (!((Object) gameObject2 == (Object) null) && !((Object) gameObject2.GetComponent<MinionIdentity>() != (Object) null) && gameObject2.GetComponent<Clearable>().isClearable)
      {
        gameObject2.GetComponent<Clearable>().MarkForClear(false);
        Prioritizable component = gameObject2.GetComponent<Prioritizable>();
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
