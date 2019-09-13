// Decompiled with JetBrains decompiler
// Type: Database.ResearchComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class ResearchComplete : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      foreach (Tech resource in Db.Get().Techs.resources)
      {
        if (!resource.IsComplete())
          return false;
      }
      return true;
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete)
    {
      if (complete)
        return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, (object) Db.Get().Techs.resources.Count, (object) Db.Get().Techs.resources.Count);
      int num = 0;
      foreach (Tech resource in Db.Get().Techs.resources)
      {
        if (resource.IsComplete())
          ++num;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TECH_RESEARCHED, (object) num, (object) Db.Get().Techs.resources.Count);
    }
  }
}
