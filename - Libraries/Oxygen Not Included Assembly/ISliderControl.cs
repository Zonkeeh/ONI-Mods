// Decompiled with JetBrains decompiler
// Type: ISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface ISliderControl
{
  string SliderTitleKey { get; }

  string SliderUnits { get; }

  int SliderDecimalPlaces(int index);

  float GetSliderMin(int index);

  float GetSliderMax(int index);

  float GetSliderValue(int index);

  void SetSliderValue(float percent, int index);

  string GetSliderTooltipKey(int index);

  string GetSliderTooltip();
}
