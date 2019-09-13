// Decompiled with JetBrains decompiler
// Type: SpiceVineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SpiceVineConfig : IEntityConfig
{
  public const string ID = "SpiceVine";
  public const string SEED_ID = "SpiceVineSeed";
  public const float FERTILIZATION_RATE = 0.001666667f;
  public const float WATER_RATE = 0.05833333f;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SpiceVine", (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME, (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC, 2f, Assets.GetAnim((HashedString) "vinespicenut_kanim"), "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, new List<Tag>()
    {
      GameTags.Hanging
    }, 320f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 3);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, (SimHashes[]) null, true, 0.0f, 0.15f, SpiceNutConfig.ID, true, true, true, true, 2400f);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.05833333f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Phosphorite,
        massConsumptionRate = 1f / 600f
      }
    });
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
    placedEntity.AddOrGet<StandardCropPlant>();
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "SpiceVineSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC, Assets.GetAnim((HashedString) "seed_spicenut_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Bottom, new Tag(), 4, (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false), "SpiceVine_preview", Assets.GetAnim((HashedString) "vinespicenut_kanim"), "place", 1, 3), 1, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
