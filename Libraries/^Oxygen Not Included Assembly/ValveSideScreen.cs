// Decompiled with JetBrains decompiler
// Type: ValveSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ValveSideScreen : SideScreenContent
{
  private Valve targetValve;
  [Header("Slider")]
  [SerializeField]
  private KSlider flowSlider;
  [SerializeField]
  private LocText minFlowLabel;
  [SerializeField]
  private LocText maxFlowLabel;
  [Header("Input Field")]
  [SerializeField]
  private KNumberInputField numberInput;
  [SerializeField]
  private LocText unitsLabel;
  private bool isEditing;
  private float targetFlow;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitsLabel.text = GameUtil.AddTimeSliceText((string) UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
    this.flowSlider.onReleaseHandle += new System.Action(this.OnReleaseHandle);
    this.flowSlider.onDrag += (System.Action) (() => this.ReceiveValueFromSlider(this.flowSlider.value));
    this.flowSlider.onPointerDown += (System.Action) (() => this.ReceiveValueFromSlider(this.flowSlider.value));
    this.flowSlider.onMove += (System.Action) (() =>
    {
      this.ReceiveValueFromSlider(this.flowSlider.value);
      this.OnReleaseHandle();
    });
    this.numberInput.onEndEdit += (System.Action) (() => this.ReceiveValueFromInput(this.numberInput.currentValue));
    this.numberInput.decimalPlaces = 1;
  }

  public void OnReleaseHandle()
  {
    this.targetValve.ChangeFlow(this.targetFlow);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Valve>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.targetValve = target.GetComponent<Valve>();
    if ((UnityEngine.Object) this.targetValve == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object does not have a Valve component.");
    }
    else
    {
      this.flowSlider.minValue = 0.0f;
      this.flowSlider.maxValue = this.targetValve.MaxFlow;
      this.flowSlider.value = this.targetValve.DesiredFlow;
      this.minFlowLabel.text = GameUtil.GetFormattedMass(0.0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
      this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
      this.numberInput.minValue = 0.0f;
      this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
      this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0.0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
      this.numberInput.Activate();
    }
  }

  private void ReceiveValueFromSlider(float newValue)
  {
    newValue = Mathf.Round(newValue * 1000f) / 1000f;
    this.UpdateFlowValue(newValue);
  }

  private void ReceiveValueFromInput(float input)
  {
    this.UpdateFlowValue(input / 1000f);
    this.targetValve.ChangeFlow(this.targetFlow);
  }

  private void UpdateFlowValue(float newValue)
  {
    this.targetFlow = newValue;
    this.flowSlider.value = newValue;
    this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
  }

  [DebuggerHidden]
  private IEnumerator SettingDelay(float delay)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ValveSideScreen.\u003CSettingDelay\u003Ec__Iterator0()
    {
      delay = delay,
      \u0024this = this
    };
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    Debug.Log((object) "ValveSideScreen OnKeyDown");
    if (this.isEditing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }
}
