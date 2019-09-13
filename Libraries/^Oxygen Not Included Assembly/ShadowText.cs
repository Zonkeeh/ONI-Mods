// Decompiled with JetBrains decompiler
// Type: ShadowText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Text.RegularExpressions;
using UnityEngine;

public class ShadowText : ShadowRect
{
  private UnityEngine.UI.Text shadowText;
  private UnityEngine.UI.Text mainText;

  protected override void MatchRect()
  {
    if ((Object) this.RectMain == (Object) null || (Object) this.RectShadow == (Object) null)
      return;
    if ((Object) this.shadowText == (Object) null)
      this.shadowText = this.RectShadow.GetComponent<UnityEngine.UI.Text>();
    if ((Object) this.mainText == (Object) null)
      this.mainText = this.RectMain.GetComponent<UnityEngine.UI.Text>();
    if ((Object) this.shadowText == (Object) null || (Object) this.mainText == (Object) null)
      return;
    if ((Object) this.shadowText.font != (Object) this.mainText.font)
      this.shadowText.font = this.mainText.font;
    if (this.shadowText.fontSize != this.mainText.fontSize)
      this.shadowText.fontSize = this.mainText.fontSize;
    if (this.shadowText.alignment != this.mainText.alignment)
      this.shadowText.alignment = this.mainText.alignment;
    if ((double) this.shadowText.lineSpacing != (double) this.mainText.lineSpacing)
      this.shadowText.lineSpacing = this.mainText.lineSpacing;
    string str = Regex.Replace(this.mainText.text, "\\</?color\\b.*?\\>", string.Empty);
    if (this.shadowText.text != str)
      this.shadowText.text = str;
    if (this.shadowText.color != this.shadowColor)
      this.shadowText.color = this.shadowColor;
    if (this.shadowText.horizontalOverflow != this.mainText.horizontalOverflow)
      this.shadowText.horizontalOverflow = this.mainText.horizontalOverflow;
    if (this.shadowText.verticalOverflow != this.mainText.verticalOverflow)
      this.shadowText.verticalOverflow = this.mainText.verticalOverflow;
    base.MatchRect();
  }
}
