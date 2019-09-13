// Decompiled with JetBrains decompiler
// Type: DuplicantTemperatureDeltaAsEnergyAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class DuplicantTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
  public DuplicantTemperatureDeltaAsEnergyAmountDisplayer(
    GameUtil.UnitClass unitClass,
    GameUtil.TimeSlice timeSlice)
    : base(unitClass, timeSlice, (StandardAttributeFormatter) null)
  {
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null), (object) this.formatter.GetFormattedValue(310.15f, GameUtil.TimeSlice.None, (GameObject) null));
    float num = (float) ((double) ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * 30.0 * 1000.0);
    string str2 = str1 + "\n\n";
    string str3 = this.formatter.DeltaTimeSlice != GameUtil.TimeSlice.PerCycle ? str2 + string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond, (GameObject) null)) + "\n" + string.Format((string) UI.CHANGEPERSECOND, (object) GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num, "F1", GameUtil.TimeSlice.None)) : str2 + string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle, (GameObject) null));
    for (int index = 0; index != instance.deltaAttribute.Modifiers.Count; ++index)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[index];
      str3 = str3 + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) GameUtil.GetFormattedHeatEnergyRate((float) ((double) modifier.Value * (double) num * 1.0), GameUtil.HeatEnergyFormatterUnit.Automatic));
    }
    return str3;
  }
}
