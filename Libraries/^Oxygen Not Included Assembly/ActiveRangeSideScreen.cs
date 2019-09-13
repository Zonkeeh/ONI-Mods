// Decompiled with JetBrains decompiler
// Type: ActiveRangeSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;
using UnityEngine.Events;

public class ActiveRangeSideScreen : SideScreenContent
{
  private IActivationRangeTarget target;
  [SerializeField]
  private KSlider activateValueSlider;
  [SerializeField]
  private KSlider deactivateValueSlider;
  [SerializeField]
  private LocText activateLabel;
  [SerializeField]
  private LocText deactivateLabel;
  [Header("Number Input")]
  [SerializeField]
  private KNumberInputField activateValueLabel;
  [SerializeField]
  private KNumberInputField deactivateValueLabel;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.activateValueLabel.maxValue = this.target.MaxValue;
    this.activateValueLabel.minValue = this.target.MinValue;
    this.deactivateValueLabel.maxValue = this.target.MaxValue;
    this.deactivateValueLabel.minValue = this.target.MinValue;
    this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
    this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
  }

  private void OnActivateValueChanged(float new_value)
  {
    this.target.ActivateValue = new_value;
    if ((double) this.target.ActivateValue < (double) this.target.DeactivateValue)
    {
      this.target.ActivateValue = this.target.DeactivateValue;
      this.activateValueSlider.value = this.target.ActivateValue;
    }
    this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
    this.RefreshTooltips();
  }

  private void OnDeactivateValueChanged(float new_value)
  {
    this.target.DeactivateValue = new_value;
    if ((double) this.target.DeactivateValue > (double) this.target.ActivateValue)
    {
      this.target.DeactivateValue = this.activateValueSlider.value;
      this.deactivateValueSlider.value = this.target.DeactivateValue;
    }
    this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
    this.RefreshTooltips();
  }

  private void RefreshTooltips()
  {
    this.activateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.ActivateTooltip, (object) this.activateValueSlider.value));
    this.deactivateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.DeactivateTooltip, (object) this.deactivateValueSlider.value));
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IActivationRangeTarget>() != null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IActivationRangeTarget>();
      if (this.target == null)
      {
        Debug.LogError((object) "The gameObject received does not contain a IActivationRangeTarget component");
      }
      else
      {
        this.activateLabel.text = this.target.ActivateSliderLabelText;
        this.deactivateLabel.text = this.target.DeactivateSliderLabelText;
        this.activateValueLabel.Activate();
        this.deactivateValueLabel.Activate();
        this.activateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnActivateValueChanged));
        this.activateValueSlider.minValue = this.target.MinValue;
        this.activateValueSlider.maxValue = this.target.MaxValue;
        this.activateValueSlider.value = this.target.ActivateValue;
        this.activateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
        this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
        this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
        this.activateValueLabel.onEndEdit += (System.Action) (() =>
        {
          float result = this.target.ActivateValue;
          float.TryParse(this.activateValueLabel.field.text, out result);
          this.OnActivateValueChanged(result);
          this.activateValueSlider.value = result;
        });
        this.deactivateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnDeactivateValueChanged));
        this.deactivateValueSlider.minValue = this.target.MinValue;
        this.deactivateValueSlider.maxValue = this.target.MaxValue;
        this.deactivateValueSlider.value = this.target.DeactivateValue;
        this.deactivateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
        this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
        this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
        this.deactivateValueLabel.onEndEdit += (System.Action) (() =>
        {
          float result = this.target.DeactivateValue;
          float.TryParse(this.deactivateValueLabel.field.text, out result);
          this.OnDeactivateValueChanged(result);
          this.deactivateValueSlider.value = result;
        });
        this.RefreshTooltips();
      }
    }
  }

  public override string GetTitle()
  {
    if (this.target != null)
      return this.target.ActivationRangeTitleText;
    return (string) UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
  }
}
