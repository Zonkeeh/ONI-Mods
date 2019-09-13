// Decompiled with JetBrains decompiler
// Type: CodexWidget`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CodexWidget<SubClass> : ICodexWidget
{
  protected CodexWidget()
  {
    this.preferredWidth = -1;
    this.preferredHeight = -1;
  }

  protected CodexWidget(int preferredWidth, int preferredHeight)
  {
    this.preferredWidth = preferredWidth;
    this.preferredHeight = preferredHeight;
  }

  public int preferredWidth { get; set; }

  public int preferredHeight { get; set; }

  public abstract void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

  protected void ConfigurePreferredLayout(GameObject contentGameObject)
  {
    LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
    componentInChildren.preferredHeight = (float) this.preferredHeight;
    componentInChildren.preferredWidth = (float) this.preferredWidth;
  }
}
