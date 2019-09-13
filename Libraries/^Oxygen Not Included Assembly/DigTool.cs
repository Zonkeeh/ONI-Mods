// Decompiled with JetBrains decompiler
// Type: DigTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DigTool : DragTool
{
  public static DigTool Instance;

  public static void DestroyInstance()
  {
    DigTool.Instance = (DigTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DigTool.Instance = this;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.Solid[cell])
    {
      foreach (Uprootable uprootable in Components.Uprootables.Items)
      {
        if (Grid.PosToCell(uprootable.gameObject) == cell)
        {
          uprootable.MarkForUproot(true);
          break;
        }
        OccupyArea area = uprootable.area;
        if ((Object) area != (Object) null && area.CheckIsOccupying(cell))
          uprootable.MarkForUproot(true);
      }
    }
    if (DebugHandler.InstantBuildMode)
    {
      if (!Grid.IsValidCell(cell) || !Grid.Solid[cell] || Grid.Foundation[cell])
        return;
      WorldDamage.Instance.DestroyCell(cell, -1);
    }
    else
    {
      GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
      if (!((Object) gameObject != (Object) null))
        return;
      Prioritizable component = gameObject.GetComponent<Prioritizable>();
      if (!((Object) component != (Object) null))
        return;
      component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
    }
  }

  public static GameObject PlaceDig(int cell, int animationDelay = 0)
  {
    if (Grid.Solid[cell] && !Grid.Foundation[cell] && (Object) Grid.Objects[cell, 7] == (Object) null)
    {
      for (int index = 0; index < 39; ++index)
      {
        if ((Object) Grid.Objects[cell, index] != (Object) null && (Object) Grid.Objects[cell, index].GetComponent<Constructable>() != (Object) null)
          return (GameObject) null;
      }
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), (GameObject) null, (string) null);
      gameObject.SetActive(true);
      Grid.Objects[cell, 7] = gameObject;
      Vector3 posCbc = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
      float num = -0.15f;
      posCbc.z += num;
      gameObject.transform.SetPosition(posCbc);
      gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0.0f, (float) animationDelay * 0.02f));
      return gameObject;
    }
    if ((Object) Grid.Objects[cell, 7] != (Object) null)
      return Grid.Objects[cell, 7];
    return (GameObject) null;
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
