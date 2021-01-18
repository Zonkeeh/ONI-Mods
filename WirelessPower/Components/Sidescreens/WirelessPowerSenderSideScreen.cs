using PeterHan.PLib;
using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.UI;
using WirelessPower.Configuration;

namespace WirelessPower.Components.Sidescreens
{
    public sealed class WirelessPowerSenderSideScreen : SideScreenContent
    {
        private GameObject ChannelSlider;
        private GameObject ChannelText;
        private GameObject TransferSlider;
        private GameObject TransferText;
        private GameObject WirelessBatteryThresholdSlider;
        private GameObject WirelessBatteryThresholdText;
        private WirelessPowerSender target;

        internal WirelessPowerSenderSideScreen() => this.target = (WirelessPowerSender)null;

        public override void ClearTarget() => this.target = (WirelessPowerSender)null;

        public override string GetTitle() => "Sender Configuration";

        public override bool IsValidForTarget(GameObject target) => target.GetComponentSafe<WirelessPowerSender>() != null;

        protected override void OnPrefabInit()
        {
            Color backColour = new Color(0.998f, 0.998f, 0.998f);
            RectOffset rectOffset = new RectOffset(8, 8, 8, 8);
            this.SetupChannelSlider(backColour, rectOffset);
            this.SetupTransferSlider(backColour, rectOffset);
            this.SetupWirelessThresholdSlider(backColour, rectOffset);
            this.ContentContainer = this.gameObject;
            base.OnPrefabInit();
            this.UpdateUI();
        }

        private void SetupChannelSlider(Color backColour, RectOffset rectOffset)
        {
            PPanel channelTitle_panel = new PPanel("ChannelTitleRow");
            channelTitle_panel.BackColor = backColour;
            channelTitle_panel.Alignment = TextAnchor.MiddleCenter;
            channelTitle_panel.Direction = PanelDirection.Horizontal;
            channelTitle_panel.Spacing = 10;
            channelTitle_panel.Margin = rectOffset;
            channelTitle_panel.FlexSize = Vector2.right;
            PLabel channelTitle_label = new PLabel("ChannelTitleLabel");
            channelTitle_label.TextAlignment = TextAnchor.MiddleRight;
            channelTitle_label.Text = WirelessPowerStrings.ChannelTitleName;
            channelTitle_label.ToolTip = WirelessPowerStrings.ChannelTitleTooltip;
            channelTitle_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            PTextField channelTitle_textField = new PTextField("ChannelSliderTextField")
            {
                Text = target.Channel.ToString("0"),
                MaxLength = WirelessPowerConfigChecker.MaxNumberOfChannels.ToString().Length,
            };
            channelTitle_textField.SetMinWidthInCharacters(WirelessPowerConfigChecker.MaxNumberOfChannels.ToString().Length);
            channelTitle_textField.ToolTip = WirelessPowerStrings.ChannelTitleTooltip;
            channelTitle_textField.OnTextChanged = this.ChangeTextFieldChannel;
            channelTitle_textField.OnRealize += (PUIDelegates.OnRealize)(obj => this.ChannelText = obj);

            PPanel channelTitle_components = channelTitle_panel.AddChild((IUIComponent)channelTitle_label);
            channelTitle_components = channelTitle_panel.AddChild((IUIComponent)channelTitle_textField);
            channelTitle_components.AddTo(this.gameObject, -2);

            PPanel channelSlider_panel = new PPanel("ChannelSliderRow");
            channelSlider_panel.BackColor = backColour;
            channelSlider_panel.ImageMode = Image.Type.Sliced;
            channelSlider_panel.Alignment = TextAnchor.MiddleCenter;
            channelSlider_panel.Direction = PanelDirection.Horizontal;
            channelSlider_panel.Spacing = 16;
            channelSlider_panel.Margin = new RectOffset(12, 12, 6, 32);
            channelSlider_panel.FlexSize = Vector2.right;

            PLabel channelSliderMin_label = new PLabel("ChannelSliderMinLabel");
            channelSliderMin_label.Text = Mathf.RoundToInt(1).ToString();
            channelSliderMin_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel channelSlider_components = channelSlider_panel.AddChild((IUIComponent)channelSliderMin_label);

            PSliderSingle channelSlider = new PSliderSingle("Channel")
            {
                Direction = Slider.Direction.LeftToRight,
                HandleColor = PUITuning.Colors.ButtonPinkStyle,
                HandleSize = 20.0f,
                InitialValue = 1,
                IntegersOnly = true,
                MaxValue = WirelessPowerConfigChecker.MaxNumberOfChannels,
                MinValue = 1,
                PreferredLength = 140.0f,
                TrackSize = 16.0f,
            };
            channelSlider.ToolTip = WirelessPowerStrings.ChannelTitleTooltip;
            channelSlider.OnRealize += (PUIDelegates.OnRealize)(obj => this.ChannelSlider = obj);
            channelSlider.OnValueChanged = ChangeChannel;
            channelSlider_components.AddChild(channelSlider);

            PLabel channelSliderMax_label = new PLabel("ChannelSliderMaxLabel");
            channelSliderMax_label.Text = WirelessPowerConfigChecker.MaxNumberOfChannels.ToString();
            channelSliderMax_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            channelSlider_components.AddChild(channelSliderMax_label);

            channelSlider_components.AddTo(this.gameObject, -2);
        }

