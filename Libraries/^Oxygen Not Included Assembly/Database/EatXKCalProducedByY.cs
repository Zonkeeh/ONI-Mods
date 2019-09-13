// Decompiled with JetBrains decompiler
// Type: Database.EatXKCalProducedByY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Database
{
  public class EatXKCalProducedByY : ColonyAchievementRequirement
  {
    private int numCalories;
    private List<Tag> foodProducers;

    public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
    {
      this.numCalories = numCalories;
      this.foodProducers = foodProducers;
    }

    public override bool Success()
    {
      List<string> source = new List<string>();
      foreach (ComplexRecipe recipe in ComplexRecipeManager.Get().recipes)
      {
        foreach (Tag foodProducer in this.foodProducers)
        {
          foreach (Tag fabricator in recipe.fabricators)
          {
            if (fabricator == foodProducer)
              source.Add(recipe.FirstResult.ToString());
          }
        }
      }
      return (double) RationTracker.Get().GetCaloiresConsumedByFood(source.Distinct<string>().ToList<string>()) / 1000.0 > (double) this.numCalories;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.foodProducers.Count);
      foreach (Tag foodProducer in this.foodProducers)
        writer.WriteKleiString(foodProducer.ToString());
      writer.Write(this.numCalories);
    }

    public override void Deserialize(IReader reader)
    {
      int capacity = reader.ReadInt32();
      this.foodProducers = new List<Tag>(capacity);
      for (int index = 0; index < capacity; ++index)
        this.foodProducers.Add(new Tag(reader.ReadKleiString()));
      this.numCalories = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      string empty = string.Empty;
      for (int index = 0; index < this.foodProducers.Count; ++index)
      {
        if (index != 0)
          empty += (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
        BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[index].Name);
        empty += buildingDef.Name;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, (object) empty);
    }
  }
}
