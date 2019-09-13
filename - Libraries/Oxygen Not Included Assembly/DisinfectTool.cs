// Decompiled with JetBrains decompiler
// Type: DisinfectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DisinfectTool : DragTool
{
  public static DisinfectTool Instance;

  public static void DestroyInstance()
  {
    DisinfectTool.Instance = (DisinfectTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DisinfectTool.Instance = this;
    this.interceptNumberKeysForPriority = true;
    this.viewMode = OverlayModes.Disease.ID;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    for (int index = 0; index < 39; ++index)
    {
      GameObject gameObject = Grid.Objects[cell, index];
      if ((Object) gameObject != (Object) null)
      {
        Disinfectable component = gameObject.GetComponent<Disinfectable>();
        if ((Object) component != (Object) null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
          component.MarkForDisinfect(false);
      }
    }
  }
}
