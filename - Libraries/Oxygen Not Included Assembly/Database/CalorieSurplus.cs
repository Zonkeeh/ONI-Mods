// Decompiled with JetBrains decompiler
// Type: Database.CalorieSurplus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class CalorieSurplus : ColonyAchievementRequirement
  {
    private double surplusAmount;

    public CalorieSurplus(float surplusAmount)
    {
      this.surplusAmount = (double) surplusAmount;
    }

    public override bool Success()
    {
      return (double) RationTracker.Get().CountRations((Dictionary<string, float>) null, true) / 1000.0 >= this.surplusAmount;
    }

    public override bool Fail()
    {
      return !this.Success();
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.surplusAmount);
    }

    public override void Deserialize(IReader reader)
    {
      this.surplusAmount = reader.ReadDouble();
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, (object) GameUtil.GetFormattedCalories(!complete ? RationTracker.Get().CountRations((Dictionary<string, float>) null, true) : (float) this.surplusAmount, GameUtil.TimeSlice.None, true), (object) GameUtil.GetFormattedCalories((float) this.surplusAmount, GameUtil.TimeSlice.None, true));
    }
  }
}
