// Decompiled with JetBrains decompiler
// Type: MopTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class MopTool : DragTool
{
  public static float maxMopAmt = 150f;
  private GameObject Placer;
  public static MopTool Instance;
  private SimHashes Element;

  public static void DestroyInstance()
  {
    MopTool.Instance = (MopTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
    this.interceptNumberKeysForPriority = true;
    MopTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.IsValidCell(cell))
      return;
    if (DebugHandler.InstantBuildMode)
    {
      if (!Grid.IsValidCell(cell))
        return;
      Moppable.MopCell(cell, 1000000f, (System.Action<Sim.MassConsumedCallback, object>) null);
    }
    else
    {
      GameObject gameObject = Grid.Objects[cell, 8];
      if (!Grid.Solid[cell] && (UnityEngine.Object) gameObject == (UnityEngine.Object) null && Grid.Element[cell].IsLiquid)
      {
        bool flag1 = Grid.Solid[Grid.CellBelow(cell)];
        bool flag2 = (double) Grid.Mass[cell] <= (double) MopTool.maxMopAmt;
        if (flag1 && flag2)
        {
          gameObject = Util.KInstantiate(this.Placer, (GameObject) null, (string) null);
          Grid.Objects[cell, 8] = gameObject;
          Vector3 posCbc = Grid.CellToPosCBC(cell, this.visualizerLayer);
          float num = -0.15f;
          posCbc.z += num;
          gameObject.transform.SetPosition(posCbc);
          gameObject.SetActive(true);
        }
        else
        {
          string text = (string) UI.TOOLS.MOP.TOO_MUCH_LIQUID;
          if (!flag1)
            text = (string) UI.TOOLS.MOP.NOT_ON_FLOOR;
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, text, (Transform) null, Grid.CellToPosCBC(cell, this.visualizerLayer), 1.5f, false, false);
        }
      }
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return;
      Prioritizable component = gameObject.GetComponent<Prioritizable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
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
