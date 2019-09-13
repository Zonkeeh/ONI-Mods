// Decompiled with JetBrains decompiler
// Type: GenericUIProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class GenericUIProgressBar : KMonoBehaviour
{
  public Image fill;
  public LocText label;
  private float maxValue;

  public void SetMaxValue(float max)
  {
    this.maxValue = max;
  }

  public void SetFillPercentage(float value)
  {
    this.fill.fillAmount = value;
    this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
  }
}
