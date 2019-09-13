// Decompiled with JetBrains decompiler
// Type: MinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MinionConfig : IEntityConfig
{
  public static string ID = "Minion";
  public static string MINION_BASE_TRAIT_ID = MinionConfig.ID + "BaseTrait";
  public const int MINION_BASE_SYMBOL_LAYER = 0;
  public const int MINION_HAIR_ALWAYS_HACK_LAYER = 1;
  public const int MINION_EXPRESSION_SYMBOL_LAYER = 2;
  public const int MINION_MOUTH_FLAP_LAYER = 3;
  public const int MINION_CLOTHING_SYMBOL_LAYER = 4;
  public const int MINION_PICKUP_SYMBOL_LAYER = 5;
  public const int MINION_SUIT_SYMBOL_LAYER = 6;

  public GameObject CreatePrefab()
  {
    string name = (string) DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME;
    GameObject entity = EntityTemplates.CreateEntity(MinionConfig.ID, name, true);
    entity.AddOrGet<StateMachineController>();
    MinionModifiers minionModifiers = entity.AddOrGet<MinionModifiers>();
    MinionConfig.AddMinionAmounts((Modifiers) minionModifiers);
    MinionConfig.AddMinionTraits(name, (Modifiers) minionModifiers);
    entity.AddOrGet<MinionBrain>();
    entity.AddOrGet<KPrefabID>().AddTag(GameTags.DupeBrain, false);
    entity.AddOrGet<Worker>();
    entity.AddOrGet<ChoreConsumer>();
    Storage storage = entity.AddOrGet<Storage>();
    storage.fxPrefix = Storage.FXPrefix.PickedUp;
    storage.dropOnLoad = true;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Preserve,
      Storage.StoredItemModifier.Seal
    });
    entity.AddOrGet<Health>();
    OxygenBreather oxygenBreather = entity.AddOrGet<OxygenBreather>();
    oxygenBreather.O2toCO2conversion = 0.02f;
    oxygenBreather.lowOxygenThreshold = 0.52f;
    oxygenBreather.noOxygenThreshold = 0.05f;
    oxygenBreather.mouthOffset = (Vector2) new Vector2f(0.25f, 0.7f);
    oxygenBreather.minCO2ToEmit = 0.02f;
    oxygenBreather.breathableCells = new CellOffset[6]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1),
      new CellOffset(1, 1),
      new CellOffset(-1, 1),
      new CellOffset(1, 0),
      new CellOffset(-1, 0)
    };
    entity.AddOrGet<WarmBlooded>();
    entity.AddOrGet<MinionIdentity>();
    GridVisibility gridVisibility = entity.AddOrGet<GridVisibility>();
    gridVisibility.radius = 30f;
    gridVisibility.innerRadius = 20f;
    entity.AddOrGet<MiningSounds>();
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<AntiCluster>();
    Navigator navigator = entity.AddOrGet<Navigator>();
    navigator.NavGridName = "MinionNavGrid";
    navigator.CurrentNavType = NavType.Floor;
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.Move;
    kbatchedAnimController.AnimFiles = new KAnimFile[8]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_firepole_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_new_kanim"),
      Assets.GetAnim((HashedString) "anim_loco_tube_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_firepole_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_jetsuit_kanim")
    };
    KBoxCollider2D kboxCollider2D = entity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = new Vector2(0.0f, 0.8f);
    kboxCollider2D.size = new Vector2(1f, 1.5f);
    entity.AddOrGet<SnapOn>().snapPoints = new List<SnapOn.SnapPoint>((IEnumerable<SnapOn.SnapPoint>) new SnapOn.SnapPoint[17]
    {
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "dig",
        buildFile = Assets.GetAnim((HashedString) "excavator_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "build",
        buildFile = Assets.GetAnim((HashedString) "constructor_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "fetchliquid",
        buildFile = Assets.GetAnim((HashedString) "water_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "paint",
        buildFile = Assets.GetAnim((HashedString) "painting_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "harvest",
        buildFile = Assets.GetAnim((HashedString) "plant_harvester_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "capture",
        buildFile = Assets.GetAnim((HashedString) "net_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "attack",
        buildFile = Assets.GetAnim((HashedString) "attack_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "pickup",
        buildFile = Assets.GetAnim((HashedString) "pickupdrop_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "store",
        buildFile = Assets.GetAnim((HashedString) "pickupdrop_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "disinfect",
        buildFile = Assets.GetAnim((HashedString) "plant_spray_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "tend",
        buildFile = Assets.GetAnim((HashedString) "plant_harvester_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "carry",
        automatic = false,
        context = (HashedString) string.Empty,
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_chest"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "build",
        automatic = false,
        context = (HashedString) string.Empty,
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "remote",
        automatic = false,
        context = (HashedString) string.Empty,
        buildFile = (KAnimFile) null,
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "snapTo_neck",
        automatic = false,
        context = (HashedString) string.Empty,
        buildFile = Assets.GetAnim((HashedString) "helm_oxygen_kanim"),
        overrideSymbol = (HashedString) "snapTo_neck"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "powertinker",
        buildFile = Assets.GetAnim((HashedString) "electrician_gun_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      },
      new SnapOn.SnapPoint()
      {
        pointName = "dig",
        automatic = false,
        context = (HashedString) "specialistdig",
        buildFile = Assets.GetAnim((HashedString) "excavator_kanim"),
        overrideSymbol = (HashedString) "snapTo_rgtHand"
      }
    });
    entity.AddOrGet<Effects>();
    entity.AddOrGet<Traits>();
    entity.AddOrGet<AttributeLevels>();
    entity.AddOrGet<AttributeConverters>();
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.InternalTemperature = 310.15f;
    primaryElement.MassPerUnit = 30f;
    primaryElement.ElementID = SimHashes.Creature;
    entity.AddOrGet<ChoreProvider>();
    entity.AddOrGetDef<DebugGoToMonitor.Def>();
    entity.AddOrGetDef<SpeechMonitor.Def>();
    entity.AddOrGetDef<BlinkMonitor.Def>();
    entity.AddOrGetDef<ConversationMonitor.Def>();
    entity.AddOrGet<Sensors>();
    entity.AddOrGet<Chattable>();
    entity.AddOrGet<FaceGraph>();
    entity.AddOrGet<Accessorizer>();
    entity.AddOrGet<Schedulable>();
    entity.AddOrGet<LoopingSounds>().updatePosition = true;
    entity.AddOrGet<AnimEventHandler>();
    entity.AddOrGet<FactionAlignment>().Alignment = FactionManager.FactionID.Duplicant;
    entity.AddOrGet<Weapon>();
    entity.AddOrGet<RangedAttackable>();
    entity.AddOrGet<CharacterOverlay>();
    OccupyArea occupyArea = entity.AddOrGet<OccupyArea>();
    occupyArea.objectLayers = new ObjectLayer[1];
    occupyArea.ApplyToCells = false;
    occupyArea.OccupiedCellsOffsets = new CellOffset[2]
    {
      new CellOffset(0, 0),
      new CellOffset(0, 1)
    };
    entity.AddOrGet<Pickupable>();
    CreatureSimTemperatureTransfer temperatureTransfer = entity.AddOrGet<CreatureSimTemperatureTransfer>();
    temperatureTransfer.SurfaceArea = 10f;
    temperatureTransfer.Thickness = 0.01f;
    entity.AddOrGet<SicknessTrigger>();
    entity.AddOrGet<ClothingWearer>();
    entity.AddOrGet<SuitEquipper>();
    DecorProvider decorProvider = entity.AddOrGet<DecorProvider>();
    decorProvider.baseRadius = 3f;
    decorProvider.isMovable = true;
    entity.AddOrGet<ConsumableConsumer>();
    entity.AddOrGet<NoiseListener>();
    entity.AddOrGet<MinionResume>();
    DuplicantNoiseLevels.SetupNoiseLevels();
    this.SetupLaserEffects(entity);
    SymbolOverrideControllerUtil.AddToPrefab(entity).applySymbolOverridesEveryFrame = true;
    MinionConfig.ConfigureSymbols(entity);
    return entity;
  }

  private void SetupLaserEffects(GameObject prefab)
  {
    GameObject gameObject = new GameObject("LaserEffect");
    gameObject.transform.parent = prefab.transform;
    KBatchedAnimEventToggler animEventToggler = gameObject.AddComponent<KBatchedAnimEventToggler>();
    animEventToggler.eventSource = prefab;
    animEventToggler.enableEvent = "LaserOn";
    animEventToggler.disableEvent = "LaserOff";
    animEventToggler.entries = new List<KBatchedAnimEventToggler.Entry>();
    MinionConfig.LaserEffect[] laserEffectArray = new MinionConfig.LaserEffect[13]
    {
      new MinionConfig.LaserEffect()
      {
        id = "DigEffect",
        animFile = "laser_kanim",
        anim = "idle",
        context = (HashedString) "dig"
      },
      new MinionConfig.LaserEffect()
      {
        id = "BuildEffect",
        animFile = "construct_beam_kanim",
        anim = "loop",
        context = (HashedString) "build"
      },
      new MinionConfig.LaserEffect()
      {
        id = "FetchLiquidEffect",
        animFile = "hose_fx_kanim",
        anim = "loop",
        context = (HashedString) "fetchliquid"
      },
      new MinionConfig.LaserEffect()
      {
        id = "PaintEffect",
        animFile = "paint_beam_kanim",
        anim = "loop",
        context = (HashedString) "paint"
      },
      new MinionConfig.LaserEffect()
      {
        id = "HarvestEffect",
        animFile = "plant_harvest_beam_kanim",
        anim = "loop",
        context = (HashedString) "harvest"
      },
      new MinionConfig.LaserEffect()
      {
        id = "CaptureEffect",
        animFile = "net_gun_fx_kanim",
        anim = "loop",
        context = (HashedString) "capture"
      },
      new MinionConfig.LaserEffect()
      {
        id = "AttackEffect",
        animFile = "attack_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "attack"
      },
      new MinionConfig.LaserEffect()
      {
        id = "PickupEffect",
        animFile = "vacuum_fx_kanim",
        anim = "loop",
        context = (HashedString) "pickup"
      },
      new MinionConfig.LaserEffect()
      {
        id = "StoreEffect",
        animFile = "vacuum_reverse_fx_kanim",
        anim = "loop",
        context = (HashedString) "store"
      },
      new MinionConfig.LaserEffect()
      {
        id = "DisinfectEffect",
        animFile = "plant_spray_beam_kanim",
        anim = "loop",
        context = (HashedString) "disinfect"
      },
      new MinionConfig.LaserEffect()
      {
        id = "TendEffect",
        animFile = "plant_tending_beam_fx_kanim",
        anim = "loop",
        context = (HashedString) "tend"
      },
      new MinionConfig.LaserEffect()
      {
        id = "PowerTinkerEffect",
        animFile = "electrician_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "powertinker"
      },
      new MinionConfig.LaserEffect()
      {
        id = "SpecialistDigEffect",
        animFile = "senior_miner_beam_fx_kanim",
        anim = "idle",
        context = (HashedString) "specialistdig"
      }
    };
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    foreach (MinionConfig.LaserEffect laserEffect in laserEffectArray)
    {
      GameObject go = new GameObject(laserEffect.id);
      go.transform.parent = gameObject.transform;
      go.AddOrGet<KPrefabID>().PrefabTag = new Tag(laserEffect.id);
      KBatchedAnimTracker kbatchedAnimTracker = go.AddOrGet<KBatchedAnimTracker>();
      kbatchedAnimTracker.controller = component;
      kbatchedAnimTracker.symbol = new HashedString("snapTo_rgtHand");
      kbatchedAnimTracker.offset = new Vector3(195f, -35f, 0.0f);
      kbatchedAnimTracker.useTargetPoint = true;
      KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) laserEffect.animFile)
      };
      KBatchedAnimEventToggler.Entry entry = new KBatchedAnimEventToggler.Entry()
      {
        anim = laserEffect.anim,
        context = laserEffect.context,
        controller = kbatchedAnimController
      };
      animEventToggler.entries.Add(entry);
      go.AddOrGet<LoopingSounds>();
    }
  }

  public void OnPrefabInit(GameObject go)
  {
    AmountInstance amountInstance1 = Db.Get().Amounts.ImmuneLevel.Lookup(go);
    amountInstance1.value = amountInstance1.GetMax();
    Db.Get().Amounts.Bladder.Lookup(go).value = Random.Range(0.0f, 10f);
    Db.Get().Amounts.Stress.Lookup(go).value = 5f;
    Db.Get().Amounts.Temperature.Lookup(go).value = 310.15f;
    AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
    amountInstance2.value = amountInstance2.GetMax();
    AmountInstance amountInstance3 = Db.Get().Amounts.Breath.Lookup(go);
    amountInstance3.value = amountInstance3.GetMax();
    AmountInstance amountInstance4 = Db.Get().Amounts.Calories.Lookup(go);
    amountInstance4.value = 0.8875f * amountInstance4.GetMax();
  }

  public void OnSpawn(GameObject go)
  {
    Sensors component1 = go.GetComponent<Sensors>();
    component1.Add((Sensor) new PathProberSensor(component1));
    component1.Add((Sensor) new SafeCellSensor(component1));
    component1.Add((Sensor) new IdleCellSensor(component1));
    component1.Add((Sensor) new PickupableSensor(component1));
    component1.Add((Sensor) new ClosestEdibleSensor(component1));
    component1.Add((Sensor) new BreathableAreaSensor(component1));
    component1.Add((Sensor) new AssignableReachabilitySensor(component1));
    component1.Add((Sensor) new ToiletSensor(component1));
    component1.Add((Sensor) new MingleCellSensor(component1));
    new RationalAi.Instance((IStateMachineTarget) go.GetComponent<StateMachineController>()).StartSM();
    if (go.GetComponent<OxygenBreather>().GetGasProvider() == null)
      go.GetComponent<OxygenBreather>().SetGasProvider((OxygenBreather.IGasProvider) new GasBreatherFromWorldProvider());
    Navigator component2 = go.GetComponent<Navigator>();
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new BipedTransitionLayer(component2, 3.325f, 2.5f));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new DoorTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new TubeTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new LadderDiseaseTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new ReactableTransitionLayer(component2));
    component2.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new SplashTransitionLayer(component2));
    ThreatMonitor.Instance smi = go.GetSMI<ThreatMonitor.Instance>();
    if (smi == null)
      return;
    smi.def.fleethresholdState = Health.HealthState.Critical;
  }

  public static void AddMinionAmounts(Modifiers modifiers)
  {
    modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Stamina.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Calories.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.ImmuneLevel.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Breath.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Stress.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Toxicity.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Bladder.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Temperature.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.ExternalTemperature.Id);
    modifiers.initialAmounts.Add(Db.Get().Amounts.Decor.Id);
  }

  public static void AddMinionTraits(string name, Modifiers modifiers)
  {
    Trait trait = Db.Get().CreateTrait(MinionConfig.MINION_BASE_TRAIT_ID, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, -0.1166667f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -1666.667f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 4000000f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Toxicity.deltaAttribute.Id, 0.0f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.AirConsumptionRate.Id, 0.1f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, 0.1666667f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 100f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.MaxUnderwaterTravelCost.Id, 8f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.DecorExpectation.Id, 0.0f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, 0.0f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, 1f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.RoomTemperaturePreference.Id, 0.0f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, 200f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 1f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.SpaceNavigation.Id, 1f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 0.0f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.ImmuneLevel.deltaAttribute.Id, 0.025f, name, false, false, true));
  }

  public static void ConfigureSymbols(GameObject go)
  {
    KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_hat", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_hat_hair", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_chest", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_goggles", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false);
    component.SetSymbolVisiblity((KAnimHashedString) "snapTo_rgtHand", false);
  }

  public struct LaserEffect
  {
    public string id;
    public string animFile;
    public string anim;
    public HashedString context;
  }
}
