// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElemExposureInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
  public struct ElemExposureInfo
  {
    public float populationHalfLife;

    public void Write(BinaryWriter writer)
    {
      writer.Write(this.populationHalfLife);
    }

    public static void SetBulk(
      ElemExposureInfo[] info,
      Func<Element, bool> test,
      ElemExposureInfo settings)
    {
      List<Element> elements = ElementLoader.elements;
      for (int index = 0; index < elements.Count; ++index)
      {
        if (test(elements[index]))
          info[index] = settings;
      }
    }

    public float CalculateExposureDiseaseCountDelta(int disease_count, float dt)
    {
      return (Klei.AI.Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float) disease_count;
    }
  }
}
