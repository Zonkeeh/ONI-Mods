// Decompiled with JetBrains decompiler
// Type: Database.CreaturePoopKGProduction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;

namespace Database
{
  public class CreaturePoopKGProduction : ColonyAchievementRequirement
  {
    private Tag poopElement;
    private float amountToPoop;

    public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
    {
      this.poopElement = poopElement;
      this.amountToPoop = amountToPoop;
    }

    public override bool Success()
    {
      if (Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement))
        return (double) Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= (double) this.amountToPoop;
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.amountToPoop);
      writer.WriteKleiString(this.poopElement.ToString());
    }

    public override void Deserialize(IReader reader)
    {
      this.amountToPoop = reader.ReadSingle();
      this.poopElement = new Tag(reader.ReadKleiString());
    }

    public override string GetProgress(bool complete)
    {
      float num = 0.0f;
      Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, (object) GameUtil.GetFormattedMass(!complete ? num : this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
    }
  }
}
