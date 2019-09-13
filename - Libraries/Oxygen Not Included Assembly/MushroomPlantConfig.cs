// Decompiled with JetBrains decompiler
// Type: MushroomPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MushroomPlantConfig : IEntityConfig
{
  public const float FERTILIZATION_RATE = 0.006666667f;
  public const string ID = "MushroomPlant";
  public const string SEED_ID = "MushroomSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MushroomPlant", (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DESC, 1f, Assets.GetAnim((HashedString) "fungusplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 228.15f, 278.15f, 308.15f, 398.15f, new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, MushroomConfig.ID, true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.SlimeMold,
        massConsumptionRate = 0.006666667f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "MushroomSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.DESC, Assets.GetAnim((HashedString) "seed_fungusplant_kanim"), "object", 0, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.33f, 0.33f, (Recipe.Ingredient[]) null, string.Empty, false), "MushroomPlant_preview", Assets.GetAnim((HashedString) "fungusplant_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
