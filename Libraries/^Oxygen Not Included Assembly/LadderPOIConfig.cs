// Decompiled with JetBrains decompiler
// Type: LadderPOIConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class LadderPOIConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    int width = 1;
    int height = 1;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropLadder", (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.NAME, (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.DESC, 50f, Assets.GetAnim((HashedString) "ladder_poi_kanim"), "off", Grid.SceneLayer.Building, width, height, TUNING.BUILDINGS.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, (List<Tag>) null, 293f);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Polypropylene);
    component.Temperature = 294.15f;
    Ladder ladder = placedEntity.AddOrGet<Ladder>();
    ladder.upwardsMovementSpeedMultiplier = 1.5f;
    ladder.downwardsMovementSpeedMultiplier = 1.5f;
    placedEntity.AddOrGet<AnimTileable>();
    Object.DestroyImmediate((Object) placedEntity.AddOrGet<OccupyArea>());
    OccupyArea occupyArea = placedEntity.AddOrGet<OccupyArea>();
    occupyArea.OccupiedCellsOffsets = EntityTemplates.GenerateOffsets(width, height);
    occupyArea.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
