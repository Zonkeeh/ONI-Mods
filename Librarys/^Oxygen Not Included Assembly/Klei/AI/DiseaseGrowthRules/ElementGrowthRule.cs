// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElementGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class ElementGrowthRule : GrowthRule
  {
    public SimHashes element;

    public ElementGrowthRule(SimHashes element)
    {
      this.element = element;
    }

    public override bool Test(Element e)
    {
      return e.id == this.element;
    }

    public override string Name()
    {
      return ElementLoader.FindElementByHash(this.element).name;
    }
  }
}
