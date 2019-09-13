// Decompiled with JetBrains decompiler
// Type: StructureTemperaturePayload
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public struct StructureTemperaturePayload
{
  public int simHandleCopy;
  public bool enabled;
  public bool bypass;
  public bool isActiveStatusItemSet;
  public bool overrideExtents;
  private PrimaryElement primaryElementBacking;
  public Overheatable overheatable;
  public Building building;
  public Operational operational;
  public List<StructureTemperaturePayload.EnergySource> energySourcesKW;
  public float pendingEnergyModifications;
  public float maxTemperature;
  public Extents overriddenExtents;

  public StructureTemperaturePayload(GameObject go)
  {
    this.simHandleCopy = -1;
    this.enabled = true;
    this.bypass = false;
    this.overrideExtents = false;
    this.overriddenExtents = new Extents();
    this.primaryElementBacking = go.GetComponent<PrimaryElement>();
    this.overheatable = !((Object) this.primaryElementBacking != (Object) null) ? (Overheatable) null : this.primaryElementBacking.GetComponent<Overheatable>();
    this.building = go.GetComponent<Building>();
    this.operational = go.GetComponent<Operational>();
    this.pendingEnergyModifications = 0.0f;
    this.maxTemperature = 10000f;
    this.energySourcesKW = (List<StructureTemperaturePayload.EnergySource>) null;
    this.isActiveStatusItemSet = false;
  }

  public PrimaryElement primaryElement
  {
    get
    {
      return this.primaryElementBacking;
    }
    set
    {
      if (!((Object) this.primaryElementBacking != (Object) value))
        return;
      this.primaryElementBacking = value;
      this.overheatable = this.primaryElementBacking.GetComponent<Overheatable>();
    }
  }

  public float TotalEnergyProducedKW
  {
    get
    {
      if (this.energySourcesKW == null || this.energySourcesKW.Count == 0)
        return 0.0f;
      float num = 0.0f;
      for (int index = 0; index < this.energySourcesKW.Count; ++index)
        num += this.energySourcesKW[index].value;
      return num;
    }
  }

  public void OverrideExtents(Extents newExtents)
  {
    this.overrideExtents = true;
    this.overriddenExtents = newExtents;
  }

  public Extents GetExtents()
  {
    if (this.overrideExtents)
      return this.overriddenExtents;
    return this.building.GetExtents();
  }

  public float Temperature
  {
    get
    {
      return this.primaryElement.Temperature;
    }
  }

  public float ExhaustKilowatts
  {
    get
    {
      return this.building.Def.ExhaustKilowattsWhenActive;
    }
  }

  public float OperatingKilowatts
  {
    get
    {
      if ((Object) this.operational != (Object) null && this.operational.IsActive)
        return this.building.Def.SelfHeatKilowattsWhenActive;
      return 0.0f;
    }
  }

  public class EnergySource
  {
    public string source;
    public RunningAverage kw_accumulator;

    public EnergySource(float kj, string source)
    {
      this.source = source;
      this.kw_accumulator = new RunningAverage(float.MinValue, float.MaxValue, Mathf.RoundToInt(186f), true);
    }

    public float value
    {
      get
      {
        return this.kw_accumulator.AverageValue;
      }
    }

    public void Accumulate(float value)
    {
      this.kw_accumulator.AddSample(value);
    }
  }
}
