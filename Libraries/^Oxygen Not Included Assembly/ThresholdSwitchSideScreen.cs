// Decompiled with JetBrains decompiler
// Type: ThresholdSwitchSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class ThresholdSwitchSideScreen : SideScreenContent, IRender200ms
{
  private GameObject target;
  private IThresholdSwitch thresholdSwitch;
  [SerializeField]
  private LocText currentValue;
  [SerializeField]
  private LocText tresholdValue;
  [SerializeField]
  private KToggle aboveToggle;
  [SerializeField]
  private KToggle belowToggle;
  [Header("Slider")]
  [SerializeField]
  private NonLinearSlider thresholdSlider;
  [Header("Number Input")]
  [SerializeField]
  private KNumberInputField numberInput;
  [SerializeField]
  private LocText unitsLabel;
  [Header("Increment Buttons")]
  [SerializeField]
  private GameObject incrementMinor;
  [SerializeField]
  private GameObject incrementMajor;
  [SerializeField]
  private GameObject decrementMinor;
  [SerializeField]
  private GameObject decrementMajor;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.aboveToggle.onClick += (System.Action) (() => this.OnConditionButtonClicked(true));
    this.belowToggle.onClick += (System.Action) (() => this.OnConditionButtonClicked(false));
    LocText component1 = this.aboveToggle.transform.GetChild(0).GetComponent<LocText>();
    LocText component2 = this.belowToggle.transform.GetChild(0).GetComponent<LocText>();
    component1.SetText((string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.ABOVE_BUTTON);
    component2.SetText((string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BELOW_BUTTON);
    this.thresholdSlider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value))));
    this.thresholdSlider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value))));
    this.thresholdSlider.onMove += (System.Action) (() => this.ReceiveValueFromSlider(this.thresholdSlider.GetValueForPercentage(GameUtil.GetRoundedTemperatureInKelvin(this.thresholdSlider.value))));
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.numberInput.currentValue));
    this.numberInput.decimalPlaces = 1;
  }

  public void Render200ms(float dt)
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      this.target = (GameObject) null;
    else
      this.UpdateLabels();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IThresholdSwitch>() != null;
  }

  public override void SetTarget(GameObject new_target)
  {
    this.target = (GameObject) null;
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target;
      this.thresholdSwitch = this.target.GetComponent<IThresholdSwitch>();
      if (this.thresholdSwitch == null)
      {
        this.target = (GameObject) null;
        Debug.LogError((object) "The gameObject received does not contain a IThresholdSwitch component");
      }
      else
      {
        this.UpdateLabels();
        if (this.target.GetComponent<IThresholdSwitch>().LayoutType == ThresholdScreenLayoutType.SliderBar)
        {
          this.thresholdSlider.gameObject.SetActive(true);
          this.thresholdSlider.minValue = 0.0f;
          this.thresholdSlider.maxValue = 100f;
          this.thresholdSlider.SetRanges(this.thresholdSwitch.GetRanges);
          this.thresholdSlider.value = this.thresholdSlider.GetPercentageFromValue(this.thresholdSwitch.Threshold);
          this.thresholdSlider.GetComponentInChildren<ToolTip>();
        }
        else
          this.thresholdSlider.gameObject.SetActive(false);
        MultiToggle incrementMinorToggle = this.incrementMinor.GetComponent<MultiToggle>();
        incrementMinorToggle.onClick = (System.Action) (() =>
        {
          this.UpdateThresholdValue(this.thresholdSwitch.Threshold + (float) this.thresholdSwitch.IncrementScale);
          incrementMinorToggle.ChangeState(1);
        });
        incrementMinorToggle.onStopHold = (System.Action) (() => incrementMinorToggle.ChangeState(0));
        MultiToggle incrementMajorToggle = this.incrementMajor.GetComponent<MultiToggle>();
        incrementMajorToggle.onClick = (System.Action) (() =>
        {
          this.UpdateThresholdValue(this.thresholdSwitch.Threshold + 10f * (float) this.thresholdSwitch.IncrementScale);
          incrementMajorToggle.ChangeState(1);
        });
        incrementMajorToggle.onStopHold = (System.Action) (() => incrementMajorToggle.ChangeState(0));
        MultiToggle decrementMinorToggle = this.decrementMinor.GetComponent<MultiToggle>();
        decrementMinorToggle.onClick = (System.Action) (() =>
        {
          this.UpdateThresholdValue(this.thresholdSwitch.Threshold - (float) this.thresholdSwitch.IncrementScale);
          decrementMinorToggle.ChangeState(1);
        });
        decrementMinorToggle.onStopHold = (System.Action) (() => decrementMinorToggle.ChangeState(0));
        MultiToggle decrementMajorToggle = this.decrementMajor.GetComponent<MultiToggle>();
        decrementMajorToggle.onClick = (System.Action) (() =>
        {
          this.UpdateThresholdValue(this.thresholdSwitch.Threshold - 10f * (float) this.thresholdSwitch.IncrementScale);
          decrementMajorToggle.ChangeState(1);
        });
        decrementMajorToggle.onStopHold = (System.Action) (() => decrementMajorToggle.ChangeState(0));
        this.unitsLabel.text = (string) this.thresholdSwitch.ThresholdValueUnits();
        this.numberInput.minValue = this.thresholdSwitch.GetRangeMinInputField();
        this.numberInput.maxValue = this.thresholdSwitch.GetRangeMaxInputField();
        this.numberInput.Activate();
        this.UpdateTargetThresholdLabel();
        this.OnConditionButtonClicked(this.thresholdSwitch.ActivateAboveThreshold);
      }
    }
  }

  private void OnThresholdValueChanged(float new_value)
  {
    this.thresholdSwitch.Threshold = new_value;
    this.UpdateTargetThresholdLabel();
  }

  private void OnConditionButtonClicked(bool activate_above_threshold)
  {
    this.thresholdSwitch.ActivateAboveThreshold = activate_above_threshold;
    if (activate_above_threshold)
    {
      this.belowToggle.isOn = true;
      this.aboveToggle.isOn = false;
      this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
      this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
    }
    else
    {
      this.belowToggle.isOn = false;
      this.aboveToggle.isOn = true;
      this.belowToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
      this.aboveToggle.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
    }
    this.UpdateTargetThresholdLabel();
  }

  private void UpdateTargetThresholdLabel()
  {
    this.numberInput.SetDisplayValue(this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, false));
    if (this.thresholdSwitch.ActivateAboveThreshold)
    {
      this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.AboveToolTip, (object) this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
      this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0.0f, 25f);
    }
    else
    {
      this.thresholdSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.thresholdSwitch.BelowToolTip, (object) this.thresholdSwitch.Format(this.thresholdSwitch.Threshold, true)));
      this.thresholdSlider.GetComponentInChildren<ToolTip>().tooltipPositionOffset = new Vector2(0.0f, 25f);
    }
  }

  private void ReceiveValueFromSlider(float newValue)
  {
    this.UpdateThresholdValue(this.thresholdSwitch.ProcessedSliderValue(newValue));
  }

  private void ReceiveValueFromInput(float newValue)
  {
    this.UpdateThresholdValue(this.thresholdSwitch.ProcessedInputValue(newValue));
  }

  private void UpdateThresholdValue(float newValue)
  {
    if ((double) newValue < (double) this.thresholdSwitch.RangeMin)
      newValue = this.thresholdSwitch.RangeMin;
    if ((double) newValue > (double) this.thresholdSwitch.RangeMax)
      newValue = this.thresholdSwitch.RangeMax;
    this.thresholdSwitch.Threshold = newValue;
    NonLinearSlider thresholdSlider = this.thresholdSlider;
    if ((UnityEngine.Object) thresholdSlider != (UnityEngine.Object) null)
      this.thresholdSlider.value = thresholdSlider.GetPercentageFromValue(newValue);
    else
      this.thresholdSlider.value = newValue;
    this.UpdateTargetThresholdLabel();
  }

  private void UpdateLabels()
  {
    this.currentValue.text = string.Format((string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CURRENT_VALUE, (object) this.thresholdSwitch.ThresholdValueName, (object) this.thresholdSwitch.Format(this.thresholdSwitch.CurrentValue, true));
  }

  public override string GetTitle()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      return (string) this.thresholdSwitch.Title;
    return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
  }
}