        private void ChangeTextFieldChannel(GameObject _, string channel)
        {
            if (this.target == null)
                return;

            int newChannel;
            bool converted = int.TryParse(channel, out newChannel);

            if (converted)
                this.ChangeChannel(null, newChannel);
            else if (this.ChannelText != null)
                PUIElements.SetText(this.ChannelText, this.target.Channel.ToString());
        }

        private void ChangeChannel(GameObject _, float channel)
        {
            if (this.target == null)
                return;

            int roundedChannel = Mathf.RoundToInt(channel);
            int newChannel = this.target.Channel = roundedChannel;

            if (newChannel == channel)
                SetChannelUI(channel);
        }

        private void SetChannelUI(float channel)
        {
            if (this.target == null)
                return;

            PSliderSingle.SetCurrentValue(this.ChannelSlider, channel);

            if (this.ChannelText == null)
                return;
            PUIElements.SetText(this.ChannelText, channel.ToString("0"));
        }

        private void SetupTransferSlider(Color backColour, RectOffset rectOffset)
        {
            PPanel transferTitle_panel = new PPanel("TransferTitleRow");
            transferTitle_panel.BackColor = backColour;
            transferTitle_panel.Alignment = TextAnchor.MiddleCenter;
            transferTitle_panel.Direction = PanelDirection.Horizontal;
            transferTitle_panel.Spacing = 10;
            transferTitle_panel.Margin = rectOffset;
            transferTitle_panel.FlexSize = Vector2.right;
            PLabel transferTitle_label = new PLabel("TransferTitleLabel");
            transferTitle_label.TextAlignment = TextAnchor.MiddleRight;
            transferTitle_label.Text = WirelessPowerStrings.JoulesToTransferTitleName;
            transferTitle_label.ToolTip = WirelessPowerStrings.JoulesToTransferTitleTooltip;
            transferTitle_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            PTextField transferTitle_TextField = new PTextField("TransferSliderTextField")
            {
                Text = target.JoulesToTransfer.ToString("0"),
                MaxLength = WirelessPowerConfigChecker.MaxTransfer.ToString().Length,
            };
            transferTitle_TextField.SetMinWidthInCharacters(WirelessPowerConfigChecker.MaxTransfer.ToString().Length);
            transferTitle_TextField.ToolTip = WirelessPowerStrings.JoulesToTransferTitleTooltip;
            transferTitle_TextField.OnTextChanged = this.ChangeTextFieldTransfer;
            transferTitle_TextField.OnRealize += (PUIDelegates.OnRealize)(obj => this.TransferText = obj);
            PLabel transferUnit_label = new PLabel("TransferUnitLabel");
            transferUnit_label.TextAlignment = TextAnchor.MiddleRight;
            transferUnit_label.Text = "W";
            transferUnit_label.ToolTip = WirelessPowerStrings.JoulesToTransferTitleTooltip;
            transferUnit_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel transferTitle_components = transferTitle_panel.AddChild((IUIComponent)transferTitle_label);
            transferTitle_components = transferTitle_panel.AddChild((IUIComponent)transferTitle_TextField);
            transferTitle_components = transferTitle_panel.AddChild((IUIComponent)transferUnit_label);
            transferTitle_components.AddTo(this.gameObject, -2);

            PPanel transferSlider_panel = new PPanel("TransferSliderRow");
            transferSlider_panel.BackColor = backColour;
            transferSlider_panel.ImageMode = Image.Type.Sliced;
            transferSlider_panel.Alignment = TextAnchor.MiddleCenter;
            transferSlider_panel.Direction = PanelDirection.Horizontal;
            transferSlider_panel.Spacing = 16;
            transferSlider_panel.Margin = new RectOffset(12, 12, 6, 32);
            transferSlider_panel.FlexSize = Vector2.right;

            PLabel transferSliderMin_label = new PLabel("TransferSliderMinLabel");
            transferSliderMin_label.Text = GameUtil.GetFormattedWattage(WirelessPowerConfigChecker.MinTransfer);
            transferSliderMin_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel transferSlider_components = transferSlider_panel.AddChild((IUIComponent)transferSliderMin_label);

            PSliderSingle transferSlider = new PSliderSingle("Transfer")
            {
                Direction = Slider.Direction.LeftToRight,
                HandleColor = PUITuning.Colors.ButtonPinkStyle,
                HandleSize = 20.0f,
                InitialValue = 200,
                IntegersOnly = true,
                MaxValue = WirelessPowerConfigChecker.MaxTransfer,
                MinValue = WirelessPowerConfigChecker.MinTransfer,
                PreferredLength = 140.0f,
                TrackSize = 16.0f,
            };
            transferSlider.ToolTip = WirelessPowerStrings.JoulesToTransferTitleTooltip;
            transferSlider.OnRealize += (PUIDelegates.OnRealize)(obj => this.TransferSlider = obj);
            transferSlider.OnValueChanged = ChangeTransfer;
            transferSlider_components.AddChild(transferSlider);

            PLabel transferSliderMax_label = new PLabel("TransferSliderMaxLabel");
            transferSliderMax_label.Text = GameUtil.GetFormattedWattage(WirelessPowerConfigChecker.MaxTransfer);
            transferSliderMax_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            transferSlider_components.AddChild(transferSliderMax_label);

            transferSlider_components.AddTo(this.gameObject, -2);
        }

