// Decompiled with JetBrains decompiler
// Type: ConduitDiseaseSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
  private static readonly HashedString TINT_SYMBOL = (HashedString) "germs";
  private const float rangeMin = 0.0f;
  private const float rangeMax = 100000f;

  protected override void UpdateVisualState(bool force = false)
  {
    if (this.wasOn == this.switchedOn && !force)
      return;
    this.wasOn = this.switchedOn;
    if (this.switchedOn)
    {
      this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
      ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(Grid.PosToCell((KMonoBehaviour) this));
      Color32 color32 = (Color32) Color.white;
      if (contents.diseaseIdx != byte.MaxValue)
        color32 = Db.Get().Diseases[(int) contents.diseaseIdx].overlayColour;
      this.animController.SetSymbolTint((KAnimHashedString) ConduitDiseaseSensor.TINT_SYMBOL, (Color) color32);
    }
    else
      this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
  }

  public override float CurrentValue
  {
    get
    {
      return (float) Conduit.GetFlowManager(this.conduitType).GetContents(Grid.PosToCell((KMonoBehaviour) this)).diseaseCount;
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

  public LocString Title
  {
    get
    {
      return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
    }
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
      return 1;
    }
  }

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return NonLinearSlider.GetDefaultRange(this.RangeMax);
    }
  }
}
