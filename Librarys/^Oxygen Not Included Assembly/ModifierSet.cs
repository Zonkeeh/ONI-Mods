// Decompiled with JetBrains decompiler
// Type: ModifierSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSet : ScriptableObject
{
  public TextAsset modifiersFile;
  public ModifierSet.ModifierInfos modifierInfos;
  public ModifierSet.TraitSet traits;
  public ResourceSet<Effect> effects;
  public ModifierSet.TraitGroupSet traitGroups;
  public FertilityModifiers FertilityModifiers;
  public Database.Attributes Attributes;
  public BuildingAttributes BuildingAttributes;
  public CritterAttributes CritterAttributes;
  public Database.Amounts Amounts;
  public Database.AttributeConverters AttributeConverters;
  public ResourceSet Root;
  public List<Resource> ResourceTable;

  public virtual void Initialize()
  {
    this.ResourceTable = new List<Resource>();
    this.Root = (ResourceSet) new ResourceSet<Resource>("Root", (ResourceSet) null);
    this.modifierInfos = new ModifierSet.ModifierInfos();
    this.modifierInfos.Load(this.modifiersFile);
    this.Attributes = new Database.Attributes(this.Root);
    this.BuildingAttributes = new BuildingAttributes(this.Root);
    this.CritterAttributes = new CritterAttributes(this.Root);
    this.effects = new ResourceSet<Effect>("Effects", this.Root);
    this.traits = new ModifierSet.TraitSet();
    this.traitGroups = new ModifierSet.TraitGroupSet();
    this.FertilityModifiers = new FertilityModifiers();
    this.Amounts = new Database.Amounts();
    this.Amounts.Load();
    this.AttributeConverters = new Database.AttributeConverters();
    this.LoadEffects();
    this.LoadFertilityModifiers();
  }

  public static float ConvertValue(float value, Units units)
  {
    if (units == Units.PerDay)
      return value * (1f / 600f);
    return value;
  }

  private void LoadEffects()
  {
    foreach (ModifierSet.ModifierInfo modifierInfo1 in (ResourceLoader<ModifierSet.ModifierInfo>) this.modifierInfos)
    {
      if (!this.effects.Exists(modifierInfo1.Id) && (modifierInfo1.Type == "Effect" || modifierInfo1.Type == "Base" || modifierInfo1.Type == "Need"))
      {
        string str = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.NAME", (object) modifierInfo1.Id.ToUpper()));
        string description = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.TOOLTIP", (object) modifierInfo1.Id.ToUpper()));
        Effect resource = new Effect(modifierInfo1.Id, str, description, modifierInfo1.Duration * 600f, modifierInfo1.ShowInUI && modifierInfo1.Type != "Need", modifierInfo1.TriggerFloatingText, modifierInfo1.IsBad, modifierInfo1.EmoteAnim, modifierInfo1.EmoteCooldown, modifierInfo1.StompGroup);
        foreach (ModifierSet.ModifierInfo modifierInfo2 in (ResourceLoader<ModifierSet.ModifierInfo>) this.modifierInfos)
        {
          if (modifierInfo2.Id == modifierInfo1.Id)
            resource.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), str, modifierInfo2.Multiplier, false, true));
        }
        this.effects.Add(resource);
      }
    }
    Effect resource1 = new Effect("Ranched", (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false, (string) null, 0.0f, (string) null);
    resource1.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
    resource1.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, (string) STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
    this.effects.Add(resource1);
    Effect resource2 = new Effect("EggSong", (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false, (string) null, 0.0f, (string) null);
    resource2.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, (string) STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true, false, true));
    this.effects.Add(resource2);
    Reactable.ReactablePrecondition precon = (Reactable.ReactablePrecondition) ((go, n) =>
    {
      int cell = Grid.PosToCell(go);
      if (Grid.IsValidCell(cell))
        return Grid.IsGas(cell);
      return false;
    });
    this.effects.Get("WetFeet").AddEmotePrecondition(precon);
    this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
  }

  public Trait CreateTrait(
    string id,
    string name,
    string description,
    string group_name,
    bool should_save,
    ChoreGroup[] disabled_chore_groups,
    bool positive_trait,
    bool is_valid_starter_trait)
  {
    Trait trait = new Trait(id, name, description, 0.0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait);
    this.traits.Add(trait);
    if (group_name == string.Empty || group_name == null)
      group_name = "Default";
    TraitGroup resource = this.traitGroups.TryGet(group_name);
    if (resource == null)
    {
      resource = new TraitGroup(group_name, group_name, group_name != "Default");
      this.traitGroups.Add(resource);
    }
    resource.Add(trait);
    return trait;
  }

  public FertilityModifier CreateFertilityModifier(
    string id,
    Tag targetTag,
    string name,
    string description,
    Func<string, string> tooltipCB,
    FertilityModifier.FertilityModFn applyFunction)
  {
    FertilityModifier resource = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
    this.FertilityModifiers.Add(resource);
    return resource;
  }

  protected void LoadTraits()
  {
    TUNING.TRAITS.TRAIT_CREATORS.ForEach((System.Action<System.Action>) (action => action()));
  }

  protected void LoadFertilityModifiers()
  {
    TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach((System.Action<System.Action>) (action => action()));
  }

  public class ModifierInfo : Resource
  {
    public string Type;
    public string Attribute;
    public float Value;
    public Units Units;
    public bool Multiplier;
    public float Duration;
    public bool ShowInUI;
    public float Tier;
    public string Notes;
    public string StompGroup;
    public bool IsBad;
    public bool TriggerFloatingText;
    public string EmoteAnim;
    public float EmoteCooldown;
  }

  [Serializable]
  public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
  {
  }

  [Serializable]
  public class TraitSet : ResourceSet<Trait>
  {
  }

  [Serializable]
  public class TraitGroupSet : ResourceSet<TraitGroup>
  {
  }
}
