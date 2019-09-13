// Decompiled with JetBrains decompiler
// Type: BasePuftConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class BasePuftConfig
{
  public static GameObject BasePuft(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    bool is_baby,
    string symbol_override_prefix,
    float warningLowTemperature,
    float warningHighTemperature)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 50f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    GameObject template = placedEntity;
    FactionManager.FactionID faction = FactionManager.FactionID.Prey;
    string initialTraitID = traitId;
    string NavGridName = "FlyerNavGrid1x1";
    NavType navType = NavType.Hover;
    string onDeathDropID = "Meat";
    int onDeathDropCount = 1;
    float lethalLowTemperature = warningLowTemperature - 45f;
    float lethalHighTemperature = warningHighTemperature + 50f;
    EntityTemplates.ExtendEntityToBasicCreature(template, faction, initialTraitID, NavGridName, navType, 32, 2f, onDeathDropID, onDeathDropCount, true, true, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
    if (!string.IsNullOrEmpty(symbol_override_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_override_prefix, (string) null, 0);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer, false);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.SlimeMold
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_intake", NOISE_POLLUTION.CREATURES.TIER4);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_toot", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_air_inflated", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("puft_kanim", "Puft_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false, false);
    string str = "Puft_air_intake";
    if (is_baby)
      str = "PuftBaby_air_intake";
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def(), true).Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new UpTopPoopStates.Def(), true).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new InhaleStates.Def()
    {
      inhaleSound = str
    }, true).Add((StateMachine.BaseDef) new MoveToLureStates.Def(), true).Add((StateMachine.BaseDef) new CallAdultStates.Def(), true).PopInterruptGroup();
    IdleStates.Def def1 = new IdleStates.Def();
    IdleStates.Def def2 = def1;
    // ISSUE: reference to a compiler-generated field
    if (BasePuftConfig.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BasePuftConfig.\u003C\u003Ef__mg\u0024cache0 = new IdleStates.Def.IdleAnimCallback(BasePuftConfig.CustomIdleAnim);
    }
    // ISSUE: reference to a compiler-generated field
    IdleStates.Def.IdleAnimCallback fMgCache0 = BasePuftConfig.\u003C\u003Ef__mg\u0024cache0;
    def2.customIdleAnim = fMgCache0;
    IdleStates.Def def3 = def1;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def3, true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.PuftSpecies, symbol_override_prefix);
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Tag consumed_tag,
    Tag producedTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced,
    float minPoopSizeInKg)
  {
    Diet.Info[] diet_infos = new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>() { consumed_tag }, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    };
    return BasePuftConfig.SetupDiet(prefab, diet_infos, caloriesPerKg, minPoopSizeInKg);
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Diet.Info[] diet_infos,
    float caloriesPerKg,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos);
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = minPoopSizeInKg * caloriesPerKg;
    prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  private static HashedString CustomIdleAnim(
    IdleStates.Instance smi,
    ref HashedString pre_anim)
  {
    CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
    return (HashedString) (smi1 == null || !smi1.stomach.IsReadyToPoop() ? "idle_loop" : "idle_loop_full");
  }

  public static void OnSpawn(GameObject inst)
  {
    Navigator component = inst.GetComponent<Navigator>();
    component.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new FullPuftTransitionLayer(component));
  }
}
