// Decompiled with JetBrains decompiler
// Type: Health
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Health : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public bool CanBeIncapacitated;
  [Serialize]
  public Health.HealthState State;
  public HealthBar healthBar;
  private Effects effects;
  private AmountInstance amountInstance;

  public AmountInstance GetAmountInstance
  {
    get
    {
      return this.amountInstance;
    }
  }

  public float hitPoints
  {
    get
    {
      return this.amountInstance.value;
    }
    set
    {
      this.amountInstance.value = value;
    }
  }

  public float maxHitPoints
  {
    get
    {
      return this.amountInstance.GetMax();
    }
  }

  public float percent()
  {
    return this.hitPoints / this.maxHitPoints;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Health.Add(this);
    this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(this.gameObject);
    this.amountInstance.value = this.amountInstance.GetMax();
    this.amountInstance.OnDelta += new System.Action<float>(this.OnHealthChanged);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.State == Health.HealthState.Incapacitated || (double) this.hitPoints == 0.0)
    {
      if (this.CanBeIncapacitated)
        this.Incapacitate(Db.Get().Deaths.Slain);
      else
        this.Kill();
    }
    if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
      this.UpdateStatus();
    this.effects = this.GetComponent<Effects>();
    this.UpdateHealthBar();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Health.Remove(this);
  }

  public void UpdateHealthBar()
  {
    if ((UnityEngine.Object) NameDisplayScreen.Instance == (UnityEngine.Object) null)
      return;
    bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || (double) this.hitPoints >= (double) this.maxHitPoints;
    NameDisplayScreen.Instance.SetHealthDisplay(this.gameObject, new Func<float>(this.percent), !flag);
  }

  private void Recover()
  {
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
  }

  public void OnHealthChanged(float delta)
  {
    this.Trigger(-1664904872, (object) delta);
    if (this.State != Health.HealthState.Invincible)
    {
      if ((double) this.hitPoints == 0.0 && !this.IsDefeated())
      {
        if (this.CanBeIncapacitated)
          this.Incapacitate(Db.Get().Deaths.Slain);
        else
          this.Kill();
      }
      else
        this.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
    }
    this.UpdateStatus();
    this.UpdateWoundEffects();
    this.UpdateHealthBar();
  }

  public void RegisterHitReaction()
  {
    ReactionMonitor.Instance smi = this.gameObject.GetSMI<ReactionMonitor.Instance>();
    if (smi == null)
      return;
    SelfEmoteReactable reactable = new SelfEmoteReactable(this.gameObject, (HashedString) "Hit", Db.Get().ChoreTypes.Cough, (HashedString) "anim_hits_kanim", 0.0f, 1f, 1f);
    reactable.AddStep(new EmoteReactable.EmoteStep()
    {
      anim = (HashedString) "hit"
    });
    if (!this.gameObject.GetComponent<Navigator>().IsMoving())
    {
      EmoteChore emote = new EmoteChore((IStateMachineTarget) this.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, (HashedString) "anim_hits_kanim", new HashedString[1]
      {
        (HashedString) "hit"
      }, (Func<StatusItem>) null);
      emote.PairReactable(reactable);
      reactable.PairEmote(emote);
    }
    smi.AddOneshotReactable(reactable);
  }

  [ContextMenu("DoDamage")]
  public void DoDamage()
  {
    this.Damage(1f);
  }

  public void Damage(float amount)
  {
    if (this.State != Health.HealthState.Invincible)
      this.hitPoints = Mathf.Max(0.0f, this.hitPoints - amount);
    this.OnHealthChanged(-amount);
  }

  private void UpdateWoundEffects()
  {
    if (!(bool) ((UnityEngine.Object) this.effects))
      return;
    switch (this.State)
    {
      case Health.HealthState.Perfect:
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
        break;
      case Health.HealthState.Alright:
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
        break;
      case Health.HealthState.Scuffed:
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
        if (this.effects.HasEffect("LightWounds"))
          break;
        this.effects.Add("LightWounds", true);
        break;
      case Health.HealthState.Injured:
        this.effects.Remove("LightWounds");
        this.effects.Remove("SevereWounds");
        if (this.effects.HasEffect("ModerateWounds"))
          break;
        this.effects.Add("ModerateWounds", true);
        break;
      case Health.HealthState.Critical:
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        if (this.effects.HasEffect("SevereWounds"))
          break;
        this.effects.Add("SevereWounds", true);
        break;
      case Health.HealthState.Incapacitated:
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
        break;
      case Health.HealthState.Dead:
        this.effects.Remove("LightWounds");
        this.effects.Remove("ModerateWounds");
        this.effects.Remove("SevereWounds");
        break;
    }
  }

  private void UpdateStatus()
  {
    float num = this.hitPoints / this.maxHitPoints;
    Health.HealthState healthState = this.State != Health.HealthState.Invincible ? ((double) num < 1.0 ? ((double) num < 0.850000023841858 ? ((double) num < 0.660000026226044 ? ((double) num < 0.33 ? ((double) num <= 0.0 ? ((double) num != 0.0 ? Health.HealthState.Dead : Health.HealthState.Incapacitated) : Health.HealthState.Critical) : Health.HealthState.Injured) : Health.HealthState.Scuffed) : Health.HealthState.Alright) : Health.HealthState.Perfect) : Health.HealthState.Invincible;
    if (this.State == healthState)
      return;
    if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
      this.Recover();
    if (healthState == Health.HealthState.Perfect)
      this.Trigger(-1491582671, (object) this);
    this.State = healthState;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright)
      component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, (object) this.State);
    else
      component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, (StatusItem) null, (object) null);
  }

  public bool IsIncapacitated()
  {
    return this.State == Health.HealthState.Incapacitated;
  }

  public bool IsDefeated()
  {
    if (this.State != Health.HealthState.Incapacitated)
      return this.State == Health.HealthState.Dead;
    return true;
  }

  public void Incapacitate(Death source_of_death)
  {
    this.State = Health.HealthState.Incapacitated;
    this.GetComponent<KPrefabID>().AddTag(GameTags.HitPointsDepleted, false);
  }

  private void Kill()
  {
    if (this.gameObject.GetSMI<DeathMonitor.Instance>() == null)
      return;
    this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
  }

  public enum HealthState
  {
    Perfect,
    Alright,
    Scuffed,
    Injured,
    Critical,
    Incapacitated,
    Dead,
    Invincible,
  }
}
