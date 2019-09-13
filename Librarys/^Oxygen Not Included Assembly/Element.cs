// Decompiled with JetBrains decompiler
// Type: Element
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
  public float thermalConductivity = 1f;
  public float molarMass = 1f;
  public float minHorizontalFlow = float.PositiveInfinity;
  public float minVerticalFlow = float.PositiveInfinity;
  public float maxMass = 10000f;
  public SimHashes highTempTransitionOreID = SimHashes.Vacuum;
  public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;
  public Tag[] oreTags = new Tag[0];
  public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
  public SimHashes id;
  public Tag tag;
  public byte idx;
  public float specificHeatCapacity;
  public float strength;
  public float flow;
  public float maxCompression;
  public float viscosity;
  public float solidSurfaceAreaMultiplier;
  public float liquidSurfaceAreaMultiplier;
  public float gasSurfaceAreaMultiplier;
  public Element.State state;
  public byte hardness;
  public float lowTemp;
  public SimHashes lowTempTransitionTarget;
  public Element lowTempTransition;
  public float highTemp;
  public SimHashes highTempTransitionTarget;
  public Element highTempTransition;
  public float highTempTransitionOreMassConversion;
  public float lowTempTransitionOreMassConversion;
  public SimHashes sublimateId;
  public SimHashes convertId;
  public SpawnFXHashes sublimateFX;
  public float lightAbsorptionFactor;
  public Sim.PhysicsData defaultValues;
  public float toxicity;
  public Substance substance;
  public Tag materialCategory;
  public int buildMenuSort;
  public bool disabled;
  public const byte StateMask = 3;

  public float PressureToMass(float pressure)
  {
    return pressure / this.defaultValues.pressure;
  }

  public bool IsUnstable
  {
    get
    {
      return this.HasTag(GameTags.Unstable);
    }
  }

  public bool IsLiquid
  {
    get
    {
      return (this.state & Element.State.Solid) == Element.State.Liquid;
    }
  }

  public bool IsGas
  {
    get
    {
      return (this.state & Element.State.Solid) == Element.State.Gas;
    }
  }

  public bool IsSolid
  {
    get
    {
      return (this.state & Element.State.Solid) == Element.State.Solid;
    }
  }

  public bool IsVacuum
  {
    get
    {
      return (this.state & Element.State.Solid) == Element.State.Vacuum;
    }
  }

  public bool IsTemperatureInsulated
  {
    get
    {
      return (this.state & Element.State.TemperatureInsulated) != Element.State.Vacuum;
    }
  }

  public bool IsState(Element.State expected_state)
  {
    return (this.state & Element.State.Solid) == expected_state;
  }

  public bool HasTransitionUp
  {
    get
    {
      if (this.highTempTransitionTarget != (SimHashes) 0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null)
        return this.highTempTransition != this;
      return false;
    }
  }

  public string name { get; set; }

  public string nameUpperCase { get; set; }

  public string description { get; set; }

  public string GetStateString()
  {
    return Element.GetStateString(this.state);
  }

  public static string GetStateString(Element.State state)
  {
    if ((state & Element.State.Solid) == Element.State.Solid)
      return (string) ELEMENTS.STATE.SOLID;
    if ((state & Element.State.Solid) == Element.State.Liquid)
      return (string) ELEMENTS.STATE.LIQUID;
    if ((state & Element.State.Solid) == Element.State.Gas)
      return (string) ELEMENTS.STATE.GAS;
    return (string) ELEMENTS.STATE.VACUUM;
  }

  public string FullDescription(bool addHardnessColor = true)
  {
    string str1 = this.Description();
    if (this.IsSolid)
      str1 = str1 + "\n\n" + string.Format((string) ELEMENTS.ELEMENTDESCSOLID, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetHardnessString(this, addHardnessColor));
    else if (this.IsLiquid)
      str1 = str1 + "\n\n" + string.Format((string) ELEMENTS.ELEMENTDESCLIQUID, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), (object) GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
    else if (!this.IsVacuum)
      str1 = str1 + "\n\n" + string.Format((string) ELEMENTS.ELEMENTDESCGAS, (object) this.GetMaterialCategoryTag().ProperName(), (object) GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
    string str2 = (string) ELEMENTS.THERMALPROPERTIES.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity)).Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
    string str3 = str1 + "\n" + str2;
    if (this.oreTags.Length > 0 && !this.IsVacuum)
    {
      string str4 = str3 + "\n\n";
      string empty = string.Empty;
      for (int index = 0; index < this.oreTags.Length; ++index)
      {
        Tag tag = new Tag(this.oreTags[index]);
        empty += tag.ProperName();
        if (index < this.oreTags.Length - 1)
          empty += ", ";
      }
      str3 = str4 + string.Format((string) ELEMENTS.ELEMENTPROPERTIES, (object) empty);
    }
    if (this.attributeModifiers.Count > 0)
    {
      foreach (AttributeModifier attributeModifier in this.attributeModifiers)
      {
        string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString((GameObject) null);
        str3 = str3 + "\n" + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
      }
    }
    return str3;
  }

  public string Description()
  {
    return this.description;
  }

  public bool HasTag(Tag search_tag)
  {
    if (this.tag == search_tag)
      return true;
    return Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
  }

  public Tag GetMaterialCategoryTag()
  {
    return this.materialCategory;
  }

  public int CompareTo(Element other)
  {
    return this.id - other.id;
  }

  [Serializable]
  public enum State : byte
  {
    Vacuum = 0,
    Gas = 1,
    Liquid = 2,
    Solid = 3,
    Unbreakable = 4,
    Unstable = 8,
    TemperatureInsulated = 16, // 0x10
  }
}
