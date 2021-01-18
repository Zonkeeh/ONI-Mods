using Harmony;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using WirelessPower.Components.Interfaces;
using WirelessPower.Configuration;
using WirelessPower.Manager;
using Zolibrary.Logging;

namespace WirelessPower.Components
{
    [HarmonyPatch(typeof(Generator), "get_WattageRating")]
    public static class WirelessPowerReceiver_WattageRating_Patch
    {
        public static void Postfix(ref Generator __instance, ref float __result)
        {
            if (__instance is WirelessPowerReceiver)
                __result = ((WirelessPowerReceiver)__instance).FalloffJoulesToTransfer;
        }
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public class WirelessPowerReceiver : Generator, IWirelessTransferer, ISim1000ms
    {
        private bool ignoreBatteryRefillPercent = false;
        private MeterController screen;
        private static StatusItem batteriesSufficientlyFull;
        public new virtual float WattageRating => this.FalloffJoulesToTransfer;

        [Serialize]
        private float _wirelessBatteryThreshold = 0f;
        public float WirelessBatteryThreshold { get { return this._wirelessBatteryThreshold; } set { if (value >= 0f && value <= 0.99f) this._wirelessBatteryThreshold = value; } }

        [Serialize]
        private float _batteryRefillPercent = 0.95f;
        public float BatteryRefillPercent { get { return this._batteryRefillPercent; } set { if (value >= 0.01f && value <= 1f) this._batteryRefillPercent = value; } }

        [Serialize]
        private float _joulesToTransfer = WirelessPowerConfigChecker.DefaultTransfer;
        public float JoulesToTransfer { get { return this._joulesToTransfer; } set { this.UpdateJoulesToTransfer(value); } }
        public float FalloffJoulesToTransfer => WirelessPowerBattery.CalculateEnergyAfterFalloff(this._joulesToTransfer);

        public bool IsOperational => this.operational.IsActive;

        public bool IsConnectedToGrid => this.operational.GetFlag(WirelessPowerPatches.WirelessConnectionFlag);

        [Serialize]
        private int _channel = 1;
        public int Channel { get { return this._channel; } set { this.UpdateChannel(value); } }

        public void UpdateJoulesToTransfer(float joules)
        {
            if (joules >= WirelessPowerConfigChecker.MinTransfer && joules <= WirelessPowerConfigChecker.MaxTransfer)
                this._joulesToTransfer = joules;
        }

        public void UpdateChannel(int newChannel)
        {
            if (newChannel <= 0 || newChannel > WirelessPowerConfigChecker.MaxNumberOfChannels)
                newChannel = 1;
            bool success = (bool)WirelessPowerGrid.Instance?.ChangeReceiverChannel(this, newChannel);
            if (!success) 
                return;
            this._channel = newChannel;
            this.OnConnectionChanged();
        }

        public void OnConnectionChanged() => this.operational.SetFlag(WirelessPowerPatches.WirelessConnectionFlag, (bool) WirelessPowerGrid.Instance?.ChannelHasBattery(this._channel));

        public bool ConnectToGrid()
        {
            bool connected = (bool) WirelessPowerGrid.Instance?.RegisterReceiver(this, this._channel);
            this.OnConnectionChanged();
            return connected;
        }

        public bool DisconnectFromGrid()
        {
            bool connected = (bool) WirelessPowerGrid.Instance?.UnregisterReceiver(this);
            this.OnConnectionChanged();
            return connected;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            if (WirelessPowerReceiver.batteriesSufficientlyFull != null)
                return;
            WirelessPowerReceiver.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.screen = new MeterController(this.GetComponent<KBatchedAnimController>(), "screen_target", "screen_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[5]
                {
                    "screen_target",
                    "screen_fill",
                    "screen_status_0",
                    "screen_status_1",
                    "screen_status_2",
                });
            this.ConnectToGrid();
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.ConnectionStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.ChannelStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.GridCapacityStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.TransferStatus, this);
        }

        protected override void OnCleanUp()
        {
            this.DisconnectFromGrid();
            base.OnCleanUp();
        }

        private bool HasCapacityOnCircuit() 
        {
            List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(this.CircuitID);
            if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
            {
                foreach (Battery battery in batteriesOnCircuit)
                    if ((double)this._batteryRefillPercent <= 0.0 && (double)battery.PercentFull <= 0.0)
                        return true;
                    else if ((double)battery.PercentFull < (double)this._batteryRefillPercent)
                        return true;

                return false;
            }
            else
                return true;
        }

        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);
            bool isActive = false;
            ushort circuitId = this.CircuitID;
            this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
            if (this.operational.IsOperational)
            {
                float toReceive = Mathf.Max(this._joulesToTransfer * dt, 1f * dt);
                bool hasCircuitCapacity = this.HasCapacityOnCircuit();
                if (!hasCircuitCapacity)
                    this.selectable.ToggleStatusItem(WirelessPowerReceiver.batteriesSufficientlyFull, !hasCircuitCapacity, (object)null);
                else if (WirelessPowerGrid.Instance.ReceiveEnergyFromGrid(this, toReceive, this._wirelessBatteryThreshold, dt))
                {
                    this.GenerateJoules(Mathf.Max(this.FalloffJoulesToTransfer * dt, 1f * dt), false);
                    isActive = true;
                }
                this.operational.SetActive(isActive);
            }
        }

        public void Sim1000ms(float dt) => UpdateScreenStatus();

        private void UpdateScreenStatus()
        {
            if (!this.operational.GetFlag(WirelessPowerReceiver.generatorConnectedFlag) || !this.operational.GetFlag(BuildingEnabledButton.EnabledFlag))
                this.screen.SetPositionPercent(0f);
            else if (!this.IsConnectedToGrid)
                this.screen.SetPositionPercent(0.25f);
            else if (!this.IsOperational)
                this.screen.SetPositionPercent(0.5f);
            else if (this.IsOperational)
                this.screen.SetPositionPercent(0.75f); 
        }
	}

}
