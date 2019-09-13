// Decompiled with JetBrains decompiler
// Type: AsPercentAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class AsPercentAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;

  public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
  {
    this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
  }

  public IAttributeFormatter Formatter
  {
    get
    {
      return (IAttributeFormatter) this.formatter;
    }
  }

  public GameUtil.TimeSlice DeltaTimeSlice
  {
    get
    {
      return this.formatter.DeltaTimeSlice;
    }
    set
    {
      this.formatter.DeltaTimeSlice = value;
    }
  }

  public string GetValueString(Amount master, AmountInstance instance)
  {
    return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None, (GameObject) null);
  }

  public virtual string GetDescription(Amount master, AmountInstance instance)
  {
    return string.Format("{0}: {1}", (object) master.Name, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None, (GameObject) null));
  }

  public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
  {
    return string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null));
  }

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null)) + "\n\n";
    string str2 = this.formatter.DeltaTimeSlice != GameUtil.TimeSlice.PerCycle ? str1 + string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond, (GameObject) null)) : str1 + string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle, (GameObject) null));
    for (int index = 0; index != instance.deltaAttribute.Modifiers.Count; ++index)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[index];
      float modifierContribution = instance.deltaAttribute.GetModifierContribution(modifier);
      str2 = str2 + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice, (GameObject) null));
    }
    return str2;
  }

  public string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.formatter.GetFormattedAttribute(instance);
  }

  public string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance)
  {
    if (modifier.IsMultiplier)
      return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
    return this.formatter.GetFormattedModifier(modifier, parent_instance);
  }

  public string GetFormattedValue(
    float value,
    GameUtil.TimeSlice timeSlice,
    GameObject parent_instance)
  {
    return this.formatter.GetFormattedValue(value, timeSlice, parent_instance);
  }

  protected float ToPercent(float value, AmountInstance instance)
  {
    return 100f * value / instance.GetMax();
  }
}
