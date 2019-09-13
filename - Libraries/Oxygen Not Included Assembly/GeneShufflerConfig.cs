// Decompiled with JetBrains decompiler
// Type: GeneShufflerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GeneShufflerConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GeneShuffler", (string) STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.NAME, (string) STRINGS.BUILDINGS.PREFABS.GENESHUFFLER.DESC, 2000f, Assets.GetAnim((HashedString) "geneshuffler_kanim"), "on", Grid.SceneLayer.Building, 4, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NOISY.TIER0, SimHashes.Creature, (List<Tag>) null, 293f);
    placedEntity.AddTag(GameTags.NotRoomAssignable);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    placedEntity.AddOrGet<Operational>();
    placedEntity.AddOrGet<Notifier>();
    placedEntity.AddOrGet<GeneShuffler>();
    placedEntity.AddOrGet<LoreBearer>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Ownable>();
    placedEntity.AddOrGet<Prioritizable>();
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.dropOnLoad = true;
    ManualDeliveryKG manualDeliveryKg = placedEntity.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg.requestedItemTag = new Tag("GeneShufflerRecharge");
    manualDeliveryKg.refillMass = 1f;
    manualDeliveryKg.minimumMass = 1f;
    manualDeliveryKg.capacity = 1f;
    KBatchedAnimController kbatchedAnimController = placedEntity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
    kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<GeneShuffler>().workLayer = Grid.SceneLayer.Building;
    inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    inst.GetComponent<Deconstructable>();
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
