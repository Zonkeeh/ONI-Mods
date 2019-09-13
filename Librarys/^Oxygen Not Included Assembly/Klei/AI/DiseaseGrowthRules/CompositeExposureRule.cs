// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.CompositeExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class CompositeExposureRule
  {
    public string name;
    public float populationHalfLife;

    public string Name()
    {
      return this.name;
    }

    public void Overlay(ExposureRule rule)
    {
      if (rule.populationHalfLife.HasValue)
        this.populationHalfLife = rule.populationHalfLife.Value;
      this.name = rule.Name();
    }

    public float GetHalfLifeForCount(int count)
    {
      return this.populationHalfLife;
    }
  }
}
