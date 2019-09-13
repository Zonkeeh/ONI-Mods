// Decompiled with JetBrains decompiler
// Type: OxyfernConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class OxyfernConfig : IEntityConfig
{
  public const string ID = "Oxyfern";
  public const string SEED_ID = "OxyfernSeed";
  public const float WATER_CONSUMPTION_RATE = 0.03166667f;
  public const float FERTILIZATION_RATE = 0.006666667f;
  public const float CO2_RATE = 0.000625f;
  private const float CONVERSION_RATIO = 50f;
  public const float OXYGEN_RATE = 0.03125f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("Oxyfern", (string) STRINGS.CREATURES.SPECIES.OXYFERN.NAME, (string) STRINGS.CREATURES.SPECIES.OXYFERN.DESC, 1f, Assets.GetAnim((HashedString) "oxy_fern_kanim"), "idle_full", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.PENALTY.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.AddOrGet<ReceptacleMonitor>();
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<WiltCondition>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<DrowningMonitor>();
    placedEntity.AddOrGet<TemperatureVulnerable>().Configure(273.15f, 253.15f, 313.15f, 373.15f);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.03166667f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.006666667f
      }
    });
    placedEntity.AddOrGet<Oxyfern>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    PressureVulnerable pressureVulnerable = placedEntity.AddOrGet<PressureVulnerable>();
    float num1 = 0.025f;
    float num2 = 0.0f;
    SimHashes[] simHashesArray = new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    };
    double num3 = (double) num1;
    double num4 = (double) num2;
    SimHashes[] safeAtmospheres = simHashesArray;
    pressureVulnerable.Configure((float) num3, (float) num4, 10f, 30f, safeAtmospheres);
    placedEntity.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<PressureVulnerable>().safe_atmospheres.Add(ElementLoader.FindElementByHash(SimHashes.CarbonDioxide)));
    placedEntity.AddOrGet<LoopingSounds>();
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.capacityKg = 1f;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.showInStatusPanel = false;
    elementConsumer.storeOnConsume = true;
    elementConsumer.storage = storage;
    elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    elementConsumer.configuration = ElementConsumer.Configuration.Element;
    elementConsumer.consumptionRadius = (byte) 2;
    elementConsumer.EnableConsumption(true);
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    elementConsumer.consumptionRate = 0.00015625f;
    ElementConverter elementConverter = placedEntity.AddOrGet<ElementConverter>();
    elementConverter.OutputMultiplier = 50f;
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(SimHashes.CarbonDioxide.ToString().ToTag(), 0.000625f)
    };
    elementConverter.outputElements = new ElementConverter.OutputElement[1]
    {
      new ElementConverter.OutputElement(0.03125f, SimHashes.Oxygen, 0.0f, true, false, 0.0f, 1f, 0.75f, byte.MaxValue, 0)
    };
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "OxyfernSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.OXYFERN.DESC, Assets.GetAnim((HashedString) "seed_oxyfern_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.OXYFERN.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false), "Oxyfern_preview", Assets.GetAnim((HashedString) "oxy_fern_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", TUNING.NOISE_POLLUTION.CREATURES.TIER4);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    inst.GetComponent<Oxyfern>().SetConsumptionRate();
  }
}
