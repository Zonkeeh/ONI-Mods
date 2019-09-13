// Decompiled with JetBrains decompiler
// Type: LeafyPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class LeafyPlantConfig : IEntityConfig
{
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;
  public const string ID = "LeafyPlant";
  public const string SEED_ID = "LeafyPlantSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("LeafyPlant", (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.DESC, 1f, Assets.GetAnim((HashedString) "potted_leafy_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, this.POSITIVE_DECOR_EFFECT, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 288f, 293.15f, 323.15f, 373f, new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.ChlorineGas,
      SimHashes.Hydrogen
    }, true, 0.0f, 0.15f, (string) null, true, false, true, true, 2400f);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "LeafyPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.LEAFYPLANT.DESC, Assets.GetAnim((HashedString) "seed_potted_leafy_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.DecorSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 7, (string) STRINGS.CREATURES.SPECIES.LEAFYPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, (Recipe.Ingredient[]) null, string.Empty, false), "LeafyPlant_preview", Assets.GetAnim((HashedString) "potted_leafy_kanim"), "place", 1, 1);
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
