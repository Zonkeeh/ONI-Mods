// Decompiled with JetBrains decompiler
// Type: Database.MiscStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

namespace Database
{
  public class MiscStatusItems : StatusItems
  {
    public StatusItem MarkedForDisinfection;
    public StatusItem MarkedForCompost;
    public StatusItem MarkedForCompostInStorage;
    public StatusItem PendingClear;
    public StatusItem PendingClearNoStorage;
    public StatusItem Edible;
    public StatusItem WaitingForDig;
    public StatusItem WaitingForMop;
    public StatusItem OreMass;
    public StatusItem OreTemp;
    public StatusItem ElementalCategory;
    public StatusItem ElementalState;
    public StatusItem ElementalTemperature;
    public StatusItem ElementalMass;
    public StatusItem ElementalDisease;
    public StatusItem TreeFilterableTags;
    public StatusItem OxyRockInactive;
    public StatusItem OxyRockEmitting;
    public StatusItem OxyRockBlocked;
    public StatusItem BuriedItem;
    public StatusItem SpoutOverPressure;
    public StatusItem SpoutEmitting;
    public StatusItem SpoutPressureBuilding;
    public StatusItem SpoutIdle;
    public StatusItem SpoutDormant;
    public StatusItem OrderAttack;
    public StatusItem OrderCapture;
    public StatusItem PendingHarvest;
    public StatusItem NotMarkedForHarvest;
    public StatusItem PendingUproot;
    public StatusItem PickupableUnreachable;
    public StatusItem Prioritized;
    public StatusItem Using;
    public StatusItem Operating;
    public StatusItem Cleaning;
    public StatusItem RegionIsBlocked;
    public StatusItem NoClearLocationsAvailable;
    public StatusItem AwaitingStudy;
    public StatusItem Studied;
    public StatusItem StudiedGeyserTimeRemaining;
    public StatusItem Space;

