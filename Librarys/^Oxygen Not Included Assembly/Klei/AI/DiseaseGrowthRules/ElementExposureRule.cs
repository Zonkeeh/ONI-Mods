// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElementExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class ElementExposureRule : ExposureRule
  {
    public SimHashes element;

    public ElementExposureRule(SimHashes element)
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
