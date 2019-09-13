// Decompiled with JetBrains decompiler
// Type: Database.NumberOfDupes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class NumberOfDupes : VictoryColonyAchievementRequirement
  {
    private int numDupes;

    public NumberOfDupes(int num)
    {
      this.numDupes = num;
    }

    public override string Name()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, (object) this.numDupes);
    }

    public override string Description()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, (object) this.numDupes);
    }

    public override bool Success()
    {
      return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.numDupes);
    }

    public override void Deserialize(IReader reader)
    {
      this.numDupes = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, (object) (!complete ? Components.LiveMinionIdentities.Items.Count : this.numDupes), (object) this.numDupes);
    }
  }
}
