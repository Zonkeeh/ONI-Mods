// Decompiled with JetBrains decompiler
// Type: Database.BeforeCycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class BeforeCycleNumber : ColonyAchievementRequirement
  {
    private int cycleNumber;

    public BeforeCycleNumber(int cycleNumber = 100)
    {
      this.cycleNumber = cycleNumber;
    }

    public override bool Success()
    {
      return GameClock.Instance.GetCycle() + 1 <= this.cycleNumber;
    }

    public override bool Fail()
    {
      return !this.Success();
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
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REMAINING_CYCLES, (object) Mathf.Max(this.cycleNumber - GameClock.Instance.GetCycle(), 0), (object) this.cycleNumber);
    }
  }
}
