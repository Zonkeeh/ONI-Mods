// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.TagGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class TagGrowthRule : GrowthRule
  {
    public Tag tag;

    public TagGrowthRule(Tag tag)
    {
      this.tag = tag;
    }

    public override bool Test(Element e)
    {
      return e.HasTag(this.tag);
    }

    public override string Name()
    {
      return this.tag.ProperName();
    }
  }
}
