// Decompiled with JetBrains decompiler
// Type: IActivationRangeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IActivationRangeTarget
{
  float ActivateValue { get; set; }

  float DeactivateValue { get; set; }

  float MinValue { get; }

  float MaxValue { get; }

  bool UseWholeNumbers { get; }

  string ActivationRangeTitleText { get; }

  string ActivateSliderLabelText { get; }

  string DeactivateSliderLabelText { get; }

  string ActivateTooltip { get; }

  string DeactivateTooltip { get; }
}
