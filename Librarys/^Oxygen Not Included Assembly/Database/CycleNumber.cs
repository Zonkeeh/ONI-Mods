// Decompiled with JetBrains decompiler
// Type: Database.CycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class CycleNumber : VictoryColonyAchievementRequirement
  {
    private int cycleNumber;

    public CycleNumber(int cycleNumber = 100)
    {
      this.cycleNumber = cycleNumber;
    }

    public override string Name()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, (object) this.cycleNumber);
    }

    public override string Description()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, (object) this.cycleNumber);
    }

    public override bool Success()
    {
      return GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.cycleNumber);
    }

    public override void Deserialize(IReader reader)
    {
      this.cycleNumber = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, (object) (!complete ? GameClock.Instance.GetCycle() + 1 : this.cycleNumber), (object) this.cycleNumber);
    }
  }
}
