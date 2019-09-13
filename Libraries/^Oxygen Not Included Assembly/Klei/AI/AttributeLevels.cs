// Decompiled with JetBrains decompiler
// Type: Klei.AI.AttributeLevels
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class AttributeLevels : KMonoBehaviour, ISaveLoadable
  {
    private List<AttributeLevel> levels = new List<AttributeLevel>();
    [Serialize]
    private AttributeLevels.LevelSaveLoad[] saveLoadLevels = new AttributeLevels.LevelSaveLoad[0];

    public IEnumerator<AttributeLevel> GetEnumerator()
    {
      return (IEnumerator<AttributeLevel>) this.levels.GetEnumerator();
    }

    public AttributeLevels.LevelSaveLoad[] SaveLoadLevels
    {
      get
      {
        return this.saveLoadLevels;
      }
      set
      {
        this.saveLoadLevels = value;
      }
    }

    protected override void OnPrefabInit()
    {
      foreach (AttributeInstance attribute in this.GetAttributes())
      {
        if (attribute.Attribute.IsTrainable)
        {
          AttributeLevel attributeLevel = new AttributeLevel(attribute);
          this.levels.Add(attributeLevel);
          attributeLevel.Apply(this);
        }
      }
    }

    [OnSerializing]
    public void OnSerializing()
    {
      this.saveLoadLevels = new AttributeLevels.LevelSaveLoad[this.levels.Count];
      for (int index = 0; index < this.levels.Count; ++index)
      {
        this.saveLoadLevels[index].attributeId = this.levels[index].attribute.Attribute.Id;
        this.saveLoadLevels[index].experience = this.levels[index].experience;
        this.saveLoadLevels[index].level = this.levels[index].level;
      }
    }

    [OnDeserialized]
    public void OnDeserialized()
    {
      foreach (AttributeLevels.LevelSaveLoad saveLoadLevel in this.saveLoadLevels)
      {
        this.SetExperience(saveLoadLevel.attributeId, saveLoadLevel.experience);
        this.SetLevel(saveLoadLevel.attributeId, saveLoadLevel.level);
      }
    }

    public int GetLevel(Attribute attribute)
    {
      foreach (AttributeLevel level in this.levels)
      {
        if (attribute == level.attribute.Attribute)
          return level.GetLevel();
      }
      return 1;
    }

    public AttributeLevel GetAttributeLevel(string attribute_id)
    {
      foreach (AttributeLevel level in this.levels)
      {
        if (level.attribute.Attribute.Id == attribute_id)
          return level;
      }
      return (AttributeLevel) null;
    }

    public bool AddExperience(string attribute_id, float time_spent, float multiplier)
    {
      AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
      if (attributeLevel == null)
      {
        Debug.LogWarning((object) (attribute_id + " has no level."));
        return false;
      }
      time_spent *= multiplier;
      AttributeConverterInstance converterInstance = Db.Get().AttributeConverters.TrainingSpeed.Lookup((Component) this);
      if (converterInstance != null)
      {
        float num = converterInstance.Evaluate();
        time_spent += time_spent * num;
      }
      bool flag = attributeLevel.AddExperience(this, time_spent);
      attributeLevel.Apply(this);
      return flag;
    }

    public void SetLevel(string attribute_id, int level)
    {
      AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
      if (attributeLevel == null)
        return;
      attributeLevel.SetLevel(level);
      attributeLevel.Apply(this);
    }

    public void SetExperience(string attribute_id, float experience)
    {
      AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
      if (attributeLevel == null)
        return;
      attributeLevel.SetExperience(experience);
      attributeLevel.Apply(this);
    }

    public float GetPercentComplete(string attribute_id)
    {
      return this.GetAttributeLevel(attribute_id).GetPercentComplete();
    }

    public int GetMaxLevel()
    {
      int num = 0;
      foreach (AttributeLevel attributeLevel in this)
      {
        if (attributeLevel.GetLevel() > num)
          num = attributeLevel.GetLevel();
      }
      return num;
    }

    [Serializable]
    public struct LevelSaveLoad
    {
      public string attributeId;
      public float experience;
      public int level;
    }
  }
}
