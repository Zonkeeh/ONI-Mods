// Decompiled with JetBrains decompiler
// Type: LogicDiseaseSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>((System.Action<LogicDiseaseSensor, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly HashedString[] ON_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pre",
    (HashedString) "on_loop"
  };
  private static readonly HashedString[] OFF_ANIMS = new HashedString[2]
  {
    (HashedString) "on_pst",
    (HashedString) "off"
  };
  private static readonly HashedString TINT_SYMBOL = (HashedString) "germs";
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  private int[] samples = new int[8];
  [SerializeField]
  [Serialize]
  private float threshold;
  private KBatchedAnimController animController;
  private bool wasOn;
  private const float rangeMin = 0.0f;
  private const float rangeMax = 100000f;
  private const int WINDOW_SIZE = 8;
  private int sampleIdx;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicDiseaseSensor component = ((GameObject) data).GetComponent<LogicDiseaseSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.OnToggle += new System.Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    if (this.sampleIdx < 8)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      if ((double) Grid.Mass[cell] <= 0.0)
        return;
      this.samples[this.sampleIdx] = Grid.DiseaseCount[cell];
      ++this.sampleIdx;
    }
    else
    {
      this.sampleIdx = 0;
      float currentValue = this.CurrentValue;
      if (this.activateAboveThreshold)
      {
        if ((double) currentValue > (double) this.threshold && !this.IsSwitchedOn || (double) currentValue <= (double) this.threshold && this.IsSwitchedOn)
          this.Toggle();
      }
      else if ((double) currentValue > (double) this.threshold && this.IsSwitchedOn || (double) currentValue <= (double) this.threshold && !this.IsSwitchedOn)
        this.Toggle();
      this.animController.SetSymbolVisiblity((KAnimHashedString) LogicDiseaseSensor.TINT_SYMBOL, (double) currentValue > 0.0);
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState(false);
  }

  public float Threshold
  {
    get
    {
      return this.threshold;
    }
    set
    {
      this.threshold = value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateAboveThreshold;
    }
    set
    {
      this.activateAboveThreshold = value;
    }
  }

  public float CurrentValue
  {
    get
    {
      float num = 0.0f;
      for (int index = 0; index < 8; ++index)
        num += (float) this.samples[index];
      return num / 8f;
    }
  }

  public float RangeMin
  {
    get
    {
      return 0.0f;
    }
  }

  public float RangeMax
  {
    get
    {
      return 100000f;
    }
  }

  public float GetRangeMinInputField()
  {
    return 0.0f;
  }

  public float GetRangeMaxInputField()
  {
    return 100000f;
  }

  public LocString ThresholdValueName
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
    }
  }

  public string AboveToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
    }
  }

  public string BelowToolTip
  {
    get
    {
      return (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
    }
  }

  public string Format(float value, bool units)
  {
    return GameUtil.GetFormattedInt((float) (int) value, GameUtil.TimeSlice.None);
  }

  public float ProcessedSliderValue(float input)
  {
    return input;
  }

  public float ProcessedInputValue(float input)
  {
    return input;
  }

  public LocString ThresholdValueUnits()
  {
    return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
  }

  public ThresholdScreenLayoutType LayoutType
  {
    get
    {
      return ThresholdScreenLayoutType.SliderBar;
    }
  }

  public int IncrementScale
  {
    get
    {
      return 100;
    }
  }

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return NonLinearSlider.GetDefaultRange(this.RangeMax);
    }
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !this.switchedOn ? 0 : 1);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    if (this.switchedOn)
    {
      this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      byte num = Grid.DiseaseIdx[cell];
      Color32 color32 = (Color32) Color.white;
      if (num != byte.MaxValue)
        color32 = Db.Get().Diseases[(int) num].overlayColour;
      this.animController.SetSymbolTint((KAnimHashedString) LogicDiseaseSensor.TINT_SYMBOL, (Color) color32);
    }
    else
      this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
  }

  protected override void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusInactive : Db.Get().BuildingStatusItems.LogicSensorStatusActive, (object) null);
  }

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
    }
  }
}
