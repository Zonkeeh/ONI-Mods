// Decompiled with JetBrains decompiler
// Type: PercentAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class PercentAttributeFormatter : StandardAttributeFormatter
{
  public PercentAttributeFormatter()
    : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
  {
  }

  public override string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.GetFormattedValue(instance.GetTotalDisplayValue(), this.DeltaTimeSlice, instance.gameObject);
  }

  public override string GetFormattedModifier(
    AttributeModifier modifier,
    GameObject parent_instance)
  {
    return this.GetFormattedValue(modifier.Value, this.DeltaTimeSlice, parent_instance);
  }

  public override string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice,
    GameObject parent_instance)
  {
    return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
  }
}
