// Decompiled with JetBrains decompiler
// Type: SkillListable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

public class SkillListable : IListableOption
{
  public LocString name;

  public SkillListable(string name)
  {
    this.skillName = name;
    Skill skill = Db.Get().Skills.TryGet(this.skillName);
    if (skill == null)
      return;
    this.name = (LocString) skill.Name;
    this.skillHat = skill.hat;
  }

  public string skillName { get; private set; }

  public string skillHat { get; private set; }

  public string GetProperName()
  {
    return (string) this.name;
  }
}