        private void ChangeTextFieldTransfer(GameObject _, string transfer)
        {
            if (this.target == null)
                return;

            float newTransfer;
            bool converted = float.TryParse(transfer, out newTransfer);

            if (converted)
                this.ChangeTransfer(null, newTransfer);
            else if (this.ChannelText != null)
                PUIElements.SetText(this.TransferText, this.target.JoulesToTransfer.ToString("0"));
        }

        private void ChangeTransfer(GameObject _, float transfer)
        {
            if (this.target == null)
                return;

            int roundedTransfer = (Mathf.RoundToInt(transfer));

            float newTransfer = this.target.JoulesToTransfer = (float)roundedTransfer;

            if (newTransfer == transfer)
                SetTransferUI(transfer);
        }

        private void SetTransferUI(float transfer)
        {
            if (this.target == null)
                return;

            PSliderSingle.SetCurrentValue(this.TransferSlider, transfer);

            if (this.TransferText == null)
                return;

            PUIElements.SetText(this.TransferText, transfer.ToString("0"));
        }

        private void SetupWirelessThresholdSlider(Color backColour, RectOffset rectOffset)
		{
            PPanel thresholdTitle_panel = new PPanel("WirelessThresholdTitleRow");
            thresholdTitle_panel.BackColor = backColour;
            thresholdTitle_panel.Alignment = TextAnchor.MiddleCenter;
            thresholdTitle_panel.Direction = PanelDirection.Horizontal;
            thresholdTitle_panel.Spacing = 10;
            thresholdTitle_panel.Margin = rectOffset;
            thresholdTitle_panel.FlexSize = Vector2.right;
            PLabel thresholdTitle_label = new PLabel("WirelessThresholdTitleLabel");
            thresholdTitle_label.TextAlignment = TextAnchor.MiddleRight;
            thresholdTitle_label.Text = WirelessPowerStrings.WirelessBatteryThresholdTitleName;
            thresholdTitle_label.ToolTip = WirelessPowerStrings.WirelessBatteryThresholdTitleTooltip;
            thresholdTitle_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            PTextField thresholdTitle_textField = new PTextField("WirelessThresholdSliderTextField")
            {
                Text = this.target.WirelessBatteryThreshold.ToString("0"),
                MaxLength = 3,
            };
            thresholdTitle_textField.ToolTip = WirelessPowerStrings.WirelessBatteryThresholdTitleTooltip;
            thresholdTitle_textField.OnTextChanged = this.ChangeTextFieldWirelessThreshold;
            thresholdTitle_textField.OnRealize += (PUIDelegates.OnRealize)(obj => this.WirelessBatteryThresholdText = obj);
            PLabel thresholdUnit_label = new PLabel("WirelessThresholdUnitLabel");
            thresholdUnit_label.TextAlignment = TextAnchor.MiddleRight;
            thresholdUnit_label.Text = "%";
            thresholdUnit_label.ToolTip = WirelessPowerStrings.WirelessBatteryThresholdTitleTooltip;
            thresholdUnit_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel thresholdTitle_components = thresholdTitle_panel.AddChild((IUIComponent)thresholdTitle_label);
            thresholdTitle_components = thresholdTitle_panel.AddChild((IUIComponent)thresholdTitle_textField);
            thresholdTitle_components = thresholdTitle_panel.AddChild((IUIComponent)thresholdUnit_label);
            thresholdTitle_components.AddTo(this.gameObject, -2);

            PPanel thresholdSlider_panel = new PPanel("WirelessThresholdSliderRow");
            thresholdSlider_panel.BackColor = backColour;
            thresholdSlider_panel.ImageMode = Image.Type.Sliced;
            thresholdSlider_panel.Alignment = TextAnchor.MiddleCenter;
            thresholdSlider_panel.Direction = PanelDirection.Horizontal;
            thresholdSlider_panel.Spacing = 16;
            thresholdSlider_panel.Margin = new RectOffset(12, 12, 6, 32);
            thresholdSlider_panel.FlexSize = Vector2.right;

            PLabel thresholdSliderMin_label = new PLabel("WirelessThresholdSliderMinLabel");
            thresholdSliderMin_label.Text = Mathf.RoundToInt(1).ToString();
            thresholdSliderMin_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel thresholdSlider_components = thresholdSlider_panel.AddChild((IUIComponent)thresholdSliderMin_label);

            PSliderSingle thresholdSlider = new PSliderSingle("WirelessThreshold")
            {
                Direction = Slider.Direction.LeftToRight,
                HandleColor = PUITuning.Colors.ButtonPinkStyle,
                HandleSize = 20.0f,
                InitialValue = this.target.WirelessBatteryThreshold,
                IntegersOnly = true,
                MaxValue = 100,
                MinValue = 1,
                PreferredLength = 140.0f,
                TrackSize = 16.0f,
            };
            thresholdSlider.ToolTip = WirelessPowerStrings.WirelessBatteryThresholdTitleTooltip;
            thresholdSlider.OnRealize += (PUIDelegates.OnRealize)(obj => this.WirelessBatteryThresholdSlider = obj);
            thresholdSlider.OnValueChanged = ChangeWirelessThreshold;
            thresholdSlider_components.AddChild(thresholdSlider);

            PLabel thresholdSliderMax_label = new PLabel("WirelessThresholdSliderMaxLabel");
            thresholdSliderMax_label.Text = Mathf.RoundToInt(100).ToString();
            thresholdSliderMax_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            thresholdSlider_components.AddChild(thresholdSliderMax_label);

            thresholdSlider_components.AddTo(this.gameObject, -2);
        }

