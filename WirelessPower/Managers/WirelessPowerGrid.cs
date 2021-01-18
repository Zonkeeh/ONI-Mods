using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using WirelessPower.Components;
using WirelessPower.Components.Interfaces;
using Zolibrary.Logging;

namespace WirelessPower.Manager
{
    internal sealed class WirelessPowerGrid : IDisposable
    {
        private readonly Dictionary<WirelessPowerReceiver, int> receivers;
        private readonly Dictionary<WirelessPowerSender, int> senders;
        private readonly Dictionary<WirelessPowerBattery, int> batteries;
        private readonly Dictionary<int, float> channelCapacity;

        public static WirelessPowerGrid Instance { get; private set; }

        public static void CreateInstance()
        {
            WirelessPowerGrid.DestroyInstance();
            WirelessPowerGrid.Instance = new WirelessPowerGrid();
        }

        public static void DestroyInstance()
        {
            if (WirelessPowerGrid.Instance != null)
                WirelessPowerGrid.Instance.Dispose();
            WirelessPowerGrid.Instance = (WirelessPowerGrid)null;
        }

        public void Dispose() => this.ClearGrid();

        private WirelessPowerGrid()
        {
            this.receivers = new Dictionary<WirelessPowerReceiver, int>();
            this.senders = new Dictionary<WirelessPowerSender, int>();
            this.batteries = new Dictionary<WirelessPowerBattery, int>();
            this.channelCapacity = new Dictionary<int, float>();
        }

        public void ClearGrid()
        {
            this.receivers.Clear();
            this.senders.Clear();
            this.batteries.Clear();
            this.channelCapacity.Clear();
        }

        public float GetChannelJoulesAvailable(int channel)
        {
            float joulesAvailable = 0f;
            foreach (WirelessPowerBattery bat in this.batteries.Keys)
                if (this.batteries[bat] == channel)
                    joulesAvailable += bat.JoulesAvailable;
            return joulesAvailable;
        }

        public float GetChannelCapacity(int channel) => this.channelCapacity.ContainsKey(channel) ? this.channelCapacity[channel] : 0f;

        private void UpdateBatteriesOnChannel(int channel) 
        {
            List<WirelessPowerBattery> registeredBatteries = this.batteries.Where(b => b.Value == channel).Select(b => b.Key).ToList();
            foreach (WirelessPowerBattery b in registeredBatteries) 
                b.OnConnectionChanged();
        }

        private void UpdateOtherOnChannel(int channel)
        {
            List<IWirelessTransferer> registeredDevices = this.receivers.Where(r => r.Value == channel).Select(r => (IWirelessTransferer) r.Key).ToList();
            registeredDevices.AddRange(this.senders.Where(s => s.Value == channel).Select(s => (IWirelessTransferer) s.Key).ToList());
            foreach (IWirelessTransferer d in registeredDevices)
                d.OnConnectionChanged();
        }

        public bool RegisterReceiver(WirelessPowerReceiver receiver, int channel)
        {
            if (receiver == null || channel <= 0)
                return false;
            if (this.receivers.ContainsKey(receiver))
                return false;

            this.receivers.Add(receiver, channel);
            this.UpdateBatteriesOnChannel(channel);
            return true;
        }

        public bool RegisterSender(WirelessPowerSender sender, int channel)
        {
            if (sender == null || channel <= 0)
                return false;
            if (this.senders.ContainsKey(sender))
                return false;
            
            this.senders.Add(sender, channel);
            this.UpdateBatteriesOnChannel(channel);
            return true;
        }

        public bool RegisterBattery(WirelessPowerBattery battery, int channel)
        {
            if (battery == null || channel <= 0)
                return false;
            if (this.batteries.ContainsKey(battery))
                return false;
            
            this.batteries.Add(battery, channel);
            this.channelCapacity[channel] = this.channelCapacity.ContainsKey(channel) ? this.channelCapacity[channel] + battery.Capacity : battery.Capacity;
            this.UpdateOtherOnChannel(channel);
            return true;
        }

        public bool UnregisterReceiver(WirelessPowerReceiver receiver)
        {
            if (!this.receivers.ContainsKey(receiver))
                return false;

            int receiverChannel = this.receivers[receiver];

            if (!this.receivers.Remove(receiver))
                return false;

            this.UpdateBatteriesOnChannel(receiverChannel);
            return true;
        }

        public bool UnregisterSender(WirelessPowerSender sender)
        {
            if (!this.senders.ContainsKey(sender))
                return false;

            int senderChannel = this.senders[sender];
            
            if (!this.senders.Remove(sender))
                return false;

            this.UpdateBatteriesOnChannel(senderChannel);
            return true;
        }

        public bool UnregisterBattery(WirelessPowerBattery battery) 
        {
            if (!this.batteries.ContainsKey(battery))
                return false;

            int batteryChannel = this.batteries[battery];

            if (!this.batteries.Remove(battery)) 
                return false;

            this.channelCapacity[batteryChannel] = this.channelCapacity.ContainsKey(batteryChannel) ? this.channelCapacity[batteryChannel] - battery.Capacity : 0f;
            this.UpdateOtherOnChannel(batteryChannel);
            return true;
        }

