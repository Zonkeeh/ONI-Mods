// Decompiled with JetBrains decompiler
// Type: AttackToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AttackToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> hover_objects)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar(false);
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    drawer.EndShadowBar();
    if (hover_objects != null)
    {
      foreach (KSelectable hoverObject in hover_objects)
      {
        if ((Object) hoverObject.GetComponent<AttackableBase>() != (Object) null)
        {
          drawer.BeginShadowBar(false);
          drawer.DrawText(hoverObject.GetProperName().ToUpper(), this.Styles_Title.Standard);
          drawer.EndShadowBar();
          break;
        }
      }
    }
    drawer.EndDrawing();
  }
}
