// Decompiled with JetBrains decompiler
// Type: TemperatureControlledSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
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

  public float StructureTemperature
  {
    get
    {
      return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.structureTemperature = GameComps.StructureTemperatures.GetHandle(this.gameObject);
  }

  public void Sim200ms(float dt)
  {
    if (this.simUpdateCounter < 8)
    {
      this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell((KMonoBehaviour) this)];
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
    return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
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
      return ThresholdScreenLayoutType.InputField;
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
      return NonLinearSlider.GetDefaultRange(this.RangeMax);
    }
  }
}
