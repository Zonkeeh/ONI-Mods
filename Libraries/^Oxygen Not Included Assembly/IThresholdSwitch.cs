// Decompiled with JetBrains decompiler
// Type: IThresholdSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IThresholdSwitch
{
  float Threshold { get; set; }

  bool ActivateAboveThreshold { get; set; }

  float CurrentValue { get; }

  float RangeMin { get; }

  float RangeMax { get; }

  float GetRangeMinInputField();

  float GetRangeMaxInputField();

  LocString Title { get; }

  LocString ThresholdValueName { get; }

  LocString ThresholdValueUnits();

  string Format(float value, bool units);

  string AboveToolTip { get; }

  string BelowToolTip { get; }

  float ProcessedSliderValue(float input);

  float ProcessedInputValue(float input);

  ThresholdScreenLayoutType LayoutType { get; }

  int IncrementScale { get; }

  NonLinearSlider.Range[] GetRanges { get; }
}
