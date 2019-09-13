// Decompiled with JetBrains decompiler
// Type: MinionStartingStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MinionStartingStats : ITelepadDeliverable
{
  public List<Trait> Traits = new List<Trait>();
  public Dictionary<string, int> StartingLevels = new Dictionary<string, int>();
  public List<Accessory> accessories = new List<Accessory>();
  public Dictionary<SkillGroup, float> skillAptitudes = new Dictionary<SkillGroup, float>();
  public string Name;
  public string NameStringKey;
  public string GenderStringKey;
  public Trait stressTrait;
  public Trait congenitaltrait;
  public int voiceIdx;
  public Personality personality;

  public MinionStartingStats(bool is_starter_minion, string guaranteedAptitudeID = null)
  {
    this.personality = !is_starter_minion ? Db.Get().Personalities[UnityEngine.Random.Range(0, 35)] : Db.Get().Personalities[UnityEngine.Random.Range(0, 29)];
    this.voiceIdx = UnityEngine.Random.Range(0, 4);
    this.Name = this.personality.Name;
    this.NameStringKey = this.personality.nameStringKey;
    this.GenderStringKey = this.personality.genderStringKey;
    this.Traits.Add(Db.Get().traits.Get(MinionConfig.MINION_BASE_TRAIT_ID));
    List<ChoreGroup> disabled_chore_groups = new List<ChoreGroup>();
    this.GenerateAptitudes(guaranteedAptitudeID);
    this.GenerateAttributes(this.GenerateTraits(is_starter_minion, disabled_chore_groups), disabled_chore_groups);
    KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(this.personality);
    foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
    {
      if (resource.accessories.Count != 0)
      {
        Accessory accessory = (Accessory) null;
        if (resource == Db.Get().AccessorySlots.HeadShape)
        {
          accessory = resource.Lookup(bodyData.headShape);
          if (accessory == null)
            this.personality.headShape = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Mouth)
        {
          accessory = resource.Lookup(bodyData.mouth);
          if (accessory == null)
            this.personality.mouth = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Eyes)
        {
          accessory = resource.Lookup(bodyData.eyes);
          if (accessory == null)
            this.personality.eyes = 0;
        }
        else if (resource == Db.Get().AccessorySlots.Hair)
        {
          accessory = resource.Lookup(bodyData.hair);
          if (accessory == null)
            this.personality.hair = 0;
        }
        else if (resource != Db.Get().AccessorySlots.HatHair)
        {
          if (resource == Db.Get().AccessorySlots.Body)
          {
            accessory = resource.Lookup(bodyData.body);
            if (accessory == null)
              this.personality.body = 0;
          }
          else if (resource == Db.Get().AccessorySlots.Arm)
            accessory = resource.Lookup(bodyData.arms);
        }
        if (accessory == null)
          accessory = resource.accessories[0];
        this.accessories.Add(accessory);
      }
    }
  }

  private int GenerateTraits(bool is_starter_minion, List<ChoreGroup> disabled_chore_groups)
  {
    int statDelta = 0;
    List<string> selectedTraits = new List<string>();
    System.Random randSeed = new System.Random();
    this.stressTrait = Db.Get().traits.Get(this.personality.stresstrait);
    Trait trait1 = Db.Get().traits.Get(this.personality.congenitaltrait);
    this.congenitaltrait = !(trait1.Name == "None") ? trait1 : (Trait) null;
    Func<List<DUPLICANTSTATS.TraitVal>, bool> func = (Func<List<DUPLICANTSTATS.TraitVal>, bool>) (traitPossibilities =>
    {
      if (this.Traits.Count > DUPLICANTSTATS.MAX_TRAITS)
        return false;
      float num = Util.GaussianRandom(0.0f, 1f);
      List<DUPLICANTSTATS.TraitVal> list = new List<DUPLICANTSTATS.TraitVal>((IEnumerable<DUPLICANTSTATS.TraitVal>) traitPossibilities);
      list.ShuffleSeeded<DUPLICANTSTATS.TraitVal>(randSeed);
      list.Sort((Comparison<DUPLICANTSTATS.TraitVal>) ((t1, t2) => -t1.probability.CompareTo(t2.probability)));
      foreach (DUPLICANTSTATS.TraitVal traitVal in list)
      {
        if (!selectedTraits.Contains(traitVal.id))
        {
          if (traitVal.requiredNonPositiveAptitudes != null)
          {
            bool flag = false;
            foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
            {
              if (!flag)
              {
                foreach (HashedString positiveAptitude in traitVal.requiredNonPositiveAptitudes)
                {
                  if (positiveAptitude == skillAptitude.Key.IdHash && (double) skillAptitude.Value > 0.0)
                  {
                    flag = true;
                    break;
                  }
                }
              }
              else
                break;
            }
            if (flag)
              continue;
          }
          if (traitVal.mutuallyExclusiveTraits != null)
          {
            bool flag = false;
            foreach (string str in selectedTraits)
            {
              flag = traitVal.mutuallyExclusiveTraits.Contains(str);
              if (flag)
                break;
            }
            if (flag)
              continue;
          }
          if ((double) num > (double) traitVal.probability)
          {
            Trait trait2 = Db.Get().traits.TryGet(traitVal.id);
            if (trait2 == null)
              Debug.LogWarning((object) ("Trying to add nonexistent trait: " + traitVal.id));
            else if (!is_starter_minion || trait2.ValidStarterTrait)
            {
              selectedTraits.Add(traitVal.id);
              statDelta += traitVal.statBonus;
              this.Traits.Add(trait2);
              if (trait2.disabledChoreGroups != null)
              {
                for (int index = 0; index < trait2.disabledChoreGroups.Length; ++index)
                  disabled_chore_groups.Add(trait2.disabledChoreGroups[index]);
              }
              return true;
            }
          }
        }
      }
      return false;
    });
    int num1 = !is_starter_minion ? 3 : 1;
    bool flag1 = false;
    while (!flag1)
    {
      for (int index = 0; index < num1; ++index)
        flag1 = func(DUPLICANTSTATS.BADTRAITS) || flag1;
    }
    bool flag2 = false;
    while (!flag2)
    {
      for (int index = 0; index < num1; ++index)
        flag2 = func(DUPLICANTSTATS.GOODTRAITS) || flag2;
    }
    return statDelta;
  }

  private void GenerateAptitudes(string guaranteedAptitudeID = null)
  {
    int num = UnityEngine.Random.Range(1, 4);
    List<SkillGroup> list = new List<SkillGroup>((IEnumerable<SkillGroup>) Db.Get().SkillGroups.resources);
    list.Shuffle<SkillGroup>();
    if (guaranteedAptitudeID != null)
    {
      this.skillAptitudes.Add(Db.Get().SkillGroups.Get(guaranteedAptitudeID), (float) DUPLICANTSTATS.APTITUDE_BONUS);
      list.Remove(Db.Get().SkillGroups.Get(guaranteedAptitudeID));
      --num;
    }
    for (int index = 0; index < num; ++index)
      this.skillAptitudes.Add(list[index], (float) DUPLICANTSTATS.APTITUDE_BONUS);
  }

  private void GenerateAttributes(int pointsDelta, List<ChoreGroup> disabled_chore_groups)
  {
    int a1 = Mathf.RoundToInt((float) ((double) Util.GaussianRandom(0.0f, 1f) * ((double) DUPLICANTSTATS.MAX_STAT_POINTS - (double) DUPLICANTSTATS.MIN_STAT_POINTS) / 2.0) + (float) DUPLICANTSTATS.MIN_STAT_POINTS);
    List<string> list = new List<string>((IEnumerable<string>) DUPLICANTSTATS.ALL_ATTRIBUTES);
    int[] randomDistribution = DUPLICANTSTATS.DISTRIBUTIONS.GetRandomDistribution();
    for (int index = 0; index < list.Count; ++index)
    {
      if (!this.StartingLevels.ContainsKey(list[index]))
        this.StartingLevels[list[index]] = 0;
    }
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
    {
      if (skillAptitude.Key.relevantAttributes.Count > 0)
      {
        for (int index = 0; index < skillAptitude.Key.relevantAttributes.Count; ++index)
        {
          Dictionary<string, int> startingLevels;
          string id;
          (startingLevels = this.StartingLevels)[id = skillAptitude.Key.relevantAttributes[index].Id] = startingLevels[id] + DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.skillAptitudes.Count - 1];
        }
      }
    }
    list.Shuffle<string>();
    for (int a2 = 0; a2 < list.Count; ++a2)
    {
      string key = list[a2];
      int b = randomDistribution[Mathf.Min(a2, randomDistribution.Length - 1)];
      int num = Mathf.Min(a1, b);
      if (!this.StartingLevels.ContainsKey(key))
        this.StartingLevels[key] = 0;
      Dictionary<string, int> startingLevels;
      string index;
      (startingLevels = this.StartingLevels)[index = key] = startingLevels[index] + num;
      a1 -= num;
    }
    if (disabled_chore_groups.Count <= 0)
      return;
    int num1 = 0;
    int num2 = 0;
    foreach (KeyValuePair<string, int> startingLevel in this.StartingLevels)
    {
      if (startingLevel.Value > num1)
        num1 = startingLevel.Value;
      if (startingLevel.Key == disabled_chore_groups[0].attribute.Id)
        num2 = startingLevel.Value;
    }
    if (num1 != num2)
      return;
    foreach (string key in list)
    {
      if (key != disabled_chore_groups[0].attribute.Id)
      {
        int num3 = 0;
        this.StartingLevels.TryGetValue(key, out num3);
        int num4 = 0;
        if (num3 > 0)
          num4 = 1;
        this.StartingLevels[disabled_chore_groups[0].attribute.Id] = num3 - num4;
        this.StartingLevels[key] = num1 + num4;
        break;
      }
    }
  }

  public void Apply(GameObject go)
  {
    MinionIdentity component = go.GetComponent<MinionIdentity>();
    component.SetName(this.Name);
    component.nameStringKey = this.NameStringKey;
    component.genderStringKey = this.GenderStringKey;
    this.ApplyTraits(go);
    this.ApplyRace(go);
    this.ApplyAptitudes(go);
    this.ApplyAccessories(go);
    this.ApplyExperience(go);
  }

  public void ApplyExperience(GameObject go)
  {
    foreach (KeyValuePair<string, int> startingLevel in this.StartingLevels)
      go.GetComponent<AttributeLevels>().SetLevel(startingLevel.Key, startingLevel.Value);
  }

  public void ApplyAccessories(GameObject go)
  {
    Accessorizer component = go.GetComponent<Accessorizer>();
    foreach (Accessory accessory in this.accessories)
      component.AddAccessory(accessory);
  }

  public void ApplyRace(GameObject go)
  {
    go.GetComponent<MinionIdentity>().voiceIdx = this.voiceIdx;
  }

  public static KCompBuilder.BodyData CreateBodyData(Personality p)
  {
    return new KCompBuilder.BodyData()
    {
      eyes = HashCache.Get().Add(string.Format("eyes_{0:000}", (object) p.eyes)),
      hair = HashCache.Get().Add(string.Format("hair_{0:000}", (object) p.hair)),
      headShape = HashCache.Get().Add(string.Format("headshape_{0:000}", (object) p.headShape)),
      mouth = HashCache.Get().Add(string.Format("mouth_{0:000}", (object) p.mouth)),
      neck = HashCache.Get().Add(string.Format("neck_{0:000}", (object) p.neck)),
      arms = HashCache.Get().Add(string.Format("arm_{0:000}", (object) p.body)),
      body = HashCache.Get().Add(string.Format("body_{0:000}", (object) p.body)),
      hat = HashedString.Invalid
    };
  }

  public void ApplyAptitudes(GameObject go)
  {
    MinionResume component = go.GetComponent<MinionResume>();
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.skillAptitudes)
      component.SetAptitude((HashedString) skillAptitude.Key.Id, skillAptitude.Value);
  }

  public void ApplyTraits(GameObject go)
  {
    Klei.AI.Traits component = go.GetComponent<Klei.AI.Traits>();
    component.Clear();
    foreach (Trait trait in this.Traits)
      component.Add(trait);
    component.Add(this.stressTrait);
    if (this.congenitaltrait != null)
      component.Add(this.congenitaltrait);
    go.GetComponent<MinionIdentity>().SetName(this.Name);
    go.GetComponent<MinionIdentity>().SetGender(this.GenderStringKey);
  }

  public GameObject Deliver(Vector3 location)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) MinionConfig.ID), (GameObject) null, (string) null);
    gameObject.SetActive(true);
    gameObject.transform.SetLocalPosition(location);
    this.Apply(gameObject);
    Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
    EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, (Func<StatusItem>) null);
    return gameObject;
  }
}
