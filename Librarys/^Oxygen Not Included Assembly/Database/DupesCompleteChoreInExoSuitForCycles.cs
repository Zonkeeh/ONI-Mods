// Decompiled with JetBrains decompiler
// Type: Database.DupesCompleteChoreInExoSuitForCycles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Database
{
  public class DupesCompleteChoreInExoSuitForCycles : ColonyAchievementRequirement
  {
    public int currentCycleStreak;
    public int numCycles;

    public DupesCompleteChoreInExoSuitForCycles(int numCycles)
    {
      this.numCycles = numCycles;
    }

    public override bool Success()
    {
      Dictionary<int, List<int>> completeChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
      if (completeChoresInSuits.Count <= this.numCycles)
        return false;
      Dictionary<int, float> dictionary = new Dictionary<int, float>();
      foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
        dictionary.Add(minionIdentity.GetComponent<KPrefabID>().InstanceID, minionIdentity.arrivalTime);
      int val2 = 0;
      for (int key = GameClock.Instance.GetCycle() - this.numCycles; key < GameClock.Instance.GetCycle(); ++key)
      {
        if (completeChoresInSuits.ContainsKey(key))
        {
          List<int> list = dictionary.Keys.Except<int>((IEnumerable<int>) completeChoresInSuits[key]).ToList<int>();
          bool flag = true;
          foreach (int index in list)
          {
            if ((double) dictionary[index] < (double) key)
            {
              flag = false;
              break;
            }
          }
          val2 = !flag ? 0 : val2 + 1;
          if (val2 >= this.numCycles)
          {
            this.currentCycleStreak = this.numCycles;
            return true;
          }
        }
        else
        {
          this.currentCycleStreak = Math.Max(this.currentCycleStreak, val2);
          val2 = 0;
        }
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numCycles);
    }

    public override void Deserialize(IReader reader)
    {
      this.numCycles = reader.ReadInt32();
    }

    public int GetNumberOfDupesForCycle(int cycle)
    {
      int num = 0;
      Dictionary<int, List<int>> completeChoresInSuits = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().dupesCompleteChoresInSuits;
      if (completeChoresInSuits.ContainsKey(GameClock.Instance.GetCycle()))
        num = completeChoresInSuits[GameClock.Instance.GetCycle()].Count;
      return num;
    }
  }
}
