// Decompiled with JetBrains decompiler
// Type: ForestForagePlantPlantedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ForestForagePlantPlantedConfig : IEntityConfig
{
  public const string ID = "ForestForagePlantPlanted";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestForagePlantPlanted", (string) STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.NAME, (string) STRINGS.CREATURES.SPECIES.FORESTFORAGEPLANTPLANTED.DESC, 100f, Assets.GetAnim((HashedString) "podmelon_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 2, TUNING.DECOR.BONUS.TIER1, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.AddOrGet<SimTemperatureTransfer>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<DrowningMonitor>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<SeedProducer>().Configure("ForestForagePlant", SeedProducer.ProductionType.DigOnly, 1);
    placedEntity.AddOrGet<BasicForagePlantPlanted>();
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
