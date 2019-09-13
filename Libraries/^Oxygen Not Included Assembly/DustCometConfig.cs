// Decompiled with JetBrains decompiler
// Type: DustCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class DustCometConfig : IEntityConfig
{
  public static string ID = "DustComet";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(DustCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME, true);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(0.2f, 0.5f);
    comet.temperatureRange = new Vector2(223.15f, 253.15f);
    comet.entityDamage = 2;
    comet.totalTileDamage = 0.15f;
    comet.splashRadius = 0;
    comet.impactSound = "Meteor_Small_Impact";
    comet.flyingSoundID = 0;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Regolith);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_sand_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
