// Decompiled with JetBrains decompiler
// Type: SeaLettuceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SeaLettuceConfig : IEntityConfig
{
  public static string ID = "SeaLettuce";
  public const float WATER_RATE = 0.008333334f;
  public const float FERTILIZATION_RATE = 0.0008333334f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(SeaLettuceConfig.ID, (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.NAME, (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.DESC, 1f, Assets.GetAnim((HashedString) "sea_lettuce_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 308.15f);
    GameObject template = placedEntity;
    float temperature_lethal_low = 248.15f;
    float temperature_warning_low = 295.15f;
    float temperature_warning_high = 338.15f;
    float temperature_lethal_high = 398.15f;
    bool pressure_sensitive = false;
    SimHashes[] safe_elements = new SimHashes[3]
    {
      SimHashes.Water,
      SimHashes.SaltWater,
      SimHashes.Brine
    };
    EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, safe_elements, pressure_sensitive, 0.0f, 0.15f, "Lettuce", true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.SaltWater.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.BleachStone.CreateTag(),
        massConsumptionRate = 0.0008333334f
      }
    });
    placedEntity.GetComponent<DrowningMonitor>().canDrownToDeath = false;
    placedEntity.GetComponent<DrowningMonitor>().livesUnderWater = true;
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, SeaLettuceConfig.ID + "Seed", (string) STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC, Assets.GetAnim((HashedString) "seed_sealettuce_kanim"), "object", 0, new List<Tag>()
    {
      GameTags.WaterSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 1, (string) STRINGS.CREATURES.SPECIES.SEALETTUCE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, (Recipe.Ingredient[]) null, string.Empty, false), SeaLettuceConfig.ID + "_preview", Assets.GetAnim((HashedString) "sea_lettuce_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", "SeaLettuce_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
