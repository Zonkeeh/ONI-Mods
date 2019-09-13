// Decompiled with JetBrains decompiler
// Type: PrickleGrassConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PrickleGrassConfig : IEntityConfig
{
  public static readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;
  public const string ID = "PrickleGrass";
  public const string SEED_ID = "PrickleGrassSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PrickleGrass", (string) STRINGS.CREATURES.SPECIES.PRICKLEGRASS.NAME, (string) STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DESC, 1f, Assets.GetAnim((HashedString) "bristlebriar_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, PrickleGrassConfig.POSITIVE_DECOR_EFFECT, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, (string) null, true, false, true, true, 2400f);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = PrickleGrassConfig.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = PrickleGrassConfig.NEGATIVE_DECOR_EFFECT;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "PrickleGrassSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEGRASS.DESC, Assets.GetAnim((HashedString) "seed_bristlebriar_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.DecorSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 5, (string) STRINGS.CREATURES.SPECIES.PRICKLEGRASS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, (Recipe.Ingredient[]) null, string.Empty, false), "PrickleGrass_preview", Assets.GetAnim((HashedString) "bristlebriar_kanim"), "place", 1, 1);
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
