// Decompiled with JetBrains decompiler
// Type: PrebuildToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

public class PrebuildToolHoverTextCard : HoverTextConfiguration
{
  public PlanScreen.RequirementsState currentReqState;
  public BuildingDef currentDef;

  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
    hoverTextDrawer.BeginShadowBar(false);
    switch (this.currentReqState)
    {
      case PlanScreen.RequirementsState.Tech:
        Tech parentTech = Db.Get().TechItems.Get(this.currentDef.PrefabID).parentTech;
        hoverTextDrawer.DrawText(string.Format((string) UI.PRODUCTINFO_RESEARCHREQUIRED, (object) parentTech.Name).ToUpper(), this.HoverTextStyleSettings[0]);
        break;
      case PlanScreen.RequirementsState.Materials:
      case PlanScreen.RequirementsState.Complete:
        hoverTextDrawer.DrawText(UI.TOOLTIPS.NOMATERIAL.text.ToUpper(), this.HoverTextStyleSettings[0]);
        hoverTextDrawer.NewLine(26);
        hoverTextDrawer.DrawText((string) UI.TOOLTIPS.SELECTAMATERIAL, this.HoverTextStyleSettings[1]);
        break;
    }
    hoverTextDrawer.NewLine(26);
    hoverTextDrawer.DrawIcon(instance.GetSprite("icon_mouse_right"), 18);
    hoverTextDrawer.DrawText(this.backStr, this.Styles_Instruction.Standard);
    hoverTextDrawer.EndShadowBar();
    hoverTextDrawer.EndDrawing();
  }
}
