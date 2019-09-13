// Decompiled with JetBrains decompiler
// Type: Klei.AI.ZombieSpores
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
  public class ZombieSpores : Disease
  {
    public const string ID = "ZombieSpores";

    public ZombieSpores()
      : base(nameof (ZombieSpores), (byte) 50, new Disease.RangeInfo(168.15f, 258.15f, 513.15f, 563.15f), new Disease.RangeInfo(10f, 1200f, 1200f, 10f), new Disease.RangeInfo(0.0f, 0.0f, 1000f, 1000f), Disease.RangeInfo.Idempotent())
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
      SimHashes[] simHashesArray1 = new SimHashes[2]
      {
        SimHashes.Carbon,
        SimHashes.Diamond
      };
      foreach (SimHashes element in simHashesArray1)
      {
        ElementGrowthRule elementGrowthRule = new ElementGrowthRule(element);
        elementGrowthRule.underPopulationDeathRate = new float?(0.0f);
        elementGrowthRule.populationHalfLife = new float?(float.PositiveInfinity);
        elementGrowthRule.overPopulationHalfLife = new float?(3000f);
        elementGrowthRule.maxCountPerKG = new float?(1000f);
        elementGrowthRule.diffusionScale = new float?(0.005f);
        this.AddGrowthRule((GrowthRule) elementGrowthRule);
      }
      ElementGrowthRule elementGrowthRule1 = new ElementGrowthRule(SimHashes.BleachStone);
      elementGrowthRule1.populationHalfLife = new float?(10f);
      elementGrowthRule1.overPopulationHalfLife = new float?(10f);
      elementGrowthRule1.minDiffusionCount = new int?(100000);
      elementGrowthRule1.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule1);
      StateGrowthRule stateGrowthRule2 = new StateGrowthRule(Element.State.Gas);
      stateGrowthRule2.minCountPerKG = new float?(250f);
      stateGrowthRule2.populationHalfLife = new float?(12000f);
      stateGrowthRule2.overPopulationHalfLife = new float?(1200f);
      stateGrowthRule2.maxCountPerKG = new float?(10000f);
      stateGrowthRule2.minDiffusionCount = new int?(5100);
      stateGrowthRule2.diffusionScale = new float?(0.005f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule2);
      SimHashes[] simHashesArray2 = new SimHashes[3]
      {
        SimHashes.CarbonDioxide,
        SimHashes.Methane,
        SimHashes.SourGas
      };
      foreach (SimHashes element in simHashesArray2)
      {
        ElementGrowthRule elementGrowthRule2 = new ElementGrowthRule(element);
        elementGrowthRule2.underPopulationDeathRate = new float?(0.0f);
        elementGrowthRule2.populationHalfLife = new float?(float.PositiveInfinity);
        elementGrowthRule2.overPopulationHalfLife = new float?(6000f);
        this.AddGrowthRule((GrowthRule) elementGrowthRule2);
      }
      ElementGrowthRule elementGrowthRule3 = new ElementGrowthRule(SimHashes.ChlorineGas);
      elementGrowthRule3.populationHalfLife = new float?(10f);
      elementGrowthRule3.overPopulationHalfLife = new float?(10f);
      elementGrowthRule3.minDiffusionCount = new int?(100000);
      elementGrowthRule3.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule3);
      StateGrowthRule stateGrowthRule3 = new StateGrowthRule(Element.State.Liquid);
      stateGrowthRule3.minCountPerKG = new float?(0.4f);
      stateGrowthRule3.populationHalfLife = new float?(1200f);
      stateGrowthRule3.overPopulationHalfLife = new float?(300f);
      stateGrowthRule3.maxCountPerKG = new float?(100f);
      stateGrowthRule3.diffusionScale = new float?(0.01f);
      this.AddGrowthRule((GrowthRule) stateGrowthRule3);
      SimHashes[] simHashesArray3 = new SimHashes[4]
      {
        SimHashes.CrudeOil,
        SimHashes.Petroleum,
        SimHashes.Naphtha,
        SimHashes.LiquidMethane
      };
      foreach (SimHashes element in simHashesArray3)
      {
        ElementGrowthRule elementGrowthRule2 = new ElementGrowthRule(element);
        elementGrowthRule2.populationHalfLife = new float?(float.PositiveInfinity);
        elementGrowthRule2.overPopulationHalfLife = new float?(6000f);
        elementGrowthRule2.maxCountPerKG = new float?(1000f);
        elementGrowthRule2.diffusionScale = new float?(0.005f);
        this.AddGrowthRule((GrowthRule) elementGrowthRule2);
      }
      ElementGrowthRule elementGrowthRule4 = new ElementGrowthRule(SimHashes.Chlorine);
      elementGrowthRule4.populationHalfLife = new float?(10f);
      elementGrowthRule4.overPopulationHalfLife = new float?(10f);
      elementGrowthRule4.minDiffusionCount = new int?(100000);
      elementGrowthRule4.diffusionScale = new float?(1f / 1000f);
      this.AddGrowthRule((GrowthRule) elementGrowthRule4);
      this.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
      this.AddExposureRule(new ExposureRule()
      {
        populationHalfLife = new float?(float.PositiveInfinity)
      });
      ElementExposureRule elementExposureRule1 = new ElementExposureRule(SimHashes.Chlorine);
      elementExposureRule1.populationHalfLife = new float?(10f);
      this.AddExposureRule((ExposureRule) elementExposureRule1);
      ElementExposureRule elementExposureRule2 = new ElementExposureRule(SimHashes.ChlorineGas);
      elementExposureRule2.populationHalfLife = new float?(10f);
      this.AddExposureRule((ExposureRule) elementExposureRule2);
    }
  }
}
