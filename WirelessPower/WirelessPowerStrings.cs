using Harmony;
using STRINGS;
using System;
using WirelessPower.Components;
using WirelessPower.Components.Interfaces;
using WirelessPower.Configuration;
using WirelessPower.Manager;
using Zolibrary.Logging;

namespace WirelessPower
{
    public static class WirelessPowerStrings
    {
        public static StatusItem ConnectionStatus;
        public static StatusItem ChannelStatus;
        public static StatusItem TransferStatus;
        public static StatusItem BatteryCapacityStatus;
        public static StatusItem GridStatus;
        public static StatusItem GridCapacityStatus;
        public static StatusItem BatteryNetStatus;

        private enum StringColour
        {
            Red,
            Green,
            Blue,
            Yellow,
            Power,
            None
        }

        private static string FormatColour(string toColour, StringColour colour = StringColour.Yellow)
        {
            if(!WirelessPowerConfigChecker.UseColourInStatusItems)
                return "<b>" + toColour + "</b>";

            switch (colour) 
            {
                case StringColour.Red: return UI.FormatAsAutomationState(toColour, UI.AutomationState.Standby);
                case StringColour.Green: return UI.FormatAsAutomationState(toColour, UI.AutomationState.Active);
                case StringColour.Blue: return UI.FormatAsPositiveRate(toColour);
                case StringColour.Yellow: return UI.FormatAsKeyWord(toColour);
                case StringColour.None: return toColour;
                default: return toColour;
            }
        }

        public static string ConnectionStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.CONNECTION";
        public static LocString ConnectionStatusName = (LocString)"Connection Status: {Status}";
        public static LocString ConnectionStatusTooltip = (LocString)("{Explanation}");

        public static string ChannelStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.CHANNEL";
        public static LocString ChannelStatusName = (LocString)"Connected Channel: {Channel}";
        public static LocString ChannelStatusTooltip = (LocString)("{Explanation}");

        public static string GridCapacityStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.GRIDCAPACITY";
        public static LocString GridCapacityStatusName = (LocString)"Grid Power Available: {Available} / {Capacity}  {Percentage}";
        public static LocString GridCapacityStatusTooltip = (LocString)("Grid Channel is currently storing <b>{Available}</b> of <b>{Capacity}</b> available " + FormatColour("power") + " for use within the " + FormatColour("Wireless Power Grid") + ".");

        public static string TransferStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.TRANSFER";
        public static LocString TransferStatusName = (LocString)"{Status}";
        public static LocString TransferStatusTooltip = (LocString)("{Explanation}");

        public static string BatteryCapacityStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.BATTERYCAPACITY";
        public static LocString BatteryCapacityStatusName = (LocString)"Battery Power Available: {Available} / {Capacity}  {Percentage}";
        public static LocString BatteryCapacityStatusTooltip = (LocString)("Stores <b>{Available}</b> of " + FormatColour("Power") + " for use within the " + FormatColour("Wireless Power Grid") + ".");

        public static string GridStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.GRIDSTATUS";
        public static LocString GridStatusName = (LocString)"Grid Status: {Status}";
        public static LocString GridStatusTooltip = (LocString)("The " + FormatColour("Wireless Power Grid") + " is currently {Explanation} {End}.");

        public static string BatteryNetStatusKey = (LocString)"STRINGS.BUILDING.STATUSITEMS.WIRELESSPOWER.BATTERYNET";
        public static LocString BatteryNetStatusName = (LocString)"Battery Net Gain: {Last}";
        public static LocString BatteryNetStatusTooltip = (LocString)("Displays how much " + FormatColour("Power") + " was net gained/lost by this battery in the last transfer cycle.");

        public static string SideScreenKey = (LocString)"STRINGS.UI.UISIDESCREENS.WIRELESSPOWER";
        public static LocString SideScreenTitleText = (LocString)"Wireless Channel Configuration";

        public static string ChannelTitleKey = (LocString)SideScreenKey + ".CHANNELLABEL";
        public static LocString ChannelTitleName = (LocString)"Channel:";
        public static LocString ChannelTitleTooltip = (LocString)("Select a channel for this device to operate on the " + FormatColour("Wireless Power Grid") + ".");

