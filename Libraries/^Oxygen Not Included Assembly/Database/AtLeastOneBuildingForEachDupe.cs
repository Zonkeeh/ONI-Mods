// Decompiled with JetBrains decompiler
// Type: Database.AtLeastOneBuildingForEachDupe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement
  {
    private List<Tag> validBuildingTypes = new List<Tag>();

    public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
    {
      this.validBuildingTypes = validBuildingTypes;
    }

    public override bool Success()
    {
      int num = 0;
      foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
      {
        if (this.validBuildingTypes.Contains(buildingComplete.prefabid.PrefabTag))
        {
          ++num;
          if (buildingComplete.prefabid.PrefabTag == (Tag) "FlushToilet" || buildingComplete.prefabid.PrefabTag == (Tag) "Outhouse")
            return true;
        }
      }
      if (Components.LiveMinionIdentities.Items.Count > 0)
        return num >= Components.LiveMinionIdentities.Items.Count;
      return false;
    }

    public override bool Fail()
    {
      return Components.LiveMinionIdentities.Items.Count <= 0;
    }

    public override void Deserialize(IReader reader)
    {
      int capacity = reader.ReadInt32();
      this.validBuildingTypes = new List<Tag>(capacity);
      for (int index = 0; index < capacity; ++index)
        this.validBuildingTypes.Add(new Tag(reader.ReadKleiString()));
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.validBuildingTypes.Count);
      foreach (Tag validBuildingType in this.validBuildingTypes)
        writer.WriteKleiString(validBuildingType.ToString());
    }

    public override string GetProgress(bool complete)
    {
      if (this.validBuildingTypes.Contains((Tag) "FlushToilet"))
        return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
      if (complete)
        return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
      int num = 0;
      foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
      {
        if (this.validBuildingTypes.Contains(buildingComplete.prefabid.PrefabTag))
          ++num;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, (object) (!complete ? num : Components.LiveMinionIdentities.Items.Count), (object) Components.LiveMinionIdentities.Items.Count);
    }
  }
}
