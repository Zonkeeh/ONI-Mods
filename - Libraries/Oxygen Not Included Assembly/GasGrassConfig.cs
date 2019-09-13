// Decompiled with JetBrains decompiler
// Type: GasGrassConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GasGrassConfig : IEntityConfig
{
  public const string ID = "GasGrass";
  public const string SEED_ID = "GasGrassSeed";
  public const float FERTILIZATION_RATE = 0.0008333334f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GasGrass", (string) STRINGS.CREATURES.SPECIES.GASGRASS.NAME, (string) STRINGS.CREATURES.SPECIES.GASGRASS.DESC, 1f, Assets.GetAnim((HashedString) "gassygrass_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, TUNING.DECOR.BONUS.TIER3, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, (float) byte.MaxValue);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 218.15f, 0.0f, 348.15f, 373.15f, (SimHashes[]) null, true, 0.0f, 0.15f, "GasGrassHarvested", true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Chlorine,
        massConsumptionRate = 0.0008333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<HarvestDesignatable>().defaultHarvestStateWhenPlanted = false;
    CropSleepingMonitor.Def def = placedEntity.AddOrGetDef<CropSleepingMonitor.Def>();
    def.lightIntensityThreshold = 20000f;
    def.prefersDarkness = false;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "GasGrassSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.DESC, Assets.GetAnim((HashedString) "seed_gassygrass_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.GASGRASS.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f, (Recipe.Ingredient[]) null, string.Empty, false), "GasGrass_preview", Assets.GetAnim((HashedString) "gassygrass_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
