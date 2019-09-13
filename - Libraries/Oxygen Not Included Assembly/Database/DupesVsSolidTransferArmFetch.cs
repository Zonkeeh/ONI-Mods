// Decompiled with JetBrains decompiler
// Type: Database.DupesVsSolidTransferArmFetch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class DupesVsSolidTransferArmFetch : ColonyAchievementRequirement
  {
    public float percentage;
    public int numCycles;
    public int currentCycleCount;
    public bool armsOutPerformingDupesThisCycle;

    public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
    {
      this.percentage = percentage;
      this.numCycles = numCycles;
    }

    public override bool Success()
    {
      Dictionary<int, int> dupeChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchDupeChoreDeliveries;
      Dictionary<int, int> automatedChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchAutomatedChoreDeliveries;
      int val2 = 0;
      this.currentCycleCount = 0;
      for (int key = GameClock.Instance.GetCycle() - this.numCycles; key < GameClock.Instance.GetCycle(); ++key)
      {
        if (automatedChoreDeliveries.ContainsKey(key) && (!dupeChoreDeliveries.ContainsKey(key) || (double) dupeChoreDeliveries[key] < (double) automatedChoreDeliveries[key] * (double) this.percentage))
        {
          ++val2;
          if (val2 >= this.numCycles)
          {
            this.currentCycleCount = this.numCycles;
            return true;
          }
        }
        else
        {
          this.currentCycleCount = Math.Max(this.currentCycleCount, val2);
          val2 = 0;
        }
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numCycles);
      writer.Write(this.percentage);
    }

    public override void Deserialize(IReader reader)
    {
      this.numCycles = reader.ReadInt32();
      this.percentage = reader.ReadSingle();
    }
  }
}
