// Decompiled with JetBrains decompiler
// Type: PropReceptionDeskConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PropReceptionDeskConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropReceptionDesk", (string) STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.NAME, (string) STRINGS.BUILDINGS.PREFABS.PROPRECEPTIONDESK.DESC, 50f, Assets.GetAnim((HashedString) "gravitas_reception_kanim"), "off", Grid.SceneLayer.Building, 5, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, (List<Tag>) null, 293f);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Steel);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<LoreBearer>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    OccupyArea component = inst.GetComponent<OccupyArea>();
    component.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    int cell = Grid.PosToCell(inst);
    foreach (CellOffset occupiedCellsOffset in component.OccupiedCellsOffsets)
      Grid.GravitasFacility[Grid.OffsetCell(cell, occupiedCellsOffset)] = true;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
