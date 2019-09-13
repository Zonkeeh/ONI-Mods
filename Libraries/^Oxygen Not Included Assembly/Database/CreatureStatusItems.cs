// Decompiled with JetBrains decompiler
// Type: Database.CreatureStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Database
{
  public class CreatureStatusItems : StatusItems
  {
    public StatusItem HealthStatus;
    public StatusItem Hot;
    public StatusItem Hot_Crop;
    public StatusItem Scalding;
    public StatusItem Cold;
    public StatusItem Cold_Crop;
    public StatusItem Crop_Too_Dark;
    public StatusItem Crop_Too_Bright;
    public StatusItem Hypothermia;
    public StatusItem Hyperthermia;
    public StatusItem Suffocating;
    public StatusItem Hatching;
    public StatusItem Incubating;
    public StatusItem Drowning;
    public StatusItem Saturated;
    public StatusItem DryingOut;
    public StatusItem Growing;
    public StatusItem CropSleeping;
    public StatusItem ReadyForHarvest;
    public StatusItem EnvironmentTooWarm;
    public StatusItem EnvironmentTooCold;
    public StatusItem Entombed;
    public StatusItem Wilting;
    public StatusItem WiltingDomestic;
    public StatusItem WiltingNonGrowing;
    public StatusItem WiltingNonGrowingDomestic;
    public StatusItem WrongAtmosphere;
    public StatusItem AtmosphericPressureTooLow;
    public StatusItem AtmosphericPressureTooHigh;
    public StatusItem Barren;
    public StatusItem NeedsFertilizer;
    public StatusItem NeedsIrrigation;
    public StatusItem WrongTemperature;
    public StatusItem WrongFertilizer;
    public StatusItem WrongIrrigation;
    public StatusItem WrongFertilizerMajor;
    public StatusItem WrongIrrigationMajor;
    public StatusItem CantAcceptFertilizer;
    public StatusItem CantAcceptIrrigation;
    public StatusItem Rotting;
    public StatusItem Fresh;
    public StatusItem Stale;
    public StatusItem Spoiled;
    public StatusItem Refrigerated;
    public StatusItem Unrefrigerated;
    public StatusItem SterilizingAtmosphere;
    public StatusItem ContaminatedAtmosphere;
    public StatusItem Old;
    public StatusItem ExchangingElementOutput;
    public StatusItem ExchangingElementConsume;
    public StatusItem Hungry;

    public CreatureStatusItems(ResourceSet parent)
      : base(nameof (CreatureStatusItems), parent)
    {
      this.CreateStatusItems();
    }

    private void CreateStatusItems()
    {
      this.Hot = new StatusItem("Hot", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Hot.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
      });
      this.Hot_Crop = new StatusItem("Hot_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Hot_Crop.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
        str = str.Replace("{low_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{high_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Scalding = new StatusItem("Scalding", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, true, OverlayModes.None.ID, true, 129022);
      this.Scalding.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        float externalTemperature = ((ExternalTemperatureMonitor.Instance) data).AverageExternalTemperature;
        float scaldingThreshold = ((ExternalTemperatureMonitor.Instance) data).GetScaldingThreshold();
        str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(externalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(scaldingThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Scalding.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Cold = new StatusItem("Cold", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Cold.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
      });
      this.Cold_Crop = new StatusItem("Cold_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Cold_Crop.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
        str = str.Replace("low_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_Low, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("high_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.internalTemperatureWarning_High, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Crop_Too_Dark = new StatusItem("Crop_Too_Dark", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Crop_Too_Dark.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.Crop_Too_Bright = new StatusItem("Crop_Too_Bright", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.Crop_Too_Bright.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.Hyperthermia = new StatusItem("Hyperthermia", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.Hyperthermia.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        float temp = ((TemperatureMonitor.Instance) data).temperature.value;
        float hyperthermiaThreshold = ((TemperatureMonitor.Instance) data).HyperthermiaThreshold;
        str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hyperthermiaThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Hypothermia = new StatusItem("Hypothermia", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.Hypothermia.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        float temp = ((TemperatureMonitor.Instance) data).temperature.value;
        float hypothermiaThreshold = ((TemperatureMonitor.Instance) data).HypothermiaThreshold;
        str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hypothermiaThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Suffocating = new StatusItem("Suffocating", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Hatching = new StatusItem("Hatching", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Incubating = new StatusItem("Incubating", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Drowning = new StatusItem("Drowning", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Drowning.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.Saturated = new StatusItem("Saturated", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Saturated.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.DryingOut = new StatusItem("DryingOut", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026);
      this.DryingOut.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.ReadyForHarvest = new StatusItem("ReadyForHarvest", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026);
      this.Growing = new StatusItem("Growing", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026);
      this.Growing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        if ((UnityEngine.Object) ((Component) data).GetComponent<Crop>() != (UnityEngine.Object) null)
        {
          float seconds = ((global::Growing) data).TimeUntilNextHarvest();
          str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(seconds, "F1"));
        }
        float val1 = 100f * ((global::Growing) data).PercentGrown();
        str = str.Replace("{PercentGrow}", Math.Floor((double) Math.Max(val1, 0.0f)).ToString("F0"));
        return str;
      });
      this.CropSleeping = new StatusItem("Crop_Sleeping", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026);
      this.CropSleeping.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CropSleepingMonitor.Instance instance = (CropSleepingMonitor.Instance) data;
        return str.Replace("{REASON}", (string) (!instance.def.prefersDarkness ? CREATURES.STATUSITEMS.CROP_SLEEPING.REASON_TOO_DARK : CREATURES.STATUSITEMS.CROP_SLEEPING.REASON_TOO_BRIGHT));
      });
      this.CropSleeping.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        CropSleepingMonitor.Instance instance = (CropSleepingMonitor.Instance) data;
        string newValue = string.Format((string) CREATURES.STATUSITEMS.CROP_SLEEPING.REQUIREMENT_LUMINANCE, (object) instance.def.lightIntensityThreshold);
        return str.Replace("{REQUIREMENTS}", newValue);
      });
      this.EnvironmentTooWarm = new StatusItem("EnvironmentTooWarm", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.EnvironmentTooWarm.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        float temp1 = Grid.Temperature[Grid.PosToCell(((Component) data).gameObject)];
        float temp2 = ((TemperatureVulnerable) data).internalTemperatureLethal_High - 1f;
        str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp1, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.EnvironmentTooCold = new StatusItem("EnvironmentTooCold", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.EnvironmentTooCold.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        float temp1 = Grid.Temperature[Grid.PosToCell(((Component) data).gameObject)];
        float temp2 = ((TemperatureVulnerable) data).internalTemperatureLethal_Low + 1f;
        str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp1, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.Entombed = new StatusItem("Entombed", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Entombed.resolveStringCallback = (Func<string, object, string>) ((str, go) => str);
      this.Entombed.resolveTooltipCallback = (Func<string, object, string>) ((str, go) =>
      {
        GameObject go1 = go as GameObject;
        return string.Format(str, (object) GameUtil.GetIdentityDescriptor(go1));
      });
      this.Wilting = new StatusItem("Wilting", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026);
      this.Wilting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        if (data is global::Growing && data != null)
          str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(((global::Growing) data).growthTime, ((global::Growing) data).TimeUntilNextHarvest()), "F1"));
        str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
        return str;
      });
      this.WiltingDomestic = new StatusItem("WiltingDomestic", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026);
      this.WiltingDomestic.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        if (data is global::Growing && data != null)
          str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(((global::Growing) data).growthTime, ((global::Growing) data).TimeUntilNextHarvest()), "F1"));
        str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
        return str;
      });
      this.WiltingNonGrowing = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026);
      this.WiltingNonGrowing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        str = (string) CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
        str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
        return str;
      });
      this.WiltingNonGrowingDomestic = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026);
      this.WiltingNonGrowingDomestic.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        str = (string) CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
        str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
        return str;
      });
      this.WrongAtmosphere = new StatusItem("WrongAtmosphere", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.WrongAtmosphere.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = string.Empty;
        foreach (Element safeAtmosphere in (data as PressureVulnerable).safe_atmospheres)
          newValue = newValue + "\n    •  " + safeAtmosphere.name;
        str = str.Replace("{elements}", newValue);
        return str;
      });
      this.AtmosphericPressureTooLow = new StatusItem("AtmosphericPressureTooLow", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.AtmosphericPressureTooLow.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        PressureVulnerable pressureVulnerable = (PressureVulnerable) data;
        str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.AtmosphericPressureTooHigh = new StatusItem("AtmosphericPressureTooHigh", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.AtmosphericPressureTooHigh.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        PressureVulnerable pressureVulnerable = (PressureVulnerable) data;
        str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.HealthStatus = new StatusItem("HealthStatus", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.HealthStatus.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = string.Empty;
        switch ((Health.HealthState) data)
        {
          case Health.HealthState.Perfect:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.NAME;
            break;
          case Health.HealthState.Scuffed:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.NAME;
            break;
          case Health.HealthState.Injured:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INJURED.NAME;
            break;
          case Health.HealthState.Critical:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.NAME;
            break;
          case Health.HealthState.Incapacitated:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.NAME;
            break;
          case Health.HealthState.Dead:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.DEAD.NAME;
            break;
        }
        str = str.Replace("{healthState}", newValue);
        return str;
      });
      this.HealthStatus.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = string.Empty;
        switch ((Health.HealthState) data)
        {
          case Health.HealthState.Perfect:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.TOOLTIP;
            break;
          case Health.HealthState.Scuffed:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.TOOLTIP;
            break;
          case Health.HealthState.Injured:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INJURED.TOOLTIP;
            break;
          case Health.HealthState.Critical:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.TOOLTIP;
            break;
          case Health.HealthState.Incapacitated:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.TOOLTIP;
            break;
          case Health.HealthState.Dead:
            newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.DEAD.TOOLTIP;
            break;
        }
        str = str.Replace("{healthState}", newValue);
        return str;
      });
      this.Barren = new StatusItem("Barren", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeedsFertilizer = new StatusItem("NeedsFertilizer", "CREATURES", "status_item_plant_solid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.NeedsFertilizer.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.NeedsIrrigation = new StatusItem("NeedsIrrigation", "CREATURES", "status_item_plant_liquid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.NeedsIrrigation.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
      this.WrongFertilizer = new StatusItem("WrongFertilizer", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      Func<string, object, string> func1 = (Func<string, object, string>) ((str, data) => str);
      this.WrongFertilizer.resolveStringCallback = func1;
      this.WrongFertilizerMajor = new StatusItem("WrongFertilizerMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.WrongFertilizerMajor.resolveStringCallback = func1;
      this.WrongIrrigation = new StatusItem("WrongIrrigation", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      Func<string, object, string> func2 = (Func<string, object, string>) ((str, data) => str);
      this.WrongIrrigation.resolveStringCallback = func2;
      this.WrongIrrigationMajor = new StatusItem("WrongIrrigationMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.WrongIrrigationMajor.resolveStringCallback = func2;
      this.CantAcceptFertilizer = new StatusItem("CantAcceptFertilizer", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Rotting = new StatusItem("Rotting", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Rotting.resolveStringCallback = (Func<string, object, string>) ((str, data) => str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(277.15f, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
      this.Fresh = new StatusItem("Fresh", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Fresh.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Rottable.Instance instance = (Rottable.Instance) data;
        return str.Replace("{RotPercentage}", "(" + Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f) + "%)");
      });
      this.Fresh.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        Rottable.Instance instance = (Rottable.Instance) data;
        return str.Replace("{RotTooltip}", instance.GetToolTip());
      });
      this.Stale = new StatusItem("Stale", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Stale.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Rottable.Instance instance = (Rottable.Instance) data;
        return str.Replace("{RotPercentage}", "(" + Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f) + "%)");
      });
      this.Stale.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        Rottable.Instance instance = (Rottable.Instance) data;
        return str.Replace("{RotTooltip}", instance.GetToolTip());
      });
      this.Spoiled = new StatusItem("Spoiled", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Refrigerated = new StatusItem("Refrigerated", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Unrefrigerated = new StatusItem("Unrefrigerated", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Unrefrigerated.resolveStringCallback = (Func<string, object, string>) ((str, data) => str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(277.15f, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
      this.SterilizingAtmosphere = new StatusItem("SterilizingAtmosphere", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ContaminatedAtmosphere = new StatusItem("ContaminatedAtmosphere", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Old = new StatusItem("Old", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Old.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        AgeMonitor.Instance instance = (AgeMonitor.Instance) data;
        return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(instance.CyclesUntilDeath * 600f, "F1"));
      });
      this.ExchangingElementConsume = new StatusItem("ExchangingElementConsume", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ExchangingElementConsume.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
        str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
        str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.ExchangingElementConsume.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
        str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
        str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.ExchangingElementOutput = new StatusItem("ExchangingElementOutput", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ExchangingElementOutput.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
        str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
        str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.ExchangingElementOutput.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
        str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
        str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.Hungry = new StatusItem("Hungry", "CREATURES", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Hungry.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        Diet diet = ((StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GenericInstance) data).master.gameObject.GetDef<CreatureCalorieMonitor.Def>().diet;
        if (diet.consumedTags.Count <= 0)
          return str;
        string[] strArray = diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>();
        if (strArray.Length > 3)
          strArray = new string[4]
          {
            strArray[0],
            strArray[1],
            strArray[2],
            "..."
          };
        string newValue = string.Join(", ", strArray);
        return str + "\n" + UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue);
      });
    }
  }
}