        public bool ChangeReceiverChannel(WirelessPowerReceiver receiver, int channel)
        {
            if (!this.receivers.ContainsKey(receiver))
                return false;

            int receiverChannel = this.receivers[receiver];
            this.receivers[receiver] = channel;
            this.UpdateBatteriesOnChannel(receiverChannel);
            return true;
        }

        public bool ChangeSenderChannel(WirelessPowerSender sender, int channel)
        {
            if (!this.senders.ContainsKey(sender))
                return false;

            int senderChannel = this.senders[sender];
            this.senders[sender] = channel;
            this.UpdateBatteriesOnChannel(senderChannel);
            return true;
        }

        public bool ChangeBatteryChannel(WirelessPowerBattery battery, int channel)
        {
            if (!this.batteries.ContainsKey(battery))
                return false;

            int batteryChannel = this.batteries[battery];
            this.batteries[battery] = channel;
            this.channelCapacity[channel] = this.channelCapacity.ContainsKey(channel) ? this.channelCapacity[channel] - battery.Capacity : 0f;
            this.channelCapacity[batteryChannel] = this.channelCapacity.ContainsKey(batteryChannel) ? this.channelCapacity[batteryChannel] + battery.Capacity : battery.Capacity;
            this.UpdateOtherOnChannel(batteryChannel);
            return true;
        }

        public bool ChannelHasSenderReceiver(int channel) => this.receivers.ContainsValue(channel) || this.senders.ContainsValue(channel);

        public bool ChannelHasBattery(int channel) => this.batteries.ContainsValue(channel);

        public bool SendEnergyToGrid(WirelessPowerSender sender, float joules, float percentFull, float dt)
        {
            if (!this.senders.ContainsKey(sender))
                return false;

            //check if batteries have free capacity;
            int channel = this.senders[sender];
            Dictionary<WirelessPowerBattery, float> energyToGive = this.GridHasCapacity(joules, channel, percentFull);
            if (energyToGive.Count == 0)
			    return false;

            //Send energy to batteries;
            foreach (WirelessPowerBattery b in energyToGive.Keys)
                b.AddEnergy(energyToGive[b], dt);
            return true;
        }

        public bool ReceiveEnergyFromGrid(WirelessPowerReceiver receiver, float joules, float percentFull, float dt) 
        {
            if (!this.receivers.ContainsKey(receiver))
                return false;

            //check if batteries have energy;
            int channel = this.receivers[receiver];
            Dictionary<WirelessPowerBattery, float> energyToTake = this.GridHasCapacity(-joules, channel, percentFull);
            if (energyToTake.Count == 0)
                return false;

            //Take energy from batteries;
            foreach (WirelessPowerBattery b in energyToTake.Keys)
                b.ConsumeEnergy(energyToTake[b], dt);
            return true;
        }

        private Dictionary<WirelessPowerBattery, float> GridHasCapacity(float joules, int channel, float percentFull) 
        {
            if(!this.batteries.ContainsValue(channel))
                return new Dictionary<WirelessPowerBattery, float>();

            bool isSendingToGrid = joules > 0f;

            //Get highest capacity when adding power, lowest when taking power.
            List<WirelessPowerBattery> sortedBatteries = joules >= 0 
                ? this.batteries.Where(b => b.Value == channel).OrderByDescending(b => b.Key.JoulesAvailable).Select(b => b.Key).ToList() 
                : this.batteries.OrderBy(b => b.Key.JoulesAvailable).Select(b => b.Key).ToList();
            Dictionary<WirelessPowerBattery, float> energyChanges = new Dictionary<WirelessPowerBattery, float>();
            float remaining = Math.Abs(joules);
            foreach (WirelessPowerBattery b in sortedBatteries)
            {
                //If receiving power from the grid and battery is non-operational: skip
                if (!b.IsOperational && !isSendingToGrid)
                    continue;
                //If sending power to the grid and battery is non-operational more than just joules available: skip
                else if (!b.IsOperational && isSendingToGrid && !b.JustNoJoulesAvailable())
                    continue;
                //If receiving power from the grid and battery is below the threshold: skip
                else if (!isSendingToGrid && b.PercentFull <= percentFull)
                    continue;
                //If sending power to the grid and battery is above the threshold: skip
                else if (isSendingToGrid && b.PercentFull >= percentFull)
                    continue;
                else
				{
                    float energyChange = joules >= 0.0f ? Math.Min(remaining, (b.Capacity - b.JoulesAvailable)) : Math.Min(remaining, b.JoulesAvailable);
                    remaining -= energyChange;
                    energyChanges.Add(b, energyChange);
                    if (remaining <= 0.0f)
                        return energyChanges;
                }
            }
            return new Dictionary<WirelessPowerBattery, float>();
        }


        [HarmonyPatch(typeof(EnergySim), nameof(EnergySim.EnergySim200ms))]
        public class EnergySim_Patch
        {
            public static void Prefix(float dt)
            {
                foreach (WirelessPowerBattery battery in WirelessPowerGrid.Instance?.batteries.Keys)
                    battery.EnergySim200ms(dt);
            }
        }
    }
}
