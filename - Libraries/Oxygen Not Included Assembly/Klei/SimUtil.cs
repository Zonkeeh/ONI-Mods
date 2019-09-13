// Decompiled with JetBrains decompiler
// Type: Klei.SimUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei
{
  public static class SimUtil
  {
    private static float MIN_DISEASE_LOG_SUBTRACTION = 2f;
    private static float MAX_DISEASE_LOG_RANGE = 6f;
    private const int MAX_ALPHA_COUNT = 1000000;

    public static float CalculateEnergyFlow(
      float source_temp,
      float source_thermal_conductivity,
      float dest_temp,
      float dest_thermal_conductivity,
      float surface_area = 1f,
      float thickness = 1f)
    {
      return (float) ((double) (source_temp - dest_temp) * (double) Math.Min(source_thermal_conductivity, dest_thermal_conductivity) * ((double) surface_area / (double) thickness));
    }

    public static float CalculateEnergyFlow(
      int cell,
      float dest_temp,
      float dest_specific_heat_capacity,
      float dest_thermal_conductivity,
      float surface_area = 1f,
      float thickness = 1f)
    {
      if ((double) Grid.Mass[cell] <= 0.0)
        return 0.0f;
      Element element = Grid.Element[cell];
      if (element.IsVacuum)
        return 0.0f;
      return SimUtil.CalculateEnergyFlow(Grid.Temperature[cell], element.thermalConductivity, dest_temp, dest_thermal_conductivity, surface_area, thickness) * (1f / 1000f);
    }

    public static float ClampEnergyTransfer(
      float dt,
      float source_temp,
      float source_mass,
      float source_specific_heat_capacity,
      float dest_temp,
      float dest_mass,
      float dest_specific_heat_capacity,
      float max_watts_transferred)
    {
      return SimUtil.ClampEnergyTransfer(dt, source_temp, source_mass * source_specific_heat_capacity, dest_temp, dest_mass * dest_specific_heat_capacity, max_watts_transferred);
    }

    public static float ClampEnergyTransfer(
      float dt,
      float source_temp,
      float source_heat_capacity,
      float dest_temp,
      float dest_heat_capacity,
      float max_watts_transferred)
    {
      float num1 = (float) ((double) max_watts_transferred * (double) dt / 1000.0);
      SimUtil.CheckValidValue(num1);
      float min = Math.Min(source_temp, dest_temp);
      float max = Math.Max(source_temp, dest_temp);
      float num2 = source_temp - num1 / source_heat_capacity;
      float num3 = dest_temp + num1 / dest_heat_capacity;
      SimUtil.CheckValidValue(num2);
      SimUtil.CheckValidValue(num3);
      float num4 = Mathf.Clamp(num2, min, max);
      float num5 = Mathf.Clamp(num3, min, max);
      float num6 = Math.Abs(num4 - source_temp);
      float num7 = Math.Abs(num5 - dest_temp);
      float num8 = Math.Min(num6 * source_heat_capacity, num7 * dest_heat_capacity) * ((double) max_watts_transferred >= 0.0 ? 1f : -1f);
      SimUtil.CheckValidValue(num8);
      return num8;
    }

    private static float GetMassAreaScale(Element element)
    {
      return element.IsGas ? 10f : 0.01f;
    }

    public static float CalculateEnergyFlowCreatures(
      int cell,
      float creature_temperature,
      float creature_shc,
      float creature_thermal_conductivity,
      float creature_surface_area = 1f,
      float creature_surface_thickness = 1f)
    {
      return SimUtil.CalculateEnergyFlow(cell, creature_temperature, creature_shc, creature_thermal_conductivity, creature_surface_area, creature_surface_thickness);
    }

    public static float EnergyFlowToTemperatureDelta(
      float kilojoules,
      float specific_heat_capacity,
      float mass)
    {
      if ((double) kilojoules * (double) specific_heat_capacity * (double) mass == 0.0)
        return 0.0f;
      return kilojoules / (specific_heat_capacity * mass);
    }

    public static float CalculateFinalTemperature(
      float mass1,
      float temp1,
      float mass2,
      float temp2)
    {
      float num = mass1 + mass2;
      if ((double) num == 0.0)
        return 0.0f;
      float val2 = (mass1 * temp1 + mass2 * temp2) / num;
      float val1_1;
      float val1_2;
      if ((double) temp1 > (double) temp2)
      {
        val1_1 = temp2;
        val1_2 = temp1;
      }
      else
      {
        val1_1 = temp1;
        val1_2 = temp2;
      }
      return Math.Max(val1_1, Math.Min(val1_2, val2));
    }

    [Conditional("STRICT_CHECKING")]
    public static void CheckValidValue(float value)
    {
      if (float.IsNaN(value) || !float.IsInfinity(value))
        ;
    }

    public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(
      SimUtil.DiseaseInfo a,
      SimUtil.DiseaseInfo b)
    {
      return SimUtil.CalculateFinalDiseaseInfo(a.idx, a.count, b.idx, b.count);
    }

    public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(
      byte src1_idx,
      int src1_count,
      byte src2_idx,
      int src2_count)
    {
      SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo();
      if ((int) src1_idx == (int) src2_idx)
      {
        diseaseInfo.idx = src1_idx;
        diseaseInfo.count = src1_count + src2_count;
      }
      else if (src1_idx == byte.MaxValue)
      {
        diseaseInfo.idx = src2_idx;
        diseaseInfo.count = src2_count;
      }
      else if (src2_idx == byte.MaxValue)
      {
        diseaseInfo.idx = src1_idx;
        diseaseInfo.count = src1_count;
      }
      else
      {
        Klei.AI.Disease disease1 = Db.Get().Diseases[(int) src1_idx];
        Klei.AI.Disease disease2 = Db.Get().Diseases[(int) src2_idx];
        float num1 = disease1.strength * (float) src1_count;
        float num2 = disease2.strength * (float) src2_count;
        if ((double) num1 > (double) num2)
        {
          int num3 = (int) ((double) src2_count - (double) num1 / (double) num2 * (double) src1_count);
          if (num3 < 0)
          {
            diseaseInfo.idx = src1_idx;
            diseaseInfo.count = -num3;
          }
          else
          {
            diseaseInfo.idx = src2_idx;
            diseaseInfo.count = num3;
          }
        }
        else
        {
          int num3 = (int) ((double) src1_count - (double) num2 / (double) num1 * (double) src2_count);
          if (num3 < 0)
          {
            diseaseInfo.idx = src2_idx;
            diseaseInfo.count = -num3;
          }
          else
          {
            diseaseInfo.idx = src1_idx;
            diseaseInfo.count = num3;
          }
        }
      }
      if (diseaseInfo.count <= 0)
      {
        diseaseInfo.count = 0;
        diseaseInfo.idx = byte.MaxValue;
      }
      return diseaseInfo;
    }

    public static byte DiseaseCountToAlpha254(int count)
    {
      return (byte) ((double) (Math.Max(0.0f, Math.Max(0.0f, Math.Min(1f, Mathf.Log((float) count, 10f) / SimUtil.MAX_DISEASE_LOG_RANGE)) - SimUtil.MIN_DISEASE_LOG_SUBTRACTION / SimUtil.MAX_DISEASE_LOG_RANGE) / (float) (1.0 - (double) SimUtil.MIN_DISEASE_LOG_SUBTRACTION / (double) SimUtil.MAX_DISEASE_LOG_RANGE)) * 254.0);
    }

    public static float DiseaseCountToAlpha(int count)
    {
      return (float) SimUtil.DiseaseCountToAlpha254(count) / (float) byte.MaxValue;
    }

    public static SimUtil.DiseaseInfo GetPercentOfDisease(PrimaryElement pe, float percent)
    {
      return new SimUtil.DiseaseInfo()
      {
        idx = pe.DiseaseIdx,
        count = (int) ((double) pe.DiseaseCount * (double) percent)
      };
    }

    public struct DiseaseInfo
    {
      public static readonly SimUtil.DiseaseInfo Invalid = new SimUtil.DiseaseInfo()
      {
        idx = byte.MaxValue,
        count = 0
      };
      public byte idx;
      public int count;
    }
  }
}