        private void ChangeTextFieldWirelessThreshold(GameObject _, string threshold)
        {
            if (this.target == null)
                return;

            int newThreshold;
            bool converted = int.TryParse(threshold, out newThreshold);

            if (converted)
                this.ChangeWirelessThreshold(null, newThreshold);
            else if (this.WirelessBatteryThresholdText != null)
                PUIElements.SetText(this.WirelessBatteryThresholdText, this.target.WirelessBatteryThreshold.ToString("0"));
        }

        private void ChangeWirelessThreshold(GameObject _, float threshold)
        {
            if (this.target == null)
                return;

            int roundedThreshold = Mathf.RoundToInt(threshold);
            float newThreshold = this.target.WirelessBatteryThreshold = roundedThreshold / 100f;
            SetWirelessThresholdUI(roundedThreshold);
        }

        private void SetWirelessThresholdUI(float threshold)
        {
            if (this.target == null)
                return;

            PSliderSingle.SetCurrentValue(this.WirelessBatteryThresholdSlider, threshold);

            if (this.WirelessBatteryThresholdText == null)
                return;
            PUIElements.SetText(this.WirelessBatteryThresholdText, threshold.ToString("0"));
        }

        public override void SetTarget(GameObject target)
        {
            this.target = target.GetComponentSafe<WirelessPowerSender>();
            this.UpdateUI();
        }

        private void UpdateUI() 
        {
            if (this.target == null)
                return;

            this.SetChannelUI(this.target.Channel);
            this.SetTransferUI(this.target.JoulesToTransfer);
            this.SetWirelessThresholdUI(this.target.WirelessBatteryThreshold * 100);
        }
    }
}