// Decompiled with JetBrains decompiler
// Type: ColdBreatherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ColdBreatherConfig : IEntityConfig
{
  public static readonly Tag TAG = TagManager.Create("ColdBreather");
  public static readonly Tag SEED_TAG = TagManager.Create("ColdBreatherSeed");
  public const string ID = "ColdBreather";
  public const float FERTILIZATION_RATE = 0.006666667f;
  public const SimHashes FERTILIZER = SimHashes.Phosphorite;
  public const float TEMP_DELTA = -5f;
  public const float CONSUMPTION_RATE = 1f;
  public const string SEED_ID = "ColdBreatherSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ColdBreather", (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.NAME, (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.DESC, 400f, Assets.GetAnim((HashedString) "coldbreather_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 2, TUNING.DECOR.BONUS.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER2, SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.AddOrGet<ReceptacleMonitor>();
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<WiltCondition>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<DrowningMonitor>();
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Phosphorite.CreateTag(),
        massConsumptionRate = 0.006666667f
      }
    });
    placedEntity.AddOrGet<TemperatureVulnerable>().Configure(213.15f, 183.15f, 368.15f, 463.15f);
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    ColdBreather coldBreather = placedEntity.AddOrGet<ColdBreather>();
    coldBreather.deltaEmitTemperature = -5f;
    coldBreather.emitOffsetCell = new Vector3(0.0f, 1f);
    coldBreather.consumptionRate = 1f;
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    BuildingTemplates.CreateDefaultStorage(placedEntity, false).showInUI = false;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.storeOnConsume = true;
    elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer.capacityKG = 2f;
    elementConsumer.consumptionRate = 0.25f;
    elementConsumer.consumptionRadius = (byte) 1;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    SimTemperatureTransfer component = placedEntity.GetComponent<SimTemperatureTransfer>();
    component.SurfaceArea = 10f;
    component.Thickness = 1f / 1000f;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "ColdBreatherSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC, Assets.GetAnim((HashedString) "seed_coldbreather_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false), "ColdBreather_preview", Assets.GetAnim((HashedString) "coldbreather_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_intake", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
