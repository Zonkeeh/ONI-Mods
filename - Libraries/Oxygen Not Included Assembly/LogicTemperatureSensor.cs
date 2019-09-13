// Decompiled with JetBrains decompiler
// Type: LogicTemperatureSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>((System.Action<LogicTemperatureSensor, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  public float thresholdTemperature = 280f;
  public float maxTemp = 373.15f;
  private float[] temperatures = new float[8];
  private HandleVector<int>.Handle structureTemperature;
  private int simUpdateCounter;
  [Serialize]
  public bool activateOnWarmerThan;
  public float minTemp;
  private const int NumFrameDelay = 8;
  private float averageTemp;
  private bool wasOn;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  public float StructureTemperature
  {
    get
    {
      return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicTemperatureSensor component = ((GameObject) data).GetComponent<LogicTemperatureSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.structureTemperature = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    if (this.simUpdateCounter < 8)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      if ((double) Grid.Mass[cell] <= 0.0)
        return;
      this.temperatures[this.simUpdateCounter] = Grid.Temperature[cell];
      ++this.simUpdateCounter;
    }
    else
    {
      this.simUpdateCounter = 0;
      this.averageTemp = 0.0f;
      for (int index = 0; index < 8; ++index)
        this.averageTemp += this.temperatures[index];
      this.averageTemp /= 8f;
      if (this.activateOnWarmerThan)
      {
        if (((double) this.averageTemp <= (double) this.thresholdTemperature || this.IsSwitchedOn) && ((double) this.averageTemp >= (double) this.thresholdTemperature || !this.IsSwitchedOn))
          return;
        this.Toggle();
      }
      else
      {
        if (((double) this.averageTemp <= (double) this.thresholdTemperature || !this.IsSwitchedOn) && ((double) this.averageTemp >= (double) this.thresholdTemperature || this.IsSwitchedOn))
          return;
        this.Toggle();
      }
    }
  }

  public float GetTemperature()
  {
    return this.averageTemp;
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateVisualState(false);
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !this.switchedOn ? 0 : 1);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (!this.switchedOn ? "on_pst" : "on_pre"), KAnim.PlayMode.Once, 1f, 0.0f);
    component.Queue((HashedString) (!this.switchedOn ? "off" : "on"), KAnim.PlayMode.Once, 1f, 0.0f);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }

  public float Threshold
  {
    get
    {
      return this.thresholdTemperature;
    }
    set
    {
      this.thresholdTemperature = value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateOnWarmerThan;
    }
    set
    {
      this.activateOnWarmerThan = value;
    }
  }

  public float CurrentValue
  {
    get
    {
      return this.GetTemperature();
    }
  }

  public float RangeMin
  {
    get
    {
      return this.minTemp;
    }
  }

  public float RangeMax
  {
    get
    {
      return this.maxTemp;
    }
  }

  public float GetRangeMinInputField()
  {
    return GameUtil.GetConvertedTemperature(this.RangeMin, false);
  }

  public float GetRangeMaxInputField()
  {
    return GameUtil.GetConvertedTemperature(this.RangeMax, false);
  }

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
    }
  }

  public LocString ThresholdValueName
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
    }
  }

  public string AboveToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
    }
  }

  public string BelowToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
    }
  }

  public string Format(float value, bool units)
  {
    return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
  }

  public float ProcessedSliderValue(float input)
  {
    return Mathf.Round(input);
  }

  public float ProcessedInputValue(float input)
  {
    return GameUtil.GetTemperatureConvertedToKelvin(input);
  }

  public LocString ThresholdValueUnits()
  {
    LocString locString = (LocString) null;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        locString = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        locString = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        locString = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
        break;
    }
    return locString;
  }

  public ThresholdScreenLayoutType LayoutType
  {
    get
    {
      return ThresholdScreenLayoutType.SliderBar;
    }
  }

  public int IncrementScale
  {
    get
    {
      return 1;
    }
  }

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return new NonLinearSlider.Range[4]
      {
        new NonLinearSlider.Range(25f, 260f),
        new NonLinearSlider.Range(50f, 400f),
        new NonLinearSlider.Range(12f, 1500f),
        new NonLinearSlider.Range(13f, 10000f)
      };
    }
  }
}
