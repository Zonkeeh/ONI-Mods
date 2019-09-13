// Decompiled with JetBrains decompiler
// Type: InfoScreenLineItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class InfoScreenLineItem : KMonoBehaviour
{
  [SerializeField]
  private LocText locText;
  [SerializeField]
  private ToolTip toolTip;
  private string text;
  private string tooltip;

  public void SetText(string text)
  {
    this.locText.text = text;
  }

  public void SetTooltip(string tooltip)
  {
    this.toolTip.toolTip = tooltip;
  }
}
