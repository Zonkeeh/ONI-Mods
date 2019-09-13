// Decompiled with JetBrains decompiler
// Type: Database.TuneUpGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.IO;

namespace Database
{
  public class TuneUpGenerator : ColonyAchievementRequirement
  {
    private float numChoreseToComplete;
    private float choresCompleted;

    public TuneUpGenerator(float numChoreseToComplete)
    {
      this.numChoreseToComplete = numChoreseToComplete;
    }

    public override bool Success()
    {
      float num = 0.0f;
      ReportManager.ReportEntry entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
      for (int index = 0; index < entry.contextEntries.Count; ++index)
      {
        ReportManager.ReportEntry contextEntry = entry.contextEntries[index];
        if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
          num += contextEntry.Negative;
      }
      for (int index1 = 0; index1 < ReportManager.Instance.reports.Count; ++index1)
      {
        for (int index2 = 0; index2 < ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.ChoreStatus).contextEntries.Count; ++index2)
        {
          ReportManager.ReportEntry contextEntry = ReportManager.Instance.reports[index1].GetEntry(ReportManager.ReportType.ChoreStatus).contextEntries[index2];
          if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
            num += contextEntry.Negative;
        }
      }
      this.choresCompleted = Math.Abs(num);
      return (double) Math.Abs(num) >= (double) this.numChoreseToComplete;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numChoreseToComplete);
    }

    public override void Deserialize(IReader reader)
    {
      this.numChoreseToComplete = reader.ReadSingle();
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, (object) (float) (!complete ? (double) this.choresCompleted : (double) this.numChoreseToComplete), (object) this.numChoreseToComplete, (object) Db.Get().ChoreTypes.PowerTinker.Name);
    }
  }
}
