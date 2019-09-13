// Decompiled with JetBrains decompiler
// Type: Database.CritterTypesWithTraits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.IO;

namespace Database
{
  public class CritterTypesWithTraits : ColonyAchievementRequirement
  {
    public Dictionary<Tag, bool> critterTypesToCheck = new Dictionary<Tag, bool>();
    private Tag trait;
    private bool hasTrait;

    public CritterTypesWithTraits(List<Tag> critterTypes, bool hasTrait = true)
    {
      foreach (Tag critterType in critterTypes)
      {
        if (!this.critterTypesToCheck.ContainsKey(critterType))
          this.critterTypesToCheck.Add(critterType, false);
      }
      this.hasTrait = hasTrait;
      this.trait = GameTags.Creatures.Wild;
    }

    public override void Update()
    {
      foreach (Capturable cmp in Components.Capturables.Items)
      {
        if (cmp.HasTag(this.trait) == this.hasTrait && this.critterTypesToCheck.ContainsKey(cmp.PrefabID()))
          this.critterTypesToCheck[cmp.PrefabID()] = true;
      }
    }

    public override bool Success()
    {
      foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
      {
        if (!keyValuePair.Value)
          return false;
      }
      return true;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.critterTypesToCheck.Count);
      foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
      {
        writer.WriteKleiString(keyValuePair.Key.ToString());
        writer.Write(!keyValuePair.Value ? (byte) 0 : (byte) 1);
      }
      writer.Write(!this.hasTrait ? (byte) 0 : (byte) 1);
    }

    public override void Deserialize(IReader reader)
    {
      this.critterTypesToCheck = new Dictionary<Tag, bool>();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        this.critterTypesToCheck.Add(new Tag(reader.ReadKleiString()), reader.ReadByte() != (byte) 0);
      this.hasTrait = reader.ReadByte() != (byte) 0;
      this.trait = GameTags.Creatures.Wild;
    }
  }
}
