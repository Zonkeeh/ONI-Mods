// Decompiled with JetBrains decompiler
// Type: Klei.AI.Disease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Klei.AI
{
  [DebuggerDisplay("{base.Id}")]
  public abstract class Disease : Resource
  {
    public static readonly ElemGrowthInfo DEFAULT_GROWTH_INFO = new ElemGrowthInfo()
    {
      underPopulationDeathRate = 0.0f,
      populationHalfLife = float.PositiveInfinity,
      overPopulationHalfLife = float.PositiveInfinity,
      minCountPerKG = 0.0f,
      maxCountPerKG = float.PositiveInfinity,
      minDiffusionCount = 0,
      diffusionScale = 1f,
      minDiffusionInfestationTickCount = byte.MaxValue
    };
    public static ElemExposureInfo DEFAULT_EXPOSURE_INFO = new ElemExposureInfo()
    {
      populationHalfLife = float.PositiveInfinity
    };
    public Color32 overlayColour = new Color32(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue);
    private StringKey name;
    public HashedString id;
    public float strength;
    public Disease.RangeInfo temperatureRange;
    public Disease.RangeInfo temperatureHalfLives;
    public Disease.RangeInfo pressureRange;
    public Disease.RangeInfo pressureHalfLives;
    public List<GrowthRule> growthRules;
    public List<ExposureRule> exposureRules;
    public ElemGrowthInfo[] elemGrowthInfo;
    public ElemExposureInfo[] elemExposureInfo;
    public string overlayLegendHovertext;
    public Amount amount;
    public Attribute amountDeltaAttribute;
    public Attribute cureSpeedBase;

    public Disease(
      string id,
      byte strength,
      Disease.RangeInfo temperature_range,
      Disease.RangeInfo temperature_half_lives,
      Disease.RangeInfo pressure_range,
      Disease.RangeInfo pressure_half_lives)
      : base(id, (ResourceSet) null, (string) null)
    {
      this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
      this.id = (HashedString) id;
      this.overlayColour = Assets.instance.DiseaseVisualization.GetInfo((HashedString) id).overlayColour;
      this.temperatureRange = temperature_range;
      this.temperatureHalfLives = temperature_half_lives;
      this.pressureRange = pressure_range;
      this.pressureHalfLives = pressure_half_lives;
      this.PopulateElemGrowthInfo();
      this.ApplyRules();
      this.overlayLegendHovertext = Strings.Get("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".LEGEND_HOVERTEXT").ToString() + (string) DUPLICANTS.DISEASES.LEGEND_POSTAMBLE;
      Attribute attribute1 = new Attribute(id + "Min", "Minimum" + id.ToString(), string.Empty, string.Empty, 0.0f, Attribute.Display.Normal, false, (string) null, (string) null);
      Attribute attribute2 = new Attribute(id + "Max", "Maximum" + id.ToString(), string.Empty, string.Empty, 1E+07f, Attribute.Display.Normal, false, (string) null, (string) null);
      this.amountDeltaAttribute = new Attribute(id + "Delta", id.ToString(), string.Empty, string.Empty, 0.0f, Attribute.Display.Normal, false, (string) null, (string) null);
      this.amount = new Amount(id, id + " " + (string) DUPLICANTS.DISEASES.GERMS, id + " " + (string) DUPLICANTS.DISEASES.GERMS, attribute1, attribute2, this.amountDeltaAttribute, false, Units.Flat, 0.01f, true, (string) null, (string) null);
      Db.Get().Attributes.Add(attribute1);
      Db.Get().Attributes.Add(attribute2);
      Db.Get().Attributes.Add(this.amountDeltaAttribute);
      this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0.0f, (string) null, (string) null);
      this.cureSpeedBase.BaseValue = 1f;
      this.cureSpeedBase.SetFormatter((IAttributeFormatter) new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
      Db.Get().Attributes.Add(this.cureSpeedBase);
    }

    public string Name
    {
      get
      {
        return (string) Strings.Get(this.name);
      }
    }

    protected virtual void PopulateElemGrowthInfo()
    {
      this.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
      this.AddGrowthRule(new GrowthRule()
      {
        underPopulationDeathRate = new float?(0.0f),
        minCountPerKG = new float?(100f),
        populationHalfLife = new float?(float.PositiveInfinity),
        maxCountPerKG = new float?(1000f),
        overPopulationHalfLife = new float?(float.PositiveInfinity),
        minDiffusionCount = new int?(1000),
        diffusionScale = new float?(1f / 1000f),
        minDiffusionInfestationTickCount = new byte?((byte) 1)
      });
      this.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
      this.AddExposureRule(new ExposureRule()
      {
        populationHalfLife = new float?(float.PositiveInfinity)
      });
    }

    protected void AddGrowthRule(GrowthRule g)
    {
      if (this.growthRules == null)
      {
        this.growthRules = new List<GrowthRule>();
        Debug.Assert(g.GetType() == typeof (GrowthRule), (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.underPopulationDeathRate.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.populationHalfLife.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.overPopulationHalfLife.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.diffusionScale.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.minCountPerKG.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.maxCountPerKG.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.minDiffusionCount.HasValue, (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.minDiffusionInfestationTickCount.HasValue, (object) "First rule must be a fully defined base rule.");
      }
      else
        Debug.Assert(g.GetType() != typeof (GrowthRule), (object) "Subsequent rules should not be base rules");
      this.growthRules.Add(g);
    }

    protected void AddExposureRule(ExposureRule g)
    {
      if (this.exposureRules == null)
      {
        this.exposureRules = new List<ExposureRule>();
        Debug.Assert(g.GetType() == typeof (ExposureRule), (object) "First rule must be a fully defined base rule.");
        Debug.Assert(g.populationHalfLife.HasValue, (object) "First rule must be a fully defined base rule.");
      }
      else
        Debug.Assert(g.GetType() != typeof (ExposureRule), (object) "Subsequent rules should not be base rules");
      this.exposureRules.Add(g);
    }

    public CompositeGrowthRule GetGrowthRuleForElement(Element e)
    {
      CompositeGrowthRule compositeGrowthRule = new CompositeGrowthRule();
      if (this.growthRules != null)
      {
        for (int index = 0; index < this.growthRules.Count; ++index)
        {
          if (this.growthRules[index].Test(e))
            compositeGrowthRule.Overlay(this.growthRules[index]);
        }
      }
      return compositeGrowthRule;
    }

    public CompositeExposureRule GetExposureRuleForElement(Element e)
    {
      CompositeExposureRule compositeExposureRule = new CompositeExposureRule();
      if (this.exposureRules != null)
      {
        for (int index = 0; index < this.exposureRules.Count; ++index)
        {
          if (this.exposureRules[index].Test(e))
            compositeExposureRule.Overlay(this.exposureRules[index]);
        }
      }
      return compositeExposureRule;
    }

    public TagGrowthRule GetGrowthRuleForTag(Tag t)
    {
      if (this.growthRules != null)
      {
        for (int index = 0; index < this.growthRules.Count; ++index)
        {
          TagGrowthRule growthRule = this.growthRules[index] as TagGrowthRule;
          if (growthRule != null && growthRule.tag == t)
            return growthRule;
        }
      }
      return (TagGrowthRule) null;
    }

    protected void ApplyRules()
    {
      if (this.growthRules != null)
      {
        for (int index = 0; index < this.growthRules.Count; ++index)
          this.growthRules[index].Apply(this.elemGrowthInfo);
      }
      if (this.exposureRules == null)
        return;
      for (int index = 0; index < this.exposureRules.Count; ++index)
        this.exposureRules[index].Apply(this.elemExposureInfo);
    }

    protected void InitializeElemGrowthArray(
      ref ElemGrowthInfo[] infoArray,
      ElemGrowthInfo default_value)
    {
      List<Element> elements = ElementLoader.elements;
      infoArray = new ElemGrowthInfo[elements.Count];
      for (int index = 0; index < elements.Count; ++index)
      {
        ElemGrowthInfo elemGrowthInfo = default_value;
        infoArray[index] = elemGrowthInfo;
      }
      infoArray[ElementLoader.GetElementIndex(SimHashes.Polypropylene)] = new ElemGrowthInfo()
      {
        underPopulationDeathRate = 2.666667f,
        populationHalfLife = 10f,
        overPopulationHalfLife = 10f,
        minCountPerKG = 0.0f,
        maxCountPerKG = float.PositiveInfinity,
        minDiffusionCount = int.MaxValue,
        diffusionScale = 1f,
        minDiffusionInfestationTickCount = byte.MaxValue
      };
      infoArray[ElementLoader.GetElementIndex(SimHashes.Vacuum)] = new ElemGrowthInfo()
      {
        underPopulationDeathRate = 0.0f,
        populationHalfLife = 0.0f,
        overPopulationHalfLife = 0.0f,
        minCountPerKG = 0.0f,
        maxCountPerKG = float.PositiveInfinity,
        diffusionScale = 0.0f,
        minDiffusionInfestationTickCount = byte.MaxValue
      };
    }

    protected void InitializeElemExposureArray(
      ref ElemExposureInfo[] infoArray,
      ElemExposureInfo default_value)
    {
      List<Element> elements = ElementLoader.elements;
      infoArray = new ElemExposureInfo[elements.Count];
      for (int index = 0; index < elements.Count; ++index)
      {
        ElemExposureInfo elemExposureInfo = default_value;
        infoArray[index] = elemExposureInfo;
      }
    }

    public float GetGrowthRateForTags(HashSet<Tag> tags, bool overpopulated)
    {
      float num = 1f;
      if (this.growthRules != null)
      {
        for (int index = 0; index < this.growthRules.Count; ++index)
        {
          TagGrowthRule growthRule = this.growthRules[index] as TagGrowthRule;
          if (growthRule != null && tags.Contains(growthRule.tag))
            num *= Disease.HalfLifeToGrowthRate((!overpopulated ? growthRule.populationHalfLife : growthRule.overPopulationHalfLife).Value, 1f);
        }
      }
      return num;
    }

    public static float HalfLifeToGrowthRate(float half_life_in_seconds, float dt)
    {
      return (double) half_life_in_seconds != 0.0 ? ((double) half_life_in_seconds != double.PositiveInfinity ? Mathf.Pow(2f, -1f / (half_life_in_seconds / dt)) : 1f) : 0.0f;
    }

    public static float GrowthRateToHalfLife(float growth_rate)
    {
      return (double) growth_rate != 0.0 ? ((double) growth_rate != 1.0 ? Mathf.Log(2f, growth_rate) : float.PositiveInfinity) : 0.0f;
    }

    public float CalculateTemperatureHalfLife(float temperature)
    {
      return Disease.CalculateRangeHalfLife(temperature, ref this.temperatureRange, ref this.temperatureHalfLives);
    }

    public static float CalculateRangeHalfLife(
      float range_value,
      ref Disease.RangeInfo range,
      ref Disease.RangeInfo half_lives)
    {
      int idx1 = 3;
      int idx2 = 3;
      for (int idx3 = 0; idx3 < 4; ++idx3)
      {
        if ((double) range_value <= (double) range.GetValue(idx3))
        {
          idx1 = idx3 - 1;
          idx2 = idx3;
          break;
        }
      }
      if (idx1 < 0)
        idx1 = idx2;
      float num1 = half_lives.GetValue(idx1);
      float num2 = half_lives.GetValue(idx2);
      if (idx1 == 1 && idx2 == 2 || (float.IsInfinity(num1) || float.IsInfinity(num2)))
        return float.PositiveInfinity;
      float num3 = range.GetValue(idx1);
      float num4 = range.GetValue(idx2);
      float t = 0.0f;
      float num5 = num4 - num3;
      if ((double) num5 > 0.0)
        t = (range_value - num3) / num5;
      return Mathf.Lerp(num1, num2, t);
    }

    public List<Descriptor> GetQuantitativeDescriptors()
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.TEMPERATURE_RANGE, (object) GameUtil.GetFormattedTemperature(this.temperatureRange.minViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(this.temperatureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.TEMPERATURE_RANGE_TOOLTIP, (object) GameUtil.GetFormattedTemperature(this.temperatureRange.minViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(this.temperatureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(this.temperatureRange.minGrowth, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(this.temperatureRange.maxGrowth, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Information, false));
      descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.PRESSURE_RANGE, (object) GameUtil.GetFormattedMass(this.pressureRange.minViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.pressureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.PRESSURE_RANGE_TOOLTIP, (object) GameUtil.GetFormattedMass(this.pressureRange.minViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.pressureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.pressureRange.minGrowth, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.pressureRange.maxGrowth, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Information, false));
      List<GrowthRule> rules1 = new List<GrowthRule>();
      List<GrowthRule> rules2 = new List<GrowthRule>();
      List<GrowthRule> rules3 = new List<GrowthRule>();
      List<GrowthRule> rules4 = new List<GrowthRule>();
      List<GrowthRule> rules5 = new List<GrowthRule>();
      foreach (GrowthRule growthRule in this.growthRules)
      {
        if (growthRule.populationHalfLife.HasValue && growthRule.Name() != null)
        {
          if ((double) growthRule.populationHalfLife.Value < 0.0)
            rules1.Add(growthRule);
          else if ((double) growthRule.populationHalfLife.Value == double.PositiveInfinity)
            rules2.Add(growthRule);
          else if ((double) growthRule.populationHalfLife.Value >= 12000.0)
            rules3.Add(growthRule);
          else if ((double) growthRule.populationHalfLife.Value >= 1200.0)
            rules4.Add(growthRule);
          else
            rules5.Add(growthRule);
        }
      }
      descriptorList.AddRange((IEnumerable<Descriptor>) this.BuildGrowthInfoDescriptors(rules1, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_ON, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_ON_TOOLTIP, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_TOOLTIP));
      descriptorList.AddRange((IEnumerable<Descriptor>) this.BuildGrowthInfoDescriptors(rules2, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_ON, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_ON_TOOLTIP, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_TOOLTIP));
      descriptorList.AddRange((IEnumerable<Descriptor>) this.BuildGrowthInfoDescriptors(rules3, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_ON, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_ON_TOOLTIP, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_TOOLTIP));
      descriptorList.AddRange((IEnumerable<Descriptor>) this.BuildGrowthInfoDescriptors(rules4, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_ON, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_ON_TOOLTIP, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_TOOLTIP));
      descriptorList.AddRange((IEnumerable<Descriptor>) this.BuildGrowthInfoDescriptors(rules5, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_ON, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_ON_TOOLTIP, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_TOOLTIP));
      return descriptorList;
    }

    private List<Descriptor> BuildGrowthInfoDescriptors(
      List<GrowthRule> rules,
      string section_text,
      string section_tooltip,
      string item_tooltip)
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      if (rules.Count > 0)
      {
        descriptorList.Add(new Descriptor(section_text, section_tooltip, Descriptor.DescriptorType.Information, false));
        for (int index = 0; index < rules.Count; ++index)
          descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWTH_FORMAT, (object) rules[index].Name()), string.Format(item_tooltip, (object) GameUtil.GetFormattedCycles(Mathf.Abs(rules[index].populationHalfLife.Value), "F1")), Descriptor.DescriptorType.Information, false));
      }
      return descriptorList;
    }

    public struct RangeInfo
    {
      public float minViable;
      public float minGrowth;
      public float maxGrowth;
      public float maxViable;

      public RangeInfo(float min_viable, float min_growth, float max_growth, float max_viable)
      {
        this.minViable = min_viable;
        this.minGrowth = min_growth;
        this.maxGrowth = max_growth;
        this.maxViable = max_viable;
      }

      public void Write(BinaryWriter writer)
      {
        writer.Write(this.minViable);
        writer.Write(this.minGrowth);
        writer.Write(this.maxGrowth);
        writer.Write(this.maxViable);
      }

      public float GetValue(int idx)
      {
        switch (idx)
        {
          case 0:
            return this.minViable;
          case 1:
            return this.minGrowth;
          case 2:
            return this.maxGrowth;
          case 3:
            return this.maxViable;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      public static Disease.RangeInfo Idempotent()
      {
        return new Disease.RangeInfo(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
      }
    }
  }
}
