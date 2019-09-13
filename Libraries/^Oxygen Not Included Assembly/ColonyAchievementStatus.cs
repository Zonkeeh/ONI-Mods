// Decompiled with JetBrains decompiler
// Type: ColonyAchievementStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class ColonyAchievementStatus
{
  private List<ColonyAchievementRequirement> requirements = new List<ColonyAchievementRequirement>();
  public bool success;
  public bool failed;

  public List<ColonyAchievementRequirement> Requirements
  {
    get
    {
      return this.requirements;
    }
  }

  public void UpdateAchievement()
  {
    if (this.requirements == null || this.requirements.Count <= 0)
      return;
    this.success = true;
    foreach (ColonyAchievementRequirement requirement in this.requirements)
    {
      requirement.Update();
      this.success &= requirement.Success();
      this.failed |= requirement.Fail();
    }
  }

  public void Deserialize(IReader reader)
  {
    this.success = reader.ReadByte() != (byte) 0;
    this.failed = reader.ReadByte() != (byte) 0;
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      System.Type type = System.Type.GetType(reader.ReadKleiString());
      if (type != null)
      {
        ColonyAchievementRequirement uninitializedObject = (ColonyAchievementRequirement) FormatterServices.GetUninitializedObject(type);
        uninitializedObject.Deserialize(reader);
        this.requirements.Add(uninitializedObject);
      }
    }
  }

  public void SetRequirements(
    List<ColonyAchievementRequirement> requirementChecklist)
  {
    this.requirements = requirementChecklist;
  }

  public void Serialize(BinaryWriter writer)
  {
    writer.Write(!this.success ? (byte) 0 : (byte) 1);
    writer.Write(!this.failed ? (byte) 0 : (byte) 1);
    writer.Write(this.requirements == null ? 0 : this.requirements.Count);
    if (this.requirements == null)
      return;
    foreach (ColonyAchievementRequirement requirement in this.requirements)
    {
      writer.WriteKleiString(requirement.GetType().ToString());
      requirement.Serialize(writer);
    }
  }
}
