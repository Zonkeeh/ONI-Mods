// Decompiled with JetBrains decompiler
// Type: BasicFabricMaterialPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BasicFabricMaterialPlantConfig : IEntityConfig
{
  public static string ID = "BasicFabricPlant";
  public static string SEED_ID = "BasicFabricMaterialPlantSeed";
  public const float WATER_RATE = 0.2666667f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(BasicFabricMaterialPlantConfig.ID, (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC, 1f, Assets.GetAnim((HashedString) "swampreed_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 3, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    GameObject template = placedEntity;
    string id = BasicFabricConfig.ID;
    SimHashes[] safe_elements = new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.DirtyWater,
      SimHashes.Water
    };
    EntityTemplates.ExtendEntityToBasicPlant(template, 248.15f, 295.15f, 310.15f, 398.15f, safe_elements, false, 0.0f, 0.15f, id, false, true, true, true, 2400f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.DirtyWater,
        massConsumptionRate = 0.2666667f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, BasicFabricMaterialPlantConfig.SEED_ID, (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC, Assets.GetAnim((HashedString) "seed_swampreed_kanim"), "object", 0, new List<Tag>()
    {
      GameTags.WaterSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 1, (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, (Recipe.Ingredient[]) null, string.Empty, false), BasicFabricMaterialPlantConfig.ID + "_preview", Assets.GetAnim((HashedString) "swampreed_kanim"), "place", 1, 3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
