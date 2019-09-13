// Decompiled with JetBrains decompiler
// Type: EntityTemplates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class EntityTemplates
{
  private static GameObject selectableEntityTemplate;
  private static GameObject unselectableEntityTemplate;
  private static GameObject baseEntityTemplate;
  private static GameObject placedEntityTemplate;
  private static GameObject baseOreTemplate;

  public static void CreateTemplates()
  {
    EntityTemplates.unselectableEntityTemplate = new GameObject("unselectableEntityTemplate");
    EntityTemplates.unselectableEntityTemplate.SetActive(false);
    EntityTemplates.unselectableEntityTemplate.AddComponent<KPrefabID>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.unselectableEntityTemplate);
    EntityTemplates.selectableEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate);
    EntityTemplates.selectableEntityTemplate.name = "selectableEntityTemplate";
    EntityTemplates.selectableEntityTemplate.AddComponent<KSelectable>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.selectableEntityTemplate);
    EntityTemplates.baseEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
    EntityTemplates.baseEntityTemplate.name = "baseEntityTemplate";
    EntityTemplates.baseEntityTemplate.AddComponent<KBatchedAnimController>();
    EntityTemplates.baseEntityTemplate.AddComponent<SaveLoadRoot>();
    EntityTemplates.baseEntityTemplate.AddComponent<StateMachineController>();
    EntityTemplates.baseEntityTemplate.AddComponent<PrimaryElement>();
    EntityTemplates.baseEntityTemplate.AddComponent<SimTemperatureTransfer>();
    EntityTemplates.baseEntityTemplate.AddComponent<InfoDescription>();
    EntityTemplates.baseEntityTemplate.AddComponent<Notifier>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.baseEntityTemplate);
    EntityTemplates.placedEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
    EntityTemplates.placedEntityTemplate.name = "placedEntityTemplate";
    EntityTemplates.placedEntityTemplate.AddComponent<KBoxCollider2D>();
    EntityTemplates.placedEntityTemplate.AddComponent<OccupyArea>();
    EntityTemplates.placedEntityTemplate.AddComponent<Modifiers>();
    EntityTemplates.placedEntityTemplate.AddComponent<DecorProvider>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.placedEntityTemplate);
  }

  private static void ConfigEntity(
    GameObject template,
    string id,
    string name,
    bool is_selectable = true)
  {
    template.name = id;
    template.AddOrGet<KPrefabID>().PrefabTag = TagManager.Create(id, name);
    if (!is_selectable)
      return;
    template.AddOrGet<KSelectable>().SetName(name);
  }

  public static GameObject CreateEntity(string id, string name, bool is_selectable = true)
  {
    GameObject template = !is_selectable ? UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate) : UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) template);
    EntityTemplates.ConfigEntity(template, id, name, is_selectable);
    return template;
  }

  public static GameObject ConfigBasicEntity(
    GameObject template,
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    EntityTemplates.ConfigEntity(template, id, name, true);
    KPrefabID kprefabId = template.AddOrGet<KPrefabID>();
    if (additionalTags != null)
    {
      foreach (Tag additionalTag in additionalTags)
        kprefabId.AddTag(additionalTag, false);
    }
    KBatchedAnimController kbatchedAnimController = template.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      anim
    };
    kbatchedAnimController.sceneLayer = sceneLayer;
    kbatchedAnimController.initialAnim = initialAnim;
    template.AddOrGet<StateMachineController>();
    PrimaryElement primaryElement = template.AddOrGet<PrimaryElement>();
    primaryElement.ElementID = element;
    primaryElement.Temperature = defaultTemperature;
    if (unitMass)
    {
      primaryElement.MassPerUnit = mass;
      primaryElement.Units = 1f;
      GameTags.DisplayAsUnits.Add(kprefabId.PrefabTag);
    }
    else
      primaryElement.Mass = mass;
    template.AddOrGet<InfoDescription>().description = desc;
    template.AddOrGet<Notifier>();
    return template;
  }

  public static GameObject CreateBasicEntity(
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    GameObject template = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) template);
    EntityTemplates.ConfigBasicEntity(template, id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
    return template;
  }

  private static GameObject ConfigPlacedEntity(
    GameObject template,
    string id,
    string name,
    string desc,
    float mass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    int width,
    int height,
    EffectorValues decor,
    EffectorValues noise = default (EffectorValues),
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    if ((UnityEngine.Object) anim == (UnityEngine.Object) null)
      Debug.LogErrorFormat("Cant create [{0}] entity without an anim", (object) name);
    EntityTemplates.ConfigBasicEntity(template, id, name, desc, mass, true, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
    KBoxCollider2D kboxCollider2D = template.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.size = (Vector2) new Vector2f(width, height);
    float num = 0.5f * (float) ((width + 1) % 2);
    kboxCollider2D.offset = (Vector2) new Vector2f(num, (float) height / 2f);
    template.GetComponent<KBatchedAnimController>().Offset = new Vector3(num, 0.0f, 0.0f);
    template.AddOrGet<OccupyArea>().OccupiedCellsOffsets = EntityTemplates.GenerateOffsets(width, height);
    DecorProvider decorProvider = template.AddOrGet<DecorProvider>();
    decorProvider.SetValues(decor);
    decorProvider.overrideName = name;
    return template;
  }

  public static GameObject CreatePlacedEntity(
    string id,
    string name,
    string desc,
    float mass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    int width,
    int height,
    EffectorValues decor,
    EffectorValues noise = default (EffectorValues),
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    GameObject template = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.placedEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) template);
    EntityTemplates.ConfigPlacedEntity(template, id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, noise, element, additionalTags, defaultTemperature);
    return template;
  }

  public static GameObject MakeHangingOffsets(GameObject template, int width, int height)
  {
    KBoxCollider2D component1 = template.GetComponent<KBoxCollider2D>();
    if ((bool) ((UnityEngine.Object) component1))
    {
      component1.size = (Vector2) new Vector2f(width, height);
      float a = 0.5f * (float) ((width + 1) % 2);
      component1.offset = (Vector2) new Vector2f(a, (float) ((double) -height / 2.0 + 1.0));
    }
    OccupyArea component2 = template.GetComponent<OccupyArea>();
    if ((bool) ((UnityEngine.Object) component2))
      component2.OccupiedCellsOffsets = EntityTemplates.GenerateHangingOffsets(width, height);
    return template;
  }

  public static GameObject ExtendBuildingToRocketModule(GameObject template)
  {
    template.GetComponent<KBatchedAnimController>().isMovable = true;
    template.GetComponent<Building>().Def.ThermalConductivity = 0.1f;
    return template;
  }

  public static GameObject ExtendEntityToBasicPlant(
    GameObject template,
    float temperature_lethal_low = 218.15f,
    float temperature_warning_low = 283.15f,
    float temperature_warning_high = 303.15f,
    float temperature_lethal_high = 398.15f,
    SimHashes[] safe_elements = null,
    bool pressure_sensitive = true,
    float pressure_lethal_low = 0.0f,
    float pressure_warning_low = 0.15f,
    string crop_id = null,
    bool can_drown = true,
    bool can_tinker = true,
    bool require_solid_tile = true,
    bool should_grow_old = true,
    float max_age = 2400f)
  {
    template.AddOrGet<EntombVulnerable>();
    PressureVulnerable pressureVulnerable1 = template.AddOrGet<PressureVulnerable>();
    if (pressure_sensitive)
    {
      PressureVulnerable pressureVulnerable2 = pressureVulnerable1;
      float num1 = pressure_warning_low;
      float num2 = pressure_lethal_low;
      SimHashes[] simHashesArray = safe_elements;
      double num3 = (double) num1;
      double num4 = (double) num2;
      SimHashes[] safeAtmospheres = simHashesArray;
      pressureVulnerable2.Configure((float) num3, (float) num4, 10f, 30f, safeAtmospheres);
    }
    else
      pressureVulnerable1.Configure(safe_elements);
    template.AddOrGet<WiltCondition>();
    template.AddOrGet<Prioritizable>();
    template.AddOrGet<Uprootable>();
    if (require_solid_tile)
      template.AddOrGet<UprootedMonitor>();
    template.AddOrGet<ReceptacleMonitor>();
    template.AddOrGet<Notifier>();
    if (can_drown)
      template.AddOrGet<DrowningMonitor>();
    template.AddOrGet<TemperatureVulnerable>().Configure(temperature_warning_low, temperature_lethal_low, temperature_warning_high, temperature_lethal_high);
    template.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    KPrefabID component1 = template.GetComponent<KPrefabID>();
    if (crop_id != null)
    {
      GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component1.PrefabID().ToString());
      Crop.CropVal cropval = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == crop_id));
      template.AddOrGet<Crop>().Configure(cropval);
      Growing growing = template.AddOrGet<Growing>();
      growing.growthTime = cropval.cropDuration;
      growing.shouldGrowOld = should_grow_old;
      growing.maxAge = max_age;
      template.AddOrGet<Harvestable>();
      template.AddOrGet<HarvestDesignatable>();
    }
    component1.prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      PressureVulnerable component2 = inst.GetComponent<PressureVulnerable>();
      if (safe_elements == null)
        return;
      foreach (SimHashes safeElement in safe_elements)
        component2.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeElement));
    });
    if (can_tinker)
      Tinkerable.MakeFarmTinkerable(template);
    return template;
  }

  public static GameObject ExtendEntityToWildCreature(
    GameObject prefab,
    int space_required_per_creature,
    float lifespan)
  {
    prefab.AddOrGetDef<AgeMonitor.Def>();
    prefab.AddOrGetDef<HappinessMonitor.Def>();
    Tag prefabTag = prefab.GetComponent<KPrefabID>().PrefabTag;
    WildnessMonitor.Def def = prefab.AddOrGetDef<WildnessMonitor.Def>();
    def.wildEffect = new Effect("Wild" + prefabTag.Name, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, (string) STRINGS.CREATURES.MODIFIERS.WILD.TOOLTIP, 0.0f, true, true, false, (string) null, 0.0f, (string) null);
    def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, 0.008333334f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, false, false, true));
    def.wildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, 25f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, false, false, true));
    def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -0.75f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, true, false, true));
    def.tameEffect = new Effect("Tame" + prefabTag.Name, (string) STRINGS.CREATURES.MODIFIERS.TAME.NAME, (string) STRINGS.CREATURES.MODIFIERS.TAME.TOOLTIP, 0.0f, true, true, false, (string) null, 0.0f, (string) null);
    def.tameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, (string) STRINGS.CREATURES.MODIFIERS.TAME.NAME, false, false, true));
    def.tameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, 100f, (string) STRINGS.CREATURES.MODIFIERS.TAME.NAME, false, false, true));
    prefab.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = space_required_per_creature;
    prefab.AddTag(GameTags.Plant);
    return prefab;
  }

  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    string eggId,
    string eggName,
    string eggDesc,
    string egg_anim,
    float egg_mass,
    string baby_id,
    float fertility_cycles,
    float incubation_cycles,
    List<FertilityMonitor.BreedingChance> egg_chances,
    int eggSortOrder = -1,
    bool is_ranchable = true,
    bool add_fish_overcrowding_monitor = false,
    bool add_fixed_capturable_monitor = true,
    float egg_anim_scale = 1f)
  {
    FertilityMonitor.Def def = prefab.AddOrGetDef<FertilityMonitor.Def>();
    def.baseFertileCycles = fertility_cycles;
    DebugUtil.DevAssert(eggSortOrder > -1, "Added a fertile creature without an egg sort order!");
    float base_incubation_rate = (float) (100.0 / (600.0 * (double) incubation_cycles));
    GameObject egg = EggConfig.CreateEgg(eggId, eggName, eggDesc, (Tag) baby_id, egg_anim, egg_mass, eggSortOrder, base_incubation_rate);
    def.eggPrefab = new Tag(eggId);
    def.initialBreedingWeights = egg_chances;
    if ((double) egg_anim_scale != 1.0)
    {
      KBatchedAnimController component = egg.GetComponent<KBatchedAnimController>();
      component.animWidth = egg_anim_scale;
      component.animHeight = egg_anim_scale;
    }
    KPrefabID egg_prefab_id = egg.GetComponent<KPrefabID>();
    SymbolOverrideController prefab1 = SymbolOverrideControllerUtil.AddToPrefab(egg);
    string symbolPrefix = prefab.GetComponent<CreatureBrain>().symbolPrefix;
    if (!string.IsNullOrEmpty(symbolPrefix))
      prefab1.ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) egg_anim), symbolPrefix, (string) null, 0);
    KPrefabID creature_prefab_id = prefab.GetComponent<KPrefabID>();
    creature_prefab_id.prefabSpawnFn += (KPrefabID.PrefabFn) (inst =>
    {
      WorldInventory.Instance.Discover(eggId.ToTag(), WorldInventory.GetCategoryForTags(egg_prefab_id.Tags));
      WorldInventory.Instance.Discover(baby_id.ToTag(), WorldInventory.GetCategoryForTags(creature_prefab_id.Tags));
    });
    if (is_ranchable)
      prefab.AddOrGetDef<RanchableMonitor.Def>();
    if (add_fixed_capturable_monitor)
      prefab.AddOrGetDef<FixedCapturableMonitor.Def>();
    if (add_fish_overcrowding_monitor)
      egg.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    return prefab;
  }

  public static GameObject ExtendEntityToBeingABaby(
    GameObject prefab,
    Tag adult_prefab_id,
    string on_grow_item_drop_id = null)
  {
    prefab.AddOrGetDef<BabyMonitor.Def>().adultPrefab = adult_prefab_id;
    prefab.AddOrGetDef<BabyMonitor.Def>().onGrowDropID = on_grow_item_drop_id;
    prefab.AddOrGetDef<IncubatorMonitor.Def>();
    prefab.AddOrGetDef<CreatureSleepMonitor.Def>();
    prefab.AddOrGetDef<CallAdultMonitor.Def>();
    prefab.AddOrGetDef<AgeMonitor.Def>().maxAgePercentOnSpawn = 0.01f;
    return prefab;
  }

  public static GameObject ExtendEntityToBasicCreature(
    GameObject template,
    FactionManager.FactionID faction = FactionManager.FactionID.Prey,
    string initialTraitID = null,
    string NavGridName = "WalkerNavGrid1x1",
    NavType navType = NavType.Floor,
    int max_probing_radius = 32,
    float moveSpeed = 2f,
    string onDeathDropID = "Meat",
    int onDeathDropCount = 1,
    bool drownVulnerable = true,
    bool entombVulnerable = true,
    float warningLowTemperature = 283.15f,
    float warningHighTemperature = 293.15f,
    float lethalLowTemperature = 243.15f,
    float lethalHighTemperature = 343.15f)
  {
    template.GetComponent<KBatchedAnimController>().isMovable = true;
    template.AddOrGet<KPrefabID>().AddTag(GameTags.Creature, false);
    Modifiers modifiers = template.AddOrGet<Modifiers>();
    if (initialTraitID != null)
      modifiers.initialTraits = new string[1]
      {
        initialTraitID
      };
    modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
    template.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false);
    template.AddOrGet<Pickupable>();
    template.AddOrGet<Clearable>().isClearable = false;
    template.AddOrGet<Traits>();
    template.AddOrGet<Health>();
    template.AddOrGet<CharacterOverlay>();
    template.AddOrGet<RangedAttackable>();
    template.AddOrGet<FactionAlignment>().Alignment = faction;
    template.AddOrGet<Prioritizable>();
    template.AddOrGet<Effects>();
    template.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
    template.AddOrGetDef<DeathMonitor.Def>();
    template.AddOrGetDef<AnimInterruptMonitor.Def>();
    SymbolOverrideControllerUtil.AddToPrefab(template);
    template.AddOrGet<TemperatureVulnerable>().Configure(warningLowTemperature, lethalLowTemperature, warningHighTemperature, lethalHighTemperature);
    if (drownVulnerable)
      template.AddOrGet<DrowningMonitor>();
    if (entombVulnerable)
      template.AddOrGet<EntombVulnerable>();
    if (onDeathDropCount > 0 && onDeathDropID != string.Empty)
    {
      string[] drops = new string[onDeathDropCount];
      for (int index = 0; index < drops.Length; ++index)
        drops[index] = onDeathDropID;
      template.AddOrGet<Butcherable>().SetDrops(drops);
    }
    Navigator navigator = template.AddOrGet<Navigator>();
    navigator.NavGridName = NavGridName;
    navigator.CurrentNavType = navType;
    navigator.defaultSpeed = moveSpeed;
    navigator.updateProber = true;
    navigator.maxProbingRadius = max_probing_radius;
    navigator.sceneLayer = Grid.SceneLayer.Creatures;
    return template;
  }

  public static void AddCreatureBrain(
    GameObject prefab,
    ChoreTable.Builder chore_table,
    Tag species,
    string symbol_prefix)
  {
    CreatureBrain creatureBrain = prefab.AddOrGet<CreatureBrain>();
    creatureBrain.species = species;
    creatureBrain.symbolPrefix = symbol_prefix;
    ChoreConsumer chore_consumer = prefab.AddOrGet<ChoreConsumer>();
    chore_consumer.choreTable = chore_table.CreateTable();
    KPrefabID kprefabId = prefab.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.CreatureBrain, false);
    kprefabId.instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<ChoreConsumer>().choreTable = chore_consumer.choreTable);
  }

  public static Tag GetBaggedCreatureTag(Tag tag)
  {
    return TagManager.Create("Bagged" + tag.Name);
  }

  public static Tag GetUnbaggedCreatureTag(Tag bagged_tag)
  {
    return TagManager.Create(bagged_tag.Name.Substring(6));
  }

  public static string GetBaggedCreatureID(string name)
  {
    return "Bagged" + name;
  }

  public static GameObject CreateAndRegisterBaggedCreature(
    GameObject creature,
    bool must_stand_on_top_for_pickup,
    bool allow_mark_for_capture,
    bool use_gun_for_pickup = false)
  {
    KPrefabID creature_prefab_id = creature.GetComponent<KPrefabID>();
    creature_prefab_id.AddTag(GameTags.BagableCreature, false);
    Baggable baggable = creature.AddOrGet<Baggable>();
    baggable.mustStandOntopOfTrapForPickup = must_stand_on_top_for_pickup;
    baggable.useGunForPickup = use_gun_for_pickup;
    creature.AddOrGet<Capturable>().allowCapture = allow_mark_for_capture;
    creature_prefab_id.prefabSpawnFn += (KPrefabID.PrefabFn) (inst => WorldInventory.Instance.Discover(creature_prefab_id.PrefabTag, WorldInventory.GetCategoryForTags(creature_prefab_id.Tags)));
    return creature;
  }

  public static GameObject CreateLooseEntity(
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    EntityTemplates.CollisionShape collisionShape,
    float width = 1f,
    float height = 1f,
    bool isPickupable = false,
    int sortOrder = 0,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null)
  {
    GameObject go = EntityTemplates.AddCollision(EntityTemplates.CreateBasicEntity(id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags, 293f), collisionShape, width, height);
    go.GetComponent<KBatchedAnimController>().isMovable = true;
    go.AddOrGet<Modifiers>();
    if (isPickupable)
    {
      Pickupable pickupable = go.AddOrGet<Pickupable>();
      pickupable.SetWorkTime(5f);
      pickupable.sortOrder = sortOrder;
    }
    return go;
  }

  public static void CreateBaseOreTemplates()
  {
    EntityTemplates.baseOreTemplate = new GameObject("OreTemplate");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.baseOreTemplate);
    EntityTemplates.baseOreTemplate.SetActive(false);
    EntityTemplates.baseOreTemplate.AddComponent<KPrefabID>();
    EntityTemplates.baseOreTemplate.AddComponent<PrimaryElement>();
    EntityTemplates.baseOreTemplate.AddComponent<Pickupable>();
    EntityTemplates.baseOreTemplate.AddComponent<KSelectable>();
    EntityTemplates.baseOreTemplate.AddComponent<SaveLoadRoot>();
    EntityTemplates.baseOreTemplate.AddComponent<StateMachineController>();
    EntityTemplates.baseOreTemplate.AddComponent<Clearable>();
    EntityTemplates.baseOreTemplate.AddComponent<Prioritizable>();
    EntityTemplates.baseOreTemplate.AddComponent<KBatchedAnimController>();
    EntityTemplates.baseOreTemplate.AddComponent<SimTemperatureTransfer>();
    EntityTemplates.baseOreTemplate.AddComponent<Modifiers>();
    EntityTemplates.baseOreTemplate.AddOrGet<OccupyArea>().OccupiedCellsOffsets = new CellOffset[1]
    {
      new CellOffset()
    };
    DecorProvider decorProvider = EntityTemplates.baseOreTemplate.AddOrGet<DecorProvider>();
    decorProvider.baseDecor = -10f;
    decorProvider.baseRadius = 1f;
    EntityTemplates.baseOreTemplate.AddOrGet<ElementChunk>();
  }

  public static void DestroyBaseOreTemplates()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) EntityTemplates.baseOreTemplate);
    EntityTemplates.baseOreTemplate = (GameObject) null;
  }

  public static GameObject CreateOreEntity(
    SimHashes elementID,
    EntityTemplates.CollisionShape shape,
    float width,
    float height,
    List<Tag> additionalTags = null,
    float default_temperature = 293f)
  {
    Element elementByHash = ElementLoader.FindElementByHash(elementID);
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseOreTemplate);
    gameObject.name = elementByHash.name;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
    KPrefabID kprefabId = gameObject.AddOrGet<KPrefabID>();
    kprefabId.PrefabTag = elementByHash.tag;
    if (additionalTags != null)
    {
      foreach (Tag additionalTag in additionalTags)
        kprefabId.AddTag(additionalTag, false);
    }
    if ((double) elementByHash.lowTemp < 296.149993896484 && (double) elementByHash.highTemp > 296.149993896484)
      kprefabId.AddTag(GameTags.PedestalDisplayable, false);
    PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(elementID);
    primaryElement.Mass = 1f;
    primaryElement.Temperature = default_temperature;
    Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
    pickupable.SetWorkTime(5f);
    pickupable.sortOrder = elementByHash.buildMenuSort;
    gameObject.AddOrGet<KSelectable>().SetName(elementByHash.name);
    KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      elementByHash.substance.anim
    };
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.Front;
    kbatchedAnimController.initialAnim = "idle1";
    kbatchedAnimController.isMovable = true;
    return EntityTemplates.AddCollision(gameObject, shape, width, height);
  }

  public static GameObject CreateSolidOreEntity(
    SimHashes elementId,
    List<Tag> additionalTags = null)
  {
    return EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, additionalTags, 293f);
  }

  public static GameObject CreateLiquidOreEntity(
    SimHashes elementId,
    List<Tag> additionalTags = null)
  {
    GameObject oreEntity = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags, 293f);
    oreEntity.AddOrGet<Dumpable>().SetWorkTime(5f);
    oreEntity.AddOrGet<SubstanceChunk>();
    return oreEntity;
  }

  public static GameObject CreateGasOreEntity(
    SimHashes elementId,
    List<Tag> additionalTags = null)
  {
    GameObject oreEntity = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags, 293f);
    oreEntity.AddOrGet<Dumpable>().SetWorkTime(5f);
    oreEntity.AddOrGet<SubstanceChunk>();
    return oreEntity;
  }

  public static GameObject ExtendEntityToFood(
    GameObject template,
    EdiblesManager.FoodInfo foodInfo)
  {
    template.AddOrGet<EntitySplitter>();
    if (foodInfo.CanRot)
    {
      Rottable.Def def = template.AddOrGetDef<Rottable.Def>();
      def.rotTemperature = foodInfo.RotTemperature;
      def.spoilTime = foodInfo.SpoilTime;
      def.staleTime = foodInfo.StaleTime;
      EntityTemplates.CreateAndRegisterCompostableFromPrefab(template);
    }
    KPrefabID component = template.GetComponent<KPrefabID>();
    component.AddTag(GameTags.PedestalDisplayable, false);
    if ((double) foodInfo.CaloriesPerUnit > 0.0)
    {
      template.AddOrGet<Edible>().FoodInfo = foodInfo;
      component.instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<Edible>().FoodInfo = foodInfo);
      GameTags.DisplayAsCalories.Add(component.PrefabTag);
    }
    else
    {
      component.AddTag(GameTags.CookingIngredient, false);
      template.AddOrGet<HasSortOrder>();
    }
    return template;
  }

  public static GameObject ExtendEntityToMedicine(
    GameObject template,
    MedicineInfo medicineInfo)
  {
    template.AddOrGet<EntitySplitter>();
    template.GetComponent<KPrefabID>().AddTag(GameTags.Medicine, false);
    template.AddOrGet<MedicinalPill>().info = medicineInfo;
    return template;
  }

  public static GameObject ExtendPlantToFertilizable(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo[] fertilizers)
  {
    HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    foreach (PlantElementAbsorber.ConsumeInfo fertilizer in fertilizers)
    {
      ManualDeliveryKG manualDeliveryKg = template.AddComponent<ManualDeliveryKG>();
      manualDeliveryKg.RequestedItemTag = fertilizer.tag;
      manualDeliveryKg.capacity = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 3.0);
      manualDeliveryKg.refillMass = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.minimumMass = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
      manualDeliveryKg.choreTypeIDHash = idHash;
    }
    KPrefabID component1 = template.GetComponent<KPrefabID>();
    FertilizationMonitor.Def def = template.AddOrGetDef<FertilizationMonitor.Def>();
    def.wrongFertilizerTestTag = GameTags.Solid;
    def.consumedElements = fertilizers;
    component1.prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      foreach (ManualDeliveryKG component2 in inst.GetComponents<ManualDeliveryKG>())
        component2.Pause(true, "init");
    });
    return template;
  }

  public static GameObject ExtendPlantToIrrigated(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo info)
  {
    return EntityTemplates.ExtendPlantToIrrigated(template, new PlantElementAbsorber.ConsumeInfo[1]
    {
      info
    });
  }

  public static GameObject ExtendPlantToIrrigated(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo[] consume_info)
  {
    HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consume_info)
    {
      ManualDeliveryKG manualDeliveryKg = template.AddComponent<ManualDeliveryKG>();
      manualDeliveryKg.RequestedItemTag = consumeInfo.tag;
      manualDeliveryKg.capacity = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 3.0);
      manualDeliveryKg.refillMass = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.minimumMass = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.Functional;
      manualDeliveryKg.choreTypeIDHash = idHash;
    }
    IrrigationMonitor.Def def = template.AddOrGetDef<IrrigationMonitor.Def>();
    def.wrongIrrigationTestTag = GameTags.Liquid;
    def.consumedElements = consume_info;
    return template;
  }

  public static GameObject CreateAndRegisterCompostableFromPrefab(GameObject original)
  {
    if ((UnityEngine.Object) original.GetComponent<Compostable>() != (UnityEngine.Object) null)
      return (GameObject) null;
    original.AddComponent<Compostable>().isMarkedForCompost = false;
    KPrefabID component = original.GetComponent<KPrefabID>();
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
    string tag_string = "Compost" + component.PrefabTag.Name;
    string str = MISC.TAGS.COMPOST_FORMAT.Replace("{Item}", component.PrefabTag.ProperName());
    gameObject.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(tag_string, str);
    gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Compostable, false);
    gameObject.name = str;
    gameObject.GetComponent<Compostable>().isMarkedForCompost = true;
    gameObject.GetComponent<KSelectable>().SetName(str);
    gameObject.GetComponent<Compostable>().originalPrefab = original;
    gameObject.GetComponent<Compostable>().compostPrefab = gameObject;
    original.GetComponent<Compostable>().originalPrefab = original;
    original.GetComponent<Compostable>().compostPrefab = gameObject;
    Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
    return gameObject;
  }

  public static GameObject CreateAndRegisterSeedForPlant(
    GameObject plant,
    SeedProducer.ProductionType productionType,
    string id,
    string name,
    string desc,
    KAnimFile anim,
    string initialAnim = "object",
    int numberOfSeeds = 1,
    List<Tag> additionalTags = null,
    SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top,
    Tag replantGroundTag = default (Tag),
    int sortOrder = 0,
    string domesticatedDescription = "",
    EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE,
    float width = 0.25f,
    float height = 0.25f,
    Recipe.Ingredient[] recipe_ingredients = null,
    string recipe_description = "",
    bool ignoreDefaultSeedTag = false)
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(id, name, desc, 1f, true, anim, initialAnim, Grid.SceneLayer.Front, collisionShape, width, height, true, SORTORDER.SEEDS + sortOrder, SimHashes.Creature, (List<Tag>) null);
    looseEntity.AddOrGet<EntitySplitter>();
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    PlantableSeed plantableSeed = looseEntity.AddOrGet<PlantableSeed>();
    plantableSeed.PlantID = new Tag(plant.name);
    plantableSeed.replantGroundTag = replantGroundTag;
    plantableSeed.domesticatedDescription = domesticatedDescription;
    plantableSeed.direction = planterDirection;
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    foreach (Tag additionalTag in additionalTags)
      component.AddTag(additionalTag, false);
    if (!ignoreDefaultSeedTag)
      component.AddTag(GameTags.Seed, false);
    component.AddTag(GameTags.PedestalDisplayable, false);
    Assets.AddPrefab(looseEntity.GetComponent<KPrefabID>());
    plant.AddOrGet<SeedProducer>().Configure(looseEntity.name, productionType, numberOfSeeds);
    return looseEntity;
  }

  public static GameObject CreateAndRegisterPreview(
    string id,
    KAnimFile anim,
    string initial_anim,
    ObjectLayer object_layer,
    int width,
    int height)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, id, id, 1f, anim, initial_anim, Grid.SceneLayer.Front, width, height, TUNING.BUILDINGS.DECOR.NONE, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.UpdateComponentRequirement<KSelectable>(false);
    placedEntity.UpdateComponentRequirement<SaveLoadRoot>(false);
    placedEntity.AddOrGet<EntityPreview>().objectLayer = object_layer;
    OccupyArea occupyArea = placedEntity.AddOrGet<OccupyArea>();
    occupyArea.objectLayers = new ObjectLayer[1]
    {
      object_layer
    };
    occupyArea.ApplyToCells = false;
    placedEntity.AddOrGet<Storage>();
    Assets.AddPrefab(placedEntity.GetComponent<KPrefabID>());
    return placedEntity;
  }

  public static GameObject CreateAndRegisterPreviewForPlant(
    GameObject seed,
    string id,
    KAnimFile anim,
    string initialAnim,
    int width,
    int height)
  {
    GameObject andRegisterPreview = EntityTemplates.CreateAndRegisterPreview(id, anim, initialAnim, ObjectLayer.Building, width, height);
    seed.GetComponent<PlantableSeed>().PreviewID = TagManager.Create(id);
    return andRegisterPreview;
  }

  public static CellOffset[] GenerateOffsets(int width, int height)
  {
    int endX = width / 2;
    int startX = endX - width + 1;
    int startY = 0;
    int endY = height - 1;
    return EntityTemplates.GenerateOffsets(startX, startY, endX, endY);
  }

  private static CellOffset[] GenerateOffsets(
    int startX,
    int startY,
    int endX,
    int endY)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>();
    for (int index1 = startY; index1 <= endY; ++index1)
    {
      for (int index2 = startX; index2 <= endX; ++index2)
        cellOffsetList.Add(new CellOffset()
        {
          x = index2,
          y = index1
        });
    }
    return cellOffsetList.ToArray();
  }

  public static CellOffset[] GenerateHangingOffsets(int width, int height)
  {
    int endX = width / 2;
    int startX = endX - width + 1;
    int startY = -height + 1;
    int endY = 0;
    return EntityTemplates.GenerateOffsets(startX, startY, endX, endY);
  }

  public static GameObject AddCollision(
    GameObject template,
    EntityTemplates.CollisionShape shape,
    float width,
    float height)
  {
    switch (shape)
    {
      case EntityTemplates.CollisionShape.RECTANGLE:
        template.AddOrGet<KBoxCollider2D>().size = (Vector2) new Vector2f(width, height);
        break;
      case EntityTemplates.CollisionShape.POLYGONAL:
        template.AddOrGet<PolygonCollider2D>();
        break;
      default:
        template.AddOrGet<KCircleCollider2D>().radius = Mathf.Max(width, height);
        break;
    }
    return template;
  }

  public enum CollisionShape
  {
    CIRCLE,
    RECTANGLE,
    POLYGONAL,
  }
}
