// Decompiled with JetBrains decompiler
// Type: Database.DuplicantStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;

namespace Database
{
  public class DuplicantStatusItems : StatusItems
  {
    public StatusItem Idle;
    public StatusItem Pacified;
    public StatusItem PendingPacification;
    public StatusItem Dead;
    public StatusItem MoveToSuitNotRequired;
    public StatusItem DroppingUnusedInventory;
    public StatusItem MovingToSafeArea;
    public StatusItem BedUnreachable;
    public StatusItem Hungry;
    public StatusItem Starving;
    public StatusItem Rotten;
    public StatusItem Quarantined;
    public StatusItem NoRationsAvailable;
    public StatusItem RationsUnreachable;
    public StatusItem RationsNotPermitted;
    public StatusItem DailyRationLimitReached;
    public StatusItem Scalding;
    public StatusItem Hot;
    public StatusItem Cold;
    public StatusItem QuarantineAreaUnassigned;
    public StatusItem QuarantineAreaUnreachable;
    public StatusItem Tired;
    public StatusItem NervousBreakdown;
    public StatusItem Unhappy;
    public StatusItem Suffocating;
    public StatusItem HoldingBreath;
    public StatusItem ToiletUnreachable;
    public StatusItem NoUsableToilets;
    public StatusItem NoToilets;
    public StatusItem Vomiting;
    public StatusItem Coughing;
    public StatusItem BreathingO2;
    public StatusItem EmittingCO2;
    public StatusItem LowOxygen;
    public StatusItem RedAlert;
    public StatusItem Digging;
    public StatusItem Eating;
    public StatusItem Sleeping;
    public StatusItem SleepingInterruptedByLight;
    public StatusItem SleepingInterruptedByNoise;
    public StatusItem SleepingPeacefully;
    public StatusItem SleepingBadly;
    public StatusItem SleepingTerribly;
    public StatusItem Cleaning;
    public StatusItem PickingUp;
    public StatusItem Mopping;
    public StatusItem Cooking;
    public StatusItem Arting;
    public StatusItem Mushing;
    public StatusItem Researching;
    public StatusItem Tinkering;
    public StatusItem Storing;
    public StatusItem Building;
    public StatusItem Equipping;
    public StatusItem WarmingUp;
    public StatusItem GeneratingPower;
    public StatusItem Harvesting;
    public StatusItem Uprooting;
    public StatusItem Emptying;
    public StatusItem Toggling;
    public StatusItem Deconstructing;
    public StatusItem Disinfecting;
    public StatusItem Relocating;
    public StatusItem Upgrading;
    public StatusItem Fabricating;
    public StatusItem Processing;
    public StatusItem Clearing;
    public StatusItem BodyRegulatingHeating;
    public StatusItem BodyRegulatingCooling;
    public StatusItem EntombedChore;
    public StatusItem EarlyMorning;
    public StatusItem NightTime;
    public StatusItem PoorDecor;
    public StatusItem PoorQualityOfLife;
    public StatusItem PoorFoodQuality;
    public StatusItem GoodFoodQuality;
    public StatusItem SevereWounds;
    public StatusItem Incapacitated;
    public StatusItem Fighting;
    public StatusItem Fleeing;
    public StatusItem Stressed;
    public StatusItem LashingOut;
    public StatusItem LowImmunity;
    public StatusItem Studying;
    public StatusItem Socializing;
    public StatusItem Dancing;
    public StatusItem Gaming;
    public StatusItem Mingling;
    public StatusItem ContactWithGerms;
    public StatusItem ExposedToGerms;
    public StatusItem LightWorkEfficiencyBonus;
    private const int NONE_OVERLAY = 0;

    public DuplicantStatusItems(ResourceSet parent)
      : base(nameof (DuplicantStatusItems), parent)
    {
      this.CreateStatusItems();
    }

