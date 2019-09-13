// Decompiled with JetBrains decompiler
// Type: BeanPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BeanPlantConfig : IEntityConfig
{
  public const string ID = "BeanPlant";
  public const string SEED_ID = "BeanPlantSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.03333334f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BeanPlant", (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME, (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DESC, 2f, Assets.GetAnim((HashedString) "beanplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 258.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 198.15f, 248.15f, 273.15f, 323.15f, (SimHashes[]) null, true, 0.0f, 0.15f, "BeanPlantSeed", true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Ethanol.CreateTag(),
        massConsumptionRate = 0.03333334f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Dirt.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
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
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "BeanPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC, Assets.GetAnim((HashedString) "seed_beanplant_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 4, (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false);
    EntityTemplates.ExtendEntityToFood(registerSeedForPlant, TUNING.FOOD.FOOD_TYPES.BEAN);
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "BeanPlant_preview", Assets.GetAnim((HashedString) "beanplant_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
