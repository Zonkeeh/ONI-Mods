// Decompiled with JetBrains decompiler
// Type: GlomConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class GlomConfig : IEntityConfig
{
  public const string ID = "Glom";
  public const string BASE_TRAIT_ID = "GlomBaseTrait";
  public const SimHashes dirtyEmitElement = SimHashes.ContaminatedOxygen;
  public const float dirtyProbabilityPercent = 25f;
  public const float dirtyCellToTargetMass = 1f;
  public const float dirtyMassPerDirty = 0.2f;
  public const float dirtyMassReleaseOnDeath = 3f;
  public const string emitDisease = "SlimeLung";
  public const int emitDiseasePerKg = 1000;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.GLOM.NAME;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("Glom", name, (string) STRINGS.CREATURES.SPECIES.GLOM.DESC, 25f, Assets.GetAnim((HashedString) "glom_kanim"), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    Db.Get().CreateTrait("GlomBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true).Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker, false);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Pest, "GlomBaseTrait", "WalkerNavGrid1x1", NavType.Floor, 32, 2f, string.Empty, 0, true, true, 293.15f, 393.15f, 273.15f, 423.15f);
    placedEntity.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f);
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    ElementDropperMonitor.Def def = placedEntity.AddOrGetDef<ElementDropperMonitor.Def>();
    def.dirtyEmitElement = SimHashes.ContaminatedOxygen;
    def.dirtyProbabilityPercent = 25f;
    def.dirtyCellToTargetMass = 1f;
    def.dirtyMassPerDirty = 0.2f;
    def.dirtyMassReleaseOnDeath = 3f;
    def.emitDiseaseIdx = Db.Get().Diseases.GetIndex((HashedString) "SlimeLung");
    def.emitDiseasePerKg = 1000f;
    placedEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.GetComponent<LoopingSounds>().updatePosition = true;
    placedEntity.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
    SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_movement_short", TUNING.NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_jump", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_land", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_expel", TUNING.NOISE_POLLUTION.CREATURES.TIER4);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, false, false);
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new TrappedStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new FallStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new FleeStates.Def(), true).Add((StateMachine.BaseDef) new DropElementStates.Def(), true).Add((StateMachine.BaseDef) new IdleStates.Def(), true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.GlomSpecies, (string) null);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
