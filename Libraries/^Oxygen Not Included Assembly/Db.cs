// Decompiled with JetBrains decompiler
// Type: Db
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Db : EntityModifierSet
{
  private static Db _Instance;
  public TextAsset researchTreeFile;
  public Diseases Diseases;
  public Database.Sicknesses Sicknesses;
  public Urges Urges;
  public AssignableSlots AssignableSlots;
  public StateMachineCategories StateMachineCategories;
  public Personalities Personalities;
  public Faces Faces;
  public Shirts Shirts;
  public Expressions Expressions;
  public Thoughts Thoughts;
  public BuildingStatusItems BuildingStatusItems;
  public MiscStatusItems MiscStatusItems;
  public CreatureStatusItems CreatureStatusItems;
  public StatusItemCategories StatusItemCategories;
  public Deaths Deaths;
  public Database.ChoreTypes ChoreTypes;
  public Techs Techs;
  public TechTreeTitles TechTreeTitles;
  public TechItems TechItems;
  public AccessorySlots AccessorySlots;
  public Accessories Accessories;
  public ScheduleBlockTypes ScheduleBlockTypes;
  public ScheduleGroups ScheduleGroups;
  public RoomTypeCategories RoomTypeCategories;
  public RoomTypes RoomTypes;
  public ArtifactDropRates ArtifactDropRates;
  public SpaceDestinationTypes SpaceDestinationTypes;
  public SkillPerks SkillPerks;
  public SkillGroups SkillGroups;
  public Skills Skills;
  public ColonyAchievements ColonyAchievements;

  public static Db Get()
  {
    if ((UnityEngine.Object) Db._Instance == (UnityEngine.Object) null)
    {
      Db._Instance = Resources.Load<Db>(nameof (Db));
      Db._Instance.Initialize();
    }
    return Db._Instance;
  }

  public override void Initialize()
  {
    base.Initialize();
    this.Urges = new Urges();
    this.AssignableSlots = new AssignableSlots();
    this.StateMachineCategories = new StateMachineCategories();
    this.Personalities = new Personalities();
    this.Faces = new Faces();
    this.Shirts = new Shirts();
    this.Expressions = new Expressions(this.Root);
    this.Thoughts = new Thoughts(this.Root);
    this.Deaths = new Deaths(this.Root);
    this.StatusItemCategories = new StatusItemCategories(this.Root);
    this.Techs = new Techs(this.Root);
    this.Techs.Load(this.researchTreeFile);
    this.TechTreeTitles = new TechTreeTitles(this.Root);
    this.TechTreeTitles.Load(this.researchTreeFile);
    this.TechItems = new TechItems(this.Root);
    this.Accessories = new Accessories(this.Root);
    this.AccessorySlots = new AccessorySlots(this.Root, (KAnimFile) null, (KAnimFile) null, (KAnimFile) null);
    this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
    this.ScheduleGroups = new ScheduleGroups(this.Root);
    this.RoomTypeCategories = new RoomTypeCategories(this.Root);
    this.RoomTypes = new RoomTypes(this.Root);
    this.ArtifactDropRates = new ArtifactDropRates(this.Root);
    this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
    this.Diseases = new Diseases(this.Root);
    this.Sicknesses = new Database.Sicknesses(this.Root);
    this.SkillPerks = new SkillPerks(this.Root);
    this.SkillGroups = new SkillGroups(this.Root);
    this.Skills = new Skills(this.Root);
    this.ColonyAchievements = new ColonyAchievements(this.Root);
    this.MiscStatusItems = new MiscStatusItems(this.Root);
    this.CreatureStatusItems = new CreatureStatusItems(this.Root);
    this.BuildingStatusItems = new BuildingStatusItems(this.Root);
    this.ChoreTypes = new Database.ChoreTypes(this.Root);
    Effect resource = new Effect("CenterOfAttention", (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0.0f, true, true, false, (string) null, 0.0f, (string) null);
    resource.Add(new AttributeModifier("StressDelta", -0.008333334f, (string) DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
    this.effects.Add(resource);
    this.CollectResources((Resource) this.Root, this.ResourceTable);
  }

  private void CollectResources(Resource resource, List<Resource> resource_table)
  {
    if (resource.Guid != (ResourceGuid) null)
      resource_table.Add(resource);
    ResourceSet resourceSet = resource as ResourceSet;
    if (resourceSet == null)
      return;
    for (int idx = 0; idx < resourceSet.Count; ++idx)
      this.CollectResources(resourceSet.GetResource(idx), resource_table);
  }

  public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
  {
    Resource resource = this.ResourceTable.FirstOrDefault<Resource>((Func<Resource, bool>) (s => s.Guid == guid));
    if (resource == null)
    {
      Debug.LogWarning((object) ("Could not find resource: " + (object) guid));
      return (ResourceType) null;
    }
    ResourceType resourceType = (ResourceType) resource;
    if ((object) resourceType != null)
      return resourceType;
    Debug.LogError((object) ("Resource type mismatch for resource: " + resource.Id + "\nExpecting Type: " + typeof (ResourceType).Name + "\nGot Type: " + resource.GetType().Name));
    return (ResourceType) null;
  }

  [Serializable]
  public class SlotInfo : Resource
  {
  }
}
