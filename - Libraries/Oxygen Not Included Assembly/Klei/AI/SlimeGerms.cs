// Decompiled with JetBrains decompiler
// Type: Klei.AI.SlimeGerms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
  public class SlimeGerms : Disease
  {
    private const float COUGH_FREQUENCY = 20f;
    private const int DISEASE_AMOUNT = 1000;
    public const string ID = "SlimeLung";

    public SlimeGerms()
      : base("SlimeLung", (byte) 20, new Disease.RangeInfo(283.15f, 293.15f, 363.15f, 373.15f), new Disease.RangeInfo(10f, 1200f, 1200f, 10f), new Disease.RangeInfo(0.0f, 0.0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
    {
    }

    protected override void PopulateElemGrowthInfo()
    {
      this.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
      this.AddGrowthRule(new GrowthRule()
      {
        underPopulationDeathRate = new float?(2.666667f),
        minCountPerKG = new float?(0.4f),
        populationHalfLife = new float?(12000f),
        maxCountPerKG = new float?(500f),
        overPopulationHalfLife = new float?(1200f),
        minDiffusionCount = new int?(1000),
        diffusionScale = new float?(1f / 1000f),
        minDiffusionInfestationTickCount = new byte?((byte) 1)
      });
      StateGrowthRule stateGrowthRule1 = new StateGrowthRule(Element.State.Solid);
      stateGrowthRule1.minCountPerKG = new float?(0.4f);
      stateGrowthRule1.populationHalfLife = new float?(3000f);
      stateGrowthRule1.overPopulationHalfLife = new float?(1200f);
      stateGrowthRule1.diffusionScale = new float?(1E-06f);
      stateGrowthRule1.minDiffusionCount = new int?(1000000);
      this.AddGrowthRule((GrowthRule) stateGrowthRule1);
      ElementGrowthRule elementGrowthRule1 = new ElementGrowthRule(SimHashes.SlimeMold);
      elementGrowthRule1.underPopulationDeathRate = new float?(0.0f);
      elementGrowthRule1.populationHalfLife = new float?(-3000f);
      elementGrowthRule1.overPopulationHalfLife = new float?(3000f);
      elementGrowthRule1.maxCountPerKG = new float?(4500f);
      elementGrowthRule1.diffusionScale = new float?(0.05f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule1);
      ElementGrowthRule elementGrowthRule2 = new ElementGrowthRule(SimHashes.BleachStone);
      elementGrowthRule2.populationHalfLife = new float?(10f);
      elementGrowthRule2.overPopulationHalfLife = new float?(10f);
      elementGrowthRule2.minDiffusionCount = new int?(100000);
      elementGrowthRule2.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule2);
      StateGrowthRule stateGrowthRule2 = new StateGrowthRule(Element.State.Gas);
      stateGrowthRule2.minCountPerKG = new float?(250f);
      stateGrowthRule2.populationHalfLife = new float?(12000f);
      stateGrowthRule2.overPopulationHalfLife = new float?(1200f);
      stateGrowthRule2.maxCountPerKG = new float?(10000f);
      stateGrowthRule2.minDiffusionCount = new int?(5100);
      stateGrowthRule2.diffusionScale = new float?(0.005f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule2);
      ElementGrowthRule elementGrowthRule3 = new ElementGrowthRule(SimHashes.ContaminatedOxygen);
      elementGrowthRule3.underPopulationDeathRate = new float?(0.0f);
      elementGrowthRule3.populationHalfLife = new float?(-300f);
      elementGrowthRule3.overPopulationHalfLife = new float?(1200f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule3);
      ElementGrowthRule elementGrowthRule4 = new ElementGrowthRule(SimHashes.Oxygen);
      elementGrowthRule4.populationHalfLife = new float?(1200f);
      elementGrowthRule4.overPopulationHalfLife = new float?(10f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule4);
      ElementGrowthRule elementGrowthRule5 = new ElementGrowthRule(SimHashes.ChlorineGas);
      elementGrowthRule5.populationHalfLife = new float?(10f);
      elementGrowthRule5.overPopulationHalfLife = new float?(10f);
      elementGrowthRule5.minDiffusionCount = new int?(100000);
      elementGrowthRule5.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule5);
      StateGrowthRule stateGrowthRule3 = new StateGrowthRule(Element.State.Liquid);
      stateGrowthRule3.minCountPerKG = new float?(0.4f);
      stateGrowthRule3.populationHalfLife = new float?(1200f);
      stateGrowthRule3.overPopulationHalfLife = new float?(300f);
      stateGrowthRule3.maxCountPerKG = new float?(100f);
      stateGrowthRule3.diffusionScale = new float?(0.01f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule3);
      this.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
      this.AddExposureRule(new ExposureRule()
      {
        populationHalfLife = new float?(float.PositiveInfinity)
      });
      ElementExposureRule elementExposureRule1 = new ElementExposureRule(SimHashes.DirtyWater);
      elementExposureRule1.populationHalfLife = new float?(-12000f);
      this.AddExposureRule((ExposureRule) elementExposureRule1);
      ElementExposureRule elementExposureRule2 = new ElementExposureRule(SimHashes.ContaminatedOxygen);
      elementExposureRule2.populationHalfLife = new float?(-12000f);
      this.AddExposureRule((ExposureRule) elementExposureRule2);
      ElementExposureRule elementExposureRule3 = new ElementExposureRule(SimHashes.Oxygen);
      elementExposureRule3.populationHalfLife = new float?(3000f);
      this.AddExposureRule((ExposureRule) elementExposureRule3);
      ElementExposureRule elementExposureRule4 = new ElementExposureRule(SimHashes.ChlorineGas);
      elementExposureRule4.populationHalfLife = new float?(10f);
      this.AddExposureRule((ExposureRule) elementExposureRule4);
    }
  }
}
