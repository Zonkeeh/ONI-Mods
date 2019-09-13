// Decompiled with JetBrains decompiler
// Type: StandardAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class StandardAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;

  public StandardAmountDisplayer(
    GameUtil.UnitClass unitClass,
    GameUtil.TimeSlice deltaTimeSlice,
    StandardAttributeFormatter formatter = null)
  {
    if (formatter != null)
      this.formatter = formatter;
    else
      this.formatter = new StandardAttributeFormatter(unitClass, deltaTimeSlice);
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

  public virtual string GetValueString(Amount master, AmountInstance instance)
  {
    if (master.showMax)
      return string.Format("{0} / {1}", (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null), (object) this.formatter.GetFormattedValue(instance.GetMax(), GameUtil.TimeSlice.None, (GameObject) null));
    StandardAttributeFormatter formatter = this.formatter;
    float num1 = instance.value;
    GameObject gameObject = instance.gameObject;
    double num2 = (double) num1;
    GameObject parent_instance = gameObject;
    return formatter.GetFormattedValue((float) num2, GameUtil.TimeSlice.None, parent_instance);
  }

  public virtual string GetDescription(Amount master, AmountInstance instance)
  {
    return string.Format("{0}: {1}", (object) master.Name, (object) this.GetValueString(master, instance));
  }

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    string empty = string.Empty;
    string str = (master.description.IndexOf("{1}") <= -1 ? empty + string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null)) : empty + string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null), (object) GameUtil.GetIdentityDescriptor(instance.gameObject))) + "\n\n";
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
      str += string.Format((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle, (GameObject) null));
    else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
      str += string.Format((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond, (GameObject) null));
    for (int index = 0; index != instance.deltaAttribute.Modifiers.Count; ++index)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[index];
      str = str + "\n" + string.Format((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedModifier(modifier, instance.gameObject));
    }
    return str;
  }

  public string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.formatter.GetFormattedAttribute(instance);
  }

  public string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance)
  {
    return this.formatter.GetFormattedModifier(modifier, parent_instance);
  }

  public string GetFormattedValue(
    float value,
    GameUtil.TimeSlice time_slice,
    GameObject parent_instance)
  {
    return this.formatter.GetFormattedValue(value, time_slice, parent_instance);
  }
}
