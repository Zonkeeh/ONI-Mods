// Decompiled with JetBrains decompiler
// Type: Klei.AI.Attributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  public class Attributes
  {
    public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();
    public GameObject gameObject;

    public Attributes(GameObject game_object)
    {
      this.gameObject = game_object;
    }

    public IEnumerator<AttributeInstance> GetEnumerator()
    {
      return (IEnumerator<AttributeInstance>) this.AttributeTable.GetEnumerator();
    }

    public int Count
    {
      get
      {
        return this.AttributeTable.Count;
      }
    }

    public AttributeInstance Add(Attribute attribute)
    {
      AttributeInstance attributeInstance = this.Get(attribute.Id);
      if (attributeInstance == null)
      {
        attributeInstance = new AttributeInstance(this.gameObject, attribute);
        this.AttributeTable.Add(attributeInstance);
      }
      return attributeInstance;
    }

    public void Add(AttributeModifier modifier)
    {
      this.Get(modifier.AttributeId)?.Add(modifier);
    }

    public float GetValuePercent(string attribute_id)
    {
      float num = 1f;
      AttributeInstance attributeInstance = this.Get(attribute_id);
      if (attributeInstance != null)
        num = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
      else
        Debug.LogError((object) ("Could not find attribute " + attribute_id));
      return num;
    }

    public AttributeInstance Get(string attribute_id)
    {
      for (int index = 0; index < this.AttributeTable.Count; ++index)
      {
        if (this.AttributeTable[index].Id == attribute_id)
          return this.AttributeTable[index];
      }
      return (AttributeInstance) null;
    }

    public AttributeInstance Get(Attribute attribute)
    {
      return this.Get(attribute.Id);
    }

    public float GetValue(string id)
    {
      float num = 0.0f;
      AttributeInstance attributeInstance = this.Get(id);
      if (attributeInstance != null)
        num = attributeInstance.GetTotalValue();
      else
        Debug.LogError((object) ("Could not find attribute " + id));
      return num;
    }

    public void Remove(AttributeModifier modifier)
    {
      if (modifier == null)
        return;
      this.Get(modifier.AttributeId)?.Remove(modifier);
    }

    public AttributeInstance GetProfession()
    {
      AttributeInstance attributeInstance1 = (AttributeInstance) null;
      foreach (AttributeInstance attributeInstance2 in this)
      {
        if (attributeInstance2.modifier.IsProfession)
        {
          if (attributeInstance1 == null)
            attributeInstance1 = attributeInstance2;
          else if ((double) attributeInstance1.GetTotalValue() < (double) attributeInstance2.GetTotalValue())
            attributeInstance1 = attributeInstance2;
        }
      }
      return attributeInstance1;
    }

    public string GetProfessionString(bool longform = true)
    {
      AttributeInstance profession = this.GetProfession();
      if ((int) profession.GetTotalValue() == 0)
        return string.Format((string) (!longform ? UI.ATTRIBUTELEVEL_SHORT : UI.ATTRIBUTELEVEL), (object) 0, (object) DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
      return string.Format((string) (!longform ? UI.ATTRIBUTELEVEL_SHORT : UI.ATTRIBUTELEVEL), (object) (int) profession.GetTotalValue(), (object) profession.modifier.ProfessionName);
    }

    public string GetProfessionDescriptionString()
    {
      AttributeInstance profession = this.GetProfession();
      if ((int) profession.GetTotalValue() == 0)
        return (string) DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
      return string.Format((string) DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, (object) profession.modifier.Name);
    }
  }
}
