// Decompiled with JetBrains decompiler
// Type: SliderSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SliderSet
{
  public KSlider valueSlider;
  public KNumberInputField numberInput;
  public LocText unitsLabel;
  public LocText minLabel;
  public LocText maxLabel;
  [NonSerialized]
  public int index;
  private ISliderControl target;

  public void SetupSlider(int index)
  {
    this.index = index;
    this.valueSlider.onReleaseHandle += (System.Action) (() =>
    {
      this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
      this.ReceiveValueFromSlider();
    });
    this.valueSlider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider());
    this.valueSlider.onMove += (System.Action) (() => this.ReceiveValueFromSlider());
    this.valueSlider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider());
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput());
  }

  public void SetTarget(ISliderControl target)
  {
    this.target = target;
    ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetSimpleTooltip(target.GetSliderTooltip());
    this.unitsLabel.text = target.SliderUnits;
    this.minLabel.text = ((double) target.GetSliderMin(this.index)).ToString() + target.SliderUnits;
    this.maxLabel.text = ((double) target.GetSliderMax(this.index)).ToString() + target.SliderUnits;
    this.numberInput.minValue = target.GetSliderMin(this.index);
    this.numberInput.maxValue = target.GetSliderMax(this.index);
    this.numberInput.decimalPlaces = target.SliderDecimalPlaces(this.index);
    this.valueSlider.minValue = target.GetSliderMin(this.index);
    this.valueSlider.maxValue = target.GetSliderMax(this.index);
    this.valueSlider.value = target.GetSliderValue(this.index);
    this.SetValue(target.GetSliderValue(this.index));
    if (this.index != 0)
      return;
    this.numberInput.Activate();
  }

  private void ReceiveValueFromSlider()
  {
    float num1 = this.valueSlider.value;
    if (this.numberInput.decimalPlaces != -1)
    {
      float num2 = Mathf.Pow(10f, (float) this.numberInput.decimalPlaces);
      num1 = Mathf.Round(num1 * num2) / num2;
    }
    this.SetValue(num1);
  }

  private void ReceiveValueFromInput()
  {
    float num1 = this.numberInput.currentValue;
    if (this.numberInput.decimalPlaces != -1)
    {
      float num2 = Mathf.Pow(10f, (float) this.numberInput.decimalPlaces);
      num1 = Mathf.Round(num1 * num2) / num2;
    }
    this.valueSlider.value = num1;
    this.SetValue(num1);
  }

  private void SetValue(float value)
  {
    float percent = value;
    if ((double) percent > (double) this.target.GetSliderMax(this.index))
      percent = this.target.GetSliderMax(this.index);
    else if ((double) percent < (double) this.target.GetSliderMin(this.index))
      percent = this.target.GetSliderMin(this.index);
    this.UpdateLabel(percent);
    this.target.SetSliderValue(percent, this.index);
    ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetSimpleTooltip(this.target.GetSliderTooltip());
  }

  private void UpdateLabel(float value)
  {
    this.numberInput.SetDisplayValue((Mathf.Round(value * 10f) / 10f).ToString());
  }
}
