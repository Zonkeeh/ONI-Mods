// Decompiled with JetBrains decompiler
// Type: DisinfectThresholdDiagram
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class DisinfectThresholdDiagram : MonoBehaviour
{
  private static int MAX_VALUE = 1000000;
  private static int SLIDER_CONVERSION = 1000;
  [SerializeField]
  private KNumberInputField inputField;
  [SerializeField]
  private KSlider slider;
  [SerializeField]
  private LocText minLabel;
  [SerializeField]
  private LocText maxLabel;
  [SerializeField]
  private LocText unitsLabel;
  [SerializeField]
  private LocText thresholdPrefix;
  [SerializeField]
  private ToolTip toolTip;
  [SerializeField]
  private KToggle toggle;
  [SerializeField]
  private Image disabledImage;

  private void Start()
  {
    this.inputField.minValue = 0.0f;
    this.inputField.maxValue = (float) DisinfectThresholdDiagram.MAX_VALUE;
    this.inputField.currentValue = (float) SaveGame.Instance.minGermCountForDisinfect;
    this.inputField.SetDisplayValue(SaveGame.Instance.minGermCountForDisinfect.ToString());
    this.inputField.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.inputField.currentValue));
    this.inputField.decimalPlaces = 1;
    this.inputField.Activate();
    this.slider.minValue = 0.0f;
    this.slider.maxValue = (float) (DisinfectThresholdDiagram.MAX_VALUE / DisinfectThresholdDiagram.SLIDER_CONVERSION);
    this.slider.wholeNumbers = true;
    this.slider.value = (float) (SaveGame.Instance.minGermCountForDisinfect / DisinfectThresholdDiagram.SLIDER_CONVERSION);
    this.slider.onReleaseHandle += new System.Action(this.OnReleaseHandle);
    this.slider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.slider.value));
    this.slider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.slider.value));
    this.slider.onMove += (System.Action) (() =>
    {
      this.ReceiveValueFromSlider(this.slider.value);
      this.OnReleaseHandle();
    });
    this.unitsLabel.SetText((string) STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.UNITS);
    this.minLabel.SetText((string) STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MIN_LABEL);
    this.maxLabel.SetText((string) STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MAX_LABEL);
    this.thresholdPrefix.SetText((string) STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.THRESHOLD_PREFIX);
    this.toolTip.OnToolTip = (Func<string>) (() =>
    {
      this.toolTip.ClearMultiStringTooltip();
      if (SaveGame.Instance.enableAutoDisinfect)
        this.toolTip.AddMultiStringTooltip(STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP.ToString().Replace("{NumberOfGerms}", SaveGame.Instance.minGermCountForDisinfect.ToString()), (ScriptableObject) null);
      else
        this.toolTip.AddMultiStringTooltip(STRINGS.UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP_DISABLED.ToString(), (ScriptableObject) null);
      return string.Empty;
    });
    this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
    this.toggle.isOn = SaveGame.Instance.enableAutoDisinfect;
    this.toggle.onValueChanged += new System.Action<bool>(this.OnClickToggle);
  }

  private void OnReleaseHandle()
  {
    float num = (float) ((int) this.slider.value * DisinfectThresholdDiagram.SLIDER_CONVERSION);
    SaveGame.Instance.minGermCountForDisinfect = (int) num;
    this.inputField.SetDisplayValue(num.ToString());
  }

  private void ReceiveValueFromSlider(float new_value)
  {
    SaveGame.Instance.minGermCountForDisinfect = (int) new_value * DisinfectThresholdDiagram.SLIDER_CONVERSION;
    this.inputField.SetDisplayValue((new_value * (float) DisinfectThresholdDiagram.SLIDER_CONVERSION).ToString());
  }

  private void ReceiveValueFromInput(float new_value)
  {
    this.slider.value = new_value / (float) DisinfectThresholdDiagram.SLIDER_CONVERSION;
    SaveGame.Instance.minGermCountForDisinfect = (int) new_value;
  }

  private void OnClickToggle(bool new_value)
  {
    SaveGame.Instance.enableAutoDisinfect = new_value;
    this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
  }
}
