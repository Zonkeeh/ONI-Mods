// Decompiled with JetBrains decompiler
// Type: BaseOilFloaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class BaseOilFloaterConfig
{
  public static GameObject BaseOilFloater(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    float warnLowTemp,
    float warnHighTemp,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 50f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0));
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Hoverer, false);
    GameObject template = placedEntity;
    FactionManager.FactionID faction = FactionManager.FactionID.Pest;
    string initialTraitID = traitId;
    string NavGridName = "FloaterNavGrid";
    NavType navType = NavType.Hover;
    string onDeathDropID = "Meat";
    int onDeathDropCount = 2;
    float warningHighTemperature = warnHighTemp;
    EntityTemplates.ExtendEntityToBasicCreature(template, faction, initialTraitID, NavGridName, navType, 32, 2f, onDeathDropID, onDeathDropCount, true, false, warnLowTemp, warningHighTemperature, warnLowTemp - 15f, warnHighTemp + 20f);
    if (!string.IsNullOrEmpty(symbolOverridePrefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix, (string) null, 0);
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false, false);
    string str1 = "OilFloater_intake_air";
    if (is_baby)
      str1 = "OilFloaterBaby_intake_air";
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new TrappedStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new FallStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def(), true).Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new InhaleStates.Def()
    {
      inhaleSound = str1
    }, true).Add((StateMachine.BaseDef) new SameSpotPoopStates.Def(), true).Add((StateMachine.BaseDef) new CallAdultStates.Def(), true).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def(), true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.OilFloaterSpecies, symbolOverridePrefix);
    string str2 = "OilFloater_move_LP";
    if (is_baby)
      str2 = "OilFloaterBaby_move_LP";
    placedEntity.AddOrGet<OilFloaterMovementSound>().sound = str2;
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
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>() { consumed_tag }, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, false)
    });
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = minPoopSizeInKg * caloriesPerKg;
    prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }
}
