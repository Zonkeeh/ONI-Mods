// Decompiled with JetBrains decompiler
// Type: VisibilityTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VisibilityTester : KMonoBehaviour
{
  public static VisibilityTester Instance;
  public bool enableTesting;

  public static void DestroyInstance()
  {
    VisibilityTester.Instance = (VisibilityTester) null;
  }

  protected override void OnPrefabInit()
  {
    VisibilityTester.Instance = this;
  }

  private void Update()
  {
    if ((Object) SelectTool.Instance == (Object) null || (Object) SelectTool.Instance.selected == (Object) null || !this.enableTesting)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) SelectTool.Instance.selected);
    int mouseCell = DebugHandler.GetMouseCell();
    string text = string.Empty + "Source Cell: " + (object) cell + "\n" + "Target Cell: " + (object) mouseCell + "\n" + "Visible: " + (object) Grid.VisibilityTest(cell, mouseCell, false);
    for (int index = 0; index < 10000; ++index)
      Grid.VisibilityTest(cell, mouseCell, false);
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }
}
