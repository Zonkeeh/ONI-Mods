// Decompiled with JetBrains decompiler
// Type: ForestTreeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ForestTreeConfig : IEntityConfig
{
  public const string ID = "ForestTree";
  public const string SEED_ID = "ForestTreeSeed";
  public const float FERTILIZATION_RATE = 0.01666667f;
  public const float WATER_RATE = 0.1166667f;
  public const float BRANCH_GROWTH_TIME = 2100f;
  public const int NUM_BRANCHES = 7;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestTree", (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME, (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC, 2f, Assets.GetAnim((HashedString) "tree_kanim"), "idle_empty", Grid.SceneLayer.Building, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, new List<Tag>(), 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 313.15f, 448.15f, (SimHashes[]) null, true, 0.0f, 0.15f, "WoodLog", true, true, true, false, 2400f);
    placedEntity.AddOrGet<BuddingTrunk>();
    placedEntity.UpdateComponentRequirement<Harvestable>(false);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.1166667f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.01666667f
      }
    });
    placedEntity.AddComponent<StandardCropPlant>();
    placedEntity.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
    placedEntity.AddOrGet<BuddingTrunk>().budPrefabID = "ForestTreeBranch";
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "ForestTreeSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC, Assets.GetAnim((HashedString) "seed_tree_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 4, (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false), "ForestTree_preview", Assets.GetAnim((HashedString) "tree_kanim"), "place", 3, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
