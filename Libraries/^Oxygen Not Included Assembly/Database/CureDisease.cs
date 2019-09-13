// Decompiled with JetBrains decompiler
// Type: Database.CureDisease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class CureDisease : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      return Game.Instance.savedInfo.curedDisease;
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override string GetProgress(bool complete)
    {
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CURED_DISEASE;
    }
  }
}
