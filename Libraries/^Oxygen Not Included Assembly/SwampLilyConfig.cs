// Decompiled with JetBrains decompiler
// Type: SwampLilyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SwampLilyConfig : IEntityConfig
{
  public static string ID = "SwampLily";
  public const string SEED_ID = "SwampLilySeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SwampLily", (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME, (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC, 1f, Assets.GetAnim((HashedString) "swamplily_kanim"), "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 328.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[1]
    {
      SimHashes.ChlorineGas
    }, true, 0.0f, 0.15f, SwampLilyFlowerConfig.ID, true, true, true, true, 2400f);
    placedEntity.AddOrGet<StandardCropPlant>();
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Harvest, "SwampLilySeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC, Assets.GetAnim((HashedString) "seed_swampLily_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.CropSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 4, (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, (Recipe.Ingredient[]) null, string.Empty, false), SwampLilyConfig.ID + "_preview", Assets.GetAnim((HashedString) "swamplily_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", TUNING.NOISE_POLLUTION.CREATURES.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
