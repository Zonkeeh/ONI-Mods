// Decompiled with JetBrains decompiler
// Type: Klei.AI.Sickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
  [DebuggerDisplay("{base.Id}")]
  public abstract class Sickness : Resource
  {
    private float sicknessDuration = 600f;
    private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();
    private StringKey name;
    private StringKey descriptiveSymptoms;
    public float fatalityDuration;
    public HashedString id;
    public Sickness.SicknessType sicknessType;
    public Sickness.Severity severity;
    public string recoveryEffect;
    public List<Sickness.InfectionVector> infectionVectors;
    public Amount amount;
    public Attribute amountDeltaAttribute;
    public Attribute cureSpeedBase;

    public Sickness(
      string id,
      Sickness.SicknessType type,
      Sickness.Severity severity,
      float immune_attack_strength,
      List<Sickness.InfectionVector> infection_vectors,
      float sickness_duration,
      string recovery_effect = null)
      : base(id, (ResourceSet) null, (string) null)
    {
      this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
      this.id = (HashedString) id;
      this.sicknessType = type;
      this.severity = severity;
      this.infectionVectors = infection_vectors;
      this.sicknessDuration = sickness_duration;
      this.recoveryEffect = recovery_effect;
      this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
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

    public float SicknessDuration
    {
      get
      {
        return this.sicknessDuration;
      }
    }

    public StringKey DescriptiveSymptoms
    {
      get
      {
        return this.descriptiveSymptoms;
      }
    }

    public object[] Infect(
      GameObject go,
      SicknessInstance diseaseInstance,
      SicknessExposureInfo exposure_info)
    {
      object[] objArray = new object[this.components.Count];
      for (int index = 0; index < this.components.Count; ++index)
        objArray[index] = this.components[index].OnInfect(go, diseaseInstance);
      return objArray;
    }

    public void Cure(GameObject go, object[] componentData)
    {
      for (int index = 0; index < this.components.Count; ++index)
        this.components[index].OnCure(go, componentData[index]);
    }

    public List<Descriptor> GetSymptoms()
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      for (int index = 0; index < this.components.Count; ++index)
      {
        List<Descriptor> symptoms = this.components[index].GetSymptoms();
        if (symptoms != null)
          descriptorList.AddRange((IEnumerable<Descriptor>) symptoms);
      }
      if ((double) this.fatalityDuration > 0.0)
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.DEATH_SYMPTOM, (object) GameUtil.GetFormattedCycles(this.fatalityDuration, "F1")), string.Format((string) DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, (object) GameUtil.GetFormattedCycles(this.fatalityDuration, "F1")), Descriptor.DescriptorType.SymptomAidable, false));
      return descriptorList;
    }

    protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
    {
      this.components.Add(cmp);
    }

    public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
    {
      for (int index = 0; index < this.components.Count; ++index)
      {
        if (this.components[index] is T)
          return this.components[index] as T;
      }
      return (T) null;
    }

    public virtual List<Descriptor> GetSicknessSourceDescriptors()
    {
      return new List<Descriptor>();
    }

    public List<Descriptor> GetQualitativeDescriptors()
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      foreach (int infectionVector in this.infectionVectors)
      {
        switch (infectionVector)
        {
          case 0:
            descriptorList.Add(new Descriptor((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
            continue;
          case 1:
            descriptorList.Add(new Descriptor((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
            continue;
          case 2:
            descriptorList.Add(new Descriptor((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
            continue;
          case 3:
            descriptorList.Add(new Descriptor((string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, (string) DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
            continue;
          default:
            continue;
        }
      }
      descriptorList.Add(new Descriptor((string) Strings.Get(this.descriptiveSymptoms), string.Empty, Descriptor.DescriptorType.Information, false));
      return descriptorList;
    }

    public abstract class SicknessComponent
    {
      public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

      public abstract void OnCure(GameObject go, object instance_data);

      public virtual List<Descriptor> GetSymptoms()
      {
        return (List<Descriptor>) null;
      }
    }

    public enum InfectionVector
    {
      Contact,
      Digestion,
      Inhalation,
      Exposure,
    }

    public enum SicknessType
    {
      Pathogen,
      Ailment,
      Injury,
    }

    public enum Severity
    {
      Benign,
      Minor,
      Major,
      Critical,
    }
  }
}
