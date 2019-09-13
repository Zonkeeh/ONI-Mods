// Decompiled with JetBrains decompiler
// Type: Klei.AI.FoodGerms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
  public class FoodGerms : Disease
  {
    public const string ID = "FoodPoisoning";
    private const float VOMIT_FREQUENCY = 200f;

    public FoodGerms()
      : base("FoodPoisoning", (byte) 10, new Disease.RangeInfo(248.15f, 278.15f, 313.15f, 348.15f), new Disease.RangeInfo(10f, 1200f, 1200f, 10f), new Disease.RangeInfo(0.0f, 0.0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
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
        maxCountPerKG = new float?(1000f),
        overPopulationHalfLife = new float?(3000f),
        minDiffusionCount = new int?(1000),
        diffusionScale = new float?(1f / 1000f),
        minDiffusionInfestationTickCount = new byte?((byte) 1)
      });
      StateGrowthRule stateGrowthRule1 = new StateGrowthRule(Element.State.Solid);
      stateGrowthRule1.minCountPerKG = new float?(0.4f);
      stateGrowthRule1.populationHalfLife = new float?(300f);
      stateGrowthRule1.overPopulationHalfLife = new float?(10f);
      stateGrowthRule1.minDiffusionCount = new int?(1000000);
      this.AddGrowthRule((GrowthRule) stateGrowthRule1);
      ElementGrowthRule elementGrowthRule1 = new ElementGrowthRule(SimHashes.ToxicSand);
      elementGrowthRule1.populationHalfLife = new float?(float.PositiveInfinity);
      elementGrowthRule1.overPopulationHalfLife = new float?(12000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule1);
      ElementGrowthRule elementGrowthRule2 = new ElementGrowthRule(SimHashes.Creature);
      elementGrowthRule2.populationHalfLife = new float?(float.PositiveInfinity);
      elementGrowthRule2.maxCountPerKG = new float?(4000f);
      elementGrowthRule2.overPopulationHalfLife = new float?(3000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule2);
      ElementGrowthRule elementGrowthRule3 = new ElementGrowthRule(SimHashes.BleachStone);
      elementGrowthRule3.populationHalfLife = new float?(10f);
      elementGrowthRule3.overPopulationHalfLife = new float?(10f);
      elementGrowthRule3.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule3);
      StateGrowthRule stateGrowthRule2 = new StateGrowthRule(Element.State.Gas);
      stateGrowthRule2.minCountPerKG = new float?(250f);
      stateGrowthRule2.populationHalfLife = new float?(1200f);
      stateGrowthRule2.overPopulationHalfLife = new float?(300f);
      stateGrowthRule2.diffusionScale = new float?(0.01f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule2);
      ElementGrowthRule elementGrowthRule4 = new ElementGrowthRule(SimHashes.ContaminatedOxygen);
      elementGrowthRule4.populationHalfLife = new float?(12000f);
      elementGrowthRule4.maxCountPerKG = new float?(10000f);
      elementGrowthRule4.overPopulationHalfLife = new float?(3000f);
      elementGrowthRule4.diffusionScale = new float?(0.05f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule4);
      ElementGrowthRule elementGrowthRule5 = new ElementGrowthRule(SimHashes.ChlorineGas);
      elementGrowthRule5.populationHalfLife = new float?(10f);
      elementGrowthRule5.overPopulationHalfLife = new float?(10f);
      elementGrowthRule5.minDiffusionCount = new int?(1000000);
      this.AddGrowthRule((GrowthRule) elementGrowthRule5);
      StateGrowthRule stateGrowthRule3 = new StateGrowthRule(Element.State.Liquid);
      stateGrowthRule3.minCountPerKG = new float?(0.4f);
      stateGrowthRule3.populationHalfLife = new float?(12000f);
      stateGrowthRule3.maxCountPerKG = new float?(5000f);
      stateGrowthRule3.diffusionScale = new float?(0.2f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule3);
      ElementGrowthRule elementGrowthRule6 = new ElementGrowthRule(SimHashes.DirtyWater);
      elementGrowthRule6.populationHalfLife = new float?(-12000f);
      elementGrowthRule6.overPopulationHalfLife = new float?(12000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule6);
      TagGrowthRule tagGrowthRule1 = new TagGrowthRule(GameTags.Edible);
      tagGrowthRule1.populationHalfLife = new float?(-12000f);
      tagGrowthRule1.overPopulationHalfLife = new float?(float.PositiveInfinity);
      this.AddGrowthRule((GrowthRule) tagGrowthRule1);
      TagGrowthRule tagGrowthRule2 = new TagGrowthRule(GameTags.Pickled);
      tagGrowthRule2.populationHalfLife = new float?(10f);
      tagGrowthRule2.overPopulationHalfLife = new float?(10f);
      this.AddGrowthRule((GrowthRule) tagGrowthRule2);
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
      ElementExposureRule elementExposureRule3 = new ElementExposureRule(SimHashes.ChlorineGas);
      elementExposureRule3.populationHalfLife = new float?(10f);
      this.AddExposureRule((ExposureRule) elementExposureRule3);
    }
  }
}