        public static string JoulesToTransferTitleKey = (LocString)SideScreenKey + ".TRANSFERLABEL";
        public static LocString JoulesToTransferTitleName = (LocString)"Transfer Amount:";
        public static LocString JoulesToTransferTitleTooltip = (LocString)("Select how much " + UI.FormatAsLink("Power", "POWER") + " is transfered to/from the " + FormatColour("Wireless Power Grid") + ".");

        public static string WirelessBatteryThresholdTitleKey = (LocString)SideScreenKey + ".WIRELESSTHRESHOLDLABEL";
        public static LocString WirelessBatteryThresholdTitleName = (LocString)"Grid Transfer Threshold:";
        public static LocString WirelessBatteryThresholdTitleTooltip = (LocString)("Select the percentage needed to refill/take from the " + FormatColour("Wireless Batteries") + " on the connected " + FormatColour("Wireless Power Grid") + ".");

        public static string InternalBatteryThresholdTitleKey = (LocString)SideScreenKey + ".INTERNALTHRESHOLDLABEL";
        public static LocString InternalBatteryThresholdTitleName = (LocString)"Battery Refill Threshold:";
        public static LocString InternalBatteryThresholdTitleTooltip = (LocString)("Select the percentage to refill batteries to on this receiver's current power curcuit.");

        public static string WirelessPowerBatteryID = "WirelessPowerBattery";
        public static string WirelessPowerBatteryName = "Wireless Battery";
        public static string WirelessPowerBatteryEffect = "Stores " + UI.FormatAsLink("Power", "POWER") + " for later access on the Wireless Power Grid.\n\nLoses " + UI.FormatAsLink("power", "POWER") + " over time.";
        public static string WirelessPowerBatteryDescription = "Wireless Batteries have many states of operation outlined below:" +
            "\n\n<b>Online:</b> Denoted by green, this state indicates the building is connected and is actively sending or receiving power." +
            "\n\n<b>Standby:</b> Denoted by yellow, this state indicates the building is connected and awaiting power transfer." +
            "\n\n<b>Offline:</b> Denoted by red, this indicates the building is disconnected from the wirless grid (no wireless senders/receivers exist on the specified channel).";

        public static string WirelessPowerReceiverID = "WirelessPowerReceiver";
        public static string WirelessPowerReceiverName = "Wireless Receiver";
        public static string WirelessPowerReceiverEffect = "Fetches " + UI.FormatAsLink("Power", "POWER") + " from wireless batteries on the Wireless Power Grid.\n\nInputs the specified amount of " + UI.FormatAsLink("power", "POWER") + " into the connected circuit.";
        public static string WirelessPowerReceiverDescription = WirelessPowerReceiverName + "s have many states of operation outlined below:" +
            "\n\n<b>Online:</b> Denoted by green, this state indicates the building is connected and receiving power." +
            "\n\n<b>Standby:</b> Denoted by yellow, this state indicates the building is connected and awaiting power transfer. This occurs when either all wireless batteries on the specified channel are empty or all batteries on the connected circuit are at their capacity thresholds." +
            "\n\n<b>Offline:</b> Denoted by red, this indicates the building is disconnected from the wirless grid (no wireless batteries exist on the specified channel).";

