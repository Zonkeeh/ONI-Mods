using KSerialization;
using UnityEngine;
using WirelessPower.Components.Interfaces;
using WirelessPower.Configuration;

namespace WirelessPower.Components.Sidescreens
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class WirelessConnecterSideScreen : SideScreen, ISaveLoadable, IIntSliderControl
    {
        private IWirelessConnecter connecter;
        public void SetConnector(IWirelessConnecter connecter) => this.connecter = connecter;

        public string SliderTitleKey => WirelessPowerStrings.SideScreenKey + ".TITLE";

        public string GetSliderTooltipKey(int index) => WirelessPowerStrings.SideScreenKey + ".TOOLTIP";

        public string GetSliderTooltip() => WirelessPowerStrings.ChannelTitleTooltip;

        public string SliderUnits => "";

        protected override void OnSpawn() 
        {
            base.OnSpawn();
        }

        public int SliderDecimalPlaces(int index) => 0;

        public float GetSliderMin(int index) => 1f;

        public float GetSliderMax(int index) => WirelessPowerConfigChecker.MaxNumberOfChannels;

        public float GetSliderValue(int index)
        {
            if (this.connecter == null)
                return 1f;

            return this.connecter.Channel;
        }

        public void SetSliderValue(float percent, int index)
        {
            if (this.connecter != null)
                this.connecter.Channel = Mathf.RoundToInt(percent);
        }
    }
}
