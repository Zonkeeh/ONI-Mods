// Decompiled with JetBrains decompiler
// Type: GeyserConfigurator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GeyserConfigurator : KMonoBehaviour
{
  public float presetMax = 1f;
  private static List<GeyserConfigurator.GeyserType> geyserTypes;
  public HashedString presetType;
  public float presetMin;

  public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
  {
    GeyserConfigurator.GeyserType geyserType = (GeyserConfigurator.GeyserType) null;
    if (typeId != HashedString.Invalid)
      geyserType = GeyserConfigurator.geyserTypes.Find((Predicate<GeyserConfigurator.GeyserType>) (t => (HashedString) t.id == typeId));
    if (geyserType == null)
      Debug.LogError((object) string.Format("Tried finding a geyser with id {0} but it doesn't exist!", (object) typeId.ToString()));
    return geyserType;
  }

  public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
  {
    return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
  }

  private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(
    HashedString typeId,
    float min,
    float max)
  {
    System.Random randomSource = new System.Random(SaveLoader.Instance.worldDetailSave.globalWorldSeed + (int) this.transform.GetPosition().x + (int) this.transform.GetPosition().y);
    return new GeyserConfigurator.GeyserInstanceConfiguration()
    {
      typeId = typeId,
      rateRoll = this.Roll(randomSource, min, max),
      iterationLengthRoll = this.Roll(randomSource, 0.0f, 1f),
      iterationPercentRoll = this.Roll(randomSource, min, max),
      yearLengthRoll = this.Roll(randomSource, 0.0f, 1f),
      yearPercentRoll = this.Roll(randomSource, min, max)
    };
  }

  private float Roll(System.Random randomSource, float min, float max)
  {
    return (float) (randomSource.NextDouble() * ((double) max - (double) min)) + min;
  }

  public class GeyserType
  {
    public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
    public string id;
    public HashedString idHash;
    public SimHashes element;
    public float temperature;
    public float minRatePerCycle;
    public float maxRatePerCycle;
    public float maxPressure;
    public float minIterationLength;
    public float maxIterationLength;
    public float minIterationPercent;
    public float maxIterationPercent;
    public float minYearLength;
    public float maxYearLength;
    public float minYearPercent;
    public float maxYearPercent;

    public GeyserType(
      string id,
      SimHashes element,
      float temperature,
      float minRatePerCycle,
      float maxRatePerCycle,
      float maxPressure,
      float minIterationLength = 60f,
      float maxIterationLength = 1140f,
      float minIterationPercent = 0.1f,
      float maxIterationPercent = 0.9f,
      float minYearLength = 15000f,
      float maxYearLength = 135000f,
      float minYearPercent = 0.4f,
      float maxYearPercent = 0.8f)
    {
      this.id = id;
      this.idHash = (HashedString) id;
      this.element = element;
      this.temperature = temperature;
      this.minRatePerCycle = minRatePerCycle;
      this.maxRatePerCycle = maxRatePerCycle;
      this.maxPressure = maxPressure;
      this.minIterationLength = minIterationLength;
      this.maxIterationLength = maxIterationLength;
      this.minIterationPercent = minIterationPercent;
      this.maxIterationPercent = maxIterationPercent;
      this.minYearLength = minYearLength;
      this.maxYearLength = maxYearLength;
      this.minYearPercent = minYearPercent;
      this.maxYearPercent = maxYearPercent;
      if (GeyserConfigurator.geyserTypes == null)
        GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
      GeyserConfigurator.geyserTypes.Add(this);
    }

    public GeyserConfigurator.GeyserType AddDisease(
      SimUtil.DiseaseInfo diseaseInfo)
    {
      this.diseaseInfo = diseaseInfo;
      return this;
    }
  }

  [Serializable]
  public class GeyserInstanceConfiguration
  {
    public HashedString typeId;
    public float rateRoll;
    public float iterationLengthRoll;
    public float iterationPercentRoll;
    public float yearLengthRoll;
    public float yearPercentRoll;
    private float scaledRate;
    private float scaledIterationLength;
    private float scaledIterationPercent;
    private float scaledYearLength;
    private float scaledYearPercent;
    private bool didInit;

    private void Init()
    {
      if (this.didInit)
        return;
      this.didInit = true;
      this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
      this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
      this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
      this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
      this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
    }

    public GeyserConfigurator.GeyserType geyserType
    {
      get
      {
        return GeyserConfigurator.FindType(this.typeId);
      }
    }

    public float GetMaxPressure()
    {
      return this.geyserType.maxPressure;
    }

    public float GetIterationLength()
    {
      this.Init();
      return this.scaledIterationLength;
    }

    public float GetIterationPercent()
    {
      this.Init();
      return this.scaledIterationPercent;
    }

    public float GetOnDuration()
    {
      return this.GetIterationLength() * this.GetIterationPercent();
    }

    public float GetOffDuration()
    {
      return this.GetIterationLength() * (1f - this.GetIterationPercent());
    }

    public float GetMassPerCycle()
    {
      this.Init();
      return this.scaledRate;
    }

    public float GetEmitRate()
    {
      return this.GetMassPerCycle() / (600f / this.GetIterationLength()) / this.GetOnDuration();
    }

    public float GetYearLength()
    {
      this.Init();
      return this.scaledYearLength;
    }

    public float GetYearPercent()
    {
      this.Init();
      return this.scaledYearPercent;
    }

    public float GetYearOnDuration()
    {
      return this.GetYearLength() * this.GetYearPercent();
    }

    public float GetYearOffDuration()
    {
      return this.GetYearLength() * (1f - this.GetYearPercent());
    }

    public SimHashes GetElement()
    {
      return this.geyserType.element;
    }

    public float GetTemperature()
    {
      return this.geyserType.temperature;
    }

    public byte GetDiseaseIdx()
    {
      return this.geyserType.diseaseInfo.idx;
    }

    public int GetDiseaseCount()
    {
      return this.geyserType.diseaseInfo.count;
    }

    private float Resample(float t, float min, float max)
    {
      float num1 = 6f;
      float num2 = 0.002472623f;
      return (float) (((double) -Mathf.Log((float) (1.0 / (double) (t * (float) (1.0 - (double) num2 * 2.0) + num2) - 1.0)) + (double) num1) / ((double) num1 * 2.0)) * (max - min) + min;
    }
  }
}
