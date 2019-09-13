// Decompiled with JetBrains decompiler
// Type: BasicSingleHarvestPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BasicSingleHarvestPlantConfig : IEntityConfig
{
  public const string ID = "BasicSingleHarvestPlant";
  public const string SEED_ID = "BasicSingleHarvestPlantSeed";
  public const float DIRT_RATE = 0.01666667f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BasicSingleHarvestPlant", (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DESC, 1f, Assets.GetAnim((HashedString) "meallice_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.PENALTY.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 218.15f, 283.15f, 303.15f, 398.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, "BasicPlantFood", true, false, true, true, 2400f);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "BasicSingleHarvestPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.DESC, Assets.GetAnim((HashedString) "seed_meallice_kanim"), "object", 0, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 1, (string) STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.01666667f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "BasicSingleHarvestPlant_preview", Assets.GetAnim((HashedString) "meallice_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", TUNING.NOISE_POLLUTION.CREATURES.TIER4);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
