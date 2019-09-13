// Decompiled with JetBrains decompiler
// Type: SaltPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SaltPlantConfig : IEntityConfig
{
  public const string ID = "SaltPlant";
  public const string SEED_ID = "SaltPlantSeed";
  public const float FERTILIZATION_RATE = 0.01166667f;
  public const float CHLORINE_CONSUMPTION_RATE = 0.006f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SaltPlant", (string) STRINGS.CREATURES.SPECIES.SALTPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SALTPLANT.DESC, 2f, Assets.GetAnim((HashedString) "saltplant_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, TUNING.DECOR.PENALTY.TIER1, new EffectorValues(), SimHashes.Creature, new List<Tag>()
    {
      GameTags.Hanging
    }, 258.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 198.15f, 248.15f, 323.15f, 393.15f, (SimHashes[]) null, true, 0.0f, 0.15f, SimHashes.Salt.ToString(), true, true, true, true, 2400f);
    placedEntity.AddOrGet<SaltPlant>();
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Sand.CreateTag(),
        massConsumptionRate = 7f / 600f
      }
    });
    PressureVulnerable pressureVulnerable = placedEntity.AddOrGet<PressureVulnerable>();
    float num1 = 0.025f;
    float num2 = 0.0f;
    SimHashes[] simHashesArray = new SimHashes[1]
    {
      SimHashes.ChlorineGas
    };
    double num3 = (double) num1;
    double num4 = (double) num2;
    SimHashes[] safeAtmospheres = simHashesArray;
    pressureVulnerable.Configure((float) num3, (float) num4, 10f, 30f, safeAtmospheres);
    placedEntity.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<PressureVulnerable>().safe_atmospheres.Add(ElementLoader.FindElementByHash(SimHashes.ChlorineGas)));
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.capacityKg = 1f;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.showInStatusPanel = true;
    elementConsumer.showDescriptor = true;
    elementConsumer.storeOnConsume = false;
    elementConsumer.elementToConsume = SimHashes.ChlorineGas;
    elementConsumer.configuration = ElementConsumer.Configuration.Element;
    elementConsumer.consumptionRadius = (byte) 4;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, -1f);
    elementConsumer.consumptionRate = 3f / 500f;
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
    placedEntity.AddOrGet<StandardCropPlant>();
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "SaltPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTPLANT.DESC, Assets.GetAnim((HashedString) "seed_saltplant_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Bottom, new Tag(), 4, (string) STRINGS.CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, (Recipe.Ingredient[]) null, string.Empty, false), "SaltPlant_preview", Assets.GetAnim((HashedString) "saltplant_kanim"), "place", 1, 2), 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    inst.GetComponent<ElementConsumer>().EnableConsumption(true);
  }
}
