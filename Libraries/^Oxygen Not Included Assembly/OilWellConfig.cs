// Decompiled with JetBrains decompiler
// Type: OilWellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class OilWellConfig : IEntityConfig
{
  public const string ID = "OilWell";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("OilWell", (string) STRINGS.CREATURES.SPECIES.OIL_WELL.NAME, (string) STRINGS.CREATURES.SPECIES.OIL_WELL.DESC, 2000f, Assets.GetAnim((HashedString) "geyser_side_oil_kanim"), "off", Grid.SceneLayer.BuildingBack, 4, 2, TUNING.BUILDINGS.DECOR.BONUS.TIER1, TUNING.NOISE_POLLUTION.NOISY.TIER5, SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.SedimentaryRock);
    component.Temperature = 372.15f;
    placedEntity.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 0), GameTags.OilWell, (AttachableBuilding) null)
    };
    SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_shake_LP", TUNING.NOISE_POLLUTION.NOISY.TIER5);
    SoundEventVolumeCache.instance.AddVolume("geyser_side_methane_kanim", "GeyserMethane_erupt_LP", TUNING.NOISE_POLLUTION.NOISY.TIER6);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