    public MiscStatusItems(ResourceSet parent)
      : base(nameof (MiscStatusItems), parent)
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
      int status_overlays = 129022)
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
      int status_overlays = 129022)
    {
      return this.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays));
    }

    private void CreateStatusItems()
    {
      this.Edible = this.CreateStatusItem("Edible", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Edible.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Edible edible = (global::Edible) data;
        str = string.Format(str, (object) GameUtil.GetFormattedCalories(edible.Calories, GameUtil.TimeSlice.None, true));
        return str;
      });
      this.PendingClear = this.CreateStatusItem("PendingClear", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingClearNoStorage = this.CreateStatusItem("PendingClearNoStorage", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.MarkedForCompost = this.CreateStatusItem("MarkedForCompost", "MISC", "status_item_pending_compost", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.MarkedForCompostInStorage = this.CreateStatusItem("MarkedForCompostInStorage", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.MarkedForDisinfection = this.CreateStatusItem("MarkedForDisinfection", "MISC", "status_item_disinfect", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Disease.ID, true, 129022);
      this.NoClearLocationsAvailable = this.CreateStatusItem("NoClearLocationsAvailable", "MISC", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.WaitingForDig = this.CreateStatusItem("WaitingForDig", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.WaitingForMop = this.CreateStatusItem("WaitingForMop", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OreMass = this.CreateStatusItem("OreMass", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OreMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        GameObject gameObject = (GameObject) data;
        str = str.Replace("{Mass}", GameUtil.GetFormattedMass(gameObject.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.OreTemp = this.CreateStatusItem("OreTemp", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OreTemp.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        GameObject gameObject = (GameObject) data;
        str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(gameObject.GetComponent<PrimaryElement>().Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.ElementalState = this.CreateStatusItem("ElementalState", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ElementalState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element element = ((Func<Element>) data)();
        str = str.Replace("{State}", element.GetStateString());
        return str;
      });
      this.ElementalCategory = this.CreateStatusItem("ElementalCategory", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ElementalCategory.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element element = ((Func<Element>) data)();
        str = str.Replace("{Category}", element.GetMaterialCategoryTag().ProperName());
        return str;
      });
      this.ElementalTemperature = this.CreateStatusItem("ElementalTemperature", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ElementalTemperature.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(cellSelectionObject.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
        return str;
      });
      this.ElementalMass = this.CreateStatusItem("ElementalMass", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ElementalMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Mass}", GameUtil.GetFormattedMass(cellSelectionObject.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.ElementalDisease = this.CreateStatusItem("ElementalDisease", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ElementalDisease.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, false));
        return str;
      });
      this.ElementalDisease.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, true));
        return str;
      });
      this.TreeFilterableTags = this.CreateStatusItem("TreeFilterableTags", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.TreeFilterableTags.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TreeFilterable treeFilterable = (TreeFilterable) data;
        str = str.Replace("{Tags}", treeFilterable.GetTagsAsStatus(6));
        return str;
      });
      this.OxyRockEmitting = this.CreateStatusItem("OxyRockEmitting", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OxyRockEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(cellSelectionObject.FlowRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.OxyRockBlocked = this.CreateStatusItem("OxyRockBlocked", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OxyRockBlocked.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        bool all_not_gaseous;
        bool all_over_pressure;
        GameUtil.IsEmissionBlocked(((CellSelectionObject) data).SelectedCell, out all_not_gaseous, out all_over_pressure);
        string newValue = (string) null;
        if (all_not_gaseous)
          newValue = (string) MISC.STATUSITEMS.OXYROCK.NEIGHBORSBLOCKED.NAME;
        else if (all_over_pressure)
          newValue = (string) MISC.STATUSITEMS.OXYROCK.OVERPRESSURE.NAME;
        str = str.Replace("{BlockedString}", newValue);
        return str;
      });
      this.OxyRockInactive = this.CreateStatusItem("OxyRockInactive", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Space = this.CreateStatusItem("Space", "MISC", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.BuriedItem = this.CreateStatusItem("BuriedItem", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SpoutOverPressure = this.CreateStatusItem("SpoutOverPressure", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SpoutOverPressure.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", string.Empty) : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTOVERPRESSURE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1")));
        return str;
      });
      this.SpoutEmitting = this.CreateStatusItem("SpoutEmitting", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SpoutEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", string.Empty) : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTEMITTING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime(), "F1")));
        return str;
      });
      this.SpoutPressureBuilding = this.CreateStatusItem("SpoutPressureBuilding", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SpoutPressureBuilding.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", string.Empty) : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTPRESSUREBUILDING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1")));
        return str;
      });
      this.SpoutIdle = this.CreateStatusItem("SpoutIdle", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SpoutIdle.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
        Studyable component = statesInstance.GetComponent<Studyable>();
        str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", string.Empty) : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTIDLE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime(), "F1")));
        return str;
      });
      this.SpoutDormant = this.CreateStatusItem("SpoutDormant", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OrderAttack = this.CreateStatusItem("OrderAttack", "MISC", "status_item_attack", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.OrderCapture = this.CreateStatusItem("OrderCapture", "MISC", "status_item_capture", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingHarvest = this.CreateStatusItem("PendingHarvest", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NotMarkedForHarvest = this.CreateStatusItem("NotMarkedForHarvest", "MISC", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NotMarkedForHarvest.conditionalOverlayCallback = (Func<HashedString, object, bool>) ((viewMode, o) => !(viewMode != OverlayModes.None.ID));
      this.PendingUproot = this.CreateStatusItem("PendingUproot", "MISC", "status_item_pending_uproot", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PickupableUnreachable = this.CreateStatusItem("PickupableUnreachable", "MISC", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Prioritized = this.CreateStatusItem("Prioritized", "MISC", "status_item_prioritized", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Using = this.CreateStatusItem("Using", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Using.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Workable workable = (Workable) data;
        if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
        {
          KSelectable component = workable.GetComponent<KSelectable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            str = str.Replace("{Target}", component.GetName());
        }
        return str;
      });
      this.Operating = this.CreateStatusItem("Operating", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Cleaning = this.CreateStatusItem("Cleaning", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.RegionIsBlocked = this.CreateStatusItem("RegionIsBlocked", "MISC", "status_item_solids_blocking", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.AwaitingStudy = this.CreateStatusItem("AwaitingStudy", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Studied = this.CreateStatusItem("Studied", "MISC", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    }
  }
}
