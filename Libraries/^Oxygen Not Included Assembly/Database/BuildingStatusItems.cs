// Decompiled with JetBrains decompiler
// Type: Database.BuildingStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
  public class BuildingStatusItems : StatusItems
  {
    public MaterialsStatusItem MaterialsUnavailable;
    public MaterialsStatusItem MaterialsUnavailableForRefill;
    public StatusItem AngerDamage;
    public StatusItem ClinicOutsideHospital;
    public StatusItem DigUnreachable;
    public StatusItem MopUnreachable;
    public StatusItem ConstructableDigUnreachable;
    public StatusItem ConstructionUnreachable;
    public StatusItem NewDuplicantsAvailable;
    public StatusItem NeedPlant;
    public StatusItem NeedPower;
    public StatusItem NotEnoughPower;
    public StatusItem PowerLoopDetected;
    public StatusItem NeedLiquidIn;
    public StatusItem NeedGasIn;
    public StatusItem NeedResourceMass;
    public StatusItem NeedSolidIn;
    public StatusItem NeedLiquidOut;
    public StatusItem NeedGasOut;
    public StatusItem NeedSolidOut;
    public StatusItem InvalidBuildingLocation;
    public StatusItem PendingDeconstruction;
    public StatusItem PendingSwitchToggle;
    public StatusItem GasVentObstructed;
    public StatusItem LiquidVentObstructed;
    public StatusItem LiquidPipeEmpty;
    public StatusItem LiquidPipeObstructed;
    public StatusItem GasPipeEmpty;
    public StatusItem GasPipeObstructed;
    public StatusItem SolidPipeObstructed;
    public StatusItem PartiallyDamaged;
    public StatusItem Broken;
    public StatusItem PendingRepair;
    public StatusItem PendingUpgrade;
    public StatusItem RequiresSkillPerk;
    public StatusItem DigRequiresSkillPerk;
    public StatusItem ColonyLacksRequiredSkillPerk;
    public StatusItem PendingWork;
    public StatusItem Flooded;
    public StatusItem PowerButtonOff;
    public StatusItem SwitchStatusActive;
    public StatusItem SwitchStatusInactive;
    public StatusItem LogicSwitchStatusActive;
    public StatusItem LogicSwitchStatusInactive;
    public StatusItem LogicSensorStatusActive;
    public StatusItem LogicSensorStatusInactive;
    public StatusItem ChangeDoorControlState;
    public StatusItem CurrentDoorControlState;
    public StatusItem Entombed;
    public MaterialsStatusItem WaitingForMaterials;
    public StatusItem WaitingForRepairMaterials;
    public StatusItem MissingFoundation;
    public StatusItem NeutroniumUnminable;
    public StatusItem NoStorageFilterSet;
    public StatusItem PendingFish;
    public StatusItem NoFishableWaterBelow;
    public StatusItem GasVentOverPressure;
    public StatusItem LiquidVentOverPressure;
    public StatusItem NoWireConnected;
    public StatusItem NoLogicWireConnected;
    public StatusItem NoTubeConnected;
    public StatusItem NoTubeExits;
    public StatusItem StoredCharge;
    public StatusItem NoPowerConsumers;
    public StatusItem PressureOk;
    public StatusItem UnderPressure;
    public StatusItem AssignedTo;
    public StatusItem Unassigned;
    public StatusItem AssignedPublic;
    public StatusItem AssignedToRoom;
    public StatusItem RationBoxContents;
    public StatusItem ConduitBlocked;
    public StatusItem OutputPipeFull;
    public StatusItem ConduitBlockedMultiples;
    public StatusItem MeltingDown;
    public StatusItem UnderConstruction;
    public StatusItem UnderConstructionNoWorker;
    public StatusItem Normal;
    public StatusItem ManualGeneratorChargingUp;
    public StatusItem ManualGeneratorReleasingEnergy;
    public StatusItem GeneratorOffline;
    public StatusItem Pipe;
    public StatusItem Conveyor;
    public StatusItem FabricatorIdle;
    public StatusItem FabricatorEmpty;
    public StatusItem FlushToilet;
    public StatusItem FlushToiletInUse;
    public StatusItem Toilet;
    public StatusItem ToiletNeedsEmptying;
    public StatusItem DesalinatorNeedsEmptying;
    public StatusItem Unusable;
    public StatusItem NoResearchSelected;
    public StatusItem NoApplicableResearchSelected;
    public StatusItem NoApplicableAnalysisSelected;
    public StatusItem NoResearchOrDestinationSelected;
    public StatusItem Researching;
    public StatusItem ValveRequest;
    public StatusItem EmittingLight;
    public StatusItem EmittingElement;
    public StatusItem EmittingOxygenAvg;
    public StatusItem EmittingGasAvg;
    public StatusItem PumpingLiquidOrGas;
    public StatusItem NoLiquidElementToPump;
    public StatusItem NoGasElementToPump;
    public StatusItem PipeFull;
    public StatusItem PipeMayMelt;
    public StatusItem ElementConsumer;
    public StatusItem ElementEmitterOutput;
    public StatusItem AwaitingWaste;
    public StatusItem AwaitingCompostFlip;
    public StatusItem JoulesAvailable;
    public StatusItem Wattage;
    public StatusItem SolarPanelWattage;
    public StatusItem SteamTurbineWattage;
    public StatusItem Wattson;
    public StatusItem WireConnected;
    public StatusItem WireNominal;
    public StatusItem WireDisconnected;
    public StatusItem Cooling;
    public StatusItem CoolingStalledHotEnv;
    public StatusItem CoolingStalledColdGas;
    public StatusItem CoolingStalledHotLiquid;
    public StatusItem CoolingStalledColdLiquid;
    public StatusItem Working;
    public StatusItem CannotCoolFurther;
    public StatusItem NeedsValidRegion;
    public StatusItem NeedSeed;
    public StatusItem AwaitingSeedDelivery;
    public StatusItem AwaitingBaitDelivery;
    public StatusItem NoAvailableSeed;
    public StatusItem NeedEgg;
    public StatusItem AwaitingEggDelivery;
    public StatusItem NoAvailableEgg;
    public StatusItem Grave;
    public StatusItem GraveEmpty;
    public StatusItem NoFilterElementSelected;
    public StatusItem NoLureElementSelected;
    public StatusItem BuildingDisabled;
    public StatusItem Overheated;
    public StatusItem Overloaded;
    public StatusItem Expired;
    public StatusItem PumpingStation;
    public StatusItem EmptyPumpingStation;
    public StatusItem GeneShuffleCompleted;
    public StatusItem DirectionControl;
    public StatusItem WellPressurizing;
    public StatusItem WellOverpressure;
    public StatusItem ReleasingPressure;
    public StatusItem NoSuitMarker;
    public StatusItem SuitMarkerWrongSide;
    public StatusItem SuitMarkerTraversalAnytime;
    public StatusItem SuitMarkerTraversalOnlyWhenRoomAvailable;
    public StatusItem TooCold;
    public StatusItem NotInAnyRoom;
    public StatusItem NotInRequiredRoom;
    public StatusItem NotInRecommendedRoom;
    public StatusItem IncubatorProgress;
    public StatusItem HabitatNeedsEmptying;
    public StatusItem DetectorScanning;
    public StatusItem IncomingMeteors;
    public StatusItem HasGantry;
    public StatusItem MissingGantry;
    public StatusItem DisembarkingDuplicant;
    public StatusItem RocketName;
    public StatusItem PathNotClear;
    public StatusItem InvalidPortOverlap;
    public StatusItem EmergencyPriority;
    public StatusItem SkillPointsAvailable;
    public StatusItem Baited;

    public BuildingStatusItems(ResourceSet parent)
      : base(nameof (BuildingStatusItems), parent)
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
      this.AngerDamage = this.CreateStatusItem("AngerDamage", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.AssignedTo = this.CreateStatusItem("AssignedTo", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.AssignedTo.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IAssignableIdentity assignee = ((Assignable) data).assignee;
        if (assignee != null)
        {
          string properName = assignee.GetProperName();
          str = str.Replace("{Assignee}", properName);
        }
        return str;
      });
      this.AssignedToRoom = this.CreateStatusItem("AssignedToRoom", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.AssignedToRoom.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IAssignableIdentity assignee = ((Assignable) data).assignee;
        if (assignee != null)
        {
          string properName = assignee.GetProperName();
          str = str.Replace("{Assignee}", properName);
        }
        return str;
      });
      this.Broken = this.CreateStatusItem("Broken", "BUILDING", "status_item_broken", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Broken.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        BuildingHP.DamageSourceInfo damageSourceInfo = ((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GenericInstance) data).master.GetDamageSourceInfo();
        return str.Replace("{DamageInfo}", damageSourceInfo.ToString());
      });
      StatusItem broken = this.Broken;
      // ISSUE: reference to a compiler-generated field
      if (BuildingStatusItems.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BuildingStatusItems.\u003C\u003Ef__mg\u0024cache0 = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
      }
      // ISSUE: reference to a compiler-generated field
      Func<HashedString, object, bool> fMgCache0 = BuildingStatusItems.\u003C\u003Ef__mg\u0024cache0;
      broken.conditionalOverlayCallback = fMgCache0;
      this.ChangeDoorControlState = this.CreateStatusItem("ChangeDoorControlState", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ChangeDoorControlState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Door door = (Door) data;
        return str.Replace("{ControlState}", door.RequestedState.ToString());
      });
      this.CurrentDoorControlState = this.CreateStatusItem("CurrentDoorControlState", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.CurrentDoorControlState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = (string) Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + ((Door) data).CurrentState.ToString().ToUpper());
        return str.Replace("{ControlState}", newValue);
      });
      this.ClinicOutsideHospital = this.CreateStatusItem("ClinicOutsideHospital", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
      this.ConduitBlocked = this.CreateStatusItem("ConduitBlocked", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.OutputPipeFull = this.CreateStatusItem("OutputPipeFull", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.ConstructionUnreachable = this.CreateStatusItem("ConstructionUnreachable", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.ConduitBlockedMultiples = this.CreateStatusItem("ConduitBlocked", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022);
      this.DigUnreachable = this.CreateStatusItem("DigUnreachable", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.MopUnreachable = this.CreateStatusItem("MopUnreachable", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.DirectionControl = this.CreateStatusItem("DirectionControl", (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME, (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
      this.DirectionControl.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::DirectionControl directionControl = (global::DirectionControl) data;
        string newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.BOTH;
        switch (directionControl.allowedDirection)
        {
          case WorkableReactable.AllowedDirection.Left:
            newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.LEFT;
            break;
          case WorkableReactable.AllowedDirection.Right:
            newValue = (string) BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.RIGHT;
            break;
        }
        str = str.Replace("{Direction}", newValue);
        return str;
      });
      this.ConstructableDigUnreachable = this.CreateStatusItem("ConstructableDigUnreachable", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Entombed = this.CreateStatusItem("Entombed", "BUILDING", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Entombed.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.Flooded = this.CreateStatusItem("Flooded", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Flooded.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.GasVentObstructed = this.CreateStatusItem("GasVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
      this.GasVentOverPressure = this.CreateStatusItem("GasVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
      this.GeneShuffleCompleted = this.CreateStatusItem("GeneShuffleCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.InvalidBuildingLocation = this.CreateStatusItem("InvalidBuildingLocation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.LiquidVentObstructed = this.CreateStatusItem("LiquidVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.LiquidVentOverPressure = this.CreateStatusItem("LiquidVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.None.ID);
      this.MaterialsUnavailable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.MaterialsUnavailable.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = string.Empty;
        Dictionary<Tag, float> dictionary = (Dictionary<Tag, float>) null;
        if (data is IFetchList)
          dictionary = ((IFetchList) data).GetRemainingMinimum();
        else if (data is Dictionary<Tag, float>)
          dictionary = data as Dictionary<Tag, float>;
        if (dictionary.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in dictionary)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                newValue += "\n";
              newValue = !Assets.IsTagCountable(keyValuePair.Key) ? newValue + string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : newValue + string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_UNITS, (object) GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", newValue);
        return str;
      });
      this.MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
      this.MaterialsUnavailableForRefill.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IFetchList fetchList = (IFetchList) data;
        string empty = string.Empty;
        Dictionary<Tag, float> remaining = fetchList.GetRemaining();
        if (remaining.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                empty += "\n";
              empty += string.Format((string) BUILDING.STATUSITEMS.MATERIALSUNAVAILABLEFORREFILL.LINE_ITEM, (object) keyValuePair.Key.ProperName());
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", empty);
        return str;
      });
      Func<string, object, string> func1 = (Func<string, object, string>) ((str, data) =>
      {
        RoomType roomType = Db.Get().RoomTypes.Get((string) data);
        if (roomType != null)
          return string.Format(str, (object) roomType.Name);
        return str;
      });
      this.NotInAnyRoom = this.CreateStatusItem("NotInAnyRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NotInRequiredRoom = this.CreateStatusItem("NotInRequiredRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NotInRequiredRoom.resolveStringCallback = func1;
      this.NotInRecommendedRoom = this.CreateStatusItem("NotInRecommendedRoom", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NotInRecommendedRoom.resolveStringCallback = func1;
      this.WaitingForRepairMaterials = this.CreateStatusItem("WaitingForRepairMaterials", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID, false, 129022);
      this.WaitingForRepairMaterials.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        KeyValuePair<Tag, float> keyValuePair = (KeyValuePair<Tag, float>) data;
        if ((double) keyValuePair.Value != 0.0)
        {
          string newValue = string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
          str = str.Replace("{ItemsRemaining}", newValue);
        }
        return str;
      });
      this.WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID);
      this.WaitingForMaterials.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IFetchList fetchList = (IFetchList) data;
        string newValue = string.Empty;
        Dictionary<Tag, float> remaining = fetchList.GetRemaining();
        if (remaining.Count > 0)
        {
          bool flag = true;
          foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
          {
            if ((double) keyValuePair.Value != 0.0)
            {
              if (!flag)
                newValue += "\n";
              newValue = !Assets.IsTagCountable(keyValuePair.Key) ? newValue + string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, (object) keyValuePair.Key.ProperName(), (object) GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : newValue + string.Format((string) BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_UNITS, (object) GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
              flag = false;
            }
          }
        }
        str = str.Replace("{ItemsRemaining}", newValue);
        return str;
      });
      this.MeltingDown = this.CreateStatusItem("MeltingDown", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.MissingFoundation = this.CreateStatusItem("MissingFoundation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeutroniumUnminable = this.CreateStatusItem("NeutroniumUnminable", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeedGasIn = this.CreateStatusItem("NeedGasIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
      this.NeedGasIn.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>) data;
        string newValue = string.Format((string) BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM, (object) tuple.second.ProperName());
        str = str.Replace("{GasRequired}", newValue);
        return str;
      });
      this.NeedGasOut = this.CreateStatusItem("NeedGasOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.GasConduits.ID, true, 129022);
      this.NeedLiquidIn = this.CreateStatusItem("NeedLiquidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.NeedLiquidIn.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Tuple<ConduitType, Tag> tuple = (Tuple<ConduitType, Tag>) data;
        string newValue = string.Format((string) BUILDING.STATUSITEMS.NEEDLIQUIDIN.LINE_ITEM, (object) tuple.second.ProperName());
        str = str.Replace("{LiquidRequired}", newValue);
        return str;
      });
      this.NeedLiquidOut = this.CreateStatusItem("NeedLiquidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.LiquidConduits.ID, true, 129022);
      this.NeedSolidIn = this.CreateStatusItem("NeedSolidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, true, 129022);
      this.NeedSolidOut = this.CreateStatusItem("NeedSolidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.SolidConveyor.ID, true, 129022);
      this.NeedResourceMass = this.CreateStatusItem("NeedResourceMass", "BUILDING", "status_item_need_resource", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeedResourceMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string empty = string.Empty;
        EnergyGenerator.Formula formula = (EnergyGenerator.Formula) data;
        if (formula.inputs.Length > 0)
        {
          bool flag = true;
          foreach (EnergyGenerator.InputItem input in formula.inputs)
          {
            if (!flag)
            {
              empty += "\n";
              flag = false;
            }
            empty += string.Format((string) BUILDING.STATUSITEMS.NEEDRESOURCEMASS.LINE_ITEM, (object) input.tag.ProperName());
          }
        }
        str = str.Replace("{ResourcesRequired}", empty);
        return str;
      });
      this.LiquidPipeEmpty = this.CreateStatusItem("LiquidPipeEmpty", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.LiquidPipeObstructed = this.CreateStatusItem("LiquidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.LiquidConduits.ID, true, 129022);
      this.GasPipeEmpty = this.CreateStatusItem("GasPipeEmpty", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
      this.GasPipeObstructed = this.CreateStatusItem("GasPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.GasConduits.ID, true, 129022);
      this.SolidPipeObstructed = this.CreateStatusItem("SolidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.SolidConveyor.ID, true, 129022);
      this.NeedPlant = this.CreateStatusItem("NeedPlant", "BUILDING", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeedPower = this.CreateStatusItem("NeedPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.NotEnoughPower = this.CreateStatusItem("NotEnoughPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.PowerLoopDetected = this.CreateStatusItem("PowerLoopDetected", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.NewDuplicantsAvailable = this.CreateStatusItem("NewDuplicantsAvailable", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NewDuplicantsAvailable.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NewDuplicantsAvailable.notificationClickCallback = (Notification.ClickCallback) (data =>
      {
        ImmigrantScreen.InitializeImmigrantScreen((Telepad) data);
        Game.Instance.Trigger(288942073, (object) null);
      });
      this.NoStorageFilterSet = this.CreateStatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoSuitMarker = this.CreateStatusItem("NoSuitMarker", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.SuitMarkerWrongSide = this.CreateStatusItem("suitMarkerWrongSide", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.SuitMarkerTraversalAnytime = this.CreateStatusItem("suitMarkerTraversalAnytime", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SuitMarkerTraversalOnlyWhenRoomAvailable = this.CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NoFishableWaterBelow = this.CreateStatusItem("NoFishableWaterBelow", "BUILDING", "status_item_no_fishable_water_below", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoPowerConsumers = this.CreateStatusItem("NoPowerConsumers", "BUILDING", "status_item_no_power_consumers", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.NoWireConnected = this.CreateStatusItem("NoWireConnected", "BUILDING", "status_item_no_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.Power.ID, true, 129022);
      this.NoLogicWireConnected = this.CreateStatusItem("NoLogicWireConnected", "BUILDING", "status_item_no_logic_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Logic.ID, true, 129022);
      this.NoTubeConnected = this.CreateStatusItem("NoTubeConnected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoTubeExits = this.CreateStatusItem("NoTubeExits", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.StoredCharge = this.CreateStatusItem("StoredCharge", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.StoredCharge.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        TravelTubeEntrance.SMInstance smInstance = (TravelTubeEntrance.SMInstance) data;
        if (smInstance != null)
          str = string.Format(str, (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.AvailableJoules), (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.TotalCapacity), (object) GameUtil.GetFormattedRoundedJoules(smInstance.master.UsageJoules));
        return str;
      });
      this.PendingDeconstruction = this.CreateStatusItem("PendingDeconstruction", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      StatusItem pendingDeconstruction = this.PendingDeconstruction;
      // ISSUE: reference to a compiler-generated field
      if (BuildingStatusItems.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BuildingStatusItems.\u003C\u003Ef__mg\u0024cache1 = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
      }
      // ISSUE: reference to a compiler-generated field
      Func<HashedString, object, bool> fMgCache1 = BuildingStatusItems.\u003C\u003Ef__mg\u0024cache1;
      pendingDeconstruction.conditionalOverlayCallback = fMgCache1;
      this.PendingRepair = this.CreateStatusItem("PendingRepair", "BUILDING", "status_item_pending_repair", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.PendingRepair.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        BuildingHP.DamageSourceInfo damageSourceInfo = ((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GenericInstance) data).master.GetComponent<BuildingHP>().GetDamageSourceInfo();
        return str.Replace("{DamageInfo}", damageSourceInfo.ToString());
      });
      this.PendingRepair.conditionalOverlayCallback = (Func<HashedString, object, bool>) ((mode, data) => true);
      this.RequiresSkillPerk = this.CreateStatusItem("RequiresSkillPerk", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.RequiresSkillPerk.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.Get((string) data));
        List<string> stringList = new List<string>();
        foreach (Skill skill in skillsWithPerk)
          stringList.Add(skill.Name);
        str = str.Replace("{Skills}", string.Join(", ", stringList.ToArray()));
        return str;
      });
      this.DigRequiresSkillPerk = this.CreateStatusItem("DigRequiresSkillPerk", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.DigRequiresSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
      this.ColonyLacksRequiredSkillPerk = this.CreateStatusItem("ColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.ColonyLacksRequiredSkillPerk.resolveStringCallback = this.RequiresSkillPerk.resolveStringCallback;
      this.SwitchStatusActive = this.CreateStatusItem("SwitchStatusActive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.SwitchStatusInactive = this.CreateStatusItem("SwitchStatusInactive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.LogicSwitchStatusActive = this.CreateStatusItem("LogicSwitchStatusActive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.LogicSwitchStatusInactive = this.CreateStatusItem("LogicSwitchStatusInactive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.LogicSensorStatusActive = this.CreateStatusItem("LogicSensorStatusActive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.LogicSensorStatusInactive = this.CreateStatusItem("LogicSensorStatusInactive", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingFish = this.CreateStatusItem("PendingFish", "BUILDING", "status_item_pending_fish", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingSwitchToggle = this.CreateStatusItem("PendingSwitchToggle", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingUpgrade = this.CreateStatusItem("PendingUpgrade", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PendingWork = this.CreateStatusItem("PendingWork", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.PowerButtonOff = this.CreateStatusItem("PowerButtonOff", "BUILDING", "status_item_power_button_off", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.PressureOk = this.CreateStatusItem("PressureOk", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
      this.UnderPressure = this.CreateStatusItem("UnderPressure", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
      this.Unassigned = this.CreateStatusItem("Unassigned", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Rooms.ID, true, 129022);
      this.AssignedPublic = this.CreateStatusItem("AssignedPublic", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Rooms.ID, true, 129022);
      this.UnderConstruction = this.CreateStatusItem("UnderConstruction", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.UnderConstructionNoWorker = this.CreateStatusItem("UnderConstructionNoWorker", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Normal = this.CreateStatusItem("Normal", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ManualGeneratorChargingUp = this.CreateStatusItem("ManualGeneratorChargingUp", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.ManualGeneratorReleasingEnergy = this.CreateStatusItem("ManualGeneratorReleasingEnergy", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.GeneratorOffline = this.CreateStatusItem("GeneratorOffline", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.Pipe = this.CreateStatusItem("Pipe", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.Pipe.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Conduit conduit = (Conduit) data;
        int cell = Grid.PosToCell((KMonoBehaviour) conduit);
        ConduitFlow.ConduitContents contents = conduit.GetFlowManager().GetContents(cell);
        string newValue = (string) BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
        if ((double) contents.mass > 0.0)
        {
          Element elementByHash = ElementLoader.FindElementByHash(contents.element);
          newValue = string.Format((string) BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS, (object) GameUtil.GetFormattedMass(contents.mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) elementByHash.name, (object) GameUtil.GetFormattedTemperature(contents.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
          if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && contents.diseaseIdx != byte.MaxValue)
            newValue += string.Format((string) BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS_WITH_DISEASE, (object) GameUtil.GetFormattedDisease(contents.diseaseIdx, contents.diseaseCount, true));
        }
        str = str.Replace("{Contents}", newValue);
        return str;
      });
      this.Conveyor = this.CreateStatusItem("Conveyor", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.SolidConveyor.ID, true, 129022);
      this.Conveyor.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        int cell = Grid.PosToCell((KMonoBehaviour) data);
        SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
        SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
        string newValue = (string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
        if (contents.pickupableHandle.IsValid())
        {
          Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
          if ((bool) ((UnityEngine.Object) pickupable))
          {
            PrimaryElement component = pickupable.GetComponent<PrimaryElement>();
            float mass = component.Mass;
            if ((double) mass > 0.0)
            {
              Element elementByHash = ElementLoader.FindElementByHash(component.ElementID);
              newValue = string.Format((string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) elementByHash.name, (object) GameUtil.GetFormattedTemperature(component.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
              if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && component.DiseaseIdx != byte.MaxValue)
                newValue += string.Format((string) BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE, (object) GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true));
            }
          }
        }
        str = str.Replace("{Contents}", newValue);
        return str;
      });
      this.FabricatorIdle = this.CreateStatusItem("FabricatorIdle", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.FabricatorEmpty = this.CreateStatusItem("FabricatorEmpty", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Toilet = this.CreateStatusItem("Toilet", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Toilet.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Toilet.StatesInstance statesInstance = (global::Toilet.StatesInstance) data;
        if (statesInstance != null)
          str = str.Replace("{FlushesRemaining}", statesInstance.GetFlushesRemaining().ToString());
        return str;
      });
      this.ToiletNeedsEmptying = this.CreateStatusItem("ToiletNeedsEmptying", "BUILDING", "status_item_toilet_needs_emptying", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.DesalinatorNeedsEmptying = this.CreateStatusItem("DesalinatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Unusable = this.CreateStatusItem("Unusable", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoResearchSelected = this.CreateStatusItem("NoResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoResearchSelected.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoResearchSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
        str = str.Replace("{RESEARCH_MENU_KEY}", newValue);
        return str;
      });
      this.NoResearchSelected.notificationClickCallback = (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenResearch());
      this.NoApplicableResearchSelected = this.CreateStatusItem("NoApplicableResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoApplicableResearchSelected.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoApplicableAnalysisSelected = this.CreateStatusItem("NoApplicableAnalysisSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoApplicableAnalysisSelected.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.NoApplicableAnalysisSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
        str = str.Replace("{STARMAP_MENU_KEY}", newValue);
        return str;
      });
      this.NoApplicableAnalysisSelected.notificationClickCallback = (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenStarmap());
      this.NoResearchOrDestinationSelected = this.CreateStatusItem("NoResearchOrDestinationSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoResearchOrDestinationSelected.resolveTooltipCallback += (Func<string, object, string>) ((str, data) =>
      {
        string newValue1 = GameInputMapping.FindEntry(Action.ManageStarmap).mKeyCode.ToString();
        str = str.Replace("{STARMAP_MENU_KEY}", newValue1);
        string newValue2 = GameInputMapping.FindEntry(Action.ManageResearch).mKeyCode.ToString();
        str = str.Replace("{RESEARCH_MENU_KEY}", newValue2);
        return str;
      });
      this.NoResearchOrDestinationSelected.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.ValveRequest = this.CreateStatusItem("ValveRequest", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.ValveRequest.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Valve valve = (Valve) data;
        str = str.Replace("{QueuedMaxFlow}", GameUtil.GetFormattedMass(valve.QueuedMaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.EmittingLight = this.CreateStatusItem("EmittingLight", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.EmittingLight.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        string newValue = GameInputMapping.FindEntry(Action.Overlay5).mKeyCode.ToString();
        str = str.Replace("{LightGridOverlay}", newValue);
        return str;
      });
      this.RationBoxContents = this.CreateStatusItem("RationBoxContents", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.RationBoxContents.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        RationBox rationBox = (RationBox) data;
        if ((UnityEngine.Object) rationBox == (UnityEngine.Object) null)
          return str;
        Storage component1 = rationBox.GetComponent<Storage>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
          return str;
        float calories = 0.0f;
        foreach (GameObject gameObject in component1.items)
        {
          Edible component2 = gameObject.GetComponent<Edible>();
          if ((bool) ((UnityEngine.Object) component2))
            calories += component2.Calories;
        }
        str = str.Replace("{Stored}", GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
        return str;
      });
      this.EmittingElement = this.CreateStatusItem("EmittingElement", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.EmittingElement.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        IElementEmitter elementEmitter = (IElementEmitter) data;
        string newValue = ElementLoader.FindElementByHash(elementEmitter.Element).tag.ProperName();
        str = str.Replace("{ElementType}", newValue);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.AverageEmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.EmittingOxygenAvg = this.CreateStatusItem("EmittingOxygenAvg", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.EmittingOxygenAvg.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Sublimates sublimates = (Sublimates) data;
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.EmittingGasAvg = this.CreateStatusItem("EmittingGasAvg", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.EmittingGasAvg.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Sublimates sublimates = (Sublimates) data;
        str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.PumpingLiquidOrGas = this.CreateStatusItem("PumpingLiquidOrGas", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.PumpingLiquidOrGas.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        float averageRate = Game.Instance.accumulators.GetAverageRate((HandleVector<int>.Handle) data);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.PipeMayMelt = this.CreateStatusItem("PipeMayMelt", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoLiquidElementToPump = this.CreateStatusItem("NoLiquidElementToPump", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
      this.NoGasElementToPump = this.CreateStatusItem("NoGasElementToPump", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
      this.NoFilterElementSelected = this.CreateStatusItem("NoFilterElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NoLureElementSelected = this.CreateStatusItem("NoLureElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.ElementConsumer = this.CreateStatusItem("ElementConsumer", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
      this.ElementConsumer.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::ElementConsumer elementConsumer = (global::ElementConsumer) data;
        string newValue = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag.ProperName();
        str = str.Replace("{ElementTypes}", newValue);
        str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementConsumer.AverageConsumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        return str;
      });
      this.ElementEmitterOutput = this.CreateStatusItem("ElementEmitterOutput", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
      this.ElementEmitterOutput.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        ElementEmitter elementEmitter = (ElementEmitter) data;
        if ((UnityEngine.Object) elementEmitter != (UnityEngine.Object) null)
        {
          str = str.Replace("{ElementTypes}", elementEmitter.outputElement.Name);
          str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.outputElement.massGenerationRate / elementEmitter.emissionFrequency, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        }
        return str;
      });
      this.AwaitingWaste = this.CreateStatusItem("AwaitingWaste", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
      this.AwaitingCompostFlip = this.CreateStatusItem("AwaitingCompostFlip", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
      this.JoulesAvailable = this.CreateStatusItem("JoulesAvailable", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.JoulesAvailable.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Battery battery = (Battery) data;
        str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(battery.JoulesAvailable, "F1", GameUtil.TimeSlice.None));
        str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(battery.Capacity, "F1", GameUtil.TimeSlice.None));
        return str;
      });
      this.Wattage = this.CreateStatusItem("Wattage", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.Wattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Generator generator = (Generator) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic));
        return str;
      });
      this.SolarPanelWattage = this.CreateStatusItem("SolarPanelWattage", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.SolarPanelWattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SolarPanel solarPanel = (SolarPanel) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic));
        return str;
      });
      this.SteamTurbineWattage = this.CreateStatusItem("SteamTurbineWattage", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.SteamTurbineWattage.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SteamTurbine steamTurbine = (SteamTurbine) data;
        str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic));
        return str;
      });
      this.Wattson = this.CreateStatusItem("Wattson", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Wattson.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Telepad telepad = (Telepad) data;
        str = !((UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null) || !GameFlowManager.Instance.IsGameOver() ? (!telepad.GetComponent<Operational>().IsOperational ? str.Replace("{TimeRemaining}", (string) BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE) : str.Replace("{TimeRemaining}", GameUtil.GetFormattedCycles(telepad.GetTimeRemaining(), "F1"))) : (string) BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME;
        return str;
      });
      this.FlushToilet = this.CreateStatusItem("FlushToilet", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.FlushToiletInUse = this.CreateStatusItem("FlushToiletInUse", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.WireNominal = this.CreateStatusItem("WireNominal", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.WireConnected = this.CreateStatusItem("WireConnected", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
      this.WireDisconnected = this.CreateStatusItem("WireDisconnected", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
      this.Overheated = this.CreateStatusItem("Overheated", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.Overloaded = this.CreateStatusItem("Overloaded", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
      this.Cooling = this.CreateStatusItem("Cooling", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      Func<string, object, string> func2 = (Func<string, object, string>) ((str, data) =>
      {
        AirConditioner airConditioner = (AirConditioner) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
      });
      this.CoolingStalledColdGas = this.CreateStatusItem("CoolingStalledColdGas", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.CoolingStalledColdGas.resolveStringCallback = func2;
      this.CoolingStalledColdLiquid = this.CreateStatusItem("CoolingStalledColdLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.CoolingStalledColdLiquid.resolveStringCallback = func2;
      Func<string, object, string> func3 = (Func<string, object, string>) ((str, data) =>
      {
        AirConditioner airConditioner = (AirConditioner) data;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(airConditioner.lastEnvTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(airConditioner.maxEnvironmentDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
      });
      this.CoolingStalledHotEnv = this.CreateStatusItem("CoolingStalledHotEnv", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.CoolingStalledHotEnv.resolveStringCallback = func3;
      this.CoolingStalledHotLiquid = this.CreateStatusItem("CoolingStalledHotLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.CoolingStalledHotLiquid.resolveStringCallback = func3;
      this.Working = this.CreateStatusItem("Working", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NeedsValidRegion = this.CreateStatusItem("NeedsValidRegion", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NeedSeed = this.CreateStatusItem("NeedSeed", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.AwaitingSeedDelivery = this.CreateStatusItem("AwaitingSeedDelivery", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.AwaitingBaitDelivery = this.CreateStatusItem("AwaitingBaitDelivery", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NoAvailableSeed = this.CreateStatusItem("NoAvailableSeed", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.NeedEgg = this.CreateStatusItem("NeedEgg", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.AwaitingEggDelivery = this.CreateStatusItem("AwaitingEggDelivery", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.NoAvailableEgg = this.CreateStatusItem("NoAvailableEgg", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.Grave = this.CreateStatusItem("Grave", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Grave.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        global::Grave.StatesInstance statesInstance = (global::Grave.StatesInstance) data;
        string str1 = str.Replace("{DeadDupe}", statesInstance.master.graveName);
        string[] strings = LocString.GetStrings(typeof (NAMEGEN.GRAVE.EPITAPHS));
        int index = statesInstance.master.epitaphIdx % strings.Length;
        return str1.Replace("{Epitaph}", strings[index]);
      });
      this.GraveEmpty = this.CreateStatusItem("GraveEmpty", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.CannotCoolFurther = this.CreateStatusItem("CannotCoolFurther", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.BuildingDisabled = this.CreateStatusItem("BuildingDisabled", "BUILDING", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.Expired = this.CreateStatusItem("Expired", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PumpingStation = this.CreateStatusItem("PumpingStation", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.PumpingStation.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        LiquidPumpingStation liquidPumpingStation = (LiquidPumpingStation) data;
        if ((UnityEngine.Object) liquidPumpingStation != (UnityEngine.Object) null)
          return liquidPumpingStation.ResolveString(str);
        return str;
      });
      this.EmptyPumpingStation = this.CreateStatusItem("EmptyPumpingStation", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.WellPressurizing = this.CreateStatusItem("WellPressurizing", (string) BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME, (string) BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
      this.WellPressurizing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        OilWellCap.StatesInstance statesInstance = (OilWellCap.StatesInstance) data;
        if (statesInstance != null)
          return string.Format(str, (object) GameUtil.GetFormattedPercent(100f * statesInstance.GetPressurePercent(), GameUtil.TimeSlice.None));
        return str;
      });
      this.WellOverpressure = this.CreateStatusItem("WellOverpressure", (string) BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME, (string) BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP, string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.ReleasingPressure = this.CreateStatusItem("ReleasingPressure", (string) BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME, (string) BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
      this.TooCold = this.CreateStatusItem("TooCold", (string) BUILDING.STATUSITEMS.TOO_COLD.NAME, (string) BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP, string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.IncubatorProgress = this.CreateStatusItem("IncubatorProgress", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.IncubatorProgress.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        EggIncubator eggIncubator = (EggIncubator) data;
        str = str.Replace("{Percent}", GameUtil.GetFormattedPercent(eggIncubator.GetProgress() * 100f, GameUtil.TimeSlice.None));
        return str;
      });
      this.HabitatNeedsEmptying = this.CreateStatusItem("HabitatNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.DetectorScanning = this.CreateStatusItem("DetectorScanning", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.IncomingMeteors = this.CreateStatusItem("IncomingMeteors", "BUILDING", string.Empty, StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.HasGantry = this.CreateStatusItem("HasGantry", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.MissingGantry = this.CreateStatusItem("MissingGantry", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.DisembarkingDuplicant = this.CreateStatusItem("DisembarkingDuplicant", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.RocketName = this.CreateStatusItem("RocketName", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
      this.RocketName.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        RocketModule rocketModule = (RocketModule) data;
        if ((UnityEngine.Object) rocketModule != (UnityEngine.Object) null)
          return str.Replace("{0}", rocketModule.GetParentRocketName());
        return str;
      });
      this.RocketName.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        RocketModule rocketModule = (RocketModule) data;
        if ((UnityEngine.Object) rocketModule != (UnityEngine.Object) null)
          return str.Replace("{0}", rocketModule.GetParentRocketName());
        return str;
      });
      this.PathNotClear = new StatusItem("PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.PathNotClear.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        ConditionFlightPathIsClear flightPathIsClear = (ConditionFlightPathIsClear) data;
        if (flightPathIsClear != null)
          str = string.Format(str, (object) flightPathIsClear.GetObstruction());
        return str;
      });
      this.InvalidPortOverlap = this.CreateStatusItem("InvalidPortOverlap", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
      this.InvalidPortOverlap.AddNotification((string) null, (string) null, (string) null, 0.0f);
      this.EmergencyPriority = this.CreateStatusItem("EmergencyPriority", (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP, "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
      this.EmergencyPriority.AddNotification((string) null, (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME, (string) BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP, 0.0f);
      this.SkillPointsAvailable = this.CreateStatusItem("SkillPointsAvailable", (string) BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME, (string) BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP, "status_item_jobs", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
      this.Baited = this.CreateStatusItem("Baited", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
      this.Baited.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element elementByName = ElementLoader.FindElementByName(((StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GenericInstance) data).master.baitElement.ToString());
        str = str.Replace("{0}", elementByName.name);
        return str;
      });
      this.Baited.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        Element elementByName = ElementLoader.FindElementByName(((StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GenericInstance) data).master.baitElement.ToString());
        str = str.Replace("{0}", elementByName.name);
        return str;
      });
    }

    private static bool ShowInUtilityOverlay(HashedString mode, object data)
    {
      Transform transform = (Transform) data;
      bool flag = false;
      if (mode == OverlayModes.GasConduits.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.GasVentIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.LiquidConduits.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.LiquidVentIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.Power.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.WireIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.Logic.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag);
      }
      else if (mode == OverlayModes.SolidConveyor.ID)
      {
        Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
        flag = OverlayScreen.SolidConveyorIDs.Contains(prefabTag);
      }
      return flag;
    }
  }
}
