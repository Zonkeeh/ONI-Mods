// Decompiled with JetBrains decompiler
// Type: ColdWheatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ColdWheatConfig : IEntityConfig
{
  public const string ID = "ColdWheat";
  public const string SEED_ID = "ColdWheatSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.03333334f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ColdWheat", (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME, (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DESC, 1f, Assets.GetAnim((HashedString) "coldwheat_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, (float) byte.MaxValue);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 118.15f, 218.15f, 278.15f, 358.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, "ColdWheatSeed", true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.03333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.DigOnly, "ColdWheatSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC, Assets.GetAnim((HashedString) "seed_coldwheat_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f, (Recipe.Ingredient[]) null, string.Empty, true);
    EntityTemplates.ExtendEntityToFood(registerSeedForPlant, TUNING.FOOD.FOOD_TYPES.COLD_WHEAT_SEED);
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "ColdWheat_preview", Assets.GetAnim((HashedString) "coldwheat_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
