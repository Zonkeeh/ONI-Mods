// Decompiled with JetBrains decompiler
// Type: BaseLightBugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class BaseLightBugConfig
{
  public static GameObject BaseLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    Color lightColor,
    EffectorValues decor,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 5f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Prey, traitId, "FlyerNavGrid1x1", NavType.Hover, 32, 2f, "Meat", 0, true, true, CREATURES.TEMPERATURE.FREEZING_1, CREATURES.TEMPERATURE.HOT_1, CREATURES.TEMPERATURE.FREEZING_2, CREATURES.TEMPERATURE.HOT_2);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix, (string) null, 0);
    KPrefabID component1 = placedEntity.GetComponent<KPrefabID>();
    component1.AddTag(GameTags.Creatures.Flyer, false);
    component1.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Phosphorite
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false, false);
    if (is_baby)
    {
      KBatchedAnimController component2 = placedEntity.GetComponent<KBatchedAnimController>();
      component2.animWidth = 0.5f;
      component2.animHeight = 0.5f;
    }
    if (lightColor != Color.black)
    {
      Light2D light2D = placedEntity.AddOrGet<Light2D>();
      light2D.Color = lightColor;
      light2D.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
      light2D.Range = 5f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
      light2D.Offset = LIGHT2D.LIGHTBUG_OFFSET;
      light2D.shape = LightShape.Circle;
      light2D.drawOverlay = true;
      light2D.Lux = 1800;
      placedEntity.AddOrGet<LightSymbolTracker>().targetSymbol = (HashedString) "snapTo_light_locator";
      placedEntity.AddOrGetDef<CreatureLightToggleController.Def>();
    }
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def(), true).Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new EatStates.Def(), true).Add((StateMachine.BaseDef) new MoveToLureStates.Def(), true).Add((StateMachine.BaseDef) new CallAdultStates.Def(), true).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def(), true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.LightBugSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    HashSet<Tag> consumed_tags,
    Tag producedTag,
    float caloriesPerKg)
  {
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(consumed_tags, producedTag, caloriesPerKg, 1f, (string) null, 0.0f, false, false)
    });
    prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  public static void SetupLoopingSounds(GameObject inst)
  {
    inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("ShineBug_wings_LP", false));
  }
}
