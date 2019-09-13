// Decompiled with JetBrains decompiler
// Type: ReportScreenHeaderRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ReportScreenHeaderRow : KMonoBehaviour
{
  private float nameWidth = 164f;
  [SerializeField]
  public LocText name;
  [SerializeField]
  private LayoutElement spacer;
  [SerializeField]
  private Image bgImage;
  public float groupSpacerWidth;
  [SerializeField]
  private Color oddRowColor;

  public void SetLine(ReportManager.ReportGroup reportGroup)
  {
    LayoutElement component = this.name.GetComponent<LayoutElement>();
    LayoutElement layoutElement = component;
    float nameWidth = this.nameWidth;
    component.preferredWidth = nameWidth;
    double num = (double) nameWidth;
    layoutElement.minWidth = (float) num;
    this.spacer.minWidth = this.groupSpacerWidth;
    this.name.text = reportGroup.stringKey;
  }
}
