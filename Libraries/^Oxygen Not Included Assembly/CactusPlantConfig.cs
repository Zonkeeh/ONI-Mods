// Decompiled with JetBrains decompiler
// Type: CactusPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CactusPlantConfig : IEntityConfig
{
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;
  public const string ID = "CactusPlant";
  public const string SEED_ID = "CactusPlantSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("CactusPlant", (string) STRINGS.CREATURES.SPECIES.CACTUSPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.CACTUSPLANT.DESC, 1f, Assets.GetAnim((HashedString) "potted_cactus_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, this.POSITIVE_DECOR_EFFECT, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 200f, 273.15f, 373.15f, 400f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, false, 0.0f, 0.15f, (string) null, true, false, true, true, 2400f);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "CactusPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.CACTUSPLANT.DESC, Assets.GetAnim((HashedString) "seed_potted_cactus_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.DecorSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 8, (string) STRINGS.CREATURES.SPECIES.CACTUSPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, (Recipe.Ingredient[]) null, string.Empty, false), "CactusPlant_preview", Assets.GetAnim((HashedString) "potted_cactus_kanim"), "place", 1, 1);
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
