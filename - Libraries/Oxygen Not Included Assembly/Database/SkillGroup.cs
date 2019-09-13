// Decompiled with JetBrains decompiler
// Type: Database.SkillGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

namespace Database
{
  public class SkillGroup : Resource, IListableOption
  {
    public string choreGroupID;
    public List<Attribute> relevantAttributes;
    public List<string> requiredChoreGroups;
    public string choreGroupIcon;
    public string archetypeIcon;

    public SkillGroup(
      string id,
      string choreGroupID,
      string name,
      string icon,
      string archetype_icon)
      : base(id, name)
    {
      this.choreGroupID = choreGroupID;
      this.choreGroupIcon = icon;
      this.archetypeIcon = archetype_icon;
    }

    string IListableOption.GetProperName()
    {
      return (string) Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
    }
  }
}
