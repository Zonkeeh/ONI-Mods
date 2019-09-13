// Decompiled with JetBrains decompiler
// Type: BaseHatchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class BaseHatchConfig
{
  public static GameObject BaseHatch(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 100f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    string NavGridName = "WalkerNavGrid1x1";
    if (is_baby)
      NavGridName = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Pest, traitId, NavGridName, NavType.Floor, 32, 2f, "Meat", 2, true, false, 283.15f, 293.15f, 243.15f, 343.15f);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix, (string) null, 0);
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<BurrowMonitor.Def>();
    WorldSpawnableMonitor.Def def = placedEntity.AddOrGetDef<WorldSpawnableMonitor.Def>();
    // ISSUE: reference to a compiler-generated field
    if (BaseHatchConfig.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaseHatchConfig.\u003C\u003Ef__mg\u0024cache0 = new Func<int, int>(BaseHatchConfig.AdjustSpawnLocationCB);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, int> fMgCache0 = BaseHatchConfig.\u003C\u003Ef__mg\u0024cache0;
    def.adjustSpawnLocationCb = fMgCache0;
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", TUNING.NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", TUNING.NOISE_POLLUTION.CREATURES.TIER1);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", TUNING.NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", TUNING.NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", TUNING.NOISE_POLLUTION.CREATURES.TIER6);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true, false);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker, false);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    bool condition = !is_baby;
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new ExitBurrowStates.Def(), condition).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), condition).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new TrappedStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new FallStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new FleeStates.Def(), true).Add((StateMachine.BaseDef) new AttackStates.Def(), condition).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def(), true).Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), condition).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new EatStates.Def(), true).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true).Add((StateMachine.BaseDef) new CallAdultStates.Def(), true).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def(), true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.HatchSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static List<Diet.Info> BasicRockDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Sand.CreateTag(),
        SimHashes.SandStone.CreateTag(),
        SimHashes.Clay.CreateTag(),
        SimHashes.CrushedRock.CreateTag(),
        SimHashes.Dirt.CreateTag(),
        SimHashes.SedimentaryRock.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    };
  }

  public static List<Diet.Info> HardRockDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.SedimentaryRock.CreateTag(),
        SimHashes.IgneousRock.CreateTag(),
        SimHashes.Obsidian.CreateTag(),
        SimHashes.Granite.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    };
  }

  public static List<Diet.Info> MetalDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.Cuprite.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Copper.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.GoldAmalgam.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Gold.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.IronOre.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Iron.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.Wolframite.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Tungsten.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.AluminumOre.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Aluminum.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false),
      new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        SimHashes.Electrum.CreateTag()
      }), !(poopTag == GameTags.Metal) ? poopTag : SimHashes.Gold.CreateTag(), caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    };
  }

  public static List<Diet.Info> VeggieDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Dirt.CreateTag(),
        SimHashes.SlimeMold.CreateTag(),
        SimHashes.Algae.CreateTag(),
        SimHashes.Fertilizer.CreateTag(),
        SimHashes.ToxicSand.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    };
  }

  public static List<Diet.Info> FoodDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    foreach (EdiblesManager.FoodInfo foodTypes in TUNING.FOOD.FOOD_TYPES_LIST)
    {
      if ((double) foodTypes.CaloriesPerUnit > 0.0)
        infoList.Add(new Diet.Info(new HashSet<Tag>()
        {
          new Tag(foodTypes.Id)
        }, poopTag, foodTypes.CaloriesPerUnit, producedConversionRate, diseaseId, diseasePerKgProduced, false, false));
    }
    return infoList;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    List<Diet.Info> diet_infos,
    float referenceCaloriesPerKg,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos.ToArray());
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = referenceCaloriesPerKg * minPoopSizeInKg;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  private static int AdjustSpawnLocationCB(int cell)
  {
    int num;
    for (; !Grid.Solid[cell]; cell = num)
    {
      num = Grid.CellBelow(cell);
      if (!Grid.IsValidCell(cell))
        break;
    }
    return cell;
  }
}
