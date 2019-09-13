// Decompiled with JetBrains decompiler
// Type: SliderContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine.Events;

public class SliderContainer : KMonoBehaviour
{
  public bool isPercentValue = true;
  public KSlider slider;
  public LocText nameLabel;
  public LocText valueLabel;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
  }

  public void UpdateSliderLabel(float newValue)
  {
    if (this.isPercentValue)
      this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
    else
      this.valueLabel.text = newValue.ToString();
  }
}
