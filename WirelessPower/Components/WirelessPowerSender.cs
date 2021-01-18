using Harmony;
using KSerialization;
using UnityEngine;
using WirelessPower.Components.Interfaces;
using WirelessPower.Configuration;
using WirelessPower.Manager;

namespace WirelessPower.Components
{
    [HarmonyPatch(typeof(EnergyConsumer), "get_WattsNeededWhenActive")]
    public static class WirelessPowerSender_WattsNeededWhenActive_Patch
    {
        public static void Postfix(ref EnergyConsumer __instance, ref float __result)
        {
            if (__instance != null && __instance.GetComponent<WirelessPowerSender>() != null) 
                __result = __instance.GetComponent<WirelessPowerSender>().JoulesToTransfer;
        }
    }

    public class WirelessPowerSender : EnergyConsumer, IWirelessTransferer, ISim1000ms
    {
        private MeterController screen;
        public new float BaseWattageRating { get { return this.JoulesToTransfer; } }
        public new float WattsNeededWhenActive => this.BaseWattageRating;
        public new float BaseWattsNeededWhenActive => this.BaseWattageRating;

        [Serialize]
        private float _wirelessBatteryThreshold = 0.95f;
        public float WirelessBatteryThreshold { get { return this._wirelessBatteryThreshold; } set { if (value >= 0.01f && value <= 1f) this._wirelessBatteryThreshold = value; } }

        [Serialize]
        private float _joulesToTransfer = WirelessPowerConfigChecker.DefaultTransfer;
        public float JoulesToTransfer { get { return this._joulesToTransfer; } set { this.UpdateJoulesToTransfer(value); } }
        public float FalloffJoulesToTransfer => WirelessPowerBattery.CalculateEnergyAfterFalloff(this._joulesToTransfer);

        public bool IsOperational => this.operational.IsActive;

        public bool IsConnectedToGrid => this.operational.GetFlag(WirelessPowerPatches.WirelessConnectionFlag);

        [Serialize]
        private int _channel = 1;
        public int Channel { get { return this._channel; } set { UpdateChannel(value); } }

        public void UpdateJoulesToTransfer(float joules)
        {
            if (joules >= WirelessPowerConfigChecker.MinTransfer && joules <= WirelessPowerConfigChecker.MaxTransfer)
                this._joulesToTransfer = joules;
        }

        public void UpdateChannel(int newChannel)
        {
            if (newChannel <= 0 || newChannel > WirelessPowerConfigChecker.MaxNumberOfChannels)
                newChannel = 1;
            bool success = (bool)WirelessPowerGrid.Instance?.ChangeSenderChannel(this, newChannel);
            if (!success)
                return;
            this._channel = newChannel;
            this.OnConnectionChanged();
        }

        public void OnConnectionChanged() => this.operational.SetFlag(WirelessPowerPatches.WirelessConnectionFlag, (bool)WirelessPowerGrid.Instance?.ChannelHasBattery(this._channel));

        public bool ConnectToGrid()
        {
            bool connected = (bool)WirelessPowerGrid.Instance?.RegisterSender(this, this._channel);
            this.OnConnectionChanged();
            return connected;
        }

        public bool DisconnectFromGrid()
        {
            bool connected = (bool)WirelessPowerGrid.Instance?.UnregisterSender(this);
            this.OnConnectionChanged();
            return connected;
        }


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
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

        public override void EnergySim200ms(float dt)
        {
            float falloffTransfer = Mathf.Max(this.FalloffJoulesToTransfer * dt, 1f * dt);
            bool isActive = false;
            if (this.operational.IsOperational && WirelessPowerGrid.Instance.SendEnergyToGrid(this, falloffTransfer, this._wirelessBatteryThreshold, dt))
			{
                isActive = true;
                base.EnergySim200ms(dt);
            }
            this.operational.SetActive(isActive);
        }

        public void Sim1000ms(float dt) => UpdateScreenStatus();

        private void UpdateScreenStatus()
        {
            if (!this.operational.GetFlag(EnergyConsumer.PoweredFlag) || !this.operational.GetFlag(BuildingEnabledButton.EnabledFlag))
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
