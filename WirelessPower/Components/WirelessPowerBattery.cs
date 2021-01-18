using KSerialization;
using STRINGS;
using UnityEngine;
using WirelessPower.Components.Interfaces;
using WirelessPower.Components.Sidescreens;
using WirelessPower.Configuration;
using WirelessPower.Manager;
using Zolibrary.Logging;

namespace WirelessPower.Components
{
	public class WirelessPowerBattery : KMonoBehaviour, IWirelessConnecter, ISim1000ms
    {
        [SerializeField]
        private float capacity = WirelessPowerConfigChecker.BatteryCapacity;
        [SerializeField]
        public float chargeWattage = float.PositiveInfinity;
        [Serialize]
        private float joulesAvailable;
        [MyCmpGet]
        protected Operational operational;
        private MeterController meter;
        private MeterController screen;
        private MeterController light;
        public float joulesLostPerSecond = WirelessPowerConfigChecker.BatteryJoulesLostPerSecond;
        private float PreviousJoulesAvailable;
        private float dt;
        private float joulesConsumed = 0.0f;

        public static Operational.Flag JoulesAvailableFlag;

        public bool IsOperational => this.operational.IsActive;

        public bool IsConnectedToGrid => this.operational.GetFlag(WirelessPowerPatches.WirelessConnectionFlag);

        [Serialize]
        private int _channel = 1;
        public int Channel { get { return this._channel; } set { UpdateChannel(value); } }

        public void UpdateChannel(int newChannel)
        {
            if (newChannel <= 0 || newChannel > WirelessPowerConfigChecker.MaxNumberOfChannels)
                newChannel = 1;
            bool success = (bool)WirelessPowerGrid.Instance?.ChangeBatteryChannel(this, newChannel);
            if (!success)
                return;
            this._channel = newChannel;
            this.OnConnectionChanged();
        }

        private float _lastSent = 0f;
        public float LastSent => this._lastSent;

        private float _lastReceived = 0f;
        public float LastReceived => this._lastReceived;

        public float WattsUsed { get; private set; }

        public float PercentFull => this.joulesAvailable / this.capacity;

        public float PreviousPercentFull => this.PreviousJoulesAvailable / this.capacity;

        public float JoulesAvailable => this.joulesAvailable;

        public float Capacity => this.capacity;

        public float ChargeCapacity { get; private set; }

        public void OnConnectionChanged() => this.operational.SetFlag(WirelessPowerPatches.WirelessConnectionFlag, (bool)WirelessPowerGrid.Instance?.ChannelHasSenderReceiver(this._channel));

        public bool ConnectToGrid()
        {
            bool connected = (bool) WirelessPowerGrid.Instance?.RegisterBattery(this, this._channel);
            this.OnConnectionChanged();
            return connected;
        }

