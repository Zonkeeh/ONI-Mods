// Decompiled with JetBrains decompiler
// Type: ConduitTemperatureSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
  public float rangeMax = 373.15f;
  public float rangeMin;

  public override float CurrentValue
  {
    get
    {
      return Conduit.GetFlowManager(this.conduitType).GetContents(Grid.PosToCell((KMonoBehaviour) this)).temperature;
    }
  }

  public float RangeMin
  {
    get
    {
      return this.rangeMin;
    }
  }

  public float RangeMax
  {
    get
    {
      return this.rangeMax;
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
