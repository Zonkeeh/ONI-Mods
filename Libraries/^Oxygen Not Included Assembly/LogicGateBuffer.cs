// Decompiled with JetBrains decompiler
// Type: LogicGateBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
  private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>((System.Action<LogicGateBuffer, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private float delayAmount = 5f;
  [Serialize]
  private bool input_was_previously_positive;
  [Serialize]
  private int delayTicksRemaining;
  private MeterController meter;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  public float DelayAmount
  {
    get
    {
      return this.delayAmount;
    }
    set
    {
      this.delayAmount = value;
      int delayAmountTicks = this.DelayAmountTicks;
      if (this.delayTicksRemaining <= delayAmountTicks)
        return;
      this.delayTicksRemaining = delayAmountTicks;
    }
  }

  private int DelayAmountTicks
  {
    get
    {
      return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
    }
  }

  public string SliderTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
    }
  }

  public string SliderUnits
  {
    get
    {
      return (string) UI.UNITSUFFIXES.SECOND;
    }
  }

  public int SliderDecimalPlaces(int index)
  {
    return 1;
  }

  public float GetSliderMin(int index)
  {
    return 0.1f;
  }

  public float GetSliderMax(int index)
  {
    return 200f;
  }

  public float GetSliderValue(int index)
  {
    return this.DelayAmount;
  }

  public void SetSliderValue(float value, int index)
  {
    this.DelayAmount = value;
  }

  public string GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
  }

  string ISliderControl.GetSliderTooltip()
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), (object) this.DelayAmount);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicGateBuffer component = ((GameObject) data).GetComponent<LogicGateBuffer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.DelayAmount = component.DelayAmount;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, (string[]) null);
    this.meter.SetPositionPercent(1f);
  }

  private void Update()
  {
    this.meter.SetPositionPercent(!this.input_was_previously_positive ? (this.delayTicksRemaining <= 0 ? 1f : (float) (this.DelayAmountTicks - this.delayTicksRemaining) / (float) this.DelayAmountTicks) : 0.0f);
  }

  public override void LogicTick()
  {
    if (this.input_was_previously_positive || this.delayTicksRemaining <= 0)
      return;
    --this.delayTicksRemaining;
    if (this.delayTicksRemaining > 0)
      return;
    this.OnDelay();
  }

  protected override int GetCustomValue(int val1, int val2)
  {
    if (val1 != 0)
    {
      this.input_was_previously_positive = true;
      this.delayTicksRemaining = 0;
      this.meter.SetPositionPercent(0.0f);
    }
    else if (this.delayTicksRemaining <= 0)
    {
      if (this.input_was_previously_positive)
        this.delayTicksRemaining = this.DelayAmountTicks;
      this.input_was_previously_positive = false;
    }
    return val1 != 0 || this.delayTicksRemaining > 0 ? 1 : 0;
  }

  private void OnDelay()
  {
    if (this.cleaningUp)
      return;
    this.delayTicksRemaining = 0;
    this.meter.SetPositionPercent(1f);
    if (this.outputValue == 0 || !(Game.Instance.logicCircuitSystem.GetNetworkForCell(this.OutputCell) is LogicCircuitNetwork))
      return;
    this.outputValue = 0;
    this.RefreshAnimation();
  }
}
