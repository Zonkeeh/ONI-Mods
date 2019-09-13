// Decompiled with JetBrains decompiler
// Type: MinionResume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class MinionResume : KMonoBehaviour, ISaveLoadable, ISim200ms
{
  [Serialize]
  public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();
  [Serialize]
  public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();
  [Serialize]
  public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();
  [Serialize]
  public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();
  [Serialize]
  private string currentRole = "NoRole";
  [Serialize]
  private string targetRole = "NoRole";
  private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();
  [MyCmpReq]
  private MinionIdentity identity;
  [Serialize]
  private string currentHat;
  [Serialize]
  private string targetHat;
  [Serialize]
  private float totalExperienceGained;
  private Notification lastSkillNotification;
  private AttributeModifier skillsMoraleExpectationModifier;
  private AttributeModifier skillsMoraleModifier;
  public float DEBUG_PassiveExperienceGained;
  public float DEBUG_ActiveExperienceGained;
  public float DEBUG_SecondsAlive;

  public MinionIdentity GetIdentity
  {
    get
    {
      return this.identity;
    }
  }

  public float TotalExperienceGained
  {
    get
    {
      return this.totalExperienceGained;
    }
  }

  public int TotalSkillPointsGained
  {
    get
    {
      return MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
    }
  }

  public static int CalculateTotalSkillPointsGained(float experience)
  {
    return Mathf.FloorToInt(Mathf.Pow((float) ((double) experience / (double) SKILLS.TARGET_SKILLS_CYCLE / 600.0), 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float) SKILLS.TARGET_SKILLS_EARNED);
  }

  public int SkillsMastered
  {
    get
    {
      int num = 0;
      foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
      {
        if (keyValuePair.Value)
          ++num;
      }
      return num;
    }
  }

  public int AvailableSkillpoints
  {
    get
    {
      return this.TotalSkillPointsGained - this.SkillsMastered;
    }
  }

  [OnDeserialized]
  private void OnDeserializedMethod()
  {
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
      return;
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
    {
      if (keyValuePair.Value && keyValuePair.Key != "NoRole")
        this.ForceAddSkillPoint();
    }
    foreach (KeyValuePair<HashedString, float> keyValuePair in this.AptitudeByRoleGroup)
      this.AptitudeBySkillGroup[keyValuePair.Key] = keyValuePair.Value;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.MinionResumes.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        foreach (SkillPerk perk in skill.perks)
        {
          if (perk.OnRemove != null)
            perk.OnRemove(this);
          if (perk.OnApply != null)
            perk.OnApply(this);
        }
        if (!this.ownedHats.ContainsKey(skill.hat))
          this.ownedHats.Add(skill.hat, true);
      }
    }
    this.UpdateExpectations();
    this.UpdateMorale();
    MinionResume.ApplyHat(this.currentHat, this.GetComponent<KBatchedAnimController>());
  }

  public void RestoreResume(
    Dictionary<string, bool> MasteryBySkillID,
    Dictionary<HashedString, float> AptitudeBySkillGroup,
    float totalExperienceGained)
  {
    this.MasteryBySkillID = MasteryBySkillID;
    this.AptitudeBySkillGroup = AptitudeBySkillGroup;
    this.totalExperienceGained = totalExperienceGained;
  }

  protected override void OnCleanUp()
  {
    Components.MinionResumes.Remove(this);
    base.OnCleanUp();
  }

  public bool HasMasteredSkill(string skillId)
  {
    if (this.MasteryBySkillID.ContainsKey(skillId))
      return this.MasteryBySkillID[skillId];
    return false;
  }

  public void UpdateUrge()
  {
    if (this.targetHat != this.currentHat)
    {
      if (this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
        return;
      this.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
    }
    else
      this.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
  }

  public string CurrentRole
  {
    get
    {
      return this.currentRole;
    }
  }

  public string CurrentHat
  {
    get
    {
      return this.currentHat;
    }
  }

  public string TargetHat
  {
    get
    {
      return this.targetHat;
    }
  }

  public void SetHats(string current, string target)
  {
    this.currentHat = current;
    this.targetHat = target;
  }

  public void SetCurrentRole(string role_id)
  {
    this.currentRole = role_id;
  }

  public string TargetRole
  {
    get
    {
      return this.targetRole;
    }
  }

  private void ApplySkillPerks(string skillId)
  {
    foreach (SkillPerk perk in Db.Get().Skills.Get(skillId).perks)
    {
      if (perk.OnApply != null)
        perk.OnApply(this);
    }
  }

  private void RemoveSkillPerks(string skillId)
  {
    foreach (SkillPerk perk in Db.Get().Skills.Get(skillId).perks)
    {
      if (perk.OnRemove != null)
        perk.OnRemove(this);
    }
  }

  public void Sim200ms(float dt)
  {
    this.DEBUG_SecondsAlive += dt;
    if (this.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
      return;
    this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
    this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
  }

  public bool IsAbleToLearnSkill(string skillId)
  {
    string choreGroupId = Db.Get().SkillGroups.Get(Db.Get().Skills.Get(skillId).skillGroup).choreGroupID;
    if (!string.IsNullOrEmpty(choreGroupId))
    {
      foreach (Trait trait in this.GetComponent<Traits>().TraitList)
      {
        if (trait.disabledChoreGroups != null)
        {
          foreach (Resource disabledChoreGroup in trait.disabledChoreGroups)
          {
            if (disabledChoreGroup.Id == choreGroupId)
              return false;
          }
        }
      }
    }
    return true;
  }

  public bool BelowMoraleExpectation(Skill skill)
  {
    float totalValue1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) this).GetTotalValue();
    float totalValue2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) this).GetTotalValue();
    int moraleExpectation = skill.GetMoraleExpectation();
    if (this.AptitudeBySkillGroup.ContainsKey((HashedString) skill.skillGroup) && (double) this.AptitudeBySkillGroup[(HashedString) skill.skillGroup] > 0.0)
      ++totalValue1;
    return (double) totalValue2 + (double) moraleExpectation <= (double) totalValue1;
  }

  public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
  {
    for (int index = 0; index < skill.priorSkills.Count; ++index)
    {
      if (!this.HasMasteredSkill(skill.priorSkills[index]))
        return false;
    }
    return true;
  }

  public bool HasSkillPointsRequiredForSkill(Skill skill)
  {
    return this.AvailableSkillpoints >= 1;
  }

  public bool HasSkillAptitude(Skill skill)
  {
    return this.AptitudeBySkillGroup.ContainsKey((HashedString) skill.skillGroup) && (double) this.AptitudeBySkillGroup[(HashedString) skill.skillGroup] > 0.0;
  }

  public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
  {
    List<MinionResume.SkillMasteryConditions> masteryConditionsList = new List<MinionResume.SkillMasteryConditions>();
    Skill skill = Db.Get().Skills.Get(skillId);
    if (this.HasSkillAptitude(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
    if (!this.BelowMoraleExpectation(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.StressWarning);
    if (!this.IsAbleToLearnSkill(skillId))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
    if (!this.HasSkillPointsRequiredForSkill(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
    if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
    return masteryConditionsList.ToArray();
  }

  public bool CanMasterSkill(
    MinionResume.SkillMasteryConditions[] masteryConditions)
  {
    return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element =>
    {
      if (element != MinionResume.SkillMasteryConditions.UnableToLearn && element != MinionResume.SkillMasteryConditions.NeedsSkillPoints)
        return element == MinionResume.SkillMasteryConditions.MissingPreviousSkill;
      return true;
    }));
  }

  public bool OwnsHat(string hatId)
  {
    if (this.ownedHats.ContainsKey(hatId))
      return this.ownedHats[hatId];
    return false;
  }

  public void SkillLearned()
  {
    if (this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
      this.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
    foreach (string index in this.ownedHats.Keys.ToList<string>())
      this.ownedHats[index] = true;
    if (this.targetHat == null || !(this.currentHat != this.targetHat))
      return;
    PutOnHatChore putOnHatChore = new PutOnHatChore((IStateMachineTarget) this, Db.Get().ChoreTypes.SwitchHat);
  }

  public void MasterSkill(string skillId)
  {
    if (!this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
      this.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
    this.MasteryBySkillID[skillId] = true;
    this.ApplySkillPerks(skillId);
    this.UpdateExpectations();
    this.UpdateMorale();
    this.TriggerMasterSkillEvents();
    if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
      this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
    if (this.AvailableSkillpoints != 0 || this.lastSkillNotification == null)
      return;
    Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
    this.lastSkillNotification = (Notification) null;
  }

  public void UnmasterSkill(string skillId)
  {
    if (!this.MasteryBySkillID.ContainsKey(skillId))
      return;
    this.MasteryBySkillID.Remove(skillId);
    this.RemoveSkillPerks(skillId);
    this.UpdateExpectations();
    this.UpdateMorale();
    this.TriggerMasterSkillEvents();
  }

  private void TriggerMasterSkillEvents()
  {
    this.Trigger(540773776, (object) null);
    Game.Instance.Trigger(-1523247426, (object) this);
  }

  public void ForceAddSkillPoint()
  {
    this.AddExperience(MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained) - this.totalExperienceGained);
  }

  public static float CalculateNextExperienceBar(int current_skill_points)
  {
    return (float) ((double) Mathf.Pow((float) (current_skill_points + 1) / (float) SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (double) SKILLS.TARGET_SKILLS_CYCLE * 600.0);
  }

  public static float CalculatePreviousExperienceBar(int current_skill_points)
  {
    return (float) ((double) Mathf.Pow((float) current_skill_points / (float) SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (double) SKILLS.TARGET_SKILLS_CYCLE * 600.0);
  }

  private void UpdateExpectations()
  {
    int num = 0;
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        num += skill.tier + 1;
      }
    }
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) this);
    if (this.skillsMoraleExpectationModifier != null)
    {
      attributeInstance.Remove(this.skillsMoraleExpectationModifier);
      this.skillsMoraleExpectationModifier = (AttributeModifier) null;
    }
    if (num <= 0)
      return;
    this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float) num, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
    attributeInstance.Add(this.skillsMoraleExpectationModifier);
  }

  private void UpdateMorale()
  {
    int num1 = 0;
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        float num2 = 0.0f;
        if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
          num1 += (int) num2;
      }
    }
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) this);
    if (this.skillsMoraleModifier != null)
    {
      attributeInstance.Remove(this.skillsMoraleModifier);
      this.skillsMoraleModifier = (AttributeModifier) null;
    }
    if (num1 <= 0)
      return;
    this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float) num1, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME, false, false, true);
    attributeInstance.Add(this.skillsMoraleModifier);
  }

  private void OnSkillPointGained()
  {
    Game.Instance.Trigger(1505456302, (object) this);
    if (this.AvailableSkillpoints == 1)
    {
      this.lastSkillNotification = new Notification((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME, NotificationType.Good, HashedString.Invalid, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), (object) null, true, 0.0f, (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenSkills(this.identity)), (object) null, (Transform) null);
      Game.Instance.GetComponent<Notifier>().Add(this.lastSkillNotification, string.Empty);
    }
    if ((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME, this.transform, new Vector3(0.0f, 0.5f, 0.0f), 1.5f, false, false);
    new UpgradeFX.Instance((IStateMachineTarget) this.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, -0.1f)).StartSM();
  }

  private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
  {
    return string.Format((string) MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP);
  }

  public void SetAptitude(HashedString skillGroupID, float amount)
  {
    this.AptitudeBySkillGroup[skillGroupID] = amount;
  }

  public float GetAptitudeExperienceMultiplier(
    HashedString skillGroupId,
    float buildingFrequencyMultiplier)
  {
    float num = 0.0f;
    this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
    return (float) (1.0 + (double) num * (double) SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * (double) buildingFrequencyMultiplier);
  }

  public void AddExperience(float amount)
  {
    float experienceGained = this.totalExperienceGained;
    float nextExperienceBar = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
    this.totalExperienceGained += amount;
    if ((double) this.totalExperienceGained < (double) nextExperienceBar || (double) experienceGained >= (double) nextExperienceBar)
      return;
    this.OnSkillPointGained();
  }

  public void AddExperienceWithAptitude(
    string skillGroupId,
    float amount,
    float buildingMultiplier)
  {
    float amount1 = amount * this.GetAptitudeExperienceMultiplier((HashedString) skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
    this.DEBUG_ActiveExperienceGained += amount1;
    this.AddExperience(amount1);
  }

  public bool HasPerk(HashedString perkId)
  {
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
        return true;
    }
    return false;
  }

  public bool HasPerk(SkillPerk perk)
  {
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
        return true;
    }
    return false;
  }

  public void RemoveHat()
  {
    MinionResume.RemoveHat(this.GetComponent<KBatchedAnimController>());
  }

  public static void RemoveHat(KBatchedAnimController controller)
  {
    AccessorySlot hat = Db.Get().AccessorySlots.Hat;
    Accessorizer component = controller.GetComponent<Accessorizer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Accessory accessory = component.GetAccessory(hat);
      if (accessory != null)
        component.RemoveAccessory(accessory);
    }
    else
      controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) hat.targetSymbolId, 4);
    controller.SetSymbolVisiblity(hat.targetSymbolId, false);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
  }

  public static void AddHat(string hat_id, KBatchedAnimController controller)
  {
    AccessorySlot hat = Db.Get().AccessorySlots.Hat;
    Accessory accessory1 = hat.Lookup(hat_id);
    if (accessory1 == null)
      Debug.LogWarning((object) ("Missing hat: " + hat_id));
    Accessorizer component1 = controller.GetComponent<Accessorizer>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Accessory accessory2 = component1.GetAccessory(Db.Get().AccessorySlots.Hat);
      if (accessory2 != null)
        component1.RemoveAccessory(accessory2);
      if (accessory1 != null)
        component1.AddAccessory(accessory1);
    }
    else
    {
      SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
      component2.TryRemoveSymbolOverride((HashedString) hat.targetSymbolId, 4);
      component2.AddSymbolOverride((HashedString) hat.targetSymbolId, accessory1.symbol, 4);
    }
    controller.SetSymbolVisiblity(hat.targetSymbolId, true);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
  }

  public void ApplyTargetHat()
  {
    MinionResume.ApplyHat(this.targetHat, this.GetComponent<KBatchedAnimController>());
    this.currentHat = this.targetHat;
    this.targetHat = (string) null;
  }

  public static void ApplyHat(string hat_id, KBatchedAnimController controller)
  {
    if (hat_id.IsNullOrWhiteSpace())
      MinionResume.RemoveHat(controller);
    else
      MinionResume.AddHat(hat_id, controller);
  }

  public string GetSkillsSubtitle()
  {
    return "Total Skill Points: " + (object) this.TotalSkillPointsGained;
  }

  public static bool AnyMinionHasPerk(string perk)
  {
    foreach (MinionResume minionResume in Components.MinionResumes.Items)
    {
      if (minionResume.HasPerk((HashedString) perk))
        return true;
    }
    return false;
  }

  public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
  {
    foreach (MinionResume minionResume in Components.MinionResumes.Items)
    {
      if (!((UnityEngine.Object) minionResume == (UnityEngine.Object) me) && minionResume.HasPerk((HashedString) perk))
        return true;
    }
    return false;
  }

  public void ResetSkillLevels(bool returnSkillPoints = true)
  {
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
        stringList.Add(keyValuePair.Key);
    }
    foreach (string skillId in stringList)
      this.UnmasterSkill(skillId);
  }

  public enum SkillMasteryConditions
  {
    SkillAptitude,
    StressWarning,
    UnableToLearn,
    NeedsSkillPoints,
    MissingPreviousSkill,
  }
}
