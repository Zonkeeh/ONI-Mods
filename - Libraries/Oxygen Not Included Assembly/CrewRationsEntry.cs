// Decompiled with JetBrains decompiler
// Type: CrewRationsEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class CrewRationsEntry : CrewListEntry
{
  public KButton incRationPerDayButton;
  public KButton decRationPerDayButton;
  public LocText rationPerDayText;
  public LocText rationsEatenToday;
  public LocText currentCaloriesText;
  public LocText currentStressText;
  public LocText currentHealthText;
  public ValueTrendImageToggle stressTrendImage;
  private RationMonitor.Instance rationMonitor;

  public override void Populate(MinionIdentity _identity)
  {
    base.Populate(_identity);
    this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
    this.Refresh();
  }

  public override void Refresh()
  {
    base.Refresh();
    this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
    if ((Object) this.identity == (Object) null)
      return;
    foreach (AmountInstance amount in (Modifications<Amount, AmountInstance>) this.identity.GetAmounts())
    {
      float min = amount.GetMin();
      float max = amount.GetMax();
      float num = max - min;
      string str = Mathf.RoundToInt((num - (max - amount.value)) / num * 100f).ToString();
      if (amount.amount == Db.Get().Amounts.Stress)
      {
        this.currentStressText.text = amount.GetValueString();
        this.currentStressText.GetComponent<ToolTip>().toolTip = amount.GetTooltip();
        this.stressTrendImage.SetValue(amount);
      }
      else if (amount.amount == Db.Get().Amounts.Calories)
      {
        this.currentCaloriesText.text = str + "%";
        this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amount.GetTooltip();
      }
      else if (amount.amount == Db.Get().Amounts.HitPoints)
      {
        this.currentHealthText.text = str + "%";
        this.currentHealthText.GetComponent<ToolTip>().toolTip = amount.GetTooltip();
      }
    }
  }
}