    private StatusItem CreateStatusItem(
      string id,
      string prefix,
      string icon,
      StatusItem.IconType icon_type,
      NotificationType notification_type,
      bool allow_multiples,
      HashedString render_overlay,
      bool showWorldIcon = true,
      int status_overlays = 2)
    {
      return this.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays));
    }

    private StatusItem CreateStatusItem(
      string id,
      string name,
      string tooltip,
      string icon,
      StatusItem.IconType icon_type,
      NotificationType notification_type,
      bool allow_multiples,
      HashedString render_overlay,
      int status_overlays = 2)
    {
      return this.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays));
    }

    private void CreateStatusItems()
    {
      Func<string, object, string> func1 = (Func<string, object, string>) ((str, data) =>
      {
        Workable workable = (Workable) data;
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
          str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
        return str;
      });
      Func<string, object, string> func2 = (Func<string, object, string>) ((str, data) =>
      {
        Workable workable = (Workable) data;
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
        {
          str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
          ComplexFabricatorWorkable fabricatorWorkable = workable as ComplexFabricatorWorkable;
          if ((UnityEngine.Object) fabricatorWorkable != (UnityEngine.Object) null)
          {
            ComplexRecipe currentWorkingOrder = fabricatorWorkable.CurrentWorkingOrder;
            if (currentWorkingOrder != null)
              str = str.Replace("{Item}", currentWorkingOrder.FirstResult.ProperName());
          }
        }
        return str;
      });
      this.BedUnreachable = this.CreateStatusItem("BedUnreachable", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.BedUnreachable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.DailyRationLimitReached = this.CreateStatusItem("DailyRationLimitReached", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.DailyRationLimitReached.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.HoldingBreath = this.CreateStatusItem("HoldingBreath", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Hungry = this.CreateStatusItem("Hungry", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Unhappy = this.CreateStatusItem("Unhappy", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Unhappy.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NervousBreakdown = this.CreateStatusItem("NervousBreakdown", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.NervousBreakdown.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoRationsAvailable = this.CreateStatusItem("NoRationsAvailable", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.PendingPacification = this.CreateStatusItem("PendingPacification", "DUPLICANTS", "status_item_pending_pacification", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.QuarantineAreaUnassigned = this.CreateStatusItem("QuarantineAreaUnassigned", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.QuarantineAreaUnassigned.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.QuarantineAreaUnreachable = this.CreateStatusItem("QuarantineAreaUnreachable", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.QuarantineAreaUnreachable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Quarantined = this.CreateStatusItem("Quarantined", "DUPLICANTS", "status_item_quarantined", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.RationsUnreachable = this.CreateStatusItem("RationsUnreachable", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.RationsUnreachable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.RationsNotPermitted = this.CreateStatusItem("RationsNotPermitted", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.RationsNotPermitted.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Rotten = this.CreateStatusItem("Rotten", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Starving = this.CreateStatusItem("Starving", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.Starving.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Suffocating = this.CreateStatusItem("Suffocating", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
      this.Suffocating.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Tired = this.CreateStatusItem("Tired", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Idle = this.CreateStatusItem("Idle", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Idle.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Pacified = this.CreateStatusItem("Pacified", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Dead = this.CreateStatusItem("Dead", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Dead.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Death death = (Death) data;
        return str.Replace("{Death}", death.Name);
      });
      this.MoveToSuitNotRequired = this.CreateStatusItem("MoveToSuitNotRequired", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.DroppingUnusedInventory = this.CreateStatusItem("DroppingUnusedInventory", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.MovingToSafeArea = this.CreateStatusItem("MovingToSafeArea", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.ToiletUnreachable = this.CreateStatusItem("ToiletUnreachable", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.ToiletUnreachable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoUsableToilets = this.CreateStatusItem("NoUsableToilets", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.NoUsableToilets.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoToilets = this.CreateStatusItem("NoToilets", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.NoToilets.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.BreathingO2 = this.CreateStatusItem("BreathingO2", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
      this.BreathingO2.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        float averageRate = Game.Instance.accumulators.GetAverageRate(((OxygenBreather) data).O2Accumulator);
        return str.Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(-averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
      });
      this.EmittingCO2 = this.CreateStatusItem("EmittingCO2", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
      this.EmittingCO2.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        OxygenBreather oxygenBreather = (OxygenBreather) data;
        return str.Replace("{EmittingRate}", GameUtil.GetFormattedMass(oxygenBreather.CO2EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
      });
      this.Vomiting = this.CreateStatusItem("Vomiting", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Coughing = this.CreateStatusItem("Coughing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.LowOxygen = this.CreateStatusItem("LowOxygen", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.LowOxygen.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.RedAlert = this.CreateStatusItem("RedAlert", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Sleeping = this.CreateStatusItem("Sleeping", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Sleeping.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        if (data is SleepChore.StatesInstance)
        {
          string changeNoiseSource = ((SleepChore.StatesInstance) data).stateChangeNoiseSource;
          if (!string.IsNullOrEmpty(changeNoiseSource))
          {
            string str1 = (string) DUPLICANTS.STATUSITEMS.SLEEPING.TOOLTIP.Replace("{Disturber}", changeNoiseSource);
            str += str1;
          }
        }
        return str;
      });
      this.SleepingInterruptedByNoise = this.CreateStatusItem("SleepingInterruptedByNoise", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.SleepingInterruptedByLight = this.CreateStatusItem("SleepingInterruptedByLight", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Eating = this.CreateStatusItem("Eating", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Eating.resolveStringCallback = func1;
      this.Digging = this.CreateStatusItem("Digging", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Cleaning = this.CreateStatusItem("Cleaning", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Cleaning.resolveStringCallback = func1;
      this.PickingUp = this.CreateStatusItem("PickingUp", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.PickingUp.resolveStringCallback = func1;
      this.Mopping = this.CreateStatusItem("Mopping", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Cooking = this.CreateStatusItem("Cooking", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Cooking.resolveStringCallback = func2;
      this.Mushing = this.CreateStatusItem("Mushing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Mushing.resolveStringCallback = func2;
      this.Researching = this.CreateStatusItem("Researching", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Researching.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TechInstance activeResearch = Research.Instance.GetActiveResearch();
        if (activeResearch != null)
          return str.Replace("{Tech}", activeResearch.tech.Name);
        return str;
      });
      this.Tinkering = this.CreateStatusItem("Tinkering", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Tinkering.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Tinkerable tinkerable = (Tinkerable) data;
        if ((UnityEngine.Object) tinkerable != (UnityEngine.Object) null)
          return string.Format(str, (object) tinkerable.tinkerMaterialTag.ProperName());
        return str;
      });
      this.Storing = this.CreateStatusItem("Storing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Storing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Workable workable = (Workable) data;
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null && (UnityEngine.Object) workable.worker != (UnityEngine.Object) null)
        {
          KSelectable component1 = workable.GetComponent<KSelectable>();
          if ((bool) ((UnityEngine.Object) component1))
            str = str.Replace("{Target}", component1.GetName());
          Pickupable workCompleteData = workable.worker.workCompleteData as Pickupable;
          if ((UnityEngine.Object) workable.worker != (UnityEngine.Object) null && (bool) ((UnityEngine.Object) workCompleteData))
          {
            KSelectable component2 = workCompleteData.GetComponent<KSelectable>();
            if ((bool) ((UnityEngine.Object) component2))
              str = str.Replace("{Item}", component2.GetName());
          }
        }
        return str;
      });
      this.Building = this.CreateStatusItem("Building", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Building.resolveStringCallback = func1;
      this.Equipping = this.CreateStatusItem("Equipping", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Equipping.resolveStringCallback = func1;
      this.WarmingUp = this.CreateStatusItem("WarmingUp", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.WarmingUp.resolveStringCallback = func1;
      this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.GeneratingPower.resolveStringCallback = func1;
      this.Harvesting = this.CreateStatusItem("Harvesting", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Harvesting.resolveStringCallback = func1;
      this.Uprooting = this.CreateStatusItem("Uprooting", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Uprooting.resolveStringCallback = func1;
      this.Emptying = this.CreateStatusItem("Emptying", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Emptying.resolveStringCallback = func1;
      this.Toggling = this.CreateStatusItem("Toggling", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Toggling.resolveStringCallback = func1;
      this.Deconstructing = this.CreateStatusItem("Deconstructing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Deconstructing.resolveStringCallback = func1;
      this.Disinfecting = this.CreateStatusItem("Disinfecting", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Disinfecting.resolveStringCallback = func1;
      this.Upgrading = this.CreateStatusItem("Upgrading", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Upgrading.resolveStringCallback = func1;
      this.Fabricating = this.CreateStatusItem("Fabricating", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Fabricating.resolveStringCallback = func2;
      this.Processing = this.CreateStatusItem("Processing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Processing.resolveStringCallback = func2;
      this.Clearing = this.CreateStatusItem("Clearing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Clearing.resolveStringCallback = func1;
      this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.GeneratingPower.resolveStringCallback = func1;
      this.Cold = this.CreateStatusItem("Cold", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Cold.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
        float dtu_s = ((ExternalTemperatureMonitor.Instance) data).temperatureTransferer.average_kilowatts_exchanged.GetWeightedAverage * 1000f;
        str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
        AttributeInstance attributeInstance = ((ExternalTemperatureMonitor.Instance) data).attributes.Get("ThermalConductivityBarrier");
        string newValue = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
        for (int index = 0; index != attributeInstance.Modifiers.Count; ++index)
        {
          AttributeModifier modifier = attributeInstance.Modifiers[index];
          newValue = newValue + "\n" + "    • " + modifier.GetDescription() + " <b>" + modifier.GetFormattedString(attributeInstance.gameObject) + "</b>";
        }
        str = str.Replace("{conductivityBarrier}", newValue);
        return str;
      });
      this.Hot = this.CreateStatusItem("Hot", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.Hot.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
        float dtu_s = ((ExternalTemperatureMonitor.Instance) data).temperatureTransferer.average_kilowatts_exchanged.GetWeightedAverage * 1000f;
        str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
        AttributeInstance attributeInstance = ((ExternalTemperatureMonitor.Instance) data).attributes.Get("ThermalConductivityBarrier");
        string newValue = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
        for (int index = 0; index != attributeInstance.Modifiers.Count; ++index)
        {
          AttributeModifier modifier = attributeInstance.Modifiers[index];
          newValue = newValue + "\n" + "    • " + modifier.GetDescription() + " <b>" + modifier.GetFormattedString(attributeInstance.gameObject) + "</b>";
        }
        str = str.Replace("{conductivityBarrier}", newValue);
        return str;
      });
      this.BodyRegulatingHeating = this.CreateStatusItem("BodyRegulatingHeating", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.BodyRegulatingHeating.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        WarmBlooded.StatesInstance statesInstance = (WarmBlooded.StatesInstance) data;
        return str.Replace("{TempDelta}", GameUtil.GetFormattedTemperature(statesInstance.TemperatureDelta, GameUtil.TimeSlice.PerSecond, GameUtil.TemperatureInterpretation.Relative, true, false));
      });
      this.BodyRegulatingCooling = this.CreateStatusItem("BodyRegulatingCooling", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.BodyRegulatingCooling.resolveStringCallback = this.BodyRegulatingHeating.resolveStringCallback;
      this.EntombedChore = this.CreateStatusItem("EntombedChore", "DUPLICANTS", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
      this.EntombedChore.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.EarlyMorning = this.CreateStatusItem("EarlyMorning", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.NightTime = this.CreateStatusItem("NightTime", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.PoorDecor = this.CreateStatusItem("PoorDecor", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.PoorQualityOfLife = this.CreateStatusItem("PoorQualityOfLife", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.PoorFoodQuality = this.CreateStatusItem("PoorFoodQuality", (string) DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.NAME, (string) DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.TOOLTIP, string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
      this.GoodFoodQuality = this.CreateStatusItem("GoodFoodQuality", (string) DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.NAME, (string) DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.TOOLTIP, string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
      this.Arting = this.CreateStatusItem("Arting", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Arting.resolveStringCallback = func1;
      this.SevereWounds = this.CreateStatusItem("SevereWounds", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.SevereWounds.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Incapacitated = this.CreateStatusItem("Incapacitated", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
      this.Incapacitated.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Incapacitated.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IncapacitationMonitor.Instance smi = (IncapacitationMonitor.Instance) data;
        float bleedLifeTime = smi.GetBleedLifeTime(smi);
        str = str.Replace("{CauseOfIncapacitation}", smi.GetCauseOfIncapacitation().Name);
        return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedTime(bleedLifeTime));
      });
      this.Relocating = this.CreateStatusItem("Relocating", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Relocating.resolveStringCallback = func1;
      this.Fighting = this.CreateStatusItem("Fighting", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.Fighting.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Fleeing = this.CreateStatusItem("Fleeing", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.Fleeing.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Stressed = this.CreateStatusItem("Stressed", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Stressed.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.LashingOut = this.CreateStatusItem("LashingOut", "DUPLICANTS", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
      this.LashingOut.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.LowImmunity = this.CreateStatusItem("LowImmunity", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
      this.LowImmunity.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Studying = this.CreateStatusItem("Studying", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
      this.Socializing = this.CreateStatusItem("Socializing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
      this.Dancing = this.CreateStatusItem("Dancing", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
      this.Gaming = this.CreateStatusItem("Gaming", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
      this.Mingling = this.CreateStatusItem("Mingling", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
      this.ContactWithGerms = this.CreateStatusItem("ContactWithGerms", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
      this.ContactWithGerms.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string name = Db.Get().Sicknesses.Get(((GermExposureMonitor.ExposureStatusData) data).exposure_type.sickness_id).Name;
        str = str.Replace("{Sickness}", name);
        return str;
      });
      this.ContactWithGerms.statusItemClickCallback = (System.Action<object>) (data =>
      {
        GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData) data;
        CameraController.Instance.CameraGoTo(exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id), 2f, true);
        if (!(OverlayScreen.Instance.mode == OverlayModes.None.ID))
          return;
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
      });
      this.ExposedToGerms = this.CreateStatusItem("ExposedToGerms", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
      this.ExposedToGerms.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData) data;
        string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
        AttributeInstance attributeInstance = Db.Get().Attributes.GermResistance.Lookup(exposureStatusData.owner.gameObject);
        string lastDiseaseSource = exposureStatusData.owner.GetLastDiseaseSource(exposureStatusData.exposure_type.germ_id);
        GermExposureMonitor.Instance smi = exposureStatusData.owner.GetSMI<GermExposureMonitor.Instance>();
        float num1 = (float) exposureStatusData.exposure_type.base_resistance + GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
        float totalValue = attributeInstance.GetTotalValue();
        float resistanceToExposureType = smi.GetResistanceToExposureType(exposureStatusData.exposure_type, -1f);
        float contractionChance = GermExposureMonitor.GetContractionChance(resistanceToExposureType);
        float exposureTier = smi.GetExposureTier(exposureStatusData.exposure_type.germ_id);
        float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int) exposureTier - 1] - GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
        str = str.Replace("{Severity}", (string) DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.EXPOSURE_TIERS[(int) exposureTier - 1]);
        str = str.Replace("{Sickness}", name);
        str = str.Replace("{Source}", lastDiseaseSource);
        str = str.Replace("{Base}", GameUtil.GetFormattedSimple(num1, GameUtil.TimeSlice.None, (string) null));
        str = str.Replace("{Dupe}", GameUtil.GetFormattedSimple(totalValue, GameUtil.TimeSlice.None, (string) null));
        str = str.Replace("{Total}", GameUtil.GetFormattedSimple(resistanceToExposureType, GameUtil.TimeSlice.None, (string) null));
        str = str.Replace("{ExposureLevelBonus}", GameUtil.GetFormattedSimple(num2, GameUtil.TimeSlice.None, (string) null));
        str = str.Replace("{Chance}", GameUtil.GetFormattedPercent(contractionChance * 100f, GameUtil.TimeSlice.None));
        return str;
      });
      this.ExposedToGerms.statusItemClickCallback = (System.Action<object>) (data =>
      {
        GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData) data;
        CameraController.Instance.CameraGoTo(exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id), 2f, true);
        if (!(OverlayScreen.Instance.mode == OverlayModes.None.ID))
          return;
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
      });
      this.LightWorkEfficiencyBonus = this.CreateStatusItem("LightWorkEfficiencyBonus", "DUPLICANTS", string.Empty, StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
    }
  }
}
