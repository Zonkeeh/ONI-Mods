// Decompiled with JetBrains decompiler
// Type: VendingMachineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class VendingMachineConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("VendingMachine", (string) STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.NAME, (string) STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.DESC, 100f, Assets.GetAnim((HashedString) "vendingmachine_kanim"), "on", Grid.SceneLayer.Building, 2, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, (List<Tag>) null, 293f);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.machineSound = "VendingMachine_LP";
    setLocker.overrideAnim = "anim_break_kanim";
    setLocker.dropOffset = new Vector2I(1, 1);
    setLocker.possible_contents_ids = new string[1]
    {
      "FieldRation"
    };
    placedEntity.AddOrGet<LoreBearer>();
    placedEntity.AddOrGet<LoopingSounds>();
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
