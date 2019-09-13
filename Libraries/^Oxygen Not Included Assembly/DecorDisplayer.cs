// Decompiled with JetBrains decompiler
// Type: DecorDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class DecorDisplayer : StandardAmountDisplayer
{
  public DecorDisplayer()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, (StandardAttributeFormatter) null)
  {
    this.formatter = (StandardAttributeFormatter) new DecorDisplayer.DecorAttributeFormatter();
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    string str1 = string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None, (GameObject) null));
    int cell = Grid.PosToCell(instance.gameObject);
    if (Grid.IsValidCell(cell))
      str1 += string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, (object) GameUtil.GetDecorAtCell(cell));
    string str2 = str1 + "\n";
    DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
    if (smi != null)
      str2 = str2 + string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, (object) this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None, (GameObject) null)) + string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, (object) this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None, (GameObject) null));
    return str2;
  }

  public class DecorAttributeFormatter : StandardAttributeFormatter
  {
    public DecorAttributeFormatter()
      : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
    {
    }
  }
}
