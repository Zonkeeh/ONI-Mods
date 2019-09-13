// Decompiled with JetBrains decompiler
// Type: TemperatureSwitchSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TemperatureSwitchSideScreen : SideScreenContent, IRender200ms
{
  private TemperatureControlledSwitch targetTemperatureSwitch;
  [SerializeField]
  private LocText currentTemperature;
  [SerializeField]
  private LocText targetTemperature;
  [SerializeField]
  private KToggle coolerToggle;
  [SerializeField]
  private KToggle warmerToggle;
  [SerializeField]
  private KSlider targetTemperatureSlider;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.coolerToggle.onClick += (System.Action) (() => this.OnConditionButtonClicked(false));
    this.warmerToggle.onClick += (System.Action) (() => this.OnConditionButtonClicked(true));
    LocText component1 = this.coolerToggle.transform.GetChild(0).GetComponent<LocText>();
    LocText component2 = this.warmerToggle.transform.GetChild(0).GetComponent<LocText>();
    component1.SetText((string) STRINGS.UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.COLDER_BUTTON);
    component2.SetText((string) STRINGS.UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.WARMER_BUTTON);
    Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
    sliderEvent.AddListener(new UnityAction<float>(this.OnTargetTemperatureChanged));
    this.targetTemperatureSlider.onValueChanged = sliderEvent;
  }

  public void Render200ms(float dt)
  {
    if ((UnityEngine.Object) this.targetTemperatureSwitch == (UnityEngine.Object) null)
      return;
    this.UpdateLabels();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<TemperatureControlledSwitch>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.targetTemperatureSwitch = target.GetComponent<TemperatureControlledSwitch>();
      if ((UnityEngine.Object) this.targetTemperatureSwitch == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The gameObject received does not contain a TimedSwitch component");
      }
      else
      {
        this.UpdateLabels();
        this.UpdateTargetTemperatureLabel();
        this.OnConditionButtonClicked(this.targetTemperatureSwitch.activateOnWarmerThan);
      }
    }
  }

  private void OnTargetTemperatureChanged(float new_value)
  {
    this.targetTemperatureSwitch.thresholdTemperature = new_value;
    this.UpdateTargetTemperatureLabel();
  }

  private void OnConditionButtonClicked(bool isWarmer)
  {
    this.targetTemperatureSwitch.activateOnWarmerThan = isWarmer;
    if (isWarmer)
    {
      this.coolerToggle.isOn = false;
      this.warmerToggle.isOn = true;
      this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
      this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
    }
    else
    {
      this.coolerToggle.isOn = true;
      this.warmerToggle.isOn = false;
      this.coolerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
      this.warmerToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
    }
  }

  private void UpdateTargetTemperatureLabel()
  {
    this.targetTemperature.text = GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.thresholdTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
  }

  private void UpdateLabels()
  {
    this.currentTemperature.text = string.Format((string) STRINGS.UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.CURRENT_TEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.targetTemperatureSwitch.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
  }
}
