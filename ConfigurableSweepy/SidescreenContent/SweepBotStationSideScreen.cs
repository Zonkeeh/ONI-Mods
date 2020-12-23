using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigurableSweepy
{
    public sealed class SweepBotStationSideScreen : SideScreenContent
    {
        private GameObject MoveSpeedSlider;
        private GameObject ProbingRadiusSlider;
        private GameObject MoveSpeedText;
        private GameObject ProbingRadiusText;
        private GameObject FindSweepyButton;
        private GameObject ResetSweepyButton;
        private SweepyConfigurator configurator;
        private SweepBotStation target;

        internal SweepBotStationSideScreen()
        {
            this.target = (SweepBotStation)null;
        }

        public override void ClearTarget() => this.target = (SweepBotStation)null;

        public override string GetTitle() => this.target.sweepBot == null ? (string) SweepyStrings.SideScreenTitleText : (string) this.target.storedName + "'s Configurator";

        public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object)target.GetComponentSafe<SweepBotStation>() != (UnityEngine.Object)null;

        protected override void OnPrefabInit()
        {
            Color backColour = new Color(0.998f, 0.998f, 0.998f);
            RectOffset rectOffset = new RectOffset(8, 8, 8, 8);

            PPanel moveTitle_panel = new PPanel("MovespeedTitleRow");
            moveTitle_panel.BackColor = backColour;
            moveTitle_panel.Alignment = TextAnchor.MiddleCenter;
            moveTitle_panel.Direction = PanelDirection.Horizontal;
            moveTitle_panel.Spacing = 10;
            moveTitle_panel.Margin = rectOffset;
            moveTitle_panel.FlexSize = Vector2.right;
            PLabel moveTitle_label = new PLabel("MovespeedTitleLabel");
            moveTitle_label.TextAlignment = TextAnchor.MiddleRight;
            moveTitle_label.Text = SweepyStrings.MoveSpeedTitleName;
            moveTitle_label.ToolTip = SweepyStrings.MoveSpeedTitleTooltip;
            moveTitle_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            PTextField moveTitle_textField = new PTextField("MovespeedSliderTextField")
            {
                Text = SweepyConfigChecker.BaseMovementSpeed.ToString("0.00"),
                MaxLength = 10,
            };
            moveTitle_textField.OnTextChanged = this.ChangeTextFieldMovespeed;
            moveTitle_textField.OnRealize += (PUIDelegates.OnRealize)(obj => this.MoveSpeedText = obj);

            PPanel moveTitle_components = moveTitle_panel.AddChild((IUIComponent)moveTitle_label);
            moveTitle_components = moveTitle_panel.AddChild((IUIComponent)moveTitle_textField);
            moveTitle_components.AddTo(this.gameObject, -2);

            PPanel moveSlider_panel = new PPanel("MovespeedSliderRow");
            moveSlider_panel.BackColor = backColour;
            moveSlider_panel.ImageMode = Image.Type.Sliced;
            moveSlider_panel.Alignment = TextAnchor.MiddleCenter;
            moveSlider_panel.Direction = PanelDirection.Horizontal;
            moveSlider_panel.Spacing = 10;
            moveSlider_panel.Margin = new RectOffset(8, 8, 6, 32);
            moveSlider_panel.FlexSize = Vector2.right;

            PLabel moveSliderMin_label = new PLabel("MovespeedSliderMinLabel");
            moveSliderMin_label.Text = Mathf.RoundToInt(SweepyConfigChecker.MinSpeedSliderValue).ToString();
            moveSliderMin_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel moveSlider_components = moveSlider_panel.AddChild((IUIComponent)moveSliderMin_label);

            PSliderSingle moveSpeedSlider = new PSliderSingle("Movespeed")
            {
                Direction = Slider.Direction.LeftToRight,
                HandleColor = PUITuning.Colors.ButtonPinkStyle,
                HandleSize = 16.0f,
                InitialValue = SweepyConfigChecker.BaseMovementSpeed,
                IntegersOnly = false,
                MaxValue = SweepyConfigChecker.MaxSpeedSliderValue,
                MinValue = SweepyConfigChecker.MinSpeedSliderValue,
                PreferredLength = 140.0f,
                TrackSize = 16.0f,
            };
            moveSpeedSlider.OnRealize += (PUIDelegates.OnRealize)(obj => this.MoveSpeedSlider = obj);
            moveSpeedSlider.OnValueChanged = ChangeMovespeed;
            moveSlider_components.AddChild(moveSpeedSlider);

            PLabel moveSliderMax_label = new PLabel("MovespeedSliderMaxLabel");
            moveSliderMax_label.Text = Mathf.RoundToInt(SweepyConfigChecker.MaxSpeedSliderValue).ToString();
            moveSliderMax_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            moveSlider_components.AddChild(moveSliderMax_label);

            moveSlider_components.AddTo(this.gameObject, -2);



            PPanel probingTitle_panel = new PPanel("ProbingRadiusTitleRow");
            probingTitle_panel.BackColor = backColour;
            probingTitle_panel.Alignment = TextAnchor.MiddleCenter;
            probingTitle_panel.Direction = PanelDirection.Horizontal;
            probingTitle_panel.Spacing = 10;
            probingTitle_panel.Margin = rectOffset;
            probingTitle_panel.FlexSize = Vector2.right;
            PLabel probingTitle_label = new PLabel("ProbingRadiusTitleLabel");
            probingTitle_label.TextAlignment = TextAnchor.MiddleRight;
            probingTitle_label.Text = SweepyStrings.ProbingRadiusTitleName;
            probingTitle_label.ToolTip = SweepyStrings.ProbingRadiusTitleTooltip;
            probingTitle_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            PTextField probingTitle_TextField = new PTextField("ProbingSliderTextField")
            {
                Text = SweepyConfigChecker.BaseProbingRadius.ToString("0"),
                MaxLength = 8,
            };
            probingTitle_TextField.OnTextChanged = this.ChangeTextFieldProbingRadius;
            probingTitle_TextField.OnRealize += (PUIDelegates.OnRealize)(obj => this.ProbingRadiusText = obj);

            PPanel probingTitle_components = probingTitle_panel.AddChild((IUIComponent)probingTitle_label);
            probingTitle_components = probingTitle_panel.AddChild((IUIComponent)probingTitle_TextField);
            probingTitle_components.AddTo(this.gameObject, -2);

            PPanel probingSlider_panel = new PPanel("ProbingRadiusSliderRow");
            probingSlider_panel.BackColor = backColour;
            probingSlider_panel.ImageMode = Image.Type.Sliced;
            probingSlider_panel.Alignment = TextAnchor.MiddleCenter;
            probingSlider_panel.Direction = PanelDirection.Horizontal;
            probingSlider_panel.Spacing = 10;
            probingSlider_panel.Margin = new RectOffset(8, 8, 6, 32);
            probingSlider_panel.FlexSize = Vector2.right;

            PLabel probingSliderMin_label = new PLabel("ProbingSliderMinLabel");
            probingSliderMin_label.Text = Mathf.RoundToInt(SweepyConfigChecker.MinProbingSliderValue).ToString();
            probingSliderMin_label.TextStyle = PUITuning.Fonts.TextDarkStyle;

            PPanel probingSlider_components = probingSlider_panel.AddChild((IUIComponent)probingSliderMin_label);

            PSliderSingle probingSpeedSlider = new PSliderSingle("Probing Radius")
            {
                Direction = Slider.Direction.LeftToRight,
                HandleColor = PUITuning.Colors.ButtonPinkStyle,
                HandleSize = 16.0f,
                InitialValue = SweepyConfigChecker.BaseProbingRadius,
                IntegersOnly = true,
                MaxValue = SweepyConfigChecker.MaxProbingSliderValue,
                MinValue = SweepyConfigChecker.MinProbingSliderValue,
                PreferredLength = 140.0f,
                TrackSize = 16.0f,
            };
            probingSpeedSlider.OnRealize += (PUIDelegates.OnRealize)(obj => this.ProbingRadiusSlider = obj);
            probingSpeedSlider.OnValueChanged = ChangeProbingRadius;
            probingSlider_components.AddChild(probingSpeedSlider);

            PLabel probingSliderMax_label = new PLabel("ProbingSliderMaxLabel");
            probingSliderMax_label.Text = Mathf.RoundToInt(SweepyConfigChecker.MaxProbingSliderValue).ToString();
            probingSliderMax_label.TextStyle = PUITuning.Fonts.TextDarkStyle;
            probingSlider_components.AddChild(probingSliderMax_label);

            probingSlider_components.AddTo(this.gameObject, -2);



            PPanel bottomRow_panel = new PPanel("BottomRow");
            bottomRow_panel.BackColor = backColour;
            bottomRow_panel.Alignment = TextAnchor.MiddleCenter;
            bottomRow_panel.Direction = PanelDirection.Horizontal;
            bottomRow_panel.Margin = rectOffset;
            bottomRow_panel.Spacing = 10;

            PButton findButton = new PButton();
            findButton.Color = PUITuning.Colors.ButtonBlueStyle;
            findButton.Margin = new RectOffset(16, 16, 8, 8);
            findButton.TextStyle = PUITuning.Fonts.TextLightStyle;
            findButton.OnClick = new PUIDelegates.OnButtonPressed(this.FindSweepyBot);
            findButton.Text = SweepyStrings.FindSweepyButtonText;
            findButton.ToolTip = SweepyStrings.FindSweepyButtonTooltip;
            findButton.OnRealize += (PUIDelegates.OnRealize)(obj => this.FindSweepyButton = obj);

            PButton resetButton = new PButton();
            resetButton.Color = PUITuning.Colors.ButtonBlueStyle;
            resetButton.Margin = new RectOffset(16, 16, 8, 8);
            resetButton.TextStyle = PUITuning.Fonts.TextLightStyle;
            resetButton.OnClick = new PUIDelegates.OnButtonPressed(this.ResetSweepyBot);
            resetButton.Text = SweepyStrings.ResetSweepyButtonText;
            resetButton.ToolTip = SweepyStrings.ResetSweepyButtonTooltip;
            resetButton.OnRealize += (PUIDelegates.OnRealize)(obj => this.ResetSweepyButton = obj);

            bottomRow_panel.AddChild(findButton);
            bottomRow_panel.AddChild(resetButton);
            bottomRow_panel.AddTo(this.gameObject, -2);


            this.ContentContainer = this.gameObject;
            base.OnPrefabInit();
            this.SetTarget(this.target.gameObject);
        }

        private void ChangeTextFieldMovespeed(GameObject _, string speed)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            float newSpeed;
            bool converted = float.TryParse(speed, out newSpeed);

            if (converted)
                this.ChangeMovespeed(null, newSpeed);
            else if ((UnityEngine.Object)this.MoveSpeedText != (UnityEngine.Object)null)
                PUIElements.SetText(this.MoveSpeedText, this.configurator.MoveSpeed.ToString("0.00"));
        }

        private void ChangeMovespeed(GameObject _, float speed) 
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            bool changed = this.configurator.ChangeMoveSpeed(speed);

            if (changed)
                SetMovespeedUI(speed);
        }

        private void SetMovespeedUI(float speed) 
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            PSliderSingle.SetCurrentValue(this.MoveSpeedSlider, speed);

            if ((UnityEngine.Object)this.MoveSpeedText == (UnityEngine.Object)null)
                return;
            PUIElements.SetText(this.MoveSpeedText, speed.ToString("0.00"));
        }

        private void ChangeTextFieldProbingRadius(GameObject _, string radius)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            int newRadius;
            bool converted = int.TryParse(radius, out newRadius);

            if (converted)
                this.ChangeProbingRadius(null, newRadius);
            else if ((UnityEngine.Object)this.MoveSpeedText != (UnityEngine.Object)null)
                PUIElements.SetText(this.ProbingRadiusText, this.configurator.ProbingRadius.ToString("0"));
        }

        private void ChangeProbingRadius(GameObject _, float value)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            int rounded = (Mathf.RoundToInt(value));

            bool changed = this.configurator.ChangeProbingRadius(rounded);

            if (changed)
                SetProbingRadiusUI(rounded);
        }

        private void SetProbingRadiusUI(int radius)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            PSliderSingle.SetCurrentValue(this.ProbingRadiusSlider, radius);

            StationaryChoreRangeVisualizer choreRangeVisualizer = this.target.GetComponent<StationaryChoreRangeVisualizer>();

            if ((UnityEngine.Object)choreRangeVisualizer == (UnityEngine.Object)null)
                return;

            choreRangeVisualizer.x = -radius;
            choreRangeVisualizer.width = 2 + 2*radius;
            Traverse.Create(choreRangeVisualizer).Method("UpdateVisualizers").GetValue();

            if ((UnityEngine.Object)this.ProbingRadiusText == (UnityEngine.Object)null)
                return;

            PUIElements.SetText(this.ProbingRadiusText, radius.ToString("0")); 
        }

        private void FindSweepyBot(GameObject _)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            this.configurator.FindSweepy();
        }

        private void ResetSweepyBot(GameObject _)
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null || (UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
                return;

            int return_state = this.configurator.ResetSweepy();

            if (return_state == 3)
            {
                this.SetMovespeedUI(SweepyConfigChecker.BaseMovementSpeed);
                this.SetProbingRadiusUI(SweepyConfigChecker.BaseProbingRadius);
            }
            else if (return_state == 2)
                this.SetProbingRadiusUI(SweepyConfigChecker.BaseProbingRadius);
            else if (return_state == 1)
                this.SetMovespeedUI(SweepyConfigChecker.BaseMovementSpeed);
        }

        private void SetButtonStates(bool enabled) 
        {
            if(this.FindSweepyButton != null)
                PButton.SetButtonEnabled(this.FindSweepyButton, enabled);

            if(this.ResetSweepyButton != null)
                PButton.SetButtonEnabled(this.ResetSweepyButton, enabled);

            if (!enabled) 
            {
                if ((UnityEngine.Object)this.MoveSpeedText != (UnityEngine.Object)null)
                    PUIElements.SetText(this.MoveSpeedText, "N/A");

                StationaryChoreRangeVisualizer choreRangeVisualizer = this.target.GetComponent<StationaryChoreRangeVisualizer>();

                if ((UnityEngine.Object)choreRangeVisualizer != (UnityEngine.Object)null)
                {
                    choreRangeVisualizer.x = 0;
                    choreRangeVisualizer.width = 0;
                    Traverse.Create(choreRangeVisualizer).Method("UpdateVisualizers").GetValue();
                }

                if ((UnityEngine.Object)this.ProbingRadiusText != (UnityEngine.Object)null)
                    PUIElements.SetText(this.ProbingRadiusText, "N/A");
            }
        }

        public override void SetTarget(GameObject target)
        {
            this.target = target.GetComponentSafe<SweepBotStation>();

            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null)
                return;

            this.configurator = this.target.sweepBot == null ? null : this.target.sweepBot.Get().gameObject.GetComponentSafe<SweepyConfigurator>();
            this.LoadSweepyStation();
        }

        private void LoadSweepyStation()
        {
            if ((UnityEngine.Object)this.target == (UnityEngine.Object)null)
                return;

            if ((UnityEngine.Object)this.configurator == (UnityEngine.Object)null)
            {
                this.SetButtonStates(false);
                return;
            }
            else
                this.SetButtonStates(true);

            this.SetMovespeedUI(this.configurator.MoveSpeed);
            this.SetProbingRadiusUI(this.configurator.ProbingRadius);
        }
    }
}