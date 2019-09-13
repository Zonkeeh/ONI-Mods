// Decompiled with JetBrains decompiler
// Type: CodexText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CodexText : CodexWidget<CodexText>
{
  public CodexText()
  {
    this.style = CodexTextStyle.Body;
  }

  public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body)
  {
    this.text = text;
    this.style = style;
  }

  public string text { get; set; }

  public CodexTextStyle style { get; set; }

  public string stringKey
  {
    set
    {
      this.text = (string) Strings.Get(value);
    }
    get
    {
      return "--> " + (this.text ?? "NULL");
    }
  }

  public void ConfigureLabel(
    LocText label,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    label.gameObject.SetActive(true);
    label.AllowLinks = this.style == CodexTextStyle.Body;
    label.textStyleSetting = textStyles[this.style];
    label.text = this.text;
    label.ApplySettings();
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
