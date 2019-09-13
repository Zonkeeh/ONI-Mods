// Decompiled with JetBrains decompiler
// Type: RockCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class RockCometConfig : IEntityConfig
{
  public static readonly string ID = "RockComet";
  private const SimHashes element = SimHashes.Regolith;
  private const int ADDED_CELLS = 6;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(RockCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    float mass = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues.mass;
    comet.massRange = new Vector2((float) ((double) mass * 0.800000011920929 * 6.0), (float) ((double) mass * 1.20000004768372 * 6.0));
    comet.temperatureRange = new Vector2(323.15f, 423.15f);
    comet.addTiles = 6;
    comet.addTilesMinHeight = 2;
    comet.addTilesMaxHeight = 8;
    comet.entityDamage = 20;
    comet.totalTileDamage = 0.0f;
    comet.splashRadius = 1;
    comet.impactSound = "Meteor_Large_Impact";
    comet.flyingSoundID = 2;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDirt;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Regolith);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_rock_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
