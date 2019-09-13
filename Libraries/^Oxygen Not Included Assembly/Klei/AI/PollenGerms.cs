// Decompiled with JetBrains decompiler
// Type: Klei.AI.PollenGerms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
  public class PollenGerms : Disease
  {
    public const string ID = "PollenGerms";

    public PollenGerms()
      : base(nameof (PollenGerms), (byte) 5, new Disease.RangeInfo(263.15f, 273.15f, 363.15f, 373.15f), new Disease.RangeInfo(10f, 100f, 100f, 10f), new Disease.RangeInfo(0.0f, 0.0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
    {
    }

    protected override void PopulateElemGrowthInfo()
    {
      this.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
      this.AddGrowthRule(new GrowthRule()
      {
        underPopulationDeathRate = new float?(0.6666667f),
        minCountPerKG = new float?(0.4f),
        populationHalfLife = new float?(3000f),
        maxCountPerKG = new float?(500f),
        overPopulationHalfLife = new float?(10f),
        minDiffusionCount = new int?(3000),
        diffusionScale = new float?(1f / 1000f),
        minDiffusionInfestationTickCount = new byte?((byte) 1)
      });
      StateGrowthRule stateGrowthRule1 = new StateGrowthRule(Element.State.Solid);
      stateGrowthRule1.minCountPerKG = new float?(0.4f);
      stateGrowthRule1.populationHalfLife = new float?(10f);
      stateGrowthRule1.overPopulationHalfLife = new float?(10f);
      stateGrowthRule1.diffusionScale = new float?(1E-06f);
      stateGrowthRule1.minDiffusionCount = new int?(1000000);
      this.AddGrowthRule((GrowthRule) stateGrowthRule1);
      StateGrowthRule stateGrowthRule2 = new StateGrowthRule(Element.State.Gas);
      stateGrowthRule2.minCountPerKG = new float?(500f);
      stateGrowthRule2.underPopulationDeathRate = new float?(2.666667f);
      stateGrowthRule2.populationHalfLife = new float?(10f);
      stateGrowthRule2.overPopulationHalfLife = new float?(10f);
      stateGrowthRule2.maxCountPerKG = new float?(1000000f);
      stateGrowthRule2.minDiffusionCount = new int?(1000);
      stateGrowthRule2.diffusionScale = new float?(0.015f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule2);
      ElementGrowthRule elementGrowthRule = new ElementGrowthRule(SimHashes.Oxygen);
      elementGrowthRule.populationHalfLife = new float?(200f);
      elementGrowthRule.overPopulationHalfLife = new float?(10f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule);
      StateGrowthRule stateGrowthRule3 = new StateGrowthRule(Element.State.Liquid);
      stateGrowthRule3.minCountPerKG = new float?(0.4f);
      stateGrowthRule3.populationHalfLife = new float?(10f);
      stateGrowthRule3.overPopulationHalfLife = new float?(10f);
      stateGrowthRule3.maxCountPerKG = new float?(100f);
      stateGrowthRule3.diffusionScale = new float?(0.01f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule3);
      this.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
      this.AddExposureRule(new ExposureRule()
      {
        populationHalfLife = new float?(1200f)
      });
      ElementExposureRule elementExposureRule = new ElementExposureRule(SimHashes.Oxygen);
      elementExposureRule.populationHalfLife = new float?(float.PositiveInfinity);
      this.AddExposureRule((ExposureRule) elementExposureRule);
    }
  }
}