        public bool DisconnectFromGrid()
        {
            bool connected = (bool) WirelessPowerGrid.Instance?.UnregisterBattery(this);
            this.OnConnectionChanged();
            return connected;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            //this.Subscribe<WirelessBattery>(-592767678, WirelessBattery.OnOperationalChangedDelegate);
            this.meter = new MeterController(this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[17]
                {
                    "meter_target",
                    "meter_frame",
                    "meter_level_0",
                    "meter_level_1",
                    "meter_level_2",
                    "meter_level_3",
                    "meter_level_4",
                    "meter_level_5",
                    "meter_level_6",
                    "meter_level_7",
                    "meter_level_8",
                    "meter_level_9",
                    "meter_level_10",
                    "meter_level_11",
                    "meter_level_12",
                    "meter_level_13",
                    "meter_level_14",
                });
            this.screen = new MeterController(this.GetComponent<KBatchedAnimController>(), "screen_target", "screen_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[5]
                {
                    "screen_target",
                    "screen_fill",
                    "screen_status_0",
                    "screen_status_1",
                    "screen_status_2",
                });
            this.light = new MeterController(this.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logic_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4] 
                {
                    "logicmeter_target",
                    "logicmeter_frame",
                    "logicmeter_level_0",
                    "logicmeter_level_1",
                });
            WirelessConnecterSideScreen channelScreen = this.FindOrAdd<WirelessConnecterSideScreen>();
            channelScreen.SetConnector((IWirelessConnecter)this);
            this.ConnectToGrid();
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.ConnectionStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.ChannelStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.GridStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.GridCapacityStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.BatteryCapacityStatus, this);
            this.GetComponent<KSelectable>().AddStatusItem(WirelessPowerStrings.BatteryNetStatus, this);
        }

        protected override void OnCleanUp()
        {
            this.DisconnectFromGrid();
            base.OnCleanUp();
        }

        public virtual void EnergySim200ms(float dt)
        {
            UpdateJoulesAvailable();
            this.dt = dt;
            this.joulesConsumed = 0.0f;
            this.WattsUsed = 0.0f;
            this.ChargeCapacity = this.chargeWattage * dt;
            this.meter.SetPositionPercent(this.PercentFull);
            this.PreviousJoulesAvailable = this.JoulesAvailable;
            this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);

            if (!this.operational.IsOperational)
            {
#if DEBUG
                LogManager.LogDebug("[WirelessBattery] Not operational.\t" + this.operational);
                foreach (var flag in this.operational.Flags)
                    LogManager.LogDebug("\t\t" + flag.Key.Name + " : " + flag.Value);
#endif
                this.operational.SetActive(false);
            }
            else
            {
#if DEBUG
                LogManager.LogDebug("[WirelessBattery] Successful Energy Sim.");
#endif
                
                this.operational.SetActive(true);
            }
        }

        public void Sim1000ms(float dt)
        {
            this.UpdateSounds();
            this.UpdateScreenStatus();
            this.UpdateLightStatus();
            this._lastSent = 0f;
            this._lastReceived = 0f;
        }

        private void UpdateJoulesAvailable() => this.operational.SetFlag(WirelessPowerBattery.JoulesAvailableFlag, this.joulesAvailable > 0f);

        public float CalculateNetTotal() => this._lastReceived - (this._lastSent + this.joulesLostPerSecond);

        public bool JustNoJoulesAvailable() 
        {
            if (this.IsOperational)
                return false;
            else if (this.operational.GetFlag(WirelessPowerBattery.JoulesAvailableFlag))
                return false;

            int currentFalse = 0;
            foreach (bool flag in this.operational.Flags.Values) if (!flag) currentFalse++;
            return currentFalse == 1;
        }

        private void UpdateLightStatus()
        {
            if (!this.operational.IsActive || !this.IsOperational)
                this.light.SetPositionPercent(0f);
            else
                this.light.SetPositionPercent(0.5f);
        }

        private void UpdateScreenStatus()
        {
            if (!this.IsConnectedToGrid)
                this.screen.SetPositionPercent(0.25f);
            else if (!this.IsOperational)
                this.screen.SetPositionPercent(0f);
            else if (this._lastSent <= 0f && this._lastReceived <= 0f)
                this.screen.SetPositionPercent(0.5f);
            else if (this._lastSent > 0f || this._lastReceived > 0f)
                this.screen.SetPositionPercent(0.75f);
            else
                this.screen.SetPositionPercent(0f);
        }

        private void UpdateSounds()
        {
            if (this.PercentFull == 0.0 && this.PreviousPercentFull != 0.0)
                this.GetComponent<LoopingSounds>()?.PlayEvent(GameSoundEvents.BatteryDischarged);
            else if (this.PercentFull > 0.999000012874603 && this.PreviousPercentFull <= 0.999000012874603)
                this.GetComponent<LoopingSounds>()?.PlayEvent(GameSoundEvents.BatteryFull);
            else if (this.PercentFull >= 0.25 || this.PreviousPercentFull < 0.25)
                return;
            else
                this.GetComponent<LoopingSounds>()?.PlayEvent(GameSoundEvents.BatteryWarning);
        }

        public static float CalculateEnergyAfterFalloff(float joules) => !WirelessPowerConfigChecker.UseEnergyFalloff ? joules : joules*(1- WirelessPowerConfigChecker.CustomEnergyFalloffPercentage);

        public void AddEnergy(float joules, float timeslice)
        {
            this._lastReceived = joules * (1 / timeslice);
            this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
            this.joulesConsumed += joules;
            this.ChargeCapacity -= joules;
            this.WattsUsed = this.joulesConsumed / this.dt;
        }

        public void ConsumeEnergy(float joules, bool report = false)
        {
            if (report)
                ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -Mathf.Min(this.JoulesAvailable, joules), StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()));
            this.joulesAvailable = Mathf.Max(0.0f, this.JoulesAvailable - joules);
        }

        public void ConsumeEnergy(float joules, float timeslice)
        {
            this._lastSent = joules * (1 / timeslice) ;
            this.ConsumeEnergy(joules, false);
        }

        [ContextMenu("Refill Power")]
        public void DEBUG_RefillPower() => this.joulesAvailable = this.capacity;

        static WirelessPowerBattery()
        {
            WirelessPowerBattery.JoulesAvailableFlag = new Operational.Flag("joules_available", Operational.Flag.Type.Requirement);
        }
	}
}
