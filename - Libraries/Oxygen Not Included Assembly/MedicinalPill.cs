// Decompiled with JetBrains decompiler
// Type: MedicinalPill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MedicinalPill : Workable, IGameObjectEffectDescriptor, IConsumableUIItem
{
  public MedicineInfo info;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(10f);
    this.showProgressBar = false;
    this.synchronizeAnims = false;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, (object) null);
    this.CreateChore();
  }

  protected override void OnCompleteWork(Worker worker)
  {
    if (!string.IsNullOrEmpty(this.info.effect))
    {
      Effects component = worker.GetComponent<Effects>();
      EffectInstance effectInstance = component.Get(this.info.effect);
      if (effectInstance != null)
        effectInstance.timeRemaining = effectInstance.effect.duration;
      else
        component.Add(this.info.effect, true);
    }
    Sicknesses sicknesses = worker.GetSicknesses();
    foreach (string curedSickness in this.info.curedSicknesses)
    {
      SicknessInstance sicknessInstance = sicknesses.Get(curedSickness);
      if (sicknessInstance != null)
      {
        Game.Instance.savedInfo.curedDisease = true;
        sicknessInstance.Cure();
      }
    }
    this.gameObject.DeleteObject();
  }

  private void CreateChore()
  {
    TakeMedicineChore takeMedicineChore = new TakeMedicineChore(this);
  }

  public bool CanBeTakenBy(GameObject consumer)
  {
    if (!string.IsNullOrEmpty(this.info.effect))
    {
      Effects component = consumer.GetComponent<Effects>();
      if ((Object) component == (Object) null || component.HasEffect(this.info.effect))
        return false;
    }
    if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
      return true;
    Sicknesses sicknesses = consumer.GetSicknesses();
    if (this.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
      return true;
    foreach (ModifierInstance<Sickness> modifierInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
    {
      if (this.info.curedSicknesses.Contains(modifierInstance.modifier.Id))
        return true;
    }
    return false;
  }

  public List<Descriptor> EffectDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    switch (this.info.medicineType)
    {
      case MedicineInfo.MedicineType.Booster:
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.BOOSTER), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.BOOSTER_TOOLTIP), Descriptor.DescriptorType.Effect, false));
        break;
      case MedicineInfo.MedicineType.CureAny:
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_ANY), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_ANY_TOOLTIP), Descriptor.DescriptorType.Effect, false));
        break;
      case MedicineInfo.MedicineType.CureSpecific:
        List<string> stringList = new List<string>();
        foreach (string curedSickness in this.info.curedSicknesses)
          stringList.Add((string) Strings.Get("STRINGS.DUPLICANTS.DISEASES." + curedSickness.ToUpper() + ".NAME"));
        string str = string.Join(",", stringList.ToArray());
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES, (object) str), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_TOOLTIP, (object) str), Descriptor.DescriptorType.Effect, false));
        break;
    }
    if (!string.IsNullOrEmpty(this.info.effect))
    {
      Effect effect = Db.Get().effects.Get(this.info.effect);
      descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.MODIFIERS.MEDICINE_GENERICPILL.EFFECT_DESC, (object) effect.Name), string.Format("{0}\n{1}", (object) effect.description, (object) Effect.CreateTooltip(effect, true, "\n")), Descriptor.DescriptorType.Effect, false));
    }
    return descriptorList;
  }

  public new List<Descriptor> GetDescriptors(GameObject go)
  {
    return this.EffectDescriptors(go);
  }

  public string ConsumableId
  {
    get
    {
      return this.PrefabID().Name;
    }
  }

  public string ConsumableName
  {
    get
    {
      return this.GetProperName();
    }
  }

  public int MajorOrder
  {
    get
    {
      return (int) (this.info.medicineType + 1000);
    }
  }

  public int MinorOrder
  {
    get
    {
      return 0;
    }
  }

  public bool Display
  {
    get
    {
      return true;
    }
  }
}
