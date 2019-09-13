// Decompiled with JetBrains decompiler
// Type: PropClockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PropClockConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropClock", (string) STRINGS.BUILDINGS.PREFABS.PROPCLOCK.NAME, (string) STRINGS.BUILDINGS.PREFABS.PROPCLOCK.DESC, 50f, Assets.GetAnim((HashedString) "clock_poi_kanim"), "off", Grid.SceneLayer.Building, 1, 2, TUNING.BUILDINGS.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, (List<Tag>) null, 293f);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
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
