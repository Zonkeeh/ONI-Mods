// Decompiled with JetBrains decompiler
// Type: Database.RevealAsteriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;

namespace Database
{
  public class RevealAsteriod : ColonyAchievementRequirement
  {
    private float percentToReveal;
    private float amountRevealed;

    public RevealAsteriod(float percentToReveal)
    {
      this.percentToReveal = percentToReveal;
    }

    public override bool Success()
    {
      this.amountRevealed = 0.0f;
      float num = 0.0f;
      for (int index = 0; index < Grid.Visible.Length; ++index)
      {
        if (Grid.Visible[index] > (byte) 0)
          ++num;
      }
      this.amountRevealed = num / (float) Grid.Visible.Length;
      return (double) num / (double) Grid.Visible.Length > (double) this.percentToReveal;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.percentToReveal);
    }

    public override void Deserialize(IReader reader)
    {
      this.percentToReveal = reader.ReadSingle();
    }

    public override string GetProgress(bool complete)
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REVEALED, (object) (float) ((double) this.amountRevealed * 100.0), (object) (float) ((double) this.percentToReveal * 100.0));
    }
  }
}
