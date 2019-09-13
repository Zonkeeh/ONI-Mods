// Decompiled with JetBrains decompiler
// Type: BulbPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BulbPlantConfig : IEntityConfig
{
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER1;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;
  public const string ID = "BulbPlant";
  public const string SEED_ID = "BulbPlantSeed";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BulbPlant", (string) STRINGS.CREATURES.SPECIES.BULBPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.BULBPLANT.DESC, 1f, Assets.GetAnim((HashedString) "potted_bulb_kanim"), "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, this.POSITIVE_DECOR_EFFECT, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 288f, 293.15f, 313.15f, 333.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, true, 0.0f, 0.15f, (string) null, true, false, true, true, 2400f);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, SeedProducer.ProductionType.Hidden, "BulbPlantSeed", (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBPLANT.DESC, Assets.GetAnim((HashedString) "seed_potted_bulb_kanim"), "object", 1, new List<Tag>()
    {
      GameTags.DecorSeed
    }, SingleEntityReceptacle.ReceptacleDirection.Top, new Tag(), 6, (string) STRINGS.CREATURES.SPECIES.BULBPLANT.DOMESTICATEDDESC, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, (Recipe.Ingredient[]) null, string.Empty, false), "BulbPlant_preview", Assets.GetAnim((HashedString) "potted_bulb_kanim"), "place", 1, 1);
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    DiseaseDropper.Def def = placedEntity.AddOrGetDef<DiseaseDropper.Def>();
    def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
    def.singleEmitQuantity = 0;
    def.averageEmitPerSecond = 5000;
    def.emitFrequency = 5f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