        public static string WirelessPowerSenderID = "WirelessPowerSender";
        public static string WirelessPowerSenderName = "Wireless Sender";
        public static string WirelessPowerSenderEffect = "Sends " + UI.FormatAsLink("Power", "POWER") + " to wireless batteries on the same channel of the Wireless Power Grid" + ".\n\n" + UI.FormatAsLink("Power", "POWER") + " is sent from the current circuit in the amount specified. ";
        public static string WirelessPowerSenderDescription = WirelessPowerSenderName + "s have many states of operation outlined below:" +
            "\n\n<b>Online:</b> Denoted by green, this state indicates the building is connected and sending power." +
            "\n\n<b>Standby:</b> Denoted by yellow, this state indicates the building is connected and awaiting power transfer (all connected batteries are full)." +
            "\n\n<b>Offline:</b> Denoted by red, this indicates the building is disconnected from the wirless grid (no wireless batteries exist on the specified channel).";

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                AddStrings();
            }
        }

        private static void AddStrings()
        {
            Strings.Add(SideScreenKey + ".TITLE", SideScreenTitleText);
            Strings.Add(ChannelTitleKey + ".NAME", ChannelTitleName);
            Strings.Add(ChannelTitleKey + ".TOOLTIP", ChannelTitleTooltip);
            Strings.Add(JoulesToTransferTitleKey + ".NAME", JoulesToTransferTitleName);
            Strings.Add(JoulesToTransferTitleKey + ".TOOLTIP", JoulesToTransferTitleTooltip);
            Strings.Add(WirelessBatteryThresholdTitleKey + ".NAME", WirelessBatteryThresholdTitleName);
            Strings.Add(WirelessBatteryThresholdTitleKey + ".TOOLTIP", WirelessBatteryThresholdTitleTooltip);
            Strings.Add(InternalBatteryThresholdTitleKey + ".NAME", InternalBatteryThresholdTitleName);
            Strings.Add(InternalBatteryThresholdTitleKey + ".TOOLTIP", InternalBatteryThresholdTitleTooltip);


            Strings.Add(ConnectionStatusKey + ".NAME", ConnectionStatusName);
            Strings.Add(ConnectionStatusKey + ".TOOLTIP", ConnectionStatusTooltip);

            WirelessPowerStrings.ConnectionStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.Connection",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.ConnectionStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (IWirelessConnecter)data == null)
                    return str;

                IWirelessConnecter wirelessConnector = (IWirelessConnecter)data;
                bool isConnected = wirelessConnector.IsConnectedToGrid;

                if (str.Contains("{Status}"))
                    str = !isConnected ? str.Replace("{Status}", FormatColour("Offline", StringColour.Red)) : str.Replace("{Status}", FormatColour("Online", StringColour.Green));
                else if (str.Contains("{Explanation}"))
                {
                    if (data is WirelessPowerSender && isConnected)
                        str = str.Replace("{Explanation}", "Sender is connected to the " + FormatColour("Wireless Power Grid") + ".");
                    else if (data is WirelessPowerSender)
                        str = str.Replace("{Explanation}", "Sender is disconnected and non-operational.\n\nBuild a <b>" + FormatColour(WirelessPowerStrings.WirelessPowerBatteryName) + "</b> to make this sender operational.");
                    else if (data is WirelessPowerReceiver && isConnected)
                        str = str.Replace("{Explanation}", "Receiver is connected to the " + FormatColour("Wireless Power Grid") + ".");
                    else if (data is WirelessPowerReceiver)
                        str = str.Replace("{Explanation}", "Receiver is disconnected and non-operational.\n\nBuild a <b>" + FormatColour(WirelessPowerStrings.WirelessPowerBatteryName) + "</b> to make this receiver operational.");
                    else if (data is WirelessPowerBattery && isConnected)
                        str = str.Replace("{Explanation}", "Battery is connected to the " + FormatColour("Wireless Power Grid") + ".");
                    else if (data is WirelessPowerBattery)
                        str = str.Replace("{Explanation}", "Battery is disconnected and non-operational.\n\nBuild either a <b>" + FormatColour(WirelessPowerStrings.WirelessPowerReceiverName) + "</b> or <b>" + FormatColour(WirelessPowerStrings.WirelessPowerSenderName) + "</b> to make this battery operational.");

                }

                return str;
            });


            Strings.Add(ChannelStatusKey + ".NAME", ChannelStatusName);
            Strings.Add(ChannelStatusKey + ".TOOLTIP", ChannelStatusTooltip);

            WirelessPowerStrings.ChannelStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.Channel",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.ChannelStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (IWirelessConnecter)data == null)
                    return str;

                IWirelessConnecter wirelessConnector = (IWirelessConnecter)data;
                int channel = wirelessConnector.Channel;

                if (str.Contains("{Channel}"))
                    str = str.Replace("{Channel}", FormatColour(channel.ToString()));
                else if (str.Contains("{Explanation}"))
                    str = str.Replace("{Explanation}", "This building is currently connected to channel " + FormatColour(channel.ToString()) + " on the " + FormatColour("Wireless Power Grid") + ".");

                return str;
            });


            Strings.Add(TransferStatusKey + ".NAME", TransferStatusName);
            Strings.Add(TransferStatusKey + ".TOOLTIP", TransferStatusTooltip);

            WirelessPowerStrings.TransferStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.Transfer",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.TransferStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (IWirelessTransferer)data == null)
                    return str;

                IWirelessTransferer trans = (IWirelessTransferer)data;

                string setJoules = GameUtil.GetFormattedWattage(trans.JoulesToTransfer);
                string falloffJoules = GameUtil.GetFormattedWattage(trans.FalloffJoulesToTransfer);
                string falloffAmount = !WirelessPowerConfigChecker.UseEnergyFalloff ? FormatColour(falloffJoules, StringColour.Blue) : FormatColour(falloffJoules, StringColour.Blue) + " (" + setJoules + ")";
                string falloffExplanation = !WirelessPowerConfigChecker.UseEnergyFalloff ? "" : ", due to wireless energy falloff,";

                string actualPowerTransfer = trans.IsConnectedToGrid
                                            ? (trans.IsOperational ? FormatColour(falloffJoules, StringColour.Green) : FormatColour("Standby"))
                                            : FormatColour("N/A", StringColour.Red);


                if (data is WirelessPowerSender)
                {
                    if (str.Contains("{Status}"))
                        str = str.Replace("{Status}", "Sending Power: " + actualPowerTransfer);
                    else if (str.Contains("{Explanation}") && trans.IsOperational)
                        str = str.Replace("{Explanation}", "Sender is beaming " + falloffAmount + " of " + FormatColour("Power") + falloffExplanation + " to the " + FormatColour("Wireless Power Grid") + ".");
                    else if (str.Contains("{Explanation}"))
                        str = str.Replace("{Explanation}", "Sender is non-operational.\n\nOnce operational: it will send " + falloffAmount + " of " + FormatColour("Power") + falloffExplanation + " from the " + FormatColour("Wireless Power Grid") + ".");
                }
                else if (data is WirelessPowerReceiver)
                {
                    if (str.Contains("{Status}"))
                        str = str.Replace("{Status}", "Receiving Power: " + actualPowerTransfer);
                    else if (data is WirelessPowerReceiver && str.Contains("{Explanation}") && trans.IsOperational)
                        str = str.Replace("{Explanation}", "Receiver is drawing " + falloffAmount + " of " + FormatColour("Power") + falloffExplanation + " from the " + FormatColour("Wireless Power Grid") + ".");
                    else if (data is WirelessPowerReceiver && str.Contains("{Explanation}"))
                        str = str.Replace("{Explanation}", "Receiver is non-operational.\n\nOnce operational: it will draw " + falloffAmount + " of " + FormatColour("Power") + falloffExplanation + " from the " + FormatColour("Wireless Power Grid") + ".");
                }
                return str;
            });


            Strings.Add(BatteryCapacityStatusKey + ".NAME", BatteryCapacityStatusName);
            Strings.Add(BatteryCapacityStatusKey + ".TOOLTIP", BatteryCapacityStatusTooltip);

            WirelessPowerStrings.BatteryCapacityStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.BatteryCapacity",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.BatteryCapacityStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (WirelessPowerBattery)data == null)
                    return str;

                WirelessPowerBattery bat = (WirelessPowerBattery)data;

                str = str.Replace("{Available}", GameUtil.GetFormattedJoules(bat.JoulesAvailable));
                if (str.Contains("{Percentage}"))
                {
                    str = str.Replace("{Capacity}", GameUtil.GetFormattedJoules(bat.Capacity)).Replace("{Percentage}", "(" + Util.FormatWholeNumber(bat.PercentFull * 100) + "%)");
                    str = str.Split(':')[0] + ":" + FormatColour(str.Split(':')[1]);
                }

                return str;
            });


            Strings.Add(BatteryNetStatusKey + ".NAME", BatteryNetStatusName);
            Strings.Add(BatteryNetStatusKey + ".TOOLTIP", BatteryNetStatusTooltip);

            WirelessPowerStrings.BatteryNetStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.BatteryNet",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.BatteryNetStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (WirelessPowerBattery)data == null)
                    return str;

                WirelessPowerBattery bat = (WirelessPowerBattery)data;
                float net = bat.CalculateNetTotal();
                if (str.Contains("{Last}")) 
                    str = str.Replace("{Last}", net >= 0f 
                        ? FormatColour(" + " + GameUtil.GetFormattedJoules(net), StringColour.Green) 
                        : FormatColour(" - " + GameUtil.GetFormattedJoules(Math.Abs(net)), StringColour.Red));
                return str;
            });


            Strings.Add(GridStatusKey + ".NAME", GridStatusName);
            Strings.Add(GridStatusKey + ".TOOLTIP", GridStatusTooltip);

            WirelessPowerStrings.GridStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.GridStatus",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.GridStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (WirelessPowerBattery)data == null)
                    return str;

                WirelessPowerBattery bat = (WirelessPowerBattery)data;

                if (!bat.IsConnectedToGrid)
                {
                    if (str.Contains("{Status}")) str = str.Replace("{Status}", FormatColour("Offline", StringColour.Red));
                    else if (str.Contains("{Explanation}")) str = str.Replace("{Explanation}", FormatColour("Offline", StringColour.Red)).Replace("{End}", "for this battery");
                }
                else if (bat.LastSent > 0f && bat.LastReceived > 0f)
                {
                    if (str.Contains("{Status}")) str = str.Replace("{Status}", FormatColour("Sending & Receiving", StringColour.Green));
                    else if (str.Contains("{Explanation}")) str = str.Replace("{Explanation}", "both " + FormatColour("Sending & Receiving", StringColour.Green)).Replace("{End}", "energy to/from this battery");
                }
                else if (bat.LastSent > 0f)
                {
                    if (str.Contains("{Status}")) str = str.Replace("{Status}", FormatColour("Sending", StringColour.Green));
                    else if (str.Contains("{Explanation}")) str = str.Replace("{Explanation}", FormatColour("Sending", StringColour.Green)).Replace("{End}", "energy to this battery");
                }
                else if (bat.LastReceived > 0f)
                {
                    if (str.Contains("{Status}")) str = str.Replace("{Status}", FormatColour("Receiving", StringColour.Green));
                    else if (str.Contains("{Explanation}")) str = str.Replace("{Explanation}", FormatColour("Receiving", StringColour.Green)).Replace("{End}", "energy from this battery");
                }
                else
                {
                    if (str.Contains("{Status}")) str = str.Replace("{Status}", FormatColour("Standby"));
                    else if (str.Contains("{Explanation}")) str = str.Replace("{Explanation}", FormatColour("Standby")).Replace("{End}", "for this battery to be needed");
                }

                return str;
            });


            Strings.Add(GridCapacityStatusKey + ".NAME", GridCapacityStatusName);
            Strings.Add(GridCapacityStatusKey + ".TOOLTIP", GridCapacityStatusTooltip);

            WirelessPowerStrings.GridCapacityStatus = (StatusItem)Traverse.Create(Db.Get().BuildingStatusItems).Method("CreateStatusItem", new object[] {
                "WirelessPower.GridCapacity",
                "BUILDING",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.Power.ID,
                true,
                129022
            }).GetValue();

            WirelessPowerStrings.GridCapacityStatus.resolveStringCallback = (Func<string, object, string>)((str, data) =>
            {
                if ((UnityEngine.Object)data == null || (IWirelessConnecter)data == null || WirelessPowerGrid.Instance == null)
                    return str;

                IWirelessConnecter wireless = (IWirelessConnecter)data;

                float channelAvailable = WirelessPowerGrid.Instance.GetChannelJoulesAvailable(wireless.Channel);
                float channelCapacity = WirelessPowerGrid.Instance.GetChannelCapacity(wireless.Channel);

                str = str.Replace("{Available}", GameUtil.GetFormattedJoules(channelAvailable));
                str = str.Replace("{Capacity}", GameUtil.GetFormattedJoules(channelCapacity));
                if (str.Contains("{Percentage}"))
                {
                    str = str.Replace("{Percentage}", "(" + Util.FormatWholeNumber((channelAvailable / channelCapacity) * 100) + "%)");
                    str = str.Split(':')[0] + ":" + FormatColour(str.Split(':')[1]);
                }

                return str;
            });
        }
    }
}
