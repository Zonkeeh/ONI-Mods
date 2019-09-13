// Decompiled with JetBrains decompiler
// Type: Database.EatXCalories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class EatXCalories : ColonyAchievementRequirement
  {
    private int numCalories;

    public EatXCalories(int numCalories)
    {
      this.numCalories = numCalories;
    }

    public override bool Success()
    {
      return (double) RationTracker.Get().GetCaloriesConsumed() / 1000.0 > (double) this.numCalories;
    }

    public override void Deserialize(IReader reader)
    {
      this.numCalories = reader.ReadInt32();
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numCalories);
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, (object) GameUtil.GetFormattedCalories(!complete ? RationTracker.Get().GetCaloriesConsumed() : (float) this.numCalories * 1000f, GameUtil.TimeSlice.None, true), (object) GameUtil.GetFormattedCalories((float) this.numCalories * 1000f, GameUtil.TimeSlice.None, true));
    }
  }
}
