// Decompiled with JetBrains decompiler
// Type: GoodnessSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class GoodnessSlider : KMonoBehaviour
{
  public Image icon;
  public Text text;
  public Slider slider;
  public Image fill;
  public Gradient gradient;
  public string[] names;

  protected override void OnSpawn()
  {
    this.Spawn();
    this.UpdateValues();
  }

  public void UpdateValues()
  {
    Text text = this.text;
    Color color1 = this.gradient.Evaluate(this.slider.value);
    this.fill.color = color1;
    Color color2 = color1;
    text.color = color2;
    for (int index = 0; index < this.gradient.colorKeys.Length; ++index)
    {
      if ((double) this.gradient.colorKeys[index].time < (double) this.slider.value)
        this.text.text = this.names[index];
      if (index == this.gradient.colorKeys.Length - 1 && (double) this.gradient.colorKeys[index - 1].time < (double) this.slider.value)
        this.text.text = this.names[index];
    }
  }
}
