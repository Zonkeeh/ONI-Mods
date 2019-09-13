// Decompiled with JetBrains decompiler
// Type: CopySettingsTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CopySettingsTool : DragTool
{
  public static CopySettingsTool Instance;
  public GameObject Placer;
  private GameObject sourceGameObject;

  public static void DestroyInstance()
  {
    CopySettingsTool.Instance = (CopySettingsTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    CopySettingsTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public void SetSourceObject(GameObject sourceGameObject)
  {
    this.sourceGameObject = sourceGameObject;
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if ((Object) this.sourceGameObject == (Object) null || !Grid.IsValidCell(cell))
      return;
    CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.sourceGameObject = (GameObject) null;
  }
}
