// Decompiled with JetBrains decompiler
// Type: NonLinearSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class NonLinearSlider : KSlider
{
  public NonLinearSlider.Range[] ranges;

  public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
  {
    return new NonLinearSlider.Range[1]
    {
      new NonLinearSlider.Range(100f, maxValue)
    };
  }

  protected override void Start()
  {
    base.Start();
    this.minValue = 0.0f;
    this.maxValue = 100f;
  }

  public void SetRanges(NonLinearSlider.Range[] ranges)
  {
    this.ranges = ranges;
  }

  public float GetPercentageFromValue(float value)
  {
    float a = 0.0f;
    float num = 0.0f;
    for (int index = 0; index < this.ranges.Length; ++index)
    {
      if ((double) value >= (double) num && (double) value <= (double) this.ranges[index].peakValue)
      {
        float t = (float) (((double) value - (double) num) / ((double) this.ranges[index].peakValue - (double) num));
        return Mathf.Lerp(a, a + this.ranges[index].width, t);
      }
      a += this.ranges[index].width;
      num = this.ranges[index].peakValue;
    }
    return 100f;
  }

  public float GetValueForPercentage(float percentage)
  {
    float num = 0.0f;
    float a = 0.0f;
    for (int index = 0; index < this.ranges.Length; ++index)
    {
      if ((double) percentage > (double) num && (double) num + (double) this.ranges[index].width >= (double) percentage)
      {
        float t = (percentage - num) / this.ranges[index].width;
        return Mathf.Lerp(a, this.ranges[index].peakValue, t);
      }
      num += this.ranges[index].width;
      a = this.ranges[index].peakValue;
    }
    return a;
  }

  protected override void Set(float input, bool sendCallback)
  {
    base.Set(input, sendCallback);
  }

  [Serializable]
  public struct Range
  {
    public float width;
    public float peakValue;

    public Range(float width, float peakValue)
    {
      this.width = width;
      this.peakValue = peakValue;
    }
  }
}
