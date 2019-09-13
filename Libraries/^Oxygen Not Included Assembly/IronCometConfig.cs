// Decompiled with JetBrains decompiler
// Type: IronCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class IronCometConfig : IEntityConfig
{
  public static string ID = "IronComet";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(IronCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME, true);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(3f, 20f);
    comet.temperatureRange = new Vector2(323.15f, 423.15f);
    comet.explosionOreCount = new Vector2I(2, 4);
    comet.entityDamage = 15;
    comet.totalTileDamage = 0.5f;
    comet.splashRadius = 1;
    comet.impactSound = "Meteor_Medium_Impact";
    comet.flyingSoundID = 1;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Iron);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_metal_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
