// Decompiled with JetBrains decompiler
// Type: Klei.AI.Effects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
  {
    private List<EffectInstance> effects = new List<EffectInstance>();
    private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();
    private List<Effect> effectImmunites = new List<Effect>();
    [Serialize]
    private Effects.SaveLoadEffect[] saveLoadEffects;

    protected override void OnPrefabInit()
    {
      this.autoRegisterSimRender = false;
    }

    protected override void OnSpawn()
    {
      if (this.saveLoadEffects != null)
      {
        foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
        {
          if (Db.Get().effects.Exists(saveLoadEffect.id))
          {
            EffectInstance effectInstance = this.Add(Db.Get().effects.Get(saveLoadEffect.id), true);
            if (effectInstance != null)
              effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
          }
        }
      }
      if (this.effectsThatExpire.Count <= 0)
        return;
      SimAndRenderScheduler.instance.Add((object) this, this.simRenderLoadBalance);
    }

    public EffectInstance Get(string effect_id)
    {
      foreach (EffectInstance effect in this.effects)
      {
        if (effect.effect.Id == effect_id)
          return effect;
      }
      return (EffectInstance) null;
    }

    public EffectInstance Get(Effect effect)
    {
      foreach (EffectInstance effect1 in this.effects)
      {
        if (effect1.effect == effect)
          return effect1;
      }
      return (EffectInstance) null;
    }

    public EffectInstance Add(string effect_id, bool should_save)
    {
      return this.Add(Db.Get().effects.Get(effect_id), should_save);
    }

    public EffectInstance Add(Effect effect, bool should_save)
    {
      if (this.effectImmunites.Contains(effect))
        return (EffectInstance) null;
      bool flag = true;
      Traits component = this.GetComponent<Traits>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        foreach (Trait trait in component.TraitList)
        {
          if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
          {
            flag = false;
            break;
          }
        }
      }
      if (!flag)
        return (EffectInstance) null;
      Attributes attributes = this.GetAttributes();
      EffectInstance effectInstance = this.Get(effect);
      if (!string.IsNullOrEmpty(effect.stompGroup))
      {
        for (int index = this.effects.Count - 1; index >= 0; --index)
        {
          if (this.effects[index] != effectInstance && this.effects[index].effect.stompGroup == effect.stompGroup)
            this.Remove(this.effects[index].effect);
        }
      }
      if (effectInstance == null)
      {
        effectInstance = new EffectInstance(this.gameObject, effect, should_save);
        effect.AddTo(attributes);
        this.effects.Add(effectInstance);
        if ((double) effect.duration > 0.0)
        {
          this.effectsThatExpire.Add(effectInstance);
          if (this.effectsThatExpire.Count == 1)
            SimAndRenderScheduler.instance.Add((object) this, this.simRenderLoadBalance);
        }
        this.Trigger(-1901442097, (object) effect);
      }
      effectInstance.timeRemaining = effect.duration;
      return effectInstance;
    }

    public void Remove(Effect effect)
    {
      this.Remove(effect.Id);
    }

    public void Remove(string effect_id)
    {
      for (int index1 = 0; index1 < this.effectsThatExpire.Count; ++index1)
      {
        if (this.effectsThatExpire[index1].effect.Id == effect_id)
        {
          int index2 = this.effectsThatExpire.Count - 1;
          this.effectsThatExpire[index1] = this.effectsThatExpire[index2];
          this.effectsThatExpire.RemoveAt(index2);
          if (this.effectsThatExpire.Count == 0)
          {
            SimAndRenderScheduler.instance.Remove((object) this);
            break;
          }
          break;
        }
      }
      for (int index1 = 0; index1 < this.effects.Count; ++index1)
      {
        if (this.effects[index1].effect.Id == effect_id)
        {
          Attributes attributes = this.GetAttributes();
          EffectInstance effect1 = this.effects[index1];
          effect1.OnCleanUp();
          Effect effect2 = effect1.effect;
          effect2.RemoveFrom(attributes);
          int index2 = this.effects.Count - 1;
          this.effects[index1] = this.effects[index2];
          this.effects.RemoveAt(index2);
          this.Trigger(-1157678353, (object) effect2);
          break;
        }
      }
    }

    public bool HasEffect(string effect_id)
    {
      foreach (EffectInstance effect in this.effects)
      {
        if (effect.effect.Id == effect_id)
          return true;
      }
      return false;
    }

    public bool HasEffect(Effect effect)
    {
      foreach (EffectInstance effect1 in this.effects)
      {
        if (effect1.effect == effect)
          return true;
      }
      return false;
    }

    public void Sim1000ms(float dt)
    {
      for (int index = 0; index < this.effectsThatExpire.Count; ++index)
      {
        EffectInstance effectInstance = this.effectsThatExpire[index];
        if (effectInstance.IsExpired())
          this.Remove(effectInstance.effect);
        effectInstance.timeRemaining -= dt;
      }
    }

    public void AddImmunity(Effect effect)
    {
      this.effectImmunites.Add(effect);
    }

    public void RemoveImmunity(Effect effect)
    {
      this.effectImmunites.Remove(effect);
    }

    [OnSerializing]
    internal void OnSerializing()
    {
      List<Effects.SaveLoadEffect> saveLoadEffectList = new List<Effects.SaveLoadEffect>();
      foreach (EffectInstance effect in this.effects)
      {
        if (effect.shouldSave)
        {
          Effects.SaveLoadEffect saveLoadEffect = new Effects.SaveLoadEffect()
          {
            id = effect.effect.Id,
            timeRemaining = effect.timeRemaining
          };
          saveLoadEffectList.Add(saveLoadEffect);
        }
      }
      this.saveLoadEffects = saveLoadEffectList.ToArray();
    }

    public List<EffectInstance> GetTimeLimitedEffects()
    {
      return this.effectsThatExpire;
    }

    [Serializable]
    private struct SaveLoadEffect
    {
      public string id;
      public float timeRemaining;
    }
  }
}
