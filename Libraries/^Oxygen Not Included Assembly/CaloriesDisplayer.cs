// Decompiled with JetBrains decompiler
// Type: CaloriesDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class CaloriesDisplayer : StandardAmountDisplayer
{
  public CaloriesDisplayer()
    : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null)
  {
    this.formatter = (StandardAttributeFormatter) new CaloriesDisplayer.CaloriesAttributeFormatter();
  }

  public class CaloriesAttributeFormatter : StandardAttributeFormatter
  {
    public CaloriesAttributeFormatter()
      : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
    {
    }

    public override string GetFormattedModifier(
      AttributeModifier modifier,
      GameObject parent_instance)
    {
      if (modifier.IsMultiplier)
        return GameUtil.GetFormattedPercent((float) (-(double) modifier.Value * 100.0), GameUtil.TimeSlice.None);
      return base.GetFormattedModifier(modifier, parent_instance);
    }

    public override string GetTooltip(Attribute master, AttributeInstance instance)
    {
      return "TEST";
    }

    public override string GetTooltipDescription(Attribute master, AttributeInstance instance)
    {
      return "TEST";
    }
  }
}
