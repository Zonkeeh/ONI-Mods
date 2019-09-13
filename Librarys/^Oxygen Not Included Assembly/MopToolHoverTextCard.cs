// Decompiled with JetBrains decompiler
// Type: MopToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MopToolHoverTextCard : HoverTextConfiguration
{
  private MopToolHoverTextCard.HoverScreenFields hoverScreenElements;

  public override void UpdateHoverElements(List<KSelectable> selected)
  {
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if (!Grid.IsValidCell(cell))
      return;
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar(false);
    if (Grid.IsVisible(cell))
    {
      this.DrawTitle(instance, drawer);
      this.DrawInstructions(HoverTextScreen.Instance, drawer);
      Element element = Grid.Element[cell];
      if (element.IsLiquid)
      {
        drawer.NewLine(26);
        drawer.DrawText(element.nameUpperCase, this.Styles_Title.Standard);
        drawer.NewLine(26);
        drawer.DrawIcon(instance.GetSprite("dash"), 18);
        drawer.DrawText(element.GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
        drawer.NewLine(26);
        drawer.DrawIcon(instance.GetSprite("dash"), 18);
        string[] strArray = WorldInspector.MassStringsReadOnly(cell);
        drawer.DrawText(strArray[0], this.Styles_Values.Property.Standard);
        drawer.DrawText(strArray[1], this.Styles_Values.Property_Decimal.Standard);
        drawer.DrawText(strArray[2], this.Styles_Values.Property.Standard);
        drawer.DrawText(strArray[3], this.Styles_Values.Property.Standard);
      }
    }
    else
    {
      drawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
      drawer.DrawText(STRINGS.UI.TOOLS.GENERIC.UNKNOWN.ToString().ToUpper(), this.Styles_BodyText.Standard);
    }
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }

  private struct HoverScreenFields
  {
    public GameObject UnknownAreaLine;
    public Image ElementStateIcon;
    public LocText ElementCategory;
    public LocText ElementName;
    public LocText[] ElementMass;
    public LocText ElementHardness;
    public LocText ElementHardnessDescription;
  }
}
