// Decompiled with JetBrains decompiler
// Type: MoveToLocationToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocationToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!Grid.IsValidCell(cell))
      return;
    HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();
    drawer.BeginShadowBar(false);
    this.DrawTitle(HoverTextScreen.Instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    if (!MoveToLocationTool.Instance.CanMoveTo(cell))
    {
      drawer.NewLine(26);
      drawer.DrawText((string) UI.TOOLS.MOVETOLOCATION.UNREACHABLE, this.HoverTextStyleSettings[1]);
    }
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }
}
