// Decompiled with JetBrains decompiler
// Type: HoverTextConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextConfiguration : KMonoBehaviour
{
  public string ToolNameStringKey = string.Empty;
  public string ActionStringKey = string.Empty;
  [HideInInspector]
  public string ActionName = string.Empty;
  public TextStyleSetting[] HoverTextStyleSettings;
  [HideInInspector]
  public string ToolName;
  protected string backStr;
  public TextStyleSetting ToolTitleTextStyle;
  public HoverTextConfiguration.TextStylePair Styles_Title;
  public HoverTextConfiguration.TextStylePair Styles_BodyText;
  public HoverTextConfiguration.TextStylePair Styles_Instruction;
  public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConfigureHoverScreen();
  }

  protected virtual void ConfigureTitle(HoverTextScreen screen)
  {
    if (!string.IsNullOrEmpty(this.ToolName))
      return;
    this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
  }

  protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
  {
    drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
  }

  protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
  {
    TextStyleSetting standard = this.Styles_Instruction.Standard;
    drawer.NewLine(26);
    drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
    drawer.DrawText(this.ActionName, standard);
    drawer.AddIndent(8);
    drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
    drawer.DrawText(this.backStr, standard);
  }

  public virtual void ConfigureHoverScreen()
  {
    if (!string.IsNullOrEmpty(this.ActionStringKey))
      this.ActionName = (string) Strings.Get(this.ActionStringKey);
    this.ConfigureTitle(HoverTextScreen.Instance);
    this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
  }

  public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar(false);
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    drawer.EndShadowBar();
    drawer.EndDrawing();
  }

  [Serializable]
  public struct TextStylePair
  {
    public TextStyleSetting Standard;
    public TextStyleSetting Selected;
  }

  [Serializable]
  public struct ValuePropertyTextStyles
  {
    public HoverTextConfiguration.TextStylePair Property;
    public HoverTextConfiguration.TextStylePair Property_Decimal;
    public HoverTextConfiguration.TextStylePair Property_Unit;
  }
}
