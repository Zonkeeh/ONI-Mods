// Decompiled with JetBrains decompiler
// Type: PrickleFlowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PrickleFlowerConfig : IEntityConfig
{
  public const float WATER_RATE = 0.03333334f;
  public const string ID = "PrickleFlower";
  public const string SEED_ID = "PrickleFlowerSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PrickleFlower", (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.NAME, (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DESC, 1f, Assets.GetAnim((HashedString) "bristleblossom_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 218.15f, 278.15f, 303.15f, 398.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, PrickleFruitConfig.ID, true, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.03333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    DiseaseDropper.Def def = placedEntity.AddOrGetDef<DiseaseDropper.Def>();
    def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
    def.singleEmitQuantity = 1000000;
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(false);
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "PrickleFlowerSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.DESC, Assets.GetAnim((HashedString) "seed_bristleblossom_kanim"), "object", 0, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 2, (string) STRINGS.CREATURES.SPECIES.PRICKLEFLOWER.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, (Recipe.Ingredient[]) null, string.Empty, false), "PrickleFlower_preview", Assets.GetAnim((HashedString) "bristleblossom_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<PrimaryElement>().Temperature = 288.15f;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
