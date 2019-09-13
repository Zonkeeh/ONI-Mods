// Decompiled with JetBrains decompiler
// Type: Database.FractionalCycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class FractionalCycleNumber : ColonyAchievementRequirement
  {
    private float fractionalCycleNumber;

    public FractionalCycleNumber(float fractionalCycleNumber)
    {
      this.fractionalCycleNumber = fractionalCycleNumber;
    }

    public override bool Success()
    {
      int fractionalCycleNumber = (int) this.fractionalCycleNumber;
      float num = this.fractionalCycleNumber - (float) fractionalCycleNumber;
      if ((double) (GameClock.Instance.GetCycle() + 1) > (double) this.fractionalCycleNumber)
        return true;
      if (GameClock.Instance.GetCycle() + 1 == fractionalCycleNumber)
        return (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= (double) num;
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.fractionalCycleNumber);
    }

    public override void Deserialize(IReader reader)
    {
      this.fractionalCycleNumber = reader.ReadSingle();
    }

    public override string GetProgress(bool complete)
    {
      float num = (float) GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, (object) (float) (!complete ? (double) num : (double) this.fractionalCycleNumber), (object) this.fractionalCycleNumber);
    }
  }
}
