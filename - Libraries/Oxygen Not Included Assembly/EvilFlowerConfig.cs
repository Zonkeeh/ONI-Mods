// Decompiled with JetBrains decompiler
// Type: EvilFlowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EvilFlowerConfig : IEntityConfig
{
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER7;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER5;
  public const string ID = "EvilFlower";
  public const string SEED_ID = "EvilFlowerSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("EvilFlower", (string) STRINGS.CREATURES.SPECIES.EVILFLOWER.NAME, (string) STRINGS.CREATURES.SPECIES.EVILFLOWER.DESC, 1f, Assets.GetAnim((HashedString) "potted_evilflower_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, this.POSITIVE_DECOR_EFFECT, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 168.15f, 258.15f, 513.15f, 563.15f, new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, (string) null, true, false, true, true, 2400f);
    EvilFlower evilFlower = placedEntity.AddOrGet<EvilFlower>();
    evilFlower.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    evilFlower.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "EvilFlowerSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.EVILFLOWER.DESC, Assets.GetAnim((HashedString) "seed_potted_evilflower_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.DecorSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 5, (string) STRINGS.CREATURES.SPECIES.EVILFLOWER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, (Recipe.Ingredient[]) null, string.Empty, false), "EvilFlower_preview", Assets.GetAnim((HashedString) "potted_evilflower_kanim"), "place", 1, 1);
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    DiseaseDropper.Def def = placedEntity.AddOrGetDef<DiseaseDropper.Def>();
    def.diseaseIdx = Db.Get().Diseases.GetIndex((HashedString) "ZombieSpores");
    def.emitFrequency = 1f;
    def.averageEmitPerSecond = 1000;
    def.singleEmitQuantity = 100000;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
