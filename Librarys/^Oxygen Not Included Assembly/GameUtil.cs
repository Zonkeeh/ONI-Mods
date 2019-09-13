// Decompiled with JetBrains decompiler
// Type: GameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class GameUtil
{
  [ThreadStatic]
  public static Queue<GameUtil.FloodFillInfo> FloodFillNext = new Queue<GameUtil.FloodFillInfo>();
  [ThreadStatic]
  public static HashSet<int> FloodFillVisited = new HashSet<int>();
  public static TagSet foodTags = new TagSet(new string[10]
  {
    "BasicPlantFood",
    "MushBar",
    "ColdWheatSeed",
    "ColdWheatSeed",
    "SpiceNut",
    "PrickleFruit",
    "Meat",
    "Mushroom",
    "ColdWheat",
    GameTags.Compostable.Name
  });
  public static TagSet solidTags = new TagSet(new string[5]
  {
    "Filter",
    "Coal",
    "BasicFabric",
    "SwampLilyFlower",
    "RefinedMetal"
  });
  public static GameUtil.TemperatureUnit temperatureUnit;
  public static GameUtil.MassUnit massUnit;
  private static string[] adjectives;

  public static string GetTemperatureUnitSuffix()
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return (string) UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return (string) UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
      default:
        return (string) UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
    }
  }

  private static string AddTemperatureUnitSuffix(string text)
  {
    return text + GameUtil.GetTemperatureUnitSuffix();
  }

  public static float GetTemperatureConvertedFromKelvin(
    float temperature,
    GameUtil.TemperatureUnit targetUnit)
  {
    if (targetUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature - 273.15f;
    if (targetUnit == GameUtil.TemperatureUnit.Fahrenheit)
      return (float) ((double) temperature * 1.79999995231628 - 459.670013427734);
    return temperature;
  }

  public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        float f1 = temperature - 273.15f;
        if (roundOutput)
          return Mathf.Round(f1);
        return f1;
      case GameUtil.TemperatureUnit.Fahrenheit:
        float f2 = (float) ((double) temperature * 1.79999995231628 - 459.670013427734);
        if (roundOutput)
          return Mathf.Round(f2);
        return f2;
      default:
        if (roundOutput)
          return Mathf.Round(temperature);
        return temperature;
    }
  }

  public static float GetTemperatureConvertedToKelvin(
    float temperature,
    GameUtil.TemperatureUnit fromUnit)
  {
    if (fromUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature + 273.15f;
    if (fromUnit == GameUtil.TemperatureUnit.Fahrenheit)
      return (float) (((double) temperature + 459.670013427734) * 5.0 / 9.0);
    return temperature;
  }

  public static float GetTemperatureConvertedToKelvin(float temperature)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return temperature + 273.15f;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return (float) (((double) temperature + 459.670013427734) * 5.0 / 9.0);
      default:
        return temperature;
    }
  }

  private static float GetConvertedTemperatureDelta(float kelvin_delta)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return kelvin_delta;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return kelvin_delta * 1.8f;
      case GameUtil.TemperatureUnit.Kelvin:
        return kelvin_delta;
      default:
        return kelvin_delta;
    }
  }

  public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice)
  {
    if (timeSlice == GameUtil.TimeSlice.PerCycle)
      return val * 600f;
    return val;
  }

  public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
  {
    switch (timeSlice)
    {
      case GameUtil.TimeSlice.PerSecond:
        return text + (string) UI.UNITSUFFIXES.PERSECOND;
      case GameUtil.TimeSlice.PerCycle:
        return text + (string) UI.UNITSUFFIXES.PERCYCLE;
      default:
        return text;
    }
  }

  public static string AddPositiveSign(string text, bool positive)
  {
    if (positive)
      return string.Format((string) UI.POSITIVE_FORMAT, (object) text);
    return text;
  }

  public static float AttributeSkillToAlpha(AttributeInstance attributeInstance)
  {
    return Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);
  }

  public static float AttributeSkillToAlpha(float attributeSkill)
  {
    return Mathf.Min(attributeSkill / 10f, 1f);
  }

  public static float AptitudeToAlpha(float aptitude)
  {
    return Mathf.Min(aptitude / 10f, 1f);
  }

  public static float GetThermalEnergy(PrimaryElement pe)
  {
    return pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;
  }

  public static float CalculateTemperatureChange(float shc, float mass, float kilowatts)
  {
    return kilowatts / (shc * mass);
  }

  public static void DeltaThermalEnergy(
    PrimaryElement pe,
    float kilowatts,
    float targetTemperature)
  {
    float temperatureChange = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
    float num1 = pe.Temperature + temperatureChange;
    float num2 = (double) targetTemperature <= (double) pe.Temperature ? Mathf.Clamp(num1, targetTemperature, pe.Temperature) : Mathf.Clamp(num1, pe.Temperature, targetTemperature);
    pe.Temperature = num2;
  }

  public static BindingEntry ActionToBinding(Action action)
  {
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      if (keyBinding.mAction == action)
        return keyBinding;
    }
    throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
  }

  public static string GetIdentityDescriptor(GameObject go)
  {
    if ((bool) ((UnityEngine.Object) go.GetComponent<MinionIdentity>()))
      return (string) DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
    if ((bool) ((UnityEngine.Object) go.GetComponent<CreatureBrain>()))
      return (string) DUPLICANTS.STATS.SUBJECTS.CREATURE;
    return (string) DUPLICANTS.STATS.SUBJECTS.PLANT;
  }

  public static float GetEnergyInPrimaryElement(PrimaryElement element)
  {
    return (float) (1.0 / 1000.0 * ((double) element.Temperature * ((double) element.Mass * 1000.0 * (double) element.Element.specificHeatCapacity)));
  }

  public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
  {
    Debug.Assert((double) element.Mass > 0.0);
    float num = Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
    float temperature = element.Temperature;
    return num / (float) (1.0 / 1000.0 * ((double) element.Mass * ((double) element.Element.specificHeatCapacity * 1000.0))) - temperature;
  }

  public static float CalculateEnergyDeltaForElement(
    PrimaryElement element,
    float startTemp,
    float endTemp)
  {
    return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
  }

  public static float CalculateEnergyDeltaForElementChange(
    float mass,
    float shc,
    float startTemp,
    float endTemp)
  {
    return (endTemp - startTemp) * mass * shc;
  }

  public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
  {
    float num1 = m1 + m2;
    float num2 = (float) ((double) t1 * (double) m1 + (double) t2 * (double) m2) / num1;
    float min = Mathf.Min(t1, t2);
    float max = Mathf.Max(t1, t2);
    float f = Mathf.Clamp(num2, min, max);
    if (float.IsNaN(f) || float.IsInfinity(f))
      Debug.LogError((object) string.Format("Calculated an invalid temperature: t1={0}, m1={1}, t2={2}, m2={3}, min_temp={4}, max_temp={5}", (object) t1, (object) m1, (object) t2, (object) m2, (object) min, (object) max));
    return f;
  }

  public static void ForceTotalConduction(PrimaryElement a, PrimaryElement b)
  {
    float num1 = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
    float temperature1 = a.Temperature;
    float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
    float temperature2 = b.Temperature;
    float num3 = num2 / (num1 + num2);
    a.Temperature = (temperature2 - temperature1) * num3 + temperature1;
    b.Temperature = (float) (((double) temperature1 - (double) temperature2) * 1.0) - num3 + temperature2;
  }

  public static string FloatToString(float f, string format = null)
  {
    if (float.IsPositiveInfinity(f))
      return UI.POS_INFINITY;
    if (float.IsNegativeInfinity(f))
      return UI.NEG_INFINITY;
    return f.ToString(format);
  }

  public static string GetUnitFormattedName(GameObject go, bool upperName = false)
  {
    KPrefabID component1 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && Assets.IsTagCountable(component1.PrefabTag))
    {
      PrimaryElement component2 = go.GetComponent<PrimaryElement>();
      return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
    }
    if (upperName)
      return StringFormatter.ToUpper(go.GetProperName());
    return go.GetProperName();
  }

  public static string GetUnitFormattedName(string name, float count, bool upperName = false)
  {
    if (upperName)
      name = name.ToUpper();
    return StringFormatter.Replace((string) UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", string.Format("{0:0.##}", (object) count));
  }

  public static string GetFormattedUnits(
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displaySuffix = true)
  {
    string units1 = (string) UI.UNITSUFFIXES.UNITS;
    units = GameUtil.ApplyTimeSlice(units, timeSlice);
    string empty = string.Empty;
    string text = (double) units != 0.0 ? ((double) Mathf.Abs(units) >= 1.0 ? ((double) Mathf.Abs(units) >= 10.0 ? GameUtil.FloatToString(units, "#,###") : GameUtil.FloatToString(units, "#,###.#")) : GameUtil.FloatToString(units, "#,##0.#")) : "0";
    if (displaySuffix)
      text += units1;
    return GameUtil.AddTimeSliceText(text, timeSlice);
  }

  public static string ApplyBoldString(string source)
  {
    return "<b>" + source + "</b>";
  }

  public static float GetRoundedTemperatureInKelvin(float kelvin)
  {
    float num = 0.0f;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        num = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        num = GameUtil.GetTemperatureConvertedToKelvin((float) Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        num = (float) Mathf.RoundToInt(kelvin);
        break;
    }
    return num;
  }

  public static string GetFormattedTemperature(
    float temp,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute,
    bool displayUnits = true,
    bool roundInDestinationFormat = false)
  {
    if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
    {
      if (interpretation == GameUtil.TemperatureInterpretation.Relative)
        ;
      temp = GameUtil.GetConvertedTemperatureDelta(temp);
    }
    else
      temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
    temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
    string empty = string.Empty;
    string text = (double) Mathf.Abs(temp) >= 0.100000001490116 ? GameUtil.FloatToString(temp, "##0.#") : GameUtil.FloatToString(temp, "##0.####");
    if (displayUnits)
      text = GameUtil.AddTemperatureUnitSuffix(text);
    return GameUtil.AddTimeSliceText(text, timeSlice);
  }

  public static string GetFormattedCaloriesForItem(
    Tag tag,
    float amount,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    EdiblesManager.FoodInfo foodInfo = Game.Instance.ediblesManager.GetFoodInfo(tag.Name);
    return GameUtil.GetFormattedCalories(foodInfo == null ? -1f : foodInfo.CaloriesPerUnit * amount, timeSlice, forceKcal);
  }

  public static string GetFormattedCalories(
    float calories,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    string str = (string) UI.UNITSUFFIXES.CALORIES.CALORIE;
    if ((double) Mathf.Abs(calories) >= 1000.0 || forceKcal)
    {
      calories /= 1000f;
      str = (string) UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
    }
    calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
    string empty = string.Empty;
    return GameUtil.AddTimeSliceText((double) calories != 0.0 ? ((double) Mathf.Abs(calories) >= 1.0 ? ((double) Mathf.Abs(calories) >= 10.0 ? GameUtil.FloatToString(calories, "#,###") + str : GameUtil.FloatToString(calories, "#,###.#") + str) : GameUtil.FloatToString(calories, "#,##0.#") + str) : "0" + str, timeSlice);
  }

  public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
    string empty = string.Empty;
    string format = (double) Mathf.Abs(percent) != 0.0 ? ((double) Mathf.Abs(percent) >= 0.100000001490116 ? ((double) Mathf.Abs(percent) >= 1.0 ? "##0" : "##0.#") : "##0.##") : "0";
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(percent, format) + (string) UI.UNITSUFFIXES.PERCENT + " " + (string) UI.UNITSUFFIXES.GROWTH, timeSlice);
  }

  public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
    string empty = string.Empty;
    string format = (double) Mathf.Abs(percent) != 0.0 ? ((double) Mathf.Abs(percent) >= 0.100000001490116 ? ((double) Mathf.Abs(percent) >= 1.0 ? "##0" : "##0.#") : "##0.##") : "0";
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(percent, format) + (string) UI.UNITSUFFIXES.PERCENT, timeSlice);
  }

  public static string GetFormattedRoundedJoules(float joules)
  {
    if ((double) Mathf.Abs(joules) > 1000.0)
      return GameUtil.FloatToString(joules / 1000f, "F1") + (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
    return GameUtil.FloatToString(joules, "F1") + (string) UI.UNITSUFFIXES.ELECTRICAL.JOULE;
  }

  public static string GetFormattedJoules(
    float joules,
    string floatFormat = "F1",
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
    return GameUtil.AddTimeSliceText((double) Math.Abs(joules) <= 1000000.0 ? ((double) Mathf.Abs(joules) <= 1000.0 ? GameUtil.FloatToString(joules, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.JOULE : GameUtil.FloatToString(joules / 1000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE) : GameUtil.FloatToString(joules / 1000000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE, timeSlice);
  }

  public static string GetFormattedWattage(float watts, GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic)
  {
    LocString locString = (LocString) string.Empty;
    switch (unit)
    {
      case GameUtil.WattageFormatterUnit.Watts:
        locString = UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
      case GameUtil.WattageFormatterUnit.Kilowatts:
        watts /= 1000f;
        locString = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
        break;
      case GameUtil.WattageFormatterUnit.Automatic:
        if ((double) Mathf.Abs(watts) > 1000.0)
        {
          watts /= 1000f;
          locString = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
          break;
        }
        locString = UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
    }
    return GameUtil.FloatToString(watts, "###0.##") + (string) locString;
  }

  public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    LocString empty = (LocString) string.Empty;
    LocString locString;
    string format;
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        locString = UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu /= 1000f;
        locString = UI.UNITSUFFIXES.HEAT.KDTU;
        format = "###0.##";
        break;
      default:
        if ((double) Mathf.Abs(dtu) > 1000.0)
        {
          dtu /= 1000f;
          locString = UI.UNITSUFFIXES.HEAT.KDTU;
          format = "###0.##";
          break;
        }
        locString = UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
    }
    return GameUtil.FloatToString(dtu, format) + (string) locString;
  }

  public static string GetFormattedHeatEnergyRate(
    float dtu_s,
    GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    LocString locString = (LocString) string.Empty;
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        locString = UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu_s /= 1000f;
        locString = UI.UNITSUFFIXES.HEAT.KDTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.Automatic:
        if ((double) Mathf.Abs(dtu_s) > 1000.0)
        {
          dtu_s /= 1000f;
          locString = UI.UNITSUFFIXES.HEAT.KDTU_S;
          break;
        }
        locString = UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
    }
    return GameUtil.FloatToString(dtu_s, "###0.##") + (string) locString;
  }

  public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
  }

  public static string GetFormattedSimple(
    float num,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    string formatString = null)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    string empty = string.Empty;
    return GameUtil.AddTimeSliceText(formatString == null ? ((double) num != 0.0 ? ((double) Mathf.Abs(num) >= 1.0 ? ((double) Mathf.Abs(num) >= 10.0 ? GameUtil.FloatToString(num, "#,###.##") : GameUtil.FloatToString(num, "#,###.##")) : GameUtil.FloatToString(num, "#,##0.##")) : "0") : GameUtil.FloatToString(num, formatString), timeSlice);
  }

  public static string GetLightDescription(int lux)
  {
    if (lux == 0)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
    if (lux < 100)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
    if (lux < 1000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
    if (lux < 10000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
    if (lux < 50000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
    if (lux < 100000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT;
    return (string) UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
  }

  public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    if (GameTags.DisplayAsCalories.Contains(tag))
      return GameUtil.GetFormattedCaloriesForItem(tag, amount, timeSlice, true);
    if (GameTags.DisplayAsUnits.Contains(tag))
      return GameUtil.GetFormattedUnits(amount, timeSlice, true);
    return GameUtil.GetFormattedMass(amount, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
  }

  public static string GetFormattedFoodQuality(int quality)
  {
    if (GameUtil.adjectives == null)
      GameUtil.adjectives = LocString.GetStrings(typeof (DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
    LocString locString = quality < 0 ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE;
    int index = Mathf.Clamp(quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET, 0, GameUtil.adjectives.Length);
    return string.Format((string) locString, (object) GameUtil.adjectives[index], (object) GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
  }

  public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
    string str = string.Empty;
    if ((double) amount < 1024.0)
      str = (string) UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
    else if ((double) amount < 1048576.0)
    {
      amount /= 1000f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
    }
    else if ((double) amount < 1073741824.0)
    {
      amount /= 1048576f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
    }
    return GameUtil.AddTimeSliceText(((double) amount).ToString() + str, timeSlice);
  }

  public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
  {
    LocString locString = (LocString) null;
    switch (GameUtil.massUnit)
    {
      case GameUtil.MassUnit.Kilograms:
        locString = !useSmallUnit ? UI.UNITSUFFIXES.MASS.KILOGRAM : UI.UNITSUFFIXES.MASS.GRAM;
        break;
      case GameUtil.MassUnit.Pounds:
        locString = UI.UNITSUFFIXES.MASS.POUND;
        break;
    }
    return locString;
  }

  public static string GetFormattedMass(
    float mass,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold,
    bool includeSuffix = true,
    string floatFormat = "{0:0.#}")
  {
    if ((double) mass == -3.40282346638529E+38)
      return (string) UI.CALCULATING;
    mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
    string str;
    if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
    {
      str = (string) UI.UNITSUFFIXES.MASS.TONNE;
      switch (massFormat)
      {
        case GameUtil.MetricMassFormat.UseThreshold:
          float num = Mathf.Abs(mass);
          if (0.0 < (double) num)
          {
            if ((double) num < 4.99999987368938E-06)
            {
              str = (string) UI.UNITSUFFIXES.MASS.MICROGRAM;
              mass = Mathf.Floor(mass * 1E+09f);
              break;
            }
            if ((double) num < 0.00499999988824129)
            {
              mass *= 1000000f;
              str = (string) UI.UNITSUFFIXES.MASS.MILLIGRAM;
              break;
            }
            if ((double) Mathf.Abs(mass) < 5.0)
            {
              mass *= 1000f;
              str = (string) UI.UNITSUFFIXES.MASS.GRAM;
              break;
            }
            if ((double) Mathf.Abs(mass) < 5000.0)
            {
              str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
              break;
            }
            mass /= 1000f;
            str = (string) UI.UNITSUFFIXES.MASS.TONNE;
            break;
          }
          str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
          break;
        case GameUtil.MetricMassFormat.Kilogram:
          str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
          break;
        case GameUtil.MetricMassFormat.Gram:
          mass *= 1000f;
          str = (string) UI.UNITSUFFIXES.MASS.GRAM;
          break;
        case GameUtil.MetricMassFormat.Tonne:
          mass /= 1000f;
          str = (string) UI.UNITSUFFIXES.MASS.TONNE;
          break;
      }
    }
    else
    {
      mass /= 2.2f;
      str = (string) UI.UNITSUFFIXES.MASS.POUND;
      if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
      {
        float num = Mathf.Abs(mass);
        if ((double) num < 5.0 && (double) num > 1.0 / 1000.0)
        {
          mass *= 256f;
          str = (string) UI.UNITSUFFIXES.MASS.DRACHMA;
        }
        else
        {
          mass *= 7000f;
          str = (string) UI.UNITSUFFIXES.MASS.GRAIN;
        }
      }
    }
    if (!includeSuffix)
    {
      str = string.Empty;
      timeSlice = GameUtil.TimeSlice.None;
    }
    return GameUtil.AddTimeSliceText(string.Format(floatFormat, (object) mass) + str, timeSlice);
  }

  public static string GetFormattedTime(float seconds)
  {
    return string.Format((string) UI.FORMATSECONDS, (object) seconds.ToString("F0"));
  }

  public static string GetFormattedEngineEfficiency(float amount)
  {
    return ((double) amount).ToString() + " km /" + (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
  }

  public static string GetFormattedDistance(float meters)
  {
    if ((double) Mathf.Abs(meters) < 1.0)
    {
      string str1 = (meters * 100f).ToString();
      string str2 = str1.Substring(0, str1.LastIndexOf('.') + Mathf.Min(3, str1.Length - str1.LastIndexOf('.')));
      if (str2 == "-0.0")
        str2 = "0";
      return str2 + " cm";
    }
    if ((double) meters < 1000.0)
      return ((double) meters).ToString() + " m";
    return Util.FormatOneDecimalPlace(meters / 1000f) + " km";
  }

  public static string GetFormattedCycles(float seconds, string formatString = "F1")
  {
    if ((double) Mathf.Abs(seconds) > 100.0)
      return string.Format((string) UI.FORMATDAY, (object) GameUtil.FloatToString(seconds / 600f, formatString));
    return GameUtil.GetFormattedTime(seconds);
  }

  public static float GetDisplaySHC(float shc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      shc /= 1.8f;
    return shc;
  }

  public static string GetSHCSuffix()
  {
    return string.Format("(DTU/g)/{0}", (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static string GetFormattedSHC(float shc)
  {
    shc = GameUtil.GetDisplaySHC(shc);
    return string.Format("{0} (DTU/g)/{1}", (object) shc.ToString("0.000"), (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static float GetDisplayThermalConductivity(float tc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      tc /= 1.8f;
    return tc;
  }

  public static string GetThermalConductivitySuffix()
  {
    return string.Format("(DTU/(m*s))/{0}", (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static string GetFormattedThermalConductivity(float tc)
  {
    tc = GameUtil.GetDisplayThermalConductivity(tc);
    return string.Format("{0} (DTU/(m*s))/{1}", (object) tc.ToString("0.000"), (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static string GetElementNameByElementHash(SimHashes elementHash)
  {
    return ElementLoader.FindElementByHash(elementHash).tag.ProperName();
  }

  public static bool HasTrait(GameObject go, string traitName)
  {
    Traits component = go.GetComponent<Traits>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return false;
    return component.HasTrait(traitName);
  }

  public static HashSet<int> GetFloodFillCavity(int startCell, bool allowLiquid)
  {
    HashSet<int> intSet = new HashSet<int>();
    return !allowLiquid ? GameUtil.FloodCollectCells(startCell, (Func<int, bool>) (cell =>
    {
      if (!Grid.Element[cell].IsVacuum)
        return Grid.Element[cell].IsGas;
      return true;
    }), 300, (HashSet<int>) null, true) : GameUtil.FloodCollectCells(startCell, (Func<int, bool>) (cell => !Grid.Solid[cell]), 300, (HashSet<int>) null, true);
  }

  public static HashSet<int> FloodCollectCells(
    int start_cell,
    Func<int, bool> is_valid,
    int maxSize = 300,
    HashSet<int> AddInvalidCellsToSet = null,
    bool clearOversizedResults = true)
  {
    HashSet<int> cells = new HashSet<int>();
    HashSet<int> invalidCells = new HashSet<int>();
    GameUtil.probeFromCell(start_cell, is_valid, cells, invalidCells, maxSize);
    if (AddInvalidCellsToSet != null)
    {
      AddInvalidCellsToSet.UnionWith((IEnumerable<int>) invalidCells);
      if (cells.Count > maxSize)
        AddInvalidCellsToSet.UnionWith((IEnumerable<int>) cells);
    }
    if (cells.Count > maxSize && clearOversizedResults)
      cells.Clear();
    return cells;
  }

  public static HashSet<int> FloodCollectCells(
    HashSet<int> results,
    int start_cell,
    Func<int, bool> is_valid,
    int maxSize = 300,
    HashSet<int> AddInvalidCellsToSet = null,
    bool clearOversizedResults = true)
  {
    HashSet<int> invalidCells = new HashSet<int>();
    GameUtil.probeFromCell(start_cell, is_valid, results, invalidCells, maxSize);
    if (AddInvalidCellsToSet != null)
    {
      AddInvalidCellsToSet.UnionWith((IEnumerable<int>) invalidCells);
      if (results.Count > maxSize)
        AddInvalidCellsToSet.UnionWith((IEnumerable<int>) results);
    }
    if (results.Count > maxSize && clearOversizedResults)
      results.Clear();
    return results;
  }

  private static void probeFromCell(
    int start_cell,
    Func<int, bool> is_valid,
    HashSet<int> cells,
    HashSet<int> invalidCells,
    int maxSize = 300)
  {
    if (cells.Count > maxSize || !Grid.IsValidCell(start_cell) || (invalidCells.Contains(start_cell) || cells.Contains(start_cell)) || !is_valid(start_cell))
    {
      invalidCells.Add(start_cell);
    }
    else
    {
      cells.Add(start_cell);
      GameUtil.probeFromCell(Grid.CellLeft(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellRight(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellAbove(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellBelow(start_cell), is_valid, cells, invalidCells, maxSize);
    }
  }

  public static bool FloodFillCheck<ArgType>(
    Func<int, ArgType, bool> fn,
    ArgType arg,
    int start_cell,
    int max_depth,
    bool stop_at_solid,
    bool stop_at_liquid)
  {
    return GameUtil.FloodFillFind<ArgType>(fn, arg, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
  }

  public static int FloodFillFind<ArgType>(
    Func<int, ArgType, bool> fn,
    ArgType arg,
    int start_cell,
    int max_depth,
    bool stop_at_solid,
    bool stop_at_liquid)
  {
    GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
    {
      cell = start_cell,
      depth = 0
    });
    int num = -1;
    while (GameUtil.FloodFillNext.Count > 0)
    {
      GameUtil.FloodFillInfo floodFillInfo = GameUtil.FloodFillNext.Dequeue();
      if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell))
      {
        Element element = Grid.Element[floodFillInfo.cell];
        if ((!stop_at_solid || !element.IsSolid) && (!stop_at_liquid || !element.IsLiquid) && !GameUtil.FloodFillVisited.Contains(floodFillInfo.cell))
        {
          GameUtil.FloodFillVisited.Add(floodFillInfo.cell);
          if (fn(floodFillInfo.cell, arg))
          {
            num = floodFillInfo.cell;
            break;
          }
          GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellLeft(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellRight(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellAbove(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellBelow(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
        }
      }
    }
    GameUtil.FloodFillVisited.Clear();
    GameUtil.FloodFillNext.Clear();
    return num;
  }

  public static void FloodFillConditional(
    int start_cell,
    Func<int, bool> condition,
    ICollection<int> visited_cells,
    ICollection<int> valid_cells = null)
  {
    GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
    {
      cell = start_cell,
      depth = 0
    });
    GameUtil.FloodFillConditional(GameUtil.FloodFillNext, condition, visited_cells, valid_cells, 10000);
  }

  public static void FloodFillConditional(
    Queue<GameUtil.FloodFillInfo> queue,
    Func<int, bool> condition,
    ICollection<int> visited_cells,
    ICollection<int> valid_cells = null,
    int max_depth = 10000)
  {
    while (queue.Count > 0)
    {
      GameUtil.FloodFillInfo floodFillInfo = queue.Dequeue();
      if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell) && !visited_cells.Contains(floodFillInfo.cell))
      {
        visited_cells.Add(floodFillInfo.cell);
        if (condition(floodFillInfo.cell))
        {
          valid_cells?.Add(floodFillInfo.cell);
          queue.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellLeft(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          queue.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellRight(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          queue.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellAbove(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
          queue.Enqueue(new GameUtil.FloodFillInfo()
          {
            cell = Grid.CellBelow(floodFillInfo.cell),
            depth = floodFillInfo.depth + 1
          });
        }
      }
    }
    queue.Clear();
  }

  public static GameUtil.Hardness GetHardness(Element element)
  {
    if (!element.IsSolid)
      return GameUtil.Hardness.NA;
    if (element.hardness >= byte.MaxValue)
      return GameUtil.Hardness.IMPENETRABLE;
    if (element.hardness >= (byte) 150)
      return GameUtil.Hardness.NEARLY_IMPENETRABLE;
    if (element.hardness >= (byte) 50)
      return GameUtil.Hardness.VERY_FIRM;
    if (element.hardness >= (byte) 25)
      return GameUtil.Hardness.FIRM;
    return element.hardness >= (byte) 10 ? GameUtil.Hardness.SOFT : GameUtil.Hardness.NA;
  }

  public static string GetHardnessString(Element element, bool addColor = true)
  {
    if (!element.IsSolid)
      return (string) ELEMENTS.HARDNESS.NA;
    Color color1 = new Color(0.8313726f, 0.2862745f, 0.282353f);
    Color color2 = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    Color color3 = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    Color color4 = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    Color color5 = new Color(0.427451f, 0.4823529f, 0.7568628f);
    Color color6 = new Color(0.4431373f, 0.6705883f, 0.8117647f);
    Color c = color4;
    string str = string.Empty;
    switch (GameUtil.GetHardness(element))
    {
      case GameUtil.Hardness.NA:
        c = color6;
        str = string.Format((string) ELEMENTS.HARDNESS.VERYSOFT, (object) element.hardness);
        break;
      case GameUtil.Hardness.SOFT:
        c = color5;
        str = string.Format((string) ELEMENTS.HARDNESS.SOFT, (object) element.hardness);
        break;
      case GameUtil.Hardness.FIRM:
        c = color4;
        str = string.Format((string) ELEMENTS.HARDNESS.FIRM, (object) element.hardness);
        break;
      case GameUtil.Hardness.VERY_FIRM:
        c = color3;
        str = string.Format((string) ELEMENTS.HARDNESS.VERYFIRM, (object) element.hardness);
        break;
      case GameUtil.Hardness.NEARLY_IMPENETRABLE:
        c = color2;
        str = string.Format((string) ELEMENTS.HARDNESS.NEARLYIMPENETRABLE, (object) element.hardness);
        break;
      case GameUtil.Hardness.IMPENETRABLE:
        c = color1;
        str = string.Format((string) ELEMENTS.HARDNESS.IMPENETRABLE, (object) element.hardness);
        break;
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) c.ToHexString(), (object) str);
    return str;
  }

  public static GameUtil.GermResistanceModifier GetGermResistanceModifier(float modifier)
  {
    if ((double) modifier > 0.0)
    {
      if ((double) modifier >= 5.0)
        return GameUtil.GermResistanceModifier.POSITIVE_LARGE;
      if ((double) modifier >= 2.0)
        return GameUtil.GermResistanceModifier.POSITIVE_MEDIUM;
      if ((double) modifier >= 1.0)
        return GameUtil.GermResistanceModifier.POSITIVE_SMALL;
    }
    else if ((double) modifier < 0.0)
    {
      if ((double) modifier <= -5.0)
        return GameUtil.GermResistanceModifier.NEGATIVE_LARGE;
      if ((double) modifier <= -2.0)
        return GameUtil.GermResistanceModifier.NEGATIVE_MEDIUM;
      if ((double) modifier <= -1.0)
        return GameUtil.GermResistanceModifier.NEGATIVE_SMALL;
    }
    return GameUtil.GermResistanceModifier.NONE;
  }

  public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
  {
    Color color1 = new Color(0.8313726f, 0.2862745f, 0.282353f);
    Color color2 = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    Color color3 = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    Color color4 = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    Color color5 = new Color(0.427451f, 0.4823529f, 0.7568628f);
    Color color6 = new Color(0.4431373f, 0.6705883f, 0.8117647f);
    Color c = color4;
    string str = string.Empty;
    switch (GameUtil.GetGermResistanceModifier(modifier) + 5)
    {
      case GameUtil.GermResistanceModifier.NONE:
        c = color4;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, (object) modifier);
        break;
      case GameUtil.GermResistanceModifier.POSITIVE_SMALL | GameUtil.GermResistanceModifier.POSITIVE_MEDIUM:
        c = color3;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, (object) modifier);
        break;
      case ~GameUtil.GermResistanceModifier.NEGATIVE_LARGE:
        c = color2;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, (object) modifier);
        break;
      case GameUtil.GermResistanceModifier.POSITIVE_LARGE:
        c = color1;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, (object) modifier);
        break;
      case (GameUtil.GermResistanceModifier) 6:
        c = color5;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, (object) modifier);
        break;
      case GameUtil.GermResistanceModifier.POSITIVE_LARGE | GameUtil.GermResistanceModifier.POSITIVE_MEDIUM:
        c = color6;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, (object) modifier);
        break;
      case (GameUtil.GermResistanceModifier) 10:
        c = color6;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, (object) modifier);
        break;
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) c.ToHexString(), (object) str);
    return str;
  }

  public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
  {
    Color color1 = new Color(0.8313726f, 0.2862745f, 0.282353f);
    Color color2 = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    Color color3 = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    Color color4 = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    Color color5 = new Color(0.427451f, 0.4823529f, 0.7568628f);
    string empty = string.Empty;
    Color c;
    string str;
    if ((double) element.thermalConductivity >= 50.0)
    {
      c = color5;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 10.0)
    {
      c = color4;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 2.0)
    {
      c = color3;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 1.0)
    {
      c = color2;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
    }
    else
    {
      c = color1;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) c.ToHexString(), (object) str);
    if (addValue)
      str = string.Format((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, (object) element.thermalConductivity.ToString(), (object) str);
    return str;
  }

  public static string GetBreathableString(Element element, float Mass)
  {
    if (!element.IsGas && !element.IsVacuum)
      return string.Empty;
    Color color1 = new Color(0.4431373f, 0.6705883f, 0.8117647f);
    Color color2 = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    Color color3 = new Color(0.8313726f, 0.2862745f, 0.282353f);
    Color c;
    LocString locString;
    switch (element.id)
    {
      case SimHashes.Oxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = color1;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = color1;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = color2;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = color3;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      case SimHashes.ContaminatedOxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = color1;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = color1;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = color2;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = color3;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      default:
        c = color3;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
    }
    return string.Format((string) ELEMENTS.BREATHABLEDESC, (object) c.ToHexString(), (object) locString);
  }

  public static string GetWireLoadColor(float load, float maxLoad)
  {
    Color color1 = new Color(1f, 1f, 1f);
    Color color2 = new Color(0.9843137f, 0.6901961f, 0.2313726f);
    Color color3 = new Color(1f, 0.1921569f, 0.1921569f);
    return ((double) load <= (double) maxLoad ? ((double) load / (double) maxLoad < 0.75 ? color1 : color2) : color3).ToHexString();
  }

  public static string AppendHotkeyString(string template, Action action)
  {
    return template + UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]");
  }

  public static string ReplaceHotkeyString(string template, Action action)
  {
    return template.Replace("{Hotkey}", UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]"));
  }

  public static string ReplaceHotkeyString(string template, Action action1, Action action2)
  {
    return template.Replace("{Hotkey}", UI.FormatAsHotkey("[" + GameUtil.GetActionString(action1) + "]") + UI.FormatAsHotkey("[" + GameUtil.GetActionString(action2) + "]"));
  }

  public static string GetKeycodeLocalized(KKeyCode key_code)
  {
    string str = key_code.ToString();
    switch (key_code)
    {
      case KKeyCode.Plus:
        str = "+";
        break;
      case KKeyCode.Comma:
        str = ",";
        break;
      case KKeyCode.Minus:
        str = "-";
        break;
      case KKeyCode.Period:
        str = (string) INPUT.PERIOD;
        break;
      case KKeyCode.Slash:
        str = "/";
        break;
      case KKeyCode.LeftBracket:
        str = "[";
        break;
      case KKeyCode.Backslash:
        str = "\\";
        break;
      case KKeyCode.RightBracket:
        str = "]";
        break;
      case KKeyCode.BackQuote:
        str = (string) INPUT.BACKQUOTE;
        break;
      case KKeyCode.Keypad0:
        str = (string) INPUT.NUM + " 0";
        break;
      case KKeyCode.Keypad1:
        str = (string) INPUT.NUM + " 1";
        break;
      case KKeyCode.Keypad2:
        str = (string) INPUT.NUM + " 2";
        break;
      case KKeyCode.Keypad3:
        str = (string) INPUT.NUM + " 3";
        break;
      case KKeyCode.Keypad4:
        str = (string) INPUT.NUM + " 4";
        break;
      case KKeyCode.Keypad5:
        str = (string) INPUT.NUM + " 5";
        break;
      case KKeyCode.Keypad6:
        str = (string) INPUT.NUM + " 6";
        break;
      case KKeyCode.Keypad7:
        str = (string) INPUT.NUM + " 7";
        break;
      case KKeyCode.Keypad8:
        str = (string) INPUT.NUM + " 8";
        break;
      case KKeyCode.Keypad9:
        str = (string) INPUT.NUM + " 9";
        break;
      case KKeyCode.KeypadPeriod:
        str = (string) INPUT.NUM + " " + (string) INPUT.PERIOD;
        break;
      case KKeyCode.KeypadDivide:
        str = (string) INPUT.NUM + " /";
        break;
      case KKeyCode.KeypadMultiply:
        str = (string) INPUT.NUM + " *";
        break;
      case KKeyCode.KeypadMinus:
        str = (string) INPUT.NUM + " -";
        break;
      case KKeyCode.KeypadPlus:
        str = (string) INPUT.NUM + " +";
        break;
      case KKeyCode.KeypadEnter:
        str = (string) INPUT.NUM + " " + (string) INPUT.ENTER;
        break;
      case KKeyCode.RightShift:
        str = (string) INPUT.RIGHT_SHIFT;
        break;
      case KKeyCode.LeftShift:
        str = (string) INPUT.LEFT_SHIFT;
        break;
      case KKeyCode.RightControl:
        str = (string) INPUT.RIGHT_CTRL;
        break;
      case KKeyCode.LeftControl:
        str = (string) INPUT.LEFT_CTRL;
        break;
      case KKeyCode.RightAlt:
        str = (string) INPUT.RIGHT_ALT;
        break;
      case KKeyCode.LeftAlt:
        str = (string) INPUT.LEFT_ALT;
        break;
      case KKeyCode.Mouse0:
        str = (string) INPUT.MOUSE + " 0";
        break;
      case KKeyCode.Mouse1:
        str = (string) INPUT.MOUSE + " 1";
        break;
      case KKeyCode.Mouse2:
        str = (string) INPUT.MOUSE + " 2";
        break;
      case KKeyCode.Mouse3:
        str = (string) INPUT.MOUSE + " 3";
        break;
      case KKeyCode.Mouse4:
        str = (string) INPUT.MOUSE + " 4";
        break;
      case KKeyCode.Mouse5:
        str = (string) INPUT.MOUSE + " 5";
        break;
      case KKeyCode.Mouse6:
        str = (string) INPUT.MOUSE + " 6";
        break;
      default:
        switch (key_code - 8)
        {
          case KKeyCode.None:
            str = (string) INPUT.BACKSPACE;
            break;
          case (KKeyCode) 1:
            str = (string) INPUT.TAB;
            break;
          case (KKeyCode) 5:
            str = (string) INPUT.ENTER;
            break;
          default:
            switch (key_code - 58)
            {
              case KKeyCode.None:
                str = ":";
                break;
              case (KKeyCode) 1:
                str = ";";
                break;
              case (KKeyCode) 3:
                str = "=";
                break;
              default:
                if (key_code != KKeyCode.MouseScrollDown)
                {
                  if (key_code != KKeyCode.MouseScrollUp)
                  {
                    if (key_code != KKeyCode.None)
                    {
                      if (key_code != KKeyCode.Escape)
                      {
                        if (key_code == KKeyCode.Space)
                        {
                          str = (string) INPUT.SPACE;
                          break;
                        }
                        if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
                        {
                          str = ((char) (65 + (key_code - 97))).ToString();
                          break;
                        }
                        if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
                        {
                          str = ((char) (48 + (key_code - 48))).ToString();
                          break;
                        }
                        if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
                        {
                          str = "F" + ((int) (key_code - 282 + 1)).ToString();
                          break;
                        }
                        Debug.LogWarning((object) ("Unable to find proper string for KKeyCode: " + key_code.ToString() + " using key_code.ToString()"));
                        break;
                      }
                      str = (string) INPUT.ESCAPE;
                      break;
                    }
                    break;
                  }
                  str = (string) INPUT.MOUSE_SCROLL_UP;
                  break;
                }
                str = (string) INPUT.MOUSE_SCROLL_DOWN;
                break;
            }
        }
    }
    return str;
  }

  public static string GetActionString(Action action)
  {
    string empty = string.Empty;
    if (action == Action.NumActions)
      return empty;
    BindingEntry binding = GameUtil.ActionToBinding(action);
    KKeyCode mKeyCode = binding.mKeyCode;
    if (binding.mModifier == Modifier.None)
      return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
    string str = string.Empty;
    switch (binding.mModifier)
    {
      case Modifier.Alt:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftAlt).ToUpper();
        break;
      case Modifier.Ctrl:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftControl).ToUpper();
        break;
      case Modifier.Shift:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.LeftShift).ToUpper();
        break;
      case Modifier.CapsLock:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock).ToUpper();
        break;
    }
    return str + " + " + GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
  }

  public static void CreateExplosion(Vector3 explosion_pos)
  {
    Vector2 vector2 = new Vector2(explosion_pos.x, explosion_pos.y);
    float num1 = 5f;
    float num2 = num1 * num1;
    foreach (Health health in Components.Health.Items)
    {
      Vector3 position = health.transform.GetPosition();
      float sqrMagnitude = (new Vector2(position.x, position.y) - vector2).sqrMagnitude;
      if ((double) num2 >= (double) sqrMagnitude && (UnityEngine.Object) health != (UnityEngine.Object) null)
        health.Damage(health.maxHitPoints);
    }
  }

  private static void GetNonSolidCells(
    int x,
    int y,
    List<int> cells,
    int min_x,
    int min_y,
    int max_x,
    int max_y)
  {
    int cell = Grid.XYToCell(x, y);
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (Grid.DupePassable[cell] || x < min_x) || (x > max_x || y < min_y || (y > max_y || cells.Contains(cell))))
      return;
    cells.Add(cell);
    GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
  }

  public static void GetNonSolidCells(int cell, int radius, List<int> cells)
  {
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    GameUtil.GetNonSolidCells(x, y, cells, x - radius, y - radius, x + radius, y + radius);
  }

  public static float GetMaxStress()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float a = 0.0f;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      a = Mathf.Max(a, Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).value);
    return a;
  }

  public static float GetAverageStress()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float num = 0.0f;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      num += Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).value;
    return num / (float) Components.LiveMinionIdentities.Count;
  }

  public static string MigrateFMOD(FMODAsset asset)
  {
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      return (string) null;
    if (asset.path != null)
      return asset.path;
    return asset.name;
  }

  private static void SortDescriptors(List<IEffectDescriptor> descriptorList)
  {
    descriptorList.Sort((Comparison<IEffectDescriptor>) ((e1, e2) => TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType()).CompareTo(TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType()))));
  }

  private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList)
  {
    descriptorList.Sort((Comparison<IGameObjectEffectDescriptor>) ((e1, e2) => TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType()).CompareTo(TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType()))));
  }

  public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
  {
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      Descriptor descriptor = list[index1];
      for (int index2 = 0; index2 < indentCount; ++index2)
        descriptor.IncreaseIndent();
      list[index1] = descriptor;
    }
  }

  public static List<Descriptor> GetAllDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IEffectDescriptor> descriptorList2 = new List<IEffectDescriptor>((IEnumerable<IEffectDescriptor>) def.BuildingComplete.GetComponents<IEffectDescriptor>());
    GameUtil.SortDescriptors(descriptorList2);
    foreach (IEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(def);
      if (descriptors != null)
        descriptorList1.AddRange((IEnumerable<Descriptor>) descriptors);
    }
    return descriptorList1;
  }

  public static List<Descriptor> GetAllDescriptors(
    GameObject go,
    bool simpleInfoScreen = false)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalRequirements != null)
    {
      foreach (Descriptor additionalRequirement in component2.AdditionalRequirements)
      {
        if (!additionalRequirement.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalRequirement);
      }
    }
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalEffect);
      }
    }
    return descriptorList1;
  }

  public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Detail)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list, 1);
    return list;
  }

  public static List<Descriptor> GetRequirementDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Requirement)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list, 1);
    return list;
  }

  public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list, 1);
    return list;
  }

  public static List<Descriptor> GetInformationDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list, 1);
    return list;
  }

  public static List<Descriptor> GetCropOptimumConditionDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor1 in descriptors)
    {
      if (descriptor1.type == Descriptor.DescriptorType.Lifecycle)
      {
        Descriptor descriptor2 = descriptor1;
        descriptor2.text = "• " + descriptor2.text;
        list.Add(descriptor2);
      }
    }
    GameUtil.IndentListOfDescriptors(list, 1);
    return list;
  }

  public static List<Descriptor> GetGameObjectRequirements(GameObject go)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (descriptor.type == Descriptor.DescriptorType.Requirement)
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if (component2.AdditionalRequirements != null)
      descriptorList1.AddRange((IEnumerable<Descriptor>) component2.AdditionalRequirements);
    return descriptorList1;
  }

  public static List<Descriptor> GetGameObjectEffects(
    GameObject go,
    bool simpleInfoScreen = false)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalEffect);
      }
    }
    return descriptorList1;
  }

  public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go, false));
    if (requirementDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
      descriptorList.AddRange((IEnumerable<Descriptor>) requirementDescriptors);
    }
    return descriptorList;
  }

  public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go, false));
    if (informationDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
      descriptorList.Add(descriptor);
      descriptorList.AddRange((IEnumerable<Descriptor>) informationDescriptors);
    }
    return descriptorList;
  }

  public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    if ((UnityEngine.Object) go.GetComponent<Growing>() == (UnityEngine.Object) null)
      return descriptorList1;
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go, false);
    List<Descriptor> descriptorList2 = new List<Descriptor>();
    descriptorList2.AddRange((IEnumerable<Descriptor>) GameUtil.GetEffectDescriptors(allDescriptors));
    if (descriptorList2.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS, Descriptor.DescriptorType.Effect);
      descriptorList1.Add(descriptor);
      descriptorList1.AddRange((IEnumerable<Descriptor>) descriptorList2);
    }
    return descriptorList1;
  }

  public static string GetGameObjectEffectsTooltipString(GameObject go)
  {
    string str = string.Empty;
    List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go, false);
    if (gameObjectEffects.Count > 0)
      str = str + (string) UI.BUILDINGEFFECTS.OPERATIONEFFECTS + "\n";
    foreach (Descriptor descriptor in gameObjectEffects)
      str = str + descriptor.IndentedText() + "\n";
    return str;
  }

  public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
  {
    Debug.Assert((UnityEngine.Object) def != (UnityEngine.Object) null);
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
    if (attributeModifiers != null)
    {
      foreach (AttributeModifier attributeModifier in attributeModifiers)
      {
        string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString((GameObject) null);
        string newValue = (double) attributeModifier.Value < 0.0 ? "consumed" : "produced";
        string str = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
        descriptorList.Add(new Descriptor(str, str, Descriptor.DescriptorType.Effect, false));
      }
    }
    return descriptorList;
  }

  public static string GetRecipeDescription(Recipe recipe)
  {
    string str = (string) null;
    if (recipe != null)
      str = recipe.recipeDescription;
    if (str == null)
    {
      str = "MISSING RECIPEDESCRIPTION";
      Debug.LogWarning((object) "Missing recipeDescription");
    }
    return str;
  }

  public static int GetCurrentCycle()
  {
    return GameClock.Instance.GetCycle() + 1;
  }

  public static GameObject GetTelepad()
  {
    if (Components.Telepads.Count > 0)
      return Components.Telepads[0].gameObject;
    return (GameObject) null;
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, position, sceneLayer, (GameObject) null, name, gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    GameObject parent,
    string name = null,
    int gameLayer = 0)
  {
    position.z = Grid.GetLayerZ(sceneLayer);
    return Util.KInstantiate(original, position, Quaternion.identity, parent, name, true, gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static GameObject KInstantiate(
    Component original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static unsafe void IsEmissionBlocked(
    int cell,
    out bool all_not_gaseous,
    out bool all_over_pressure)
  {
    // ISSUE: untyped stack allocation
    int* numPtr = (int*) __untypedstackalloc((int) checked (4U * 4U));
    *numPtr = Grid.CellBelow(cell);
    numPtr[1] = Grid.CellLeft(cell);
    numPtr[2] = Grid.CellRight(cell);
    numPtr[3] = Grid.CellAbove(cell);
    all_not_gaseous = true;
    all_over_pressure = true;
    for (int index = 0; index < 4; ++index)
    {
      int cell1 = numPtr[index];
      if (Grid.IsValidCell(cell1))
      {
        Element element = Grid.Element[cell1];
        all_not_gaseous = all_not_gaseous && (element.IsGas ? 1 : (element.IsVacuum ? 1 : 0)) == 0;
        all_over_pressure = all_over_pressure && (!element.IsGas && !element.IsVacuum || (double) Grid.Mass[cell1] >= 1.79999995231628);
      }
    }
  }

  public static float GetDecorAtCell(int cell)
  {
    float num = 0.0f;
    if (!Grid.Solid[cell])
      num = Grid.Decor[cell] + (float) DecorProvider.GetLightDecorBonus(cell);
    return num;
  }

  public static string GetKeywordStyle(Tag tag)
  {
    Element element = ElementLoader.GetElement(tag);
    return element == null ? (!GameUtil.foodTags.Contains(tag) ? (!GameUtil.solidTags.Contains(tag) ? (string) null : "solid") : "food") : GameUtil.GetKeywordStyle(element);
  }

  public static string GetKeywordStyle(SimHashes hash)
  {
    Element elementByHash = ElementLoader.FindElementByHash(hash);
    if (elementByHash != null)
      return GameUtil.GetKeywordStyle(elementByHash);
    return (string) null;
  }

  public static string GetKeywordStyle(Element element)
  {
    if (element.id == SimHashes.Oxygen)
      return "oxygen";
    if (element.IsSolid)
      return "solid";
    if (element.IsLiquid)
      return "liquid";
    if (element.IsGas)
      return "gas";
    if (element.IsVacuum)
      return "vacuum";
    return (string) null;
  }

  public static string GetKeywordStyle(GameObject go)
  {
    string str = string.Empty;
    Edible component1 = go.GetComponent<Edible>();
    Equippable component2 = go.GetComponent<Equippable>();
    MedicinalPill component3 = go.GetComponent<MedicinalPill>();
    ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      str = "food";
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      str = "equipment";
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      str = "medicine";
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      str = "research";
    return str;
  }

  public static string GenerateRandomDuplicantName()
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    string empty = string.Empty;
    bool flag = (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5;
    List<string> tList1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.NB)));
    tList1.AddRange(!flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.FEMALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.MALE)));
    string random = tList1.GetRandom<string>();
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > 0.699999988079071)
    {
      List<string> tList2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.NB)));
      tList2.AddRange(!flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.FEMALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.MALE)));
      str1 = tList2.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str1))
      str1 += " ";
    if ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.899999976158142)
    {
      List<string> tList2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.NB)));
      tList2.AddRange(!flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.FEMALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.MALE)));
      str2 = tList2.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str2))
      str2 = " " + str2;
    return str1 + random + str2;
  }

  public static string GenerateRandomRocketName()
  {
    string empty = string.Empty;
    string newValue1 = string.Empty;
    string newValue2 = string.Empty;
    string newValue3 = string.Empty;
    int num1 = 1;
    int num2 = 2;
    int num3 = 4;
    string random = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
    int num4 = 0;
    if ((double) UnityEngine.Random.value > 0.699999988079071)
    {
      newValue1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
      num4 |= num1;
    }
    if ((double) UnityEngine.Random.value > 0.5)
    {
      newValue2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
      num4 |= num2;
    }
    if ((double) UnityEngine.Random.value > 0.100000001490116)
    {
      newValue3 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
      num4 |= num3;
    }
    string str = num4 != (num1 | num2 | num3) ? (num4 != (num2 | num3) ? (num4 != (num1 | num3) ? (num4 != num3 ? (num4 != (num1 | num2) ? (num4 != num1 ? (num4 != num2 ? (string) NAMEGEN.ROCKET.FMT_NOUN : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
    DebugUtil.LogArgs((object) "Rocket name bits:", (object) Convert.ToString(num4, 2));
    return str.Replace("{Prefix}", newValue1).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
  }

  public static float GetThermalComfort(int cell, float tolerance = -0.08368f)
  {
    float num1 = tolerance;
    float num2 = 0.0f;
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Creature);
    if ((double) Grid.Element[cell].thermalConductivity != 0.0)
      num2 = SimUtil.CalculateEnergyFlowCreatures(cell, 310.15f, elementByHash.specificHeatCapacity, elementByHash.thermalConductivity, 1f, 0.0045f);
    return (num2 - num1) * 1000f;
  }

  public static string GetFormattedDiseaseName(byte idx, bool color = false)
  {
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    if (color)
      return string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, (object) disease.Name, (object) GameUtil.ColourToHex(disease.overlayColour));
    return string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, (object) disease.Name);
  }

  public static string GetFormattedDisease(byte idx, int units, bool color = false)
  {
    if (idx == byte.MaxValue || units <= 0)
      return (string) UI.OVERLAYS.DISEASE.NO_DISEASE;
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    if (color)
      return string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units), (object) GameUtil.ColourToHex(disease.overlayColour));
    return string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units));
  }

  public static string GetFormattedDiseaseAmount(int units)
  {
    return units.ToString("#,##0") + (string) UI.UNITSUFFIXES.DISEASE.UNITS;
  }

  public static string ColourizeString(Color32 colour, string str)
  {
    return string.Format("<color=#{0}>{1}</color>", (object) GameUtil.ColourToHex(colour), (object) str);
  }

  public static string ColourToHex(Color32 colour)
  {
    return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", (object) colour.r, (object) colour.g, (object) colour.b, (object) colour.a);
  }

  public static string GetFormattedDecor(float value, bool enforce_max = false)
  {
    string str = string.Empty;
    LocString locString = (double) value <= (double) DecorMonitor.MAXIMUM_DECOR_VALUE || !enforce_max ? UI.OVERLAYS.DECOR.VALUE : UI.OVERLAYS.DECOR.MAXIMUM_DECOR;
    if (enforce_max)
      value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
    if ((double) value > 0.0)
      str = "+";
    else if ((double) value >= 0.0)
      locString = UI.OVERLAYS.DECOR.VALUE_ZERO;
    return string.Format((string) locString, (object) str, (object) value);
  }

  public static Color GetDecorColourFromValue(int decor)
  {
    Color black = Color.black;
    float f = (float) decor / 100f;
    return (double) f <= 0.0 ? Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f), Mathf.Abs(f)) : Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f), Mathf.Abs(f));
  }

  public static List<Descriptor> GetMaterialDescriptors(Element element)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (element.attributeModifiers.Count > 0)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
        string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
        descriptor.IncreaseIndent();
        descriptorList.Add(descriptor);
      }
    }
    descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    return descriptorList;
  }

  public static string GetMaterialTooltips(Element element)
  {
    string str = element.tag.ProperName();
    foreach (AttributeModifier attributeModifier in element.attributeModifiers)
    {
      string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
      string formattedString = attributeModifier.GetFormattedString((GameObject) null);
      str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
    }
    return str + GameUtil.GetSignificantMaterialPropertyTooltips(element);
  }

  public static string GetSignificantMaterialPropertyTooltips(Element element)
  {
    string str = string.Empty;
    List<Descriptor> propertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
    if (propertyDescriptors.Count > 0)
    {
      str += "\n";
      for (int index = 0; index < propertyDescriptors.Count; ++index)
        str = str + "    • " + Util.StripTextFormatting(propertyDescriptors[index].text) + "\n";
    }
    return str;
  }

  public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(
    Element element)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if ((double) element.thermalConductivity > 10.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.thermalConductivity < 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity <= 0.200000002980232)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)), Descriptor.DescriptorType.Effect);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity >= 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)), Descriptor.DescriptorType.Effect);
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public static int NaturalBuildingCell(this KMonoBehaviour cmp)
  {
    return Grid.PosToCell(cmp.transform.GetPosition());
  }

  public static List<Descriptor> GetMaterialDescriptors(Tag tag)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      if (element.attributeModifiers.Count > 0)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
        {
          string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
          string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
          Descriptor descriptor = new Descriptor();
          descriptor.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
          descriptor.IncreaseIndent();
          descriptorList.Add(descriptor);
        }
      }
      descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor1 in component.descriptors)
          {
            string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString((GameObject) null));
            string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString((GameObject) null));
            Descriptor descriptor2 = new Descriptor();
            descriptor2.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
            descriptor2.IncreaseIndent();
            descriptorList.Add(descriptor2);
          }
        }
      }
    }
    return descriptorList;
  }

  public static string GetMaterialTooltips(Tag tag)
  {
    string str = tag.ProperName();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString((GameObject) null);
        str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
      }
      str += GameUtil.GetSignificantMaterialPropertyTooltips(element);
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor in component.descriptors)
          {
            string name = Db.Get().BuildingAttributes.Get(descriptor.AttributeId).Name;
            string formattedString = descriptor.GetFormattedString((GameObject) null);
            str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
          }
        }
      }
    }
    return str;
  }

  public static bool AreChoresUIMergeable(
    Chore.Precondition.Context choreA,
    Chore.Precondition.Context choreB)
  {
    if (choreA.chore.target.isNull || choreB.chore.target.isNull)
      return false;
    ChoreType choreType1 = choreB.chore.choreType;
    ChoreType choreType2 = choreA.chore.choreType;
    return choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag || choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig || choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax || ((choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType1 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType1 == Db.Get().ChoreTypes.ReturnSuitUrgent) || (UnityEngine.Object) choreA.chore.target.gameObject == (UnityEngine.Object) choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType);
  }

  public static string GetChoreName(Chore chore, object choreData)
  {
    string str = string.Empty;
    if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || (chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical) || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
      str = chore.GetReportName(chore.gameObject.GetProperName());
    else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
    {
      FetchChore fetchChore = chore as FetchChore;
      FetchAreaChore fetchAreaChore = chore as FetchAreaChore;
      if (fetchAreaChore != null)
      {
        GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        str = !((UnityEngine.Object) getFetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName((string) null) : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(getFetchTarget.GetProperName());
      }
      else if (fetchChore != null)
      {
        Pickupable fetchTarget = fetchChore.fetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        str = !((UnityEngine.Object) fetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName((string) null) : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(fetchTarget.GetProperName());
      }
    }
    else
      str = chore.GetReportName((string) null);
    return str;
  }

  public static string ChoreGroupsForChoreType(ChoreType choreType)
  {
    if (choreType.groups == null || choreType.groups.Length == 0)
      return (string) null;
    string empty = string.Empty;
    for (int index = 0; index < choreType.groups.Length; ++index)
    {
      if (index != 0)
        empty += (string) UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
      empty += choreType.groups[index].Name;
    }
    return empty;
  }

  public static bool IsCapturingTimeLapse()
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Game.Instance.timelapser != (UnityEngine.Object) null)
      return Game.Instance.timelapser.CapturingTimelapseScreenshot;
    return false;
  }

  public static ExposureType GetExposureTypeForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return GERM_EXPOSURE.TYPES[index];
    }
    return (ExposureType) null;
  }

  public static Sickness GetSicknessForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[index].sickness_id);
    }
    return (Sickness) null;
  }

  public enum UnitClass
  {
    SimpleFloat,
    SimpleInteger,
    Temperature,
    Mass,
    Calories,
    Percent,
    Distance,
    Disease,
  }

  public enum TemperatureUnit
  {
    Celsius,
    Fahrenheit,
    Kelvin,
  }

  public enum MassUnit
  {
    Kilograms,
    Pounds,
  }

  public enum MetricMassFormat
  {
    UseThreshold,
    Kilogram,
    Gram,
    Tonne,
  }

  public enum TemperatureInterpretation
  {
    Absolute,
    Relative,
  }

  public enum TimeSlice
  {
    None,
    ModifyOnly,
    PerSecond,
    PerCycle,
  }

  public enum MeasureUnit
  {
    mass,
    kcal,
    quantity,
  }

  public enum WattageFormatterUnit
  {
    Watts,
    Kilowatts,
    Automatic,
  }

  public enum HeatEnergyFormatterUnit
  {
    DTU_S,
    KDTU_S,
    Automatic,
  }

  public struct FloodFillInfo
  {
    public int cell;
    public int depth;
  }

  public enum Hardness
  {
    NA = 0,
    VERY_SOFT = 0,
    SOFT = 10, // 0x0000000A
    FIRM = 25, // 0x00000019
    VERY_FIRM = 50, // 0x00000032
    NEARLY_IMPENETRABLE = 150, // 0x00000096
    SUPER_HARD = 200, // 0x000000C8
    IMPENETRABLE = 255, // 0x000000FF
  }

  public enum GermResistanceModifier
  {
    NEGATIVE_LARGE = -5, // 0xFFFFFFFB
    NEGATIVE_MEDIUM = -2, // 0xFFFFFFFE
    NEGATIVE_SMALL = -1, // 0xFFFFFFFF
    NONE = 0,
    POSITIVE_SMALL = 1,
    POSITIVE_MEDIUM = 2,
    POSITIVE_LARGE = 5,
  }
}
